using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class AuthorModel : ObservableObject, ICloneable
    {
        [ObservableProperty]
        public string? firstName;

        [ObservableProperty]
        public string? lastName;

        [ObservableProperty]
        public string? totalBooksstring;

        [ObservableProperty]
        public bool hideAuthor;

        public AuthorModel()
        {
            this.AuthorGuid = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid? AuthorGuid { get; set; }

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

        public async void SetTotalBooks(bool showHiddenBooks)
        {
            var bookAuthorList = await FilterLists.GetAllBookAuthorsForAuthor(this.AuthorGuid);

            var list = await FilterLists.GetAllBooksInAuthorList(bookAuthorList, showHiddenBooks);

            this.TotalBooksstring = StringManipulation.SetTotalBooksString(list.Count);
            this.AuthorTotalBooks = list.Count;
        }

        public async void SetTotalCostOfBooks(bool showHiddenBooks)
        {
            var bookAuthorList = await FilterLists.GetAllBookAuthorsForAuthor(this.AuthorGuid);

            this.TotalCostOfBooks = await FilterLists.GetAllBookPricesInAuthorList(bookAuthorList, showHiddenBooks);
        }
    }
}
