// <copyright file="SeriesModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;
    using BookCollector.ViewModels.BaseViewModels;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// SeriesModel class.
    /// </summary>
    public partial class SeriesModel : SeriesDatabaseModel, ICloneable
    {
        /// <summary>
        /// Gets or sets the total count of books to be read.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int toBeReadCount;

        /// <summary>
        /// Gets or sets the total count of books reading.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int readingCount;

        /// <summary>
        /// Gets or sets the total count of books read.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int readCount;

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
        /// Gets the total price of books, formatted as a string.
        /// </summary>
        public string? TotalCostOfBooksString
        {
            get => StringManipulation.SetBookPrice(this.TotalCostOfBooks.ToString());
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
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        public async Task SetTotalBooks(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            var list = await FillLists.GetAllBooksInSeriesList(this.SeriesGuid, showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);

            (this.TotalBooksString,
                this.SeriesTotalBooks,
                this.ToBeReadCount,
                this.ReadingCount,
                this.ReadCount) = GroupingBaseViewModel.SetTotalBooksStringAndCounts(list, this.TotalBooksInSeries);
        }

        /// <summary>
        /// Sets the total cost of books for the author, and updates the TotalCostOfBooks property accordingly.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        public async Task SetTotalCostOfBooks(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInSeriesList(this.SeriesGuid, showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
        }
    }
}