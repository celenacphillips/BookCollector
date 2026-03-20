// <copyright file="BookModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using BookCollector.Data.Database;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// BookModel class.
    /// </summary>
    public partial class BookModel : BookDatabaseModel, ICloneable
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
        /// Gets or sets the total time string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string totalTimeString;

        /// <summary>
        /// Gets or sets the listen time span.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public TimeSpan listenTimeSpan;

        /// <summary>
        /// Gets or sets the listen time.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public double? bookListenedTime;

        /// <summary>
        /// Gets or sets the listen time string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string listenTimeString;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookModel"/> class.
        /// </summary>
        public BookModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookModel"/> class.
        /// </summary>
        /// <param name="dbModel">Database model to convert from.</param>
        public BookModel(BookDatabaseModel dbModel)
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
            this.BookPageRead = dbModel.BookPageRead;
            this.BookHourListened = dbModel.BookHourListened;
            this.BookMinuteListened = dbModel.BookMinuteListened;
            this.BookPageTotal = dbModel.BookPageTotal;
            this.BookHoursTotal = dbModel.BookHoursTotal;
            this.BookMinutesTotal = dbModel.BookMinutesTotal;
            this.Progress = dbModel.Progress;
            this.PageReadPercent = dbModel.PageReadPercent;
            this.BookStartDate = dbModel.BookStartDate;
            this.BookEndDate = dbModel.BookEndDate;
            this.BookComments = dbModel.BookComments;
            this.BookCoverUrl = dbModel.BookCoverUrl;
            this.HasBookCover = dbModel.HasBookCover;
            this.HasNoBookCover = dbModel.HasNoBookCover;
            this.HasSeries = dbModel.HasSeries;
            this.HasCollection = dbModel.HasCollection;
            this.BookURL = dbModel.BookURL;
            this.BookWhereToBuy = dbModel.BookWhereToBuy;
            this.LoanedTo = dbModel.LoanedTo;
            this.BookLoanedOutOn = dbModel.BookLoanedOutOn;
            this.UpNext = dbModel.UpNext;
            this.HideBook = dbModel.HideBook;
            this.HalfPage = dbModel.HalfPage;
            this.FourthPage = dbModel.FourthPage;
            this.ThreeFourthPage = dbModel.ThreeFourthPage;
            this.HalfHours = dbModel.HalfHours;
            this.FourthHours = dbModel.FourthHours;
            this.ThreeFourthHours = dbModel.ThreeFourthHours;
            this.PartOfSeries = dbModel.PartOfSeries;
            this.PartOfCollection = dbModel.PartOfCollection;
            this.BookCoverFileName = dbModel.BookCoverFileName;
            this.BookSeriesGuid = dbModel.BookSeriesGuid;
            this.BookCollectionGuid = dbModel.BookCollectionGuid;
            this.BookGenreGuid = dbModel.BookGenreGuid;
            this.BookLocationGuid = dbModel.BookLocationGuid;
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
        /// Gets the book start date value.
        /// </summary>
        public DateTime? StartDateValue
        {
            get => !string.IsNullOrEmpty(this.BookStartDate) ? DateTime.Parse(this.BookStartDate) : null;
        }

        /// <summary>
        /// Gets the book end date value.
        /// </summary>
        public DateTime? EndDateValue
        {
            get => !string.IsNullOrEmpty(this.BookEndDate) ? DateTime.Parse(this.BookEndDate) : null;
        }

        /// <summary>
        /// Gets the loaned out date value.
        /// </summary>
        public DateTime? LoanedOutOnValue
        {
            get => !string.IsNullOrEmpty(this.BookLoanedOutOn) ? DateTime.Parse(this.BookLoanedOutOn) : null;
        }

        /// <summary>
        /// Gets the book duration total string.
        /// </summary>
        public string? BookDurationTotal
        {
            get => StringManipulation.SetBookDurationTotal(this.BookFormat, this.BookPageTotal, this.BookHoursTotal, this.BookMinutesTotal);
        }

        /// <summary>
        /// Formats the input date string to "MM/dd/yyyy" format if it's not null or empty.
        /// </summary>
        /// <param name="input">Date to format.</param>
        /// <returns>Formatted date as string.</returns>
        public static string? SetDate(string? input)
        {
            string? output = null;

            if (!string.IsNullOrEmpty(input))
            {
                output = DateTime.Parse(input).ToString("MM/dd/yyy");
            }

            return output;
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
        /// Sets the reading progress of the book based on the format and updates the page read percentage string.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task SetReadingProgress()
        {
            if (this.BookFormat == null || (this.BookFormat != null && !this.BookFormat.Equals(AppStringResources.Audiobook)))
            {
                this.Progress = this.BookPageTotal != 0 ? this.BookPageRead / (double)this.BookPageTotal : 0;
            }
            else
            {
                this.SetBookListenedTime();
                await this.SetBookTotalTime();
                this.Progress = ((this.BookTotalTime != null && this.BookTotalTime != 0) ? this.BookListenedTime / (double)this.BookTotalTime : 0) ?? 0;
            }

            this.PageReadPercent = $"{System.Math.Round(this.Progress * 100, 2)}%";
        }

        /// <summary>
        /// Sets the book checkpoints (half, fourth, three fourth) based on the total pages or total time depending on the book format.
        /// </summary>
        /// <param name="showCheckpoints">Show checkpoints.</param>
        /// <returns>A task.</returns>
        public async Task SetBookCheckpoints(bool showCheckpoints)
        {
            if (showCheckpoints && (this.BookFormat == null || (this.BookFormat != null && !this.BookFormat.Equals(AppStringResources.Audiobook))))
            {
                this.HalfPage = this.BookPageTotal / 2;
                this.FourthPage = this.BookPageTotal / 4;
                this.ThreeFourthPage = this.HalfPage + this.FourthPage;
            }

            if (showCheckpoints && this.BookFormat != null && this.BookFormat!.Equals(AppStringResources.Audiobook))
            {
                await this.SetBookTotalTime();
                this.HalfHours = (this.BookTotalTime ?? 0) / 2;
                this.FourthHours = (this.BookTotalTime ?? 0) / 4;
                this.ThreeFourthHours = this.HalfHours + this.FourthHours;
            }
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
        /// Sets the part of collection string based on the book's collection information.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task SetPartOfCollection()
        {
            this.HasCollection = this.BookCollectionGuid != null;
            var output = string.Empty;

            if (this.BookCollectionGuid != null)
            {
                var collection = await GetItems.GetCollectionForBook(this.BookCollectionGuid);

                if (collection != null)
                {
                    output = $"{AppStringResources.PartOfCollection.Replace("blank", $"{collection.CollectionName}")}";
                }
            }

            this.PartOfCollection = output;
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
        /// Set the author list string for the book.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task SetAuthorListStringFromDatabase()
        {
            ObservableCollection<AuthorModel>? authorList = [];
            ObservableCollection<Guid>? authorGuidList = await FillLists.GetAllAuthorGuidsForBook(this.BookGuid);

            if (authorGuidList != null && authorGuidList.Count > 0)
            {
                var list = await BaseViewModel.Database.GetAllAuthorsForBookAsync([.. authorGuidList]);
                authorList = list.ToObservableCollection();
            }

            if (authorList != null)
            {
                this.AuthorListString = BookBaseViewModel.SetAuthorListStringFromInputList(authorList);
            }
        }

        /// <summary>
        /// Sets the author list string for the book.
        /// </summary>
        /// <param name="authorList">Author list to parse.</param>
        /// <returns>A task.</returns>
        public async Task SetAuthorListStringFromInputList(ObservableCollection<AuthorModel>? authorList)
        {
            this.AuthorListString = BookBaseViewModel.SetAuthorListStringFromInputList(authorList);
        }

        /// <summary>
        /// Sets the book chapters for the book by saving each chapter in the provided list to the
        /// database with the associated book GUID.
        /// </summary>
        /// <param name="chaptersList">Chapter list to parse.</param>
        /// <returns>A task.</returns>
        public async Task SetBookChapters(ObservableCollection<ChapterModel>? chaptersList)
        {
            if (chaptersList != null)
            {
                foreach (var chapter in chaptersList)
                {
                    if (!string.IsNullOrEmpty(chapter.ChapterName) && this.BookGuid != null)
                    {
                        chapter.BookGuid = (Guid)this.BookGuid;

                        await BaseViewModel.Database.SaveChapterAsync(BaseViewModel.ConvertTo<ChapterDatabaseModel>(chapter));
                    }
                }
            }
        }

        /// <summary>
        /// Removes the book chapters for the book by deleting each chapter in the provided list from
        /// the database based on the associated book GUID and chapter name.
        /// </summary>
        /// <param name="chaptersList">Chapter list to remove.</param>
        /// <returns>A task.</returns>
        public async Task RemoveBookChapters(List<ChapterModel>? chaptersList)
        {
            if (chaptersList != null)
            {
                foreach (var chapter in chaptersList)
                {
                    if (!string.IsNullOrEmpty(chapter.ChapterName) && this.BookGuid != null)
                    {
                        await BaseViewModel.Database.DeleteChapterAsync(BaseViewModel.ConvertTo<ChapterDatabaseModel>(chapter));
                    }
                }
            }
        }

        /// <summary>
        /// Set book price, formatted with currency symbol.
        /// </summary>
        public void SetBookPrice()
        {
            this.BookPrice = StringManipulation.SetBookPrice(this.BookPrice);
        }

        /// <summary>
        /// Sets the book listened time based on the book format and updates the book listened time property accordingly.
        /// </summary>
        public void SetBookListenedTime()
        {
            this.BookListenedTime = this.BookFormat!.Equals(AppStringResources.Audiobook) ? (double)this.BookHourListened + ((double)this.BookMinuteListened / 60) : null;
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
