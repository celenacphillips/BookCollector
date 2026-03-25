// <copyright file="SortLists.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// SortLists class.
    /// </summary>
    public partial class SortLists : ObservableObject
    {
        /// <summary>
        /// Sort list based on the input parameters.
        /// </summary>
        /// <param name="bookList">List to sort.</param>
        /// <param name="bookTitleChecked">Book title checked option.</param>
        /// <param name="bookReadingDateChecked">Book reading date checked option.</param>
        /// <param name="bookReadPercentageChecked">Book reading percent checked option.</param>
        /// <param name="bookPublisherChecked">Book publisher checked option.</param>
        /// <param name="bookPublishYearChecked">Book publish year checked option.</param>
        /// <param name="authorLastNameChecked">Author last name checked option.</param>
        /// <param name="seriesOrderChecked">Series order checked option.</param>
        /// <param name="bookFormatChecked">Book format checked option.</param>
        /// <param name="bookPriceChecked">Book price checked option.</param>
        /// <param name="pageCountBookTimeChecked">Page count/book time checked option.</param>
        /// <param name="ascendingChecked">Ascending checked option.</param>
        /// <param name="descendingChecked">Descending checked option.</param>
        /// <returns>The sorted list.</returns>
        public static async Task<ObservableCollection<BookModel>> SortList(
            ObservableCollection<BookModel> bookList,
            bool bookTitleChecked,
            bool bookReadingDateChecked,
            bool bookReadPercentageChecked,
            bool bookPublisherChecked,
            bool bookPublishYearChecked,
            bool authorLastNameChecked,
            bool seriesOrderChecked,
            bool bookFormatChecked,
            bool bookPriceChecked,
            bool pageCountBookTimeChecked,
            bool ascendingChecked,
            bool descendingChecked)
        {
            var filteredList = bookList;

            if (bookTitleChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookReadingDateChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.EndDateValue).ThenBy(x => x.StartDateValue).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.EndDateValue).ThenByDescending(x => x.StartDateValue).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookReadPercentageChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.Progress).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.Progress).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookPublisherChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.BookPublisher).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.BookPublisher).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookPublishYearChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.BookPublishYear).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.BookPublishYear).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (authorLastNameChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.AuthorListString).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.AuthorListString).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookFormatChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.BookFormat).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.BookFormat).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.BookPrice).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.BookPrice).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (seriesOrderChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.BookNumberInSeries).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.BookNumberInSeries).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (pageCountBookTimeChecked)
            {
                if (ascendingChecked)
                {
                    var nonAudio = filteredList.Where(x => x.BookFormat != AppStringResources.Audiobook).OrderBy(x => x.BookPageTotal).ThenBy(x => x.ParsedTitle);
                    var audio = filteredList.Where(x => x.BookFormat == AppStringResources.Audiobook).OrderBy(x => x.BookTotalTime).ThenBy(x => x.ParsedTitle);

                    var list = new List<BookModel>();
                    list.AddRange(nonAudio);
                    list.AddRange(audio);

                    filteredList = list.ToObservableCollection();
                }

                if (descendingChecked)
                {
                    var nonAudio = filteredList.Where(x => x.BookFormat != AppStringResources.Audiobook).OrderByDescending(x => x.BookPageTotal).ThenByDescending(x => x.ParsedTitle);
                    var audio = filteredList.Where(x => x.BookFormat == AppStringResources.Audiobook).OrderByDescending(x => x.BookTotalTime).ThenByDescending(x => x.ParsedTitle);

                    var list = new List<BookModel>();
                    list.AddRange(nonAudio);
                    list.AddRange(audio);

                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Sort list based on the input parameters.
        /// </summary>
        /// <param name="bookList">List to sort.</param>
        /// <param name="bookTitleChecked">Book title checked option.</param>
        /// <param name="bookPublisherChecked">Book publisher checked option.</param>
        /// <param name="bookPublishYearChecked">Book publish year checked option.</param>
        /// <param name="authorLastNameChecked">Author last name checked option.</param>
        /// <param name="bookFormatChecked">Book format checked option.</param>
        /// <param name="bookPriceChecked">Book price checked option.</param>
        /// <param name="pageCountBookTimeChecked">Page count/book time checked option.</param>
        /// <param name="ascendingChecked">Ascending checked option.</param>
        /// <param name="descendingChecked">Descending checked option.</param>
        /// <returns>The sorted list.</returns>
        public static async Task<ObservableCollection<WishlistBookModel>> SortList(
            ObservableCollection<WishlistBookModel> bookList,
            bool bookTitleChecked,
            bool bookPublisherChecked,
            bool bookPublishYearChecked,
            bool authorLastNameChecked,
            bool bookFormatChecked,
            bool bookPriceChecked,
            bool pageCountBookTimeChecked,
            bool ascendingChecked,
            bool descendingChecked)
        {
            var filteredList = bookList;

            if (bookTitleChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookPublisherChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.BookPublisher).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.BookPublisher).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookPublishYearChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.BookPublishYear).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.BookPublishYear).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (authorLastNameChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.AuthorListString).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.AuthorListString).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookFormatChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.BookFormat).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.BookFormat).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (bookPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.BookPrice).ThenBy(x => x.ParsedTitle).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.BookPrice).ThenByDescending(x => x.ParsedTitle).ToObservableCollection();
                }
            }

            if (pageCountBookTimeChecked)
            {
                if (ascendingChecked)
                {
                    var nonAudio = filteredList.Where(x => x.BookFormat != AppStringResources.Audiobook).OrderBy(x => x.BookPageTotal).ThenBy(x => x.ParsedTitle);
                    var audio = filteredList.Where(x => x.BookFormat == AppStringResources.Audiobook).OrderBy(x => x.BookTotalTime).ThenBy(x => x.ParsedTitle);

                    var list = new List<WishlistBookModel>();
                    list.AddRange(nonAudio);
                    list.AddRange(audio);

                    filteredList = list.ToObservableCollection();
                }

                if (descendingChecked)
                {
                    var nonAudio = filteredList.Where(x => x.BookFormat != AppStringResources.Audiobook).OrderByDescending(x => x.BookPageTotal).ThenByDescending(x => x.ParsedTitle);
                    var audio = filteredList.Where(x => x.BookFormat == AppStringResources.Audiobook).OrderByDescending(x => x.BookTotalTime).ThenByDescending(x => x.ParsedTitle);

                    var list = new List<WishlistBookModel>();
                    list.AddRange(nonAudio);
                    list.AddRange(audio);

                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Sort list based on the input parameters.
        /// </summary>
        /// <param name="collectionList">List to sort.</param>
        /// <param name="collectionNameChecked">Name checked option.</param>
        /// <param name="totalBooksChecked">Total books checked option.</param>
        /// <param name="totalPriceChecked">Total price checked option.</param>
        /// <param name="ascendingChecked">Ascending checked option.</param>
        /// <param name="descendingChecked">Descending checked option.</param>
        /// <returns>The sorted list.</returns>
        public static async Task<ObservableCollection<CollectionModel>> SortList(
            ObservableCollection<CollectionModel> collectionList,
            bool collectionNameChecked,
            bool totalBooksChecked,
            bool totalPriceChecked,
            bool ascendingChecked,
            bool descendingChecked)
        {
            var filteredList = collectionList;

            if (collectionNameChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedCollectionName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedCollectionName).ToObservableCollection();
                }
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.CollectionTotalBooks).ThenBy(x => x.ParsedCollectionName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.CollectionTotalBooks).ThenByDescending(x => x.ParsedCollectionName).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.TotalCostOfBooks).ThenBy(x => x.ParsedCollectionName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.TotalCostOfBooks).ThenByDescending(x => x.ParsedCollectionName).ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Sort list based on the input parameters.
        /// </summary>
        /// <param name="genreList">List to sort.</param>
        /// <param name="genreNameChecked">Name checked option.</param>
        /// <param name="totalBooksChecked">Total books checked option.</param>
        /// <param name="totalPriceChecked">Total price checked option.</param>
        /// <param name="ascendingChecked">Ascending checked option.</param>
        /// <param name="descendingChecked">Descending checked option.</param>
        /// <returns>The sorted list.</returns>
        public static async Task<ObservableCollection<GenreModel>> SortList(
            ObservableCollection<GenreModel> genreList,
            bool genreNameChecked,
            bool totalBooksChecked,
            bool totalPriceChecked,
            bool ascendingChecked,
            bool descendingChecked)
        {
            var filteredList = genreList;

            if (genreNameChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedGenreName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedGenreName).ToObservableCollection();
                }
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.GenreTotalBooks).ThenBy(x => x.ParsedGenreName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.GenreTotalBooks).ThenByDescending(x => x.ParsedGenreName).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.TotalCostOfBooks).ThenBy(x => x.ParsedGenreName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.TotalCostOfBooks).ThenByDescending(x => x.ParsedGenreName).ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Sort list based on the input parameters.
        /// </summary>
        /// <param name="seriesList">List to sort.</param>
        /// <param name="seriesNameChecked">Name checked option.</param>
        /// <param name="totalBooksChecked">Total books checked option.</param>
        /// <param name="totalPriceChecked">Total price checked option.</param>
        /// <param name="ascendingChecked">Ascending checked option.</param>
        /// <param name="descendingChecked">Descending checked option.</param>
        /// <returns>The sorted list.</returns>
        public static async Task<ObservableCollection<SeriesModel>> SortList(
            ObservableCollection<SeriesModel> seriesList,
            bool seriesNameChecked,
            bool totalBooksChecked,
            bool totalPriceChecked,
            bool ascendingChecked,
            bool descendingChecked)
        {
            var filteredList = seriesList;

            if (seriesNameChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedSeriesName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedSeriesName).ToObservableCollection();
                }
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.SeriesTotalBooks).ThenBy(x => x.ParsedSeriesName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.SeriesTotalBooks).ThenByDescending(x => x.ParsedSeriesName).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.TotalCostOfBooks).ThenBy(x => x.ParsedSeriesName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.TotalCostOfBooks).ThenByDescending(x => x.ParsedSeriesName).ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Sort list based on the input parameters.
        /// </summary>
        /// <param name="locationList">List to sort.</param>
        /// <param name="locationNameChecked">Name checked option.</param>
        /// <param name="totalBooksChecked">Total books checked option.</param>
        /// <param name="totalPriceChecked">Total price checked option.</param>
        /// <param name="ascendingChecked">Ascending checked option.</param>
        /// <param name="descendingChecked">Descending checked option.</param>
        /// <returns>The sorted list.</returns>
        public static async Task<ObservableCollection<LocationModel>> SortList(
            ObservableCollection<LocationModel> locationList,
            bool locationNameChecked,
            bool totalBooksChecked,
            bool totalPriceChecked,
            bool ascendingChecked,
            bool descendingChecked)
        {
            var filteredList = locationList;

            if (locationNameChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedLocationName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedLocationName).ToObservableCollection();
                }
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.LocationTotalBooks).ThenBy(x => x.ParsedLocationName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.LocationTotalBooks).ThenByDescending(x => x.ParsedLocationName).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.TotalCostOfBooks).ThenBy(x => x.ParsedLocationName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.TotalCostOfBooks).ThenByDescending(x => x.ParsedLocationName).ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Sort list based on the input parameters.
        /// </summary>
        /// <param name="authorList">List to sort.</param>
        /// <param name="authorLastNameChecked">Name checked option.</param>
        /// <param name="totalBooksChecked">Total books checked option.</param>
        /// <param name="totalPriceChecked">Total price checked option.</param>
        /// <param name="ascendingChecked">Ascending checked option.</param>
        /// <param name="descendingChecked">Descending checked option.</param>
        /// <returns>The sorted list.</returns>
        public static async Task<ObservableCollection<AuthorModel>> SortList(
            ObservableCollection<AuthorModel> authorList,
            bool authorLastNameChecked,
            bool totalBooksChecked,
            bool totalPriceChecked,
            bool ascendingChecked,
            bool descendingChecked)
        {
            var filteredList = authorList;

            if (authorLastNameChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName).ToObservableCollection();
                }
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.AuthorTotalBooks).ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.AuthorTotalBooks).ThenByDescending(x => x.LastName).ThenByDescending(x => x.FirstName).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.TotalCostOfBooks).ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.TotalCostOfBooks).ThenByDescending(x => x.LastName).ThenByDescending(x => x.FirstName).ToObservableCollection();
                }
            }

            return filteredList;
        }
    }
}
