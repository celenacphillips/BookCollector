using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;
using BookCollector.ViewModels.WishListBook;

namespace BookCollector.Views.WishListBook;

public partial class WishListBookEditView : ContentPage
{
    private WishListBookEditViewModel _viewModel { get; set; }

    public WishListBookEditView(BookModel book, string viewTitle, bool removeMainViewBefore = false, WishListBookMainView? mainViewBefore = null)
	{
        WishListBookEditViewModel viewModel = new WishListBookEditViewModel(book, this);
        viewModel.ViewTitle = viewTitle;
        viewModel.RemoveMainViewBefore = removeMainViewBefore;
        viewModel.MainViewBefore = mainViewBefore;
        _viewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        _viewModel.SetViewModelData();
    }
}