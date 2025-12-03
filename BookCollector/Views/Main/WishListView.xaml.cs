using BookCollector.ViewModels.Main;

namespace BookCollector.Views.Main;

public partial class WishListView : ContentPage
{
	private WishListViewModel _viewModel { get; set; }

	public WishListView()
	{
		WishListViewModel viewModel = new WishListViewModel(this);
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