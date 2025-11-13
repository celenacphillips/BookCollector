using BookCollector.Data.Models;
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
                    ParsedTitle = "New Book",
                    BookTitle = "New Book",
                    AuthorListString = "Last, First",
                    PublisherPublishDateString = "Publisher, yyyy",
                    BookPageTotal = 100,
                    BookFormat = "Hardcover",
                    PageReadPercent = "5%",
                    HasNoBookCover = true,
                    BookStartDate = DateTime.Now.ToString(),
                    BookEndDate = DateTime.Now.AddDays(1).ToString(),
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
                }
            };
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
