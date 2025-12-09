using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class LocationsView : ContentPage
{
    private LocationsViewModel ViewModel { get; set; }

    public LocationsView()
	{
        var viewModel = new LocationsViewModel(this);
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