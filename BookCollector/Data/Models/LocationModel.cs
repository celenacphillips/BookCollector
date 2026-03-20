// <copyright file="LocationModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;
    using BookCollector.ViewModels.BaseViewModels;

    /// <summary>
    /// LocationModel class.
    /// </summary>
    public partial class LocationModel : LocationDatabaseModel, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationModel"/> class.
        /// </summary>
        public LocationModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationModel"/> class.
        /// </summary>
        /// <param name="dbModel">Database model to convert from.</param>
        public LocationModel(LocationDatabaseModel dbModel)
        {
            this.LocationGuid = dbModel.LocationGuid;
            this.LocationName = dbModel.LocationName;
            this.TotalBooksString = dbModel.TotalBooksString;
            this.TotalCostOfBooks = dbModel.TotalCostOfBooks;
            this.HideLocation = dbModel.HideLocation;
            this.TotalCostOfBooks = dbModel.LocationTotalBooks;
        }

        /// <summary>
        /// Gets the parsed location name.
        /// </summary>
        public string? ParsedLocationName
        {
            get => StringManipulation.SetParsedName(this.LocationName);
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
            var list = await FillLists.GetAllBooksInLocationList(this.LocationGuid, showHiddenBooks);
            (this.TotalBooksString, this.LocationTotalBooks) = GroupingBaseViewModel.SetTotalBooksStringAndCounts(list);
        }

        /// <summary>
        /// Sets the total cost of books for the author, and updates the TotalCostOfBooks property accordingly.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
        public async Task SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInLocationList(this.LocationGuid, showHiddenBooks);
        }
    }
}