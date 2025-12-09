using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class AllBooksView : ContentPage
{
    private AllBooksViewModel ViewModel { get; set; }
    public AllBooksView()
	{
        var viewModel = new AllBooksViewModel(this);
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