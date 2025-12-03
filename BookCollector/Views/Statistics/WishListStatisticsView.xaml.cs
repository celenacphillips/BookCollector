using BookCollector.ViewModels.Statistics;

namespace BookCollector.Views.Statistics;

public partial class WishListStatisticsView : ContentPage
{
    private WishListStatisticsViewModel _viewModel;

    // If data isn't showing, check here.
    public WishListStatisticsView()
	{
        WishListStatisticsViewModel viewModel = new WishListStatisticsViewModel(this);
        _viewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        _viewModel.SetViewModelData();
    }
}