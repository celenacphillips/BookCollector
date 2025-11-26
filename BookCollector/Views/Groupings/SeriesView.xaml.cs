using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class SeriesView : ContentPage
{
    private SeriesViewModel _viewModel { get; set; }

    public SeriesView()
	{
        SeriesViewModel viewModel = new SeriesViewModel(this);
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