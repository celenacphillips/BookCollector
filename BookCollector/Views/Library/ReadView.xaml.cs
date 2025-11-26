using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ReadView : ContentPage
{
    private ReadViewModel _viewModel {  get; set; }

	public ReadView()
	{
        ReadViewModel viewModel = new ReadViewModel(this);
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