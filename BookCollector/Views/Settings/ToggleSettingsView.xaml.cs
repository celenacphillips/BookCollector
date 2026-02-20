// <copyright file="ToggleSettingsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Settings;

using BookCollector.ViewModels.Groupings;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Main;

/// <summary>
/// ToggleSettingsView class.
/// </summary>
public partial class ToggleSettingsView : ContentPage
{
    private bool hiddenBooksOnField;

    private bool hiddenCollectionsOnField;

    private bool hiddenGenresOnField;

    private bool hiddenSeriesOnField;

    private bool hiddenAuthorsOnField;

    private bool hiddenLocationsOnField;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleSettingsView"/> class.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether the comments toggle is on.
    /// </summary>
    public bool CommentsOn { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chapters toggle is on.
    /// </summary>
    public bool ChaptersOn { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the favorites toggle is on.
    /// </summary>
    public bool FavoritesOn { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the ratings toggle is on.
    /// </summary>
    public bool RatingsOn { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the hidden books toggle is on.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether the hidden collections toggle is on.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether the hidden genres toggle is on.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether the hidden series toggle is on.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether the hidden authors toggle is on.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether the hidden locations toggle is on.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether the hidden wishlist books toggle is on.
    /// </summary>
    public bool HiddenWishlistBooksOn { get; set; }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnCommentsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("CommentsOn", e.Value);
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnChaptersToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("ChaptersOn", e.Value);
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnFavoritesToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("FavoritesOn", e.Value);
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnRatingsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set("RatingsOn", e.Value);
    }

    /// <summary>
    /// Saves the value of the toggle to preferences. Resets the view models and changes the toggles if the hidden books toggle is turned on.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
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

        ReadingViewModel.filteredBookList2 = null;
        ToBeReadViewModel.filteredBookList2 = null;
        ReadViewModel.filteredBookList2 = null;
        AllBooksViewModel.filteredBookList2 = null;

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

    /// <summary>
    /// Saves the value of the toggle to preferences. Resets the view model and changes the hidden books toggle if the hidden collections toggle is turned on.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnHiddenCollectionsToggled(object sender, ToggledEventArgs e)
    {
        CollectionsViewModel.RefreshView = true;

        CollectionsViewModel.filteredCollectionList2 = null;

        var variable = CollectionsViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = false;
        }

        Preferences.Set("HiddenCollectionsOn", e.Value);
    }

    /// <summary>
    /// Saves the value of the toggle to preferences. Resets the view model and changes the hidden books toggle if the hidden genres toggle is turned on.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnHiddenGenresToggled(object sender, ToggledEventArgs e)
    {
        GenresViewModel.RefreshView = true;

        GenresViewModel.filteredGenreList2 = null;

        var variable = GenresViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = e.Value;
        }

        Preferences.Set("HiddenGenresOn", e.Value);
    }

    /// <summary>
    /// Saves the value of the toggle to preferences. Resets the view model and changes the hidden books toggle if the hidden series toggle is turned on.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnHiddenSeriesToggled(object sender, ToggledEventArgs e)
    {
        SeriesViewModel.RefreshView = true;

        SeriesViewModel.filteredSeriesList2 = null;

        var variable = SeriesViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = e.Value;
        }

        Preferences.Set("HiddenSeriesOn", e.Value);
    }

    /// <summary>
    /// Saves the value of the toggle to preferences. Resets the view model and changes the hidden books toggle if the hidden authors toggle is turned on.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnHiddenAuthorsToggled(object sender, ToggledEventArgs e)
    {
        AuthorsViewModel.RefreshView = true;

        AuthorsViewModel.filteredAuthorList2 = null;

        var variable = AuthorsViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = e.Value;
        }

        Preferences.Set("HiddenAuthorsOn", e.Value);
    }

    /// <summary>
    /// Saves the value of the toggle to preferences. Resets the view model and changes the hidden books toggle if the hidden locations toggle is turned on.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnHiddenLocationsToggled(object sender, ToggledEventArgs e)
    {
        LocationsViewModel.RefreshView = true;

        LocationsViewModel.filteredLocationList2 = null;

        var variable = LocationsViewModel.HideBooks(e.Value);

        if (!e.Value)
        {
            this.HiddenBooksOn = e.Value;
        }

        Preferences.Set("HiddenLocationsOn", e.Value);
    }

    /// <summary>
    /// Saves the value of the toggle to preferences. Resets the view model.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnHiddenWishlistBooksToggled(object sender, ToggledEventArgs e)
    {
        WishListViewModel.RefreshView = true;

        WishListViewModel.filteredWishlistBookList2 = null;

        Preferences.Set("HiddenWishlistBooksOn", e.Value);
    }
}