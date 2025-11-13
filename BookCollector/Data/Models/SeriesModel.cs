using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;


namespace BookCollector.Data.Models
{
    public partial class SeriesModel : ObservableObject
    {
        [PrimaryKey]
        public Guid? SeriesGuid { get; set; }

        [ObservableProperty]
        public string seriesName;
        [ObservableProperty]
        public string totalBooksString;
        [ObservableProperty]
        public string? totalBooksInSeries;

        public int? ID { get; set; }
        public string? ParsedSeriesName { get; set; }
        public int SeriesTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }
    }
}
