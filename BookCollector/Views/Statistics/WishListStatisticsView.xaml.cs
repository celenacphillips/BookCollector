using BookCollector.ViewModels.Statistics;

namespace BookCollector.Views.Statistics;

public partial class WishListStatisticsView : ContentPage
{
    private WishListStatisticsViewModel ViewModel;

    public WishListStatisticsView()
	{
        var viewModel = new WishListStatisticsViewModel(this);
        ViewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        using var _ = ViewModel.SetViewModelData();
    }
}