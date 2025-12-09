using BookCollector.ViewModels.Main;

namespace BookCollector.Views.Main;

public partial class WishListView : ContentPage
{
	private WishListViewModel ViewModel { get; set; }

    public WishListView()
	{
		var viewModel = new WishListViewModel(this);
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