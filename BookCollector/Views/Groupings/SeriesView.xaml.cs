using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class SeriesView : ContentPage
{
    private SeriesViewModel ViewModel { get; set; }

    public SeriesView()
	{
        var viewModel = new SeriesViewModel(this);
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