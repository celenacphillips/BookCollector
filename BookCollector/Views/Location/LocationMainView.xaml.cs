using BookCollector.Data.Models;
using BookCollector.ViewModels.Location;

namespace BookCollector.Views.Location;

public partial class LocationMainView : ContentPage
{
    private LocationMainViewModel _viewModel;

    public LocationMainView(LocationModel location, string viewTitle)
	{
        LocationMainViewModel viewModel = new LocationMainViewModel(location, this);
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