using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class ExistingBooksView : ContentPage
{
    private ExistingBooksViewModel _viewModel;

    public ExistingBooksView(object selected, string viewTitle)
	{
        ExistingBooksViewModel viewModel = new ExistingBooksViewModel(selected, this);
        viewModel.ViewTitle = viewTitle;
        _viewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    // Need this to make sure books populate when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        _viewModel.SetViewModelData();
    }
}