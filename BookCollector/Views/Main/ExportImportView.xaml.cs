using BookCollector.ViewModels.Main;

namespace BookCollector.Views.Main;

public partial class ExportImportView : ContentPage
{
	private ExportImportViewModel ViewModel;

    public ExportImportView()
	{
        var viewModel = new ExportImportViewModel(this);
        ViewModel = viewModel;
        BindingContext = viewModel;

		InitializeComponent();
	}

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        var _ = ViewModel.SetViewModelData();
    }
}