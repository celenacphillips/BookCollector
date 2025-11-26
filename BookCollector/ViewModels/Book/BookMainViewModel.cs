using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views.Book;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Book
{
    public partial class BookMainViewModel : BookBaseViewModel
    {
        public BookMainViewModel(BookModel book, ContentPage view)
        {
            _view = view;

            SelectedBook = book;
            InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book",$"{SelectedBook.BookTitle}")}";
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            ReadingDataSectionValue = true;
            ChapterListSectionValue = true;
            AuthorListSectionValue = true;
            BookInfoSectionValue = true;
            SummarySectionValue = true;
            CommentsSectionValue = true;

            BookIsRead = SelectedBook.BookPageRead == SelectedBook.BookPageTotal && SelectedBook.BookPageTotal != 0;
            ShowUpNext = SelectedBook.BookPageRead == 0;

            if (SelectedBook.BookCoverBytes != null)
            {
                var imageSource = ImageSource.FromStream(() => new MemoryStream(SelectedBook.BookCoverBytes));
                BookCover = imageSource;
            }

            // Unit test data
            ChapterList = TestData.ChapterList;
            AuthorList = TestData.AuthorList;
            SelectedGenre = TestData.GenreList.FirstOrDefault(x => x.GenreGuid == SelectedBook.BookGenreGuid);

            Task.WaitAll(
            [
                Task.Run (async () => await SelectedBook.SetBookCheckpoints() ),
                Task.Run (async () => await SelectedBook.SetCoverDisplay() ),
                Task.Run (async () => await SelectedBook.SetPartOfSeries() ),
                Task.Run (async () => await SelectedBook.SetPartOfCollection() ),
                Task.Run (async () => await SelectedBook.SetDates() ),
                Task.Run (async () => await ReadingDataChanged() ),
                Task.Run (async () => await ChapterListChanged() ),
                Task.Run (async () => await AuthorListChanged() ),
                Task.Run (async () => await BookInfoChanged() ),
                Task.Run (async () => await SummaryChanged() ),
                Task.Run (async () => await CommentsChanged() ),
            ]);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }


        [RelayCommand]
        public async Task EditBook()
        {
            SetIsBusyTrue();

            BookEditView view = new BookEditView(SelectedBook, $"{AppStringResources.EditBook}");

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        // TO DO:
        // Add checks for deleting book - 11/19/2025
        [RelayCommand]
        public async Task DeleteBook()
        {
            SetIsBusyTrue();

            // Unit test data
            TestData.DeleteBook(SelectedBook);

            await Shell.Current.Navigation.PopAsync();

            SetIsBusyFalse();
        }
    }
}
