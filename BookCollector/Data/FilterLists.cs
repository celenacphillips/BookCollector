using BookCollector.Data.Models;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

namespace BookCollector.Data
{
    public partial class FilterLists : BaseViewModel
    {
        public static async Task<ObservableCollection<BookModel>> GetReadingBooksList(ObservableCollection<BookModel> bookList, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => (x.BookPageRead != x.BookPageTotal &&
                                                    x.BookPageRead != 0) ||
                                                   (x.UpNext == true))
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetToBeReadBooksList(ObservableCollection<BookModel> bookList, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookPageRead == 0 && x.UpNext == false)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetReadBooksList(ObservableCollection<BookModel> bookList, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookPageRead == x.BookPageTotal &&
                                                   x.BookPageRead != 0)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksList(ObservableCollection<BookModel> bookList, bool showHiddenBooks = true)
        {
            var filteredList = bookList.OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<ChapterModel>> GetAllChaptersInBook(ObservableCollection<ChapterModel> chapterList, Guid? inputGuid)
        {
            var filteredList = chapterList.Where(x => x.BookGuid == inputGuid)
                                          .OrderBy(x => x.ChapterOrder)
                                          .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>> GetAllBookAuthorsForBook(ObservableCollection<BookAuthorModel> bookAuthorList, Guid? inputGuid)
        {
            var filteredList = bookAuthorList.Where(x => x.BookGuid == inputGuid)
                                             .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>> GetAllBookAuthorsForAuthor(ObservableCollection<BookAuthorModel> bookAuthorList, Guid? inputGuid)
        {
            var filteredList = bookAuthorList.Where(x => x.AuthorGuid == inputGuid)
                                             .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<AuthorModel>> GetAllAuthorsForBook(ObservableCollection<BookAuthorModel> bookAuthorList, ObservableCollection<AuthorModel> authorList, Guid? inputGuid)
        {
            var filteredList = new ObservableCollection<AuthorModel>();

            foreach (var bookAuthor in bookAuthorList)
            {
                filteredList.Add(authorList.FirstOrDefault(x => x.AuthorGuid == bookAuthor.AuthorGuid));
            }

            return filteredList.OrderBy(x => x.LastName)
                               .ToObservableCollection();
        }

        public static async Task<GenreModel?> GetGenreForBook(ObservableCollection<GenreModel> genreList, Guid? inputGuid)
        {
            var filteredList = genreList.FirstOrDefault(x => x.GenreGuid == inputGuid);

            return filteredList;
        }

        public static async Task<LocationModel?> GetLocationForBook(ObservableCollection<LocationModel> locationList, Guid? inputGuid)
        {
            var filteredList = locationList.FirstOrDefault(x => x.LocationGuid == inputGuid);

            return filteredList;
        }

        public static async Task<SeriesModel?> GetSeriesForBook(ObservableCollection<SeriesModel> seriesList, Guid? inputGuid)
        {
            var filteredList = seriesList.FirstOrDefault(x => x.SeriesGuid == inputGuid);

            return filteredList;
        }

        public static async Task<CollectionModel?> GetCollectionForBook(ObservableCollection<CollectionModel> collectionList, Guid? inputGuid)
        {
            var filteredList = collectionList.FirstOrDefault(x => x.CollectionGuid == inputGuid);

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInCollectionList(ObservableCollection<BookModel> bookList, Guid? inputGuid, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookCollectionGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutACollectionList(ObservableCollection<BookModel> bookList, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookCollectionGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInGenreList(ObservableCollection<BookModel> bookList, Guid? inputGuid, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookGenreGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutAGenreList(ObservableCollection<BookModel> bookList, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookGenreGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInSeriesList(ObservableCollection<BookModel> bookList, Guid? inputGuid, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookSeriesGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutASeriesList(ObservableCollection<BookModel> bookList, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookSeriesGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInAuthorList(ObservableCollection<BookAuthorModel> bookAuthorList, ObservableCollection<BookModel> bookList, bool showHiddenBooks = true)
        {
            var filteredList = new ObservableCollection<BookModel>();

            foreach (var bookAuthor in bookAuthorList)
            {
                filteredList.Add(bookList.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid));
            }

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList.OrderBy(x => x.ParsedTitle)
                               .ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutAuthorList(ObservableCollection<BookModel> bookList, string reverseAuthorName, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => string.IsNullOrEmpty(x.AuthorListString) || !x.AuthorListString.Contains(reverseAuthorName))
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInLocationList(ObservableCollection<BookModel> bookList, Guid? inputGuid, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookLocationGuid == inputGuid)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksWithoutALocationList(ObservableCollection<BookModel> bookList, bool showHiddenBooks = true)
        {
            var filteredList = bookList.Where(x => x.BookLocationGuid == null)
                                       .OrderBy(x => x.ParsedTitle)
                                       .ToObservableCollection();

            if (!showHiddenBooks)
                filteredList = filteredList.Where(x => !x.HideBook)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<CollectionModel>> GetAllCollectionsList(ObservableCollection<CollectionModel> collectionList, bool showHiddenCollections = true)
        {
            var filteredList = collectionList.OrderBy(x => x.CollectionName)
                                             .ToObservableCollection();

            if (!showHiddenCollections)
                filteredList = filteredList.Where(x => !x.HideCollection)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<GenreModel>> GetAllGenresList(ObservableCollection<GenreModel> genreList, bool showHiddenGenres = true)
        {
            var filteredList = genreList.OrderBy(x => x.GenreName)
                                        .ToObservableCollection();

            if (!showHiddenGenres)
                filteredList = filteredList.Where(x => !x.HideGenre)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<SeriesModel>> GetAllSeriesList(ObservableCollection<SeriesModel> seriesList, bool showHiddenSeries = true)
        {
            var filteredList = seriesList.OrderBy(x => x.SeriesName)
                                         .ToObservableCollection();

            if (!showHiddenSeries)
                filteredList = filteredList.Where(x => !x.HideSeries)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<LocationModel>> GetAllLocationsList(ObservableCollection<LocationModel> locationList, bool showHiddenLocations = true)
        {
            var filteredList = locationList.OrderBy(x => x.LocationName)
                                           .ToObservableCollection();

            if (!showHiddenLocations)
                filteredList = filteredList.Where(x => !x.HideLocation)
                                           .ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<AuthorModel>> GetAllAuthorsList(ObservableCollection<AuthorModel> authorList, bool showHiddenAuthors = true)
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
