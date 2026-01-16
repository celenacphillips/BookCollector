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
        public static async Task<int> GetBooksListCountByFavorite(bool showHiddenBooks, bool favoriteValue)
        {
            ObservableCollection<BookModel>? filteredList;

            filteredList = AllBooksViewModel.filteredBookList1?
                .Where(x => x.IsFavorite == favoriteValue)
                .ToObservableCollection();

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetBooksListCountByRating(bool showHiddenBooks, int starRating)
        {
            ObservableCollection<BookModel>? filteredList;

            filteredList = AllBooksViewModel.filteredBookList1?
                .Where(x => x.Rating == starRating)
                .ToObservableCollection();

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<double> GetAllBookPricesInCollectionList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (AllBooksViewModel.filteredBookList1 != null)
            {
                filteredList = AllBooksViewModel.filteredBookList1
                    .Where(x => x.BookCollectionGuid == inputGuid)
                    .ToObservableCollection();
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

            if (AllBooksViewModel.filteredBookList1 != null)
            {
                filteredList = AllBooksViewModel.filteredBookList1
                    .Where(x => x.BookGenreGuid == inputGuid)
                    .ToObservableCollection();
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

            if (AllBooksViewModel.filteredBookList1 != null)
            {
                filteredList = AllBooksViewModel.filteredBookList1
                    .Where(x => x.BookSeriesGuid == inputGuid)
                    .ToObservableCollection();
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

            if (AllBooksViewModel.filteredBookList1 != null)
            {
                filteredList = AllBooksViewModel.filteredBookList1
                    .Where(x => x.BookLocationGuid == inputGuid)
                    .ToObservableCollection();
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

            bookList = AllBooksViewModel.filteredBookList1?
                .ToObservableCollection();

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

            bookList = AllBooksViewModel.filteredBookList1?
                .ToObservableCollection();

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

            bookList = WishListViewModel.filteredWishlistBookList1?
                .ToObservableCollection();

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

            bookList = WishListViewModel.filteredWishlistBookList1?
                .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                .ToObservableCollection();

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

            filteredList = AllBooksViewModel.filteredBookList1?
                .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year)
                .ToObservableCollection();

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetBookPageCountReadInYear(int year, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            filteredList = AllBooksViewModel.filteredBookList1?
                .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year &&
                            x.BookPageTotal != null)
                .ToObservableCollection();

            if (filteredList != null)
            {
                count = filteredList.Sum(x => (int)x.BookPageTotal);
            }

            return count;
        }

        public static async Task<double> GetBookTimeCountReadInYear(int year, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0.0;

            filteredList = AllBooksViewModel.filteredBookList1?
                .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year &&
                            x.BookHoursTotal != 0 && x.BookMinutesTotal != 0)
                .ToObservableCollection();

            if (filteredList != null)
            {
                filteredList.ToList().ForEach(x => x.SetBookTotalTime());

                count = filteredList.Sum(x => (double)x.BookTotalTime);
            }

            return count;
        }

        public static async Task<string> GetPriceOfAllBooks(bool showHiddenBooks)
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
            var cultureInfo = new CultureInfo(cultureCode);

            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            filteredList = AllBooksViewModel.filteredBookList1?
                .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                .ToObservableCollection();

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

            filteredList = WishListViewModel.filteredWishlistBookList1?
                .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                .ToObservableCollection();

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

            var list = await Database.GetAllBooksForAuthorAsync((Guid)inputGuid, showHiddenBooks);
            filteredList = list.ToObservableCollection();

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

            await Task.WhenAll(AuthorsViewModel.filteredAuthorList1?.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = AuthorsViewModel.filteredAuthorList1?.OrderByDescending(x => x.FirstName)?.OrderByDescending(x => x.LastName).ToObservableCollection();

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

            await Task.WhenAll(CollectionsViewModel.filteredCollectionList1?.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = CollectionsViewModel.filteredCollectionList1?.OrderByDescending(x => x.ParsedCollectionName).ToObservableCollection();

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

            await Task.WhenAll(GenresViewModel.filteredGenreList1?.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = GenresViewModel.filteredGenreList1?.OrderByDescending(x => x.ParsedGenreName).ToObservableCollection();

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

            await Task.WhenAll(SeriesBaseViewModel.filteredSeriesList1?.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = SeriesViewModel.filteredSeriesList1?.OrderByDescending(x => x.ParsedSeriesName).ToObservableCollection();

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

            await Task.WhenAll(LocationsViewModel.filteredLocationList1?.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = LocationsViewModel.filteredLocationList1?.OrderByDescending(x => x.ParsedLocationName).ToObservableCollection();

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

            filteredList1 = WishListViewModel.filteredWishlistBookList1?
                .Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy))
                .ToObservableCollection();

            list = [.. filteredList1!
                .Select(x => x.BookWhereToBuy)
                .Distinct()];

            var counts = new List<CountModel>();

            if (filteredList1 != null && list != null)
            {
                list = [.. list.OrderByDescending(x => x)];

                for (int i = 0; i < list.Count; i++)
                {
                    var count = filteredList1.Count(x => !string.IsNullOrEmpty(x.BookWhereToBuy) && x.BookWhereToBuy.Equals(list[i]));

                    counts.Add(new CountModel()
                    {
                        Label = list[i],
                        Count = count,
                    });
                }

                counts = [.. counts.OrderByDescending(x => x.Count)];

                var max = maxLimit;

                if (list.Count < max)
                {
                    max = list.Count;
                }

                counts.RemoveRange(max, list.Count - max);
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndSeriesList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<WishlistBookModel>? filteredList1 = null;
            ObservableCollection<WishlistBookModel>? filteredList2 = null;
            List<string?>? list = null;

            filteredList1 = WishListViewModel.filteredWishlistBookList1?
                .Where(x => !string.IsNullOrEmpty(x.BookSeries))
                .ToObservableCollection();

            list = [.. filteredList1!
                .Select(x => x.BookSeries)
                .Distinct()];

            var counts = new List<CountModel>();

            if (filteredList1 != null && list != null)
            {
                list = [.. list.OrderByDescending(x => x)];

                for (int i = 0; i < list.Count; i++)
                {
                    var count = filteredList1.Count(x => !string.IsNullOrEmpty(x.BookSeries) && x.BookSeries.Equals(list[i]));

                    counts.Add(new CountModel()
                    {
                        Label = list[i],
                        Count = count,
                    });
                }

                counts = [.. counts.OrderByDescending(x => x.Count)];

                var max = maxLimit;

                if (list.Count < max)
                {
                    max = list.Count;
                }

                counts.RemoveRange(max, list.Count - max);
            }

            return counts;
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndAuthorList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<WishlistBookModel>? filteredList1 = null;
            ObservableCollection<WishlistBookModel>? filteredList2 = null;
            List<string?>? authorStringList = null;

            filteredList1 = WishListViewModel.filteredWishlistBookList1?
                .Where(x => !string.IsNullOrEmpty(x.AuthorListString))
                .ToObservableCollection();

            authorStringList = [.. filteredList1!
                .Select(x => x.AuthorListString)
                .Distinct()];

            var counts = new List<CountModel>();

            if (filteredList1 != null && authorStringList != null)
            {
                var list = new List<AuthorModel>();

                foreach (var authorString in authorStringList)
                {
                    if (!string.IsNullOrEmpty(authorString))
                    {
                        list.AddRange(SplitStringIntoAuthorList(authorString));
                    }
                }

                list = [.. list.DistinctBy(x => x.FullName)];
                list = [.. list.OrderByDescending(x => x.FirstName).OrderByDescending(x => x.LastName)];

                for (int i = 0; i < list.Count; i++)
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

                counts = [.. counts.OrderByDescending(x => x.Count)];

                var max = maxLimit;

                if (list.Count < max)
                {
                    max = list.Count;
                }

                counts.RemoveRange(max, list.Count - max);
            }

            return counts;
        }
    }
}
