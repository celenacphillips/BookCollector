using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;
using DocumentFormat.OpenXml.Bibliography;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BookCollector.Data
{
    public partial class FilterLists : BaseViewModel
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
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.AuthorListstring).ToObservableCollection();
                }

                if (descendingChecked)
                {
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.AuthorListstring).ToObservableCollection();
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

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetReadingBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => (x.BookPageRead != x.BookPageTotal &&
                                                   x.BookPageRead != 0) ||
                                                   (x.UpNext == true))
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<int> GetReadingBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => (x.BookPageRead != x.BookPageTotal &&
                                                    x.BookPageRead != 0) ||
                                                    (x.UpNext == true)).ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<ObservableCollection<BookModel>?> GetToBeReadBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookPageRead == 0 &&
                                                   x.UpNext == false)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<int> GetToBeReadBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookPageRead == 0 &&
                                            x.UpNext == false).ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<ObservableCollection<BookModel>?> GetReadBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookPageRead == x.BookPageTotal &&
                                                   x.BookPageRead != 0)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<int> GetReadBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookPageRead == x.BookPageTotal &&
                                                   x.BookPageRead != 0).ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<int> GetAllBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList;
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetAllWishListBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList;
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetBooksListCountByFavorite(bool showHiddenBooks, bool favoriteValue)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.IsFavorite == favoriteValue).ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetBooksListCountByRating(bool showHiddenBooks, int starRating)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.Rating == starRating).ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<ObservableCollection<BookModel>?> GetBookWishList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<ChapterModel>?> GetAllChaptersInBook(Guid? inputGuid)
        {
            ObservableCollection<ChapterModel>? chapterList = null;
            ObservableCollection<ChapterModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                chapterList = TestData.ChapterList;
            }
            else
            {
            }

            if (chapterList != null)
            {
                filteredList = chapterList.Where(x => x.BookGuid == inputGuid)
                                          .OrderBy(x => x.ChapterOrder)
                                          .ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<ChapterModel>?> GetAllChapters()
        {
            ObservableCollection<ChapterModel>? chapterList = null;
            ObservableCollection<ChapterModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                chapterList = TestData.ChapterList;
            }
            else
            {
            }

            if (chapterList != null)
            {
                filteredList = chapterList.OrderBy(x => x.ChapterOrder)
                                          .OrderBy(x => x.BookGuid)
                                          .ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<string>> GetAllPublishersInBookList(ObservableCollection<BookModel> bookList)
        {
            var publisherList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookPublisher) && !publisherList.Any(x => x.Equals(book.BookPublisher)))
                {
                    publisherList.Add(book.BookPublisher);
                }
            }

            return publisherList;
        }

        public static async Task<ObservableCollection<string>> GetAllAuthorsInBookList(ObservableCollection<BookModel> bookList)
        {
            var authorList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.AuthorListstring))
                {
                    string[] authorNames = book.AuthorListstring.Split(";");

                    foreach (var authorName in authorNames)
                    {
                        if (!string.IsNullOrEmpty(authorName.Trim()))
                        {
                            string[] name = authorName.Split(",");

                            AuthorModel author = new ()
                            {
                                FirstName = name[1].Trim(),
                                LastName = name[0].Trim(),
                            };

                            authorList.Add(author.FullName);
                        }
                    }
                }
            }

            return authorList;
        }

        public static async Task<ObservableCollection<string>> GetAllLocationsInBookList(ObservableCollection<BookModel> bookList)
        {
            var locationList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookWhereToBuy) && !locationList.Any(x => x.Equals(book.BookWhereToBuy)))
                {
                    locationList.Add(book.BookWhereToBuy);
                }
            }

            return locationList;
        }

        public static async Task<ObservableCollection<string>> GetAllSeriesInBookList(ObservableCollection<BookModel> bookList)
        {
            var seriesList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookSeries) && !seriesList.Any(x => x.Equals(book.BookSeries)))
                {
                    seriesList.Add(book.BookSeries);
                }
            }

            return seriesList;
        }

        public static async Task<ObservableCollection<string>> GetAllPublisherYearsInBookList(ObservableCollection<BookModel> bookList)
        {
            var publishYearList = new ObservableCollection<string>();

            foreach (var book in bookList.OrderBy(x => x.BookPublishYear))
            {
                if (!string.IsNullOrEmpty(book.BookPublishYear))
                {
                    var publishYearSubstring = book.BookPublishYear[..3];
                    var publishRange = $"{publishYearSubstring}0 - {publishYearSubstring}9";

                    if (!publishYearList.Any(x => x.Equals(publishRange)))
                    {
                        publishYearList.Add(publishRange);
                    }
                }
            }

            return publishYearList;
        }

        public static async Task<ObservableCollection<string>> GetAllLanguagesInBookList(ObservableCollection<BookModel> bookList)
        {
            var languageList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookLanguage) && !languageList.Any(x => x.Equals(book.BookLanguage)))
                {
                    languageList.Add(book.BookLanguage);
                }
            }

            return languageList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthorsForBook(Guid? inputGuid)
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;
            ObservableCollection<BookAuthorModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookAuthorList = TestData.BookAuthorList;
            }
            else
            {
            }

            if (bookAuthorList != null)
            {
                filteredList = bookAuthorList.Where(x => x.BookGuid == inputGuid)
                                             .ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthors()
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;
            ObservableCollection<BookAuthorModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookAuthorList = TestData.BookAuthorList;
            }
            else
            {
            }

            if (bookAuthorList != null)
            {
                filteredList = bookAuthorList.OrderBy(x => x.BookGuid)
                                             .ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthorsForAuthor(Guid? inputGuid)
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;
            ObservableCollection<BookAuthorModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookAuthorList = TestData.BookAuthorList;
            }
            else
            {
            }

            if (bookAuthorList != null)
            {
                filteredList = bookAuthorList.Where(x => x.AuthorGuid == inputGuid)
                                             .ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<GenreModel?> GetGenreForBook(Guid? inputGuid)
        {
            ObservableCollection<GenreModel>? genreList = null;
            GenreModel? genre = null;

            if (TestData.UseTestData)
            {
                genreList = TestData.GenreList;
            }
            else
            {
            }

            if (genreList != null)
            {
                genre = genreList.FirstOrDefault(x => x.GenreGuid == inputGuid);
            }

            return genre;
        }

        public static async Task<LocationModel?> GetLocationForBook(Guid? inputGuid)
        {
            ObservableCollection<LocationModel>? locationList = null;
            LocationModel? location = null;

            if (TestData.UseTestData)
            {
                locationList = TestData.LocationList;
            }
            else
            {
            }

            if (locationList != null)
            {
                location = locationList.FirstOrDefault(x => x.LocationGuid == inputGuid);
            }

            return location;
        }

        public static async Task<SeriesModel?> GetSeriesForBook(Guid? inputGuid)
        {
            ObservableCollection<SeriesModel>? seriesList = null;
            SeriesModel? series = null;

            if (TestData.UseTestData)
            {
                seriesList = TestData.SeriesList;
            }
            else
            {
            }

            if (seriesList != null)
            {
                series = seriesList.FirstOrDefault(x => x.SeriesGuid == inputGuid);
            }

            return series;
        }

        public static async Task<CollectionModel?> GetCollectionForBook(Guid? inputGuid)
        {
            ObservableCollection<CollectionModel>? collectionList = null;
            CollectionModel? collection = null;

            if (TestData.UseTestData)
            {
                collectionList = TestData.CollectionList;
            }
            else
            {
            }

            if (collectionList != null)
            {
                collection = collectionList.FirstOrDefault(x => x.CollectionGuid == inputGuid);
            }

            return collection;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInCollectionList(
            Guid? inputGuid,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookCollectionGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<double> GetAllBookPricesInCollectionList(
            Guid? inputGuid,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookCollectionGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return price;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutACollectionList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookCollectionGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInGenreList(
            Guid? inputGuid,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookGenreGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<double> GetAllBookPricesInGenreList(
            Guid? inputGuid,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookGenreGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return price;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutAGenreList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookGenreGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInSeriesList(
            Guid? inputGuid,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookSeriesGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<double> GetAllBookPricesInSeriesList(
            Guid? inputGuid,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookSeriesGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return price;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutASeriesList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.HideBook != showHiddenBooks &&
                                                   x.BookSeriesGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInAuthorList(
            ObservableCollection<BookAuthorModel>? bookAuthorList,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            var filteredList = new ObservableCollection<BookModel>();

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null && bookAuthorList != null)
            {
                foreach (var bookAuthor in bookAuthorList)
                {
                    var book = bookList.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid);

                    if (book != null)
                    {
                        filteredList.Add(book);
                    }
                }
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList.OrderBy(x => x.ParsedTitle)
                               .ToObservableCollection();
        }

        public static async Task<double> GetAllBookPricesInAuthorList(
            ObservableCollection<BookAuthorModel>? bookAuthorList,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            var filteredList = new ObservableCollection<BookModel>();
            var price = 0.0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);

            var cultureInfo = new CultureInfo(cultureCode);

            if (bookList != null && bookAuthorList != null)
            {
                foreach (var bookAuthor in bookAuthorList)
                {
                    var book = bookList.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid);

                    if (book != null)
                    {
                        filteredList.Add(book);
                    }
                }
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
            }

            price = filteredList.Sum(x => x.BookPriceValue);

            return price;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutAuthorList(
            string reverseAuthorName,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => string.IsNullOrEmpty(x.AuthorListstring) ||
                                                   (!string.IsNullOrEmpty(x.AuthorListstring) && !x.AuthorListstring.Contains(reverseAuthorName)))
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInLocationList(
            Guid? inputGuid,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookLocationGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<double> GetAllBookPricesInLocationList(
            Guid? inputGuid,
            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookLocationGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            if (filteredList != null)
            {
                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return price;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutALocationList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => x.BookLocationGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<CollectionModel>?> GetAllCollectionsList(bool showHiddenCollections)
        {
            ObservableCollection<CollectionModel>? collectionList = null;
            ObservableCollection<CollectionModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                collectionList = TestData.CollectionList;
            }
            else
            {
            }

            if (collectionList != null)
            {
                filteredList = collectionList.OrderBy(x => x.CollectionName)
                                             .ToObservableCollection();
            }

            if (!showHiddenCollections)
            {
                filteredList = filteredList?.Where(x => !x.HideCollection).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllCollectionsList(
            bool showHiddenCollections,
            bool showHiddenBooks,
            int maxLimit)
        {
            ObservableCollection<CollectionModel>? collectionList = null;
            ObservableCollection<CollectionModel>? filteredList = null;
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                collectionList = TestData.CollectionList;
                bookList = TestData.BookList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            if (collectionList != null && bookList != null)
            {
                filteredList = collectionList.Where(x => x.CollectionTotalBooks != 0)
                                                 .OrderByDescending(x => x.CollectionTotalBooks)
                                                 .ToObservableCollection();

                if (!showHiddenCollections)
                {
                    filteredList = filteredList.Where(x => !x.HideCollection).ToObservableCollection();
                }

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

                if (!showHiddenBooks)
                {
                    filteredBookList = bookList.Where(x => !x.HideBook).ToObservableCollection();
                }
                else
                {
                    filteredBookList = bookList;
                }

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoCollection,
                    Count = filteredBookList.Count(x => x.BookCollectionGuid == null),
                });
            }

            return counts;
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

        public static async Task<ObservableCollection<GenreModel>?> GetAllGenresList(bool showHiddenGenres)
        {
            ObservableCollection<GenreModel>? genreList = null;
            ObservableCollection<GenreModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                genreList = TestData.GenreList;
            }
            else
            {
            }

            if (genreList != null)
            {
                filteredList = genreList.OrderBy(x => x.GenreName)
                                        .ToObservableCollection();
            }

            if (!showHiddenGenres)
            {
                filteredList = filteredList?.Where(x => !x.HideGenre).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllGenresList(
            bool showHiddenGenres,
            bool showHiddenBooks,
            int maxLimit)
        {
            ObservableCollection<GenreModel>? genreList = null;
            ObservableCollection<GenreModel>? filteredList = null;
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                genreList = TestData.GenreList;
                bookList = TestData.BookList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            if (genreList != null && bookList != null)
            {
                filteredList = genreList.Where(x => x.GenreTotalBooks != 0)
                                        .OrderByDescending(x => x.GenreTotalBooks)
                                        .ToObservableCollection();

                if (!showHiddenGenres)
                {
                    filteredList = filteredList.Where(x => !x.HideGenre).ToObservableCollection();
                }

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

                if (!showHiddenBooks)
                {
                    filteredBookList = bookList.Where(x => !x.HideBook).ToObservableCollection();
                }
                else
                {
                    filteredBookList = bookList;
                }

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoGenre,
                    Count = filteredBookList.Count(x => x.BookGenreGuid == null),
                });
            }

            return counts;
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

        public static async Task<ObservableCollection<SeriesModel>?> GetAllSeriesList(bool showHiddenSeries)
        {
            ObservableCollection<SeriesModel>? seriesList = null;
            ObservableCollection<SeriesModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                seriesList = TestData.SeriesList;
            }
            else
            {
            }

            if (seriesList != null)
            {
                filteredList = seriesList.OrderBy(x => x.SeriesName)
                                         .ToObservableCollection();
            }

            if (!showHiddenSeries)
            {
                filteredList = filteredList?.Where(x => !x.HideSeries).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllSeriesList(
            bool showHiddenSeries,
            bool showHiddenBooks,
            int maxLimit)
        {
            ObservableCollection<SeriesModel>? seriesList = null;
            ObservableCollection<SeriesModel>? filteredList = null;
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                seriesList = TestData.SeriesList;
                bookList = TestData.BookList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            if (seriesList != null && bookList != null)
            {
                filteredList = seriesList.Where(x => x.SeriesTotalBooks != 0)
                                             .OrderByDescending(x => x.SeriesTotalBooks)
                                             .ToObservableCollection();

                if (!showHiddenSeries)
                {
                    filteredList = filteredList.Where(x => !x.HideSeries).ToObservableCollection();
                }

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

                if (!showHiddenBooks)
                {
                    filteredBookList = bookList.Where(x => !x.HideBook).ToObservableCollection();
                }
                else
                {
                    filteredBookList = bookList;
                }

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoSeries,
                    Count = filteredBookList.Count(x => x.BookSeriesGuid == null),
                });
            }

            return counts;
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

        public static async Task<ObservableCollection<LocationModel>?> GetAllLocationsList(bool showHiddenLocations)
        {
            ObservableCollection<LocationModel>? locationList = null;
            ObservableCollection<LocationModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                locationList = TestData.LocationList;
            }
            else
            {
            }

            if (locationList != null)
            {
                filteredList = locationList.OrderBy(x => x.LocationName)
                                           .ToObservableCollection();
            }

            if (!showHiddenLocations)
            {
                filteredList = filteredList?.Where(x => !x.HideLocation).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllLocationsList(
            bool showHiddenLocations,
            bool showHiddenBooks,
            int maxLimit)
        {
            ObservableCollection<LocationModel>? locationList = null;
            ObservableCollection<LocationModel>? filteredList = null;
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                locationList = TestData.LocationList;
                bookList = TestData.BookList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            if (locationList != null && bookList != null)
            {
                filteredList = locationList.Where(x => x.LocationTotalBooks != 0)
                                               .OrderByDescending(x => x.LocationTotalBooks)
                                               .ToObservableCollection();

                if (!showHiddenLocations)
                {
                    filteredList = filteredList.Where(x => !x.HideLocation).ToObservableCollection();
                }

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

                if (!showHiddenBooks)
                {
                    filteredBookList = bookList.Where(x => !x.HideBook).ToObservableCollection();
                }
                else
                {
                    filteredBookList = bookList;
                }

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoLocation,
                    Count = filteredBookList.Count(x => x.BookLocationGuid == null),
                });
            }

            return counts;
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

        public static async Task<ObservableCollection<AuthorModel>?> GetAllAuthorsList(bool showHiddenAuthors)
        {
            ObservableCollection<AuthorModel>? authorList = null;
            ObservableCollection<AuthorModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                authorList = TestData.AuthorList;
            }
            else
            {
            }

            if (authorList != null)
            {
                filteredList = authorList.OrderBy(x => x.LastName)
                                         .OrderBy(x => x.FirstName)
                                         .ToObservableCollection();
            }

            if (!showHiddenAuthors)
            {
                filteredList = filteredList?.Where(x => !x.HideAuthor).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllAuthorsList(
            bool showHiddenAuthors,
            bool showHiddenBooks,
            int maxLimit)
        {
            ObservableCollection<AuthorModel>? authorList = null;
            ObservableCollection<AuthorModel>? filteredList = null;
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredBookList = null;

            if (TestData.UseTestData)
            {
                authorList = TestData.AuthorList;
                bookList = TestData.BookList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            if (authorList != null && bookList != null)
            {
                filteredList = authorList.Where(x => x.AuthorTotalBooks != 0)
                                         .OrderByDescending(x => x.AuthorTotalBooks)
                                         .ToObservableCollection();

                if (!showHiddenAuthors)
                {
                    filteredList = filteredList.Where(x => !x.HideAuthor).ToObservableCollection();
                }

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

                if (!showHiddenBooks)
                {
                    filteredBookList = bookList.Where(x => !x.HideBook).ToObservableCollection();
                }
                else
                {
                    filteredBookList = bookList;
                }

                counts.Add(new CountModel()
                {
                    Label = AppStringResources.NoAuthor,
                    Count = filteredBookList.Count(x => string.IsNullOrEmpty(x.AuthorListstring)),
                });
            }

            return counts;
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

        public static List<CountModel> GetAllBooksAndBookFormatsList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            if (formatList != null && bookList != null)
            {
                for (int i = 0; i < formatList.Count; i++)
                {
                    var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) &&
                                                    x.BookFormat.Equals(formatList[i])).ToObservableCollection();

                    if (!showHiddenBooks)
                    {
                        filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                    }

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

        public static List<CountModel> GetPriceOfBooksAndBookFormatsList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            if (formatList != null && bookList != null)
            {
                for (int i = 0; i < formatList.Count; i++)
                {
                    var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) &&
                                                   x.BookFormat.Equals(formatList[i])).ToObservableCollection();

                    if (!showHiddenBooks)
                    {
                        filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                    }

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

        public static List<CountModel> GetAllWishListBooksAndBookFormatsList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            if (formatList != null && bookList != null)
            {
                for (int i = 0; i < formatList.Count; i++)
                {
                    var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) &&
                                                    x.BookFormat.Equals(formatList[i])).ToObservableCollection();

                    if (!showHiddenBooks)
                    {
                        filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                    }

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

        public static List<CountModel> GetPriceOfWishListBooksAndBookFormatsList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            if (formatList != null && bookList != null)
            {
                for (int i = 0; i < formatList.Count; i++)
                {
                    var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) &&
                                                   x.BookFormat.Equals(formatList[i])).ToObservableCollection();

                    if (!showHiddenBooks)
                    {
                        filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                    }

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
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookStartDate) &&
                                            !string.IsNullOrEmpty(x.BookEndDate) &&
                                            DateTime.Parse(x.BookEndDate).Year == year).ToObservableCollection();
            }

            if (!showHiddenBooks)
            {
                filteredList = filteredList?.Where(x => !x.HideBook).ToObservableCollection();
            }

            count = filteredList != null ? filteredList.Count : 0;

            return count;
        }

        public static async Task<int> GetBookPageCountReadInYear(int year, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            var count = 0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            if (bookList != null)
            {
                var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookStartDate) &&
                                           !string.IsNullOrEmpty(x.BookEndDate) &&
                                           DateTime.Parse(x.BookEndDate).Year == year).ToObservableCollection();

                if (!showHiddenBooks)
                {
                    filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                }

                count = filteredList.Sum(x => x.BookPageTotal);
            }

            return count;
        }

        public static async Task<string> GetPriceOfAllBooks(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {
            }

            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);

            var cultureInfo = new CultureInfo(cultureCode);

            if (bookList != null)
            {
                var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookPrice));

                if (!showHiddenBooks)
                {
                    filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                }

                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return string.Format(cultureInfo, "{0:C}", price);
        }

        public static async Task<string> GetPriceOfAllWishListBooks(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            var price = 0.0;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {
            }

            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);

            var cultureInfo = new CultureInfo(cultureCode);

            if (bookList != null)
            {
                var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookPrice)).ToObservableCollection();

                if (!showHiddenBooks)
                {
                    filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                }

                price = filteredList.Sum(x => x.BookPriceValue);
            }

            return string.Format(cultureInfo, "{0:C}", price);
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndSeriesList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            if (bookList != null)
            {
                var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookSeries))
                               .ToObservableCollection();

                var filteredList2 = bookList.Where(x => string.IsNullOrEmpty(x.BookSeries)).ToObservableCollection();

                if (!showHiddenBooks)
                {
                    filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                    filteredList2 = filteredList2.Where(x => !x.HideBook).ToObservableCollection();
                }

                var list = filteredList.Select(x => x.BookSeries)
                               .Distinct()
                               .ToObservableCollection();

                var max = maxLimit;

                if (list.Count < max)
                {
                    max = list.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    var count = bookList.Count(x => !string.IsNullOrEmpty(x.BookSeries) && x.BookSeries.Equals(list[i]));

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

        public static async Task<List<CountModel>> GetAllWishListBooksAndLocationList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            if (bookList != null)
            {
                var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy))
                               .ToObservableCollection();

                var filteredList2 = bookList.Where(x => string.IsNullOrEmpty(x.BookWhereToBuy)).ToObservableCollection();

                if (!showHiddenBooks)
                {
                    filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                    filteredList2 = filteredList2.Where(x => !x.HideBook).ToObservableCollection();
                }

                var list = filteredList.Select(x => x.BookWhereToBuy)
                               .Distinct()
                               .ToObservableCollection();

                var max = maxLimit;

                if (list.Count < max)
                {
                    max = list.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    var count = bookList.Where(x => x.HideBook != showHiddenBooks).Count(x => !string.IsNullOrEmpty(x.BookWhereToBuy) && x.BookWhereToBuy.Equals(list[i]));

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

        public static async Task<List<CountModel>> GetAllWishListBooksAndAuthorList(bool showHiddenBooks, int maxLimit)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {
            }

            var counts = new List<CountModel>();

            if (bookList != null)
            {
                var filteredList = bookList.Where(x => !string.IsNullOrEmpty(x.AuthorListstring))
                                         .ToObservableCollection();

                var filteredList2 = bookList.Where(x => string.IsNullOrEmpty(x.AuthorListstring)).ToObservableCollection();

                if (!showHiddenBooks)
                {
                    filteredList = filteredList.Where(x => !x.HideBook).ToObservableCollection();
                    filteredList2 = filteredList2.Where(x => !x.HideBook).ToObservableCollection();
                }

                var authorList = filteredList.Select(x => x.AuthorListstring)
                                         .Distinct()
                                         .ToObservableCollection();

                List<AuthorModel> list = [];

                foreach (var author in authorList)
                {
                    if (!string.IsNullOrEmpty(author))
                    {
                        string[] authorNames = author.Split(";");

                        foreach (var authorName in authorNames)
                        {
                            if (!string.IsNullOrEmpty(authorName.Trim()))
                            {
                                string[] name = authorName.Split(",");

                                AuthorModel author1 = new ()
                                {
                                    FirstName = name[1].Trim(),
                                    LastName = name[0].Trim(),
                                };

                                list.Add(author1);
                            }
                        }
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
                    var filteredBookList = bookList.Where(x => !string.IsNullOrEmpty(x.AuthorListstring) &&
                                                    x.AuthorListstring.Contains(list[i].ReverseFullName)).ToObservableCollection();

                    if (!showHiddenBooks)
                    {
                        filteredBookList = filteredBookList.Where(x => !x.HideBook).ToObservableCollection();
                    }

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

        public static async Task<ObservableCollection<BookModel>> FilterBookList(
            ObservableCollection<BookModel> bookList,
            string? favoritesOption,
            string? formatOption,
            string? publisherOption,
            string? languageOption,
            string? ratingOption,
            string? publishYearOption,
            string? authorOption = null,
            string? locationOption = null,
            string? seriesOption = null)
        {
            var filteredList = bookList;

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

            // For wishlist only
            if (!string.IsNullOrEmpty(authorOption))
            {
                filteredList = FilterBookAuthor(filteredList, authorOption);
            }

            if (!string.IsNullOrEmpty(locationOption))
            {
                filteredList = FilterBookLocation(filteredList, locationOption);
            }

            if (!string.IsNullOrEmpty(seriesOption))
            {
                filteredList = FilterBookSeries(filteredList, seriesOption);
            }

            return filteredList;
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

        private static ObservableCollection<BookModel> FilterBookFormat(
            ObservableCollection<BookModel> bookList,
            string? formatOption)
        {
            var filterList = bookList;

            if (!string.IsNullOrEmpty(formatOption) && !formatOption.Equals(AppStringResources.AllFormats))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookFormat) && x.BookFormat.Equals(formatOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookPublisher(
            ObservableCollection<BookModel> bookList,
            string? publisherOption)
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

        private static ObservableCollection<BookModel> FilterBookPublishYear(
            ObservableCollection<BookModel> bookList,
            string? publishYearOption)
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

        private static ObservableCollection<BookModel> FilterBookLanguage(
            ObservableCollection<BookModel> bookList,
            string? languageOption)
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

        private static ObservableCollection<BookModel> FilterBookRating(
            ObservableCollection<BookModel> bookList,
            string ratingOption)
        {
            var filterList = bookList;

            if (!ratingOption.Equals(AppStringResources.AllRatings))
            {
                filterList = bookList.Where(x => x.Rating == int.Parse(ratingOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookAuthor(
            ObservableCollection<BookModel> bookList,
            string authorOption)
        {
            var filterList = bookList;
            var newFilteredList = new ObservableCollection<BookModel>();

            if (!string.IsNullOrEmpty(authorOption) && authorOption.Equals(AppStringResources.NoAuthor))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.AuthorListstring))
                                     .ToObservableCollection();
            }

            if (!string.IsNullOrEmpty(authorOption) && !authorOption.Equals(AppStringResources.NoAuthor) && !authorOption.Equals(AppStringResources.AllAuthors))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.AuthorListstring))
                                     .ToObservableCollection();

                foreach (var book in filterList)
                {
                    if (!string.IsNullOrEmpty(book.AuthorListstring))
                    {
                        string[] authorNames = book.AuthorListstring.Split(";");

                        foreach (var authorName in authorNames)
                        {
                            if (!string.IsNullOrEmpty(authorName.Trim()))
                            {
                                string[] name = authorName.Split(",");

                                AuthorModel author = new ()
                                {
                                    FirstName = name[1].Trim(),
                                    LastName = name[0].Trim(),
                                };

                                if (author.FullName.Equals(authorOption))
                                {
                                    newFilteredList.Add(book);
                                }
                            }
                        }
                    }
                }

                filterList = newFilteredList.Distinct().ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookLocation(
            ObservableCollection<BookModel> bookList,
            string locationOption)
        {
            var filterList = bookList;

            if (locationOption.Equals(AppStringResources.NoLocation))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookWhereToBuy))
                                     .ToObservableCollection();
            }

            if (!locationOption.Equals(AppStringResources.NoLocation) && !locationOption.Equals(AppStringResources.AllLocations))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy) && x.BookWhereToBuy.Equals(locationOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookSeries(
            ObservableCollection<BookModel> bookList,
            string seriesOption)
        {
            var filterList = bookList;

            if (seriesOption.Equals(AppStringResources.NoSeries))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookSeries))
                                     .ToObservableCollection();
            }

            if (!seriesOption.Equals(AppStringResources.NoSeries) && !seriesOption.Equals(AppStringResources.AllSeries))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookSeries) && x.BookSeries.Equals(seriesOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }
    }
}
