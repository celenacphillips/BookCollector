using BookCollector.Data.Models;
using BookCollector.ViewModels.Location;

namespace BookCollector.Views.Location;

public partial class LocationMainView : ContentPage
{
    private LocationMainViewModel viewModel;

    public LocationMainView(LocationModel location, string viewTitle)
    {
        this.viewModel = new LocationMainViewModel(location, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.viewModel.SetViewModelData();
    }
}