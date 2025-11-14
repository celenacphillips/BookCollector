using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ToBeReadView : ContentPage
{
	public ToBeReadView()
	{
        ToBeReadViewModel viewModel = new ToBeReadViewModel(this);
        BindingContext = viewModel;

        InitializeComponent();
	}
}