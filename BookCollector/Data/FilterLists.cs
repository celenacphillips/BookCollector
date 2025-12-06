using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;
using DocumentFormat.OpenXml.Bibliography;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml.Linq;

namespace BookCollector.Data
{
    public partial class FilterLists : BaseViewModel
    {
        private static ObservableCollection<BookModel> FilterFavoriteBooks(ObservableCollection<BookModel> bookList, string favoritesOption)
        {
            var filterList = bookList;

            switch (favoritesOption)
            {
                case "Favorites":
                    filterList = bookList.Where(x => x.IsFavorite)
                                         .ToObservableCollection();
                    break;

                case "Non-Favorites":
                    filterList = bookList.Where(x => !x.IsFavorite)
                                         .ToObservableCollection();
                    break;

                default:
                    filterList = bookList;
                    break;
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookFormat(ObservableCollection<BookModel> bookList, string formatOption)
        {
            var filterList = bookList;

            if (!formatOption.Equals(AppStringResources.AllFormats))
            {
                filterList = bookList.Where(x => x.BookFormat.Equals(formatOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookPublisher(ObservableCollection<BookModel> bookList, string publisherOption)
        {
            var filterList = bookList;

            if (publisherOption.Equals(AppStringResources.NoPublisher))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookPublisher))
                                     .ToObservableCollection();
            }

            if (!publisherOption.Equals(AppStringResources.NoPublisher) && !publisherOption.Equals(AppStringResources.AllPublishers))
            {
                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookPublisher) && x.BookPublisher.Equals(publisherOption))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookPublishYear(ObservableCollection<BookModel> bookList, string publishYearOption)
        {
            var filterList = bookList;

            if (publishYearOption.Equals(AppStringResources.NoPublishYear))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            if (!publishYearOption.Equals(AppStringResources.NoPublishYear) && !publishYearOption.Equals(AppStringResources.AllPublishYears))
            {
                var years = publishYearOption.Split(" - ");

                filterList = bookList.Where(x => !string.IsNullOrEmpty(x.BookPublishYear) &&
                                            int.Parse(years[0]) <= int.Parse(x.BookPublishYear) &&
                                            int.Parse(years[1]) >= int.Parse(x.BookPublishYear))
                                     .ToObservableCollection();
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookLanguage(ObservableCollection<BookModel> bookList, string languageOption)
        {
            var filterList = bookList;

            if (languageOption.Equals(AppStringResources.NoLanguage))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.BookLanguage))
                                     .ToObservableCollection();
            }

            if (!languageOption.Equals(AppStringResources.NoLanguage) && !languageOption.Equals(AppStringResources.AllLanguages))
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

        private static ObservableCollection<BookModel> FilterBookAuthor(ObservableCollection<BookModel> bookList,
                                                                        string authorOption)
        {
            var filterList = bookList;

            if (authorOption.Equals(AppStringResources.NoAuthor))
            {
                filterList = bookList.Where(x => string.IsNullOrEmpty(x.AuthorListString))
                                     .ToObservableCollection();
            }

            if (!authorOption.Equals(AppStringResources.NoAuthor) && !authorOption.Equals(AppStringResources.AllAuthors))
            {
                AuthorModel? author = null;

                if (TestData.UseTestData)
                {
                    author = TestData.AuthorList.FirstOrDefault(x => x.FullName.Equals(authorOption));
                }
                else
                {

                }

                if (author != null)
                {
                    filterList = bookList.Where(x => !string.IsNullOrEmpty(x.AuthorListString) && x.AuthorListString.Contains(author.ReverseFullName))
                                         .ToObservableCollection();
                }
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookLocation(ObservableCollection<BookModel> bookList,
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

        private static ObservableCollection<BookModel> FilterBookSeries(ObservableCollection<BookModel> bookList, string seriesOption)
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

        public static async Task<ObservableCollection<BookModel>> FilterBookList(ObservableCollection<BookModel> bookList,
                                                                                 string? favoritesOption,
                                                                                 string formatOption,
                                                                                 string publisherOption,
                                                                                 string languageOption,
                                                                                 string? ratingOption,
                                                                                 string publishYearOption,
                                                                                 string? authorOption = null,
                                                                                 string? locationOption = null,
                                                                                 string? seriesOption = null)
        {
            var filteredList = bookList;

            if(!string.IsNullOrEmpty(favoritesOption))
                filteredList = FilterFavoriteBooks(filteredList, favoritesOption);

            filteredList = FilterBookFormat(filteredList, formatOption);

            filteredList = FilterBookPublisher(filteredList, publisherOption);

            filteredList = FilterBookLanguage(filteredList, languageOption);

            if (!string.IsNullOrEmpty(ratingOption))
                filteredList = FilterBookRating(filteredList, ratingOption);

            filteredList = FilterBookPublishYear(filteredList, publishYearOption);

            if (!string.IsNullOrEmpty(authorOption))
                filteredList = FilterBookAuthor(filteredList, authorOption);

            if (!string.IsNullOrEmpty(locationOption))
                filteredList = FilterBookLocation(filteredList, locationOption);

            if (!string.IsNullOrEmpty(seriesOption))
                filteredList = FilterBookSeries(filteredList, seriesOption);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> SortBookList(ObservableCollection<BookModel> bookList,
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
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).ToObservableCollection();
            }

            if (bookReadingDateChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.StartDateValue).OrderBy(x => x.EndDateValue).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.StartDateValue).OrderByDescending(x => x.EndDateValue).ToObservableCollection();
            }

            if (bookReadPercentageChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.Progress).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.Progress).ToObservableCollection();
            }

            if (bookPublisherChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPublisher).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPublisher).ToObservableCollection();
            }

            if (bookPublishYearChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPublishYear).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPublishYear).ToObservableCollection();
            }

            if (authorLastNameChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.AuthorListString).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.AuthorListString).ToObservableCollection();
            }

            if (bookFormatChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookFormat).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookFormat).ToObservableCollection();
            }

            if (bookPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookPrice).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookPrice).ToObservableCollection();
            }

            if (seriesOrderChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookNumberInSeries).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookNumberInSeries).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetReadingBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks &&
                                                   (x.BookPageRead != x.BookPageTotal &&
                                                   x.BookPageRead != 0) ||
                                                   (x.UpNext == true))
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            //foreach (var book in filteredList)
            //{
            //    await book.SetAuthorListString();
            //}

            return filteredList;
        }

        public static async Task<int> GetReadingBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks &&
                                            (x.BookPageRead != x.BookPageTotal &&
                                            x.BookPageRead != 0) ||
                                            (x.UpNext == true));

            return count;
        }

        public static async Task<ObservableCollection<BookModel>> GetToBeReadBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks &&
                                                   x.BookPageRead == 0 &&
                                                   x.UpNext == false)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<int> GetToBeReadBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks &&
                                            x.BookPageRead == 0 &&
                                            x.UpNext == false);

            return count;
        }

        public static async Task<ObservableCollection<BookModel>> GetReadBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks && 
                                                   x.BookPageRead == x.BookPageTotal &&
                                                   x.BookPageRead != 0)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<int> GetReadBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks &&
                                            x.BookPageRead == x.BookPageTotal &&
                                            x.BookPageRead != 0);

            return count;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<int> GetAllBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks);

            return count;
        }

        public static async Task<int> GetAllWishListBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks);

            return count;
        }

        public static async Task<int> GetFavoriteBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks &&
                                            x.IsFavorite);

            return count;
        }

        public static async Task<int> GetNonFavoriteBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks && 
                                            !x.IsFavorite);

            return count;
        }

        public static async Task<int> GetZeroStarBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks && 
                                            x.Rating == 0);

            return count;
        }

        public static async Task<int> GetOneStarBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks && 
                                            x.Rating == 1);

            return count;
        }

        public static async Task<int> GetTwoStarBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks && 
                                            x.Rating == 2);

            return count;
        }

        public static async Task<int> GetThreeStarBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks && 
                                            x.Rating == 3);

            return count;
        }

        public static async Task<int> GetFourStarBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks && 
                                            x.Rating == 4);

            return count;
        }

        public static async Task<int> GetFiveStarBooksListCount(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks && 
                                            x.Rating == 5);

            return count;
        }

        public static async Task<ObservableCollection<BookModel>> GetBookWishList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<ChapterModel>> GetAllChaptersInBook(Guid? inputGuid)
        {
            ObservableCollection<ChapterModel>? chapterList = null;

            if (TestData.UseTestData)
            {
                chapterList = TestData.ChapterList;
            }
            else
            {

            }

            var filteredList = chapterList.Where(x => x.BookGuid == inputGuid)
                                          .OrderBy(x => x.ChapterOrder)
                                          .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<ChapterModel>> GetAllChapters()
        {
            ObservableCollection<ChapterModel>? chapterList = null;

            if (TestData.UseTestData)
            {
                chapterList = TestData.ChapterList;
            }
            else
            {

            }

            var filteredList = chapterList.OrderBy(x => x.ChapterOrder)
                                          .OrderBy(x => x.BookGuid)
                                          .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<string>> GetAllPublishersInBookList(ObservableCollection<BookModel> bookList)
        {
            var publisherList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookPublisher) && !publisherList.Any(x => x.Equals(book.BookPublisher)))
                    publisherList.Add(book.BookPublisher);
            }

            return publisherList;
        }

        public static async Task<ObservableCollection<string>> GetAllAuthorsInBookList(ObservableCollection<BookModel> bookList)
        {
            var authorList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    string[] authorNames = book.AuthorListString.Split(";");

                    foreach (var authorName in authorNames)
                    {
                        if (!string.IsNullOrEmpty(authorName.Trim()))
                        {
                            string[] name = authorName.Split(",");

                            AuthorModel author = new()
                            {
                                FirstName = name[1].Trim(),
                                LastName = name[0].Trim()
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
                    locationList.Add(book.BookWhereToBuy);
            }

            return locationList;
        }

        public static async Task<ObservableCollection<string>> GetAllSeriesInBookList(ObservableCollection<BookModel> bookList)
        {
            var seriesList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookSeries) && !seriesList.Any(x => x.Equals(book.BookSeries)))
                    seriesList.Add(book.BookSeries);
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
                    var publishYearSubstring = book.BookPublishYear.Substring(0, 3);
                    var publishRange = $"{publishYearSubstring}0 - {publishYearSubstring}9";

                    if (!publishYearList.Any(x => x.Equals(publishRange)))
                        publishYearList.Add(publishRange);
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
                    languageList.Add(book.BookLanguage);
            }

            return languageList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>> GetAllBookAuthorsForBook(Guid? inputGuid)
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            if (TestData.UseTestData)
            {
                bookAuthorList = TestData.BookAuthorList;
            }
            else
            {

            }

            var filteredList = bookAuthorList.Where(x => x.BookGuid == inputGuid)
                                             .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>> GetAllBookAuthors()
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            if (TestData.UseTestData)
            {
                bookAuthorList = TestData.BookAuthorList;
            }
            else
            {

            }

            var filteredList = bookAuthorList.OrderBy(x => x.BookGuid)
                                             .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>> GetAllBookAuthorsForAuthor(Guid? inputGuid)
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            if (TestData.UseTestData)
            {
                bookAuthorList = TestData.BookAuthorList;
            }
            else
            {

            }

            var filteredList = bookAuthorList.Where(x => x.AuthorGuid == inputGuid)
                                             .ToObservableCollection();

            return filteredList;
        }

        public static async Task<GenreModel?> GetGenreForBook(Guid? inputGuid)
        {
            ObservableCollection<GenreModel>? genreList = null;

            if (TestData.UseTestData)
            {
                genreList = TestData.GenreList;
            }
            else
            {

            }

            var filteredList = genreList.FirstOrDefault(x => x.GenreGuid == inputGuid);

            return filteredList;
        }

        public static async Task<LocationModel?> GetLocationForBook(Guid? inputGuid)
        {
            ObservableCollection<LocationModel>? locationList = null;

            if (TestData.UseTestData)
            {
                locationList = TestData.LocationList;
            }
            else
            {

            }

            var filteredList = locationList.FirstOrDefault(x => x.LocationGuid == inputGuid);

            return filteredList;
        }

        public static async Task<SeriesModel?> GetSeriesForBook(Guid? inputGuid)
        {
            ObservableCollection<SeriesModel>? seriesList = null;

            if (TestData.UseTestData)
            {
                seriesList = TestData.SeriesList;
            }
            else
            {

            }

            var filteredList = seriesList.FirstOrDefault(x => x.SeriesGuid == inputGuid);

            return filteredList;
        }

        public static async Task<CollectionModel?> GetCollectionForBook(Guid? inputGuid)
        {
            ObservableCollection<CollectionModel>? collectionList = null;

            if (TestData.UseTestData)
            {
                collectionList = TestData.CollectionList;
            }
            else
            {

            }

            var filteredList = collectionList.FirstOrDefault(x => x.CollectionGuid == inputGuid);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInCollectionList(Guid? inputGuid,
                                                                                              bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks &&
                                                   x.BookCollectionGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutACollectionList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks && 
                                                   x.BookCollectionGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInGenreList(Guid? inputGuid,
                                                                                         bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks && 
                                                   x.BookGenreGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutAGenreList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks && 
                                                   x.BookGenreGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInSeriesList(Guid? inputGuid,
                                                                                          bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks && 
                                                   x.BookSeriesGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutASeriesList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks && 
                                                   x.BookSeriesGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInAuthorList(ObservableCollection<BookAuthorModel> bookAuthorList,
                                                                                          bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = new ObservableCollection<BookModel>();

            foreach (var bookAuthor in bookAuthorList)
            {
                filteredList.Add(bookList.FirstOrDefault(x => x.HideBook != showHiddenBooks && 
                                                              x.BookGuid == bookAuthor.BookGuid));
            }

            return filteredList.OrderBy(x => x.ParsedTitle)
                               .ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutAuthorList(string reverseAuthorName,
                                                                                               bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks && 
                                                   string.IsNullOrEmpty(x.AuthorListString) || !x.AuthorListString.Contains(reverseAuthorName))
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInLocationList(Guid? inputGuid,
                                                                                            bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks && 
                                                   x.BookLocationGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutALocationList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var filteredList = bookList.Where(x => x.HideBook != showHiddenBooks && 
                                                   x.BookLocationGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<CollectionModel>> GetAllCollectionsList(bool showHiddenCollections)
        {
            ObservableCollection<CollectionModel>? collectionList = null;

            if (TestData.UseTestData)
            {
                collectionList = TestData.CollectionList;
            }
            else
            {

            }

            var filteredList = collectionList.Where(x => x.HideCollection != showHiddenCollections)
                                             .OrderBy(x => x.CollectionName)
                                             .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllCollectionsList(bool showHiddenCollections,
                                                                                   bool showHiddenBooks,
                                                                                   int maxLimit)
        {
            ObservableCollection<CollectionModel>? collectionList = null;
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                collectionList = TestData.CollectionList;
                bookList = TestData.BookList;
            }
            else
            {

            }

            var counts = new List<CountModel>();

            var filteredList = collectionList.Where(x => x.HideCollection != showHiddenCollections &&
                                                         x.CollectionTotalBooks != 0)
                                             .OrderByDescending(x => x.CollectionTotalBooks)
                                             .ToObservableCollection();

            var max = maxLimit;

            if (filteredList.Count < max)
                max = filteredList.Count;

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
                Count = bookList.Count(x => x.BookCollectionGuid == null)
            });

            return counts;
        }

        public static async Task<ObservableCollection<CollectionModel>> SortCollectionsList(ObservableCollection<CollectionModel> collectionList,
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
                    filteredList = filteredList.OrderBy(x => x.ParsedCollectionName).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedCollectionName).ToObservableCollection();
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedCollectionName).OrderBy(x => x.CollectionTotalBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedCollectionName).OrderByDescending(x => x.CollectionTotalBooks).ToObservableCollection();
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedCollectionName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedCollectionName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<GenreModel>> GetAllGenresList(bool showHiddenGenres)
        {
            ObservableCollection<GenreModel>? genreList = null;

            if (TestData.UseTestData)
            {
                genreList = TestData.GenreList;
            }
            else
            {

            }

            var filteredList = genreList.Where(x => x.HideGenre != showHiddenGenres)
                                        .OrderBy(x => x.GenreName)
                                        .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllGenresList(bool showHiddenGenres,
                                                                              bool showHiddenBooks,
                                                                              int maxLimit)
        {
            ObservableCollection<GenreModel>? genreList = null;
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                genreList = TestData.GenreList;
                bookList = TestData.BookList;
            }
            else
            {

            }

            var counts = new List<CountModel>();

            var filteredList = genreList.Where(x => x.HideGenre != showHiddenGenres &&
                                                    x.GenreTotalBooks != 0)
                                        .OrderByDescending(x => x.GenreTotalBooks)
                                        .ToObservableCollection();

            var max = maxLimit;

            if (filteredList.Count < max)
                max = filteredList.Count;

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
                Count = bookList.Count(x => x.BookGenreGuid == null)
            });

            return counts;
        }

        public static async Task<ObservableCollection<GenreModel>> SortGenresList(ObservableCollection<GenreModel> genreList,
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
                    filteredList = filteredList.OrderBy(x => x.ParsedGenreName).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedGenreName).ToObservableCollection();
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedGenreName).OrderBy(x => x.GenreTotalBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedGenreName).OrderByDescending(x => x.GenreTotalBooks).ToObservableCollection();
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedGenreName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedGenreName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<SeriesModel>> GetAllSeriesList(bool showHiddenSeries)
        {
            ObservableCollection<SeriesModel>? seriesList = null;

            if (TestData.UseTestData)
            {
                seriesList = TestData.SeriesList;
            }
            else
            {

            }

            var filteredList = seriesList.Where(x => x.HideSeries != showHiddenSeries)
                                         .OrderBy(x => x.SeriesName)
                                         .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllSeriesList(bool showHiddenSeries,
                                                                              bool showHiddenBooks,
                                                                              int maxLimit)
        {
            ObservableCollection<SeriesModel>? seriesList = null;
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                seriesList = TestData.SeriesList;
                bookList = TestData.BookList;
            }
            else
            {

            }

            var counts = new List<CountModel>();

            var filteredList = seriesList.Where(x => x.HideSeries != showHiddenSeries &&
                                                     x.SeriesTotalBooks != 0)
                                         .OrderByDescending(x => x.SeriesTotalBooks)
                                         .ToObservableCollection();

            var max = maxLimit;

            if (filteredList.Count < max)
                max = filteredList.Count;

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
                Count = bookList.Count(x => x.BookSeriesGuid == null)
            });

            return counts;
        }

        public static async Task<ObservableCollection<SeriesModel>> SortSeriesList(ObservableCollection<SeriesModel> seriesList,
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
                    filteredList = filteredList.OrderBy(x => x.ParsedSeriesName).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedSeriesName).ToObservableCollection();
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedSeriesName).OrderBy(x => x.SeriesTotalBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedSeriesName).OrderByDescending(x => x.SeriesTotalBooks).ToObservableCollection();
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedSeriesName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedSeriesName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<LocationModel>> GetAllLocationsList(bool showHiddenLocations)
        {
            ObservableCollection<LocationModel>? locationList = null;

            if (TestData.UseTestData)
            {
                locationList = TestData.LocationList;
            }
            else
            {

            }

            var filteredList = locationList.Where(x => x.HideLocation != showHiddenLocations)
                                           .OrderBy(x => x.LocationName)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllLocationsList(bool showHiddenLocations,
                                                                                 bool showHiddenBooks,
                                                                                 int maxLimit)
        {
            ObservableCollection<LocationModel>? locationList = null;
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                locationList = TestData.LocationList;
                bookList = TestData.BookList;
            }
            else
            {

            }

            var counts = new List<CountModel>();

            var filteredList = locationList.Where(x => x.HideLocation != showHiddenLocations &&
                                                       x.LocationTotalBooks != 0)
                                           .OrderByDescending(x => x.LocationTotalBooks)
                                           .ToObservableCollection();
            var max = maxLimit;

            if (filteredList.Count < max)
                max = filteredList.Count;

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
                Count = bookList.Count(x => x.BookLocationGuid == null)
            });

            return counts;
        }

        public static async Task<ObservableCollection<LocationModel>> SortLocationsList(ObservableCollection<LocationModel> locationList,
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
                    filteredList = filteredList.OrderBy(x => x.ParsedLocationName).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedLocationName).ToObservableCollection();
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedLocationName).OrderBy(x => x.LocationTotalBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedLocationName).OrderByDescending(x => x.LocationTotalBooks).ToObservableCollection();
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.ParsedLocationName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedLocationName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<AuthorModel>> GetAllAuthorsList(bool showHiddenAuthors)
        {
            ObservableCollection<AuthorModel>? authorList = null;

            if (TestData.UseTestData)
            {
                authorList = TestData.AuthorList;
            }
            else
            {

            }

            var filteredList = authorList.Where(x => x.HideAuthor != showHiddenAuthors)
                                         .OrderBy(x => x.LastName)
                                         .OrderBy(x => x.FirstName)
                                         .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllAuthorsList(bool showHiddenAuthors,
                                                                               bool showHiddenBooks,
                                                                               int maxLimit)
        {
            ObservableCollection<AuthorModel>? authorList = null;
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                authorList = TestData.AuthorList;
                bookList = TestData.BookList;
            }
            else
            {

            }

            var counts = new List<CountModel>();

            var filteredList = authorList.Where(x => x.HideAuthor != showHiddenAuthors &&
                                                     x.AuthorTotalBooks != 0)
                                         .OrderByDescending(x => x.AuthorTotalBooks)
                                         .ToObservableCollection();

            if (!showHiddenAuthors)
                filteredList = filteredList.Where(x => !x.HideAuthor)
                                           .ToObservableCollection();

            var max = maxLimit;

            if (filteredList.Count < max)
                max = filteredList.Count;

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
                Count = bookList.Count(x => string.IsNullOrEmpty(x.AuthorListString))
            });

            return counts;
        }

        public static async Task<ObservableCollection<AuthorModel>> SortAuthorList(ObservableCollection<AuthorModel> authorList,
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
                    filteredList = filteredList.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.FirstName).OrderByDescending(x => x.LastName).ToObservableCollection();
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).OrderBy(x => x.AuthorTotalBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.FirstName).OrderByDescending(x => x.LastName).OrderByDescending(x => x.AuthorTotalBooks).ToObservableCollection();
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.FirstName).OrderByDescending(x => x.LastName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
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

            for (int i = 0; i < formatList.Count; i++)
            {
                var count = bookList.Count(x => x.HideBook != showHiddenBooks && x.BookFormat.Equals(formatList[i]));

                counts.Add(new CountModel()
                {
                    Label = formatList[i],
                    Count = count,
                });
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

            for (int i = 0; i < formatList.Count; i++)
            {
                var list = bookList.Where(x => x.HideBook != showHiddenBooks && x.BookFormat.Equals(formatList[i])).ToList();

                var price = list.Sum(x => x.BookPriceValue);

                counts.Add(new CountModel()
                {
                    Label = formatList[i],
                    CountDouble = price,
                });
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

            for (int i = 0; i < formatList.Count; i++)
            {
                var count = bookList.Count(x => x.HideBook != showHiddenBooks && x.BookFormat.Equals(formatList[i]));

                counts.Add(new CountModel()
                {
                    Label = formatList[i],
                    Count = count,
                });
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

            for (int i = 0; i < formatList.Count; i++)
            {
                var list = bookList.Where(x => x.HideBook != showHiddenBooks && x.BookFormat.Equals(formatList[i])).ToList();

                var price = list.Sum(x => x.BookPriceValue);

                counts.Add(new CountModel()
                {
                    Label = formatList[i],
                    CountDouble = price,
                });
            }

            return counts;
        }

        public static async Task<int> GetBookCountReadInYear(int year, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var count = bookList.Count(x => x.HideBook != showHiddenBooks &&
                                            !string.IsNullOrEmpty(x.BookStartDate) &&
                                            !string.IsNullOrEmpty(x.BookEndDate) &&
                                            DateTime.Parse(x.BookEndDate).Year == year);

            return count;
        }

        public static async Task<int> GetBookPageCountReadInYear(int year, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var list = bookList.Where(x => x.HideBook != showHiddenBooks &&
                                           !string.IsNullOrEmpty(x.BookStartDate) &&
                                           !string.IsNullOrEmpty(x.BookEndDate) &&
                                           DateTime.Parse(x.BookEndDate).Year == year);

            return list.Sum(x => x.BookPageTotal);
        }

        public static async Task<string> GetPriceOfAllBooks(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookList;
            }
            else
            {

            }

            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);

            var cultureInfo = new CultureInfo(cultureCode);

            var list = bookList.Where(x => x.HideBook != showHiddenBooks &&
                                           !string.IsNullOrEmpty(x.BookPrice));

            return string.Format(cultureInfo, "{0:C}", list.Sum(x => x.BookPriceValue));
        }

        public static async Task<string> GetPriceOfAllWishListBooks(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;

            if (TestData.UseTestData)
            {
                bookList = TestData.BookWishList;
            }
            else
            {

            }

            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);

            var cultureInfo = new CultureInfo(cultureCode);

            var list = bookList.Where(x => x.HideBook != showHiddenBooks &&
                                                   !string.IsNullOrEmpty(x.BookPrice));

            return string.Format(cultureInfo, "{0:C}", list.Sum(x => x.BookPriceValue));
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

            var list = bookList.Where(x => !string.IsNullOrEmpty(x.BookSeries))
                               .Select(x => x.BookSeries)
                               .Distinct()
                               .ToList();

            var max = maxLimit;

            if (list.Count < max)
                max = list.Count;

            for (int i = 0; i < max; i++)
            {
                var count = bookList.Count(x => x.BookSeries.Equals(list[i]));

                counts.Add(new CountModel()
                {
                    Label = list[i],
                    Count = count,
                });
            }

            counts.Add(new CountModel()
            {
                Label = AppStringResources.NoSeries,
                Count = bookList.Count(x => string.IsNullOrEmpty(x.BookSeries))
            });

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

            var list = bookList.Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy))
                               .Select(x => x.BookWhereToBuy)
                               .Distinct()
                               .ToList();

            var max = maxLimit;

            if (list.Count < max)
                max = list.Count;

            for (int i = 0; i < max; i++)
            {
                var count = bookList.Count(x => x.BookWhereToBuy.Equals(list[i]));

                counts.Add(new CountModel()
                {
                    Label = list[i],
                    Count = count,
                });
            }

            counts.Add(new CountModel()
            {
                Label = AppStringResources.NoLocation,
                Count = bookList.Count(x => string.IsNullOrEmpty(x.BookWhereToBuy))
            });

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

            var authorList = bookList.Where(x => !string.IsNullOrEmpty(x.AuthorListString))
                                     .Select(x => x.AuthorListString)
                                     .Distinct()
                                     .ToList();

            List<AuthorModel> list = new List<AuthorModel>();

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

                            AuthorModel author1 = new()
                            {
                                FirstName = name[1].Trim(),
                                LastName = name[0].Trim()
                            };

                            list.Add(author1);
                        }
                    }
                }
            }

            list = list.DistinctBy(x => x.FullName).ToList();

            var max = maxLimit;

            if (list.Count < max)
                max = list.Count;

            for (int i = 0; i < max; i++)
            {
                var count = bookList.Count(x => !string.IsNullOrEmpty(x.AuthorListString) &&
                                                x.AuthorListString.Contains(list[i].ReverseFullName));

                counts.Add(new CountModel()
                {
                    Label = list[i].FullName,
                    Count = count,
                });
            }

            counts.Add(new CountModel()
            {
                Label = AppStringResources.NoAuthor,
                Count = bookList.Count(x => string.IsNullOrEmpty(x.AuthorListString))
            });

            return counts;
        }
    }
}
