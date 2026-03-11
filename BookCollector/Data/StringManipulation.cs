// <copyright file="StringManipulation.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using BookCollector.Resources.Localization;

    /// <summary>
    /// StringManipulation class.
    /// </summary>
    public class StringManipulation
    {
        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="filteredCount">Filtered count of books.</param>
        /// <param name="totalCount">Total count of books.</param>
        /// <returns>Parsed string.</returns>
        public static string SetTotalBooksString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Book.ToLower() : AppStringResources.Books.ToLower());
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="filteredCount">Filtered count of books.</param>
        /// <param name="totalCount">Total count of books.</param>
        /// <param name="unreadCount">Unread count of books.</param>
        /// <returns>Parsed string.</returns>
        public static string SetTotalBooksString(int filteredCount, int totalCount, int unreadCount)
        {
            return AppStringResources.Blank1OfBlank2ItemsBlank3Unread.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("Blank3", $"{unreadCount}").Replace("items", totalCount == 1 ? AppStringResources.Book.ToLower() : AppStringResources.Books.ToLower());
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="totalCount">Total count of books.</param>
        /// <param name="unreadCount">Unread count of books.</param>
        /// <returns>Parsed string.</returns>
        public static string SetTotalBooksAndUnreadString(int totalCount, int unreadCount)
        {
            return AppStringResources.Blank1ItemsBlank2Unread.Replace("Blank1", $"{totalCount}").Replace("Blank2", $"{unreadCount}").Replace("items", totalCount == 1 ? AppStringResources.Book.ToLower() : AppStringResources.Books.ToLower());
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="filteredCount">Filtered count of collections.</param>
        /// <param name="totalCount">Total count of collections.</param>
        /// <returns>Parsed string.</returns>
        public static string SetTotalCollectionsString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Collection.ToLower() : AppStringResources.Collections.ToLower());
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="filteredCount">Filtered count of genres.</param>
        /// <param name="totalCount">Total count of genres.</param>
        /// <returns>Parsed string.</returns>
        public static string SetTotalGenresString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Genre.ToLower() : AppStringResources.Genres.ToLower());
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="filteredCount">Filtered count of series.</param>
        /// <param name="totalCount">Total count of series.</param>
        /// <returns>Parsed string.</returns>
        public static string SetTotalSeriesString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Series.ToLower() : AppStringResources.Series.ToLower());
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="filteredCount">Filtered count of authors.</param>
        /// <param name="totalCount">Total count of authors.</param>
        /// <returns>Parsed string.</returns>
        public static string SetTotalAuthorsString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Author.ToLower() : AppStringResources.Authors.ToLower());
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="filteredCount">Filtered count of locations.</param>
        /// <param name="totalCount">Total count of locations.</param>
        /// <returns>Parsed string.</returns>
        public static string SetTotalLocationsString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Location.ToLower() : AppStringResources.Locations.ToLower());
        }
    }
}
