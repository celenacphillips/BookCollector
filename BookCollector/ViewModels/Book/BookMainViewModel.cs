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
        BookMainView _view;

        public BookMainViewModel(BookModel book, ContentPage view)
        {
            _view = (BookMainView)view;

            SelectedBook = book;
        }

        public async Task SetViewModelData()
        {
            if (ViewTitle.Equals($"{AppStringResources.AddNewBook}"))
            {
                BookEditView view = new BookEditView(SelectedBook, $"{AppStringResources.EditBook}");
                await Shell.Current.Navigation.PushAsync(view);
            }

            SetIsBusyTrue();

            if (_view.ReceivedObject != null)
            {
                SelectedBook = _view.ReceivedObject;
                ViewTitle = SelectedBook.BookTitle;
                TestData.UpdateBook(SelectedBook);
            }

            ReadingDataValue = true;
            ChapterListValue = true;
            AuthorListValue = true;
            BookInfoValue = true;
            SummaryValue = true;
            CommentsValue = true;

            BookIsRead = SelectedBook.BookPageRead == SelectedBook.BookPageTotal && SelectedBook.BookPageTotal != 0;
            ShowUpNext = SelectedBook.BookPageRead == 0;

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
        public async Task EditBook()
        {
            SetIsBusyTrue();

            BookEditView view = new BookEditView(SelectedBook, $"{AppStringResources.EditBook}");

            //var stepper = (Stepper)view.FindByName("PageReadStepper");
            //stepper.Maximum = SelectedBook.BookPageTotal;
            //stepper.Value = SelectedBook.BookPageRead;

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        // TO DO:
        // Add checks for deleting book - 11/19/2025
        [RelayCommand]
        public async Task DeleteBook()
        {
            SetIsBusyTrue();

            TestData.DeleteBook(SelectedBook);
            await Shell.Current.Navigation.PopAsync();

            SetIsBusyFalse();
        }
    }
}
