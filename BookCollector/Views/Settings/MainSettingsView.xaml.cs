// <copyright file="MainSettingsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Settings;

using BookCollector.Data.Enums;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Storage;

/// <summary>
/// MainSettingsView class.
/// </summary>
public partial class MainSettingsView : ContentPage
{
    private readonly string appThemeDefault = Application.Current?.PlatformAppTheme == AppTheme.Dark ? AppStringResources.Dark : AppStringResources.Light;

    private readonly string appColorDefault = "#336699";

    private readonly string exportLocationDefault = AppStringResources.DefaultExportLocation;

    private readonly string appLanguageDefault = AppStringResources.English;

    private readonly string appCurrencyDefault = "$ USD";

    private readonly string cultureCodeDefault = "en-US";

    private string selectedAppThemeField;

    private string selectedColorField;

    private string selectedLanguageField;

    private string selectedCurrencyField;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainSettingsView"/> class.
    /// </summary>
    public MainSettingsView()
    {
        this.AppThemeList = [AppStringResources.Light, AppStringResources.Dark];
        this.SelectedAppTheme = Application.Current?.UserAppTheme == AppTheme.Dark ? this.AppThemeList[1] : this.AppThemeList[0];

        this.SelectedColor = Preferences.Get("AppColor", this.appColorDefault /* Default */);

        this.LanguageList = [AppStringResources.English];
        this.SelectedLanguage = Preferences.Get("Language", this.appLanguageDefault /* Default */);

        this.CurrencyList = ["$ USD"];
        this.SelectedCurrency = Preferences.Get("Currency", this.appCurrencyDefault /* Default */);

        var exportLocation = Preferences.Get("ExportLocation", this.exportLocationDefault /* Default */);
        this.SelectedExportLocation = exportLocation.Equals("Not Set") ? exportLocation : exportLocation[(exportLocation.IndexOf('0') + 2) ..];

        this.InitializeComponent();
        this.BindingContext = this;
    }

    /// <summary>
    /// Gets or sets the list of app themes.
    /// </summary>
    public List<string> AppThemeList { get; set; }

    /// <summary>
    /// Gets or sets the selected app theme.
    /// </summary>
    public string SelectedAppTheme
    {
        get => this.selectedAppThemeField;
        set
        {
            if (this.selectedAppThemeField != value)
            {
                this.selectedAppThemeField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the selected color.
    /// </summary>
    public string SelectedColor
    {
        get => this.selectedColorField;
        set
        {
            if (this.selectedColorField != value)
            {
                this.selectedColorField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the list of languages.
    /// </summary>
    public List<string> LanguageList { get; set; }

    /// <summary>
    /// Gets or sets the selected language.
    /// </summary>
    public string SelectedLanguage
    {
        get => this.selectedLanguageField;
        set
        {
            if (this.selectedLanguageField != value)
            {
                this.selectedLanguageField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the list of currencies.
    /// </summary>
    public List<string> CurrencyList { get; set; }

    /// <summary>
    /// Gets or sets the selected currency.
    /// </summary>
    public string SelectedCurrency
    {
        get => this.selectedCurrencyField;
        set
        {
            if (this.selectedCurrencyField != value)
            {
                this.selectedCurrencyField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the selected export location.
    /// </summary>
    public string SelectedExportLocation { get; set; }

    private async void OnExportLocationButton_Clicked(object sender, EventArgs e)
    {
        var result = await FolderPicker.Default.PickAsync(CancellationToken.None);

        var folder = result.Folder;

        if (folder != null)
        {
            this.SelectedExportLocation = folder.Path[(folder.Path.IndexOf('0') + 2) ..];
            this.SelectedExportLocationLabel.Text = folder.Path[(folder.Path.IndexOf('0') + 2) ..];
            Preferences.Set("ExportLocation", folder.Path);
        }
    }

    private async void OnDeleteButton_Clicked(object sender, EventArgs e)
    {
        var deviceWidth = DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;

        var inputTitle = AppStringResources.DeleteAllData_Question;

        var inputConfirm = AppStringResources.Yes;

        var inputDeny = AppStringResources.No;

        var inputMessage = AppStringResources.WarningThisActionCannotBeUndone;

        var answer = await this.ShowPopupAsync<string>(new ChoiceDialogPopup(deviceWidth - 50, inputTitle, inputMessage, inputConfirm, inputDeny, DialogState.Choice));

        if (!string.IsNullOrEmpty(answer.Result) && answer.Result.Equals(inputConfirm))
        {
            await BaseViewModel.Database.DropAllTables();
            BaseViewModel.ClearAllLists();

            await this.ShowPopupAsync<string>(new ChoiceDialogPopup(deviceWidth - 50, AppStringResources.AllDataHasBeenDeleted, AppStringResources.AllDataHasBeenDeleted, inputConfirm, null, DialogState.Choice));
        }
        else
        {
            await this.ShowPopupAsync<string>(new ChoiceDialogPopup(deviceWidth - 50, AppStringResources.ActionCanceled, AppStringResources.ActionCanceled, inputConfirm, null, DialogState.Choice));
        }
    }

    private async void AppThemePickerButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var filterablePopup = new FilterableListPopup(
                AppStringResources.SelectYourAppTheme,
                [.. this.AppThemeList],
                this.SelectedAppTheme,
                false);
            var result = await this.ShowPopupAsync<string?>(filterablePopup);

            if (!string.IsNullOrEmpty(result.Result))
            {
                this.SelectedAppTheme = result.Result;
                Application.Current?.UserAppTheme = result.Result.Equals(AppStringResources.Light) ? AppTheme.Light : AppTheme.Dark;
                Preferences.Set("AppTheme", result.Result.ToString());
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async void ColorPickerButton_Clicked(object sender, EventArgs e)
    {
        var color = Color.FromArgb(this.SelectedColor);

        Data.Colors.SetPreviewColors(this.SelectedColor);

        var colorPickerResult = await this.ShowPopupAsync<string>(new ColorPickerPopup(color));

        if (!colorPickerResult.WasDismissedByTappingOutsideOfPopup)
        {
            var hexCode = colorPickerResult.Result;
            this.SelectedColor = hexCode!;
            Data.Colors.SetColors(hexCode!);

            // https://developer.android.com/about/versions/15/behavior-changes-15#custom-background-protection
            Preferences.Set("AppColor", hexCode!);
        }
    }

    private async void LanguagePickerButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var filterablePopup = new FilterableListPopup(
                AppStringResources.SelectYourAppLanguage,
                [.. this.LanguageList],
                this.SelectedLanguage,
                true);
            var result = await this.ShowPopupAsync<string?>(filterablePopup);

            if (!string.IsNullOrEmpty(result.Result))
            {
                this.SelectedLanguage = result.Result;
                Preferences.Set("Language", this.SelectedLanguage);
            }
        }
        catch (Exception ex)
        {
        }
    }

    private async void CurrencyPickerButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var filterablePopup = new FilterableListPopup(
                AppStringResources.SelectYourAppCurrency,
                [.. this.CurrencyList],
                this.SelectedCurrency,
                true);
            var result = await this.ShowPopupAsync<string?>(filterablePopup);

            if (!string.IsNullOrEmpty(result.Result))
            {
                this.SelectedCurrency = result.Result;
                Preferences.Set("Currency", this.SelectedCurrency);

                if (this.SelectedCurrency.Equals("$ USD"))
                {
                    Preferences.Set("CultureCode", "en-US");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void OnAppThemeResetButton_Clicked(object sender, EventArgs e)
    {
        this.SelectedAppTheme = this.appThemeDefault;
        Application.Current?.UserAppTheme = this.appThemeDefault.Equals(AppStringResources.Light) ? AppTheme.Light : AppTheme.Dark;
        Preferences.Set("AppTheme", this.appThemeDefault);
    }

    private void OnColorResetButton_Clicked(object sender, EventArgs e)
    {
        this.SelectedColor = this.appColorDefault;
        Data.Colors.SetColors(this.appColorDefault);
        Data.Colors.SetPreviewColors(this.appColorDefault);
        Preferences.Set("AppColor", this.appColorDefault);
    }

    private void OnExportLocationResetButton_Clicked(object sender, EventArgs e)
    {
        this.SelectedExportLocation = this.exportLocationDefault;
        this.SelectedExportLocationLabel.Text = this.exportLocationDefault;
        Preferences.Set("ExportLocation", this.exportLocationDefault);
    }

    private void OnLanguageResetButton_Clicked(object sender, EventArgs e)
    {
        this.SelectedLanguage = this.appLanguageDefault;
        this.SelectedCurrency = this.appCurrencyDefault;
        Preferences.Set("Language", this.appLanguageDefault);
    }

    private void OnCurrencyResetButton_Clicked(object sender, EventArgs e)
    {
        this.SelectedCurrency = this.appCurrencyDefault;
        Preferences.Set("Currency", this.appCurrencyDefault);
        Preferences.Set("CultureCode", this.cultureCodeDefault);
    }
}