// <copyright file="GetCounts.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using BookCollector.Data.Models;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Library;
    using BookCollector.ViewModels.Main;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// GetCounts class.
    /// </summary>
    public partial class GetCounts : ObservableObject
    {
        /// <summary>
        /// Get the count of books in the list based on the favorite value.
        /// </summary>
        /// <param name="favoriteValue">Favorite value.</param>
        /// <returns>The count of books based on the filter.</returns>
        public static async Task<int> GetBooksListCountByFavorite(bool favoriteValue)
        {
            ObservableCollection<BookModel>? filteredList;

            filteredList = AllBooksViewModel.hiddenFilteredBookList?
                .Where(x => x.IsFavorite == favoriteValue)
                .ToObservableCollection();

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        /// <summary>
        /// Get the count of books in the list based on the star rating value.
        /// </summary>
        /// <param name="starRating">Star rating value.</param>
        /// <returns>The count of books based on the filter.</returns>
        public static async Task<int> GetBooksListCountByRating(int starRating)
        {
            ObservableCollection<BookModel>? filteredList;

            filteredList = AllBooksViewModel.hiddenFilteredBookList?
                .Where(x => x.Rating == starRating)
                .ToObservableCollection();

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        /// <summary>
        /// Get the total price of books in the selected collection.
        /// </summary>
        /// <param name="inputGuid">Collection guid.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>Total price of books in selected collection.</returns>
        public static async Task<double> GetAllBookPricesInCollectionList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (inputGuid != null)
            {
                if (AllBooksViewModel.hiddenFilteredBookList != null)
                {
                    filteredList = AllBooksViewModel.hiddenFilteredBookList
                        .Where(x => x.BookCollectionGuid == inputGuid)
                        .ToObservableCollection();
                }
                else
                {
                    var list = await BaseViewModel.Database.GetAllBooksInCollectionAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }

                if (filteredList != null)
                {
                    price = filteredList.Sum(x => x.BookPriceValue);
                }
            }

            return price;
        }

        /// <summary>
        /// Get the total price of books in the selected genre.
        /// </summary>
        /// <param name="inputGuid">Genre guid.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>Total price of books in selected genre.</returns>
        public static async Task<double> GetAllBookPricesInGenreList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (inputGuid != null)
            {
                if (AllBooksViewModel.hiddenFilteredBookList != null)
                {
                    filteredList = AllBooksViewModel.hiddenFilteredBookList
                        .Where(x => x.BookGenreGuid == inputGuid)
                        .ToObservableCollection();
                }
                else
                {
                    var list = await BaseViewModel.Database.GetAllBooksInGenreAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }

                if (filteredList != null)
                {
                    price = filteredList.Sum(x => x.BookPriceValue);
                }
            }

            return price;
        }

        /// <summary>
        /// Get the total price of books in the selected series.
        /// </summary>
        /// <param name="inputGuid">Series guid.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>Total price of books in selected series.</returns>
        public static async Task<double> GetAllBookPricesInSeriesList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (inputGuid != null)
            {
                if (AllBooksViewModel.hiddenFilteredBookList != null)
                {
                    filteredList = AllBooksViewModel.hiddenFilteredBookList
                        .Where(x => x.BookSeriesGuid == inputGuid)
                        .ToObservableCollection();
                }
                else
                {
                    var list = await BaseViewModel.Database.GetAllBooksInSeriesAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }

                if (filteredList != null)
                {
                    price = filteredList.Sum(x => x.BookPriceValue);
                }
            }

            return price;
        }

        /// <summary>
        /// Get the total price of books in the selected location.
        /// </summary>
        /// <param name="inputGuid">Location guid.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>Total price of books in selected location.</returns>
        public static async Task<double> GetAllBookPricesInLocationList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;
            if (inputGuid != null)
            {
                if (AllBooksViewModel.hiddenFilteredBookList != null)
                {
                    filteredList = AllBooksViewModel.hiddenFilteredBookList
                        .Where(x => x.BookLocationGuid == inputGuid)
                        .ToObservableCollection();
                }
                else
                {
                    var list = await BaseViewModel.Database.GetAllBooksInLocationAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }

                if (filteredList != null)
                {
                    price = filteredList.Sum(x => x.BookPriceValue);
                }
            }

            return price;
        }

        /// <summary>
        /// Get the count of books in the list based on the book format value.
        /// </summary>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllBooksAndBookFormatsList()
        {
            ObservableCollection<BookModel>? bookList = null;

            bookList = AllBooksViewModel.hiddenFilteredBookList?
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

        /// <summary>
        /// Get the total price of books in the list based on the book format value.
        /// </summary>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetPriceOfBooksAndBookFormatsList()
        {
            ObservableCollection<BookModel>? bookList = null;

            bookList = AllBooksViewModel.hiddenFilteredBookList?
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

        /// <summary>
        /// Get the count of books in the wishlist based on the book format value.
        /// </summary>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllWishListBooksAndBookFormatsList()
        {
            ObservableCollection<WishlistBookModel>? bookList = null;

            bookList = WishListViewModel.hiddenFilteredWishlistBookList?
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

        /// <summary>
        /// Get the total price of books in the wishlist based on the book format value.
        /// </summary>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetPriceOfWishListBooksAndBookFormatsList()
        {
            ObservableCollection<WishlistBookModel>? bookList = null;

            bookList = WishListViewModel.hiddenFilteredWishlistBookList?
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

        /// <summary>
        /// Get the count of books read in the year based on the book end date value.
        /// </summary>
        /// <param name="year">Year to search on.</param>
        /// <returns>The count of books.</returns>
        public static async Task<int> GetBookCountReadInYear(int year)
        {
            ObservableCollection<BookModel>? filteredList;

            filteredList = AllBooksViewModel.hiddenFilteredBookList?
                .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year)
                .ToObservableCollection();

            var count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        /// <summary>
        /// Get the total page count of books read in the year based on the book end date value.
        /// </summary>
        /// <param name="year">Year to search on.</param>
        /// <returns>The count of pages.</returns>
        public static async Task<int> GetBookPageCountReadInYear(int year)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            filteredList = AllBooksViewModel.hiddenFilteredBookList?
                .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year)
                .ToObservableCollection();

            if (filteredList != null)
            {
                count = filteredList.Sum(x => x.BookPageTotal);
            }

            return count;
        }

        /// <summary>
        /// Get the total time count of books read in the year based on the book end date value.
        /// </summary>
        /// <param name="year">Year to search on.</param>
        /// <returns>The count of hours.</returns>
        public static async Task<double> GetBookTimeCountReadInYear(int year)
        {
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0.0;

            filteredList = AllBooksViewModel.hiddenFilteredBookList?
                .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year &&
                            x.BookHoursTotal != 0 && x.BookMinutesTotal != 0)
                .ToObservableCollection();

            if (filteredList != null)
            {
                await Task.WhenAll(filteredList.ToList().Select(x => x.SetBookTotalTime()));

                count = filteredList.Sum(x => x.BookTotalTime ?? 0);
            }

            return count;
        }

        /// <summary>
        /// Get the total price of all books in the list.
        /// </summary>
        /// <returns>The total price, formatted with currency symbol.</returns>
        public static async Task<string> GetPriceOfAllBooks()
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
            var cultureInfo = new CultureInfo(cultureCode);

            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            filteredList = AllBooksViewModel.hiddenFilteredBookList?
                .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                .ToObservableCollection();

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return string.Format(cultureInfo, "{0:C}", price);
        }

        /// <summary>
        /// Get the total price of all books in the wishlist.
        /// </summary>
        /// <returns>The total price, formatted with currency symbol.</returns>
        public static async Task<string> GetPriceOfAllWishListBooks()
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
            var cultureInfo = new CultureInfo(cultureCode);

            ObservableCollection<WishlistBookModel>? filteredList = null;
            var price = 0.0;

            filteredList = WishListViewModel.hiddenFilteredWishlistBookList?
                .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                .ToObservableCollection();

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return string.Format(cultureInfo, "{0:C}", price);
        }

        /// <summary>
        /// Get the total price of books in the selected author.
        /// </summary>
        /// <param name="inputGuid">Author guid.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>Total price for books for selected author.</returns>
        public static async Task<double> GetAllBookPricesInAuthorList(Guid? inputGuid, bool showHiddenBooks)
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
            var cultureInfo = new CultureInfo(cultureCode);

            var filteredList = new ObservableCollection<BookModel>();
            var price = 0.0;

            if (inputGuid != null)
            {
                var list = await BaseViewModel.Database.GetAllBooksForAuthorAsync((Guid)inputGuid, showHiddenBooks);
                filteredList = list.ToObservableCollection();

                if (filteredList != null)
                {
                    price = filteredList.Sum(x => x.BookPriceValue);
                }
            }

            return price;
        }

        /// <summary>
        /// Get all books in all authors list with the count of books for each author, ordered by the count of books,
        /// then by author name, and limited to the max limit value.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="maxLimit">Max number to limit to.</param>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllBooksInAllAuthorsList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<AuthorModel>? filteredList = null;

            await Task.WhenAll(AuthorsViewModel.hiddenFilteredAuthorList!.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = AuthorsViewModel.hiddenFilteredAuthorList?.OrderByDescending(x => x.FirstName)?.OrderByDescending(x => x.LastName).ToObservableCollection();

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

        /// <summary>
        /// Get all books in all collections list with the count of books for each collection, ordered by the count of books,
        /// then by collection name, and limited to the max limit value.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="maxLimit">Max number to limit to.</param>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllBooksInAllCollectionsList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<CollectionModel>? filteredList = null;

            await Task.WhenAll(CollectionsViewModel.hiddenFilteredCollectionList!.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = CollectionsViewModel.hiddenFilteredCollectionList?.OrderByDescending(x => x.ParsedCollectionName).ToObservableCollection();

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

        /// <summary>
        /// Get all books in all collections list with the count of books for each collection, ordered by the count of books,
        /// then by collection name, and limited to the max limit value.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="maxLimit">Max number to limit to.</param>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllBooksInAllGenresList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<GenreModel>? filteredList = null;

            await Task.WhenAll(GenresViewModel.hiddenFilteredGenreList!.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = GenresViewModel.hiddenFilteredGenreList?.OrderByDescending(x => x.ParsedGenreName).ToObservableCollection();

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

        /// <summary>
        /// Get all books in all series list with the count of books for each series, ordered by the count of books,
        /// then by series name, and limited to the max limit value.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="maxLimit">Max number to limit to.</param>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllBooksInAllSeriesList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<SeriesModel>? filteredList = null;

            await Task.WhenAll(SeriesViewModel.hiddenFilteredSeriesList!.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = SeriesViewModel.hiddenFilteredSeriesList?.OrderByDescending(x => x.ParsedSeriesName).ToObservableCollection();

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

        /// <summary>
        /// Get all books in all locations list with the count of books for each location, ordered by the count of books,
        /// then by location name, and limited to the max limit value.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="maxLimit">Max number to limit to.</param>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllBooksInAllLocationsList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<LocationModel>? filteredList = null;

            await Task.WhenAll(LocationsViewModel.hiddenFilteredLocationList!.Select(x => x.SetTotalBooks(showHiddenBooks)));

            filteredList = LocationsViewModel.hiddenFilteredLocationList?.OrderByDescending(x => x.ParsedLocationName).ToObservableCollection();

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

        /// <summary>
        /// Get all wishlist books in all locations list with the count of books for each location, ordered by the count of books,
        /// then by location name, and limited to the max limit value.
        /// </summary>
        /// <param name="maxLimit">Max number to limit to.</param>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllWishListBooksAndLocationList(int maxLimit)
        {
            ObservableCollection<WishlistBookModel>? filteredList1 = null;
            List<string?>? list = null;

            filteredList1 = WishListViewModel.hiddenFilteredWishlistBookList?
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

        /// <summary>
        /// Get all wishlist books in all series list with the count of books for each series, ordered by the count of books,
        /// then by series name, and limited to the max limit value.
        /// </summary>
        /// <param name="maxLimit">Max number to limit to.</param>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllWishListBooksAndSeriesList(int maxLimit)
        {
            ObservableCollection<WishlistBookModel>? filteredList1 = null;
            List<string?>? list = null;

            filteredList1 = WishListViewModel.hiddenFilteredWishlistBookList?
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

        /// <summary>
        /// Get all wishlist books in all authors list with the count of books for each author, ordered by the count of books,
        /// then by author name, and limited to the max limit value.
        /// </summary>
        /// <param name="maxLimit">Max number to limit to.</param>
        /// <returns>A list formatted with label and value.</returns>
        public static async Task<List<CountModel>> GetAllWishListBooksAndAuthorList(int maxLimit)
        {
            ObservableCollection<WishlistBookModel>? filteredList1 = null;
            List<string?>? authorStringList = null;

            filteredList1 = WishListViewModel.hiddenFilteredWishlistBookList?
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
                        list.AddRange(await StringManipulation.SplitAuthorListStringIntoAuthorList(authorString));
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
