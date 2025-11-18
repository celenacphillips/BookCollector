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
        public BookModel? editedBook;

        [ObservableProperty]
        public bool bookInfo1Value;

        [ObservableProperty]
        public bool bookInfo1Open;

        [ObservableProperty]
        public bool bookInfo1NotOpen;

        [ObservableProperty]
        public bool stepperEnabled;

        [ObservableProperty]
        ObservableCollection<string> seriesList;

        [ObservableProperty]
        string selectedSeries;

        [ObservableProperty]
        ObservableCollection<string> collectionList;

        [ObservableProperty]
        string selectedCollection;

        [ObservableProperty]
        ObservableCollection<string> genreList;

        [ObservableProperty]
        string selectedGenre;

        public BookEditViewModel(BookModel book, ContentPage view)
        {
            EditedBook = (BookModel)book.Clone();
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            BookIsRead = EditedBook.BookPageRead == EditedBook.BookPageTotal && EditedBook.BookPageTotal != 0;
            Half = EditedBook.BookPageTotal / 2;
            Fourth = EditedBook.BookPageTotal / 4;
            ThreeFourth = Half + Fourth;
            StepperEnabled = true;
            ChapterList = TestData.AddChaptersToList();
            AuthorList = TestData.AddAuthorsToList();

            BookInfo1Value = true;
            BookInfo1Changed();
            ReadingDataValue = true;
            ReadingDataChanged();
            ChapterListValue = true;
            ChapterListChanged();
            AuthorListValue = true;
            AuthorListChanged();
            BookInfoValue = true;
            BookInfoChanged();
            SummaryValue = true;
            SummaryChanged();
            CommentsValue = true;
            CommentsChanged();

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task BookSearch()
        {

        }

        [RelayCommand]
        public async Task SaveBook()
        {
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
        public void BookInfo1Changed()
        {
            BookInfo1Open = BookInfo1Value;
            BookInfo1NotOpen = !BookInfo1Value;
        }

        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {

        }

        [RelayCommand]
        public async Task AddSeries()
        {

        }

        [RelayCommand]
        public async Task AddCollection()
        {

        }

        [RelayCommand]
        public async Task AddGenre()
        {

        }

        [RelayCommand]
        public async Task RemoveSeries()
        {

        }

        [RelayCommand]
        public async Task RemoveCollection()
        {

        }

        [RelayCommand]
        public async Task RemoveGenre()
        {

        }

        [RelayCommand]
        public async Task UpdateProgress()
        {

        }

        [RelayCommand]
        public async Task PagesReadPopup()
        {

        }

        [RelayCommand]
        public async Task StepperValueChange()
        {

        }

        [RelayCommand]
        public async Task AddChapter()
        {

        }

        [RelayCommand]
        public async Task RemoveChapter()
        {

        }

        [RelayCommand]
        public async Task AddAuthor()
        {

        }

        [RelayCommand]
        public async Task RemoveAuthor()
        {

        }
    }
}
