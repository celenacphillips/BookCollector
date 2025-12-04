using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Collections.ObjectModel;


namespace BookCollector.Data.Models
{
    public partial class SeriesModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? SeriesGuid { get; set; }

        [ObservableProperty]
        public string seriesName;
        [ObservableProperty]
        public string totalBooksString;
        [ObservableProperty]
        public string? totalBooksInSeries;
        [ObservableProperty]
        public bool hideSeries;

        public int? ID { get; set; }
        public string? ParsedSeriesName { get; set; }
        public int SeriesTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }

        public SeriesModel()
        {
            SeriesGuid = Guid.NewGuid();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FilterLists.GetAllBooksInSeriesList(this.SeriesGuid, showHiddenBooks);

            this.TotalBooksString = StringManipulation.SetTotalBooksString(list.Count);
            this.SeriesTotalBooks = list.Count;
        }
    }
}
