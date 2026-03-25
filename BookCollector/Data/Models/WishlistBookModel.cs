// <copyright file="WishlistBookModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.ViewModels.BaseViewModels;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// WishlistBookModel class.
    /// </summary>
    public partial class WishlistBookModel : WishlistBookDatabaseModel, ICloneable
    {
        /// <summary>
        /// Gets or sets the book cover image source.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ImageSource? bookCover;

        /// <summary>
        /// Gets or sets the book total time.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public double? bookTotalTime;

        /// <summary>
        /// Gets or sets the book time span.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public TimeSpan totalTimeSpan;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistBookModel"/> class.
        /// </summary>
        public WishlistBookModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistBookModel"/> class.
        /// </summary>
        /// <param name="dbModel">Database model to convert from.</param>
        public WishlistBookModel(WishlistBookDatabaseModel dbModel)
        {
            this.BookGuid = dbModel.BookGuid;
            this.BookTitle = dbModel.BookTitle;
            this.BookNumberInSeries = dbModel.BookNumberInSeries;
            this.BookPublisher = dbModel.BookPublisher;
            this.BookPublishYear = dbModel.BookPublishYear;
            this.BookFormat = dbModel.BookFormat;
            this.BookLanguage = dbModel.BookLanguage;
            this.BookPrice = dbModel.BookPrice;
            this.BookSummary = dbModel.BookSummary;
            this.BookPageTotal = dbModel.BookPageTotal;
            this.BookHoursTotal = dbModel.BookHoursTotal;
            this.BookMinutesTotal = dbModel.BookMinutesTotal;
            this.BookComments = dbModel.BookComments;
            this.BookCoverUrl = dbModel.BookCoverUrl;
            this.HasBookCover = dbModel.HasBookCover;
            this.HasNoBookCover = dbModel.HasNoBookCover;
            this.HasSeries = dbModel.HasSeries;
            this.BookURL = dbModel.BookURL;
            this.BookWhereToBuy = dbModel.BookWhereToBuy;
            this.HideBook = dbModel.HideBook;
            this.PartOfSeries = dbModel.PartOfSeries;
            this.BookCoverFileName = dbModel.BookCoverFileName;
            this.AuthorListString = dbModel.AuthorListString;
            this.IsFavorite = dbModel.IsFavorite;
            this.Rating = dbModel.Rating;
            this.BookAuthors = dbModel.BookAuthors;
            this.BookSeries = dbModel.BookSeries;
            this.BookIdentifier = dbModel.BookIdentifier;
        }

        /// <summary>
        /// Gets or sets the list of selected authors.
        /// </summary>
        public List<AuthorModel?>? SelectedAuthors { get; set; }

        /// <summary>
        /// Gets publisher publish date string.
        /// </summary>
        public string PublisherPublishDatestring
        {
            get => StringManipulation.SetPublisherPublishDateString(this.BookPublisher, this.BookPublishYear);
        }

        /// <summary>
        /// Gets the parsed book title.
        /// </summary>
        public string? ParsedTitle
        {
            get => StringManipulation.SetParsedName(this.BookTitle);
        }

        /// <summary>
        /// Gets the book price as a double.
        /// </summary>
        public double BookPriceValue
        {
            get => BookBaseViewModel.SetBookPriceValue(this.BookPrice);
        }

        /// <summary>
        /// Gets the book duration total string.
        /// </summary>
        public string? BookDurationTotal
        {
            get => StringManipulation.SetBookDurationTotal(this.BookFormat, this.BookPageTotal, this.BookHoursTotal, this.BookMinutesTotal);
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
        /// Sets the part of series string based on the book's series information, including the series name and book number in the series if available.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task SetPartOfSeries()
        {
            (this.HasSeries, this.PartOfSeries, this.BookSeries) = await StringManipulation.SetSeriesString(this.BookSeriesGuid, this.BookSeries, this.BookNumberInSeries);
        }

        /// <summary>
        /// Set the book cover display.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task SetCoverDisplay()
        {
            (this.HasBookCover, this.HasNoBookCover, this.BookCover) = await BookBaseViewModel.SetCoverDisplay(this.BookCoverFileName, this.BookCoverUrl, this.BookCover);
        }

        /// <summary>
        /// Sets the author list string for the book.
        /// </summary>
        /// <param name="authorList">Author list to parse.</param>
        /// <returns>A list of book authors.</returns>
        public async Task SetAuthorListStringFromInputList(ObservableCollection<AuthorModel>? authorList)
        {
            this.AuthorListString = BookBaseViewModel.SetAuthorListStringFromInputList(authorList);
        }

        /// <summary>
        /// Set book price, formatted with currency symbol.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task SetBookPrice()
        {
            this.BookPrice = StringManipulation.SetBookPrice(this.BookPrice);
        }

        /// <summary>
        /// Sets the book total time based on the book format and updates the book total time property accordingly.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task SetBookTotalTime()
        {
            this.BookTotalTime = BookBaseViewModel.SetBookTotalTime(this.BookFormat, this.BookHoursTotal, this.BookMinutesTotal);
        }
    }
}
