// <copyright file="GetCounts.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Main;
using CommunityToolkit.Maui.Core.Extensions;
using DocumentFormat.OpenXml.Bibliography;
using System.Collections.ObjectModel;
using System.Globalization;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookCollector.Data
{
    public partial class GetCounts : BaseViewModel
    {
        public static async Task<int> GetReadingBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetReadingBooks(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllReadingBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            var count = filteredList != null ? filteredList.Count : 0;
            return count;
        }

        public static async Task<int> GetToBeReadBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetToBeReadBooks(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllToBeReadBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetReadBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetReadBooks(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllReadBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetAllBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBooks(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetAllWishListBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? filteredList;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllWishlistBooks(showHiddenBooks);
            }
            else
            {
                filteredList = WishListViewModel.fullWishlistBookList?
                    .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                    .ToObservableCollection();
            }

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetBooksListCountByFavorite(bool showHiddenBooks, bool favoriteValue)
        {
            ObservableCollection<BookModel>? filteredList;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetBooksListByFavorite(showHiddenBooks, favoriteValue);
            }
            else
            {
                filteredList = AllBooksViewModel.fullBookList?
                    .Where(x => x.IsFavorite == favoriteValue)
                    .ToObservableCollection();
            }

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetBooksListCountByRating(bool showHiddenBooks, int starRating)
        {
            ObservableCollection<BookModel>? filteredList;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetBooksListByRating(showHiddenBooks, starRating);
            }
            else
            {
                filteredList = AllBooksViewModel.fullBookList?
                    .Where(x => x.Rating == starRating)
                    .ToObservableCollection();
            }

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<double> GetAllBookPricesInCollectionList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBookPricesInCollectionList(inputGuid, showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksInCollectionAsync((Guid)inputGuid, showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return price;
        }

        public static async Task<double> GetAllBookPricesInGenreList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBookPricesInGenreList(inputGuid, showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksInGenreAsync((Guid)inputGuid, showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return price;
        }

        public static async Task<double> GetAllBookPricesInSeriesList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBookPricesInSeriesList(inputGuid, showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksInSeriesAsync((Guid)inputGuid, showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return price;
        }

        public static async Task<double> GetAllBookPricesInLocationList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBookPricesInLocationList(inputGuid, showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksInLocationAsync((Guid)inputGuid, showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return price;
        }

        public static async Task<List<CountModel>> GetAllBooksAndBookFormatsList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.GetAllBooks(showHiddenBooks);
            }
            else
            {
                bookList = AllBooksViewModel.fullBookList?
                    .ToObservableCollection();
            }

            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            if (formatList != null && bookList != null)
            {
                for (int i = 0; i < formatList.Count; i++)
                {
                    var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) &&
                                                    x.BookFormat.Equals(formatList[i])).ToObservableCollection();

                    var count = filteredList.Count;

                    counts.Add(new CountModel()
                    {
                        Label = formatList[i],
                        Count = count,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetPriceOfBooksAndBookFormatsList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.GetAllBooks(showHiddenBooks);
            }
            else
            {
                bookList = AllBooksViewModel.fullBookList?
                    .ToObservableCollection();
            }

            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            if (formatList != null && bookList != null)
            {
                for (int i = 0; i < formatList.Count; i++)
                {
                    var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) &&
                                                   x.BookFormat.Equals(formatList[i])).ToObservableCollection();

                    var price = filteredList.Sum(x => x.BookPriceValue);

                    counts.Add(new CountModel()
                    {
                        Label = formatList[i],
                        CountDouble = price,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndBookFormatsList(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.GetAllWishlistBooks(showHiddenBooks);
            }
            else
            {
                bookList = WishListViewModel.fullWishlistBookList?
                    .ToObservableCollection();
            }

            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            if (formatList != null && bookList != null)
            {
                for (int i = 0; i < formatList.Count; i++)
                {
                    var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) &&
                                                    x.BookFormat.Equals(formatList[i])).ToObservableCollection();

                    var count = filteredList.Count;

                    counts.Add(new CountModel()
                    {
                        Label = formatList[i],
                        Count = count,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetPriceOfWishListBooksAndBookFormatsList(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.GetAllWishlistBooks(showHiddenBooks);
            }
            else
            {
                bookList = WishListViewModel.fullWishlistBookList?
                    .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                    .ToObservableCollection();
            }

            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            if (formatList != null && bookList != null)
            {
                for (int i = 0; i < formatList.Count; i++)
                {
                    var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) &&
                                                   x.BookFormat.Equals(formatList[i])).ToObservableCollection();

                    var price = filteredList.Sum(x => x.BookPriceValue);

                    counts.Add(new CountModel()
                    {
                        Label = formatList[i],
                        CountDouble = price,
                    });
                }
            }

            return counts;
        }

        public static async Task<int> GetBookCountReadInYear(int year, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBooksReadInYear(year, showHiddenBooks);
            }
            else
            {
                filteredList = AllBooksViewModel.fullBookList?
                    .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year)
                    .ToObservableCollection();
            }

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetBookPageCountReadInYear(int year, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBooksReadInYear(year, showHiddenBooks);
            }
            else
            {
                filteredList = AllBooksViewModel.fullBookList?
                    .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year)
                    .ToObservableCollection();
            }

            if (filteredList != null)
            {
                count = filteredList.Sum(x => x.BookPageTotal);
            }

            return count;
        }

        public static async Task<string> GetPriceOfAllBooks(bool showHiddenBooks)
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
            var cultureInfo = new CultureInfo(cultureCode);

            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBooksWithPrices(showHiddenBooks);
            }
            else
            {
                filteredList = AllBooksViewModel.fullBookList?
                    .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                    .ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return string.Format(cultureInfo, "{0:C}", price);
        }

        public static async Task<string> GetPriceOfAllWishListBooks(bool showHiddenBooks)
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
            var cultureInfo = new CultureInfo(cultureCode);

            ObservableCollection<WishlistBookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllWishlistBooksWithPrices(showHiddenBooks);
            }
            else
            {
                filteredList = WishListViewModel.fullWishlistBookList?
                    .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                    .ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return string.Format(cultureInfo, "{0:C}", price);
        }

        public static async Task<double> GetAllBookPricesInAuthorList(Guid? inputGuid, bool showHiddenBooks)
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
            var cultureInfo = new CultureInfo(cultureCode);

            var filteredList = new ObservableCollection<BookModel>();
            var price = 0.0;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBookPricesInAuthorList(inputGuid, showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksForAuthorAsync((Guid)inputGuid, showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return price;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllAuthorsList(bool showHiddenAuthors, bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<AuthorModel>? filteredList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllAuthorsWithBooks(showHiddenAuthors);
                filteredBookList = TestData.GetAllBooksWithoutAuthorsList(showHiddenBooks);
            }
            else
            {
                filteredList = AuthorsViewModel.fullAuthorList;
            }

            var counts = new List<CountModel>();

            if (filteredList != null)
            {
                filteredList = [.. filteredList.OrderByDescending(x => x.AuthorTotalBooks)];

                var max = maxLimit;

                if (filteredList.Count < max)
                {
                    max = filteredList.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    counts.Add(new CountModel()
                    {
                        Label = filteredList[i].FullName,
                        Count = filteredList[i].AuthorTotalBooks,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllCollectionsList(bool showHiddenCollections, bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<CollectionModel>? filteredList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllCollectionsWithBooks(showHiddenCollections);
                filteredBookList = TestData.GetAllBooksWithoutCollections(showHiddenBooks);
            }
            else
            {
                filteredList = CollectionsViewModel.fullCollectionList;
            }

            var counts = new List<CountModel>();

            if (filteredList != null)
            {
                filteredList = [.. filteredList.OrderByDescending(x => x.CollectionTotalBooks)];

                var max = maxLimit;

                if (filteredList.Count < max)
                {
                    max = filteredList.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    counts.Add(new CountModel()
                    {
                        Label = filteredList[i].CollectionName,
                        Count = filteredList[i].CollectionTotalBooks,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllGenresList(bool showHiddenGenres, bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<GenreModel>? filteredList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllGenresWithBooks(showHiddenGenres);
                filteredBookList = TestData.GetAllBooksWithoutGenres(showHiddenBooks);
            }
            else
            {
                filteredList = GenresViewModel.fullGenreList;
            }

            var counts = new List<CountModel>();

            if (filteredList != null)
            {
                filteredList = [.. filteredList.OrderByDescending(x => x.GenreTotalBooks)];

                var max = maxLimit;

                if (filteredList.Count < max)
                {
                    max = filteredList.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    counts.Add(new CountModel()
                    {
                        Label = filteredList[i].GenreName,
                        Count = filteredList[i].GenreTotalBooks,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllSeriesList(bool showHiddenSeries, bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<SeriesModel>? filteredList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllSeriesWithBooks(showHiddenSeries);
                filteredBookList = TestData.GetAllBooksWithoutSeries(showHiddenBooks);
            }
            else
            {
                filteredList = SeriesViewModel.fullSeriesList;
            }

            var counts = new List<CountModel>();

            if (filteredList != null)
            {
                filteredList = [.. filteredList.OrderByDescending(x => x.SeriesTotalBooks)];

                var max = maxLimit;

                if (filteredList.Count < max)
                {
                    max = filteredList.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    counts.Add(new CountModel()
                    {
                        Label = filteredList[i].SeriesName,
                        Count = filteredList[i].SeriesTotalBooks,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllLocationsList(bool showHiddenLocations, bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<LocationModel>? filteredList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllLocationsWithBooks(showHiddenLocations);
                filteredBookList = TestData.GetAllBooksWithoutLocations(showHiddenBooks);
            }
            else
            {
                filteredList = LocationsViewModel.fullLocationList;
            }

            var counts = new List<CountModel>();

            if (filteredList != null)
            {
                filteredList = [.. filteredList.OrderByDescending(x => x.LocationTotalBooks)];

                var max = maxLimit;

                if (filteredList.Count < max)
                {
                    max = filteredList.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    counts.Add(new CountModel()
                    {
                        Label = filteredList[i].LocationName,
                        Count = filteredList[i].LocationTotalBooks,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndLocationList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<WishlistBookModel>? filteredList1 = null;
            ObservableCollection<WishlistBookModel>? filteredList2 = null;
            List<string?>? list = null;

            if (TestData.UseTestData)
            {
                filteredList1 = TestData.GetAllWishlistBooksWithLocations(showHiddenBooks);
                filteredList2 = TestData.GetAllWishlistBooksWithoutLocations(showHiddenBooks);
                list = TestData.GetAllWishlistBookLocations(showHiddenBooks);
            }
            else
            {
                filteredList1 = WishListViewModel.fullWishlistBookList?
                    .Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy))
                    .ToObservableCollection();

                list = [.. filteredList1!
                    .Select(x => x.BookWhereToBuy)
                    .Distinct()];
            }

            var counts = new List<CountModel>();

            if (filteredList1 != null && list != null)
            {
                var max = maxLimit;

                if (list.Count < max)
                {
                    max = list.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    var count = filteredList1.Count(x => !string.IsNullOrEmpty(x.BookWhereToBuy) && x.BookWhereToBuy.Equals(list[i]));

                    counts.Add(new CountModel()
                    {
                        Label = list[i],
                        Count = count,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndSeriesList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<WishlistBookModel>? filteredList1 = null;
            ObservableCollection<WishlistBookModel>? filteredList2 = null;
            List<string?>? list = null;

            if (TestData.UseTestData)
            {
                filteredList1 = TestData.GetAllWishlistBooksWithSeries(showHiddenBooks);
                filteredList2 = TestData.GetAllWishlistBooksWithoutSeries(showHiddenBooks);
                list = TestData.GetAllWishlistBookSeries(showHiddenBooks);
            }
            else
            {
                filteredList1 = WishListViewModel.fullWishlistBookList?
                    .Where(x => !string.IsNullOrEmpty(x.BookSeries))
                    .ToObservableCollection();

                list = [.. filteredList1!
                    .Select(x => x.BookSeries)
                    .Distinct()];
            }

            var counts = new List<CountModel>();

            if (filteredList1 != null && list != null)
            {
                var max = maxLimit;

                if (list.Count < max)
                {
                    max = list.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    var count = filteredList1.Count(x => !string.IsNullOrEmpty(x.BookSeries) && x.BookSeries.Equals(list[i]));

                    counts.Add(new CountModel()
                    {
                        Label = list[i],
                        Count = count,
                    });
                }
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndAuthorList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<WishlistBookModel>? filteredList1 = null;
            ObservableCollection<WishlistBookModel>? filteredList2 = null;
            List<string?>? authorStringList = null;

            if (TestData.UseTestData)
            {
                filteredList1 = TestData.GetAllWishlistBooksWithAuthors(showHiddenBooks);
                filteredList2 = TestData.GetAllWishlistBooksWithoutAuthors(showHiddenBooks);
                authorStringList = TestData.GetAllWishlistBookAuthors(showHiddenBooks);
            }
            else
            {
                filteredList1 = WishListViewModel.fullWishlistBookList?
                    .Where(x => !string.IsNullOrEmpty(x.AuthorListString))
                    .ToObservableCollection();

                authorStringList = [.. filteredList1!
                    .Select(x => x.AuthorListString)
                    .Distinct()];
            }

            var counts = new List<CountModel>();

            if (filteredList1 != null && authorStringList != null)
            {
                var list = new List<AuthorModel>();

                foreach (var authorString in authorStringList)
                {
                    if (!string.IsNullOrEmpty(authorString))
                    {
                        list = SplitStringIntoAuthorList(authorString);
                    }
                }

                list = [.. list.DistinctBy(x => x.FullName)];

                var max = maxLimit;

                if (list.Count < max)
                {
                    max = list.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    var filteredBookList = filteredList1.Where(x => !string.IsNullOrEmpty(x.AuthorListString) &&
                                                    x.AuthorListString.Contains(list[i].ReverseFullName)).ToObservableCollection();

                    var count = filteredBookList.Count;

                    counts.Add(new CountModel()
                    {
                        Label = list[i].FullName,
                        Count = count,
                    });
                }
            }

            return counts;
        }
    }
}
