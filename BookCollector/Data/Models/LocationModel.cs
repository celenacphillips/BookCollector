using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class LocationModel : ObservableObject, ICloneable
    {
        [ObservableProperty]
        public string? locationName;
        [ObservableProperty]
        public string? totalBooksstring;
        [ObservableProperty]
        public bool hideLocation;

        public LocationModel()
        {
            this.LocationGuid = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid? LocationGuid { get; set; }

        public string? ParsedLocationName
        {
            get => (!string.IsNullOrEmpty(this.LocationName) &&
                    (this.LocationName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.LocationName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.LocationName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.LocationName[(this.LocationName.IndexOf(' ') + 1) ..]
                        : this.LocationName;
        }

        public int LocationTotalBooks { get; set; }

        // TO DO
        // Set value - 12/8/2025
        public double TotalCostOfBooks { get; set; }

        public int? ID { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FilterLists.GetAllBooksInLocationList(this.LocationGuid, showHiddenBooks);
            var count = 0;

            if (list != null)
            {
                count = list.Count;
            }

            this.TotalBooksstring = StringManipulation.SetTotalBooksString(count);
            this.LocationTotalBooks = count;
        }
    }
}
