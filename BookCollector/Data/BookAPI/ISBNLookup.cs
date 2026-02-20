// <copyright file="ISBNLookup.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.BookAPI
{
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// ISBN Lookup class for Google Books API.
    /// </summary>
    public class ISBNLookup
    {
        /// <summary>
        /// Gets or sets the kind of the response.
        /// </summary>
        public string kind { get; set; }

        /// <summary>
        /// Gets or sets the total items of the response.
        /// </summary>
        public int totalItems { get; set; }

        /// <summary>
        /// Gets or sets the items of the response.
        /// </summary>
        public List<Item> items { get; set; }
    }

    /// <summary>
    /// Access Info class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public class AccessInfo
    {
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Gets or sets the viewability.
        /// </summary>
        public string viewability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the volume is embeddable.
        /// </summary>
        public bool embeddable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the volume is in the public domain.
        /// </summary>
        public bool publicDomain { get; set; }

        /// <summary>
        /// Gets or sets the text to speech permission.
        /// </summary>
        public string textToSpeechPermission { get; set; }

        /// <summary>
        /// Gets or sets the epub value.
        /// </summary>
        public Epub epub { get; set; }

        /// <summary>
        /// Gets or sets the pdf value.
        /// </summary>
        public Pdf pdf { get; set; }

        /// <summary>
        /// Gets or sets the web reader link.
        /// </summary>
        public string webReaderLink { get; set; }

        /// <summary>
        /// Gets or sets the access view status.
        /// </summary>
        public string accessViewStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the volume has quote sharing allowed.
        /// </summary>
        public bool quoteSharingAllowed { get; set; }
    }

    /// <summary>
    /// Epub class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public class Epub
    {
        /// <summary>
        /// Gets or sets a value indicating whether the epub is available.
        /// </summary>
        public bool isAvailable { get; set; }
    }

    /// <summary>
    /// Image Links class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public partial class ImageLinks : ObservableObject
    {
        /// <summary>
        /// Gets or sets the image source value.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ImageSource imageSource;

        /// <summary>
        /// Gets or sets the small thumbnail.
        /// </summary>
        public string smallThumbnail { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail.
        /// </summary>
        public string thumbnail { get; set; }
    }

    /// <summary>
    /// Industry Identifier class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public class IndustryIdentifier
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string identifier { get; set; }
    }

    /// <summary>
    /// Item class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public partial class Item : ObservableObject
    {
        /// <summary>
        /// Gets or sets the volume info value.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public VolumeInfo volumeInfo;

        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        public string kind { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the etag.
        /// </summary>
        public string etag { get; set; }

        /// <summary>
        /// Gets or sets the self link.
        /// </summary>
        public string selfLink { get; set; }

        /// <summary>
        /// Gets or sets the sale info value.
        /// </summary>
        public SaleInfo saleInfo { get; set; }

        /// <summary>
        /// Gets or sets the access info value.
        /// </summary>
        public AccessInfo accessInfo { get; set; }

        /// <summary>
        /// Gets or sets the search info value.
        /// </summary>
        public SearchInfo searchInfo { get; set; }
    }

    /// <summary>
    /// Panelization Summary class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public class PanelizationSummary
    {
        /// <summary>
        /// Gets or sets a value indicating whether the panelization summary contains epub bubbles.
        /// </summary>
        public bool containsEpubBubbles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the panelization summary contains image bubbles.
        /// </summary>
        public bool containsImageBubbles { get; set; }
    }

    /// <summary>
    /// PDF class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public class Pdf
    {
        /// <summary>
        /// Gets or sets a value indicating whether the pdf is available.
        /// </summary>
        public bool isAvailable { get; set; }
    }

    /// <summary>
    /// Reading Modes class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public class ReadingModes
    {
        /// <summary>
        /// Gets or sets a value indicating whether the reading mode is a text.
        /// </summary>
        public bool text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the reading mode is an image.
        /// </summary>
        public bool image { get; set; }
    }

    /// <summary>
    /// Sale Info class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public class SaleInfo
    {
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Gets or sets the saleability.
        /// </summary>
        public string saleability { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the volume is an ebook.
        /// </summary>
        public bool isEBook { get; set; }
    }

    /// <summary>
    /// Search Info class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public class SearchInfo
    {
        /// <summary>
        /// Gets or sets the text snippet.
        /// </summary>
        public string textSnippet { get; set; }
    }

    /// <summary>
    /// Volume Info class for Google Books API.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Request JSON output.")]
    public partial class VolumeInfo : ObservableObject
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string title;

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string subtitle;

        /// <summary>
        /// Gets or sets the list of authors.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string> authors;

        /// <summary>
        /// Gets or sets the author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string authorList;

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string publisher;

        /// <summary>
        /// Gets or sets the published date.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string publishedDate;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string description;

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int pageCount;

        /// <summary>
        /// Gets or sets the image links value.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ImageLinks imageLinks;

        /// <summary>
        /// Gets or sets a value indicating whether the volume has a book cover.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool hasBookCover;

        /// <summary>
        /// Gets or sets a value indicating whether the volume has no book cover.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool hasNoBookCover;

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string language;

        /// <summary>
        /// Gets or sets the list of industry identifers.
        /// </summary>
        public List<IndustryIdentifier> industryIdentifiers { get; set; }

        /// <summary>
        /// Gets or sets the reading modes value.
        /// </summary>
        public ReadingModes readingModes { get; set; }

        /// <summary>
        /// Gets or sets the print type.
        /// </summary>
        public string printType { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        public List<string> categories { get; set; }

        /// <summary>
        /// Gets or sets the maturity rating.
        /// </summary>
        public string maturityRating { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the volume allows anonymous logging.
        /// </summary>
        public bool allowAnonLogging { get; set; }

        /// <summary>
        /// Gets or sets the content version.
        /// </summary>
        public string contentVersion { get; set; }

        /// <summary>
        /// Gets or sets the panelization summary value.
        /// </summary>
        public PanelizationSummary panelizationSummary { get; set; }

        /// <summary>
        /// Gets or sets the preview link.
        /// </summary>
        public string previewLink { get; set; }

        /// <summary>
        /// Gets or sets the info link.
        /// </summary>
        public string infoLink { get; set; }

        /// <summary>
        /// Gets or sets the canonical volume link.
        /// </summary>
        public string canonicalVolumeLink { get; set; }
    }
}