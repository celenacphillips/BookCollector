// <copyright file="FilterLists.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;

namespace BookCollector.Data
{
    public partial class FilterLists : BookBaseViewModel
    {
        public static async Task<ObservableCollection<BookModel>> FilterBookList(
            ObservableCollection<BookModel> bookList,
            string? favoritesOption,
            string? formatOption,
            string? publisherOption,
            string? languageOption,
            string? ratingOption,
            string? publishYearOption,
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

            return filteredList;
        }

        public static async Task<ObservableCollection<WishlistBookModel>> FilterWishlistBookList(
            ObservableCollection<WishlistBookModel> bookList,
            string? formatOption,
            string? publisherOption,
            string? languageOption,
            string? publishYearOption,
            string? authorOption,
            string? locationOption,
            string? seriesOption)
        {
            var filteredList = bookList;

            filteredList = FilterBookFormat_Wishlist(filteredList, formatOption);

            filteredList = FilterBookPublisher_Wishlist(filteredList, publisherOption);

            filteredList = FilterBookLanguage_Wishlist(filteredList, languageOption);

            filteredList = FilterBookPublishYear_Wishlist(filteredList, publishYearOption);

            filteredList = FilterBookAuthor(filteredList, authorOption);

            filteredList = FilterBookLocation(filteredList, locationOption);

            filteredList = FilterBookSeries(filteredList, seriesOption);

            return filteredList;
        }

        public static ObservableCollection<BookModel> FilterOnSearchString(ObservableCollection<BookModel> bookList, string? searchString)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(searchString))
            {
                filterList = filterList.Where(x => !string.IsNullOrEmpty(x.BookTitle) && x.BookTitle.Contains(searchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterFavoriteBooks(ObservableCollection<BookModel> bookList, string favoritesOption)
        {
            var filterList = bookList;

            filterList = favoritesOption switch
            {
                "Favorites" => bookList.Where(x => x.IsFavorite)
                                                         .ToObservableCollection(),
                "Non-Favorites" => bookList.Where(x => !x.IsFavorite)
                                                         .ToObservableCollection(),
                _ => bookList,
            };

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookFormat(ObservableCollection<BookModel> bookList, string? formatOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(formatOption) && !formatOption.Equals(AppStringResources.AllFormats))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) && x.BookFormat.Equals(formatOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel> FilterBookFormat_Wishlist(ObservableCollection<WishlistBookModel> bookList, string? formatOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(formatOption) && !formatOption.Equals(AppStringResources.AllFormats))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) && x.BookFormat.Equals(formatOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookPublisher(ObservableCollection<BookModel> bookList, string? publisherOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(publisherOption) && publisherOption.Equals(AppStringResources.NoPublisher))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookPublisher))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(publisherOption) && !publisherOption.Equals(AppStringResources.NoPublisher) && !publisherOption.Equals(AppStringResources.AllPublishers))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookPublisher) && x.BookPublisher.Equals(publisherOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel> FilterBookPublisher_Wishlist(ObservableCollection<WishlistBookModel> bookList, string? publisherOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(publisherOption) && publisherOption.Equals(AppStringResources.NoPublisher))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookPublisher))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(publisherOption) && !publisherOption.Equals(AppStringResources.NoPublisher) && !publisherOption.Equals(AppStringResources.AllPublishers))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookPublisher) && x.BookPublisher.Equals(publisherOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookPublishYear(ObservableCollection<BookModel> bookList, string? publishYearOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(publishYearOption) && publishYearOption.Equals(AppStringResources.NoPublishYear))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(publishYearOption) && !publishYearOption.Equals(AppStringResources.NoPublishYear) && !publishYearOption.Equals(AppStringResources.AllPublishYears))
            {
                var years = publishYearOption.Split(" - ");

                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookPublishYear) &&
                                            int.Parse(years[0]) <= int.Parse(x.BookPublishYear) &&
                                            int.Parse(years[1]) >= int.Parse(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel> FilterBookPublishYear_Wishlist(ObservableCollection<WishlistBookModel> bookList, string? publishYearOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(publishYearOption) && publishYearOption.Equals(AppStringResources.NoPublishYear))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(publishYearOption) && !publishYearOption.Equals(AppStringResources.NoPublishYear) && !publishYearOption.Equals(AppStringResources.AllPublishYears))
            {
                var years = publishYearOption.Split(" - ");

                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookPublishYear) &&
                                            int.Parse(years[0]) <= int.Parse(x.BookPublishYear) &&
                                            int.Parse(years[1]) >= int.Parse(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookLanguage(ObservableCollection<BookModel> bookList,  string? languageOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(languageOption) && languageOption.Equals(AppStringResources.NoLanguage))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookLanguage))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(languageOption) && !languageOption.Equals(AppStringResources.NoLanguage) && !languageOption.Equals(AppStringResources.AllLanguages))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookLanguage) && x.BookLanguage.Equals(languageOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel> FilterBookLanguage_Wishlist(ObservableCollection<WishlistBookModel> bookList, string? languageOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(languageOption) && languageOption.Equals(AppStringResources.NoLanguage))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookLanguage))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(languageOption) && !languageOption.Equals(AppStringResources.NoLanguage) && !languageOption.Equals(AppStringResources.AllLanguages))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookLanguage) && x.BookLanguage.Equals(languageOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookRating(ObservableCollection<BookModel> bookList, string ratingOption)
        {
            var filterList = bookList;

            if (!ratingOption.Equals(AppStringResources.AllRatings))
            {
                filterList = bookList.Where(x => x.Rating == int.Parse(ratingOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel> FilterBookAuthor(ObservableCollection<WishlistBookModel> bookList, string? authorOption)
        {
            var filterList = bookList;
            var newFilteredList = new ObservableCollection<WishlistBookModel>();

            if (!string.IsNullOrEmpty(authorOption) && authorOption.Equals(AppStringResources.NoAuthor))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.AuthorListString))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(authorOption) && !authorOption.Equals(AppStringResources.NoAuthor) && !authorOption.Equals(AppStringResources.AllAuthors))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.AuthorListString))
                                     .ToObservableCollection();

                foreach (var book in filterList)
                {
                    if (!string.IsNullOrEmpty(book.AuthorListString))
                    {
                        var list = SplitStringIntoAuthorList(book.AuthorListString);

                        foreach (var author in list)
                        {
                            if (author.FullName.Equals(authorOption))
                            {
                                newFilteredList.Add(book);
                            }
                        }
                    }
                }

                filterList = newFilteredList.Distinct().ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel> FilterBookLocation(ObservableCollection<WishlistBookModel> bookList, string? locationOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(locationOption) && locationOption.Equals(AppStringResources.NoLocation))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookWhereToBuy))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(locationOption) && !locationOption.Equals(AppStringResources.NoLocation) && !locationOption.Equals(AppStringResources.AllLocations))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy) && x.BookWhereToBuy.Equals(locationOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<WishlistBookModel> FilterBookSeries(ObservableCollection<WishlistBookModel> bookList, string? seriesOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(seriesOption) && seriesOption.Equals(AppStringResources.NoSeries))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookSeries))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(seriesOption) && !seriesOption.Equals(AppStringResources.NoSeries) && !seriesOption.Equals(AppStringResources.AllSeries))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookSeries) && x.BookSeries.Equals(seriesOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }
    }
}
