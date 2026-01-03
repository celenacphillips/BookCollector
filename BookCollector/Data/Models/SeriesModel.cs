// <copyright file="SeriesModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.DatabaseModels;

namespace BookCollector.Data.Models
{
    public partial class SeriesModel : SeriesDatabaseModel, ICloneable
    {
        public SeriesModel()
        {
        }

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

        public string? ParsedSeriesName
        {
            get => (!string.IsNullOrEmpty(this.SeriesName) &&
                    (this.SeriesName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.SeriesName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.SeriesName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.SeriesName[(this.SeriesName.IndexOf(' ') + 1) ..]
                        : this.SeriesName;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FillLists.GetAllBooksInSeriesList(this.SeriesGuid, showHiddenBooks);
            var count = 0;
            var unread = 0;

            if (list != null)
            {
                count = list.Count;
                unread = list.Count(x => x.BookPageRead == 0 && !x.UpNext);
            }

            this.TotalBooksString = !string.IsNullOrEmpty(this.TotalBooksInSeries) ?
                                    StringManipulation.SetTotalBooksString(count, int.Parse(this.TotalBooksInSeries), unread) :
                                    StringManipulation.SetTotalBooksAndUnreadString(count, unread);
            this.SeriesTotalBooks = count;
        }

        public async Task SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInSeriesList(this.SeriesGuid, showHiddenBooks);
        }
    }
}
