// <copyright file="MainSettingsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.Database;
using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Storage;

namespace BookCollector.Views.Settings;

public partial class MainSettingsView : ContentPage
{
    internal static BookCollectorDatabase Database;

    public MainSettingsView()
    {
        Database = new BookCollectorDatabase();

        var userAppTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme : Application.Current?.UserAppTheme;

        this.AppThemeList = [AppStringResources.Light, AppStringResources.Dark];
        this.SelectedAppTheme = userAppTheme == AppTheme.Dark ? this.AppThemeList[1] : this.AppThemeList[0];

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

    public string SelectedAppTheme { get; set; }

    public List<string> ColorList { get; set; }

    public string SelectedColor { get; set; }

    public List<string> LanguageList { get; set; }

    public string SelectedLanguage { get; set; }

    public List<string> CurrencyList { get; set; }

    public string SelectedCurrency { get; set; }

    public string SelectedExportLocation { get; set; }

    public void OnAppThemePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var item = picker.SelectedItem.ToString();

        if (!string.IsNullOrEmpty(item))
        {
            Application.Current?.UserAppTheme = item.Equals(AppStringResources.Light) ? AppTheme.Light : AppTheme.Dark;
            Preferences.Set("AppTheme", picker.SelectedItem.ToString());
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

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            try
            {
                CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(hexCode));
            }
            catch
            {
            }
        }

        Preferences.Set("AppColor", hexCode);
    }

    public void OnLanguagePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        Preferences.Set("Language", picker.SelectedItem.ToString());
    }

    public void OnCurrencyPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;

        var item = picker.SelectedItem.ToString();

        if (!string.IsNullOrEmpty(item))
        {
            Preferences.Set("Currency", picker.SelectedItem.ToString());

            if (item.Equals("$ USD"))
            {
                Preferences.Set("CultureCode", "en-US");
            }
        }
    }

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
            if (TestData.UseTestData)
            {
                TestData.DeleteAllData();
            }
            else
            {
                await Database.DropAllTables();
            }

            await Shell.Current.DisplayAlertAsync(AppStringResources.AllDataHasBeenDeleted, AppStringResources.AllDataHasBeenDeleted, AppStringResources.OK);
        }
        else
        {
            await Shell.Current.DisplayAlertAsync(AppStringResources.ActionCanceled, AppStringResources.ActionCanceled, AppStringResources.OK);
        }
    }
}