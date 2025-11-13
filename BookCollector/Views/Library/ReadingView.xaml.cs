using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ReadingView : ContentPage
{
	public ReadingView()
	{
        // Put on first view to set the status bar to whatever color the user wants the app to be.
        var savedColor = Preferences.Get("AppColor", "#336699"  /* Default */);
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(savedColor));

        ReadingViewModel viewModel = new ReadingViewModel(this);
        BindingContext = viewModel;

        InitializeComponent();
    }
}