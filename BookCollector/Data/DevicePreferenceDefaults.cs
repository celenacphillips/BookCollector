// <copyright file="DevicePreferenceDefaults.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using BookCollector.Resources.Localization;

    /// <summary>
    /// DevicePreferenceDefaults class.
    /// </summary>
    public class DevicePreferenceDefaults
    {
        /********************************************************/

        /// <summary>
        /// Sets the default value for the showing the Reading View.
        /// </summary>
        public const bool ReadingViewShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the To Be Read View.
        /// </summary>
        public const bool ToBeReadViewShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the Read View.
        /// </summary>
        public const bool ReadViewShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the All Books View.
        /// </summary>
        public const bool AllBooksViewShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the Collections View.
        /// </summary>
        public const bool CollectionsViewShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the Genres View.
        /// </summary>
        public const bool GenresViewShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the Series View.
        /// </summary>
        public const bool SeriesViewShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the Authors View.
        /// </summary>
        public const bool AuthorsViewShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the Locations View.
        /// </summary>
        public const bool LocationsViewShowDefault = true;

        /********************************************************/

        /// <summary>
        /// Sets the default value for the showing hidden collections.
        /// </summary>
        public const bool HiddenCollectionShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing hidden genres.
        /// </summary>
        public const bool HiddenGenresShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing hidden series.
        /// </summary>
        public const bool HiddenSeriesShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing hidden authors.
        /// </summary>
        public const bool HiddenAuthorsShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing hidden locations.
        /// </summary>
        public const bool HiddenLocationsShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing hidden books.
        /// </summary>
        public const bool HiddenBookShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing hidden wishlist books.
        /// </summary>
        public const bool HiddenWishlistBooksShowDefault = true;

        /********************************************************/

        /// <summary>
        /// Sets the default value for the showing audiobooks.
        /// </summary>
        public const bool AudiobookShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing eBooks.
        /// </summary>
        public const bool EbookShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing hardcovers.
        /// </summary>
        public const bool HardcoverShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing paperbacks.
        /// </summary>
        public const bool PaperbackShowDefault = true;

        /********************************************************/

        /// <summary>
        /// Sets the default value for the showing the favorites feature.
        /// </summary>
        public const bool FavoritesShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the ratings feature.
        /// </summary>
        public const bool RatingsShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the chapters feature.
        /// </summary>
        public const bool ChaptersShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the comments feature.
        /// </summary>
        public const bool CommentsShowDefault = true;

        /********************************************************/

        /// <summary>
        /// Sets the default value for the showing the Loaned Out Books features.
        /// </summary>
        public const bool LoanedOutBooksShowDefault = true;

        /// <summary>
        /// Sets the default value for the showing the Borrowed Books features.
        /// </summary>
        public const bool BorrowedBooksShowDefault = true;

        /********************************************************/

        /// <summary>
        /// Sets the default value for the App Theme.
        /// </summary>
        public const string AppThemeDefault = "System";

        /// <summary>
        /// Sets the default value for the App Color.
        /// </summary>
        public const string AppColorDefault = "#336699";

        /// <summary>
        /// Sets the default value for the App Currency.
        /// </summary>
        public const string AppCurrencyDefault = "$ USD";

        /// <summary>
        /// Sets the default value for the Culture Code.
        /// </summary>
        public const string CultureCodeDefault = "en-US";

        /********************************************************/

        /// <summary>
        /// Sets the default value for the App Language.
        /// </summary>
        public static readonly string AppLanguageDefault = AppStringResources.English;

        /// <summary>
        /// Sets the default order for the Library tab views.
        /// </summary>
        public static readonly string LibraryTabViewsOrderDefault = $"{AppStringResources.Reading},{AppStringResources.ToBeRead},{AppStringResources.Read},{AppStringResources.AllBooks}";

        /// <summary>
        /// Sets the default order for the Groupings tab views.
        /// </summary>
        public static readonly string GroupingsTabViewsOrderDefault = $"{AppStringResources.Collections},{AppStringResources.Genres},{AppStringResources.Series},{AppStringResources.Authors},{AppStringResources.Locations}";

        /********************************************************/
    }
}
