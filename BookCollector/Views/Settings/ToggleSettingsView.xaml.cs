// <copyright file="ToggleSettingsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Main;

namespace BookCollector.Views.Settings;

public partial class ToggleSettingsView : ContentPage
{
    public ToggleSettingsView()
    {
        this.CommentsOn = Preferences.Get("CommentsOn", true /* Default */);
        this.ChaptersOn = Preferences.Get("ChaptersOn", true /* Default */);
        this.FavoritesOn = Preferences.Get("FavoritesOn", true /* Default */);
        this.RatingsOn = Preferences.Get("RatingsOn", true /* Default */);

        this.HiddenBooksOn = Preferences.Get("HiddenBooksOn", true /* Default */);
        this.HiddenCollectionsOn = Preferences.Get("HiddenCollectionsOn", true /* Default */);
        this.HiddenGenresOn = Preferences.Get("HiddenGenresOn", true /* Default */);
        this.HiddenSeriesOn = Preferences.Get("HiddenSeriesOn", true /* Default */);
        this.HiddenAuthorsOn = Preferences.Get("HiddenAuthorsOn", true /* Default */);
        this.HiddenLocationsOn = Preferences.Get("HiddenLocationsOn", true /* Default */);
        this.HiddenWishlistBooksOn = Preferences.Get("HiddenWishlistBooksOn", true /* Default */);

        this.InitializeComponent();
        this.BindingContext = this;
    }

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

    public bool HiddenWishlistBooksOn { get; set; }

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
        ReadingViewModel.RefreshView = true;
        ToBeReadViewModel.RefreshView = true;
        ReadViewModel.RefreshView = true;
        AllBooksViewModel.RefreshView = true;

        Preferences.Set("HiddenBooksOn", e.Value);
    }

    public void OnHiddenCollectionsToggled(object sender, ToggledEventArgs e)
    {
        CollectionsViewModel.RefreshView = true;

        Preferences.Set("HiddenCollectionsOn", e.Value);
    }

    public void OnHiddenGenresToggled(object sender, ToggledEventArgs e)
    {
        GenresViewModel.RefreshView = true;

        Preferences.Set("HiddenGenresOn", e.Value);
    }

    public void OnHiddenSeriesToggled(object sender, ToggledEventArgs e)
    {
        SeriesViewModel.RefreshView = true;

        Preferences.Set("HiddenSeriesOn", e.Value);
    }

    public void OnHiddenAuthorsToggled(object sender, ToggledEventArgs e)
    {
        AuthorsViewModel.RefreshView = true;

        Preferences.Set("HiddenAuthorsOn", e.Value);
    }

    public void OnHiddenLocationsToggled(object sender, ToggledEventArgs e)
    {
        LocationsViewModel.RefreshView = true;

        Preferences.Set("HiddenLocationsOn", e.Value);
    }

    public void OnHiddenWishlistBooksToggled(object sender, ToggledEventArgs e)
    {
        WishListViewModel.RefreshView = true;

        Preferences.Set("HiddenWishlistBooksOn", e.Value);
    }
}