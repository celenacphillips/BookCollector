using CommunityToolkit.Mvvm.ComponentModel;

namespace BookCollector.Data.BookAPI
{
    public class ISBNLookup
    {
        public string kind { get; set; }
        public int totalItems { get; set; }
        public List<Item> items { get; set; }
    }

    public class AccessInfo
    {
        public string country { get; set; }
        public string viewability { get; set; }
        public bool embeddable { get; set; }
        public bool publicDomain { get; set; }
        public string textToSpeechPermission { get; set; }
        public Epub epub { get; set; }
        public Pdf pdf { get; set; }
        public string webReaderLink { get; set; }
        public string accessViewStatus { get; set; }
        public bool quoteSharingAllowed { get; set; }
    }

    public class Epub
    {
        public bool isAvailable { get; set; }
    }

    public partial class ImageLinks : ObservableObject
    {
        public string smallThumbnail { get; set; }

        public string thumbnail { get; set; }

        [ObservableProperty]
        public ImageSource imageSource;

        public string? ImageURL { get; set; }
    }

    public class IndustryIdentifier
    {
        public string type { get; set; }
        public string identifier { get; set; }
    }

    public partial class Item : ObservableObject
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string etag { get; set; }
        public string selfLink { get; set; }

        [ObservableProperty]
        public VolumeInfo volumeInfo;
        public SaleInfo saleInfo { get; set; }
        public AccessInfo accessInfo { get; set; }
        public SearchInfo searchInfo { get; set; }
    }

    public class PanelizationSummary
    {
        public bool containsEpubBubbles { get; set; }
        public bool containsImageBubbles { get; set; }
    }

    public class Pdf
    {
        public bool isAvailable { get; set; }
    }

    public class ReadingModes
    {
        public bool text { get; set; }
        public bool image { get; set; }
    }

    public class SaleInfo
    {
        public string country { get; set; }
        public string saleability { get; set; }
        public bool isEBook { get; set; }
    }

    public class SearchInfo
    {
        public string textSnippet { get; set; }
    }

    public partial class VolumeInfo : ObservableObject
    {
        [ObservableProperty]
        public string title;

        [ObservableProperty]
        public string subtitle;

        [ObservableProperty]
        public List<string> authors;

        [ObservableProperty]
        public string authorList;

        [ObservableProperty]
        public string publisher;

        [ObservableProperty]
        public string publishedDate;

        [ObservableProperty]
        public string description;
        public List<IndustryIdentifier> industryIdentifiers { get; set; }
        public ReadingModes readingModes { get; set; }

        [ObservableProperty]
        public int pageCount;
        public string printType { get; set; }
        public List<string> categories { get; set; }
        public string maturityRating { get; set; }
        public bool allowAnonLogging { get; set; }
        public string contentVersion { get; set; }
        public PanelizationSummary panelizationSummary { get; set; }

        [ObservableProperty]
        public ImageLinks imageLinks;

        [ObservableProperty]
        public bool hasBookCover;

        [ObservableProperty]
        public bool hasNoBookCover;

        [ObservableProperty]
        public string language;
        public string previewLink { get; set; }
        public string infoLink { get; set; }
        public string canonicalVolumeLink { get; set; }
    }
}