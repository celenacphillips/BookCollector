using BookCollector.ViewModels.Main;

namespace BookCollector.Views.Main;

public partial class ExportImportView : ContentPage
{
	private ExportImportViewModel _viewModel;

	public ExportImportView()
	{
        ExportImportViewModel viewModel = new ExportImportViewModel(this);
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