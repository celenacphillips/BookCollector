// <copyright file="BookDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.DatabaseModels
{
    /// <summary>
    /// Book Database Model class.
    /// </summary>
    public partial class BookDatabaseModel : ObservableObject
    {
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
        public string? bookCoverFileLocation;

        [PrimaryKey]
        public Guid? BookGuid { get; set; }

        public Guid? BookSeriesGuid { get; set; }

        public Guid? BookCollectionGuid { get; set; }

        public Guid? BookGenreGuid { get; set; }

        public Guid? BookLocationGuid { get; set; }

        public string? AuthorListString { get; set; }

        public bool IsFavorite { get; set; }

        public int Rating { get; set; }

        public string? BookAuthors { get; set; }

        public string? BookSeries { get; set; }
    }
}
