using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ToBeReadView : ContentPage
{
    private ToBeReadViewModel _viewModel {  get; set; }

	public ToBeReadView()
	{
        ToBeReadViewModel viewModel = new ToBeReadViewModel(this);
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