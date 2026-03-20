// <copyright file="StringManipulation.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using System.Globalization;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;

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

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="publisher">Book publisher.</param>
        /// <param name="date">Book publish year.</param>
        /// <returns>Parsed string.</returns>
        public static string SetPublisherPublishDateString(string? publisher, string? date)
        {
            return $"{(!string.IsNullOrEmpty(publisher) ? publisher : AppStringResources.NoPublisher)}, {(!string.IsNullOrEmpty(date) ? date : AppStringResources.NoDate)}";
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="inputName">Input name.</param>
        /// <returns>Parsed string.</returns>
        public static string? SetParsedName(string? inputName)
        {
            return (!string.IsNullOrEmpty(inputName) &&
                    (inputName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    inputName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    inputName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? inputName[(inputName.IndexOf(' ') + 1) ..]
                        : inputName;
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="format">Book format.</param>
        /// <param name="pageTotal">Page total.</param>
        /// <param name="hourTotal">Hour total.</param>
        /// <param name="minuteTotal">Minute total.</param>
        /// <returns>Parsed string.</returns>
        public static string? SetBookDurationTotal(string? format, int pageTotal, int hourTotal, int minuteTotal)
        {
            return !format!.Equals(AppStringResources.Audiobook) ?
                AppStringResources.BlankPages.Replace("Blank", pageTotal.ToString()) :
                AppStringResources.Blank1HoursBlank2Minutes.Replace("Blank1", hourTotal.ToString().PadLeft(2, '0')).Replace("Blank2", minuteTotal.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="bookPrice">Book price.</param>
        /// <returns>Parsed string.</returns>
        public static string? SetBookPrice(string? bookPrice)
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
            var cultureInfo = new CultureInfo(cultureCode);

            if (bookPrice == null || !bookPrice.Contains(cultureInfo.NumberFormat.CurrencySymbol))
            {
                var parsed = double.TryParse(bookPrice, out double price);

                return string.Format(cultureInfo, "{0:C}", parsed ? price : 0);
            }

            return bookPrice;
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="bookSeriesGuid">Book series guid.</param>
        /// <param name="bookSeriesName">Book series name.</param>
        /// <param name="numberInSeries">Book number in series.</param>
        /// <returns>Parsed string.</returns>
        public static async Task<(bool, string?, string?)> SetSeriesString(Guid? bookSeriesGuid, string? bookSeriesName, string? numberInSeries)
        {
            var hasSeries = bookSeriesGuid != null || !string.IsNullOrEmpty(bookSeriesName);
            var seriesOutput = string.Empty;

            if (bookSeriesGuid != null)
            {
                var series = await GetItems.GetSeriesForBook(bookSeriesGuid);

                bookSeriesName = null;

                if (series != null)
                {
                    if (!string.IsNullOrEmpty(numberInSeries))
                    {
                        seriesOutput = $"{AppStringResources.PartofSeries.Replace("blank", $"{series.SeriesName}")}, {AppStringResources.BookNumber.Replace("Number", $"{numberInSeries}")}";
                    }

                    if (!string.IsNullOrEmpty(numberInSeries) && !string.IsNullOrEmpty(series.TotalBooksInSeries))
                    {
                        seriesOutput = $"{AppStringResources.PartofSeries.Replace("blank", $"{series.SeriesName}")}, {AppStringResources.BookNumberOfTotal.Replace("number", $"{numberInSeries}").Replace("total", $"{series.TotalBooksInSeries}")}";
                    }

                    if (string.IsNullOrEmpty(numberInSeries))
                    {
                        seriesOutput = $"{AppStringResources.PartofSeries.Replace("blank", $"{series.SeriesName}")}";
                    }
                }
            }

            if (!string.IsNullOrEmpty(bookSeriesName))
            {
                if (!string.IsNullOrEmpty(numberInSeries))
                {
                    seriesOutput = $"{AppStringResources.PartofSeries.Replace("blank", $"{bookSeriesName}")}, {AppStringResources.BookNumber.Replace("Number", $"{numberInSeries}")}";
                }

                if (string.IsNullOrEmpty(numberInSeries))
                {
                    seriesOutput = $"{AppStringResources.PartofSeries.Replace("blank", $"{bookSeriesName}")}";
                }
            }

            return (hasSeries, seriesOutput, bookSeriesName);
        }

        /// <summary>
        /// Parse author list string to author list.
        /// </summary>
        /// <param name="input">String to parse.</param>
        /// <returns>A list of authors.</returns>
        public static async Task<List<AuthorModel>> SplitAuthorListStringIntoAuthorList(string input)
        {
            var list = new List<AuthorModel>();

            string[] authorNames = input.Split(";");

            foreach (var authorName in authorNames)
            {
                if (!string.IsNullOrEmpty(authorName.Trim()))
                {
                    string[] name = authorName.Split(",");

                    var author = await BaseViewModel.Database.GetAuthorByNameAsync(name[1].Trim(), name[0].Trim());

                    author ??= new AuthorModel();

                    author.FirstName ??= name[1].Trim();
                    author.LastName ??= name[0].Trim();

                    list.Add(author);
                }
            }

            return list;
        }

        /// <summary>
        /// Formats the input time to string.
        /// </summary>
        /// <param name="hour">Input hour.</param>
        /// <param name="minute">Input minute.</param>
        /// <returns>Formatted time as a string.</returns>
        public static string FormatTimeString(int hour, int minute)
        {
            return $"{hour:0}:{minute:00}";
        }
    }
}
