using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class CollectionModel : ObservableObject, ICloneable
    {
        [ObservableProperty]
        public string? collectionName;
        [ObservableProperty]
        public string? totalBooksstring;
        [ObservableProperty]
        public bool hideCollection;

        public CollectionModel()
        {
            this.CollectionGuid = Guid.NewGuid();
        }

        [PrimaryKey]
        public Guid? CollectionGuid { get; set; }

        public int? ID { get; set; }

        public string? ParsedCollectionName
        {
            get => (!string.IsNullOrEmpty(this.CollectionName) &&
                    (this.CollectionName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.CollectionName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.CollectionName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.CollectionName[(this.CollectionName.IndexOf(' ') + 1) ..]
                        : this.CollectionName;
        }

        public int CollectionTotalBooks { get; set; }

        public double TotalCostOfBooks { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async void SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FilterLists.GetAllBooksInCollectionList(this.CollectionGuid, showHiddenBooks);
            var count = 0;

            if (list != null)
            {
                count = list.Count;
            }

            this.TotalBooksstring = StringManipulation.SetTotalBooksString(count);
            this.CollectionTotalBooks = count;
        }

        public async void SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await FilterLists.GetAllBookPricesInCollectionList(this.CollectionGuid, showHiddenBooks);
        }
    }
}
