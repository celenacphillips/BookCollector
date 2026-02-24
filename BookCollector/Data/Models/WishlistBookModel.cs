// <copyright file="WishlistBookModel.cs" company="Castle Software">
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

    public partial class WishlistBookModel : WishlistBookDatabaseModel, ICloneable
    {
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ImageSource? bookCover;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public double? bookTotalTime;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public TimeSpan totalTimeSpan;

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

        public List<AuthorModel?>? SelectedAuthors { get; set; }

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

        public string? BookDurationTotal
        {
            get => !this.BookFormat!.Equals(AppStringResources.Audiobook) ?
                AppStringResources.BlankPages.Replace("Blank", this.BookPageTotal.ToString()) :
                AppStringResources.Blank1HoursBlank2Minutes.Replace("Blank1", this.BookHoursTotal.ToString().PadLeft(2, '0')).Replace("Blank2", this.BookMinutesTotal.ToString().PadLeft(2, '0'));
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetPartOfSeries()
        {
            this.HasSeries = !string.IsNullOrEmpty(this.BookSeries);
            var output = string.Empty;

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
            this.HasBookCover = !string.IsNullOrEmpty(this.BookCoverFileName) || !string.IsNullOrEmpty(this.BookCoverUrl) || this.BookCover != null;
            this.HasNoBookCover = string.IsNullOrEmpty(this.BookCoverFileName) && string.IsNullOrEmpty(this.BookCoverUrl) && this.BookCover == null;

            BaseViewModel.SetBookCover(this);
        }

        public async Task<ObservableCollection<BookAuthorModel>> SetAuthorListString(ObservableCollection<AuthorModel>? authorList, bool addToBookAuthorlist = true)
        {
            var bookAuthorList = new ObservableCollection<BookAuthorModel>();

            this.AuthorListString = string.Empty;

            if (authorList != null)
            {
                authorList = authorList.Where(x => !string.IsNullOrEmpty(x.FirstName) && !string.IsNullOrEmpty(x.LastName)).ToObservableCollection();

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
                            this.AuthorListString = this.AuthorListString[.. (this.AuthorListString.LastIndexOf("; ") - 1)];
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

        public async Task SetBookTotalTime()
        {
            this.BookTotalTime = this.BookFormat!.Equals(AppStringResources.Audiobook) ? (double)this.BookHoursTotal + ((double)this.BookMinutesTotal / 60) : null;
        }

        public static TimeSpan SetTime(int hour, int minute)
        {
            return new TimeSpan(hour, minute, 0);
        }
    }
}
