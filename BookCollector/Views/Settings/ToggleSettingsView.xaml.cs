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
    private readonly bool commentsDefault = true;

    private readonly bool chaptersDefault = true;

    private readonly bool favoritesDefault = true;

    private readonly bool ratingsDefault = true;

    private readonly bool hiddenDefault = true;

    private bool commentsOnField;

    private bool chaptersOnField;

    private bool favoritesOnField;

    private bool ratingsOnField;

    private bool hiddenBooksOnField;

    private bool hiddenCollectionsOnField;

    private bool hiddenGenresOnField;

    private bool hiddenSeriesOnField;

    private bool hiddenAuthorsOnField;

    private bool hiddenLocationsOnField;

    private bool hiddenWishlistBooksOnField;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleSettingsView"/> class.
    /// </summary>
    public ToggleSettingsView()
    {
        this.CommentsOn = Preferences.Get("CommentsOn", this.commentsDefault /* Default */);
        this.ChaptersOn = Preferences.Get("ChaptersOn", this.chaptersDefault /* Default */);
        this.FavoritesOn = Preferences.Get("FavoritesOn", this.favoritesDefault /* Default */);
        this.RatingsOn = Preferences.Get("RatingsOn", this.ratingsDefault /* Default */);

        this.HiddenBooksOn = Preferences.Get("HiddenBooksOn", this.hiddenDefault /* Default */);
        this.HiddenCollectionsOn = Preferences.Get("HiddenCollectionsOn", this.hiddenDefault /* Default */);
        this.HiddenGenresOn = Preferences.Get("HiddenGenresOn", this.hiddenDefault /* Default */);
        this.HiddenSeriesOn = Preferences.Get("HiddenSeriesOn", this.hiddenDefault /* Default */);
        this.HiddenAuthorsOn = Preferences.Get("HiddenAuthorsOn", this.hiddenDefault /* Default */);
        this.HiddenLocationsOn = Preferences.Get("HiddenLocationsOn", this.hiddenDefault /* Default */);
        this.HiddenWishlistBooksOn = Preferences.Get("HiddenWishlistBooksOn", this.hiddenDefault /* Default */);

        this.InitializeComponent();
        this.BindingContext = this;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the comments toggle is on.
    /// </summary>
    public bool CommentsOn
    {
        get => this.commentsOnField;
        set
        {
            if (this.commentsOnField != value)
            {
                this.commentsOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the chapters toggle is on.
    /// </summary>
    public bool ChaptersOn
    {
        get => this.chaptersOnField;
        set
        {
            if (this.chaptersOnField != value)
            {
                this.chaptersOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the favorites toggle is on.
    /// </summary>
    public bool FavoritesOn
    {
        get => this.favoritesOnField;
        set
        {
            if (this.favoritesOnField != value)
            {
                this.favoritesOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the ratings toggle is on.
    /// </summary>
    public bool RatingsOn
    {
        get => this.ratingsOnField;
        set
        {
            if (this.ratingsOnField != value)
            {
                this.ratingsOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

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
    public bool HiddenWishlistBooksOn
    {
        get => this.hiddenWishlistBooksOnField;
        set
        {
            if (this.hiddenWishlistBooksOnField != value)
            {
                this.hiddenWishlistBooksOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

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

        ReadingViewModel.filteredBookList = null;
        ToBeReadViewModel.filteredBookList = null;
        ReadViewModel.filteredBookList = null;
        AllBooksViewModel.filteredBookList = null;

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

        CollectionsViewModel.filteredCollectionList = null;

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

        GenresViewModel.filteredGenreList = null;

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

        SeriesViewModel.filteredSeriesList = null;

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

        AuthorsViewModel.filteredAuthorList = null;

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

        LocationsViewModel.filteredLocationList = null;

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

        WishListViewModel.filteredWishlistBookList = null;

        Preferences.Set("HiddenWishlistBooksOn", e.Value);
    }

    private void OnResetButton_Clicked(object sender, EventArgs e)
    {
        this.CommentsOn = this.commentsDefault;
        Preferences.Set("CommentsOn", this.commentsDefault);
        this.ChaptersOn = this.chaptersDefault;
        Preferences.Set("ChaptersOn", this.chaptersDefault);
        this.FavoritesOn = this.favoritesDefault;
        Preferences.Set("FavoritesOn", this.favoritesDefault);
        this.RatingsOn = this.favoritesDefault;
        Preferences.Set("RatingsOn", this.favoritesDefault);

        this.HiddenBooksOn = this.hiddenDefault;
        Preferences.Set("HiddenBooksOn", this.hiddenDefault);
        this.HiddenCollectionsOn = this.hiddenDefault;
        Preferences.Set("HiddenCollectionsOn", this.hiddenDefault);
        this.HiddenGenresOn = this.hiddenDefault;
        Preferences.Set("HiddenGenresOn", this.hiddenDefault);
        this.HiddenSeriesOn = this.hiddenDefault;
        Preferences.Set("HiddenSeriesOn", this.hiddenDefault);
        this.HiddenAuthorsOn = this.hiddenDefault;
        Preferences.Set("HiddenAuthorsOn", this.hiddenDefault);
        this.HiddenLocationsOn = this.hiddenDefault;
        Preferences.Set("HiddenLocationsOn", this.hiddenDefault);
        this.HiddenWishlistBooksOn = this.hiddenDefault;
        Preferences.Set("HiddenWishlistBooksOn", this.hiddenDefault);

        ReadingViewModel.RefreshView = true;
        ToBeReadViewModel.RefreshView = true;
        ReadViewModel.RefreshView = true;
        AllBooksViewModel.RefreshView = true;
        CollectionsViewModel.RefreshView = true;
        GenresViewModel.RefreshView = true;
        SeriesViewModel.RefreshView = true;
        AuthorsViewModel.RefreshView = true;
        LocationsViewModel.RefreshView = true;
        WishListViewModel.RefreshView = true;

        ReadingViewModel.filteredBookList = null;
        ToBeReadViewModel.filteredBookList = null;
        ReadViewModel.filteredBookList = null;
        AllBooksViewModel.filteredBookList = null;
        CollectionsViewModel.filteredCollectionList = null;
        GenresViewModel.filteredGenreList = null;
        SeriesViewModel.filteredSeriesList = null;
        AuthorsViewModel.filteredAuthorList = null;
        LocationsViewModel.filteredLocationList = null;
        WishListViewModel.filteredWishlistBookList = null;
    }
}