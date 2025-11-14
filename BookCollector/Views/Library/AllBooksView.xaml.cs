using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class AllBooksView : ContentPage
{
	public AllBooksView()
	{
        AllBooksViewModel viewModel = new AllBooksViewModel(this);
        BindingContext = viewModel;

        InitializeComponent();
	}
}