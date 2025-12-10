using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;
using BookCollector.ViewModels.WishListBook;
using BookCollector.Views.WishListBook;

namespace BookCollector.Views.Book;

public partial class BookEditView : ContentPage
{
    public BookEditView(BookModel book, string viewTitle, bool removeMainViewBefore = false, BookMainView? mainViewBefore = null)
    {
        this.ViewModel = new BookEditViewModel(book, this)
        {
            ViewTitle = viewTitle,
            RemoveMainViewBefore = removeMainViewBefore,
            MainViewBefore = mainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private BookEditViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.ViewModel.SetViewModelData();
    }
}