using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class LocationsView : ContentPage
{
    private LocationsViewModel _viewModel { get; set; }

    public LocationsView()
	{
        LocationsViewModel viewModel = new LocationsViewModel(this);
        _viewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    // Need this to make sure new book info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        _viewModel.SetViewModelData();
    }
}