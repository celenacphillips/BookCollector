using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml.Linq;

namespace BookCollector.Data
{
    public partial class FilterLists : BaseViewModel
    {
        private static ObservableCollection<BookModel> FilterHiddenBooks(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filterList = bookList;

            if (!showHiddenBooks)
            {
                filterList = bookList.Where(x => !x.HideBook)
                                     .ToObservableCollection();
            }

            return filterList;
        }

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
                // Unit test data
                var author = TestData.AuthorList.FirstOrDefault(x => x.FullName.Equals(authorOption));

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
                    filteredList = filteredList.OrderBy(x => x.ParsedTitle).OrderBy(x => x.BookStartDate).OrderBy(x => x.BookEndDate).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.ParsedTitle).OrderByDescending(x => x.BookStartDate).OrderByDescending(x => x.BookEndDate).ToObservableCollection();
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

        public static async Task<ObservableCollection<BookModel>> GetReadingBooksList(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => (x.BookPageRead != x.BookPageTotal &&
                                                    x.BookPageRead != 0) ||
                                                   (x.UpNext == true))
                                   .OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<int> GetReadingBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => (x.BookPageRead != x.BookPageTotal &&
                                                    x.BookPageRead != 0) ||
                                                   (x.UpNext == true))
                                   .OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<ObservableCollection<BookModel>> GetToBeReadBooksList(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookPageRead == 0 && x.UpNext == false)
                                   .OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<int> GetToBeReadBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookPageRead == 0 && x.UpNext == false)
                                   .OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<ObservableCollection<BookModel>> GetReadBooksList(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookPageRead == x.BookPageTotal &&
                                                   x.BookPageRead != 0)
                                   .OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<int> GetReadBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookPageRead == x.BookPageTotal &&
                                                   x.BookPageRead != 0)
                                   .OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksList(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<int> GetAllBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<int> GetAllWishListBooksListCount(bool showHiddenBooks)
        {
            return TestData.BookWishList.Count(x => x.HideBook != showHiddenBooks);
        }

        public static async Task<int> GetFavoriteBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.IsFavorite)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<int> GetNonFavoriteBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => !x.IsFavorite)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<int> GetZeroStarBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.Rating == 0)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<int> GetOneStarBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.Rating == 1)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<int> GetTwoStarBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.Rating == 2)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<int> GetThreeStarBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.Rating == 3)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<int> GetFourStarBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.Rating == 4)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<int> GetFiveStarBooksListCount(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.Rating == 5)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.Count();
        }

        public static async Task<ObservableCollection<BookModel>> GetBookWishList(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<ChapterModel>> GetAllChaptersInBook(ObservableCollection<ChapterModel> chapterList,
                                                                                          Guid? inputGuid)
        {
            var filteredList = chapterList.Where(x => x.BookGuid == inputGuid)
                                          .OrderBy(x => x.ChapterOrder)
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

        public static async Task<ObservableCollection<string>> GetAllLocationsInBookList(ObservableCollection<BookModel> bookList,
                                                                                         ObservableCollection<LocationModel> locationList)
        {
            var locationNameList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (book.BookLocationGuid != null)
                {
                    var location = await GetLocationForBook(locationList, book.BookLocationGuid);

                    if (location != null && !locationNameList.Any(x => x.Equals(location.LocationName)))
                        locationNameList.Add(location.LocationName);
                }
            }

            return locationNameList;
        }

        public static async Task<ObservableCollection<string>> GetAllAuthorNamesInBookList(ObservableCollection<BookModel> bookList,
                                                                                           ObservableCollection<BookAuthorModel> bookAuthorList,
                                                                                           ObservableCollection<AuthorModel> authorList)
        {
            var authorNameList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                bookAuthorList = await GetAllBookAuthorsForBook(bookAuthorList, book.BookGuid);

                authorList = await GetAllAuthorsForBookList(bookAuthorList, authorList);

                foreach (var author in authorList)
                {
                    if (!string.IsNullOrEmpty(author.FullName) && !authorNameList.Any(x => x.Equals(author.FullName)))
                        authorNameList.Add(author.FullName);
                }
            }

            return authorNameList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>> GetAllBookAuthorsForBook(ObservableCollection<BookAuthorModel> bookAuthorList,
                                                                                                 Guid? inputGuid)
        {
            var filteredList = bookAuthorList.Where(x => x.BookGuid == inputGuid)
                                             .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>> GetAllBookAuthorsForAuthor(ObservableCollection<BookAuthorModel> bookAuthorList,
                                                                                                   Guid? inputGuid)
        {
            var filteredList = bookAuthorList.Where(x => x.AuthorGuid == inputGuid)
                                             .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<AuthorModel>> GetAllAuthorsForBookList(ObservableCollection<BookAuthorModel> bookAuthorList,
                                                                                             ObservableCollection<AuthorModel> authorList)
        {
            var filteredList = new ObservableCollection<AuthorModel>();

            foreach (var bookAuthor in bookAuthorList)
            {
                filteredList.Add(authorList.FirstOrDefault(x => x.AuthorGuid == bookAuthor.AuthorGuid));
            }

            return filteredList.OrderBy(x => x.LastName)
                               .ToObservableCollection();
        }

        public static async Task<AuthorModel?> GetAuthorByFullName(ObservableCollection<AuthorModel> authorList,
                                                                                        string fullName)
        {
            return authorList.SingleOrDefault(x => x.FullName.Equals(fullName));
        }

        public static async Task<GenreModel?> GetGenreForBook(ObservableCollection<GenreModel> genreList,
                                                              Guid? inputGuid)
        {
            var filteredList = genreList.FirstOrDefault(x => x.GenreGuid == inputGuid);

            return filteredList;
        }

        public static async Task<LocationModel?> GetLocationForBook(ObservableCollection<LocationModel> locationList,
                                                                    Guid? inputGuid)
        {
            var filteredList = locationList.FirstOrDefault(x => x.LocationGuid == inputGuid);

            return filteredList;
        }

        public static async Task<LocationModel?> GetLocationByLocationName(ObservableCollection<LocationModel> locationList,
                                                                           string locationName)
        {
            return locationList.FirstOrDefault(x => x.LocationName.Equals(locationName));
        }

        public static async Task<SeriesModel?> GetSeriesForBook(ObservableCollection<SeriesModel> seriesList,
                                                                Guid? inputGuid)
        {
            var filteredList = seriesList.FirstOrDefault(x => x.SeriesGuid == inputGuid);

            return filteredList;
        }

        public static async Task<CollectionModel?> GetCollectionForBook(ObservableCollection<CollectionModel> collectionList,
                                                                        Guid? inputGuid)
        {
            var filteredList = collectionList.FirstOrDefault(x => x.CollectionGuid == inputGuid);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInCollectionList(ObservableCollection<BookModel> bookList,
                                                                                              Guid? inputGuid,
                                                                                              bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookCollectionGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutACollectionList(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookCollectionGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInGenreList(ObservableCollection<BookModel> bookList,
                                                                                         Guid? inputGuid,
                                                                                         bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookGenreGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutAGenreList(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookGenreGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInSeriesList(ObservableCollection<BookModel> bookList,
                                                                                          Guid? inputGuid,
                                                                                          bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookSeriesGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutASeriesList(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookSeriesGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInAuthorList(ObservableCollection<BookAuthorModel> bookAuthorList,
                                                                                          ObservableCollection<BookModel> bookList,
                                                                                          bool showHiddenBooks)
        {
            var filteredList = new ObservableCollection<BookModel>();

            foreach (var bookAuthor in bookAuthorList)
            {
                filteredList.Add(bookList.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid));
            }

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList.OrderBy(x => x.ParsedTitle)
                               .ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutAuthorList(ObservableCollection<BookModel> bookList,
                                                                                               string reverseAuthorName,
                                                                                               bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => string.IsNullOrEmpty(x.AuthorListString) || !x.AuthorListString.Contains(reverseAuthorName))
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInLocationList(ObservableCollection<BookModel> bookList,
                                                                                            Guid? inputGuid,
                                                                                            bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookLocationGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutALocationList(ObservableCollection<BookModel> bookList, bool showHiddenBooks)
        {
            var filteredList = bookList.Where(x => x.BookLocationGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            return filteredList;
        }

        public static async Task<ObservableCollection<CollectionModel>> GetAllCollectionsList(ObservableCollection<CollectionModel> collectionList,
                                                                                              bool showHiddenCollections)
        {
            var filteredList = collectionList.OrderBy(x => x.CollectionName)
                                             .ToObservableCollection();

            if (!showHiddenCollections)
                filteredList = filteredList.Where(x => !x.HideCollection)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllCollectionsList(ObservableCollection<CollectionModel> collectionList,
                                                                                   bool showHiddenCollections,
                                                                                   bool showHiddenBooks)
        {
            var counts = new List<CountModel>();

            var filteredList = collectionList.OrderByDescending(x => x.CollectionTotalBooks)
                                             .ToObservableCollection();

            if (!showHiddenCollections)
                filteredList = filteredList.Where(x => !x.HideCollection)
                                           .ToObservableCollection();

            var max = 5;

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
                Count = TestData.BookList.Where(x => x.BookCollectionGuid == null).Count()
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
                    filteredList = filteredList.OrderBy(x => x.CollectionName).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.CollectionName).ToObservableCollection();
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.CollectionName).OrderBy(x => x.CollectionTotalBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.CollectionName).OrderByDescending(x => x.CollectionTotalBooks).ToObservableCollection();
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.CollectionName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.CollectionName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<GenreModel>> GetAllGenresList(ObservableCollection<GenreModel> genreList,
                                                                                    bool showHiddenGenres)
        {
            var filteredList = genreList.OrderBy(x => x.GenreName)
                                        .ToObservableCollection();

            if (!showHiddenGenres)
                filteredList = filteredList.Where(x => !x.HideGenre)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllGenresList(ObservableCollection<GenreModel> genreList,
                                                                 bool showHiddenGenres,
                                                                 bool showHiddenBooks)
        {
            var counts = new List<CountModel>();

            var filteredList = genreList.OrderByDescending(x => x.GenreTotalBooks)
                                        .ToObservableCollection();

            if (!showHiddenGenres)
                filteredList = filteredList.Where(x => !x.HideGenre)
                                           .ToObservableCollection();

            var max = 5;

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
                Count = TestData.BookList.Where(x => x.BookGenreGuid == null).Count()
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
                    filteredList = filteredList.OrderBy(x => x.GenreName).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.GenreName).ToObservableCollection();
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.GenreName).OrderBy(x => x.GenreTotalBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.GenreName).OrderByDescending(x => x.GenreTotalBooks).ToObservableCollection();
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.GenreName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.GenreName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<SeriesModel>> GetAllSeriesList(ObservableCollection<SeriesModel> seriesList,
                                                                                     bool showHiddenSeries)
        {
            var filteredList = seriesList.OrderBy(x => x.SeriesName)
                                         .ToObservableCollection();

            if (!showHiddenSeries)
                filteredList = filteredList.Where(x => !x.HideSeries)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllSeriesList(ObservableCollection<SeriesModel> seriesList,
                                                                              bool showHiddenSeries,
                                                                              bool showHiddenBooks)
        {
            var counts = new List<CountModel>();

            var filteredList = seriesList.OrderByDescending(x => x.SeriesTotalBooks)
                                         .ToObservableCollection();

            if (!showHiddenSeries)
                filteredList = filteredList.Where(x => !x.HideSeries)
                                           .ToObservableCollection();

            var max = 5;

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
                Count = TestData.BookList.Where(x => x.BookSeriesGuid == null).Count()
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
                    filteredList = filteredList.OrderBy(x => x.SeriesName).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.SeriesName).ToObservableCollection();
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.SeriesName).OrderBy(x => x.SeriesTotalBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.SeriesName).OrderByDescending(x => x.SeriesTotalBooks).ToObservableCollection();
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.SeriesName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.SeriesName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<LocationModel>> GetAllLocationsList(ObservableCollection<LocationModel> locationList,
                                                                                          bool showHiddenLocations)
        {
            var filteredList = locationList.OrderBy(x => x.LocationName)
                                           .ToObservableCollection();

            if (!showHiddenLocations)
                filteredList = filteredList.Where(x => !x.HideLocation)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllLocationsList(ObservableCollection<LocationModel> locationList,
                                                                                 bool showHiddenLocations,
                                                                                 bool showHiddenBooks)
        {
            var counts = new List<CountModel>();

            var filteredList = locationList.OrderByDescending(x => x.LocationTotalBooks)
                                           .ToObservableCollection();

            if (!showHiddenLocations)
                filteredList = filteredList.Where(x => !x.HideLocation)
                                           .ToObservableCollection();

            var max = 5;

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
                Count = TestData.BookList.Where(x => x.BookLocationGuid == null).Count()
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
                    filteredList = filteredList.OrderBy(x => x.LocationName).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.LocationName).ToObservableCollection();
            }

            if (totalBooksChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.LocationName).OrderBy(x => x.LocationTotalBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.LocationName).OrderByDescending(x => x.LocationTotalBooks).ToObservableCollection();
            }

            if (totalPriceChecked)
            {
                if (ascendingChecked)
                    filteredList = filteredList.OrderBy(x => x.LocationName).OrderBy(x => x.TotalCostOfBooks).ToObservableCollection();

                if (descendingChecked)
                    filteredList = filteredList.OrderByDescending(x => x.LocationName).OrderByDescending(x => x.TotalCostOfBooks).ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<AuthorModel>> GetAllAuthorsList(ObservableCollection<AuthorModel> authorList,
                                                                                      bool showHiddenAuthors)
        {
            var filteredList = authorList.OrderBy(x => x.LastName)
                                         .OrderBy(x => x.FirstName)
                                         .ToObservableCollection();

            if (!showHiddenAuthors)
                filteredList = filteredList.Where(x => !x.HideAuthor)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<List<CountModel>> GetAllBooksInAllAuthorsList(ObservableCollection<AuthorModel> authorList,
                                                                               bool showHiddenAuthors,
                                                                               bool showHiddenBooks)
        {
            var counts = new List<CountModel>();

            var filteredList = authorList.OrderByDescending(x => x.AuthorTotalBooks)
                                         .ToObservableCollection();

            if (!showHiddenAuthors)
                filteredList = filteredList.Where(x => !x.HideAuthor)
                                           .ToObservableCollection();

            var max = 5;

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
                Count = TestData.BookList.Where(x => string.IsNullOrEmpty(x.AuthorListString)).Count()
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
            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            for (int i = 0; i < formatList.Count; i++)
            {
                var count = TestData.BookList.Count(x => x.HideBook != showHiddenBooks && x.BookFormat.Equals(formatList[i]));

                counts.Add(new CountModel()
                {
                    Label = formatList[i],
                    Count = count,
                });
            }

            return counts;
        }

        public static List<CountModel> GetAllWishListBooksAndBookFormatsList(bool showHiddenBooks)
        {
            var counts = new List<CountModel>();

            var formatList = BookBaseViewModel.bookFormats;

            for (int i = 0; i < formatList.Count; i++)
            {
                var count = TestData.BookWishList.Count(x => x.HideBook != showHiddenBooks && x.BookFormat.Equals(formatList[i]));

                counts.Add(new CountModel()
                {
                    Label = formatList[i],
                    Count = count,
                });
            }

            return counts;
        }

        public static async Task<int> GetBookCountReadInYear(int year, bool showHiddenBooks)
        {
            var count = TestData.BookList.Count(x => x.HideBook != showHiddenBooks &&
                                                x.BookStartDate != null &&
                                                x.BookEndDate != null &&
                                                DateTime.Parse(x.BookEndDate).Year == year);


            return count;
        }

        public static async Task<int> GetBookPageCountReadInYear(int year, bool showHiddenBooks)
        {
            var list = TestData.BookList.Where(x => x.HideBook != showHiddenBooks &&
                                                x.BookStartDate != null &&
                                                x.BookEndDate != null &&
                                                DateTime.Parse(x.BookEndDate).Year == year);

            return list.Sum(x => x.BookPageTotal);
        }

        public static async Task<string> GetPriceOfAllBooks(bool showHiddenBooks)
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);

            var cultureInfo = new CultureInfo(cultureCode);

            var list = TestData.BookList.Where(x => x.HideBook != showHiddenBooks &&
                                               !string.IsNullOrEmpty(x.BookPrice));

            return string.Format(cultureInfo, "{0:C}", list.Sum(x => x.BookPriceValue));
        }

        public static async Task<string> GetPriceOfAllWishListBooks(bool showHiddenBooks)
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);

            var cultureInfo = new CultureInfo(cultureCode);

            var list = TestData.BookWishList.Where(x => x.HideBook != showHiddenBooks &&
                                                   !string.IsNullOrEmpty(x.BookPrice));

            return string.Format(cultureInfo, "{0:C}", list.Sum(x => x.BookPriceValue));
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndSeriesList(bool showHiddenBooks)
        {
            var counts = new List<CountModel>();

            var list = TestData.BookWishList.Select(x => x.BookSeries).Distinct().ToList();

            var max = 5;

            if (list.Count < max)
                max = list.Count;

            for (int i = 0; i < max; i++)
            {
                var count = TestData.BookWishList.Count(x => x.BookSeries.Equals(list[i]));

                counts.Add(new CountModel()
                {
                    Label = list[i],
                    Count = count,
                });
            }

            counts.Add(new CountModel()
            {
                Label = AppStringResources.NoSeries,
                Count = TestData.BookWishList.Where(x => string.IsNullOrEmpty(x.BookSeries)).Count()
            });

            return counts;
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndLocationList(bool showHiddenBooks)
        {
            var counts = new List<CountModel>();

            var list = TestData.BookWishList.Select(x => x.BookWhereToBuy).Distinct().ToList();

            var max = 5;

            if (list.Count < max)
                max = list.Count;

            for (int i = 0; i < max; i++)
            {
                var count = TestData.BookWishList.Count(x => x.BookWhereToBuy.Equals(list[i]));

                counts.Add(new CountModel()
                {
                    Label = list[i],
                    Count = count,
                });
            }

            counts.Add(new CountModel()
            {
                Label = AppStringResources.NoLocation,
                Count = TestData.BookWishList.Where(x => string.IsNullOrEmpty(x.BookWhereToBuy)).Count()
            });

            return counts;
        }

        public static async Task<List<CountModel>> GetAllWishListBooksAndAuthorList(bool showHiddenBooks)
        {
            var counts = new List<CountModel>();

            var authorList = TestData.BookWishList.Select(x => x.AuthorListString).Distinct().ToList();

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

            var max = 5;

            if (list.Count < max)
                max = list.Count;

            for (int i = 0; i < max; i++)
            {
                var count = TestData.BookWishList.Count(x => !string.IsNullOrEmpty(x.AuthorListString) &&
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
                Count = TestData.BookWishList.Where(x => string.IsNullOrEmpty(x.AuthorListString)).Count()
            });

            return counts;
        }
    }
}
