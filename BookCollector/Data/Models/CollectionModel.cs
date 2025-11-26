using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.ComponentModel.DataAnnotations;

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
        [ObservableProperty]
        public bool hideCollection;

        public int? ID { get; set; }
        public string? ParsedCollectionName { get; set; }
        public int CollectionTotalBooks { get; set; }
        public double TotalCostOfBooks { get; set; }

        public CollectionModel()
        {
            CollectionGuid = Guid.NewGuid();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
