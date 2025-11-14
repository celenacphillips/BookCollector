using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;

namespace BookCollector.Views.Book;

public partial class BookMainView : ContentPage
{
	public BookMainView(BookModel book, string viewTitle)
	{
        BookMainViewModel viewModel = new BookMainViewModel(book, this);
        viewModel.ViewTitle = viewTitle;
        BindingContext = viewModel;
        InitializeComponent();
	}
}