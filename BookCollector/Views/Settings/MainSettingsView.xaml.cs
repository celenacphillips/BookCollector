// <copyright file="MainSettingsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Settings;

using BookCollector.Data.Database;
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
    private static BookCollectorDatabase database;

    private string selectedAppThemeField;

    private string selectedColorField;

    private string selectedLanguageField;

    private string selectedCurrencyField;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainSettingsView"/> class.
    /// </summary>
    public MainSettingsView()
    {
        database = new BookCollectorDatabase();

        this.AppThemeList = [AppStringResources.Light, AppStringResources.Dark];
        this.SelectedAppTheme = Application.Current?.UserAppTheme == AppTheme.Dark ? this.AppThemeList[1] : this.AppThemeList[0];

        this.ColorList = [AppStringResources.BlueGray, "Red", "Purple", "Green", "Orange", "Teal", "Magenta"];
        var color = (Color?)Application.Current?.Resources["Primary"];
        var hexCode = color?.ToHex().ToLower();
        this.SelectedColor = hexCode switch
        {
            "#ff0000" => this.ColorList[1],
            "#751aff" => this.ColorList[2],
            "#2db300" => this.ColorList[3],
            "#b36b00" => this.ColorList[4],
            "#248f8f" => this.ColorList[5],
            "#b300b3" => this.ColorList[6],
            _ => this.ColorList[0],
        };

        this.LanguageList = [AppStringResources.English];
        this.SelectedLanguage = Preferences.Get("Language", AppStringResources.English /* Default */);

        this.CurrencyList = ["$ USD"];
        this.SelectedCurrency = Preferences.Get("Currency", "$ USD" /* Default */);

        var exportLocation = Preferences.Get("ExportLocation", AppStringResources.DefaultExportLocation /* Default */);
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
    /// Gets or sets the list of colors.
    /// </summary>
    public List<string> ColorList { get; set; }

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

    private async void OnExportLocationButtonClicked(object sender, EventArgs e)
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

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var deviceWidth = DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;

        var inputTitle = AppStringResources.DeleteAllData_Question;

        var inputConfirm = AppStringResources.Yes;

        var inputDeny = AppStringResources.No;

        var inputMessage = AppStringResources.WarningThisActionCannotBeUndone;

        var answer = await this.ShowPopupAsync<string>(new ChoiceDialogPopup(deviceWidth - 50, inputTitle, inputMessage, inputConfirm, inputDeny, "Commands"));

        if (!string.IsNullOrEmpty(answer.Result) && answer.Result.Equals(inputConfirm))
        {
            await database.DropAllTables();
            BaseViewModel.ClearAllLists();

            await this.ShowPopupAsync<string>(new ChoiceDialogPopup(deviceWidth - 50, AppStringResources.AllDataHasBeenDeleted, AppStringResources.AllDataHasBeenDeleted, inputConfirm, null, "Commands"));
        }
        else
        {
            await this.ShowPopupAsync<string>(new ChoiceDialogPopup(deviceWidth - 50, AppStringResources.ActionCanceled, AppStringResources.ActionCanceled, inputConfirm, null, "Commands"));
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
        try
        {
            var filterablePopup = new FilterableListPopup(
                AppStringResources.SelectYourAppColor,
                [.. this.ColorList],
                this.SelectedColor,
                false);
            var result = await this.ShowPopupAsync<string?>(filterablePopup);

            if (!string.IsNullOrEmpty(result.Result))
            {
                this.SelectedColor = result.Result;

                string hexCode = this.SelectedColor switch
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

                //#if ANDROID
                //                CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(hexCode));
                //#endif

                Preferences.Set("AppColor", hexCode);
            }
        }
        catch (Exception ex)
        {
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
                this.SelectedAppTheme = result.Result;
                Preferences.Set("Currency", this.SelectedAppTheme);

                if (this.SelectedAppTheme.Equals("$ USD"))
                {
                    Preferences.Set("CultureCode", "en-US");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
}