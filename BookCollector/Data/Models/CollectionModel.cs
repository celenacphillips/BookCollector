using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BookCollector.Data.Models
{
    public partial class CollectionModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? CollectionGuid { get; set; }

        [ObservableProperty]
        public string? collectionName;
        [ObservableProperty]
        public string? totalBooksString;
        [ObservableProperty]
        public bool hideCollection;

        public int? ID { get; set; }
        public string? ParsedCollectionName
        {
            get => (!string.IsNullOrEmpty(this.CollectionName) &&
                    (this.CollectionName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.CollectionName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.CollectionName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.CollectionName[(this.CollectionName.IndexOf(' ') + 1)..]
                        : this.CollectionName;
        }
        public int CollectionTotalBooks { get; set; }
        // TO DO
        // Set value - 12/8/2025
        public double TotalCostOfBooks { get; set; }

        public CollectionModel()
        {
            CollectionGuid = Guid.NewGuid();
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FilterLists.GetAllBooksInCollectionList(this.CollectionGuid, showHiddenBooks);
            var count = 0;

            if (list != null)
            {
                count = list.Count;
            }

            this.TotalBooksString = StringManipulation.SetTotalBooksString(count);
            this.CollectionTotalBooks = count;
        }
    }
}
