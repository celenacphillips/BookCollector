namespace BookCollector.Views;

public partial class SettingsView : ContentPage
{
    public List<string> AppThemeList { get; set; }
    public string SelectedAppTheme { get; set; }
    public List<string> ColorList { get; set; }
    public string SelectedColor { get; set; }


    // TO DO:
    // Pick more colors - 11/12/2025
    // Try to add color preview in the picker - 11/12/2025
    // Export location - 11/12/2025
    // Language - 11/12/2025
    // Turn on Comments - 11/12/2025
    // Turn on Chapters - 11/12/2025
    // Turn on Favorites - 11/12/2025
    // Turn on Ratings - 11/12/2025
    // Turn on Hidden Books - 11/12/2025

    public SettingsView()
	{
        AppThemeList = ["Light", "Dark"];
        SelectedAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppThemeList[0] : AppThemeList[1];

        ColorList = ["Blue Gray", "Red"];
        var color = (Color)Application.Current.Resources["Primary"];
        var hexCode = color.ToHex().ToLower();
        SelectedColor = hexCode switch
        {
            "#ff0000" => ColorList[1],
            _ => ColorList[0],
        };

        InitializeComponent();
        BindingContext = this;
    }

    void OnAppThemePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        ChangeTheme(picker.SelectedItem.ToString());
    }

    void OnColorPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        ChangeColor(picker.SelectedItem.ToString());
    }

    private void ChangeTheme(string theme)
    {
        Application.Current.UserAppTheme = theme.Equals("Light") ? AppTheme.Light : AppTheme.Dark;
        Preferences.Set("AppTheme", theme);
    }

    private void ChangeColor(string color)
    {
        string hexCode = color switch
        {
            "Red" => "ff0000",
            _ => "#336699"
        };
        Application.Current.Resources["Primary"] = Color.FromArgb(hexCode);
        // Update status bar color manually when theme changes
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(hexCode));
        Preferences.Set("AppColor", hexCode);
    }
}