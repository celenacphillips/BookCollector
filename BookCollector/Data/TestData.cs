// <copyright file="TestData.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data.Models;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;

namespace BookCollector.Data
{
    public partial class TestData : BaseViewModel
    {
        internal static ObservableCollection<BookModel> BookList = [];
        internal static ObservableCollection<ChapterModel> ChapterList = [];
        internal static ObservableCollection<AuthorModel> AuthorList = [];
        internal static ObservableCollection<SeriesModel> SeriesList = [];
        internal static ObservableCollection<GenreModel> GenreList = [];
        internal static ObservableCollection<CollectionModel> CollectionList = [];
        internal static ObservableCollection<LocationModel> LocationList = [];
        internal static ObservableCollection<BookAuthorModel> BookAuthorList = [];
        internal static ObservableCollection<WishlistBookModel> BookWishList = [];

        public TestData()
        {
        }

        public static bool UseTestData { get; set; }

        public static async void AddBooksToList()
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
                    using var list = book.SetAuthorListString();
                }

                var showHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);

                foreach (var author in AuthorList)
                {
                    author.SetTotalBooks(true);
                    author.SetTotalCostOfBooks(true);
                }

                foreach (var series in SeriesList)
                {
                    series.SetTotalBooks(showHiddenBooks);
                    series.SetTotalCostOfBooks(showHiddenBooks);
                }

                foreach (var genre in GenreList)
                {
                    genre.SetTotalBooks(showHiddenBooks);
                    genre.SetTotalCostOfBooks(showHiddenBooks);
                }

                foreach (var collection in CollectionList)
                {
                    collection.SetTotalBooks(showHiddenBooks);
                    collection.SetTotalCostOfBooks(showHiddenBooks);
                }

                foreach (var location in LocationList)
                {
                    location.SetTotalBooks(showHiddenBooks);
                    location.SetTotalCostOfBooks(showHiddenBooks);
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
                new WishlistBookModel()
                {
                    BookTitle = "Wishlist Book 1",
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
                new WishlistBookModel()
                {
                    BookTitle = "Wishlist Book 2",
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
                new WishlistBookModel()
                {
                    BookTitle = "Wishlist Book 3",
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

        public static void UpdateWishListBook(WishlistBookModel book)
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
                book.SetCoverDisplay();
                InsertWishListBook(book);
            }
        }

        public static void InsertWishListBook(WishlistBookModel book)
        {
            if (book.BookGuid == null)
            {
                book.BookGuid = Guid.NewGuid();
            }

            BookWishList.Add(book);
        }

        public static void DeleteWishListBook(WishlistBookModel book)
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

        public static void DeleteBookAuthor(Guid authorGuid, Guid bookGuid)
        {
            var bookAuthor = BookAuthorList.Where(x => x.AuthorGuid == authorGuid && x.BookGuid == bookGuid).ToList().FirstOrDefault();

            if (bookAuthor != null)
            {
                BookAuthorList.Remove(bookAuthor);
            }
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

                if (book != null && !string.IsNullOrEmpty(book.AuthorListString))
                {
                    book.AuthorListString = book.AuthorListString.Replace(author.ReverseFullName, string.Empty);
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

        public static async void DataCleanup()
        {
            foreach (var collection in CollectionList)
            {
                collection.SetTotalBooks(true);
                collection.SetTotalCostOfBooks(true);
            }

            foreach (var author in AuthorList)
            {
                author.SetTotalBooks(true);
                author.SetTotalCostOfBooks(true);
            }

            foreach (var series in SeriesList)
            {
                series.SetTotalBooks(true);
                series.SetTotalCostOfBooks(true);
            }

            foreach (var location in LocationList)
            {
                location.SetTotalBooks(true);
                location.SetTotalCostOfBooks(true);
            }

            foreach (var genre in GenreList)
            {
                genre.SetTotalBooks(true);
                genre.SetTotalCostOfBooks(true);
            }

            foreach (var book in BookList)
            {
                using var variable = book.SetAuthorListString();
            }

            // foreach (var book in BookWishList)
            // {
            //    using var variable = book.SetAuthorListString();
            // }
        }

        public static ObservableCollection<BookModel>? GetReadingBooks(bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => (x.BookPageRead != x.BookPageTotal && x.BookPageRead != 0) || (x.UpNext == true))
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetToBeReadBooks(bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => x.BookPageRead == 0 && x.UpNext == false)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetReadBooks(bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => x.BookPageRead == x.BookPageTotal && x.BookPageRead != 0)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooks(bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<WishlistBookModel>? GetAllWishlistBooks(bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<WishlistBookModel>();
            var filteredList = new ObservableCollection<WishlistBookModel>();

            bookList = BookWishList
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<ChapterModel>? GetAllChaptersInBook(Guid? inputGuid)
        {
            var chapterList = new ObservableCollection<ChapterModel>();

            chapterList = ChapterList
                    .Where(x => x.BookGuid == inputGuid)
                    .OrderBy(x => x.ChapterOrder)
                    .ToObservableCollection();

            return chapterList;
        }

        public static ObservableCollection<ChapterModel>? GetAllChapters()
        {
            var chapterList = new ObservableCollection<ChapterModel>();

            chapterList = ChapterList
                    .OrderBy(x => x.ChapterOrder)
                    .ToObservableCollection();

            return chapterList;
        }

        public static ObservableCollection<BookAuthorModel>? GetAllBookAuthorsForBook(Guid? inputGuid)
        {
            var bookAuthorList = new ObservableCollection<BookAuthorModel>();

            bookAuthorList = BookAuthorList
                    .Where(x => x.BookGuid == inputGuid)
                    .OrderBy(x => x.BookGuid)
                    .ToObservableCollection();

            return bookAuthorList;
        }

        public static ObservableCollection<Guid>? GetAllAuthorGuidsForBook(Guid? inputGuid)
        {
            var authorGuidList = new ObservableCollection<Guid>();

            authorGuidList = BookAuthorList
                    .Where(x => x.BookGuid == inputGuid)
                    .Select(x => x.AuthorGuid)
                    .Distinct()
                    .ToObservableCollection();

            return authorGuidList;
        }

        public static ObservableCollection<BookAuthorModel>? GetAllBookAuthors()
        {
            var bookAuthorList = new ObservableCollection<BookAuthorModel>();

            bookAuthorList = BookAuthorList
                    .OrderBy(x => x.BookGuid)
                    .ToObservableCollection();

            return bookAuthorList;
        }

        public static ObservableCollection<BookAuthorModel>? GetAllBookAuthorsForAuthor(Guid? inputGuid)
        {
            var bookAuthorList = new ObservableCollection<BookAuthorModel>();

            bookAuthorList = BookAuthorList
                    .Where(x => x.AuthorGuid == inputGuid)
                    .OrderBy(x => x.BookGuid)
                    .ToObservableCollection();

            return bookAuthorList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksInCollectionList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            bookList = BookList
                    .Where(x => x.BookCollectionGuid == inputGuid)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutACollectionList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            bookList = BookList
                    .Where(x => x.BookCollectionGuid == null)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksInGenreList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            bookList = BookList
                    .Where(x => x.BookGenreGuid == inputGuid)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutAGenreList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            bookList = BookList
                    .Where(x => x.BookGenreGuid == null)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksInSeriesList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            bookList = BookList
                    .Where(x => x.BookSeriesGuid == inputGuid)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutASeriesList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            bookList = BookList
                    .Where(x => x.BookSeriesGuid == null)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksInLocationList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            bookList = BookList
                    .Where(x => x.BookLocationGuid == inputGuid)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutALocationList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = null;

            bookList = BookList
                    .Where(x => x.BookSeriesGuid == null)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<CollectionModel>? GetAllCollectionsList(bool showHiddenCollections)
        {
            ObservableCollection<CollectionModel>? collectionList = null;
            ObservableCollection<CollectionModel>? filteredList = null;

            collectionList = CollectionList
                    .OrderBy(x => x.ParsedCollectionName)
                    .ToObservableCollection();

            if (!showHiddenCollections)
            {
                filteredList = collectionList?
                    .Where(x => !x.HideCollection)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = collectionList;
            }

            return filteredList;
        }

        public static ObservableCollection<GenreModel>? GetAllGenresList(bool showHiddenGenres)
        {
            ObservableCollection<GenreModel>? genreList = null;
            ObservableCollection<GenreModel>? filteredList = null;

            genreList = GenreList
                    .OrderBy(x => x.ParsedGenreName)
                    .ToObservableCollection();

            if (!showHiddenGenres)
            {
                filteredList = genreList?
                    .Where(x => !x.HideGenre)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = genreList;
            }

            return filteredList;
        }

        public static ObservableCollection<SeriesModel>? GetAllSeriesList(bool showHiddenSeries)
        {
            ObservableCollection<SeriesModel>? seriesList = null;
            ObservableCollection<SeriesModel>? filteredList = null;

            seriesList = SeriesList
                    .OrderBy(x => x.ParsedSeriesName)
                    .ToObservableCollection();

            if (!showHiddenSeries)
            {
                filteredList = seriesList?.Where(x => !x.HideSeries).ToObservableCollection();
            }
            else
            {
                filteredList = seriesList;
            }

            return filteredList;
        }

        public static ObservableCollection<LocationModel>? GetAllLocationsList(bool showHiddenLocations)
        {
            ObservableCollection<LocationModel>? locationList = null;
            ObservableCollection<LocationModel>? filteredList = null;

            locationList = LocationList
                    .OrderBy(x => x.ParsedLocationName)
                    .ToObservableCollection();

            if (!showHiddenLocations)
            {
                filteredList = locationList?.Where(x => !x.HideLocation).ToObservableCollection();
            }
            else
            {
                filteredList = locationList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksInAuthorList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = [];

            bookList = BookList;

            var bookAuthorList = BookAuthorList?
                                        .Where(x => x.AuthorGuid == inputGuid)
                                        .ToObservableCollection();

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

            filteredList = filteredList
                .OrderBy(x => x.ParsedTitle)
                .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = filteredList?
                    .Where(x => !x.HideBook)
                    .ToObservableCollection();
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutAuthorList(string reverseAuthorName, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? bookList = null;
            ObservableCollection<BookModel>? filteredList = [];

            bookList = BookList
                    .Where(x => string.IsNullOrEmpty(x.AuthorListString) || (!string.IsNullOrEmpty(x.AuthorListString) && !x.AuthorListString.Contains(reverseAuthorName)))
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<AuthorModel>? GetAllAuthorsList(bool showHiddenAuthors)
        {
            ObservableCollection<AuthorModel>? authorList = null;
            ObservableCollection<AuthorModel>? filteredList = [];

            authorList = AuthorList
                    .OrderBy(x => x.LastName)
                    .OrderBy(x => x.FirstName)
                    .ToObservableCollection();

            if (!showHiddenAuthors)
            {
                filteredList = authorList?.Where(x => !x.HideAuthor).ToObservableCollection();
            }
            else
            {
                filteredList = authorList;
            }

            return filteredList;
        }

        public static GenreModel? GetGenreForBook(Guid? inputGuid)
        {
            return GenreList.FirstOrDefault(x => x.GenreGuid == inputGuid);
        }

        public static LocationModel? GetLocationForBook(Guid? inputGuid)
        {
            return LocationList.FirstOrDefault(x => x.LocationGuid == inputGuid);
        }

        public static SeriesModel? GetSeriesForBook(Guid? inputGuid)
        {
            return SeriesList.FirstOrDefault(x => x.SeriesGuid == inputGuid);
        }

        public static CollectionModel? GetCollectionForBook(Guid? inputGuid)
        {
            return CollectionList.FirstOrDefault(x => x.CollectionGuid == inputGuid);
        }

        public static ObservableCollection<BookModel>? GetBooksListByFavorite(bool showHiddenBooks, bool favoriteValue)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => x.IsFavorite == favoriteValue)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetBooksListByRating(bool showHiddenBooks, int starRating)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => x.Rating == starRating)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBookPricesInCollectionList(Guid? inputGuid, bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => x.BookCollectionGuid == inputGuid)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBookPricesInGenreList(Guid? inputGuid, bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => x.BookGenreGuid == inputGuid)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBookPricesInSeriesList(Guid? inputGuid, bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => x.BookSeriesGuid == inputGuid)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBookPricesInLocationList(Guid? inputGuid, bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => x.BookLocationGuid == inputGuid)
                    .OrderBy(x => x.ParsedTitle)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?.Where(x => !x.HideBook).ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksReadInYear(int year, bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year)
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList?
                    .Where(x => !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithPrices(bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<BookModel>();
            var filteredList = new ObservableCollection<BookModel>();

            bookList = BookList
                    .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList
                    .Where(x => !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<WishlistBookModel>? GetAllWishlistBooksWithPrices(bool showHiddenBooks)
        {
            var bookList = new ObservableCollection<WishlistBookModel>();
            var filteredList = new ObservableCollection<WishlistBookModel>();

            bookList = BookWishList
                    .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                    .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = bookList
                    .Where(x => !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = bookList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBookPricesInAuthorList(Guid? inputGuid, bool showHiddenBooks)
        {
            var filteredList = new ObservableCollection<BookModel>();

            var bookAuthorList = BookAuthorList?
                                        .Where(x => x.AuthorGuid == inputGuid)
                                        .ToObservableCollection();

            if (BookList != null && bookAuthorList != null)
            {
                foreach (var bookAuthor in bookAuthorList)
                {
                    var book = BookList.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid);

                    if (book != null)
                    {
                        filteredList.Add(book);
                    }
                }
            }

            filteredList = filteredList
                .OrderBy(x => x.ParsedTitle)
                .ToObservableCollection();

            if (!showHiddenBooks)
            {
                filteredList = filteredList?
                    .Where(x => !x.HideBook)
                    .ToObservableCollection();
            }

            return filteredList;
        }

        public static ObservableCollection<AuthorModel>? GetAllAuthorsWithBooks(bool showHiddenAuthors)
        {
            ObservableCollection<AuthorModel>? authorList = null;
            ObservableCollection<AuthorModel>? filteredList = null;

            authorList = AuthorList
                    .Where(x => x.AuthorTotalBooks != 0)
                    .OrderByDescending(x => x.AuthorTotalBooks)
                    .ToObservableCollection();

            if (!showHiddenAuthors)
            {
                filteredList = authorList
                    .Where(x => !x.HideAuthor)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = authorList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutAuthorsList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookList
                    .Where(x => string.IsNullOrEmpty(x.AuthorListString) && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookList
                    .Where(x => string.IsNullOrEmpty(x.AuthorListString))
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static ObservableCollection<CollectionModel>? GetAllCollectionsWithBooks(bool showHiddenCollections)
        {
            ObservableCollection<CollectionModel>? collectionList = null;
            ObservableCollection<CollectionModel>? filteredList = null;

            collectionList = CollectionList
                    .Where(x => x.CollectionTotalBooks != 0)
                    .OrderByDescending(x => x.CollectionTotalBooks)
                    .ToObservableCollection();

            if (!showHiddenCollections)
            {
                filteredList = collectionList
                    .Where(x => !x.HideCollection)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = collectionList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutCollections(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookList
                    .Where(x => x.BookCollectionGuid == null && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookList
                    .Where(x => x.BookCollectionGuid == null)
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static ObservableCollection<GenreModel>? GetAllGenresWithBooks(bool showHiddenGenres)
        {
            ObservableCollection<GenreModel>? genreList = null;
            ObservableCollection<GenreModel>? filteredList = null;

            genreList = GenreList
                    .Where(x => x.GenreTotalBooks != 0)
                    .OrderByDescending(x => x.GenreTotalBooks)
                    .ToObservableCollection();

            if (!showHiddenGenres)
            {
                filteredList = genreList
                    .Where(x => !x.HideGenre)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = genreList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutGenres(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookList
                    .Where(x => x.BookGenreGuid == null && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookList
                    .Where(x => x.BookGenreGuid == null)
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static ObservableCollection<SeriesModel>? GetAllSeriesWithBooks(bool showHiddenSeries)
        {
            ObservableCollection<SeriesModel>? seriesList = null;
            ObservableCollection<SeriesModel>? filteredList = null;

            seriesList = SeriesList
                    .Where(x => x.SeriesTotalBooks != 0)
                    .OrderByDescending(x => x.SeriesTotalBooks)
                    .ToObservableCollection();

            if (!showHiddenSeries)
            {
                filteredList = seriesList
                    .Where(x => !x.HideSeries)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = seriesList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutSeries(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookList
                    .Where(x => x.BookSeriesGuid == null && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookList
                    .Where(x => x.BookSeriesGuid == null)
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static ObservableCollection<LocationModel>? GetAllLocationsWithBooks(bool showHiddenLocations)
        {
            ObservableCollection<LocationModel>? locationList = null;
            ObservableCollection<LocationModel>? filteredList = null;

            locationList = LocationList
                    .Where(x => x.LocationTotalBooks != 0)
                    .OrderByDescending(x => x.LocationTotalBooks)
                    .ToObservableCollection();

            if (!showHiddenLocations)
            {
                filteredList = locationList
                    .Where(x => !x.HideLocation)
                    .ToObservableCollection();
            }
            else
            {
                filteredList = locationList;
            }

            return filteredList;
        }

        public static ObservableCollection<BookModel>? GetAllBooksWithoutLocations(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookList
                    .Where(x => x.BookLocationGuid == null && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookList
                    .Where(x => x.BookLocationGuid == null)
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static ObservableCollection<WishlistBookModel>? GetAllWishlistBooksWithLocations(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookWishList
                    .Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy) && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookWishList
                    .Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy))
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static ObservableCollection<WishlistBookModel>? GetAllWishlistBooksWithoutLocations(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookWishList
                    .Where(x => string.IsNullOrEmpty(x.BookWhereToBuy) && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookWishList
                    .Where(x => string.IsNullOrEmpty(x.BookWhereToBuy))
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static List<string?>? GetAllWishlistBookLocations(bool showHiddenBooks)
        {
            List<string?>? filteredList = null;

            var locationList = GetAllWishlistBooksWithLocations(showHiddenBooks);
            filteredList = locationList?.Select(x => x.BookWhereToBuy)
                .Distinct()
                .ToList();

            return filteredList;
        }

        public static ObservableCollection<WishlistBookModel>? GetAllWishlistBooksWithSeries(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookWishList
                    .Where(x => !string.IsNullOrEmpty(x.BookSeries) && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookWishList
                    .Where(x => !string.IsNullOrEmpty(x.BookSeries))
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static ObservableCollection<WishlistBookModel>? GetAllWishlistBooksWithoutSeries(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookWishList
                    .Where(x => string.IsNullOrEmpty(x.BookSeries) && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookWishList
                    .Where(x => string.IsNullOrEmpty(x.BookSeries))
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static List<string?>? GetAllWishlistBookSeries(bool showHiddenBooks)
        {
            List<string?>? filteredList = null;

            var seriesList = GetAllWishlistBooksWithSeries(showHiddenBooks);
            filteredList = seriesList?.Select(x => x.BookSeries)
                .Distinct()
                .ToList();

            return filteredList;
        }

        public static ObservableCollection<WishlistBookModel>? GetAllWishlistBooksWithAuthors(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookWishList
                    .Where(x => !string.IsNullOrEmpty(x.AuthorListString) && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookWishList
                    .Where(x => !string.IsNullOrEmpty(x.AuthorListString))
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static ObservableCollection<WishlistBookModel>? GetAllWishlistBooksWithoutAuthors(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? filteredBookList = null;

            if (!showHiddenBooks)
            {
                filteredBookList = BookWishList
                    .Where(x => string.IsNullOrEmpty(x.AuthorListString) && !x.HideBook)
                    .ToObservableCollection();
            }
            else
            {
                filteredBookList = BookWishList
                    .Where(x => string.IsNullOrEmpty(x.AuthorListString))
                    .ToObservableCollection();
            }

            return filteredBookList;
        }

        public static List<string?>? GetAllWishlistBookAuthors(bool showHiddenBooks)
        {
            List<string?>? filteredList = null;

            var authorList = GetAllWishlistBooksWithAuthors(showHiddenBooks);
            filteredList = authorList?.Select(x => x.AuthorListString)
                .Distinct()
                .ToList();

            return filteredList;
        }

        public static void DeleteAllData()
        {
            BookList.Clear();
            AuthorList.Clear();
            BookAuthorList.Clear();
            GenreList.Clear();
            SeriesList.Clear();
            LocationList.Clear();
            CollectionList.Clear();
            BookWishList.Clear();
        }
    }
}
