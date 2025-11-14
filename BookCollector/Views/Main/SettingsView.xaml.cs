using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Storage;

namespace BookCollector.Views.Main;

public partial class SettingsView : ContentPage
{
    public List<string> AppThemeList { get; set; }
    public string SelectedAppTheme { get; set; }
    public List<string> ColorList { get; set; }
    public string SelectedColor { get; set; }
    public List<string> LanguageList { get; set; }
    public string SelectedLanguage { get; set; }
    public string SelectedExportLocation { get; set; }
    public bool CommentsOn { get; set; }
    public bool ChaptersOn { get; set; }
    public bool FavoritesOn { get; set; }
    public bool RatingsOn { get; set; }
    public bool HiddenBooksOn { get; set; }

    // TO DO:
    // Add other languages - 11/12/2025
    // Try to add color preview in the picker - 11/12/2025
    // Add currency option - 11/13/2025
    // Add Hide Collection, Series, Genre, and Author - 11/13/2025

    public SettingsView()
	{
        AppThemeList = [AppStringResources.Light, AppStringResources.Dark];
        SelectedAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppThemeList[0] : AppThemeList[1];

        ColorList = [AppStringResources.BlueGray, "Red", "Purple", "Green", "Orange", "Teal", "Magenta"];
        var color = (Color)Application.Current.Resources["Primary"];
        var hexCode = color.ToHex().ToLower();
        SelectedColor = hexCode switch
        {
            "#ff0000" => ColorList[1],
            "#751aff" => ColorList[2],
            "#2db300" => ColorList[3],
            "#b36b00" => ColorList[4],
            "#248f8f" => ColorList[5],
            "#b300b3" => ColorList[6],
            _ => ColorList[0],
        };

        LanguageList = [AppStringResources.English];
        SelectedLanguage = LanguageList[0];

        var exportLocation = Preferences.Get("ExportLocation", AppStringResources.DefaultExportLocation  /* Default */);
        SelectedExportLocation = exportLocation.Equals("Not Set") ? exportLocation : exportLocation.Substring(exportLocation.IndexOf("0") + 2);

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
        var colorName = picker.SelectedItem.ToString();
        string hexCode = colorName switch
        {
            "Red" => "ff0000",
            "Purple" => "#751aff",
            "Green" => "#2db300",
            "Orange" => "#b36b00",
            "Teal" => "#248f8f",
            "Magenta" => "#b300b3",
            _ => "#336699"
        };

        var color = Color.FromArgb(hexCode);
        var secondary = color.AddLuminosity((float)0.1);
        var tertiary = color.AddLuminosity((float)0.2);

        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(hexCode));

        Application.Current.Resources["Primary"] = Color.FromArgb(hexCode);
        Application.Current.Resources["Secondary"] = Color.FromArgb(secondary.ToHex());
        Application.Current.Resources["Tertiary"] = Color.FromArgb(tertiary.ToHex());

        Preferences.Set("AppColor", hexCode);
        Preferences.Set("Secondary", secondary.ToHex());
        Preferences.Set("Tertiary", tertiary.ToHex());
    }

    void OnLanguagePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        Preferences.Set("Language", picker.SelectedItem.ToString());
        // TO DO:
        // Add ability to convert AppStringResources string to English string for the Preferences set
    }

    async void OnExportLocationButtonClicked(object sender, EventArgs e)
    {
        var result = await FolderPicker.Default.PickAsync(CancellationToken.None);
        SelectedExportLocation = result.Folder.Path.Substring(result.Folder.Path.IndexOf("0") + 2);
        SelectedExportLocationLabel.Text = result.Folder.Path.Substring(result.Folder.Path.IndexOf("0") + 2);
        Preferences.Set("ExportLocation", result.Folder.Path);
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