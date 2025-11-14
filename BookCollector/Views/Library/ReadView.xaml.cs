using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ReadView : ContentPage
{
	public ReadView()
	{
        ReadViewModel viewModel = new ReadViewModel(this);
        BindingContext = viewModel;

        InitializeComponent();
	}
}