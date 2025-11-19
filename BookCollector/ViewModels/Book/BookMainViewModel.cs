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

            SelectedBook.SetDates();

            ChapterList = TestData.ChapterList;
            AuthorList = TestData.AuthorList;
            SelectedGenre = TestData.GenreList.FirstOrDefault(x => x.GenreGuid == SelectedBook.BookGenreGuid);

            BookIsRead = SelectedBook.BookPageRead == SelectedBook.BookPageTotal && SelectedBook.BookPageTotal != 0;
            ShowUpNext = SelectedBook.BookPageRead == 0;
            SelectedBook.SetBookCheckpoints();
            SelectedBook.SetPartOfSeries();
            SelectedBook.SetPartOfCollection();

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
