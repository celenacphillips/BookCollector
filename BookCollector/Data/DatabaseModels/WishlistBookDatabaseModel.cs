// <copyright file="WishlistBookDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.DatabaseModels
{
    public partial class WishlistBookDatabaseModel : ObservableObject
    {
        [ObservableProperty]
        public string? bookTitle;
        [ObservableProperty]
        public string? bookNumberInSeries;
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
        public int bookPageTotal;
        [ObservableProperty]
        public int bookHoursTotal;
        [ObservableProperty]
        public int bookMinutesTotal;
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
        public string? bookURL;
        [ObservableProperty]
        public string? bookWhereToBuy;
        [ObservableProperty]
        public bool hideBook;
        [ObservableProperty]
        public string? partOfSeries;
        [ObservableProperty]
        public string? bookCoverFileLocation;

        [PrimaryKey]
        public Guid? BookGuid { get; set; }

        public Guid? BookSeriesGuid { get; set; }

        public string? AuthorListString { get; set; }

        public bool IsFavorite { get; set; }

        public int Rating { get; set; }

        public string? BookAuthors { get; set; }

        public string? BookSeries { get; set; }
    }
}
