using BookCollector.Data.Models;
using BookCollector.ViewModels.Location;

namespace BookCollector.Views.Location;

public partial class LocationMainView : ContentPage
{
    private LocationMainViewModel ViewModel;

    public LocationMainView(LocationModel location, string viewTitle)
	{
        var viewModel = new LocationMainViewModel(location, this)
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