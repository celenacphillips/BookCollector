using BookCollector.Resources.Localization;

namespace BookCollector.Views;

public partial class SettingsView : ContentPage
{
    public List<string> AppThemeList { get; set; }
    public string SelectedAppTheme { get; set; }
    public List<string> ColorList { get; set; }
    public string SelectedColor { get; set; }
    public List<string> LanguageList { get; set; }
    public string SelectedLanguage { get; set; }
    public bool CommentsOn { get; set; }
    public bool ChaptersOn { get; set; }
    public bool FavoritesOn { get; set; }
    public bool RatingsOn { get; set; }
    public bool HiddenBooksOn { get; set; }


    // TO DO:
    // Pick more colors - 11/12/2025
    // Try to add color preview in the picker - 11/12/2025
    // Export location - 11/12/2025
    // Language - 11/12/2025

    public SettingsView()
	{
        AppThemeList = [AppStringResources.Light, AppStringResources.Dark];
        SelectedAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppThemeList[0] : AppThemeList[1];

        ColorList = [AppStringResources.BlueGray, "Red"];
        var color = (Color)Application.Current.Resources["Primary"];
        var hexCode = color.ToHex().ToLower();
        SelectedColor = hexCode switch
        {
            "#ff0000" => ColorList[1],
            _ => ColorList[0],
        };

        LanguageList = [AppStringResources.English];
        SelectedLanguage = LanguageList[0];

        CommentsOn = Preferences.Get("CommentsOn", true  /* Default */);
        ChaptersOn = Preferences.Get("ChaptersOn", true  /* Default */);
        FavoritesOn = Preferences.Get("FavoritesOn", true  /* Default */);
        RatingsOn = Preferences.Get("RatingsOn", true  /* Default */);
        HiddenBooksOn = Preferences.Get("HiddenBooksOn", true  /* Default */);

        InitializeComponent();
        BindingContext = this;
    }

    void OnAppThemePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        Application.Current.UserAppTheme = picker.SelectedItem.ToString().Equals(AppStringResources.Light) ? AppTheme.Light : AppTheme.Dark;
        Preferences.Set("AppTheme", picker.SelectedItem.ToString());
        // TO DO:
        // Add ability to convert AppStringResources string to English string for the Preferences set
    }

    void OnColorPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var color = picker.SelectedItem.ToString();
        string hexCode = color switch
        {
            "Red" => "ff0000",
            _ => "#336699"
        };
        Application.Current.Resources["Primary"] = Color.FromArgb(hexCode);
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(hexCode));
        Preferences.Set("AppColor", hexCode);
    }

    void OnLanguagePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        Preferences.Set("Language", picker.SelectedItem.ToString());
        // TO DO:
        // Add ability to convert AppStringResources string to English string for the Preferences set
    }

    void OnCommentsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("CommentsOn", e.Value);
    }

    void OnChaptersToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("ChaptersOn", e.Value);
    }

    void OnFavoritesToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("FavoritesOn", e.Value);
    }

    void OnRatingsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("RatingsOn", e.Value);
    }

    void OnHiddenBooksToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("HiddenBooksOn", e.Value);
    }
}