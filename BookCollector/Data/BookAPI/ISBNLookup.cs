// <copyright file="ISBNLookup.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;

namespace BookCollector.Data.BookAPI
{
    /// <summary>
    /// ISBN Lookup class for Google Books API.
    /// </summary>
    public class ISBNLookup
    {
        public string kind { get; set; }

        public int totalItems { get; set; }

        public List<Item> items { get; set; }
    }

    /// <summary>
    /// Access Info class for Google Books API.
    /// </summary>
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

    /// <summary>
    /// Epub class for Google Books API.
    /// </summary>
    public class Epub
    {
        public bool isAvailable { get; set; }
    }

    /// <summary>
    /// Image Links class for Google Books API.
    /// </summary>
    public partial class ImageLinks : ObservableObject
    {
        [ObservableProperty]
        public ImageSource imageSource;

        public string smallThumbnail { get; set; }

        public string thumbnail { get; set; }

    }

    /// <summary>
    /// Industry Identifier class for Google Books API.
    /// </summary>
    public class IndustryIdentifier
    {
        public string type { get; set; }

        public string identifier { get; set; }
    }

    /// <summary>
    /// Item class for Google Books API.
    /// </summary>
    public partial class Item : ObservableObject
    {
        [ObservableProperty]
        public VolumeInfo volumeInfo;

        public string kind { get; set; }

        public string id { get; set; }

        public string etag { get; set; }

        public string selfLink { get; set; }

        public SaleInfo saleInfo { get; set; }

        public AccessInfo accessInfo { get; set; }

        public SearchInfo searchInfo { get; set; }
    }

    /// <summary>
    /// Panelization Summary class for Google Books API.
    /// </summary>
    public class PanelizationSummary
    {
        public bool containsEpubBubbles { get; set; }

        public bool containsImageBubbles { get; set; }
    }

    /// <summary>
    /// PDF class for Google Books API.
    /// </summary>
    public class Pdf
    {
        public bool isAvailable { get; set; }
    }

    /// <summary>
    /// Reading Modes class for Google Books API.
    /// </summary>
    public class ReadingModes
    {
        public bool text { get; set; }

        public bool image { get; set; }
    }

    /// <summary>
    /// Sale Info class for Google Books API.
    /// </summary>
    public class SaleInfo
    {
        public string country { get; set; }

        public string saleability { get; set; }

        public bool isEBook { get; set; }
    }

    /// <summary>
    /// Search Info class for Google Books API.
    /// </summary>
    public class SearchInfo
    {
        public string textSnippet { get; set; }
    }

    /// <summary>
    /// Volume Info class for Google Books API.
    /// </summary>
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

        [ObservableProperty]
        public int pageCount;

        [ObservableProperty]
        public ImageLinks imageLinks;

        [ObservableProperty]
        public bool hasBookCover;

        [ObservableProperty]
        public bool hasNoBookCover;

        [ObservableProperty]
        public string language;

        public List<IndustryIdentifier> industryIdentifiers { get; set; }

        public ReadingModes readingModes { get; set; }

        public string printType { get; set; }

        public List<string> categories { get; set; }

        public string maturityRating { get; set; }

        public bool allowAnonLogging { get; set; }

        public string contentVersion { get; set; }

        public PanelizationSummary panelizationSummary { get; set; }

        public string previewLink { get; set; }

        public string infoLink { get; set; }

        public string canonicalVolumeLink { get; set; }
    }
}