// <copyright file="SeriesModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;

    /// <summary>
    /// SeriesModel class.
    /// </summary>
    public partial class SeriesModel : SeriesDatabaseModel, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesModel"/> class.
        /// </summary>
        public SeriesModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesModel"/> class.
        /// </summary>
        /// <param name="dbModel">Database model to convert from.</param>
        public SeriesModel(SeriesDatabaseModel dbModel)
        {
            this.SeriesGuid = dbModel.SeriesGuid;
            this.SeriesName = dbModel.SeriesName;
            this.TotalBooksString = dbModel.TotalBooksString;
            this.TotalCostOfBooks = dbModel.TotalCostOfBooks;
            this.HideSeries = dbModel.HideSeries;
            this.SeriesTotalBooks = dbModel.SeriesTotalBooks;
            this.TotalBooksInSeries = dbModel.TotalBooksInSeries;
        }

        /// <summary>
        /// Gets the parsed series name.
        /// </summary>
        public string? ParsedSeriesName
        {
            get => (!string.IsNullOrEmpty(this.SeriesName) &&
                    (this.SeriesName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.SeriesName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.SeriesName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.SeriesName[(this.SeriesName.IndexOf(' ') + 1) ..]
                        : this.SeriesName;
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
            var list = await FillLists.GetAllBooksInSeriesList(this.SeriesGuid, showHiddenBooks);
            var count = 0;
            var unread = 0;

            if (list != null)
            {
                count = list.Count;
                unread = list.Count(x => (x.BookPageRead == 0 &&
                    (x.BookHourListened == 0 && x.BookMinuteListened == 0))
                    && !x.UpNext);
            }

            this.TotalBooksString = !string.IsNullOrEmpty(this.TotalBooksInSeries) ?
                                    StringManipulation.SetTotalBooksString(count, int.Parse(this.TotalBooksInSeries), unread) :
                                    StringManipulation.SetTotalBooksAndUnreadString(count, unread);
            this.SeriesTotalBooks = count;
        }

        /// <summary>
        /// Sets the total cost of books for the author, and updates the TotalCostOfBooks property accordingly.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
        public async Task SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInSeriesList(this.SeriesGuid, showHiddenBooks);
        }
    }
}
