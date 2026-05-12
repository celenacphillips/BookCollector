// <copyright file="ToggleSettingsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Settings;

using BookCollector.Data;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Groupings;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Main;

/// <summary>
/// ToggleSettingsView class.
/// </summary>
public partial class ToggleSettingsView : ContentPage
{
    private bool commentsOnField;

    private bool chaptersOnField;

    private bool favoritesOnField;

    private bool ratingsOnField;

    private bool loanOutBooksField;

    private bool borrowBooksField;

    private bool hiddenBooksOnField;

    private bool hiddenCollectionsOnField;

    private bool hiddenGenresOnField;

    private bool hiddenSeriesOnField;

    private bool hiddenAuthorsOnField;

    private bool hiddenLocationsOnField;

    private bool hiddenWishlistBooksOnField;

    private bool audiobookOnField;

    private bool eBookOnField;

    private bool hardcoverOnField;

    private bool paperbackOnField;

    /********************************************************/

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleSettingsView"/> class.
    /// </summary>
    public ToggleSettingsView()
    {
        this.CommentsOn = DevicePreferences.CommentsShowValue;
        this.ChaptersOn = DevicePreferences.ChaptersShowValue;
        this.FavoritesOn = DevicePreferences.FavoritesShowValue;
        this.RatingsOn = DevicePreferences.RatingsShowValue;
        this.LoanOutBooks = DevicePreferences.LoanedOutBooksShowValue;
        this.BorrowBooks = DevicePreferences.BorrowedBooksShowValue;

        this.HiddenBooksOn = DevicePreferences.ShowHiddenBooksValue;
        this.HiddenCollectionsOn = DevicePreferences.ShowHiddenCollectionsValue;
        this.HiddenGenresOn = DevicePreferences.ShowHiddenGenresValue;
        this.HiddenSeriesOn = DevicePreferences.ShowHiddenSeriesValue;
        this.HiddenAuthorsOn = DevicePreferences.ShowHiddenAuthorsValue;
        this.HiddenLocationsOn = DevicePreferences.ShowHiddenLocationsValue;
        this.HiddenWishlistBooksOn = DevicePreferences.ShowHiddenWishlistBooksValue;

        this.AudiobookOn = DevicePreferences.ShowAudiobooksValue;
        this.eBookOn = DevicePreferences.ShoweBooksValue;
        this.HardcoverOn = DevicePreferences.ShowHardcoversValue;
        this.PaperbackOn = DevicePreferences.ShowPaperbacksValue;

        this.InitializeComponent();
        this.BindingContext = this;

        this.loanOutBooksAnswer.Text = this.LoanOutBooks ? AppStringResources.Yes : AppStringResources.No;
        this.borrowBooksAnswer.Text = this.BorrowBooks ? AppStringResources.Yes : AppStringResources.No;
    }

    /********************************************************/

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
    /// Gets or sets a value indicating whether the loan out books toggle is on.
    /// </summary>
    public bool LoanOutBooks
    {
        get => this.loanOutBooksField;
        set
        {
            if (this.loanOutBooksField != value)
            {
                this.loanOutBooksField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the borrow books toggle is on.
    /// </summary>
    public bool BorrowBooks
    {
        get => this.borrowBooksField;
        set
        {
            if (this.borrowBooksField != value)
            {
                this.borrowBooksField = value;
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
    /// Gets or sets a value indicating whether the Audiobook toggle is on.
    /// </summary>
    public bool AudiobookOn
    {
        get => this.audiobookOnField;
        set
        {
            if (this.audiobookOnField != value)
            {
                this.audiobookOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the eBook toggle is on.
    /// </summary>
    public bool eBookOn
    {
        get => this.eBookOnField;
        set
        {
            if (this.eBookOnField != value)
            {
                this.eBookOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Hardcover toggle is on.
    /// </summary>
    public bool HardcoverOn
    {
        get => this.hardcoverOnField;
        set
        {
            if (this.hardcoverOnField != value)
            {
                this.hardcoverOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the Paperback toggle is on.
    /// </summary>
    public bool PaperbackOn
    {
        get => this.paperbackOnField;
        set
        {
            if (this.paperbackOnField != value)
            {
                this.paperbackOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /********************************************************/

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnCommentsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set(DevicePreferences.CommentsFeatureShow, e.Value);
        DevicePreferences.CommentsShowValue = e.Value;
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnChaptersToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set(DevicePreferences.ChaptersFeatureShow, e.Value);
        DevicePreferences.ChaptersShowValue = e.Value;
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnFavoritesToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set(DevicePreferences.FavoriteFeatureShow, e.Value);
        DevicePreferences.FavoritesShowValue = e.Value;
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnRatingsToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set(DevicePreferences.RatingFeatureShow, e.Value);
        DevicePreferences.RatingsShowValue = e.Value;
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnLoanOutBooksToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set(DevicePreferences.LoanedOutBooksShow, e.Value);

        this.loanOutBooksAnswer.Text = e.Value ? AppStringResources.Yes : AppStringResources.No;

        var appshell = (AppShell)Application.Current?.Windows[0].Page!;
        appshell.ShowBooksLoanedOut = e.Value;
        DevicePreferences.LoanedOutBooksShowValue = e.Value;
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnBorrowBooksToggled(object sender, ToggledEventArgs e)
    {
        Preferences.Set(DevicePreferences.BorrowedBooksShow, e.Value);

        this.borrowBooksAnswer.Text = e.Value ? AppStringResources.Yes : AppStringResources.No;

        var appshell = (AppShell)Application.Current?.Windows[0].Page!;
        appshell.ShowBorrowedBooks = e.Value;
        DevicePreferences.BorrowedBooksShowValue = e.Value;
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

        Preferences.Set(DevicePreferences.HiddenBooksShow, e.Value);
        DevicePreferences.ShowHiddenBooksValue = e.Value;
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

        Preferences.Set(DevicePreferences.HiddenCollectionsShow, e.Value);
        DevicePreferences.ShowHiddenCollectionsValue = e.Value;
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

        Preferences.Set(DevicePreferences.HiddenGenresShow, e.Value);
        DevicePreferences.ShowHiddenGenresValue = e.Value;
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

        Preferences.Set(DevicePreferences.HiddenSeriesShow, e.Value);
        DevicePreferences.ShowHiddenSeriesValue = e.Value;
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

        var variable = AuthorsViewModel.HideBooks(e.Value, this.AudiobookOn, this.eBookOn, this.HardcoverOn, this.PaperbackOn);

        if (!e.Value)
        {
            this.HiddenBooksOn = e.Value;
        }

        Preferences.Set(DevicePreferences.HiddenAuthorsShow, e.Value);
        DevicePreferences.ShowHiddenAuthorsValue = e.Value;
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

        Preferences.Set(DevicePreferences.HiddenLocationsShow, e.Value);
        DevicePreferences.ShowHiddenLocationsValue = e.Value;
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

        Preferences.Set(DevicePreferences.HiddenWishlistBooksShow, e.Value);
        DevicePreferences.ShowHiddenWishlistBooksValue = e.Value;
    }

    /// <summary>
    /// Resets the toggles values.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnBookSettingsResetButton_Clicked(object sender, EventArgs e)
    {
        this.CommentsOn = DevicePreferenceDefaults.CommentsShowDefault;
        Preferences.Set(DevicePreferences.CommentsFeatureShow, DevicePreferenceDefaults.CommentsShowDefault);
        DevicePreferences.CommentsShowValue = DevicePreferenceDefaults.CommentsShowDefault;

        this.ChaptersOn = DevicePreferenceDefaults.ChaptersShowDefault;
        Preferences.Set(DevicePreferences.ChaptersFeatureShow, DevicePreferenceDefaults.ChaptersShowDefault);
        DevicePreferences.ChaptersShowValue = DevicePreferenceDefaults.ChaptersShowDefault;

        this.FavoritesOn = DevicePreferenceDefaults.FavoritesShowDefault;
        Preferences.Set(DevicePreferences.FavoriteFeatureShow, DevicePreferenceDefaults.FavoritesShowDefault);
        DevicePreferences.FavoritesShowValue = DevicePreferenceDefaults.FavoritesShowDefault;

        this.RatingsOn = DevicePreferenceDefaults.RatingsShowDefault;
        Preferences.Set(DevicePreferences.RatingFeatureShow, DevicePreferenceDefaults.RatingsShowDefault);
        DevicePreferences.RatingsShowValue = DevicePreferenceDefaults.RatingsShowDefault;
    }

    /// <summary>
    /// Resets the toggles values.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnListSettingsResetButton_Clicked(object sender, EventArgs e)
    {
        this.HiddenBooksOn = DevicePreferenceDefaults.HiddenBookShowDefault;
        Preferences.Set(DevicePreferences.HiddenBooksShow, DevicePreferenceDefaults.HiddenBookShowDefault);
        DevicePreferences.ShowHiddenBooksValue = DevicePreferenceDefaults.HiddenBookShowDefault;

        this.HiddenCollectionsOn = DevicePreferenceDefaults.HiddenCollectionShowDefault;
        Preferences.Set(DevicePreferences.HiddenCollectionsShow, DevicePreferenceDefaults.HiddenCollectionShowDefault);
        DevicePreferences.ShowHiddenCollectionsValue = DevicePreferenceDefaults.HiddenCollectionShowDefault;

        this.HiddenGenresOn = DevicePreferenceDefaults.HiddenGenresShowDefault;
        Preferences.Set(DevicePreferences.HiddenGenresShow, DevicePreferenceDefaults.HiddenGenresShowDefault);
        DevicePreferences.ShowHiddenGenresValue = DevicePreferenceDefaults.HiddenGenresShowDefault;

        this.HiddenSeriesOn = DevicePreferenceDefaults.HiddenSeriesShowDefault;
        Preferences.Set(DevicePreferences.HiddenSeriesShow, DevicePreferenceDefaults.HiddenSeriesShowDefault);
        DevicePreferences.ShowHiddenSeriesValue = DevicePreferenceDefaults.HiddenSeriesShowDefault;

        this.HiddenAuthorsOn = DevicePreferenceDefaults.HiddenAuthorsShowDefault;
        Preferences.Set(DevicePreferences.HiddenAuthorsShow, DevicePreferenceDefaults.HiddenAuthorsShowDefault);
        DevicePreferences.ShowHiddenAuthorsValue = DevicePreferenceDefaults.HiddenAuthorsShowDefault;

        this.HiddenLocationsOn = DevicePreferenceDefaults.HiddenLocationsShowDefault;
        Preferences.Set(DevicePreferences.HiddenLocationsShow, DevicePreferenceDefaults.HiddenLocationsShowDefault);
        DevicePreferences.ShowHiddenLocationsValue = DevicePreferenceDefaults.HiddenLocationsShowDefault;

        this.HiddenWishlistBooksOn = DevicePreferenceDefaults.HiddenWishlistBooksShowDefault;
        Preferences.Set(DevicePreferences.HiddenWishlistBooksShow, DevicePreferenceDefaults.HiddenWishlistBooksShowDefault);
        DevicePreferences.ShowHiddenWishlistBooksValue = DevicePreferenceDefaults.HiddenWishlistBooksShowDefault;

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

    /// <summary>
    /// Resets the toggles values.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnBookFormatSettingsResetButton_Clicked(object sender, EventArgs e)
    {
        this.AudiobookOn = DevicePreferenceDefaults.AudiobookShowDefault;
        Preferences.Set(DevicePreferences.AudiobookShow, DevicePreferenceDefaults.AudiobookShowDefault);
        DevicePreferences.ShowAudiobooksValue = DevicePreferenceDefaults.AudiobookShowDefault;

        this.eBookOn = DevicePreferenceDefaults.EbookShowDefault;
        Preferences.Set(DevicePreferences.eBookShow, DevicePreferenceDefaults.EbookShowDefault);
        DevicePreferences.ShoweBooksValue = DevicePreferenceDefaults.EbookShowDefault;

        this.HardcoverOn = DevicePreferenceDefaults.HardcoverShowDefault;
        Preferences.Set(DevicePreferences.HardcoverShow, DevicePreferenceDefaults.HardcoverShowDefault);
        DevicePreferences.ShowHardcoversValue = DevicePreferenceDefaults.HardcoverShowDefault;

        this.PaperbackOn = DevicePreferenceDefaults.PaperbackShowDefault;
        Preferences.Set(DevicePreferences.PaperbackShow, DevicePreferenceDefaults.PaperbackShowDefault);
        DevicePreferences.ShowPaperbacksValue = DevicePreferenceDefaults.PaperbackShowDefault;
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnAudiobookToggled(object sender, ToggledEventArgs e)
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
        WishListViewModel.RefreshView = true;

        ReadingViewModel.filteredBookList = null;
        ToBeReadViewModel.filteredBookList = null;
        ReadViewModel.filteredBookList = null;
        AllBooksViewModel.filteredBookList = null;
        WishListViewModel.filteredWishlistBookList = null;

        Preferences.Set(DevicePreferences.AudiobookShow, e.Value);
        DevicePreferences.ShowAudiobooksValue = e.Value;
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnEBookToggled(object sender, ToggledEventArgs e)
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
        WishListViewModel.RefreshView = true;

        ReadingViewModel.filteredBookList = null;
        ToBeReadViewModel.filteredBookList = null;
        ReadViewModel.filteredBookList = null;
        AllBooksViewModel.filteredBookList = null;
        WishListViewModel.filteredWishlistBookList = null;

        Preferences.Set(DevicePreferences.eBookShow, e.Value);
        DevicePreferences.ShoweBooksValue = e.Value;
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnHardcoverToggled(object sender, ToggledEventArgs e)
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
        WishListViewModel.RefreshView = true;

        ReadingViewModel.filteredBookList = null;
        ToBeReadViewModel.filteredBookList = null;
        ReadViewModel.filteredBookList = null;
        AllBooksViewModel.filteredBookList = null;
        WishListViewModel.filteredWishlistBookList = null;

        Preferences.Set(DevicePreferences.HardcoverShow, e.Value);
        DevicePreferences.ShowHardcoversValue = e.Value;
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnPaperbackToggled(object sender, ToggledEventArgs e)
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
        WishListViewModel.RefreshView = true;

        ReadingViewModel.filteredBookList = null;
        ToBeReadViewModel.filteredBookList = null;
        ReadViewModel.filteredBookList = null;
        AllBooksViewModel.filteredBookList = null;
        WishListViewModel.filteredWishlistBookList = null;

        Preferences.Set(DevicePreferences.PaperbackShow, e.Value);
        DevicePreferences.ShowPaperbacksValue = e.Value;
    }
}