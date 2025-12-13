using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class GenreModel : ObservableObject, ICloneable
    {
        [ObservableProperty]
        public string? genreName;

        [ObservableProperty]
        public string? totalBooksstring;

        [ObservableProperty]
        public bool hideGenre;

        public GenreModel()
        {
            this.GenreGuid = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid? GenreGuid { get; set; }

        public string? ParsedGenreName
        {
            get => (!string.IsNullOrEmpty(this.GenreName) &&
                    (this.GenreName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.GenreName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.GenreName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.GenreName[(this.GenreName.IndexOf(' ') + 1) ..]
                        : this.GenreName;
        }

        public int GenreTotalBooks { get; set; }

        public double TotalCostOfBooks { get; set; }

        public int? ID { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async void SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FilterLists.GetAllBooksInGenreList(this.GenreGuid, showHiddenBooks);
            var count = 0;

            if (list != null)
            {
                count = list.Count;
            }

            this.TotalBooksstring = StringManipulation.SetTotalBooksString(count);
            this.GenreTotalBooks = count;
        }

        public async void SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await FilterLists.GetAllBookPricesInGenreList(this.GenreGuid, showHiddenBooks);
        }
    }
}
