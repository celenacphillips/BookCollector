using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views.Book;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

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
            var chapterList = TestData.ChapterList;
            var authorList = TestData.AuthorList;
            var genreList = TestData.GenreList;
            var locationList = TestData.LocationList;
            var bookAuthorList = TestData.BookAuthorList;

            Task.WaitAll(
            [
                Task.Run (async () => bookAuthorList  = await FilterLists.GetAllBookAuthorsForBook(bookAuthorList, SelectedBook.BookGuid) ),
                Task.Run (async () => AuthorList = await FilterLists.GetAllAuthorsForBook(bookAuthorList, authorList, SelectedBook.BookGuid) ),
                Task.Run (async () => ChapterList = await FilterLists.GetAllChaptersInBook(chapterList, SelectedBook.BookGuid) ),
                Task.Run (async () => SelectedGenre = await FilterLists.GetGenreForBook(genreList, SelectedBook.BookGenreGuid) ),
                Task.Run (async () => SelectedLocation = await FilterLists.GetLocationForBook(locationList, SelectedBook.BookLocationGuid) ),
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

            BookEditView view = new BookEditView(SelectedBook, $"{AppStringResources.EditBook}", true, (BookMainView)_view);

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
