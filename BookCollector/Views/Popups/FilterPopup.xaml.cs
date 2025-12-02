using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;

namespace BookCollector.Views.Popups;

public partial class FilterPopup : Popup
{
    public string ViewTitle { get; set; }
    public double PopupWidth { get; set; }

	public bool FavoriteVisible { get; set; }
	public List<string> FavoritePicker { get; set; }
	public string FavoriteOption { get; set; }

	public FilterPopup(string viewTitle)
	{
        ViewTitle = viewTitle;
        PopupWidth = BaseViewModel.DeviceWidth - 50;

		FavoriteVisible = Preferences.Get("FavoritesOn", true  /* Default */);
        FavoritePicker = [AppStringResources.Favorites, AppStringResources.NonFavorites, AppStringResources.Both];
        FavoriteOption = Preferences.Get($"{viewTitle}_FavoriteSelection", AppStringResources.Both /* Default */);

        InitializeComponent();
        BindingContext = this;
    }

    public async void OnCloseButtonClicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        Preferences.Set($"{ViewTitle}_FavoriteSelection", FavoriteOption);

        await this.CloseAsync(token: cts.Token);
    }
}