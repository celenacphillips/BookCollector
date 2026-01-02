// <copyright file="BookModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Globalization;
using BookCollector.Data.Database;
using BookCollector.Data.DatabaseModels;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BookCollector.Data.Models
{
    public partial class BookModel : BookDatabaseModel, ICloneable
    {
        [ObservableProperty]
        public ImageSource? bookCover;

        internal static BookCollectorDatabase Database;

        public BookModel()
        {
            Database = new BookCollectorDatabase();
        }

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
            this.BookPageTotal = dbModel.BookPageTotal;
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
            this.Half = dbModel.Half;
            this.Fourth = dbModel.Fourth;
            this.ThreeFourth = dbModel.ThreeFourth;
            this.PartOfSeries = dbModel.PartOfSeries;
            this.PartOfCollection = dbModel.PartOfCollection;
            this.BookCoverFileLocation = dbModel.BookCoverFileLocation;
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

        public AuthorModel? SelectedAuthor { get; set; }

        public string PublisherPublishDatestring
        {
            get => $"{(!string.IsNullOrEmpty(this.BookPublisher) ? this.BookPublisher : AppStringResources.NoPublisher)}, {(!string.IsNullOrEmpty(this.BookPublishYear) ? this.BookPublishYear : AppStringResources.NoDate)}";
        }

        public string? ParsedTitle
        {
            get => (!string.IsNullOrEmpty(this.BookTitle) &&
                    (this.BookTitle.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.BookTitle.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.BookTitle.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.BookTitle[(this.BookTitle.IndexOf(' ') + 1) ..]
                        : this.BookTitle;
        }

        public double BookPriceValue
        {
            get => !string.IsNullOrEmpty(this.BookPrice) ? (this.BookPrice.StartsWith(new CultureInfo(Preferences.Get("CultureCode", "en-US" /* Default */)).NumberFormat.CurrencySymbol) ? double.Parse(this.BookPrice[1..]) : double.Parse(this.BookPrice)) : 0;
        }

        public DateTime? StartDateValue
        {
            get => !string.IsNullOrEmpty(this.BookStartDate) ? DateTime.Parse(this.BookStartDate) : null;
        }

        public DateTime? EndDateValue
        {
            get => !string.IsNullOrEmpty(this.BookEndDate) ? DateTime.Parse(this.BookEndDate) : null;
        }

        public DateTime? LoanedOutOnValue
        {
            get => !string.IsNullOrEmpty(this.BookLoanedOutOn) ? DateTime.Parse(this.BookLoanedOutOn) : null;
        }

        public static string? SetDate(string? input)
        {
            string? output = null;

            if (!string.IsNullOrEmpty(input))
            {
                output = DateTime.Parse(input).ToString("MM/dd/yyy");
            }

            return output;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetReadingProgress()
        {
            this.Progress = this.BookPageTotal != 0 ? this.BookPageRead / (double)this.BookPageTotal : 0;
            this.PageReadPercent = $"{System.Math.Round(this.Progress * 100, 2)}%";
        }

        public void SetBookCheckpoints(bool showCheckpoints)
        {
            if (showCheckpoints)
            {
                this.Half = this.BookPageTotal / 2;
                this.Fourth = this.BookPageTotal / 4;
                this.ThreeFourth = this.Half + this.Fourth;
            }
        }

        public async Task SetPartOfSeries()
        {
            this.HasSeries = this.BookSeriesGuid != null || !string.IsNullOrEmpty(this.BookSeries);
            var output = string.Empty;

            if (this.BookSeriesGuid != null)
            {
                var series = await GetItems.GetSeriesForBook(this.BookSeriesGuid);

                this.BookSeries = null;

                if (series != null)
                {
                    if (this.BookNumberInSeries != null)
                    {
                        output = $"{AppStringResources.PartofSeries.Replace("blank", $"{series.SeriesName}")}, {AppStringResources.BookNumber.Replace("Number", $"{this.BookNumberInSeries}")}";
                    }

                    if (this.BookNumberInSeries != null && !string.IsNullOrEmpty(series.TotalBooksInSeries))
                    {
                        output = $"{AppStringResources.PartofSeries.Replace("blank", $"{series.SeriesName}")}, {AppStringResources.BookNumberOfTotal.Replace("number", $"{this.BookNumberInSeries}").Replace("total", $"{series.TotalBooksInSeries}")}";
                    }

                    if (this.BookNumberInSeries == null)
                    {
                        output = $"{AppStringResources.PartofSeries.Replace("blank", $"{series.SeriesName}")}";
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.BookSeries))
            {
                if (this.BookNumberInSeries != null)
                {
                    output = $"{AppStringResources.PartofSeries.Replace("blank", $"{this.BookSeries}")}, {AppStringResources.BookNumber.Replace("Number", $"{this.BookNumberInSeries}")}";
                }

                if (this.BookNumberInSeries == null)
                {
                    output = $"{AppStringResources.PartofSeries.Replace("blank", $"{this.BookSeries}")}";
                }
            }

            this.PartOfSeries = output;
        }

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

        public async Task SetCoverDisplay()
        {
            this.HasBookCover = !string.IsNullOrEmpty(this.BookCoverFileLocation) || !string.IsNullOrEmpty(this.BookCoverUrl) || this.BookCover != null;
            this.HasNoBookCover = string.IsNullOrEmpty(this.BookCoverFileLocation) && string.IsNullOrEmpty(this.BookCoverUrl) && this.BookCover == null;

            BaseViewModel.SetBookCover(this);
        }

        public async Task SetAuthorListString()
        {
            Database = Database ?? new BookCollectorDatabase();

            ObservableCollection<AuthorModel>? authorList = [];
            ObservableCollection<Guid>? authorGuidList = await FillLists.GetAllAuthorGuidsForBook(this.BookGuid);

            if (TestData.UseTestData)
            {
                if (authorGuidList != null && authorGuidList.Count > 0)
                {
                    foreach (var authorGuid in authorGuidList)
                    {
                        var author = TestData.AuthorList.FirstOrDefault(x => x.AuthorGuid == authorGuid);

                        if (author != null)
                        {
                            authorList.Add(author);
                        }
                    }
                }
            }
            else
            {
                if (authorGuidList != null && authorGuidList.Count > 0)
                {
                    var list = await Database.GetAllAuthorsForBookAsync([.. authorGuidList]);
                    authorList = list.ToObservableCollection();
                }
            }

            if (authorList != null)
            {
                this.AuthorListString = string.Empty;

                for (int i = 0; i < authorList.Count; i++)
                {
                    var author = authorList[i];

                    if (author != null)
                    {
                        this.AuthorListString += author.ReverseFullName;

                        if (i != authorList.Count - 1)
                        {
                            this.AuthorListString += "; ";
                        }
                    }
                }
            }
        }

        public async Task<ObservableCollection<BookAuthorModel>> SetAuthorListString(ObservableCollection<AuthorModel>? authorList, bool addToBookAuthorlist = true)
        {
            var bookAuthorList = new ObservableCollection<BookAuthorModel>();

            this.AuthorListString = string.Empty;

            if (authorList != null)
            {
                for (int i = 0; i < authorList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(authorList[i].FirstName) &&
                        !string.IsNullOrEmpty(authorList[i].LastName))
                    {
                        if (addToBookAuthorlist && this.BookGuid != null && authorList[i].AuthorGuid != null)
                        {
                            bookAuthorList.Add(new BookAuthorModel()
                            {
                                BookGuid = this.BookGuid.Value,
                                AuthorGuid = authorList[i].AuthorGuid.Value,
                            });
                        }

                        this.AuthorListString += authorList[i].ReverseFullName;

                        if (i != authorList.Count - 1)
                        {
                            this.AuthorListString += "; ";
                        }
                    }
                    else
                    {
                        this.AuthorListString = this.AuthorListString[.. (this.AuthorListString.LastIndexOf("; ") - 1)];
                    }
                }
            }

            return bookAuthorList;
        }

        public async Task SetBookChapters(ObservableCollection<ChapterModel>? chaptersList)
        {
            Database = Database ?? new BookCollectorDatabase();

            if (chaptersList != null)
            {
                foreach (var chapter in chaptersList)
                {
                    if (!string.IsNullOrEmpty(chapter.ChapterName) && this.BookGuid != null)
                    {
                        chapter.BookGuid = (Guid)this.BookGuid;

                        if (TestData.UseTestData)
                        {
                            TestData.UpdateChapter(chapter);
                        }
                        else
                        {
                            await Database.SaveChapterAsync(BaseViewModel.ConvertTo<ChapterDatabaseModel>(chapter));
                        }
                    }
                }
            }
        }

        public async Task RemoveBookChapters(List<ChapterModel>? chaptersList)
        {
            Database = Database ?? new BookCollectorDatabase();

            if (chaptersList != null)
            {
                foreach (var chapter in chaptersList)
                {
                    if (!string.IsNullOrEmpty(chapter.ChapterName) && this.BookGuid != null)
                    {
                        if (TestData.UseTestData)
                        {
                            TestData.DeleteChapter(chapter);
                        }
                        else
                        {
                            await Database.DeleteChapterAsync(BaseViewModel.ConvertTo<ChapterDatabaseModel>(chapter));
                        }
                    }
                }
            }
        }

        public void SetBookPrice()
        {
            var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);

            var cultureInfo = new CultureInfo(cultureCode);

            if (this.BookPrice == null || !this.BookPrice.Contains(cultureInfo.NumberFormat.CurrencySymbol))
            {
                var parsed = double.TryParse(this.BookPrice, out double price);

                this.BookPrice = string.Format(cultureInfo, "{0:C}", parsed ? price : 0);
            }
        }
    }
}
