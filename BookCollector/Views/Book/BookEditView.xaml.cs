using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;

namespace BookCollector.Views.Book;

public partial class BookEditView : ContentPage
{
    private BookEditViewModel _viewModel { get; set; }

    public BookEditView(BookModel book, string viewTitle)
	{
        BookEditViewModel viewModel = new BookEditViewModel(book, this);
        viewModel.ViewTitle = viewTitle;
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