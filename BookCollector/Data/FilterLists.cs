// <copyright file="FilterLists.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// FilterLists class.
    /// </summary>
    public partial class FilterLists : ObservableObject
    {
        /// <summary>
        /// Apply filters to book list based on the given filter options and search string.
        /// </summary>
        /// <param name="bookList">Book list to filter.</param>
        /// <param name="favoritesOption">Favorite option to find.</param>
        /// <param name="formatOption">Format option to find.</param>
        /// <param name="publisherOption">Publisher option to find.</param>
        /// <param name="languageOption">Language option to find.</param>
        /// <param name="ratingOption">Rating option to find.</param>
        /// <param name="publishYearOption">Publish year option to find.</param>
        /// <param name="authorOption">Author option to find.</param>
        /// <param name="bookCoverOption">Book cover option to find.</param>
        /// <param name="readingStatusOption">Reading status option to find.</param>
        /// <param name="loanedOutOption">Loaned out option to find.</param>
        /// <param name="borrowedOption">Borrowed option to find.</param>
        /// <param name="searchString">Book title search string to find.</param>
        /// <returns>A filtered book list.</returns>
        public static async Task<ObservableCollection<BookModel>?> FilterList(
            ObservableCollection<BookModel>? bookList,
            string? favoritesOption,
            string? formatOption,
            string? publisherOption,
            string? languageOption,
            string? ratingOption,
            string? publishYearOption,
            string? authorOption,
            string? bookCoverOption,
            string? readingStatusOption,
            string? loanedOutOption,
            string? borrowedOption,
            string? searchString)
        {
            var filteredList = bookList;

            filteredList = FilterOnSearchString(filteredList, searchString);

            if (!string.IsNullOrEmpty(favoritesOption))
            {
                filteredList = FilterFavoriteBooks(filteredList, favoritesOption);
            }

            filteredList = FilterBookFormat(filteredList, formatOption);

            filteredList = FilterBookPublisher(filteredList, publisherOption);

            filteredList = FilterBookLanguage(filteredList, languageOption);

            if (!string.IsNullOrEmpty(ratingOption))
            {
                filteredList = FilterBookRating(filteredList, ratingOption);
            }

            filteredList = FilterBookPublishYear(filteredList, publishYearOption);

            if (!string.IsNullOrEmpty(authorOption))
            {
                filteredList = FilterBookAuthor(filteredList, authorOption);
            }

            if (!string.IsNullOrEmpty(bookCoverOption))
            {
                filteredList = FilterBooksOnBookCovers(filteredList, bookCoverOption);
            }

            if (!string.IsNullOrEmpty(readingStatusOption))
            {
                filteredList = FilterBooksOnReadingStatus(filteredList, readingStatusOption);
            }

            if (!string.IsNullOrEmpty(loanedOutOption))
            {
                filteredList = FilterBooksOnLoanedOutStatus(filteredList, loanedOutOption);
            }

            if (!string.IsNullOrEmpty(borrowedOption))
            {
                filteredList = FilterBooksOnBorrowedStatus(filteredList, borrowedOption);
            }

            return filteredList;
        }

        /// <summary>
        /// Apply filters to wishlist book list based on the given filter options and search string.
        /// </summary>
        /// <param name="bookList">Book list to filter.</param>
        /// <param name="formatOption">Format option to find.</param>
        /// <param name="publisherOption">Publisher option to find.</param>
        /// <param name="languageOption">Language option to find.</param>
        /// <param name="publishYearOption">Publish year option to find.</param>
        /// <param name="authorOption">Author option to find.</param>
        /// <param name="locationOption">Location option to find.</param>
        /// <param name="seriesOption">Series option to find.</param>
        /// <param name="bookCoverOption">Book cover option to find.</param>
        /// <param name="searchString">Book title search string to find.</param>
        /// <returns>A filtered book list.</returns>
        public static async Task<ObservableCollection<WishlistBookModel>?> FilterList(
            ObservableCollection<WishlistBookModel>? bookList,
            string? formatOption,
            string? publisherOption,
            string? languageOption,
            string? publishYearOption,
            string? authorOption,
            string? locationOption,
            string? seriesOption,
            string? bookCoverOption,
            string? searchString)
        {
            var filteredList = bookList;

            filteredList = FilterOnSearchString(filteredList, searchString);

            filteredList = FilterBookFormat(filteredList, formatOption);

            filteredList = FilterBookPublisher(filteredList, publisherOption);

            filteredList = FilterBookLanguage(filteredList, languageOption);

            filteredList = FilterBookPublishYear(filteredList, publishYearOption);

            filteredList = FilterBookAuthor(filteredList, authorOption);

            filteredList = FilterBookLocation(filteredList, locationOption);

            filteredList = FilterBookSeries(filteredList, seriesOption);

            if (!string.IsNullOrEmpty(bookCoverOption))
            {
                filteredList = FilterBooksOnBookCovers(filteredList, bookCoverOption);
            }

            return filteredList;
        }

        /// <summary>
        /// Apply filters to author list based on the given filter options and search string.
        /// </summary>
        /// <param name="authorList">Author list to filter.</param>
        /// <param name="favoritesOption">Favorite option to find.</param>
        /// <param name="searchString">Author name search string to find.</param>
        /// <returns>A filtered author list.</returns>
        public static async Task<ObservableCollection<AuthorModel>?> FilterList(
            ObservableCollection<AuthorModel>? authorList,
            string? favoritesOption,
            string? searchString)
        {
            var filteredList = authorList;

            if (!string.IsNullOrEmpty(favoritesOption))
            {
                filteredList = FilterFavoriteAuthors(filteredList, favoritesOption);
            }

            filteredList = FilterOnSearchString(filteredList, searchString);

            return filteredList;
        }

        /// <summary>
        /// Apply filters to collection list based on the given filter options and search string.
        /// </summary>
        /// <param name="collectionList">Collection list to filter.</param>
        /// <param name="favoritesOption">Favorite option to find.</param>
        /// <param name="searchString">Collection name search string to find.</param>
        /// <returns>A filtered collection list.</returns>
        public static async Task<ObservableCollection<CollectionModel>?> FilterList(
            ObservableCollection<CollectionModel>? collectionList,
            string? favoritesOption,
            string? searchString)
        {
            var filteredList = collectionList;

            if (!string.IsNullOrEmpty(favoritesOption))
            {
                filteredList = FilterFavoriteCollections(filteredList, favoritesOption);
            }

            filteredList = FilterOnSearchString(filteredList, searchString);

            return filteredList;
        }

        /// <summary>
        /// Apply filters to genre list based on the given filter options and search string.
        /// </summary>
        /// <param name="genreList">Genre list to filter.</param>
        /// <param name="favoritesOption">Favorite option to find.</param>
        /// <param name="searchString">Genre name search string to find.</param>
        /// <returns>A filtered genre list.</returns>
        public static async Task<ObservableCollection<GenreModel>?> FilterList(
            ObservableCollection<GenreModel>? genreList,
            string? favoritesOption,
            string? searchString)
        {
            var filteredList = genreList;

            if (!string.IsNullOrEmpty(favoritesOption))
            {
                filteredList = FilterFavoriteGenres(filteredList, favoritesOption);
            }

            filteredList = FilterOnSearchString(filteredList, searchString);

            return filteredList;
        }

        /// <summary>
        /// Apply filters to location list based on the given filter options and search string.
        /// </summary>
        /// <param name="locationList">Location list to filter.</param>
        /// <param name="favoritesOption">Favorite option to find.</param>
        /// <param name="searchString">Location name search string to find.</param>
        /// <returns>A filtered location list.</returns>
        public static async Task<ObservableCollection<LocationModel>?> FilterList(
            ObservableCollection<LocationModel>? locationList,
            string? favoritesOption,
            string? searchString)
        {
            var filteredList = locationList;

            if (!string.IsNullOrEmpty(favoritesOption))
            {
                filteredList = FilterFavoriteLocations(filteredList, favoritesOption);
            }

            filteredList = FilterOnSearchString(filteredList, searchString);

            return filteredList;
        }

        /// <summary>
        /// Apply filters to series list based on the given filter options and search string.
        /// </summary>
        /// <param name="seriesList">Series list to filter.</param>
        /// <param name="favoritesOption">Favorite option to find.</param>
        /// <param name="searchString">Series name search string to find.</param>
        /// <returns>A filtered series list.</returns>
        public static async Task<ObservableCollection<SeriesModel>?> FilterList(
            ObservableCollection<SeriesModel>? seriesList,
            string? favoritesOption,
            string? searchString)
        {
            var filteredList = seriesList;

            if (!string.IsNullOrEmpty(favoritesOption))
            {
                filteredList = FilterFavoriteSeries(filteredList, favoritesOption);
            }

            filteredList = FilterOnSearchString(filteredList, searchString);

            return filteredList;
        }

        /********************************************************/

        /// <summary>
        /// Apply search string filter to book list based on the given search string.
        /// </summary>
        /// <param name="bookList">Book list to filter.</param>
        /// <param name="searchString">Book title search string to find.</param>
        /// <returns>A filtered book list.</returns>
        public static ObservableCollection<BookModel>? FilterOnSearchString(ObservableCollection<BookModel>? bookList, string? searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return bookList?.Where(x => !string.IsNullOrEmpty(x.BookTitle) && x.BookTitle.ToLower().Contains(searchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            }
            else
            {
                return bookList;
            }
        }

        /// <summary>
        /// Apply search string filter to book list based on the given search string.
        /// </summary>
        /// <param name="bookList">Book list to filter.</param>
        /// <param name="searchString">Book title search string to find.</param>
        /// <returns>A filtered book list.</returns>
        public static ObservableCollection<WishlistBookModel>? FilterOnSearchString(ObservableCollection<WishlistBookModel>? bookList, string? searchString)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(searchString))
            {
                filterList = filterList?.Where(x => !string.IsNullOrEmpty(x.BookTitle) && x.BookTitle.ToLower().Contains(searchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            }

            return filterList;
        }

        /// <summary>
        /// Apply search string filter to author list based on the given search string.
        /// </summary>
        /// <param name="authorList">Author list to filter.</param>
        /// <param name="searchString">Author name search string to find.</param>
        /// <returns>A filtered book list.</returns>
        public static ObservableCollection<AuthorModel>? FilterOnSearchString(ObservableCollection<AuthorModel>? authorList, string? searchString)
        {
            var filterList = authorList;

            if (!string.IsNullOrEmpty(searchString))
            {
                filterList = filterList?.Where(x => !string.IsNullOrEmpty(x.FullName) && x.FullName.ToLower().Contains(searchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            }

            return filterList;
        }

        /// <summary>
        /// Apply search string filter to collection list based on the given search string.
        /// </summary>
        /// <param name="collectionList">Collection list to filter.</param>
        /// <param name="searchString">Collection name search string to find.</param>
        /// <returns>A filtered book list.</returns>
        public static ObservableCollection<CollectionModel>? FilterOnSearchString(ObservableCollection<CollectionModel>? collectionList, string? searchString)
        {
            var filterList = collectionList;

            if (!string.IsNullOrEmpty(searchString))
            {
                filterList = filterList?.Where(x => !string.IsNullOrEmpty(x.CollectionName) && x.CollectionName.ToLower().Contains(searchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            }

            return filterList;
        }

        /// <summary>
        /// Apply search string filter to genre list based on the given search string.
        /// </summary>
        /// <param name="genreList">Genre list to filter.</param>
        /// <param name="searchString">Genre name search string to find.</param>
        /// <returns>A filtered book list.</returns>
        public static ObservableCollection<GenreModel>? FilterOnSearchString(ObservableCollection<GenreModel>? genreList, string? searchString)
        {
            var filterList = genreList;

            if (!string.IsNullOrEmpty(searchString))
            {
                filterList = filterList?.Where(x => !string.IsNullOrEmpty(x.GenreName) && x.GenreName.ToLower().Contains(searchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            }

            return filterList;
        }

        /// <summary>
        /// Apply search string filter to location list based on the given search string.
        /// </summary>
        /// <param name="locationList">Location list to filter.</param>
        /// <param name="searchString">Location name search string to find.</param>
        /// <returns>A filtered book list.</returns>
        public static ObservableCollection<LocationModel>? FilterOnSearchString(ObservableCollection<LocationModel>? locationList, string? searchString)
        {
            var filterList = locationList;

            if (!string.IsNullOrEmpty(searchString))
            {
                filterList = filterList?.Where(x => !string.IsNullOrEmpty(x.LocationName) && x.LocationName.ToLower().Contains(searchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            }

            return filterList;
        }

        /// <summary>
        /// Apply search string filter to series list based on the given search string.
        /// </summary>
        /// <param name="seriesList">Series list to filter.</param>
        /// <param name="searchString">Series name search string to find.</param>
        /// <returns>A filtered book list.</returns>
        public static ObservableCollection<SeriesModel>? FilterOnSearchString(ObservableCollection<SeriesModel>? seriesList, string? searchString)
        {
            var filterList = seriesList;

            if (!string.IsNullOrEmpty(searchString))
            {
                filterList = filterList?.Where(x => !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.ToLower().Contains(searchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterFavoriteBooks(ObservableCollection<BookModel>? bookList, string favoritesOption)
        {
            var filterList = bookList;

            if (favoritesOption.Equals(AppStringResources.Favorites))
            {
                filterList = bookList?.Where(x => x.IsFavorite).ToObservableCollection();
            }

            if (favoritesOption.Equals(AppStringResources.NonFavorites))
            {
                filterList = bookList?.Where(x => !x.IsFavorite).ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<AuthorModel>? FilterFavoriteAuthors(ObservableCollection<AuthorModel>? authorList, string favoritesOption)
        {
            var filterList = authorList;

            if (favoritesOption.Equals(AppStringResources.Favorites))
            {
                filterList = authorList?.Where(x => x.IsFavorite).ToObservableCollection();
            }

            if (favoritesOption.Equals(AppStringResources.NonFavorites))
            {
                filterList = authorList?.Where(x => !x.IsFavorite).ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<CollectionModel>? FilterFavoriteCollections(ObservableCollection<CollectionModel>? collectionList, string favoritesOption)
        {
            var filterList = collectionList;

            if (favoritesOption.Equals(AppStringResources.Favorites))
            {
                filterList = collectionList?.Where(x => x.IsFavorite).ToObservableCollection();
            }

            if (favoritesOption.Equals(AppStringResources.NonFavorites))
            {
                filterList = collectionList?.Where(x => !x.IsFavorite).ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<GenreModel>? FilterFavoriteGenres(ObservableCollection<GenreModel>? genreList, string favoritesOption)
        {
            var filterList = genreList;

            if (favoritesOption.Equals(AppStringResources.Favorites))
            {
                filterList = genreList?.Where(x => x.IsFavorite).ToObservableCollection();
            }

            if (favoritesOption.Equals(AppStringResources.NonFavorites))
            {
                filterList = genreList?.Where(x => !x.IsFavorite).ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<LocationModel>? FilterFavoriteLocations(ObservableCollection<LocationModel>? locationList, string favoritesOption)
        {
            var filterList = locationList;

            if (favoritesOption.Equals(AppStringResources.Favorites))
            {
                filterList = locationList?.Where(x => x.IsFavorite).ToObservableCollection();
            }

            if (favoritesOption.Equals(AppStringResources.NonFavorites))
            {
                filterList = locationList?.Where(x => !x.IsFavorite).ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<SeriesModel>? FilterFavoriteSeries(ObservableCollection<SeriesModel>? seriesList, string favoritesOption)
        {
            var filterList = seriesList;

            if (favoritesOption.Equals(AppStringResources.Favorites))
            {
                filterList = seriesList?.Where(x => x.IsFavorite).ToObservableCollection();
            }

            if (favoritesOption.Equals(AppStringResources.NonFavorites))
            {
                filterList = seriesList?.Where(x => !x.IsFavorite).ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterBookFormat(ObservableCollection<BookModel>? bookList, string? formatOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(formatOption) && !formatOption.Equals(AppStringResources.AllFormats))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookFormat) && x.BookFormat.Equals(formatOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel>? FilterBookFormat(ObservableCollection<WishlistBookModel>? bookList, string? formatOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(formatOption) && !formatOption.Equals(AppStringResources.AllFormats))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookFormat) && x.BookFormat.Equals(formatOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterBookPublisher(ObservableCollection<BookModel>? bookList, string? publisherOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(publisherOption) && publisherOption.Equals(AppStringResources.NoPublisher))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookPublisher))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(publisherOption) && !publisherOption.Equals(AppStringResources.NoPublisher) && !publisherOption.Equals(AppStringResources.AllPublishers))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookPublisher) && x.BookPublisher.ToLower().Equals(publisherOption.ToLower().Trim()))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel>? FilterBookPublisher(ObservableCollection<WishlistBookModel>? bookList, string? publisherOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(publisherOption) && publisherOption.Equals(AppStringResources.NoPublisher))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookPublisher))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(publisherOption) && !publisherOption.Equals(AppStringResources.NoPublisher) && !publisherOption.Equals(AppStringResources.AllPublishers))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookPublisher) && x.BookPublisher.ToLower().Equals(publisherOption.ToLower().Trim()))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterBookPublishYear(ObservableCollection<BookModel>? bookList, string? publishYearOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(publishYearOption) && publishYearOption.Equals(AppStringResources.NoPublishYear))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(publishYearOption) && !publishYearOption.Equals(AppStringResources.NoPublishYear) && !publishYearOption.Equals(AppStringResources.AllPublishYears))
            {
                var years = publishYearOption.Split(" - ");

                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookPublishYear) &&
                                            int.Parse(years[0]) <= int.Parse(x.BookPublishYear) &&
                                            int.Parse(years[1]) >= int.Parse(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel>? FilterBookPublishYear(ObservableCollection<WishlistBookModel>? bookList, string? publishYearOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(publishYearOption) && publishYearOption.Equals(AppStringResources.NoPublishYear))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(publishYearOption) && !publishYearOption.Equals(AppStringResources.NoPublishYear) && !publishYearOption.Equals(AppStringResources.AllPublishYears))
            {
                var years = publishYearOption.Split(" - ");

                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookPublishYear) &&
                                            int.Parse(years[0]) <= int.Parse(x.BookPublishYear) &&
                                            int.Parse(years[1]) >= int.Parse(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterBookLanguage(ObservableCollection<BookModel>? bookList, string? languageOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(languageOption) && languageOption.Equals(AppStringResources.NoLanguage))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookLanguage))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(languageOption) && !languageOption.Equals(AppStringResources.NoLanguage) && !languageOption.Equals(AppStringResources.AllLanguages))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookLanguage) && x.BookLanguage.ToLower().Equals(languageOption.ToLower().Trim()))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel>? FilterBookLanguage(ObservableCollection<WishlistBookModel>? bookList, string? languageOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(languageOption) && languageOption.Equals(AppStringResources.NoLanguage))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookLanguage))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(languageOption) && !languageOption.Equals(AppStringResources.NoLanguage) && !languageOption.Equals(AppStringResources.AllLanguages))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookLanguage) && x.BookLanguage.ToLower().Equals(languageOption.ToLower().Trim()))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterBookRating(ObservableCollection<BookModel>? bookList, string ratingOption)
        {
            var filterList = bookList;

            ratingOption = ratingOption.Equals(AppStringResources.ZeroStars) ? "0" : ratingOption;
            ratingOption = ratingOption.Equals(AppStringResources.OneStar) ? "1" : ratingOption;
            ratingOption = ratingOption.Equals(AppStringResources.TwoStars) ? "2" : ratingOption;
            ratingOption = ratingOption.Equals(AppStringResources.ThreeStars) ? "3" : ratingOption;
            ratingOption = ratingOption.Equals(AppStringResources.FourStars) ? "4" : ratingOption;
            ratingOption = ratingOption.Equals(AppStringResources.FiveStars) ? "5" : ratingOption;

            if (!ratingOption.Equals(AppStringResources.AllRatings))
            {
                filterList = bookList?.Where(x => x.Rating == int.Parse(ratingOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterBookAuthor(ObservableCollection<BookModel>? bookList, string? authorOption)
        {
            var filterList = bookList;
            var newFilteredList = new ObservableCollection<BookModel>();

            if (!string.IsNullOrEmpty(authorOption) && authorOption.Equals(AppStringResources.NoAuthor))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.AuthorListString))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(authorOption) && !authorOption.Equals(AppStringResources.NoAuthor) && !authorOption.Equals(AppStringResources.AllAuthors))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.AuthorListString) && x.AuthorListString.Contains(authorOption.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<WishlistBookModel>? FilterBookAuthor(ObservableCollection<WishlistBookModel>? bookList, string? authorOption)
        {
            var filterList = bookList;
            var newFilteredList = new ObservableCollection<WishlistBookModel>();

            if (!string.IsNullOrEmpty(authorOption) && authorOption.Equals(AppStringResources.NoAuthor))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.AuthorListString))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(authorOption) && !authorOption.Equals(AppStringResources.NoAuthor) && !authorOption.Equals(AppStringResources.AllAuthors))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.AuthorListString) && x.AuthorListString.Contains(authorOption.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<WishlistBookModel>? FilterBookLocation(ObservableCollection<WishlistBookModel>? bookList, string? locationOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(locationOption) && locationOption.Equals(AppStringResources.NoLocation))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookWhereToBuy))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(locationOption) && !locationOption.Equals(AppStringResources.NoLocation) && !locationOption.Equals(AppStringResources.AllLocations))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy) && x.BookWhereToBuy.ToLower().Equals(locationOption.ToLower().Trim()))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<WishlistBookModel>? FilterBookSeries(ObservableCollection<WishlistBookModel>? bookList, string? seriesOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(seriesOption) && seriesOption.Equals(AppStringResources.NoSeries))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookSeries))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(seriesOption) && !seriesOption.Equals(AppStringResources.NoSeries) && !seriesOption.Equals(AppStringResources.AllSeries))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookSeries) && x.BookSeries.ToLower().Equals(seriesOption.ToLower().Trim()))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterBooksOnBookCovers(ObservableCollection<BookModel>? bookList, string bookCoverOption)
        {
            var filterList = bookList;

            if (bookCoverOption.Equals(AppStringResources.HasABookCover))
            {
                filterList = bookList?.Where(x => x.HasBookCover).ToObservableCollection();
            }

            if (bookCoverOption.Equals(AppStringResources.HasNoBookCover))
            {
                filterList = bookList?.Where(x => x.HasNoBookCover).ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel>? FilterBooksOnBookCovers(ObservableCollection<WishlistBookModel>? bookList, string bookCoverOption)
        {
            var filterList = bookList;

            if (bookCoverOption.Equals(AppStringResources.HasABookCover))
            {
                filterList = bookList?.Where(x => x.HasBookCover).ToObservableCollection();
            }

            if (bookCoverOption.Equals(AppStringResources.HasNoBookCover))
            {
                filterList = bookList?.Where(x => x.HasNoBookCover).ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterBooksOnReadingStatus(ObservableCollection<BookModel>? bookList, string readingStatusOption)
        {
            var filterList = bookList;

            if (readingStatusOption.Equals(AppStringResources.ToBeRead))
            {
                filterList = bookList?
                    .Where(x => (x.BookPageRead == 0 &&
                    (x.BookHourListened == 0 && x.BookMinuteListened == 0)) &&
                    (string.IsNullOrEmpty(x.BookStartDate) && string.IsNullOrEmpty(x.BookEndDate)))
                    .ToObservableCollection();
            }

            if (readingStatusOption.Equals(AppStringResources.Reading))
            {
                filterList = bookList?
                    .Where(x => (x.BookPageRead != x.BookPageTotal && x.BookPageRead != 0) ||
                    (x.BookHourListened != x.BookHoursTotal && x.BookMinuteListened != x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0) ||
                    (!string.IsNullOrEmpty(x.BookStartDate) && string.IsNullOrEmpty(x.BookEndDate)))
                    .ToObservableCollection();
            }

            if (readingStatusOption.Equals(AppStringResources.Read))
            {
                filterList = bookList?
                    .Where(x => (x.BookPageRead == x.BookPageTotal && x.BookPageRead != 0) ||
                    (x.BookHourListened == x.BookHoursTotal && x.BookMinuteListened == x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0) ||
                    (!string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate)))
                    .ToObservableCollection();
            }

            return filterList;
        }

        /********************************************************/

        private static ObservableCollection<BookModel>? FilterBooksOnLoanedOutStatus(ObservableCollection<BookModel>? bookList, string loanedOutStatus)
        {
            var filterList = bookList;

            if (loanedOutStatus.Equals(AppStringResources.LoanedOut))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookLoanedOutOn) || !string.IsNullOrEmpty(x.LoanedTo)).ToObservableCollection();
            }

            if (loanedOutStatus.Equals(AppStringResources.NotLoanedOut))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookLoanedOutOn) && string.IsNullOrEmpty(x.LoanedTo)).ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel>? FilterBooksOnBorrowedStatus(ObservableCollection<BookModel>? bookList, string borrowedStatus)
        {
            var filterList = bookList;

            if (borrowedStatus.Equals(AppStringResources.Borrowed))
            {
                filterList = bookList?.Where(x => !string.IsNullOrEmpty(x.BookBorrowedOn) || !string.IsNullOrEmpty(x.BorrowedFrom)).ToObservableCollection();
            }

            if (borrowedStatus.Equals(AppStringResources.NotBorrowed))
            {
                filterList = bookList?.Where(x => string.IsNullOrEmpty(x.BookBorrowedOn) && string.IsNullOrEmpty(x.BorrowedFrom)).ToObservableCollection();
            }

            return filterList;
        }
    }
}
