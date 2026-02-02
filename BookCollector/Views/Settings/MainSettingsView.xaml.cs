// <copyright file="MainSettingsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.Database;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Controls;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Storage;

namespace BookCollector.Views.Settings;

public partial class MainSettingsView : ContentPage
{
    internal static BookCollectorDatabase Database;

    public MainSettingsView()
    {
        Database = new BookCollectorDatabase();

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

    public List<string> AppThemeList { get; set; }

    private string selectedAppThemeField;

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

    public List<string> ColorList { get; set; }

    private string selectedColorField;

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

    public List<string> LanguageList { get; set; }

    private string selectedLanguageField;

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

    public List<string> CurrencyList { get; set; }

    private string selectedCurrencyField;

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

    public string SelectedExportLocation { get; set; }

    public async void OnExportLocationButtonClicked(object sender, EventArgs e)
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

    public async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var inputTitle = AppStringResources.DeleteAllData_Question;

        var inputConfirm = AppStringResources.Yes;

        var inputDeny = AppStringResources.No;

        var inputMessage = AppStringResources.WarningThisActionCannotBeUndone;

        var action = await Shell.Current.DisplayAlertAsync(inputTitle, inputMessage, inputConfirm, inputDeny);

        if (action)
        {
            await Database.DropAllTables();
            BaseViewModel.ClearAllLists();

            await Shell.Current.DisplayAlertAsync(AppStringResources.AllDataHasBeenDeleted, AppStringResources.AllDataHasBeenDeleted, AppStringResources.OK);
        }
        else
        {
            await Shell.Current.DisplayAlertAsync(AppStringResources.ActionCanceled, AppStringResources.ActionCanceled, AppStringResources.OK);
        }
    }

    private async void AppThemePickerButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var filterablePopup = new FilterableListPopup(
                AppStringResources.SelectYourAppTheme,
                this.AppThemeList.ToList(),
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
                this.ColorList.ToList(),
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
                this.LanguageList.ToList(),
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
                this.CurrencyList.ToList(),
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