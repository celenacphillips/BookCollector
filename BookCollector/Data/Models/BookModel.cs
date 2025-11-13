using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.Models
{
    public partial class BookModel : ObservableObject
    {
        //TO DO:
        // Add Loaned To and Loaned out to - 11/13/2025

        [PrimaryKey]
        public Guid? BookGuid { get; set; }

        [ObservableProperty]
        public string bookTitle;
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
        public double? bookPriceValue;
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
        public DateTime? startDateValue;
        [ObservableProperty]
        public string? bookEndDate;
        [ObservableProperty]
        public DateTime? endDateValue;
        [ObservableProperty]
        public string? bookLocation;
        [ObservableProperty]
        public string? bookComments;
        [ObservableProperty]
        public byte[]? bookCoverBytes;
        [ObservableProperty]
        public bool hasBookCover;
        [ObservableProperty]
        public bool hasNoBookCover;
        [ObservableProperty]
        public string publisherPublishDateString;
        [ObservableProperty]
        public bool hasSeries;
        [ObservableProperty]
        public bool hasCollection;
        [ObservableProperty]
        public string? bookURL;
        [ObservableProperty]
        public string? bookWhereToBuy;

        public string? ParsedTitle { get; set; }
        public Guid? BookSeriesGuid { get; set; }
        public Guid? BookCollectionGuid { get; set; }
        public Guid? BookGenreGuid { get; set; }
        public string? BookImageBase64String { get; set; }
        public string? AuthorListString { get; set; }
        public string? SelectedAuthorString { get; set; }
        public bool IsFavorite { get; set; }
        public int Rating { get; set; }
        public bool HideBook { get; set; }
        public string? BookAuthors { get; set; }
        public string? BookSeries { get; set; }
    }
}
