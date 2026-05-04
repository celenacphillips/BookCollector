// <copyright file="LocationModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;
    using BookCollector.ViewModels.BaseViewModels;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// LocationModel class.
    /// </summary>
    public partial class LocationModel : LocationDatabaseModel, ICloneable
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
            this.IsFavorite = dbModel.IsFavorite;
        }

        /// <summary>
        /// Gets the parsed location name.
        /// </summary>
        public string? ParsedLocationName
        {
            get => StringManipulation.SetParsedName(this.LocationName);
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
            var list = await FillLists.GetAllBooksInLocationList(this.LocationGuid, showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);

            (this.TotalBooksString,
                this.LocationTotalBooks,
                this.ToBeReadCount,
                this.ReadingCount,
                this.ReadCount) = GroupingBaseViewModel.SetTotalBooksStringAndCounts(list);
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
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInLocationList(this.LocationGuid, showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
        }
    }
}