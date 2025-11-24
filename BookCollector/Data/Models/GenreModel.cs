using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

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

        public string? ParsedGenreName { get; set; }
        public int GenreTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }
        public int? ID { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
