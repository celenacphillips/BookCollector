using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace BookCollector.Data.Models
{
    public partial class CollectionModel : ObservableValidator, ICloneable
    {
        [PrimaryKey]
        public Guid? CollectionGuid { get; set; }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Field is required.")]
        [MinLength(2)]
        public string collectionName;
        [ObservableProperty]
        public string totalBooksString;
        [ObservableProperty]
        public bool hideCollection;

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
