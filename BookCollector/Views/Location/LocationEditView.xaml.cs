using BookCollector.Data.Models;
using BookCollector.ViewModels.Location;

namespace BookCollector.Views.Location;

public partial class LocationEditView : ContentPage
{
    private LocationEditViewModel _viewModel { get; set; }

    public LocationEditView(LocationModel location, string viewTitle)
	{
        LocationEditViewModel viewModel = new LocationEditViewModel(location, this);
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