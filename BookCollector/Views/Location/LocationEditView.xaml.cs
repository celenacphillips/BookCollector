using BookCollector.Data.Models;
using BookCollector.ViewModels.Location;

namespace BookCollector.Views.Location;

public partial class LocationEditView : ContentPage
{
    public LocationEditView(LocationModel location, string viewTitle, bool insertMainViewBefore = false)
    {
        this.ViewModel = new LocationEditViewModel(location, this)
        {
            ViewTitle = viewTitle,
            InsertMainViewBefore = insertMainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private LocationEditViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.ViewModel.SetViewModelData();
    }
}