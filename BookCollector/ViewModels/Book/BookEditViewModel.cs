using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Series;
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
using System.Collections.ObjectModel;
using System.Net;

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

        public bool RemoveMainViewBefore { get; set; }

        public BookMainView? MainViewBefore { get; set; }

        private Popup? _pagesReadPopup;

        public double PopupWidth { get; set; }
        
        private bool HiddenCollectionsOn { get; set; }
        private bool HiddenGenresOn { get; set; }
        private bool HiddenSeriesOn { get; set; }
        private bool HiddenAuthorsOn { get; set; }
        private bool HiddenLocationsOn { get; set; }

        public BookEditViewModel(BookModel book, ContentPage view)
        {
            View = view;

            EditedBook = (BookModel)book.Clone();
            InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{EditedBook.BookTitle}")}";

            PopupWidth = DeviceWidth - 50;
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                ValidateEntry();

                BookInfo1SectionValue = true;
                ReadingDataSectionValue = true;
                ChapterListSectionValue = true;
                AuthorListSectionValue = true;
                BookInfoSectionValue = true;
                SummarySectionValue = true;
                CommentsSectionValue = true;

                BookIsRead = EditedBook.BookPageRead == EditedBook.BookPageTotal && EditedBook.BookPageTotal != 0;
                ShowUpNext = EditedBook.BookPageRead == 0;

                if (!string.IsNullOrEmpty(EditedBook.BookCoverFileLocation) && EditedBook.BookCover == null)
                {
                    var imageBytes = File.ReadAllBytes(EditedBook.BookCoverFileLocation);
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                }

                if (!string.IsNullOrEmpty(EditedBook.BookCoverUrl) && EditedBook.BookCover == null)
                {
                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        var byteArray = DownloadImage(EditedBook.BookCoverUrl);
                        EditedBook.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                    }
                }

                BookCover = EditedBook.BookCover;

                StepperEnabled = EditedBook.BookPageTotal != 0;

                AuthorList = !string.IsNullOrEmpty(EditedBook.AuthorListString) ? ParseOutAuthorsFromString(EditedBook.AuthorListString) : [];

                ChapterList = [];

                Task.WaitAll(
                [
                    Task.Run (async () => ChapterList = await FilterLists.GetAllChaptersInBook(EditedBook.BookGuid) ),
                    Task.Run (async () => SeriesList = await FilterLists.GetAllSeriesList(HiddenSeriesOn) ),
                    Task.Run (async () => CollectionList = await FilterLists.GetAllCollectionsList(HiddenCollectionsOn) ),
                    Task.Run (async () => GenreList = await FilterLists.GetAllGenresList(HiddenGenresOn) ),
                    Task.Run (async () => LocationList = await FilterLists.GetAllLocationsList(HiddenLocationsOn) ),
                ]);

                Task.WaitAll(
                [
                    Task.Run (async () => SelectedGenre = await FilterLists.GetGenreForBook(EditedBook.BookGenreGuid) ),
                    Task.Run (async () => SelectedLocation = await FilterLists.GetLocationForBook(EditedBook.BookLocationGuid) ),
                    Task.Run (async () => SelectedCollection = await FilterLists.GetCollectionForBook(EditedBook.BookCollectionGuid) ),
                    Task.Run (async () => SelectedSeries = await FilterLists.GetSeriesForBook(EditedBook.BookSeriesGuid) ),
                    Task.Run (async () => EditedBook.SetBookCheckpoints() ),
                    Task.Run (async () => EditedBook.SetCoverDisplay() ),
                    Task.Run (async () => await EditedBook.SetBookPrice() ),
                    Task.Run (async () => await BookInfo1Changed() ),
                    Task.Run (async () => await ReadingDataChanged() ),
                    Task.Run (async () => await ChapterListChanged() ),
                    Task.Run (async () => await AuthorListChanged() ),
                    Task.Run (async () => await BookInfoChanged() ),
                    Task.Run (async () => await SummaryChanged() ),
                    Task.Run (async () => await CommentsChanged() ),
                ]);

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        [RelayCommand]
        public async Task BookSearch()
        {
            SetIsBusyTrue();

            var view = new BookSearchView();
            var bindingContext = new BookSearchViewModel(null, view)
            {
                ViewTitle = $"{AppStringResources.BookSearch}",
                SelectedBook = EditedBook
            };
            view.BindingContext = bindingContext;

            await Shell.Current.Navigation.PushModalAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveBook()
        {
            if (!BookTitleValid && BookFormatNotValid)
            {
                if (!BookTitleValid)
                {
                    await DisplayMessage(AppStringResources.BookTitleNotValid, null);
                }

                if (BookFormatNotValid)
                {
                    await DisplayMessage(AppStringResources.BookFormatNotValid, null);
                }
            }
            else
            {
                EditedBook.BookSeriesGuid = SelectedSeries?.SeriesGuid;
                EditedBook.BookCollectionGuid = SelectedCollection?.CollectionGuid;
                EditedBook.BookGenreGuid = SelectedGenre?.GenreGuid;
                EditedBook.BookLocationGuid = SelectedLocation?.LocationGuid;

                if (AuthorList != null)
                {
                    foreach (var author in AuthorList)
                    {

                        if (TestData.UseTestData)
                        {
                            TestData.InsertAuthor(author, EditedBook.BookGuid);
                        }
                        else
                        {

                        }
                    }
                }

                Task.WaitAll(
                [
                    Task.Run (async () => await EditedBook.SetDates() ),
                    Task.Run (async () => EditedBook.SetReadingProgress() ),
                    Task.Run (async () => await EditedBook.SetPartOfSeries() ),
                    Task.Run (async () => await EditedBook.SetPartOfCollection() ),
                    Task.Run (async () => EditedBook.SetCoverDisplay() ),
                    Task.Run (async () => await EditedBook.SetBookPrice() ),
                    Task.Run (async () => await EditedBook.SetAuthorListString(AuthorList) ),
                    Task.Run (async () => await EditedBook.SetBookChapters(ChapterList) ),
                ]);

#if ANDROID
                if (Platform.CurrentActivity != null &&
                    Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (!string.IsNullOrEmpty(ViewTitle) && ViewTitle.Equals($"{AppStringResources.AddNewBook}"))
                {
                    if (TestData.UseTestData)
                    {
                        TestData.InsertBook(EditedBook);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (TestData.UseTestData)
                    {
                        TestData.UpdateBook(EditedBook);
                    }
                    else
                    {

                    }
                }

                if (RemoveMainViewBefore)
                {
                    Shell.Current.Navigation.RemovePage(MainViewBefore);
                }

                var view = new BookMainView(EditedBook, $"{EditedBook.BookTitle}");
                Shell.Current.Navigation.InsertPageBefore(view, View);

                await Shell.Current.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        public void BookInfo1Changed()
        {
            BookInfo1Open = BookInfo1SectionValue;
            BookInfo1NotOpen = !BookInfo1SectionValue;
        }

        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {
            var action = await PopupMenu_CoverPhoto();

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.UploadExistingFile))
            {
                SetIsBusyTrue();

                PermissionStatus storageReadStatus = await Permissions.RequestAsync<Permissions.Photos>();
                storageReadStatus = await Permissions.CheckStatusAsync<Permissions.Photos>();

                if (storageReadStatus == PermissionStatus.Granted)
                {
                    MediaPickerOptions pickerOptions = new();

                    try
                    {
                        var photos = await MediaPicker.PickPhotosAsync(pickerOptions);

                        if (photos?.Count > 0)
                        {
                            var firstPhoto = photos.First();
                            BookCover = ImageSource.FromFile(firstPhoto.FullPath);
                            EditedBook.HasBookCover = true;
                            EditedBook.HasNoBookCover = false;

                            var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                            if (!Directory.Exists(directory))
                                Directory.CreateDirectory(directory);

                            var fi = new FileInfo(firstPhoto.FullPath);
                            var filePath = $"{directory}/{fi.Name}";
                            File.Copy(firstPhoto.FullPath, filePath, true);

                            EditedBook.BookCoverFileLocation = filePath;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetIsBusyFalse();
                        await DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                    }

                    if (EditedBook.HasNoBookCover)
                        await DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                }
                else
                {
                    await DisplayMessage(AppStringResources.PleaseAllowPhotoPermissionToAddCover, null);
                }


                SetIsBusyFalse();
            }

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.BookCoverUrl))
            {
                var result = await View.ShowPopupAsync<string>(new BookCoverUrlPopup(PopupWidth, EditedBook.BookCoverUrl));
                var bookCoverUrl = result.Result;

                if (!string.IsNullOrEmpty(bookCoverUrl))
                {
                    SetIsBusyTrue();

                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        try
                        {
                            var byteArray = DownloadImage(bookCoverUrl);
                            BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                            EditedBook.BookCover = BookCover;
                            EditedBook.HasBookCover = true;
                            EditedBook.HasNoBookCover = false;
                            EditedBook.BookCoverUrl = bookCoverUrl;

                            SetIsBusyFalse();
                        }
                        catch (Exception ex)
                        {
                            SetIsBusyFalse();
                            BookCover = null;
                            EditedBook.BookCover = null;
                            EditedBook.HasBookCover = false;
                            EditedBook.HasNoBookCover = true;
                            EditedBook.BookCoverUrl = null;
                            await DisplayMessage(AppStringResources.AnErrorOccurred, AppStringResources.ErrorDownloadingImage);
                        }
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                        SetIsBusyFalse();
                    }

                }
            }
        }

        [RelayCommand]
        public void RemoveCoverPhoto()
        {
            EditedBook.HasBookCover = false;
            EditedBook.HasNoBookCover = true;
            EditedBook.BookCover = null;
            EditedBook.BookCoverUrl = null;
            EditedBook.BookCoverFileLocation = null;
        }

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

        [RelayCommand]
        public void RemoveSeries()
        {
            SelectedSeries = null;
        }

        [RelayCommand]
        public void RemoveCollection()
        {
            SelectedCollection = null;
        }

        [RelayCommand]
        public void RemoveGenre()
        {
            SelectedGenre = null;
        }

        [RelayCommand]
        public void RemoveLocation()
        {
            SelectedLocation = null;
        }

        [RelayCommand]
        public void UpdateProgress()
        {
            StepperEnabled = EditedBook.BookPageTotal != 0;
            EditedBook.SetReadingProgress();
            EditedBook.SetBookCheckpoints();
        }

        [RelayCommand]
        public async Task PagesReadPopup()
        {
            try
            {
                _pagesReadPopup = new PagesReadPopup(PopupWidth, EditedBook.BookPageRead, EditedBook.BookPageTotal);
                var result = await View.ShowPopupAsync<int>(_pagesReadPopup);
                EditedBook.BookPageRead = result.Result;
                UpdateProgress();
            }
            catch (Exception ex)
            {

            }
        }

        [RelayCommand]
        public void StepperValueChange(double value)
        {
            EditedBook.BookPageRead = (int)value;
            EditedBook.SetReadingProgress();
            BookIsRead = EditedBook.BookPageRead == EditedBook.BookPageTotal;
        }

        [RelayCommand]
        public void AddChapter()
        {
            ChapterList ??= [];

            ChapterList.Add(new ChapterModel());
        }

        [RelayCommand]
        public void RemoveChapter(ChapterModel chapter)
        {
            ChapterList?.Remove(chapter);
        }

        [RelayCommand]
        public void AddAuthor()
        {
            AuthorList ??= [];

            AuthorList.Add(new AuthorModel());
        }

        [RelayCommand]
        public void RemoveAuthor(AuthorModel author)
        {
            AuthorList?.Remove(author);
        }

        [RelayCommand]
        public void ReadToggle(bool value)
        {
            if (value && EditedBook.BookPageRead != EditedBook.BookPageTotal)
            {
                EditedBook.BookPageRead = EditedBook.BookPageTotal;
                EditedBook.SetReadingProgress();
            }
        }

        [RelayCommand]
        public void UpNextToggle(bool value)
        {
            EditedBook.UpNext = value;
        }

        [RelayCommand]
        public void ValidateBookFormat()
        {
            ValidateEntry();
        }

        [RelayCommand]
        public void ValidateBookTitle()
        {
            ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(EditedBook.BookTitle))
            {
                var bookTitleEditor = View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                bookTitleEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                BookTitleValid = false;
            }
            else
            {
                var bookTitleEditor = View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                bookTitleEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                BookTitleValid = true;
            }

            if (string.IsNullOrEmpty(EditedBook.BookFormat))
                BookFormatNotValid = true;
            else
                BookFormatNotValid = false;
        }
        private void GetPreferences()
        {
            HiddenCollectionsOn = Preferences.Get("HiddenCollectionsOn", true  /* Default */);
            HiddenGenresOn = Preferences.Get("HiddenGenresOn", true  /* Default */);
            HiddenSeriesOn = Preferences.Get("HiddenSeriesOn", true  /* Default */);
            HiddenLocationsOn = Preferences.Get("HiddenLocationsOn", true  /* Default */);
            HiddenAuthorsOn = Preferences.Get("HiddenAuthorsOn", true  /* Default */);
        }

    }
}
