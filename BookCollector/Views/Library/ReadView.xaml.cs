using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ReadView : ContentPage
{
    private ReadViewModel ViewModel {  get; set; }

    public ReadView()
	{
        var viewModel = new ReadViewModel(this);
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