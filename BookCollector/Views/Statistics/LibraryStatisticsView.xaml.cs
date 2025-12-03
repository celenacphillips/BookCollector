using BookCollector.ViewModels.Statistics;

namespace BookCollector.Views.Statistics;

public partial class LibraryStatisticsView : ContentPage
{
    private LibraryStatisticsViewModel _viewModel;

    // If data isn't showing, check here.
    public LibraryStatisticsView()
	{
        LibraryStatisticsViewModel viewModel = new LibraryStatisticsViewModel(this);
        _viewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        _viewModel.SetViewModelData();
    }
}