using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Collections.ObjectModel;

namespace BookCollector.Data.Models
{
    public partial class GenreModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? GenreGuid { get; set; }

        [ObservableProperty]
        public string genreName;
        [ObservableProperty]
        public string totalBooksString;
        [ObservableProperty]
        public bool hideGenre;

        public string? ParsedGenreName { get; set; }
        public int GenreTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }
        public int? ID { get; set; }

        public GenreModel()
        {
            GenreGuid = Guid.NewGuid();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetTotalBooks(bool showHiddenBooks)
        {
            // Unit test data
            var bookList = TestData.BookList;

            var list = await FilterLists.GetAllBooksInGenreList(bookList, this.GenreGuid, showHiddenBooks);

            this.TotalBooksString = StringManipulation.SetTotalBooksString(list.Count);
        }
    }
}
