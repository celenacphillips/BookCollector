using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Views.Book;
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
        public bool bookInfo1Value;

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

        public BookEditViewModel(BookModel book, ContentPage view)
        {
            EditedBook = (BookModel)book.Clone();
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            BookInfo1Value = true;
            ReadingDataValue = true;
            ChapterListValue = true;
            AuthorListValue = true;
            BookInfoValue = true;
            SummaryValue = true;
            CommentsValue = true;

            BookIsRead = EditedBook.BookPageRead == EditedBook.BookPageTotal && EditedBook.BookPageTotal != 0;
            ShowUpNext = EditedBook.BookPageRead == 0;

            StepperEnabled = EditedBook.BookPageTotal != 0;
            ChapterList = TestData.ChapterList;
            AuthorList = TestData.AuthorList;
            SeriesList = TestData.SeriesList;
            CollectionList = TestData.CollectionList;
            GenreList = TestData.GenreList;

            SelectedCollection = CollectionList.FirstOrDefault(x => x.CollectionGuid == EditedBook.BookCollectionGuid);
            SelectedSeries = SeriesList.FirstOrDefault(x => x.SeriesGuid == EditedBook.BookSeriesGuid);
            SelectedGenre = GenreList.FirstOrDefault(x => x.GenreGuid == EditedBook.BookGenreGuid);

            Task.WaitAll(
            [
                Task.Run (async () => await EditedBook.SetBookCheckpoints() ),
                Task.Run (async () => await EditedBook.SetCoverDisplay() ),
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

        // TO DO:
        // Add Book Search screen - 11/19/2025
        [RelayCommand]
        public async Task BookSearch()
        {

        }

        [RelayCommand]
        public async Task SaveBook()
        {
            EditedBook.BookSeriesGuid = SelectedSeries?.SeriesGuid;
            EditedBook.BookCollectionGuid = SelectedCollection?.CollectionGuid;
            EditedBook.BookGenreGuid = SelectedGenre?.GenreGuid;

            Task.WaitAll(
            [
                Task.Run (async () => await EditedBook.SetDates() ),
                Task.Run (async () => await EditedBook.SetReadingProgress() ),
                Task.Run (async () => await EditedBook.SetPartOfSeries() ),
                Task.Run (async () => await EditedBook.SetPartOfCollection() ),
                Task.Run (async () => await EditedBook.SetCoverDisplay() ),
            ]);

#if ANDROID
            if (Platform.CurrentActivity != null &&
                Platform.CurrentActivity.Window != null)
                Platform.CurrentActivity.Window.DecorView.ClearFocus();
            #endif

            var parameters = new Dictionary<string, object>
            {
                { "SelectedObject", EditedBook }
            };
            await Shell.Current.GoToAsync("..", parameters);
        }

        [RelayCommand]
        public async Task BookInfo1Changed()
        {
            BookInfo1Open = BookInfo1Value;
            BookInfo1NotOpen = !BookInfo1Value;
        }

        // TO DO:
        // Set up Add Upload Cover photo - 11/19/2025
        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {

        }

        // TO DO:
        // Add remove cover photo - 11/19/2025

        // TO DO:
        // Set up Add Series screen - 11/19/2025
        [RelayCommand]
        public async Task AddSeries()
        {

        }

        // TO DO:
        // Set up Add Collection screen - 11/19/2025
        [RelayCommand]
        public async Task AddCollection()
        {

        }

        // TO DO:
        // Set up Add Genre screen - 11/19/2025
        [RelayCommand]
        public async Task AddGenre()
        {

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
        public async Task UpdateProgress()
        {
            StepperEnabled = EditedBook.BookPageTotal != 0;
            EditedBook.SetReadingProgress();
            EditedBook.SetBookCheckpoints();
        }

        [RelayCommand]
        public async Task PagesReadPopup()
        {

        }

        [RelayCommand]
        public async Task StepperValueChange(double value)
        {
            EditedBook.BookPageRead = (int)value;
            EditedBook.SetReadingProgress();
            BookIsRead = EditedBook.BookPageRead == EditedBook.BookPageTotal;
        }

        // TO DO:
        // Fix Add Chapter and Add Author to not add to main list without save - 11/19/2025
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
    }
}
