using System.Collections.ObjectModel;
using BookCollector.Data.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookCollector.Data
{
    public partial class TestData : ObservableObject
    {
        internal static ObservableCollection<BookModel> BookList = [];
        internal static ObservableCollection<ChapterModel> ChapterList = [];
        internal static ObservableCollection<AuthorModel> AuthorList = [];
        internal static ObservableCollection<SeriesModel> SeriesList = [];
        internal static ObservableCollection<GenreModel> GenreList = [];
        internal static ObservableCollection<CollectionModel> CollectionList = [];
        internal static ObservableCollection<LocationModel> LocationList = [];
        internal static ObservableCollection<BookAuthorModel> BookAuthorList = [];
        internal static ObservableCollection<BookModel> BookWishList = [];

        public TestData()
        {
        }

        public static bool UseTestData { get; set; }

        public static void AddBooksToList()
        {
            AddChaptersToList();
            AddAuthorsToList();
            AddSeriesToList();
            AddGenresToList();
            AddCollectionsToList();
            AddLocationsToList();

            BookList =
           [
                new BookModel()
                {
                    BookTitle = "Reading Book",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    BookStartDate = "11/13/2025",
                    BookPageRead = 5,
                    BookPublisher = "Publisher1",
                    BookPublishYear = "2025",
                    BookIdentifier = "1234",
                    BookLanguage = "english",
                    BookPrice = "$10.00",
                    BookURL = "test.com",
                    BookSummary = "Text",
                    BookComments = "Comments",
                    Rating = 2,
                    IsFavorite = true,
                    BookGenreGuid = GenreList[0].GenreGuid,
                    BookCollectionGuid = CollectionList[0].CollectionGuid,
                    BookSeriesGuid = SeriesList[0].SeriesGuid,
                },
                new BookModel()
                {
                    BookTitle = "A Read Book",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    BookStartDate = "11/13/2025",
                    BookEndDate = "11/14/2025",
                    BookPageRead = 100,
                    BookPublisher = "Publisher1",
                    BookPublishYear = "2000",
                    BookIdentifier = "1234",
                    BookLanguage = "english",
                    BookPrice = "$10.00",
                    BookURL = "test.com",
                    BookSummary = "Text",
                    BookComments = "Comments",
                    Rating = 4,
                    IsFavorite = false,
                },
                new BookModel()
                {
                    BookTitle = "To Be Read Book",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    BookPageRead = 0,
                    BookPublisher = "Publisher2",
                    BookPublishYear = "1990",
                    BookIdentifier = "1234",
                    BookLanguage = "english",
                    BookPrice = "$10.00",
                    BookURL = "test.com",
                    BookSummary = "Text",
                    BookComments = "Comments",
                    Rating = 5,
                    IsFavorite = true,
                },
            ];

            foreach (var chapter in ChapterList)
            {
                if (BookList != null && BookList[0].BookGuid != null)
                {
                    chapter.BookGuid = BookList[0].BookGuid.Value;
                }
            }

            if (BookList != null && BookList[0].BookGuid != null && AuthorList != null && AuthorList[0].AuthorGuid != null)
            {
                BookAuthorList =
                [
                    new BookAuthorModel()
                    {
                        BookGuid = BookList[0].BookGuid.Value,
                        AuthorGuid = AuthorList[0].AuthorGuid.Value,
                    },
                ];

                foreach (var book in BookList)
                {
                    book.SetReadingProgress();
                    book.SetCoverDisplay();
                    using var list = book.SetAuthorListstring();
                }

                var showHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);

                foreach (var author in AuthorList)
                {
                    using var list = author.SetTotalBooks(showHiddenBooks);
                }

                foreach (var series in SeriesList)
                {
                    using var list = series.SetTotalBooks(showHiddenBooks);
                }

                foreach (var genre in GenreList)
                {
                    using var list = genre.SetTotalBooks(showHiddenBooks);
                }

                foreach (var collection in CollectionList)
                {
                    using var list = collection.SetTotalBooks(showHiddenBooks);
                }

                foreach (var location in LocationList)
                {
                    using var list = location.SetTotalBooks(showHiddenBooks);
                }
            }
        }

        public static void UpdateBook(BookModel book)
        {
            var oldBook = BookList.Where(x => x.BookGuid == book.BookGuid).ToList().FirstOrDefault();

            if (oldBook != null)
            {
                var index = BookList.IndexOf(oldBook);
                BookList.Remove(oldBook);
                BookList.Insert(index, book);
            }
            else
            {
                book.SetReadingProgress();
                book.SetCoverDisplay();
                InsertBook(book);
            }
        }

        public static void InsertBook(BookModel book)
        {
            if (book.BookGuid == null)
            {
                book.BookGuid = Guid.NewGuid();
            }

            BookList.Add(book);
        }

        public static void DeleteBook(BookModel book)
        {
            BookList.Remove(book);
        }

        public static void AddWishListBooksToList()
        {
            BookWishList =
           [
                new BookModel()
                {
                    BookTitle = "Book 1",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    BookPublisher = "Publisher",
                    BookPublishYear = "2025",
                    BookIdentifier = "1234",
                    BookLanguage = "english",
                    BookPrice = "$15.00",
                    BookURL = "test.com",
                    BookSummary = "Text",
                    BookComments = "Comments",
                    BookSeries = "Series1",
                    BookNumberInSeries = 1,
                    BookWhereToBuy = "website",
                },
                new BookModel()
                {
                    BookTitle = "Book 2",
                    BookPageTotal = 500,
                    BookFormat = "Hardcover",
                    BookPublisher = "Publisher1",
                    BookPublishYear = "1978",
                    BookIdentifier = "1234",
                    BookLanguage = "english",
                    BookPrice = "$25.00",
                    BookURL = "test.com",
                    BookSummary = "Text",
                    BookComments = "Comments",
                    BookSeries = "Series1",
                    BookNumberInSeries = 2,
                    BookWhereToBuy = "website",
                },
                new BookModel()
                {
                    BookTitle = "Book 3",
                    BookPageTotal = 450,
                    BookFormat = "Paperback",
                    BookPublisher = "Publisher2",
                    BookPublishYear = "2020",
                    BookIdentifier = "1234",
                    BookLanguage = "english",
                    BookPrice = "$50.00",
                    BookURL = "test.com",
                    BookSummary = "Text",
                    BookComments = "Comments",
                    BookSeries = "Series2",
                    BookNumberInSeries = 1,
                    BookWhereToBuy = "website",
                },
            ];

            foreach (var book in BookWishList)
            {
                book.SetCoverDisplay();
            }
        }

        public static void UpdateWishListBook(BookModel book)
        {
            var oldBook = BookWishList.Where(x => x.BookGuid == book.BookGuid).ToList().FirstOrDefault();

            if (oldBook != null)
            {
                var index = BookWishList.IndexOf(oldBook);
                BookWishList.Remove(oldBook);
                BookWishList.Insert(index, book);
            }
            else
            {
                book.SetReadingProgress();
                book.SetCoverDisplay();
                InsertWishListBook(book);
            }
        }

        public static void InsertWishListBook(BookModel book)
        {
            if (book.BookGuid == null)
            {
                book.BookGuid = Guid.NewGuid();
            }

            BookWishList.Add(book);
        }

        public static void DeleteWishListBook(BookModel book)
        {
            BookWishList.Remove(book);
        }

        public static void AddChaptersToList()
        {
            ChapterList =
           [
                new ChapterModel()
                {
                    ChapterName = "Chapter One",
                    PageRange = "1-5",
                    ChapterOrder = 0,
                },
                new ChapterModel()
                {
                    ChapterName = "Chapter Two",
                    PageRange = "5-10",
                    ChapterOrder = 1,
                }

            ];
        }

        public static void UpdateChapter(ChapterModel chapter)
        {
            var oldChapter = ChapterList.Where(x => x.ChapterGuid == chapter.ChapterGuid).ToList().FirstOrDefault();

            if (oldChapter != null)
            {
                var index = ChapterList.IndexOf(oldChapter);
                ChapterList.Remove(oldChapter);
                ChapterList.Insert(index, chapter);
            }
            else
            {
                InsertChapter(chapter);
            }
        }

        public static void InsertChapter(ChapterModel chapter)
        {
            if (chapter.ChapterGuid == null)
            {
                chapter.ChapterGuid = Guid.NewGuid();
            }

            ChapterList.Add(chapter);
        }

        public static void DeleteChapter(ChapterModel chapter)
        {
            ChapterList.Remove(chapter);
        }

        public static void UpdateBookAuthor(BookAuthorModel bookAuthor)
        {
            var oldBookAuthor = BookAuthorList.Where(x => x.BookAuthorGuid == bookAuthor.BookAuthorGuid).ToList().FirstOrDefault();

            if (oldBookAuthor != null)
            {
                var index = BookAuthorList.IndexOf(oldBookAuthor);
                BookAuthorList.Remove(oldBookAuthor);
                BookAuthorList.Insert(index, bookAuthor);
            }
            else
            {
                InsertBookAuthor(bookAuthor);
            }
        }

        public static void InsertBookAuthor(BookAuthorModel bookAuthor)
        {
            if (bookAuthor.BookAuthorGuid == null)
            {
                bookAuthor.BookAuthorGuid = Guid.NewGuid();
            }

            BookAuthorList.Add(bookAuthor);
        }

        public static void DeleteBookAuthor(BookAuthorModel bookAuthor)
        {
            BookAuthorList.Remove(bookAuthor);
        }

        public static void AddAuthorsToList()
        {
            AuthorList =
           [
                new AuthorModel()
                {
                    FirstName = "First1",
                    LastName = "Last1",
                },
                new AuthorModel()
                {
                    FirstName = "First2",
                    LastName = "Last2",
                }

            ];
        }

        public static void UpdateAuthor(AuthorModel author)
        {
            var oldAuthor = AuthorList.Where(x => x.AuthorGuid == author.AuthorGuid).ToList().FirstOrDefault();

            if (oldAuthor != null)
            {
                var index = AuthorList.IndexOf(oldAuthor);
                AuthorList.Remove(oldAuthor);
                AuthorList.Insert(index, author);
            }
            else
            {
                InsertAuthor(author);
            }
        }

        public static void InsertAuthor(AuthorModel author)
        {
            if (author.AuthorGuid == null)
            {
                author.AuthorGuid = Guid.NewGuid();
            }

            AuthorList.Add(author);
        }

        public static void InsertAuthor(AuthorModel author, Guid? bookGuid)
        {
            UpdateAuthor(author);
            AddAuthorToBook(author.AuthorGuid, bookGuid);
        }

        public static void AddAuthorToBook(Guid? authorGuid, Guid? bookGuid)
        {
            if (authorGuid != null && bookGuid != null)
            {
                var existingBookAuthor = BookAuthorList.Where(x => x.AuthorGuid == authorGuid && x.BookGuid == bookGuid).ToList().FirstOrDefault();

                if (existingBookAuthor == null)
                {
                    var bookAuthor = new BookAuthorModel()
                    {
                        AuthorGuid = (Guid)authorGuid,
                        BookGuid = (Guid)bookGuid,
                    };

                    UpdateBookAuthor(bookAuthor);
                }
                else
                {
                    UpdateBookAuthor(existingBookAuthor);
                }
            }
        }

        public static void DeleteAuthor(AuthorModel author)
        {
            AuthorList.Remove(author);
            var bookAuthorList = BookAuthorList.Where(x => x.AuthorGuid == author.AuthorGuid).ToList();

            foreach (var bookAuthor in bookAuthorList)
            {
                BookAuthorList.Remove(bookAuthor);

                var book = BookList.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid);

                if (book != null && !string.IsNullOrEmpty(book.AuthorListstring))
                {
                    book.AuthorListstring = book.AuthorListstring.Replace(author.ReverseFullName, string.Empty);
                }
            }
        }

        public static void AddSeriesToList()
        {
            SeriesList =
           [
                new SeriesModel()
                {
                    SeriesName = "Series 1",
                    TotalBooksInSeries = "5",
                },
                new SeriesModel()
                {
                    SeriesName = "Series 2",
                }

            ];
        }

        public static void UpdateSeries(SeriesModel series)
        {
            var oldSeries = SeriesList.Where(x => x.SeriesGuid == series.SeriesGuid).ToList().FirstOrDefault();

            if (oldSeries != null)
            {
                var index = SeriesList.IndexOf(oldSeries);
                SeriesList.Remove(oldSeries);
                SeriesList.Insert(index, series);
            }
            else
            {
                InsertSeries(series);
            }
        }

        public static void InsertSeries(SeriesModel series)
        {
            if (series.SeriesGuid == null)
            {
                series.SeriesGuid = Guid.NewGuid();
            }

            SeriesList.Add(series);
        }

        public static void DeleteSeries(SeriesModel series)
        {
            SeriesList.Remove(series);
            var bookList = BookList.Where(x => x.BookSeriesGuid == series.SeriesGuid);

            foreach (var book in bookList)
            {
                book.BookSeriesGuid = null;
            }
        }

        public static void AddGenresToList()
        {
            GenreList =
           [
                new GenreModel()
                {
                    GenreName = "Genre 1",
                },
                new GenreModel()
                {
                    GenreName = "Genre 2",
                }

            ];
        }

        public static void UpdateGenre(GenreModel genre)
        {
            var oldGenre = GenreList.Where(x => x.GenreGuid == genre.GenreGuid).ToList().FirstOrDefault();

            if (oldGenre != null)
            {
                var index = GenreList.IndexOf(oldGenre);
                GenreList.Remove(oldGenre);
                GenreList.Insert(index, genre);
            }
            else
            {
                InsertGenre(genre);
            }
        }

        public static void InsertGenre(GenreModel genre)
        {
            if (genre.GenreGuid == null)
            {
                genre.GenreGuid = Guid.NewGuid();
            }

            GenreList.Add(genre);
        }

        public static void DeleteGenre(GenreModel genre)
        {
            GenreList.Remove(genre);
            var bookList = BookList.Where(x => x.BookGenreGuid == genre.GenreGuid);

            foreach (var book in bookList)
            {
                book.BookGenreGuid = null;
            }
        }

        public static void AddCollectionsToList()
        {
            CollectionList =
           [
                new CollectionModel()
                {
                    CollectionName = "Collection 1",
                },
                new CollectionModel()
                {
                    CollectionName = "Collection 2",
                }

            ];
        }

        public static void UpdateCollection(CollectionModel collection)
        {
            var oldCollection = CollectionList.Where(x => x.CollectionGuid == collection.CollectionGuid).ToList().FirstOrDefault();

            if (oldCollection != null)
            {
                var index = CollectionList.IndexOf(oldCollection);
                CollectionList.Remove(oldCollection);
                CollectionList.Insert(index, collection);
            }
            else
            {
                InsertCollection(collection);
            }
        }

        public static void InsertCollection(CollectionModel collection)
        {
            if (collection.CollectionGuid == null)
            {
                collection.CollectionGuid = Guid.NewGuid();
            }

            CollectionList.Add(collection);
        }

        public static void DeleteCollection(CollectionModel collection)
        {
            CollectionList.Remove(collection);

            var bookList = BookList.Where(x => x.BookCollectionGuid == collection.CollectionGuid);

            foreach (var book in bookList)
            {
                book.BookCollectionGuid = null;
            }
        }

        public static void AddLocationsToList()
        {
            LocationList =
           [
                new LocationModel()
                {
                    LocationName = "Room 1",
                },
                new LocationModel()
                {
                    LocationName = "Room 2",
                }

            ];
        }

        public static void UpdateLocation(LocationModel location)
        {
            var oldLocation = LocationList.Where(x => x.LocationGuid == location.LocationGuid).ToList().FirstOrDefault();

            if (oldLocation != null)
            {
                var index = LocationList.IndexOf(oldLocation);
                LocationList.Remove(oldLocation);
                LocationList.Insert(index, location);
            }
            else
            {
                InsertLocation(location);
            }
        }

        public static void InsertLocation(LocationModel location)
        {
            if (location.LocationGuid == null)
            {
                location.LocationGuid = Guid.NewGuid();
            }

            LocationList.Add(location);
        }

        public static void DeleteLocation(LocationModel location)
        {
            LocationList.Remove(location);
            var bookList = BookList.Where(x => x.BookLocationGuid == location.LocationGuid);

            foreach (var book in bookList)
            {
                book.BookLocationGuid = null;
            }
        }

        public static void DataCleanup()
        {
            foreach (var collection in CollectionList)
            {
                using var variable = collection.SetTotalBooks(true);
            }

            foreach (var author in AuthorList)
            {
                using var variable = author.SetTotalBooks(true);
            }

            foreach (var series in SeriesList)
            {
                using var variable = series.SetTotalBooks(true);
            }

            foreach (var location in LocationList)
            {
                using var variable = location.SetTotalBooks(true);
            }

            foreach (var genre in GenreList)
            {
                using var variable = genre.SetTotalBooks(true);
            }

            foreach (var book in BookList)
            {
                using var variable = book.SetAuthorListstring();
            }
        }
    }
}
