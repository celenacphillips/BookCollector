// <copyright file="BookEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Author;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Author;
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
using DocumentFormat.OpenXml.Bibliography;
using System.Collections.ObjectModel;
using Editor = Microsoft.Maui.Controls.Editor;

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

        [ObservableProperty]
        public ObservableCollection<AuthorModel>? authorList;

        [ObservableProperty]
        public ObservableCollection<AuthorPicker>? authorPickers;

        private Popup? pagesReadPopup;

        public BookEditViewModel(BookModel book, ContentPage view)
        {
            this.View = view;

            this.EditedBook = (BookModel)book.Clone();
            this.SelectedBook = book;
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

        [RelayCommand]
        public static async Task AddNewAuthor()
        {
            var view = new AuthorEditView(new AuthorModel(), $"{AppStringResources.AddNewAuthor}");
            await Shell.Current.Navigation.PushAsync(view);
        }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                var chapters = FillLists.GetAllChaptersInBook(this.EditedBook.BookGuid);
                var series = FillLists.GetAllSeriesList(this.HiddenSeriesOn);
                var collections = FillLists.GetAllCollectionsList(this.HiddenCollectionsOn);
                var genres = FillLists.GetAllGenresList(this.HiddenGenresOn);
                var locations = FillLists.GetAllLocationsList(this.HiddenLocationsOn);
                var authors = FillLists.GetAllAuthorsList(this.HiddenAuthorsOn);
                var bookAuthorList = FillLists.GetAllBookAuthorsForBook(this.EditedBook.BookGuid);

                this.ChaptersToDelete = [];
                this.AuthorsToDelete = [];

                this.BookInfo1SectionValue = true;
                this.ReadingDataSectionValue = true;
                this.ChapterListSectionValue = true;
                this.AuthorListSectionValue = true;
                this.BookInfoSectionValue = true;
                this.SummarySectionValue = true;
                this.CommentsSectionValue = true;

                this.BookIsRead = this.EditedBook.BookPageRead == this.EditedBook.BookPageTotal && this.EditedBook.BookPageTotal != 0;
                this.ShowUpNext = this.EditedBook.BookPageRead == 0;
                this.StepperEnabled = this.EditedBook.BookPageTotal != 0;

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

                        var directory = Path.Combine(FileSystem.AppDataDirectory, "BookCovers");

                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);

                            var filePath = Path.Combine(directory, $"{this.EditedBook.BookGuid}.jpg");
                            File.WriteAllBytes(filePath, byteArray);
                        }
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                    }
                }

                this.BookCover = this.EditedBook.BookCover;

                var loadDataTasks = new Task[]
                {
                    Task.Run(() => this.ValidateEntry()),
                    Task.Run(() => this.EditedBook.SetBookPrice()),
                    Task.Run(() => this.EditedBook.SetBookCheckpoints()),
                    Task.Run(() => this.EditedBook.SetCoverDisplay()),
                    Task.Run(() => BookModel.SetDate(this.EditedBook.BookStartDate)),
                    Task.Run(() => BookModel.SetDate(this.EditedBook.BookEndDate)),
                    Task.Run(() => BookModel.SetDate(this.EditedBook.BookLoanedOutOn)),
                    Task.Run(() => this.BookInfo1Changed()),
                    Task.Run(() => this.ReadingDataChanged()),
                    Task.Run(() => this.ChapterListChanged()),
                    Task.Run(() => this.AuthorListChanged()),
                    Task.Run(() => this.BookInfoChanged()),
                    Task.Run(() => this.SummaryChanged()),
                    Task.Run(() => this.CommentsChanged()),
                };

                await Task.WhenAll(chapters, series, collections, genres, locations, authors, bookAuthorList);

                this.ChapterList = chapters.Result;
                this.SeriesList = series.Result;
                this.CollectionList = collections.Result;
                this.GenreList = genres.Result;
                this.LocationList = locations.Result;
                this.AuthorList = authors.Result;
                var bookAuthors = bookAuthorList.Result;

                this.AuthorPickers = [];

                if (bookAuthors != null && bookAuthors.Count > 0)
                {
                    foreach (var bookAuthor in bookAuthors)
                    {
                        if (this.AuthorList != null)
                        {
                            var author = this.AuthorList.SingleOrDefault(x => x.AuthorGuid == bookAuthor.AuthorGuid);

                            this.AuthorPickers.Add(new AuthorPicker()
                            {
                                AuthorList = this.AuthorList,
                                SelectedAuthor = author,
                            });
                        }
                    }
                }

                if (this.EditedBook.SelectedAuthor != null)
                {
                    var author = this.AuthorList.SingleOrDefault(x => x.AuthorGuid == this.EditedBook.SelectedAuthor.AuthorGuid);

                    this.AuthorPickers.Add(new AuthorPicker()
                    {
                        AuthorList = this.AuthorList,
                        SelectedAuthor = author,
                    });
                }

                this.SelectedGenre = this.GenreList != null ? this.GenreList.FirstOrDefault(x => x.GenreGuid == this.EditedBook.BookGenreGuid) : null;
                this.SelectedLocation = this.LocationList != null ? this.LocationList.FirstOrDefault(x => x.LocationGuid == this.EditedBook.BookLocationGuid) : null;
                this.SelectedCollection = this.CollectionList != null ? this.CollectionList.FirstOrDefault(x => x.CollectionGuid == this.EditedBook.BookCollectionGuid) : null;
                this.SelectedSeries = this.SeriesList != null ? this.SeriesList.FirstOrDefault(x => x.SeriesGuid == this.EditedBook.BookSeriesGuid) : null;

                await Task.WhenAll(loadDataTasks);

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
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
            try
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
                    this.SetIsBusyTrue();

                    this.EditedBook.BookSeriesGuid = this.SelectedSeries?.SeriesGuid;
                    this.EditedBook.BookCollectionGuid = this.SelectedCollection?.CollectionGuid;
                    this.EditedBook.BookGenreGuid = this.SelectedGenre?.GenreGuid;
                    this.EditedBook.BookLocationGuid = this.SelectedLocation?.LocationGuid;

                    var dataTasks = new Task[]
                    {
                        Task.Run(() => this.EditedBook.SetReadingProgress()),
                        Task.Run(() => this.EditedBook.SetCoverDisplay()),
                        Task.Run(() => this.EditedBook.SetPartOfSeries()),
                        Task.Run(() => this.EditedBook.SetPartOfCollection()),
                        Task.Run(() => this.EditedBook.SetBookPrice()),
                        Task.Run(() => this.EditedBook.SetBookChapters(this.ChapterList)),
                        Task.Run(() => this.EditedBook.RemoveBookChapters(this.ChaptersToDelete)),
                    };

                    if (this.EditedBook.UpNext == true && this.EditedBook.BookPageRead > 0)
                    {
                        this.EditedBook.UpNext = false;
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
                                await Database.DeleteBookAuthorAsync((Guid)author.AuthorGuid, (Guid)this.EditedBook.BookGuid);
                            }
                        }
                    }

                    var authorList = new ObservableCollection<AuthorModel>();

                    if (this.AuthorPickers != null)
                    {
                        foreach (var authorPicker in this.AuthorPickers)
                        {
                            if (authorPicker.SelectedAuthor != null)
                            {
                                if (TestData.UseTestData)
                                {
                                }
                                else
                                {
                                    authorList.Add(authorPicker.SelectedAuthor);
                                }
                            }
                        }

                        await this.EditedBook.SetAuthorListString(authorList, false);
                    }

                    await Task.WhenAll(dataTasks);

#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    if (TestData.UseTestData)
                    {
                        TestData.UpdateBook(this.EditedBook);
                    }
                    else
                    {
                        this.EditedBook = ConvertTo<BookModel>(await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(this.EditedBook)));
                    }

                    foreach (var author in authorList)
                    {
                        if (TestData.UseTestData)
                        {
                            TestData.InsertAuthor(author, this.EditedBook.BookGuid);
                        }
                        else
                        {
                            var author1 = await Database.InsertAuthorAsync(ConvertTo<AuthorDatabaseModel>(author), this.EditedBook.BookGuid);
                            await Database.SaveAuthorAsync(ConvertTo<AuthorDatabaseModel>(author1));
                        }
                    }

                    if (this.RemoveMainViewBefore)
                    {
                        Shell.Current.Navigation.RemovePage(this.MainViewBefore);
                    }

                    var view = new BookMainView(this.EditedBook, $"{this.EditedBook.BookTitle}");
                    Shell.Current.Navigation.InsertPageBefore(view, this.View);

                    await Shell.Current.Navigation.PopAsync();

                    this.SetIsBusyFalse();
                }
            }
            catch (Exception ex)
            {
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
                    catch (Exception ex)
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
                        catch (Exception ex)
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
            catch (Exception ex)
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

            this.ChaptersToDelete ??= [];
            this.ChaptersToDelete?.Add(chapter);
        }

        [RelayCommand]
        public void AddAuthor()
        {
            this.AuthorPickers?.Add(new AuthorPicker
            {
                AuthorList = this.AuthorList,
                SelectedAuthor = null,
            });
        }

        [RelayCommand]
        public void RemoveAuthor(AuthorPicker authorPicker)
        {
            this.AuthorPickers?.Remove(authorPicker);

            if (authorPicker.SelectedAuthor != null)
            {
                this.AuthorsToDelete ??= [];
                this.AuthorsToDelete?.Add(authorPicker.SelectedAuthor);
            }
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
                var userAppTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme : Application.Current?.UserAppTheme;

                var bookTitleEditor = this.View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                bookTitleEditor.PlaceholderColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
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
