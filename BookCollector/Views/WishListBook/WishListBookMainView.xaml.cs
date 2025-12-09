using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;
using BookCollector.ViewModels.WishListBook;

namespace BookCollector.Views.WishListBook;

public partial class WishListBookMainView : ContentPage
{
    private WishListBookMainViewModel ViewModel;

    public WishListBookMainView(BookModel book, string viewTitle)
	{
        var viewModel = new WishListBookMainViewModel(book, this)
        {
            ViewTitle = viewTitle
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