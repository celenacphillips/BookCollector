// <copyright file="SortLists.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data.Models;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;

namespace BookCollector.Data
{
    public partial class SortLists : BaseViewModel
    {
        public static async Task<ObservableCollection<BookModel>> SortBookList(
            ObservableCollection<BookModel> bookList,
            bool bookTitleChecked,
            bool bookReadingDateChecked,
            bool bookReadPercentageChecked,
            bool bookPublisherChecked,
            bool bookPublishYearChecked,
            bool authorLastNameChecked,
            bool bookFormatChecked,
            bool bookPriceChecked,
            bool pageCountChecked,
            bool ascendingChecked,
            bool descendingChecked,
            bool seriesOrderChecked = false)
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
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.StartDateValue).OrderBy(x => x.EndDateValue).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.StartDateValue).OrderByDescending(x => x.EndDateValue).ToObservableCollection();
                }
            }

            if (bookReadPercentageChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.Progress).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.Progress).ToObservableCollection();
                }
            }

            if (bookPublisherChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPublisher).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPublisher).ToObservableCollection();
                }
            }

            if (bookPublishYearChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPublishYear).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPublishYear).ToObservableCollection();
                }
            }

            if (authorLastNameChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.AuthorListString).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.AuthorListString).ToObservableCollection();
                }
            }

            if (bookFormatChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookFormat).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookFormat).ToObservableCollection();
                }
            }

            if (bookPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPrice).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPrice).ToObservableCollection();
                }
            }

            if (seriesOrderChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookNumberInSeries).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookNumberInSeries).ToObservableCollection();
                }
            }

            if (pageCountChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPageTotal).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPageTotal).ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<CollectionModel>> SortCollectionsList(
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
                    filteredList = filteredList.OrderBy(x => x.ParsedCollectionName).OrderBy(x => x.CollectionTotalBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedCollectionName).OrderByDescending(x => x.CollectionTotalBooks).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedCollectionName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedCollectionName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<GenreModel>> SortGenresList(
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
                    filteredList = filteredList.OrderBy(x => x.ParsedGenreName).OrderBy(x => x.GenreTotalBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedGenreName).OrderByDescending(x => x.GenreTotalBooks).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedGenreName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedGenreName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<SeriesModel>> SortSeriesList(
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
                    filteredList = filteredList.OrderBy(x => x.ParsedSeriesName).OrderBy(x => x.SeriesTotalBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedSeriesName).OrderByDescending(x => x.SeriesTotalBooks).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedSeriesName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedSeriesName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<LocationModel>> SortLocationsList(
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
                    filteredList = filteredList.OrderBy(x => x.ParsedLocationName).OrderBy(x => x.LocationTotalBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedLocationName).OrderByDescending(x => x.LocationTotalBooks).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedLocationName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedLocationName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<AuthorModel>> SortAuthorList(
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
                    filteredList = filteredList.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.FirstName).OrderByDescending(x => x.LastName).ToObservableCollection();
                }
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).OrderBy(x => x.AuthorTotalBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.FirstName).OrderByDescending(x => x.LastName).OrderByDescending(x => x.AuthorTotalBooks).ToObservableCollection();
                }
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.FirstName).OrderByDescending(x => x.LastName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<WishlistBookModel>> SortWishlistBookList(
            ObservableCollection<WishlistBookModel> bookList,
            bool bookTitleChecked,
            bool bookReadingDateChecked,
            bool bookReadPercentageChecked,
            bool bookPublisherChecked,
            bool bookPublishYearChecked,
            bool authorLastNameChecked,
            bool bookFormatChecked,
            bool bookPriceChecked,
            bool pageCountChecked,
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
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.StartDateValue).OrderBy(x => x.EndDateValue).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.StartDateValue).OrderByDescending(x => x.EndDateValue).ToObservableCollection();
                }
            }

            if (bookReadPercentageChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.Progress).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.Progress).ToObservableCollection();
                }
            }

            if (bookPublisherChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPublisher).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPublisher).ToObservableCollection();
                }
            }

            if (bookPublishYearChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPublishYear).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPublishYear).ToObservableCollection();
                }
            }

            if (authorLastNameChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.AuthorListString).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.AuthorListString).ToObservableCollection();
                }
            }

            if (bookFormatChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookFormat).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookFormat).ToObservableCollection();
                }
            }

            if (bookPriceChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPrice).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPrice).ToObservableCollection();
                }
            }

            if (pageCountChecked)
            {
                if (ascendingChecked)
                {
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPageTotal).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPageTotal).ToObservableCollection();
                }
            }

            return filteredList;
        }
    }
}
