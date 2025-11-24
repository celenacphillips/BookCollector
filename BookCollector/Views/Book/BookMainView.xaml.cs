using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;

namespace BookCollector.Views.Book;

[QueryProperty(nameof(ReceivedObject), "SelectedObject")]
public partial class BookMainView : ContentPage
{
    private BookMainViewModel _viewModel;
    public BookModel? ReceivedObject { get; set; }

    public BookMainView(BookModel book, string viewTitle)
	{
        BookMainViewModel viewModel = new BookMainViewModel(book, this);
        viewModel.ViewTitle = viewTitle;
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
	}

    // Need this to make sure new book info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        _viewModel.SetViewModelData();
    }
}