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
            SetIsBusyTrue();

            if (_view.ReceivedObject != null)
            {
                SelectedBook = _view.ReceivedObject;
                ViewTitle = SelectedBook.BookTitle;
                TestData.UpdateBook(SelectedBook);
            }

            ChapterList = TestData.AddChaptersToList();
            AuthorList = TestData.AddAuthorsToList();

            BookIsRead = SelectedBook.BookPageRead == SelectedBook.BookPageTotal && SelectedBook.BookPageTotal != 0;
            Half = SelectedBook.BookPageTotal / 2;
            Fourth = SelectedBook.BookPageTotal / 4;
            ThreeFourth = Half + Fourth;

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

        [RelayCommand]
        public async Task DeleteBook()
        {

        }
    }
}
