using BookCollector.ViewModels.Statistics;

namespace BookCollector.Views.Statistics;

public partial class LibraryStatisticsView : ContentPage
{
    private LibraryStatisticsViewModel ViewModel;

    public LibraryStatisticsView()
	{
        var viewModel = new LibraryStatisticsViewModel(this);
        ViewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        using var _ = ViewModel.SetViewModelData();
    }
}