// <copyright file="CollectionModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;

    /// <summary>
    /// CollectionModel class.
    /// </summary>
    public partial class CollectionModel : CollectionDatabaseModel, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionModel"/> class.
        /// </summary>
        public CollectionModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionModel"/> class.
        /// </summary>
        /// <param name="dbModel">Database model to convert from.</param>
        public CollectionModel(CollectionDatabaseModel dbModel)
        {
            this.CollectionGuid = dbModel.CollectionGuid;
            this.CollectionName = dbModel.CollectionName;
            this.TotalBooksString = dbModel.TotalBooksString;
            this.TotalCostOfBooks = dbModel.TotalCostOfBooks;
            this.HideCollection = dbModel.HideCollection;
            this.CollectionTotalBooks = dbModel.CollectionTotalBooks;
        }

        /// <summary>
        /// Gets the parsed collection name.
        /// </summary>
        public string? ParsedCollectionName
        {
            get => (!string.IsNullOrEmpty(this.CollectionName) &&
                    (this.CollectionName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.CollectionName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.CollectionName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.CollectionName[(this.CollectionName.IndexOf(' ') + 1) ..]
                        : this.CollectionName;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Sets the total books and unread books for the author, and updates the TotalBooksString property accordingly.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Sets the total cost of books for the author, and updates the TotalCostOfBooks property accordingly.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
        public async Task SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInCollectionList(this.CollectionGuid, showHiddenBooks);
        }
    }
}
