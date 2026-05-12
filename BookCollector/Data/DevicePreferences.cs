// <copyright file="DevicePreferences.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    /// <summary>
    /// DevicePreferences class.
    /// </summary>
    public class DevicePreferences
    {
        /// <summary>
        /// Sets the value for the ReadingViewShow preference.
        /// </summary>
        public const string ReadingViewShow = "ReadingViewShow";

        /// <summary>
        /// Sets the value for the ToBeReadViewShow preference.
        /// </summary>
        public const string ToBeReadViewShow = "ToBeReadViewShow";

        /// <summary>
        /// Sets the value for the ReadViewShow preference.
        /// </summary>
        public const string ReadViewShow = "ReadViewShow";

        /// <summary>
        /// Sets the value for the AllBooksViewShow preference.
        /// </summary>
        public const string AllBooksViewShow = "AllBooksViewShow";

        /// <summary>
        /// Sets the value for the CollectionsViewShow preference.
        /// </summary>
        public const string CollectionsViewShow = "CollectionsViewShow";

        /// <summary>
        /// Sets the value for the GenresViewShow preference.
        /// </summary>
        public const string GenresViewShow = "GenresViewShow";

        /// <summary>
        /// Sets the value for the SeriesViewShow preference.
        /// </summary>
        public const string SeriesViewShow = "SeriesViewShow";

        /// <summary>
        /// Sets the value for the AuthorsViewShow preference.
        /// </summary>
        public const string AuthorsViewShow = "AuthorsViewShow";

        /// <summary>
        /// Sets the value for the LocationsViewShow preference.
        /// </summary>
        public const string LocationsViewShow = "LocationsViewShow";

        /********************************************************/

        /// <summary>
        /// Sets the value for the FavoriteFeatureShow preference.
        /// </summary>
        public const string FavoriteFeatureShow = "FavoritesOn";

        /// <summary>
        /// Sets the value for the RatingFeatureShow preference.
        /// </summary>
        public const string RatingFeatureShow = "RatingsOn";

        /// <summary>
        /// Sets the value for the CommentsFeatureShow preference.
        /// </summary>
        public const string CommentsFeatureShow = "CommentsOn";

        /// <summary>
        /// Sets the value for the ChaptersFeatureShow preference.
        /// </summary>
        public const string ChaptersFeatureShow = "ChaptersOn";

        /// <summary>
        /// Sets the value for the LoanedOutBooksShow preference.
        /// </summary>
        public const string LoanedOutBooksShow = "LoanedOutBooksOn";

        /// <summary>
        /// Sets the value for the BorrowedBooksShow preference.
        /// </summary>
        public const string BorrowedBooksShow = "BorrowedBooksOn";

        /********************************************************/

        /// <summary>
        /// Sets the value for the HiddenCollectionsShow preference.
        /// </summary>
        public const string HiddenCollectionsShow = "HiddenCollectionsOn";

        /// <summary>
        /// Sets the value for the HiddenGenresShow preference.
        /// </summary>
        public const string HiddenGenresShow = "HiddenGenresOn";

        /// <summary>
        /// Sets the value for the HiddenSeriesShow preference.
        /// </summary>
        public const string HiddenSeriesShow = "HiddenSeriesOn";

        /// <summary>
        /// Sets the value for the HiddenAuthorsShow preference.
        /// </summary>
        public const string HiddenAuthorsShow = "HiddenAuthorsOn";

        /// <summary>
        /// Sets the value for the HiddenLocationsShow preference.
        /// </summary>
        public const string HiddenLocationsShow = "HiddenLocationsOn";

        /// <summary>
        /// Sets the value for the HiddenBooksShow preference.
        /// </summary>
        public const string HiddenBooksShow = "HiddenBooksOn";

        /// <summary>
        /// Sets the value for the HiddenWishlistBooksShow preference.
        /// </summary>
        public const string HiddenWishlistBooksShow = "HiddenWishlistBooksOn";

        /********************************************************/

        /// <summary>
        /// Sets the value for the AudiobookShow preference.
        /// </summary>
        public const string AudiobookShow = "AudiobookOn";

        /// <summary>
        /// Sets the value for the eBookShow preference.
        /// </summary>
#pragma warning disable SA1303 // Const field names should begin with upper-case letter
        public const string eBookShow = "eBookOn";
#pragma warning restore SA1303 // Const field names should begin with upper-case letter

        /// <summary>
        /// Sets the value for the HardcoverShow preference.
        /// </summary>
        public const string HardcoverShow = "HardcoverOn";

        /// <summary>
        /// Sets the value for the PaperbackShow preference.
        /// </summary>
        public const string PaperbackShow = "PaperbackOn";

        /********************************************************/

        /// <summary>
        /// Sets the value for the AuthorFilterSelection preference.
        /// </summary>
        public const string AuthorFilterSelection = "AuthorSelection";

        /// <summary>
        /// Sets the value for the FavoriteFilterSelection preference.
        /// </summary>
        public const string FavoriteFilterSelection = "FavoriteSelection";

        /// <summary>
        /// Sets the value for the FormatFilterSelection preference.
        /// </summary>
        public const string FormatFilterSelection = "FormatSelection";

        /// <summary>
        /// Sets the value for the PublisherFilterSelection preference.
        /// </summary>
        public const string PublisherFilterSelection = "PublisherSelection";

        /// <summary>
        /// Sets the value for the PublishYearFilterSelection preference.
        /// </summary>
        public const string PublishYearFilterSelection = "PublishYearSelection";

        /// <summary>
        /// Sets the value for the LanguageFilterSelection preference.
        /// </summary>
        public const string LanguageFilterSelection = "LanguageSelection";

        /// <summary>
        /// Sets the value for the RatingFilterSelection preference.
        /// </summary>
        public const string RatingFilterSelection = "RatingSelection";

        /// <summary>
        /// Sets the value for the BookCoverFilterSelection preference.
        /// </summary>
        public const string BookCoverFilterSelection = "BookCoverSelection";

        /// <summary>
        /// Sets the value for the ReadingStatusFilterSelection preference.
        /// </summary>
        public const string ReadingStatusFilterSelection = "ReadingStatusSelection";

        /// <summary>
        /// Sets the value for the LoanedOutBooksFilterSelection preference.
        /// </summary>
        public const string LoanedOutBooksFilterSelection = "LoanedOutBooksSelection";

        /// <summary>
        /// Sets the value for the BorrowedBooksFilterSelection preference.
        /// </summary>
        public const string BorrowedBooksFilterSelection = "BorrowedBooksSelection";

        /// <summary>
        /// Sets the value for the SeriesFilterSelection preference.
        /// </summary>
        public const string SeriesFilterSelection = "SeriesSelection";

        /// <summary>
        /// Sets the value for the LocationFilterSelection preference.
        /// </summary>
        public const string LocationFilterSelection = "LocationSelection";

        /********************************************************/

        /// <summary>
        /// Sets the value for the BookTitleSortSelection preference.
        /// </summary>
        public const string BookTitleSortSelection = "BookTitleSelection";

        /// <summary>
        /// Sets the value for the BookReadingDateSortSelection preference.
        /// </summary>
        public const string BookReadingDateSortSelection = "BookReadingDateSelection";

        /// <summary>
        /// Sets the value for the BookReadPercentageSortSelection preference.
        /// </summary>
        public const string BookReadPercentageSortSelection = "BookReadPercentageSelection";

        /// <summary>
        /// Sets the value for the BookPublisherSortSelection preference.
        /// </summary>
        public const string BookPublisherSortSelection = "BookPublisherSelection";

        /// <summary>
        /// Sets the value for the BookPublishYearSortSelection preference.
        /// </summary>
        public const string BookPublishYearSortSelection = "BookPublishYearSelection";

        /// <summary>
        /// Sets the value for the AuthorLastNameSortSelection preference.
        /// </summary>
        public const string AuthorLastNameSortSelection = "AuthorLastNameSelection";

        /// <summary>
        /// Sets the value for the BookFormatSortSelection preference.
        /// </summary>
        public const string BookFormatSortSelection = "BookFormatSelection";

        /// <summary>
        /// Sets the value for the PageCountBookTimeSortSelection preference.
        /// </summary>
        public const string PageCountBookTimeSortSelection = "PageCountBookTimeSelection";

        /// <summary>
        /// Sets the value for the BookPriceSortSelection preference.
        /// </summary>
        public const string BookPriceSortSelection = "BookPriceSelection";

        /// <summary>
        /// Sets the value for the TotalBooksSortSelection preference.
        /// </summary>
        public const string TotalBooksSortSelection = "TotalBooksSelection";

        /// <summary>
        /// Sets the value for the TotalPriceSortSelection preference.
        /// </summary>
        public const string TotalPriceSortSelection = "TotalPriceSelection";

        /// <summary>
        /// Sets the value for the LocationNameSortSelection preference.
        /// </summary>
        public const string LocationNameSortSelection = "LocationNameSelection";

        /// <summary>
        /// Sets the value for the SeriesNameSortSelection preference.
        /// </summary>
        public const string SeriesNameSortSelection = "SeriesNameSelection";

        /// <summary>
        /// Sets the value for the GenreNameSortSelection preference.
        /// </summary>
        public const string GenreNameSortSelection = "GenreNameSelection";

        /// <summary>
        /// Sets the value for the CollectionNameSortSelection preference.
        /// </summary>
        public const string CollectionNameSortSelection = "CollectionNameSelection";

        /// <summary>
        /// Sets the value for the SeriesOrderSortSelection preference.
        /// </summary>
        public const string SeriesOrderSortSelection = "SeriesOrderSelection";

        /// <summary>
        /// Sets the value for the AscendingSortSelection preference.
        /// </summary>
        public const string AscendingSortSelection = "AscendingSelection";

        /// <summary>
        /// Sets the value for the DescendingSortSelection preference.
        /// </summary>
        public const string DescendingSortSelection = "DescendingSelection";

        /********************************************************/

        /// <summary>
        /// Sets the value for the LibraryTabViewsOrder preference.
        /// </summary>
        public const string LibraryTabViewsOrder = "LibraryTabViewsOrder";

        /// <summary>
        /// Sets the value for the GroupingsTabViewsOrder preference.
        /// </summary>
        public const string GroupingsTabViewsOrder = "GroupingsTabViewsOrder";

        /********************************************************/

        /// <summary>
        /// Sets the value for the AppTheme preference.
        /// </summary>
        public const string AppTheme = "AppTheme";

        /// <summary>
        /// Sets the value for the AppColor preference.
        /// </summary>
        public const string AppColor = "AppColor";

        /// <summary>
        /// Sets the value for the AppLanguage preference.
        /// </summary>
        public const string AppLanguage = "Language";

        /// <summary>
        /// Sets the value for the AppCurrency preference.
        /// </summary>
        public const string AppCurrency = "Currency";

        /// <summary>
        /// Sets the value for the AppExportLocation preference.
        /// </summary>
        public const string AppExportLocation = "ExportLocation";

        /// <summary>
        /// Sets the value for the AppCultureCode preference.
        /// </summary>
        public const string AppCultureCode = "CultureCode";

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden collections or not.
        /// </summary>
        public static bool ShowHiddenCollectionsValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden genres or not.
        /// </summary>
        public static bool ShowHiddenGenresValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden series or not.
        /// </summary>
        public static bool ShowHiddenSeriesValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden authors or not.
        /// </summary>
        public static bool ShowHiddenAuthorsValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden locations or not.
        /// </summary>
        public static bool ShowHiddenLocationsValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden books or not.
        /// </summary>
        public static bool ShowHiddenBooksValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden wishlist books or not.
        /// </summary>
        public static bool ShowHiddenWishlistBooksValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show audiobooks or not.
        /// </summary>
        public static bool ShowAudiobooksValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show eBooks or not.
        /// </summary>
        public static bool ShoweBooksValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hardcovers or not.
        /// </summary>
        public static bool ShowHardcoversValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show paperbacks or not.
        /// </summary>
        public static bool ShowPaperbacksValue { get; set; }

        /// <summary>
        /// Gets or sets the value for the AppTheme.
        /// </summary>
        public static string AppThemeValue { get; set; }

        /// <summary>
        /// Gets or sets the value for the AppColor.
        /// </summary>
        public static string AppColorValue { get; set; }

        /// <summary>
        /// Gets or sets the value for the AppLanguage.
        /// </summary>
        public static string AppLanguageValue { get; set; }

        /// <summary>
        /// Gets or sets the value for the AppCurrency.
        /// </summary>
        public static string AppCurrencyValue { get; set; }

        /// <summary>
        /// Gets or sets the value for the AppExportLocation.
        /// </summary>
        public static string AppExportLocationValue { get; set; }

        /// <summary>
        /// Gets or sets the value for the AppCultureCode.
        /// </summary>
        public static string AppCultureCodeValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show comments or not.
        /// </summary>
        public static bool CommentsShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show chapters or not.
        /// </summary>
        public static bool ChaptersShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show favorites or not.
        /// </summary>
        public static bool FavoritesShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show ratings or not.
        /// </summary>
        public static bool RatingsShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show loaned out books or not.
        /// </summary>
        public static bool LoanedOutBooksShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show borrowed books or not.
        /// </summary>
        public static bool BorrowedBooksShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Reading view or not.
        /// </summary>
        public static bool ReadingViewShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the To Be Read view or not.
        /// </summary>
        public static bool ToBeReadViewShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Read view or not.
        /// </summary>
        public static bool ReadViewShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the All Books view or not.
        /// </summary>
        public static bool AllBooksViewShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Collections view or not.
        /// </summary>
        public static bool CollectionsViewShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Genres view or not.
        /// </summary>
        public static bool GenresViewShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Series view or not.
        /// </summary>
        public static bool SeriesViewShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Authors view or not.
        /// </summary>
        public static bool AuthorsViewShowValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Locations view or not.
        /// </summary>
        public static bool LocationsViewShowValue { get; set; }

        /// <summary>
        /// Gets or sets the value for the LibraryTabViewsOrder.
        /// </summary>
        public static string LibraryTabViewsOrderValue { get; set; }

        /// <summary>
        /// Gets or sets the value for the GroupingsTabViewOrder.
        /// </summary>
        public static string GroupingsTabViewOrderValue { get; set; }
    }
}
