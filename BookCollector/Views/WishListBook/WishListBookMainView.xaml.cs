using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;
using BookCollector.ViewModels.WishListBook;

namespace BookCollector.Views.WishListBook;

public partial class WishListBookMainView : ContentPage
{
    private WishListBookMainViewModel viewModel;

    public WishListBookMainView(BookModel book, string viewTitle)
    {
        this.viewModel = new WishListBookMainViewModel(book, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.viewModel.SetViewModelData();
    }
}