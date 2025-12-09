using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;

namespace BookCollector.Views.Book;

public partial class BookMainView : ContentPage
{
    private BookMainViewModel ViewModel;

    public BookMainView(BookModel book, string viewTitle)
	{
        var viewModel = new BookMainViewModel(book, this)
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