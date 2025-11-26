using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class GenresView : ContentPage
{
    private GenresViewModel _viewModel { get; set; }

    public GenresView()
	{
        GenresViewModel viewModel = new GenresViewModel(this);
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