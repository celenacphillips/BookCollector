using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.Data.Models
{
    public partial class LocationModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? LocationGuid { get; set; }

        [ObservableProperty]
        public string locationName;
        [ObservableProperty]
        public string totalBooksString;
        [ObservableProperty]
        public bool hideLocation;

        public string? ParsedLocationName { get; set; }
        public int LocationTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }
        public int? ID { get; set; }

        public LocationModel()
        {
            LocationGuid = Guid.NewGuid();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
