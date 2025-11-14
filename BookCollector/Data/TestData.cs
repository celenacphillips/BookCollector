using BookCollector.Data.Models;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BookCollector.Data
{
    public partial class TestData : ObservableObject
    {
        public static ObservableCollection<BookModel> AddBooksToList()
        {
            // TO DO:
            // Add Genre, Collection, and Series - 11/13/2025

            return new ObservableCollection<BookModel>()
            {
                new BookModel()
                {
                    ParsedTitle = "Reading Book",
                    BookTitle = "Reading Book",
                    AuthorListString = "Last, First",
                    PublisherPublishDateString = "Publisher, yyyy",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    PageReadPercent = "5%",
                    HasNoBookCover = true,
                    BookStartDate = "11/13/2025",
                    BookPageRead = 5,
                    Progress = 0.05,
                    BookPublisher = "Publisher",
                    BookPublishYear = "yyyy",
                    BookIdentifier = "1234",
                    BookLocation = "Shelf",
                    BookLanguage = "english",
                    BookPrice = "$10.00",
                    BookURL = "test.com",
                    BookSummary = "Text",
                    BookComments = "Comments",
                    Rating = 2,
                    IsFavorite = true,
                },
                new BookModel()
                {
                    ParsedTitle = "Read Book",
                    BookTitle = "Read Book",
                    AuthorListString = "Last, First",
                    PublisherPublishDateString = "Publisher, yyyy",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    PageReadPercent = "100%",
                    HasNoBookCover = true,
                    BookStartDate = "11/13/2025",
                    BookEndDate = "11/14/2025",
                    BookPageRead = 100,
                    Progress = 1,
                    BookPublisher = "Publisher",
                    BookPublishYear = "yyyy",
                    BookIdentifier = "1234",
                    BookLocation = "Shelf",
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
                    ParsedTitle = "To Be Read Book",
                    BookTitle = "To Be Read Book",
                    AuthorListString = "Last, First",
                    PublisherPublishDateString = "Publisher, yyyy",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    PageReadPercent = "0%",
                    HasNoBookCover = true,
                    BookPageRead = 0,
                    Progress = 0,
                    BookPublisher = "Publisher",
                    BookPublishYear = "yyyy",
                    BookIdentifier = "1234",
                    BookLocation = "Shelf",
                    BookLanguage = "english",
                    BookPrice = "$10.00",
                    BookURL = "test.com",
                    BookSummary = "Text",
                    BookComments = "Comments",
                    Rating = 5,
                    IsFavorite = true,
                }
            };
        }

        public static ObservableCollection<BookModel> GetReadingBooksList()
        {
            var list = AddBooksToList();
            return list.Where(x => (x.BookPageRead != x.BookPageTotal &&
                                   x.BookPageRead != 0) ||
                                   (x.UpNext == true)).ToObservableCollection();
        }

        public static ObservableCollection<BookModel> GetToBeReadBooksList()
        {
            var list = AddBooksToList();
            return list.Where(x => x.BookPageRead == 0).ToObservableCollection();
        }

        public static ObservableCollection<BookModel> GetReadBooksList()
        {
            var list = AddBooksToList();
            return list.Where(x => x.BookPageRead == x.BookPageTotal &&
                                   x.BookPageRead != 0).ToObservableCollection();
        }

        public static ObservableCollection<BookModel> GetAllBooksList()
        {
            var list = AddBooksToList();
            return list.ToObservableCollection();
        }

        public static ObservableCollection<ChapterModel> AddChaptersToList()
        {
            return new ObservableCollection<ChapterModel>()
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

        public static ObservableCollection<AuthorModel> AddAuthorsToList()
        {
            return new ObservableCollection<AuthorModel>()
            {
                new AuthorModel()
                {
                    FirstName = "First1",
                    LastName = "Last1",
                    FullName = "First1 Last1"
                },
                new AuthorModel()
                {
                    FirstName = "First2",
                    LastName = "Last2",
                    FullName = "First2 Last2"
                }
            };
        }
    }
}
