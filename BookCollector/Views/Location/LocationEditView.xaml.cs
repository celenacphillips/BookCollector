using BookCollector.Data.Models;
using BookCollector.ViewModels.Location;

namespace BookCollector.Views.Location;

public partial class LocationEditView : ContentPage
{
    private LocationEditViewModel ViewModel { get; set; }

    public LocationEditView(LocationModel location, string viewTitle, bool insertMainViewBefore = false)
	{
        var viewModel = new LocationEditViewModel(location, this)
        {
            ViewTitle = viewTitle,
            InsertMainViewBefore = insertMainViewBefore
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