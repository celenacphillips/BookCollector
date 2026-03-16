// <copyright file="LocationModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;

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
            get => (!string.IsNullOrEmpty(this.LocationName) &&
                    (this.LocationName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.LocationName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.LocationName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.LocationName[(this.LocationName.IndexOf(' ') + 1) ..]
                        : this.LocationName;
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
            this.LocationTotalBooks = count;
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