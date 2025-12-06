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

        public string? ParsedGenreName
        {
            get => (this.GenreName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.GenreName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.GenreName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase))
                        ? this.GenreName.Remove(0, this.GenreName.IndexOf(" ") + 1)
                        : this.GenreName;
        }
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
            var list = await FilterLists.GetAllBooksInGenreList(this.GenreGuid, showHiddenBooks);

            this.TotalBooksString = StringManipulation.SetTotalBooksString(list.Count);
            this.GenreTotalBooks = list.Count;
        }
    }
}
