// <copyright file="CollectionModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;
    using BookCollector.ViewModels.BaseViewModels;

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
            get => StringManipulation.SetParsedName(this.CollectionName);
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
            (this.TotalBooksString, this.CollectionTotalBooks) = GroupingBaseViewModel.SetTotalBooksStringAndCounts(list);
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
