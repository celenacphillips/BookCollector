using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;
using BookCollector.ViewModels.WishListBook;
using BookCollector.Views.WishListBook;

namespace BookCollector.Views.Book;

public partial class BookEditView : ContentPage
{
    private BookEditViewModel ViewModel { get; set; }

    public BookEditView(BookModel book, string viewTitle, bool removeMainViewBefore = false, BookMainView? mainViewBefore = null)
	{
        var viewModel = new BookEditViewModel(book, this)
        {
            ViewTitle = viewTitle,
            RemoveMainViewBefore = removeMainViewBefore,
            MainViewBefore = mainViewBefore
        };
        ViewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
    }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var _ = ViewModel.SetViewModelData();
    }
}