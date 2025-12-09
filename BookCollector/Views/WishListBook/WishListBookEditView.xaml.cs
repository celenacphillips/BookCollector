using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;
using BookCollector.ViewModels.WishListBook;

namespace BookCollector.Views.WishListBook;

public partial class WishListBookEditView : ContentPage
{
    private WishListBookEditViewModel ViewModel { get; set; }

    public WishListBookEditView(BookModel book, string viewTitle, bool removeMainViewBefore = false, WishListBookMainView? mainViewBefore = null)
	{
        var viewModel = new WishListBookEditViewModel(book, this)
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