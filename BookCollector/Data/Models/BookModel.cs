using BookCollector.Resources.Localization;
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
        public byte[]? bookCoverBytes;
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
            get => (BookTitle.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) || BookTitle.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) || BookTitle.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)) ? this.BookTitle.Remove(0, this.BookTitle.IndexOf(" ") + 1) : this.BookTitle;
        }
        public Guid? BookSeriesGuid { get; set; }
        public Guid? BookCollectionGuid { get; set; }
        public Guid? BookGenreGuid { get; set; }
        public Guid? BookLocationGuid { get; set; }
        public string? BookImageBase64String { get; set; }
        public string? AuthorListString { get; set; }
        public string? SelectedAuthorString { get; set; }
        public bool IsFavorite { get; set; }
        public int Rating { get; set; }
        public string? BookAuthors { get; set; }
        public string? BookSeries { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetReadingProgress()
        {
            this.Progress = this.BookPageTotal != 0 ? this.BookPageRead / (double)this.BookPageTotal : 0;
            this.PageReadPercent = $"{Math.Round(this.Progress * 100, 2)}%";
        }

        public async Task SetBookCheckpoints()
        {
            this.Half = this.BookPageTotal / 2;
            this.Fourth = this.BookPageTotal / 4;
            this.ThreeFourth = this.Half + this.Fourth;
        }

        public async Task SetDates()
        {
            if (!string.IsNullOrEmpty(this.BookStartDate))
            {
                this.BookStartDate = DateTime.Parse(this.BookStartDate).Date.ToShortDateString();
            }

            if (!string.IsNullOrEmpty(this.BookEndDate))
            {
                this.BookEndDate = DateTime.Parse(this.BookEndDate).Date.ToShortDateString();
            }

            if (!string.IsNullOrEmpty(this.BookLoanedOutOn))
            {
                this.BookLoanedOutOn = DateTime.Parse(this.BookLoanedOutOn).Date.ToShortDateString();
            }
        }

        public async Task SetPartOfSeries()
        {
            this.HasSeries = this.BookSeriesGuid != null;
            var output = string.Empty;

            if (this.BookSeriesGuid != null)
            {
                // Unit test data
                var series = TestData.SeriesList.FirstOrDefault(x => x.SeriesGuid == this.BookSeriesGuid);

                if (this.BookNumberInSeries != null)
                {
                    output = $"{AppStringResources.PartofSeries.Replace("blank", $"{series.SeriesName}")}, {AppStringResources.BookNumber.Replace("Number", $"{this.BookNumberInSeries}")}";
                }

                if (this.BookNumberInSeries != null && !string.IsNullOrEmpty(series.TotalBooksInSeries))
                {
                    output = $"{AppStringResources.PartofSeries.Replace("blank", $"{series.SeriesName}")}, {AppStringResources.BookNumberOfTotal.Replace("number", $"{this.BookNumberInSeries}").Replace("total", $"{series.totalBooksInSeries}")}";
                }
                
                if (this.BookNumberInSeries == null)
                {
                    output = $"{AppStringResources.PartofSeries.Replace("blank", $"{series.SeriesName}")}";
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
                // Unit test data
                var collection = TestData.CollectionList.FirstOrDefault(x => x.CollectionGuid == this.BookCollectionGuid);

                output = $"{AppStringResources.PartOfCollection.Replace("blank", $"{collection.CollectionName}")}";
            }

            this.PartOfCollection = output;
        }

        public async Task SetCoverDisplay()
        {
            this.HasBookCover = this.BookCoverBytes != null;
            this.HasNoBookCover = this.BookCoverBytes == null;
        }

        public async Task SetAuthorListString(ObservableCollection<BookAuthorModel> bookAuthorList, ObservableCollection<AuthorModel> authorList)
        {
            if (bookAuthorList.Any(x => x.BookGuid == this.BookGuid))
            {
                this.AuthorListString = string.Empty;

                for (int i = 0; i < bookAuthorList.Count; i++)
                {
                    var author = authorList.FirstOrDefault(x => x.AuthorGuid == bookAuthorList[i].AuthorGuid);

                    this.AuthorListString += authorList[i].ReverseFullName;

                    if (i != bookAuthorList.Count - 1)
                        this.AuthorListString += "; ";
                }
            }
        }

        public async Task<ObservableCollection<BookAuthorModel>> SetAuthorListString(ObservableCollection<AuthorModel> authorList)
        {
            var bookAuthorList = new ObservableCollection<BookAuthorModel>();

            this.AuthorListString = string.Empty;

            for (int i = 0; i < authorList.Count; i++)
            {
                if (!string.IsNullOrEmpty(authorList[i].FirstName) &&
                    !string.IsNullOrEmpty(authorList[i].LastName))
                {
                    bookAuthorList.Add(new BookAuthorModel()
                    {
                        BookGuid = (Guid)this.BookGuid,
                        AuthorGuid = (Guid)authorList[i].AuthorGuid,
                    });

                    this.AuthorListString += authorList[i].ReverseFullName;

                    if (i != authorList.Count - 1)
                        this.AuthorListString += "; ";
                }
                else
                {
                    this.AuthorListString = this.AuthorListString.Substring(0, this.AuthorListString.LastIndexOf("; ") - 1);
                }
            }

            return bookAuthorList;
        }

        public async Task SetBookChapters(ObservableCollection<ChapterModel> chaptersList)
        {
            foreach (var chapter in chaptersList)
            {
                if (!string.IsNullOrEmpty(chapter.ChapterName))
                {
                    chapter.BookGuid = (Guid)this.BookGuid;

                    // Unit test data
                    TestData.InsertChapter(chapter);
                }
            }
        }

        // TO DO
        // Add ability to change currency type - 11/27/2025
        public async Task SetBookPrice()
        {
            var currency = Preferences.Get("Currency", "$ USD"  /* Default */);

            var cultureInfo = new CultureInfo("en-US");

            if (this.BookPrice == null || !this.BookPrice.Contains(cultureInfo.NumberFormat.CurrencySymbol))
            {
                double.TryParse(this.BookPrice, out double price);

                this.BookPrice = string.Format("{0:C}", price);
            }
        }
    }
}
