// <copyright file="LocationModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.DatabaseModels;
using System.Threading;

namespace BookCollector.Data.Models
{
    public partial class LocationModel : LocationDatabaseModel, ICloneable
    {
        public LocationModel()
        {
        }

        public LocationModel(LocationDatabaseModel dbModel)
        {
            this.LocationGuid = dbModel.LocationGuid;
            this.LocationName = dbModel.LocationName;
            this.TotalBooksString = dbModel.TotalBooksString;
            this.TotalCostOfBooks = dbModel.TotalCostOfBooks;
            this.HideLocation = dbModel.HideLocation;
            this.TotalCostOfBooks = dbModel.LocationTotalBooks;
        }

        public string? ParsedLocationName
        {
            get => (!string.IsNullOrEmpty(this.LocationName) &&
                    (this.LocationName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.LocationName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.LocationName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.LocationName[(this.LocationName.IndexOf(' ') + 1) ..]
                        : this.LocationName;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

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

        public async Task SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInLocationList(this.LocationGuid, showHiddenBooks);
        }
    }
}
