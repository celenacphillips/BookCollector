using CommunityToolkit.Mvvm.ComponentModel;

namespace BookCollector.Data.BookAPI
{
    public class ISBNLookup
    {
        public string? Kind { get; set; }

        public int TotalItems { get; set; }

        public List<Item>? Items { get; set; }
    }

    public class AccessInfo
    {
        public string? Country { get; set; }

        public string? Viewability { get; set; }

        public bool? Embeddable { get; set; }

        public bool? PublicDomain { get; set; }

        public string? TextToSpeechPermission { get; set; }

        public Epub? Epub { get; set; }

        public Pdf? Pdf { get; set; }

        public string? WebReaderLink { get; set; }

        public string? AccessViewStatus { get; set; }

        public bool QuoteSharingAllowed { get; set; }
    }

    public class Epub
    {
        public bool IsAvailable { get; set; }
    }

    public partial class ImageLinks : ObservableObject
    {
        [ObservableProperty]
        public ImageSource? imageSource;

        public string? SmallThumbnail { get; set; }

        public string? Thumbnail { get; set; }

        public string? ImageURL { get; set; }
    }

    public class IndustryIdentifier
    {
        public string? Type { get; set; }

        public string? Identifier { get; set; }
    }

    public partial class Item : ObservableObject
    {
        [ObservableProperty]
        public VolumeInfo? volumeInfo;

        public string? Kind { get; set; }

        public string? Id { get; set; }

        public string? Etag { get; set; }

        public string? SelfLink { get; set; }

        public SaleInfo? SaleInfo { get; set; }

        public AccessInfo? AccessInfo { get; set; }

        public SearchInfo? SearchInfo { get; set; }
    }

    public class PanelizationSummary
    {
        public bool ContainsEpubBubbles { get; set; }

        public bool ContainsImageBubbles { get; set; }
    }

    public class Pdf
    {
        public bool IsAvailable { get; set; }
    }

    public class ReadingModes
    {
        public bool Text { get; set; }

        public bool Image { get; set; }
    }

    public class SaleInfo
    {
        public string? Country { get; set; }

        public string? Saleability { get; set; }

        public bool IsEBook { get; set; }
    }

    public class SearchInfo
    {
        public string? TextSnippet { get; set; }
    }

    public partial class VolumeInfo : ObservableObject
    {
        [ObservableProperty]
        public string? title;

        [ObservableProperty]
        public string? subtitle;

        [ObservableProperty]
        public List<string>? authors;

        [ObservableProperty]
        public string? authorList;

        [ObservableProperty]
        public string? publisher;

        [ObservableProperty]
        public string? publishedDate;

        [ObservableProperty]
        public string? description;

        [ObservableProperty]
        public int pageCount;

        [ObservableProperty]
        public ImageLinks? imageLinks;

        [ObservableProperty]
        public bool hasBookCover;

        [ObservableProperty]
        public bool hasNoBookCover;

        [ObservableProperty]
        public string? language;

        public List<IndustryIdentifier>? IndustryIdentifiers { get; set; }

        public ReadingModes? ReadingModes { get; set; }

        public string? PrintType { get; set; }

        public List<string>? Categories { get; set; }

        public string? MaturityRating { get; set; }

        public bool AllowAnonLogging { get; set; }

        public string? ContentVersion { get; set; }

        public PanelizationSummary? PanelizationSummary { get; set; }

        public string? PreviewLink { get; set; }

        public string? InfoLink { get; set; }

        public string? CanonicalVolumeLink { get; set; }
    }
}