using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class ExistingBooksView : ContentPage
{
    private ExistingBooksViewModel ViewModel;

    public ExistingBooksView(object selected, string viewTitle)
	{
        var viewModel = new ExistingBooksViewModel(selected, this)
        {
            ViewTitle = viewTitle
        };
        ViewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    // Need this to make sure books populate when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var _ = ViewModel.SetViewModelData();
    }
}