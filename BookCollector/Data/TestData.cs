using BookCollector.Data.Models;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BookCollector.Data
{
    public partial class TestData : ObservableObject
    {
        public static ObservableCollection<BookModel> BookList { get; set; }
        public static ObservableCollection<ChapterModel> ChapterList { get; set; }
        public static ObservableCollection<AuthorModel> AuthorList { get; set; }
        public static ObservableCollection<SeriesModel> SeriesList { get; set; }
        public static ObservableCollection<GenreModel> GenreList { get; set; }
        public static ObservableCollection<CollectionModel> CollectionList { get; set; }
        public static ObservableCollection<LocationModel> LocationList { get; set; }
        public static ObservableCollection<BookAuthorModel> BookAuthorList { get; set; }

        public TestData()
        {
            BookList = new ObservableCollection<BookModel>();
            ChapterList = new ObservableCollection<ChapterModel>();
            AuthorList = new ObservableCollection<AuthorModel>();
            SeriesList = new ObservableCollection<SeriesModel>();
            GenreList = new ObservableCollection<GenreModel>();
            CollectionList = new ObservableCollection<CollectionModel>();
            LocationList = new ObservableCollection<LocationModel>();
            BookAuthorList = new ObservableCollection<BookAuthorModel>();
        }

        public static void AddBooksToList()
        {
            AddChaptersToList();
            AddAuthorsToList();
            AddSeriesToList();
            AddGenresToList();
            AddCollectionsToList();
            AddLocationsToList();

            BookList = new ObservableCollection<BookModel>()
            {
                new BookModel()
                {
                    BookTitle = "Reading Book",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    BookStartDate = "11/13/2025",
                    BookPageRead = 5,
                    BookPublisher = "Publisher",
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
                    BookSeriesGuid = SeriesList[0].SeriesGuid
                },
                new BookModel()
                {
                    BookTitle = "A Read Book",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    BookStartDate = "11/13/2025",
                    BookEndDate = "11/14/2025",
                    BookPageRead = 100,
                    BookPublisher = "Publisher",
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
                    BookPublisher = "Publisher",
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
            };

            foreach (var chapter in ChapterList)
            {
                chapter.BookGuid = (Guid)BookList[0].BookGuid;
            }

            BookAuthorList =
            [
                new BookAuthorModel()
                {
                    BookGuid = (Guid)BookList[0].BookGuid,
                    AuthorGuid = (Guid)AuthorList[0].AuthorGuid
                },
            ];

            foreach (var book in BookList)
            {
                book.SetReadingProgress();
                book.SetCoverDisplay();
                book.SetAuthorListString(BookAuthorList, AuthorList);
            }
        }

        public static void UpdateBook(BookModel book)
        {
            var oldBook = BookList.Where(x => x.BookGuid == book.BookGuid).ToList().FirstOrDefault();
            var index = BookList.IndexOf(oldBook);
            BookList.Remove(oldBook);
            BookList.Insert(index, book);
        }

        public static void InsertBook(BookModel book)
        {
            BookList.Add(book);
        }

        public static void DeleteBook(BookModel book)
        {
            BookList.Remove(book);
        }

        public static void AddChaptersToList()
        {
            ChapterList =  new ObservableCollection<ChapterModel>()
            {
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
            };
        }

        public static void UpdateChapter(ChapterModel chapter)
        {
            var oldChapter = ChapterList.Where(x => x.ChapterGuid == chapter.ChapterGuid).ToList().FirstOrDefault();
            var index = ChapterList.IndexOf(oldChapter);
            ChapterList.Remove(oldChapter);
            ChapterList.Insert(index, chapter);
        }

        public static void InsertChapter(ChapterModel chapter)
        {
            ChapterList.Add(chapter);
        }

        public static void DeleteChapter(ChapterModel chapter)
        {
            ChapterList.Remove(chapter);
        }

        public static void AddAuthorsToList()
        {
            AuthorList =  new ObservableCollection<AuthorModel>()
            {
                new AuthorModel()
                {
                    FirstName = "First1",
                    LastName = "Last1"
                },
                new AuthorModel()
                {
                    FirstName = "First2",
                    LastName = "Last2",
                }
            };
        }

        // TO DO
        // Bug where removing and re-inserting isn't working - 12/1/2025
        public static void UpdateAuthor(AuthorModel author)
        {
            var oldAuthor = AuthorList.Where(x => x.AuthorGuid == author.AuthorGuid).ToList().FirstOrDefault();
            var index = AuthorList.IndexOf(oldAuthor);
            AuthorList.Remove(oldAuthor);
            AuthorList.Insert(index, author);
        }

        public static void InsertAuthor(AuthorModel author)
        {
            AuthorList.Add(author);
        }

        public static void InsertAuthor(AuthorModel author, Guid? bookGuid)
        {
            AuthorList.Add(author);
            AddAuthorToBook(author.AuthorGuid, bookGuid);
        }

        public static void AddAuthorToBook(Guid? authorGuid, Guid? bookGuid)
        {
            BookAuthorList.Add(new BookAuthorModel()
            {
                AuthorGuid = (Guid)authorGuid,
                BookGuid = (Guid)bookGuid,
            });
        }

        public static void DeleteAuthor(AuthorModel author)
        {
            AuthorList.Remove(author);
            var bookAuthorList = BookAuthorList.Where(x => x.AuthorGuid == author.AuthorGuid).ToList();

            foreach (var bookAuthor in bookAuthorList)
            {
                BookAuthorList.Remove(bookAuthor);

                var book = BookList.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid);

                book.AuthorListString = book.AuthorListString.Replace(author.ReverseFullName, string.Empty);
            }
        }

        public static void AddSeriesToList()
        {
            SeriesList = new ObservableCollection<SeriesModel>()
            {
                new SeriesModel()
                {
                    SeriesName = "Series 1",
                    TotalBooksInSeries = "5"

                },
                new SeriesModel()
                {
                    SeriesName = "Series 2",
                }
            };
        }

        public static void UpdateSeries(SeriesModel series)
        {
            var oldSeries = SeriesList.Where(x => x.SeriesGuid == series.SeriesGuid).ToList().FirstOrDefault();
            var index = SeriesList.IndexOf(oldSeries);
            SeriesList.Remove(oldSeries);
            SeriesList.Insert(index, series);
        }

        public static void InsertSeries(SeriesModel series)
        {
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
            GenreList = new ObservableCollection<GenreModel>()
            {
                new GenreModel()
                {
                    GenreName = "Genre 1"
                },
                new GenreModel()
                {
                    GenreName = "Genre 2"
                }
            };
        }

        public static void UpdateGenre(GenreModel genre)
        {
            var oldGenre = GenreList.Where(x => x.GenreGuid == genre.GenreGuid).ToList().FirstOrDefault();
            var index = GenreList.IndexOf(oldGenre);
            GenreList.Remove(oldGenre);
            GenreList.Insert(index, genre);
        }

        public static void InsertGenre(GenreModel genre)
        {
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
            CollectionList = new ObservableCollection<CollectionModel>()
            {
                new CollectionModel()
                {
                    CollectionName = "Collection 1"
                },
                new CollectionModel()
                {
                    CollectionName = "Collection 2"
                }
            };
        }

        public static void UpdateCollection(CollectionModel collection)
        {
            var oldCollection = CollectionList.Where(x => x.CollectionGuid == collection.CollectionGuid).ToList().FirstOrDefault();
            var index = CollectionList.IndexOf(oldCollection);
            CollectionList.Remove(oldCollection);
            CollectionList.Insert(index, collection);
        }

        public static void InsertCollection(CollectionModel collection)
        {
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
            LocationList = new ObservableCollection<LocationModel>()
            {
                new LocationModel()
                {
                    LocationName = "Room 1"
                },
                new LocationModel()
                {
                    LocationName = "Room 2"
                }
            };
        }

        public static void UpdateLocation(LocationModel location)
        {
            var oldLocation = LocationList.Where(x => x.LocationGuid == location.LocationGuid).ToList().FirstOrDefault();
            var index = LocationList.IndexOf(oldLocation);
            LocationList.Remove(oldLocation);
            LocationList.Insert(index, location);
        }

        public static void InsertLocation(LocationModel location)
        {
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
    }
}
