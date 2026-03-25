// <copyright file="SeriesModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;
    using BookCollector.ViewModels.BaseViewModels;

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
            get => StringManipulation.SetParsedName(this.SeriesName);
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
            (this.TotalBooksString, this.SeriesTotalBooks) = GroupingBaseViewModel.SetTotalBooksStringAndCounts(list, this.TotalBooksInSeries);
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