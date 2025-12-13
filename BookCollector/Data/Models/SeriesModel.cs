using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class SeriesModel : ObservableObject, ICloneable
    {
        [ObservableProperty]
        public string? seriesName;
        [ObservableProperty]
        public string? totalBooksstring;
        [ObservableProperty]
        public string? totalBooksInSeries;
        [ObservableProperty]
        public bool hideSeries;

        public SeriesModel()
        {
            this.SeriesGuid = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid? SeriesGuid { get; set; }

        public int? ID { get; set; }

        public string? ParsedSeriesName
        {
            get => (!string.IsNullOrEmpty(this.SeriesName) &&
                    (this.SeriesName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.SeriesName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.SeriesName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.SeriesName[(this.SeriesName.IndexOf(' ') + 1) ..]
                        : this.SeriesName;
        }

        public int SeriesTotalBooks { get; set; }

        public double TotalCostOfBooks { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async void SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FilterLists.GetAllBooksInSeriesList(this.SeriesGuid, showHiddenBooks);
            var count = 0;

            if (list != null)
            {
                count = list.Count;
            }

            this.TotalBooksstring = !string.IsNullOrEmpty(this.TotalBooksInSeries) ?
                                    StringManipulation.SetTotalBooksString(count, int.Parse(this.TotalBooksInSeries)) :
                                    StringManipulation.SetTotalBooksString(count);
            this.SeriesTotalBooks = count;
        }

        public async void SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await FilterLists.GetAllBookPricesInSeriesList(this.SeriesGuid, showHiddenBooks);
        }
    }
}
