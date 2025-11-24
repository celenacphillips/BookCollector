using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;

namespace BookCollector.Views.Book;

[QueryProperty(nameof(ReceivedObject), "SelectedObject")]
public partial class BookEditView : ContentPage
{
    public BookModel? ReceivedObject { get; set; }
    private BookEditViewModel _viewModel { get; set; }

    public BookEditView(BookModel book, string viewTitle)
	{
        BookEditViewModel viewModel = new BookEditViewModel(book, this);
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