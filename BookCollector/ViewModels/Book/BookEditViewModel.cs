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
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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
        public ObservableCollection<SeriesModel> seriesList;

        [ObservableProperty]
        public SeriesModel? selectedSeries;

        [ObservableProperty]
        public ObservableCollection<CollectionModel> collectionList;

        [ObservableProperty]
        public CollectionModel? selectedCollection;

        [ObservableProperty]
        public ObservableCollection<GenreModel> genreList;

        [ObservableProperty]
        public ObservableCollection<LocationModel> locationList;

        public bool RemoveMainViewBefore { get; set; }

        public BookMainView? MainViewBefore { get; set; }

        private Popup _pagesReadPopup;

        public double PopupWidth { get; set; }

        public BookEditViewModel(BookModel book, ContentPage view)
        {
            _view = view;

            EditedBook = (BookModel)book.Clone();
            InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{EditedBook.BookTitle}")}";

            PopupWidth = DeviceWidth - 50;
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

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

                if (EditedBook.BookCoverBytes != null)
                {
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(EditedBook.BookCoverBytes));
                    BookCover = imageSource;
                }

                StepperEnabled = EditedBook.BookPageTotal != 0;

                // Unit test data
                var chapterList = TestData.ChapterList;
                var genreList = TestData.GenreList;
                var seriesList = TestData.SeriesList;
                var collectionList = TestData.CollectionList;
                var locationList = TestData.LocationList;

                AuthorList = !string.IsNullOrEmpty(EditedBook.AuthorListString) ? ParseOutAuthorsFromString(EditedBook.AuthorListString) : new ObservableCollection<AuthorModel>();
                SeriesList = seriesList;
                CollectionList = collectionList;
                GenreList = genreList;
                LocationList = locationList;
                ChapterList = new ObservableCollection<ChapterModel>();

                Task.WaitAll(
                [
                    Task.Run (async () => ChapterList = await FilterLists.GetAllChaptersInBook(chapterList, EditedBook.BookGuid) ),
                    Task.Run (async () => SelectedGenre = await FilterLists.GetGenreForBook(genreList, EditedBook.BookGenreGuid) ),
                    Task.Run (async () => SelectedLocation = await FilterLists.GetLocationForBook(locationList, EditedBook.BookLocationGuid) ),
                    Task.Run (async () => SelectedCollection = await FilterLists.GetCollectionForBook(collectionList, EditedBook.BookCollectionGuid) ),
                    Task.Run (async () => SelectedSeries = await FilterLists.GetSeriesForBook(seriesList, EditedBook.BookSeriesGuid) ),
                    Task.Run (async () => await EditedBook.SetBookCheckpoints() ),
                    Task.Run (async () => await EditedBook.SetCoverDisplay() ),
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

            BookSearchView view = new BookSearchView();
            BookSearchViewModel bindingContext = new BookSearchViewModel(null, view);
            bindingContext.ViewTitle = $"{AppStringResources.BookSearch}";
            bindingContext.SelectedBook = EditedBook;
            view.BindingContext = bindingContext;

            await Shell.Current.Navigation.PushModalAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveBook()
        {
            if (BookTitleValid && !BookFormatNotValid)
            {
                EditedBook.BookSeriesGuid = SelectedSeries?.SeriesGuid;
                EditedBook.BookCollectionGuid = SelectedCollection?.CollectionGuid;
                EditedBook.BookGenreGuid = SelectedGenre?.GenreGuid;
                EditedBook.BookLocationGuid = SelectedLocation?.LocationGuid;

                foreach (var author in AuthorList)
                {
                    // Unit test data
                    if (TestData.AuthorList.Where(x => x.AuthorGuid == author.AuthorGuid).ToList().Count == 0)
                    {
                        TestData.InsertAuthor(author, EditedBook.BookGuid);
                    }
                    else
                    {
                        TestData.UpdateAuthor(author);
                    }
                }

                Task.WaitAll(
                [
                    Task.Run (async () => await EditedBook.SetDates() ),
                    Task.Run (async () => await EditedBook.SetReadingProgress() ),
                    Task.Run (async () => await EditedBook.SetPartOfSeries() ),
                    Task.Run (async () => await EditedBook.SetPartOfCollection() ),
                    Task.Run (async () => await EditedBook.SetCoverDisplay() ),
                    Task.Run (async () => await EditedBook.SetBookPrice() ),
                    Task.Run (async () => await EditedBook.SetAuthorListString(AuthorList) ),
                    Task.Run (async () => await EditedBook.SetBookChapters(ChapterList) ),
                ]);

#if ANDROID
                if (Platform.CurrentActivity != null &&
                    Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (ViewTitle.Equals($"{AppStringResources.AddNewBook}"))
                {
                    // Unit test data
                    TestData.InsertBook(EditedBook);
                }
                else
                {
                    // Unit test data
                    TestData.UpdateBook(EditedBook);
                }

                if (RemoveMainViewBefore)
                {
                    Shell.Current.Navigation.RemovePage(MainViewBefore);
                }

                BookMainView view = new BookMainView(EditedBook, $"{EditedBook.BookTitle}");
                Shell.Current.Navigation.InsertPageBefore(view, _view);

                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                if (!BookTitleValid)
                    await Shell.Current.DisplayAlert("Book Title not valid", "Book Title not valid", $"{AppStringResources.OK}");

                if (BookFormatNotValid)
                    await Shell.Current.DisplayAlert("Book Format not valid", "Book Format not valid", $"{AppStringResources.OK}");
            }
        }

        [RelayCommand]
        public async Task BookInfo1Changed()
        {
            BookInfo1Open = BookInfo1SectionValue;
            BookInfo1NotOpen = !BookInfo1SectionValue;
        }

        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {
            SetIsBusyTrue();

            PermissionStatus storageReadStatus = await Permissions.RequestAsync<Permissions.Photos>();
            storageReadStatus = await Permissions.CheckStatusAsync<Permissions.Photos>();

            if (storageReadStatus == PermissionStatus.Granted)
            {
                MediaPickerOptions pickerOptions = new();

                try
                {
                    var result = await MediaPicker.PickPhotoAsync(pickerOptions);

                    if (result != null)
                    {
                        BookCover = ImageSource.FromFile(result.FullPath);
                        EditedBook.HasBookCover = true;
                        EditedBook.HasNoBookCover = false;
                        EditedBook.BookCoverBytes = await File.ReadAllBytesAsync(result.FullPath);
                    }
                }
                catch (Exception ex)
                {
                    SetIsBusyFalse();
                    await Shell.Current.DisplayAlert(null, AppStringResources.PickingCoverCanceled, AppStringResources.OK);
                }

                if (EditedBook.HasNoBookCover)
                    await Shell.Current.DisplayAlert(null, AppStringResources.PickingCoverCanceled, AppStringResources.OK);

                
            }
            else
            {
                await Shell.Current.DisplayAlert(null, AppStringResources.PleaseAllowPhotoPermissionToAddCover, AppStringResources.OK);
            }


            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task RemoveCoverPhoto()
        {
            EditedBook.HasBookCover = false;
            EditedBook.HasNoBookCover = true;
            EditedBook.BookCoverBytes = null;
        }

        [RelayCommand]
        public async Task AddSeries()
        {
            SeriesEditView view = new SeriesEditView(new SeriesModel(), $"{AppStringResources.AddNewSeries}");
            await Shell.Current.Navigation.PushAsync(view);
        }

        [RelayCommand]
        public async Task AddCollection()
        {
            CollectionEditView view = new CollectionEditView(new CollectionModel(), $"{AppStringResources.AddNewCollection}");
            await Shell.Current.Navigation.PushAsync(view);
        }

        [RelayCommand]
        public async Task AddGenre()
        {
            GenreEditView view = new GenreEditView(new GenreModel(), $"{AppStringResources.AddNewGenre}");
            await Shell.Current.Navigation.PushAsync(view);
        }

        [RelayCommand]
        public async Task AddLocation()
        {
            LocationEditView view = new LocationEditView(new LocationModel(), $"{AppStringResources.AddNewLocation}");
            await Shell.Current.Navigation.PushAsync(view);
        }

        [RelayCommand]
        public async Task RemoveSeries()
        {
            SelectedSeries = null;
        }

        [RelayCommand]
        public async Task RemoveCollection()
        {
            SelectedCollection = null;
        }

        [RelayCommand]
        public async Task RemoveGenre()
        {
            SelectedGenre = null;
        }

        [RelayCommand]
        public async Task RemoveLocation()
        {
            SelectedLocation = null;
        }

        [RelayCommand]
        public async Task UpdateProgress()
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
                var result = await _view.ShowPopupAsync(_pagesReadPopup);
                EditedBook.BookPageRead = (int)result;
            }
            catch (Exception ex)
            {

            }
        }

        [RelayCommand]
        public async Task StepperValueChange(double value)
        {
            EditedBook.BookPageRead = (int)value;
            EditedBook.SetReadingProgress();
            BookIsRead = EditedBook.BookPageRead == EditedBook.BookPageTotal;
        }

        [RelayCommand]
        public async Task AddChapter()
        {
            ChapterList.Add(new ChapterModel());
        }

        [RelayCommand]
        public async Task RemoveChapter(ChapterModel chapter)
        {
            ChapterList.Remove(chapter);
        }

        [RelayCommand]
        public async Task AddAuthor()
        {
            AuthorList.Add(new AuthorModel());
        }

        [RelayCommand]
        public async Task RemoveAuthor(AuthorModel author)
        {
            AuthorList.Remove(author);
        }

        [RelayCommand]
        public async Task ReadToggle(bool value)
        {
            if (value && EditedBook.BookPageRead != EditedBook.BookPageTotal)
            {
                EditedBook.BookPageRead = EditedBook.BookPageTotal;
                EditedBook.SetReadingProgress();
            }
        }

        [RelayCommand]
        public async Task UpNextToggle(bool value)
        {
            EditedBook.UpNext = value;
        }


        [RelayCommand]
        public async Task ValidateBookFormat()
        {
            ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(EditedBook.BookTitle))
                BookTitleValid = false;
            else
                BookTitleValid = true;

            if (string.IsNullOrEmpty(EditedBook.BookFormat))
                BookFormatNotValid = true;
            else
                BookFormatNotValid = false;
        }
    }
}
