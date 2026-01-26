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

    private bool hiddenBooksOnField;

    public bool HiddenBooksOn
    {
        get => this.hiddenBooksOnField;
        set
        {
            if (this.hiddenBooksOnField != value)
            {
                this.hiddenBooksOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    private bool hiddenCollectionsOnField;

    public bool HiddenCollectionsOn
    {
        get => this.hiddenCollectionsOnField;
        set
        {
            if (this.hiddenCollectionsOnField != value)
            {
                this.hiddenCollectionsOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    private bool hiddenGenresOnField;

    public bool HiddenGenresOn
    {
        get => this.hiddenGenresOnField;
        set
        {
            if (this.hiddenGenresOnField != value)
            {
                this.hiddenGenresOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    private bool hiddenSeriesOnField;

    public bool HiddenSeriesOn
    {
        get => this.hiddenSeriesOnField;
        set
        {
            if (this.hiddenSeriesOnField != value)
            {
                this.hiddenSeriesOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    private bool hiddenAuthorsOnField;

    public bool HiddenAuthorsOn
    {
        get => this.hiddenAuthorsOnField;
        set
        {
            if (this.hiddenAuthorsOnField != value)
            {
                this.hiddenAuthorsOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    private bool hiddenLocationsOnField;

    public bool HiddenLocationsOn
    {
        get => this.hiddenLocationsOnField;
        set
        {
            if (this.hiddenLocationsOnField != value)
            {
                this.hiddenLocationsOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

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
        CollectionsViewModel.RefreshView = true;
        GenresViewModel.RefreshView = true;
        SeriesViewModel.RefreshView = true;
        AuthorsViewModel.RefreshView = true;
        LocationsViewModel.RefreshView = true;

        if (e.Value)
        {
            if (!this.HiddenCollectionsOn)
            {
                this.HiddenCollectionsOn = true;
            }

            if (!this.HiddenGenresOn)
            {
                this.HiddenGenresOn = true;
            }

            if (!this.HiddenSeriesOn)
            {
                this.HiddenSeriesOn = true;
            }

            if (!this.HiddenAuthorsOn)
            {
                this.HiddenAuthorsOn = true;
            }

            if (!this.HiddenLocationsOn)
            {
                this.HiddenLocationsOn = true;
            }
        }

        Preferences.Set("HiddenBooksOn", e.Value);
    }

    public void OnHiddenCollectionsToggled(object sender, ToggledEventArgs e)
    {
        CollectionsViewModel.RefreshView = true;

        var variable = CollectionsViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = false;
        }

        Preferences.Set("HiddenCollectionsOn", e.Value);
    }

    public void OnHiddenGenresToggled(object sender, ToggledEventArgs e)
    {
        GenresViewModel.RefreshView = true;

        var variable = GenresViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = e.Value;
        }

        Preferences.Set("HiddenGenresOn", e.Value);
    }

    public void OnHiddenSeriesToggled(object sender, ToggledEventArgs e)
    {
        SeriesViewModel.RefreshView = true;

        var variable = SeriesViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = e.Value;
        }

        Preferences.Set("HiddenSeriesOn", e.Value);
    }

    public void OnHiddenAuthorsToggled(object sender, ToggledEventArgs e)
    {
        AuthorsViewModel.RefreshView = true;

        var variable = AuthorsViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = e.Value;
        }

        Preferences.Set("HiddenAuthorsOn", e.Value);
    }

    public void OnHiddenLocationsToggled(object sender, ToggledEventArgs e)
    {
        LocationsViewModel.RefreshView = true;

        var variable = LocationsViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = e.Value;
        }

        Preferences.Set("HiddenLocationsOn", e.Value);
    }

    public void OnHiddenWishlistBooksToggled(object sender, ToggledEventArgs e)
    {
        WishListViewModel.RefreshView = true;

        Preferences.Set("HiddenWishlistBooksOn", e.Value);
    }
}