using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.Bibliography;
using SQLite;
using System.Collections.ObjectModel;


namespace BookCollector.Data.Models
{
    public partial class SeriesModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? SeriesGuid { get; set; }

        [ObservableProperty]
        public string? seriesName;
        [ObservableProperty]
        public string? totalBooksString;
        [ObservableProperty]
        public string? totalBooksInSeries;
        [ObservableProperty]
        public bool hideSeries;

        public int? ID { get; set; }
        public string? ParsedSeriesName
        {
            get => (!string.IsNullOrEmpty(this.SeriesName) &&
                    (this.SeriesName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.SeriesName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.SeriesName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.SeriesName[(this.SeriesName.IndexOf(' ') + 1)..]
                        : this.SeriesName;
        }
        public int SeriesTotalBooks { get; set; }
        // TO DO
        // Set value - 12/8/2025
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
            var count = 0;

            if (list != null)
            {
                count = list.Count;
            }

            this.TotalBooksString = !string.IsNullOrEmpty(this.TotalBooksInSeries) ?
                                    StringManipulation.SetTotalBooksString(count, int.Parse(this.TotalBooksInSeries)) :
                                    StringManipulation.SetTotalBooksString(count);
            this.SeriesTotalBooks = count;
        }
    }
}
