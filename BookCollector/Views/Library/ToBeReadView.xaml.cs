using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ToBeReadView : ContentPage
{
    private ToBeReadViewModel ViewModel {  get; set; }

    public ToBeReadView()
	{
        var viewModel = new ToBeReadViewModel(this);
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