// <copyright file="WishlistBookModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Database;
using BookCollector.Data.DatabaseModels;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BookCollector.Data.Models
{
    public partial class WishlistBookModel : WishlistBookDatabaseModel, ICloneable
    {
        [ObservableProperty]
        public ImageSource? bookCover;

        internal static BookCollectorDatabase Database;

        public WishlistBookModel()
        {
            Database = new BookCollectorDatabase();
        }

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
            get => !string.IsNullOrEmpty(this.BookPrice) ? double.Parse(this.BookPrice[1..]) : 0;
        }

        public DateTime? StartDateValue
        {
            get => !string.IsNullOrEmpty(this.BookStartDate) ? DateTime.Parse(this.BookStartDate) : null;
        }

        public DateTime? EndDateValue
        {
            get => !string.IsNullOrEmpty(this.BookEndDate) ? DateTime.Parse(this.BookEndDate) : null;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetPartOfSeries()
        {
            this.HasSeries = this.BookSeriesGuid != null || !string.IsNullOrEmpty(this.BookSeries);
            var output = string.Empty;

            if (this.BookSeriesGuid != null)
            {
                var series = await GetItems.GetSeriesForBook(this.BookSeriesGuid);

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

        public async Task SetCoverDisplay()
        {
            this.HasBookCover = !string.IsNullOrEmpty(this.BookCoverFileLocation) || !string.IsNullOrEmpty(this.BookCoverUrl) || this.BookCover != null;
            this.HasNoBookCover = string.IsNullOrEmpty(this.BookCoverFileLocation) && string.IsNullOrEmpty(this.BookCoverUrl) && this.BookCover == null;

            BaseViewModel.SetBookCover(this);
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
                        if (authorList.Count > 1)
                        {
                            this.AuthorListString = this.AuthorListString[..(this.AuthorListString.LastIndexOf("; ") - 1)];
                        }
                    }
                }
            }

            return bookAuthorList;
        }

        public async Task SetBookPrice()
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
