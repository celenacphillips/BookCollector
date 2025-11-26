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

        // TO DO
        public static async Task<ObservableCollection<BookModel>> GetAllBooksInAuthorList(ObservableCollection<BookModel> bookList, Guid? inputGuid)
        {
            return bookList.Where(x => x.BookCollectionGuid == inputGuid).ToObservableCollection();
        }

        // TO DO
        public static async Task<ObservableCollection<BookModel>> GetAllBooksInLocationList(ObservableCollection<BookModel> bookList, Guid? inputGuid)
        {
            return bookList.Where(x => x.BookCollectionGuid == inputGuid).ToObservableCollection();
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
