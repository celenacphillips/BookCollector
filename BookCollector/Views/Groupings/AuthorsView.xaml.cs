using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class AuthorsView : ContentPage
{
    private AuthorsViewModel _viewModel { get; set; }

    public AuthorsView()
	{
        AuthorsViewModel viewModel = new AuthorsViewModel(this);
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