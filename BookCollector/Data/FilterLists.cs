using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

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
                                                                        ObservableCollection<AuthorModel> authorList,
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
                var author = GetAuthorByFullName(authorList, authorOption).Result;

                if (author != null)
                {
                    filterList = bookList.Where(x => !string.IsNullOrEmpty(x.AuthorListString) && x.AuthorListString.Contains(author.ReverseFullName))
                                         .ToObservableCollection();
                }
            }

            return filterList;
        }

        private static ObservableCollection<BookModel> FilterBookLocation(ObservableCollection<BookModel> bookList,
                                                                          ObservableCollection<LocationModel> locationList,
                                                                          string locationOption)
        {
            var filterList = bookList;

            if (locationOption.Equals(AppStringResources.NoLocation))
            {
                filterList = bookList.Where(x => x.BookLocationGuid == null)
                                     .ToObservableCollection();
            }

            if (!locationOption.Equals(AppStringResources.NoLocation) && !locationOption.Equals(AppStringResources.AllLocations))
            {
                var location = GetLocationByLocationName(locationList, locationOption).Result;

                if (location != null)
                {
                    filterList = bookList.Where(x => x.BookLocationGuid != null && x.BookLocationGuid == location.LocationGuid)
                                         .ToObservableCollection();
                }
            }

            return filterList;
        }

        public static async Task<ObservableCollection<BookModel>> FilterBookList(ObservableCollection<BookModel> bookList,
                                                                                 bool showHiddenBooks,
                                                                                 string favoritesOption,
                                                                                 string formatOption,
                                                                                 string publisherOption,
                                                                                 string languageOption,
                                                                                 string ratingOption,
                                                                                 string publishYearOption)
        {
            var filteredList = bookList;

            filteredList = FilterHiddenBooks(filteredList, showHiddenBooks);

            filteredList = FilterFavoriteBooks(filteredList, favoritesOption);

            filteredList = FilterBookFormat(filteredList, formatOption);

            filteredList = FilterBookPublisher(filteredList, publisherOption);

            filteredList = FilterBookLanguage(filteredList, languageOption);

            filteredList = FilterBookRating(filteredList, ratingOption);

            filteredList = FilterBookPublishYear(filteredList, publishYearOption);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetReadingBooksList(ObservableCollection<BookModel> bookList)
        {
            var fullList = bookList.Where(x => (x.BookPageRead != x.BookPageTotal &&
                                                    x.BookPageRead != 0) ||
                                                   (x.UpNext == true))
                                   .OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            return fullList;
        }

        public static async Task<ObservableCollection<BookModel>> GetToBeReadBooksList(ObservableCollection<BookModel> bookList)
        {
            var fullList = bookList.Where(x => x.BookPageRead == 0 && x.UpNext == false)
                                   .OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            return fullList;
        }

        public static async Task<ObservableCollection<BookModel>> GetReadBooksList(ObservableCollection<BookModel> bookList)
        {
            var fullList = bookList.Where(x => x.BookPageRead == x.BookPageTotal &&
                                                   x.BookPageRead != 0)
                                   .OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            return fullList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksList(ObservableCollection<BookModel> bookList)
        {
            var fullList = bookList.OrderBy(x => x.ParsedTitle)
                                   .ToObservableCollection();

            return fullList;
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
                                                                                              Guid? inputGuid)
        {
            var filteredList = bookList.Where(x => x.BookCollectionGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutACollectionList(ObservableCollection<BookModel> bookList)
        {
            var filteredList = bookList.Where(x => x.BookCollectionGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInGenreList(ObservableCollection<BookModel> bookList,
                                                                                         Guid? inputGuid)
        {
            var filteredList = bookList.Where(x => x.BookGenreGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutAGenreList(ObservableCollection<BookModel> bookList)
        {
            var filteredList = bookList.Where(x => x.BookGenreGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInSeriesList(ObservableCollection<BookModel> bookList,
                                                                                          Guid? inputGuid)
        {
            var filteredList = bookList.Where(x => x.BookSeriesGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutASeriesList(ObservableCollection<BookModel> bookList)
        {
            var filteredList = bookList.Where(x => x.BookSeriesGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInAuthorList(ObservableCollection<BookAuthorModel> bookAuthorList,
                                                                                          ObservableCollection<BookModel> bookList)
        {
            var filteredList = new ObservableCollection<BookModel>();

            foreach (var bookAuthor in bookAuthorList)
            {
                filteredList.Add(bookList.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid));
            }

            return filteredList.OrderBy(x => x.ParsedTitle)
                               .ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutAuthorList(ObservableCollection<BookModel> bookList,
                                                                                               string reverseAuthorName)
        {
            var filteredList = bookList.Where(x => string.IsNullOrEmpty(x.AuthorListString) || !x.AuthorListString.Contains(reverseAuthorName))
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();


            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInLocationList(ObservableCollection<BookModel> bookList,
                                                                                            Guid? inputGuid)
        {
            var filteredList = bookList.Where(x => x.BookLocationGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutALocationList(ObservableCollection<BookModel> bookList)
        {
            var filteredList = bookList.Where(x => x.BookLocationGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

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
    }
}
