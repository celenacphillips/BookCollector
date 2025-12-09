using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BookCollector.Data.Models
{
    public partial class BookModel : ObservableObject, ICloneable
    {
        [PrimaryKey]
        public Guid? BookGuid { get; set; }

        [ObservableProperty]
        public string? bookTitle;
        [ObservableProperty]
        public double? bookNumberInSeries;
        [ObservableProperty]
        public string? bookPublisher;
        [ObservableProperty]
        public string? bookPublishYear;
        [ObservableProperty]
        public string? bookIdentifier;
        [ObservableProperty]
        public string? bookFormat;
        [ObservableProperty]
        public string? bookLanguage;
        [ObservableProperty]
        public string? bookPrice;
        [ObservableProperty]
        public string? bookSummary;
        [ObservableProperty]
        public int bookPageRead;
        [ObservableProperty]
        public int bookPageTotal;
        [ObservableProperty]
        public double progress;
        [ObservableProperty]
        public string? pageReadPercent;
        [ObservableProperty]
        public string? bookStartDate;
        [ObservableProperty]
        public string? bookEndDate;
        [ObservableProperty]
        public string? bookComments;
        [ObservableProperty]
        public string? bookCoverUrl;
        [ObservableProperty]
        public bool hasBookCover;
        [ObservableProperty]
        public bool hasNoBookCover;
        [ObservableProperty]
        public bool hasSeries;
        [ObservableProperty]
        public bool hasCollection;
        [ObservableProperty]
        public string? bookURL;
        [ObservableProperty]
        public string? bookWhereToBuy;
        [ObservableProperty]
        public string? loanedTo;
        [ObservableProperty]
        public string? bookLoanedOutOn;
        [ObservableProperty]
        public bool upNext;
        [ObservableProperty]
        public bool hideBook;
        [ObservableProperty]
        public int half;
        [ObservableProperty]
        public int fourth;
        [ObservableProperty]
        public int threeFourth;
        [ObservableProperty]
        public string? partOfSeries;
        [ObservableProperty]
        public string? partOfCollection;
        [ObservableProperty]
        public ImageSource? bookCover;
        [ObservableProperty]
        public string? bookCoverFileLocation;

        public BookModel()
        {
            BookGuid = Guid.NewGuid();
        }

        public string PublisherPublishDateString
        {
            get => $"{(!string.IsNullOrEmpty(this.BookPublisher) ? this.BookPublisher : AppStringResources.NoPublisher)}, {(!string.IsNullOrEmpty(this.BookPublishYear) ? this.BookPublishYear : AppStringResources.NoDate)}";
        }
        public string? ParsedTitle
        {
            get => (!string.IsNullOrEmpty(this.BookTitle) &&
                    (this.BookTitle.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.BookTitle.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.BookTitle.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.BookTitle[(this.BookTitle.IndexOf(' ') + 1)..]
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
        public DateTime? LoanedOutOnValue
        {
            get => !string.IsNullOrEmpty(this.BookLoanedOutOn) ? DateTime.Parse(this.BookLoanedOutOn) : null;
        }

        public Guid? BookSeriesGuid { get; set; }
        public Guid? BookCollectionGuid { get; set; }
        public Guid? BookGenreGuid { get; set; }
        public Guid? BookLocationGuid { get; set; }
        public string? AuthorListString { get; set; }
        public bool IsFavorite { get; set; }
        public int Rating { get; set; }
        public string? BookAuthors { get; set; }
        public string? BookSeries { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void SetReadingProgress()
        {
            this.Progress = this.BookPageTotal != 0 ? this.BookPageRead / (double)this.BookPageTotal : 0;
            this.PageReadPercent = $"{Math.Round(this.Progress * 100, 2)}%";
        }

        public void SetBookCheckpoints()
        {
            this.Half = this.BookPageTotal / 2;
            this.Fourth = this.BookPageTotal / 4;
            this.ThreeFourth = this.Half + this.Fourth;
        }

        //TO DO
        // Bug when adding date and going back to Main View - 12/8/2025
        public async Task SetDates()
        {
            if (!string.IsNullOrEmpty(this.BookStartDate))
            {
                this.BookStartDate = this.StartDateValue?.ToShortDateString();
            }

            if (!string.IsNullOrEmpty(this.BookEndDate))
            {
                this.BookEndDate = this.EndDateValue?.ToShortDateString();
            }

            if (!string.IsNullOrEmpty(this.BookLoanedOutOn))
            {
                this.BookLoanedOutOn = this.LoanedOutOnValue?.ToShortDateString();
            }
        }

        public async Task SetPartOfSeries()
        {
            this.HasSeries = this.BookSeriesGuid != null || !string.IsNullOrEmpty(this.BookSeries);
            var output = string.Empty;

            if (this.BookSeriesGuid != null)
            {
                var series = await FilterLists.GetSeriesForBook(this.BookSeriesGuid);

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
                var collection = await FilterLists.GetCollectionForBook(this.BookCollectionGuid);

                if (collection != null)
                {
                    output = $"{AppStringResources.PartOfCollection.Replace("blank", $"{collection.CollectionName}")}";
                }
            }

            this.PartOfCollection = output;
        }

        public void SetCoverDisplay()
        {
            this.HasBookCover = !string.IsNullOrEmpty(this.BookCoverFileLocation) || !string.IsNullOrEmpty(this.BookCoverUrl) || this.BookCover != null;
            this.HasNoBookCover = string.IsNullOrEmpty(this.BookCoverFileLocation) && string.IsNullOrEmpty(this.BookCoverUrl) && this.BookCover == null;
        }

        public async Task SetAuthorListString()
        {
            ObservableCollection<AuthorModel>? authorList = null;
            ObservableCollection<BookAuthorModel>? bookAuthorList = await FilterLists.GetAllBookAuthorsForBook(this.BookGuid);

            if (TestData.UseTestData)
            {
                authorList = TestData.AuthorList;

            }
            else
            {

            }

            if (bookAuthorList != null && bookAuthorList.Any(x => x.BookGuid == this.BookGuid) && authorList != null)
            {
                this.AuthorListString = string.Empty;

                for (int i = 0; i < bookAuthorList.Count; i++)
                {
                    var author = authorList.FirstOrDefault(x => x.AuthorGuid == bookAuthorList[i].AuthorGuid);

                    if (author != null)
                    {
                        this.AuthorListString += author.ReverseFullName;

                        if (i != bookAuthorList.Count - 1)
                            this.AuthorListString += "; ";
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

                        if (addToBookAuthorlist)
                        {
                            bookAuthorList.Add(new BookAuthorModel()
                            {
                                BookGuid = (Guid)this.BookGuid,
                                AuthorGuid = (Guid)authorList[i].AuthorGuid,
                            });
                        }

                        this.AuthorListString += authorList[i].ReverseFullName;

                        if (i != authorList.Count - 1)
                            this.AuthorListString += "; ";
                    }
                    else
                    {
                        this.AuthorListString = this.AuthorListString[..(this.AuthorListString.LastIndexOf("; ") - 1)];
                    }
                }
            }

            return bookAuthorList;
        }

        public async Task SetBookChapters(ObservableCollection<ChapterModel>? chaptersList)
        {
            if (chaptersList != null)
            {
                foreach (var chapter in chaptersList)
                {
                    if (!string.IsNullOrEmpty(chapter.ChapterName))
                    {
                        chapter.BookGuid = (Guid)this.BookGuid;

                        if (TestData.UseTestData)
                        {
                            TestData.UpdateChapter(chapter);
                        }
                        else
                        {

                        }
                    }
                }
            }
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
