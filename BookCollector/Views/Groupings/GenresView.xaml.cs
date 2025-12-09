using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class GenresView : ContentPage
{
    private GenresViewModel ViewModel { get; set; }

    public GenresView()
	{
        var viewModel = new GenresViewModel(this);
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