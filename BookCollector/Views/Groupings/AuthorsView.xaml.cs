using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class AuthorsView : ContentPage
{
    private AuthorsViewModel ViewModel { get; set; }

    public AuthorsView()
	{
        var viewModel = new AuthorsViewModel(this);
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