namespace BookCollector.Views.Library;

public partial class ReadingView : ContentPage
{
	public ReadingView()
	{
        var savedColor = Preferences.Get("AppColor", "#336699"  /* Default */);
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(savedColor));

        InitializeComponent();
	}
}