using BookCollector.Data;
using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Storage;
using System.Globalization;

namespace BookCollector.Views.Main;

public partial class SettingsView : ContentPage
{
    public List<string> AppThemeList { get; set; }
    public string SelectedAppTheme { get; set; }
    public List<string> ColorList { get; set; }
    public string SelectedColor { get; set; }
    public List<string> LanguageList { get; set; }
    public string SelectedLanguage { get; set; }
    public List<string> CurrencyList { get; set; }
    public string SelectedCurrency { get; set; }
    public string SelectedExportLocation { get; set; }
    public bool CommentsOn { get; set; }
    public bool ChaptersOn { get; set; }
    public bool FavoritesOn { get; set; }
    public bool RatingsOn { get; set; }
    public bool HiddenBooksOn { get; set; }
    public bool HiddenCollectionsOn { get; set; }
    public bool HiddenGenresOn { get; set; }
    public bool HiddenSeriesOn { get; set; }
    public bool HiddenAuthorsOn { get; set; }
    public bool HiddenLocationsOn { get; set; }

    // TO DO:
    // Add other languages - 11/12/2025
    // Try to add color preview in the picker - 11/12/2025
    // Add more currency options - 12/1/2025
    // Try to add a color wheel instead of a dropdown picker - / 11/20/2025
    // WishListBook Toggle - 12/8/2025

    public SettingsView()
	{
        var userAppTheme = Application.Current?.UserAppTheme;

        if (userAppTheme == AppTheme.Unspecified)
            userAppTheme = Application.Current?.PlatformAppTheme;

        AppThemeList = [AppStringResources.Light, AppStringResources.Dark];
        SelectedAppTheme = userAppTheme == AppTheme.Dark ? AppThemeList[1] : AppThemeList[0];

        ColorList = [AppStringResources.BlueGray, "Red", "Purple", "Green", "Orange", "Teal", "Magenta"];
        var color = (Color?)Application.Current?.Resources["Primary"];
        var hexCode = color?.ToHex().ToLower();
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
        SelectedLanguage = Preferences.Get("Language", AppStringResources.English /* Default */);

        CurrencyList = ["$ USD"];
        SelectedCurrency = Preferences.Get("Currency", "$ USD" /* Default */);

        var exportLocation = Preferences.Get("ExportLocation", AppStringResources.DefaultExportLocation  /* Default */);
        SelectedExportLocation = exportLocation.Equals("Not Set") ? exportLocation : exportLocation[(exportLocation.IndexOf('0') + 2)..];

        CommentsOn = Preferences.Get("CommentsOn", true  /* Default */);
        ChaptersOn = Preferences.Get("ChaptersOn", true  /* Default */);
        FavoritesOn = Preferences.Get("FavoritesOn", true  /* Default */);
        RatingsOn = Preferences.Get("RatingsOn", true  /* Default */);
        
        HiddenBooksOn = Preferences.Get("HiddenBooksOn", true  /* Default */);
        HiddenCollectionsOn = Preferences.Get("HiddenCollectionsOn", true  /* Default */);
        HiddenGenresOn = Preferences.Get("HiddenGenresOn", true  /* Default */);
        HiddenSeriesOn = Preferences.Get("HiddenSeriesOn", true  /* Default */);
        HiddenAuthorsOn = Preferences.Get("HiddenAuthorsOn", true  /* Default */);
        HiddenLocationsOn = Preferences.Get("HiddenLocationsOn", true  /* Default */);

        InitializeComponent();
        BindingContext = this;
    }

    public void OnAppThemePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var item = picker.SelectedItem.ToString();

        if (!string.IsNullOrEmpty(item))
        {
            Application.Current?.UserAppTheme = item.Equals(AppStringResources.Light) ? AppTheme.Light : AppTheme.Dark;
            Preferences.Set("AppTheme", picker.SelectedItem.ToString());
            // TO DO:
            // Add ability to convert AppStringResources string to English string for the Preferences set
            // If not, when someone changes the language, the default preference will be a different string.
        }

    }

    public void OnColorPickerSelectedIndexChanged(object sender, EventArgs e)
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

        Data.Colors.SetColors(hexCode);

#if ANDROID
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(hexCode));
#endif

        Preferences.Set("AppColor", hexCode);
    }

    public void OnLanguagePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        Preferences.Set("Language", picker.SelectedItem.ToString());
        // TO DO:
        // Add ability to convert AppStringResources string to English string for the Preferences set
        // If not, when someone changes the language, the default preference will be a different string.
    }

    public void OnCurrencyPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;

        var item = picker.SelectedItem.ToString();

        if (!string.IsNullOrEmpty(item))
        {
            Preferences.Set("Currency", picker.SelectedItem.ToString());

            if (item.Equals("$ USD"))
                Preferences.Set("CultureCode", "en-US");
        }
    }

    public async void OnExportLocationButtonClicked(object sender, EventArgs e)
    {
        var result = await FolderPicker.Default.PickAsync(CancellationToken.None);

        var folder = result.Folder;

        if (folder != null)
        {
            SelectedExportLocation = folder.Path[(folder.Path.IndexOf('0') + 2)..];
            SelectedExportLocationLabel.Text = folder.Path[(folder.Path.IndexOf('0') + 2)..];
            Preferences.Set("ExportLocation", folder.Path);
        }
    }

    public void OnCommentsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("CommentsOn", e.Value);
    }

    public void OnChaptersToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("ChaptersOn", e.Value);
    }

    public void OnFavoritesToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("FavoritesOn", e.Value);
    }

    public void OnRatingsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("RatingsOn", e.Value);
    }

    public void OnHiddenBooksToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("HiddenBooksOn", e.Value);
    }

    public void OnHiddenCollectionsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("HiddenCollectionsOn", e.Value);
    }

    public void OnHiddenGenresToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("HiddenGenresOn", e.Value);
    }

    public void OnHiddenSeriesToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("HiddenSeriesOn", e.Value);
    }

    public void OnHiddenAuthorsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("HiddenAuthorsOn", e.Value);
    }

    public void OnHiddenLocationsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("HiddenLocationsOn", e.Value);
    }
}