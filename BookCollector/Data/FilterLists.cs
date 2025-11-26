using BookCollector.Data.Models;
using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

namespace BookCollector.Data
{
    internal class FilterLists
    {
        public static async Task<ObservableCollection<BookModel>> GetReadingBooksList(ObservableCollection<BookModel> bookList)
        {
            return bookList.Where(x => (x.BookPageRead != x.BookPageTotal &&
                                   x.BookPageRead != 0) ||
                                   (x.UpNext == true)).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetToBeReadBooksList(ObservableCollection<BookModel> bookList)
        {
            return bookList.Where(x => x.BookPageRead == 0 && x.UpNext == false).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetReadBooksList(ObservableCollection<BookModel> bookList)
        {
            return bookList.Where(x => x.BookPageRead == x.BookPageTotal &&
                                   x.BookPageRead != 0).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksList(ObservableCollection<BookModel> bookList)
        {
            return bookList.ToObservableCollection();
        }

        public static async Task<ObservableCollection<ChapterModel>> GetAllChaptersInBook(ObservableCollection<ChapterModel> chapterList, Guid? inputGuid)
        {
            return chapterList.Where(x => x.BookGuid == inputGuid).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookAuthorModel>> GetAllBookAuthorsForBook(ObservableCollection<BookAuthorModel> bookAuthorList, Guid? inputGuid)
        {
            return bookAuthorList.Where(x => x.BookGuid == inputGuid).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookAuthorModel>> GetAllBookAuthorsForAuthor(ObservableCollection<BookAuthorModel> bookAuthorList, Guid? inputGuid)
        {
            return bookAuthorList.Where(x => x.AuthorGuid == inputGuid).ToObservableCollection();
        }

        public static async Task<ObservableCollection<AuthorModel>> GetAllAuthorsForBook(ObservableCollection<BookAuthorModel> bookAuthorList, ObservableCollection<AuthorModel> authorList, Guid? inputGuid)
        {
            var returnAuthorList = new ObservableCollection<AuthorModel>();

            foreach (var bookAuthor in bookAuthorList)
            {
                returnAuthorList.Add(authorList.FirstOrDefault(x => x.AuthorGuid == bookAuthor.AuthorGuid));
            }

            return returnAuthorList;
        }

        public static async Task<GenreModel?> GetGenreForBook(ObservableCollection<GenreModel> genreList, Guid? inputGuid)
        {
            return genreList.FirstOrDefault(x => x.GenreGuid == inputGuid);
        }

        public static async Task<LocationModel?> GetLocationForBook(ObservableCollection<LocationModel> locationList, Guid? inputGuid)
        {
            return locationList.FirstOrDefault(x => x.LocationGuid == inputGuid);
        }

        public static async Task<SeriesModel?> GetSeriesForBook(ObservableCollection<SeriesModel> seriesList, Guid? inputGuid)
        {
            return seriesList.FirstOrDefault(x => x.SeriesGuid == inputGuid);
        }

        public static async Task<CollectionModel?> GetCollectionForBook(ObservableCollection<CollectionModel> collectionList, Guid? inputGuid)
        {
            return collectionList.FirstOrDefault(x => x.CollectionGuid == inputGuid);
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInCollectionList(ObservableCollection<BookModel> bookList, Guid? inputGuid)
        {
            return bookList.Where(x => x.BookCollectionGuid == inputGuid).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInGenreList(ObservableCollection<BookModel> bookList, Guid? inputGuid)
        {
            return bookList.Where(x => x.BookGenreGuid == inputGuid).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInSeriesList(ObservableCollection<BookModel> bookList, Guid? inputGuid)
        {
            return bookList.Where(x => x.BookSeriesGuid == inputGuid).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInAuthorList(ObservableCollection<BookAuthorModel> bookAuthorList, ObservableCollection<BookModel> bookList)
        {
            var returnBookList = new ObservableCollection<BookModel>();

            foreach (var bookAuthor in bookAuthorList)
            {
                returnBookList.Add(bookList.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid));
            }

            return returnBookList;
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksInLocationList(ObservableCollection<BookModel> bookList, Guid? inputGuid)
        {
            return bookList.Where(x => x.BookLocationGuid == inputGuid).ToObservableCollection();
        }

        public static async Task<ObservableCollection<CollectionModel>> GetAllCollectionsList(ObservableCollection<CollectionModel> collectionList)
        {
            return collectionList.ToObservableCollection();
        }

        public static async Task<ObservableCollection<GenreModel>> GetAllGenresList(ObservableCollection<GenreModel> genreList)
        {
            return genreList.ToObservableCollection();
        }

        public static async Task<ObservableCollection<SeriesModel>> GetAllSeriesList(ObservableCollection<SeriesModel> seriesList)
        {
            return seriesList.ToObservableCollection();
        }

        public static async Task<ObservableCollection<LocationModel>> GetAllLocationsList(ObservableCollection<LocationModel> locationList)
        {
            return locationList.ToObservableCollection();
        }

        public static async Task<ObservableCollection<AuthorModel>> GetAllAuthorsList(ObservableCollection<AuthorModel> authorList)
        {
            return authorList.ToObservableCollection();
        }
    }
}
