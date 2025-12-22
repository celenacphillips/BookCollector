// <copyright file="GetCounts.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Globalization;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;

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
                var list = await Database.GetAllWishlistBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
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
                var list = await Database.GetBooksByFavoriteAsync(showHiddenBooks, favoriteValue);
                filteredList = list.ToObservableCollection();
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
                var list = await Database.GetBooksByRatingAsync(showHiddenBooks, starRating);
                filteredList = list.ToObservableCollection();
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
                var list = await Database.GetAllBooksAsync(showHiddenBooks);
                bookList = list.ToObservableCollection();
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
                var list = await Database.GetAllBooksAsync(showHiddenBooks);
                bookList = list.ToObservableCollection();
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
                var list = await Database.GetAllWishlistBooksAsync(showHiddenBooks);
                bookList = list.ToObservableCollection();
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
                var list = await Database.GetAllWishlistBooksAsync(showHiddenBooks);
                bookList = list.ToObservableCollection();
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
                var list = await Database.GetBooksReadInYearAsync(year, showHiddenBooks);
                filteredList = list.ToObservableCollection();
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
                var list = await Database.GetBooksReadInYearAsync(year, showHiddenBooks);
                filteredList = list.ToObservableCollection();
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
                var list = await Database.GetAllBooksWithAPriceAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
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
                var list = await Database.GetAllWishlistBooksWithAPriceAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
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
                var list1 = await Database.GetAllAuthorsAsync(showHiddenAuthors);
                filteredList = list1.ToObservableCollection();

                var list2 = await Database.GetAllBooksWithoutAnAuthorAsync(showHiddenBooks);
                filteredBookList = list2.ToObservableCollection();
            }

            var counts = new List<CountModel>();

            if (filteredList != null && filteredBookList != null)
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

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoAuthor,
                    Count = filteredBookList.Count(x => string.IsNullOrEmpty(x.AuthorListString)),
                });
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
                var list1 = await Database.GetAllCollectionsAsync(showHiddenCollections);
                filteredList = list1.ToObservableCollection();

                var list2 = await Database.GetAllBooksWithoutACollectionAsync(showHiddenBooks);
                filteredBookList = list2.ToObservableCollection();
            }

            var counts = new List<CountModel>();

            if (filteredList != null && filteredBookList != null)
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

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoCollection,
                    Count = filteredBookList.Count(x => x.BookCollectionGuid == null),
                });
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
                var list1 = await Database.GetAllGenresAsync(showHiddenGenres);
                filteredList = list1.ToObservableCollection();

                var list2 = await Database.GetAllBooksWithoutAGenreAsync(showHiddenBooks);
                filteredBookList = list2.ToObservableCollection();
            }

            var counts = new List<CountModel>();

            if (filteredList != null && filteredBookList != null)
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

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoGenre,
                    Count = filteredBookList.Count(x => x.BookGenreGuid == null),
                });
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
                var list1 = await Database.GetAllSeriesAsync(showHiddenSeries);
                filteredList = list1.ToObservableCollection();

                var list2 = await Database.GetAllBooksWithoutASeriesAsync(showHiddenBooks);
                filteredBookList = list2.ToObservableCollection();
            }

            var counts = new List<CountModel>();

            if (filteredList != null && filteredBookList != null)
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

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoSeries,
                    Count = filteredBookList.Count(x => x.BookSeriesGuid == null),
                });
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
                var list1 = await Database.GetAllLocationsAsync(showHiddenLocations);
                filteredList = list1.ToObservableCollection();

                var list2 = await Database.GetAllBooksWithoutALocationAsync(showHiddenBooks);
                filteredBookList = list2.ToObservableCollection();
            }

            var counts = new List<CountModel>();

            if (filteredList != null && filteredBookList != null)
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

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoLocation,
                    Count = filteredBookList.Count(x => x.BookLocationGuid == null),
                });
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
                var list1 = await Database.GetAllWishlistBooksWithALocationAsync(showHiddenBooks);
                filteredList1 = list1.ToObservableCollection();

                var list2 = await Database.GetAllWishlistBooksWithoutALocationAsync(showHiddenBooks);
                filteredList2 = list2.ToObservableCollection();

                list = await Database.GetAllWishlistBooksLocationsAsync(showHiddenBooks);
            }

            var counts = new List<CountModel>();

            if (filteredList1 != null && list != null && filteredList2 != null)
            {
                // filteredList1 = [.. filteredList1.OrderByDescending(x => x.CollectionTotalBooks)];

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

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoLocation,
                    Count = filteredList2.Count,
                });
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
                var list1 = await Database.GetAllWishlistBooksWithASeriesAsync(showHiddenBooks);
                filteredList1 = list1.ToObservableCollection();

                var list2 = await Database.GetAllWishlistBooksWithoutASeriesAsync(showHiddenBooks);
                filteredList2 = list2.ToObservableCollection();

                list = await Database.GetAllWishlistBooksSeriesAsync(showHiddenBooks);
            }

            var counts = new List<CountModel>();

            if (filteredList1 != null && list != null && filteredList2 != null)
            {
                // filteredList = [.. filteredList.OrderByDescending(x => x.CollectionTotalBooks)];

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

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoSeries,
                    Count = filteredList2.Count,
                });
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
                var list1 = await Database.GetAllWishlistBooksWithAuthorsAsync(showHiddenBooks);
                filteredList1 = list1.ToObservableCollection();

                var list2 = await Database.GetAllWishlistBooksWithoutAuthorsAsync(showHiddenBooks);
                filteredList2 = list2.ToObservableCollection();

                authorStringList = await Database.GetAllWishlistBooksAuthorsAsync(showHiddenBooks);
            }

            var counts = new List<CountModel>();

            if (filteredList1 != null && authorStringList != null && filteredList2 != null)
            {
                // filteredList = [.. filteredList.OrderByDescending(x => x.CollectionTotalBooks)];

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

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoAuthor,
                    Count = filteredList2.Count,
                });
            }

            return counts;
        }
    }
}
