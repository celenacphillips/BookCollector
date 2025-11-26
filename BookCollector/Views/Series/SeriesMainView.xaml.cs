using BookCollector.Data.Models;
using BookCollector.ViewModels.Series;

namespace BookCollector.Views.Series;

public partial class SeriesMainView : ContentPage
{
    private SeriesMainViewModel _viewModel;

    public SeriesMainView(SeriesModel series, string viewTitle)
	{
        SeriesMainViewModel viewModel = new SeriesMainViewModel(series, this);
        viewModel.ViewTitle = viewTitle;
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