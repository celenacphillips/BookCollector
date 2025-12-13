using System.Collections.ObjectModel;
using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Book;
using BookCollector.Views.Collection;
using BookCollector.Views.Genre;
using BookCollector.Views.Location;
using BookCollector.Views.Popups;
using BookCollector.Views.Series;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Book
{
    public partial class BookEditViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public BookModel editedBook;

        [ObservableProperty]
        public bool bookTitleValid;

        [ObservableProperty]
        public bool bookFormatNotValid;

        [ObservableProperty]
        public bool bookInfo1SectionValue;

        [ObservableProperty]
        public bool bookInfo1Open;

        [ObservableProperty]
        public bool bookInfo1NotOpen;

        [ObservableProperty]
        public bool stepperEnabled;

        [ObservableProperty]
        public ObservableCollection<SeriesModel>? seriesList;

        [ObservableProperty]
        public SeriesModel? selectedSeries;

        [ObservableProperty]
        public ObservableCollection<CollectionModel>? collectionList;

        [ObservableProperty]
        public CollectionModel? selectedCollection;

        [ObservableProperty]
        public ObservableCollection<GenreModel>? genreList;

        [ObservableProperty]
        public ObservableCollection<LocationModel>? locationList;

        private Popup? pagesReadPopup;

        public BookEditViewModel(BookModel book, ContentPage view)
        {
            this.View = view;

            this.EditedBook = (BookModel)book.Clone();
            this.InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{this.EditedBook.BookTitle}")}";

            this.PopupWidth = this.DeviceWidth - 50;
        }

        public bool RemoveMainViewBefore { get; set; }

        public BookMainView? MainViewBefore { get; set; }

        public double PopupWidth { get; set; }

        private bool HiddenCollectionsOn { get; set; }

        private bool HiddenGenresOn { get; set; }

        private bool HiddenSeriesOn { get; set; }

        private bool HiddenLocationsOn { get; set; }

        private List<ChapterModel>? ChaptersToDelete { get; set; }

        private List<AuthorModel>? AuthorsToDelete { get; set; }

        [RelayCommand]
        public static async Task AddSeries()
        {
            var view = new SeriesEditView(new SeriesModel(), $"{AppStringResources.AddNewSeries}");
            await Shell.Current.Navigation.PushAsync(view);
        }

        [RelayCommand]
        public static async Task AddCollection()
        {
            var view = new CollectionEditView(new CollectionModel(), $"{AppStringResources.AddNewCollection}");
            await Shell.Current.Navigation.PushAsync(view);
        }

        [RelayCommand]
        public static async Task AddGenre()
        {
            var view = new GenreEditView(new GenreModel(), $"{AppStringResources.AddNewGenre}");
            await Shell.Current.Navigation.PushAsync(view);
        }

        [RelayCommand]
        public static async Task AddLocation()
        {
            var view = new LocationEditView(new LocationModel(), $"{AppStringResources.AddNewLocation}");
            await Shell.Current.Navigation.PushAsync(view);
        }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                this.ValidateEntry();

                this.BookInfo1SectionValue = true;
                this.ReadingDataSectionValue = true;
                this.ChapterListSectionValue = true;
                this.AuthorListSectionValue = true;
                this.BookInfoSectionValue = true;
                this.SummarySectionValue = true;
                this.CommentsSectionValue = true;

                this.BookIsRead = this.EditedBook.BookPageRead == this.EditedBook.BookPageTotal && this.EditedBook.BookPageTotal != 0;
                this.ShowUpNext = this.EditedBook.BookPageRead == 0;

                if (!string.IsNullOrEmpty(this.EditedBook.BookCoverFileLocation) && this.EditedBook.BookCover == null)
                {
                    var imageBytes = File.ReadAllBytes(this.EditedBook.BookCoverFileLocation);
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    this.EditedBook.BookCover = imageSource;
                }

                if (!string.IsNullOrEmpty(this.EditedBook.BookCoverUrl) && this.EditedBook.BookCover == null)
                {
                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        var byteArray = DownloadImage(this.EditedBook.BookCoverUrl);
                        this.EditedBook.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                    }
                }

                this.BookCover = this.EditedBook.BookCover;

                this.StepperEnabled = this.EditedBook.BookPageTotal != 0;

                this.AuthorList = !string.IsNullOrEmpty(this.EditedBook.AuthorListstring) ? ParseOutAuthorsFromstring(this.EditedBook.AuthorListstring, this.HiddenAuthorsOn) : [];

                this.ChapterList = [];

                Task.WaitAll(
                [
                    Task.Run(async () => this.ChapterList = await FilterLists.GetAllChaptersInBook(this.EditedBook.BookGuid)),
                    Task.Run(async () => this.SeriesList = await FilterLists.GetAllSeriesList(this.HiddenSeriesOn)),
                    Task.Run(async () => this.CollectionList = await FilterLists.GetAllCollectionsList(this.HiddenCollectionsOn)),
                    Task.Run(async () => this.GenreList = await FilterLists.GetAllGenresList(this.HiddenGenresOn)),
                    Task.Run(async () => this.LocationList = await FilterLists.GetAllLocationsList(this.HiddenLocationsOn)),
                ]);

                Task.WaitAll(
                [
                    Task.Run(async () => this.SelectedGenre = await FilterLists.GetGenreForBook(this.EditedBook.BookGenreGuid)),
                    Task.Run(async () => this.SelectedLocation = await FilterLists.GetLocationForBook(this.EditedBook.BookLocationGuid)),
                    Task.Run(async () => this.SelectedCollection = await FilterLists.GetCollectionForBook(this.EditedBook.BookCollectionGuid)),
                    Task.Run(async () => this.SelectedSeries = await FilterLists.GetSeriesForBook(this.EditedBook.BookSeriesGuid)),
                    Task.Run(async () => this.EditedBook.SetBookCheckpoints()),
                    Task.Run(async () => this.EditedBook.SetCoverDisplay()),
                    Task.Run(async () => await this.EditedBook.SetBookPrice()),
                    Task.Run(() => this.BookInfo1Changed()),
                    Task.Run(() => this.ReadingDataChanged()),
                    Task.Run(() => this.ChapterListChanged()),
                    Task.Run(() => this.AuthorListChanged()),
                    Task.Run(() => this.BookInfoChanged()),
                    Task.Run(() => this.SummaryChanged()),
                    Task.Run(() => this.CommentsChanged()),
                ]);

                this.ChaptersToDelete = [];
                this.AuthorsToDelete = [];

                this.SetIsBusyFalse();
            }
            catch (Exception)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task BookSearch()
        {
            this.SetIsBusyTrue();

            var view = new BookSearchView();
            var bindingContext = new BookSearchViewModel(null, view)
            {
                ViewTitle = $"{AppStringResources.BookSearch}",
                SelectedBook = this.EditedBook,
            };
            view.BindingContext = bindingContext;

            await Shell.Current.Navigation.PushModalAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveBook()
        {
            if (!this.BookTitleValid || this.BookFormatNotValid)
            {
                if (!this.BookTitleValid)
                {
                    await DisplayMessage(AppStringResources.BookTitleNotValid, null);
                }

                if (this.BookFormatNotValid)
                {
                    await DisplayMessage(AppStringResources.BookFormatNotValid, null);
                }
            }
            else
            {
                this.EditedBook.BookSeriesGuid = this.SelectedSeries?.SeriesGuid;
                this.EditedBook.BookCollectionGuid = this.SelectedCollection?.CollectionGuid;
                this.EditedBook.BookGenreGuid = this.SelectedGenre?.GenreGuid;
                this.EditedBook.BookLocationGuid = this.SelectedLocation?.LocationGuid;

                if (this.EditedBook.UpNext == true && this.EditedBook.BookPageRead > 0)
                {
                    this.EditedBook.UpNext = false;
                }

                if (this.AuthorList != null)
                {
                    foreach (var author in this.AuthorList)
                    {
                        if (TestData.UseTestData)
                        {
                            TestData.InsertAuthor(author, this.EditedBook.BookGuid);
                        }
                        else
                        {
                        }
                    }
                }

                if (this.AuthorsToDelete != null)
                {
                    foreach (var author in this.AuthorsToDelete)
                    {
                        if (TestData.UseTestData)
                        {
                            TestData.DeleteBookAuthor((Guid)author.AuthorGuid, (Guid)this.EditedBook.BookGuid);
                        }
                        else
                        {
                        }
                    }
                }

                Task.WaitAll(
                [
                    Task.Run(async () => this.EditedBook.SetReadingProgress()),
                    Task.Run(async () => await this.EditedBook.SetPartOfSeries()),
                    Task.Run(async () => await this.EditedBook.SetPartOfCollection()),
                    Task.Run(async () => this.EditedBook.SetCoverDisplay()),
                    Task.Run(async () => await this.EditedBook.SetBookPrice()),
                    Task.Run(async () => await this.EditedBook.SetAuthorListstring(this.AuthorList)),
                    Task.Run(async () => await this.EditedBook.SetBookChapters(this.ChapterList)),
                    Task.Run(async () => await this.EditedBook.RemoveBookChapters(this.ChaptersToDelete)),
                ]);

#if ANDROID
                if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                {
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
                }
#endif

                if (!string.IsNullOrEmpty(this.ViewTitle) && this.ViewTitle.Equals($"{AppStringResources.AddNewBook}"))
                {
                    if (TestData.UseTestData)
                    {
                        TestData.InsertBook(this.EditedBook);
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (TestData.UseTestData)
                    {
                        TestData.UpdateBook(this.EditedBook);
                    }
                    else
                    {
                    }
                }

                if (this.RemoveMainViewBefore)
                {
                    Shell.Current.Navigation.RemovePage(this.MainViewBefore);
                }

                var view = new BookMainView(this.EditedBook, $"{this.EditedBook.BookTitle}");
                Shell.Current.Navigation.InsertPageBefore(view, this.View);

                await Shell.Current.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        public void BookInfo1Changed()
        {
            this.BookInfo1Open = this.BookInfo1SectionValue;
            this.BookInfo1NotOpen = !this.BookInfo1SectionValue;
        }

        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {
            var action = await PopupMenu_CoverPhoto();

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.UploadExistingFile))
            {
                this.SetIsBusyTrue();

                PermissionStatus storageReadStatus = await Permissions.RequestAsync<Permissions.Photos>();
                storageReadStatus = await Permissions.CheckStatusAsync<Permissions.Photos>();

                if (storageReadStatus == PermissionStatus.Granted)
                {
                    MediaPickerOptions pickerOptions = new ();

                    try
                    {
                        var photos = await MediaPicker.PickPhotosAsync(pickerOptions);

                        if (photos?.Count > 0)
                        {
                            var firstPhoto = photos.First();
                            this.BookCover = ImageSource.FromFile(firstPhoto.FullPath);
                            this.EditedBook.HasBookCover = true;
                            this.EditedBook.HasNoBookCover = false;

                            var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            var fi = new FileInfo(firstPhoto.FullPath);
                            var filePath = $"{directory}/{fi.Name}";
                            File.Copy(firstPhoto.FullPath, filePath, true);

                            this.EditedBook.BookCoverFileLocation = filePath;
                        }
                    }
                    catch (Exception)
                    {
                        this.SetIsBusyFalse();
                        await DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                    }

                    if (this.EditedBook.HasNoBookCover)
                    {
                        await DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                    }
                }
                else
                {
                    await DisplayMessage(AppStringResources.PleaseAllowPhotoPermissionToAddCover, null);
                }

                this.SetIsBusyFalse();
            }

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.BookCoverUrl))
            {
                var result = await this.View.ShowPopupAsync<string>(new BookCoverUrlPopup(this.PopupWidth, this.EditedBook.BookCoverUrl));
                var bookCoverUrl = result.Result;

                if (!string.IsNullOrEmpty(bookCoverUrl))
                {
                    this.SetIsBusyTrue();

                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        try
                        {
                            var byteArray = DownloadImage(bookCoverUrl);
                            this.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                            this.EditedBook.BookCover = this.BookCover;
                            this.EditedBook.HasBookCover = true;
                            this.EditedBook.HasNoBookCover = false;
                            this.EditedBook.BookCoverUrl = bookCoverUrl;

                            this.SetIsBusyFalse();
                        }
                        catch (Exception)
                        {
                            this.SetIsBusyFalse();
                            this.BookCover = null;
                            this.EditedBook.BookCover = null;
                            this.EditedBook.HasBookCover = false;
                            this.EditedBook.HasNoBookCover = true;
                            this.EditedBook.BookCoverUrl = null;
                            await DisplayMessage(AppStringResources.AnErrorOccurred, AppStringResources.ErrorDownloadingImage);
                        }
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                        this.SetIsBusyFalse();
                    }
                }
            }
        }

        [RelayCommand]
        public void RemoveCoverPhoto()
        {
            this.EditedBook.HasBookCover = false;
            this.EditedBook.HasNoBookCover = true;
            this.EditedBook.BookCover = null;
            this.EditedBook.BookCoverUrl = null;
            this.EditedBook.BookCoverFileLocation = null;
        }

        [RelayCommand]
        public void RemoveSeries()
        {
            this.SelectedSeries = null;
        }

        [RelayCommand]
        public void RemoveCollection()
        {
            this.SelectedCollection = null;
        }

        [RelayCommand]
        public void RemoveGenre()
        {
            this.SelectedGenre = null;
        }

        [RelayCommand]
        public void RemoveLocation()
        {
            this.SelectedLocation = null;
        }

        [RelayCommand]
        public void UpdateProgress()
        {
            this.StepperEnabled = this.EditedBook.BookPageTotal != 0;
            this.EditedBook.SetReadingProgress();
            this.EditedBook.SetBookCheckpoints();
        }

        [RelayCommand]
        public async Task PagesReadPopup()
        {
            try
            {
                this.pagesReadPopup = new PagesReadPopup(this.PopupWidth, this.EditedBook.BookPageRead, this.EditedBook.BookPageTotal);
                var result = await this.View.ShowPopupAsync<int>(this.pagesReadPopup);
                this.EditedBook.BookPageRead = result.Result;
                this.UpdateProgress();
            }
            catch (Exception)
            {
            }
        }

        [RelayCommand]
        public void StepperValueChange(double value)
        {
            this.EditedBook.BookPageRead = (int)value;
            this.EditedBook.SetReadingProgress();
            this.BookIsRead = this.EditedBook.BookPageRead == this.EditedBook.BookPageTotal;
        }

        [RelayCommand]
        public void AddChapter()
        {
            this.ChapterList ??= [];

            this.ChapterList.Add(new ChapterModel());
        }

        [RelayCommand]
        public void RemoveChapter(ChapterModel chapter)
        {
            this.ChapterList?.Remove(chapter);
            this.ChaptersToDelete.Add(chapter);
        }

        [RelayCommand]
        public void AddAuthor()
        {
            this.AuthorList ??= [];

            this.AuthorList.Add(new AuthorModel());
        }

        [RelayCommand]
        public void RemoveAuthor(AuthorModel author)
        {
            this.AuthorList?.Remove(author);
            this.AuthorsToDelete.Add(author);
        }

        [RelayCommand]
        public void ReadToggle(bool value)
        {
            if (value && this.EditedBook.BookPageRead != this.EditedBook.BookPageTotal)
            {
                this.EditedBook.BookPageRead = this.EditedBook.BookPageTotal;
                this.EditedBook.SetReadingProgress();
            }
        }

        [RelayCommand]
        public void UpNextToggle(bool value)
        {
            this.EditedBook.UpNext = value;
        }

        [RelayCommand]
        public void ValidateBookFormat()
        {
            this.ValidateEntry();
        }

        [RelayCommand]
        public void ValidateBookTitle()
        {
            this.ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(this.EditedBook.BookTitle))
            {
                var bookTitleEditor = this.View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                bookTitleEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                this.BookTitleValid = false;
            }
            else
            {
                var bookTitleEditor = this.View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                bookTitleEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.BookTitleValid = true;
            }

            if (string.IsNullOrEmpty(this.EditedBook.BookFormat))
            {
                this.BookFormatNotValid = true;
            }
            else
            {
                this.BookFormatNotValid = false;
            }
        }

        private void GetPreferences()
        {
            this.HiddenCollectionsOn = Preferences.Get("HiddenCollectionsOn", true /* Default */);
            this.HiddenGenresOn = Preferences.Get("HiddenGenresOn", true /* Default */);
            this.HiddenSeriesOn = Preferences.Get("HiddenSeriesOn", true /* Default */);
            this.HiddenLocationsOn = Preferences.Get("HiddenLocationsOn", true /* Default */);
            this.HiddenAuthorsOn = Preferences.Get("HiddenAuthorsOn", true /* Default */);
        }
    }
}
