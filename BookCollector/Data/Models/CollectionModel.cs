// <copyright file="CollectionModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.DatabaseModels;

namespace BookCollector.Data.Models
{
    public partial class CollectionModel : CollectionDatabaseModel, ICloneable
    {
        public CollectionModel()
        {
        }

        public CollectionModel(CollectionDatabaseModel dbModel)
        {
            this.CollectionGuid = dbModel.CollectionGuid;
            this.CollectionName = dbModel.CollectionName;
            this.TotalBooksString = dbModel.TotalBooksString;
            this.TotalCostOfBooks = dbModel.TotalCostOfBooks;
            this.HideCollection = dbModel.HideCollection;
            this.CollectionTotalBooks = dbModel.CollectionTotalBooks;
        }

        public string? ParsedCollectionName
        {
            get => (!string.IsNullOrEmpty(this.CollectionName) &&
                    (this.CollectionName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.CollectionName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.CollectionName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.CollectionName[(this.CollectionName.IndexOf(' ') + 1) ..]
                        : this.CollectionName;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FillLists.GetAllBooksInCollectionList(this.CollectionGuid, showHiddenBooks);
            var count = 0;
            var unread = 0;

            if (list != null)
            {
                count = list.Count;
                unread = list.Count(x => (x.BookPageRead == 0 &&
                    (x.BookHourListened == 0 && x.BookMinuteListened == 0))
                    && !x.UpNext);
            }

            this.TotalBooksString = StringManipulation.SetTotalBooksAndUnreadString(count, unread);
            this.CollectionTotalBooks = count;
        }

        public async Task SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInCollectionList(this.CollectionGuid, showHiddenBooks);
        }
    }
}
