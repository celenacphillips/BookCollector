using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Collections.ObjectModel;

namespace BookCollector.Data.Models
{
    public partial class AuthorModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? AuthorGuid { get; set; }

        [ObservableProperty]
        public string firstName;
        [ObservableProperty]
        public string lastName;
        [ObservableProperty]
        public string? totalBooksString;
        [ObservableProperty]
        public bool hideAuthor;

        public AuthorModel()
        {
            AuthorGuid = Guid.NewGuid();
        }

        public string FullName
        {
            get => $"{this.FirstName} {this.LastName}";
        }
        public string ReverseFullName
        {
            get => $"{this.LastName}, {this.FirstName}";
        }

        public int AuthorTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetTotalBooks(bool showHiddenBooks)
        {
            // Unit test data
            var bookList = TestData.BookList;
            var bookAuthorList = TestData.BookAuthorList;

            bookAuthorList = await FilterLists.GetAllBookAuthorsForAuthor(bookAuthorList, this.AuthorGuid);

            var list = await FilterLists.GetAllBooksInAuthorList(bookAuthorList, bookList, showHiddenBooks);

            this.TotalBooksString = StringManipulation.SetTotalBooksString(list.Count);
            this.AuthorTotalBooks = list.Count;
        }
    }
}
