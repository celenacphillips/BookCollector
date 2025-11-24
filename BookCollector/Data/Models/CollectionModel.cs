using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class CollectionModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? CollectionGuid { get; set; }

        [ObservableProperty]
        public string collectionName;
        [ObservableProperty]
        public string totalBooksString;

        public int? ID { get; set; }
        public string? ParsedCollectionName { get; set; }
        public int CollectionTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
