using BookCollector.Data.Models;
using BookCollector.ViewModels.Series;

namespace BookCollector.Views.Series;

public partial class SeriesMainView : ContentPage
{
    private SeriesMainViewModel ViewModel;

    public SeriesMainView(SeriesModel series, string viewTitle)
	{
        var viewModel = new SeriesMainViewModel(series, this)
        {
            ViewTitle = viewTitle
        };
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