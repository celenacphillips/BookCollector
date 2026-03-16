// <copyright file="BookBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.Author;
    using BookCollector.ViewModels.Collection;
    using BookCollector.ViewModels.Genre;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Library;
    using BookCollector.ViewModels.Location;
    using BookCollector.ViewModels.Popups;
    using BookCollector.ViewModels.Series;
    using BookCollector.Views.Book;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// BookBaseViewModel class.
    /// </summary>
    public abstract partial class BookBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<BookModel>? filteredBookList;

        /// <summary>
        /// Gets or sets the book format list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<string>? bookFormats;

        /// <summary>
        /// Gets or sets the selected book.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public BookModel? selectedBook;

        /// <summary>
        /// Gets or sets a value indicating whether a book is read or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookIsRead;

        /// <summary>
        /// Gets or sets a value indicating whether to show book is up next.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showUpNext;

        /// <summary>
        /// Gets or sets a value indicating whether the reading section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool readingDataSectionValue;

        /// <summary>
        /// Gets or sets a value indicating whether the reading section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool readingDataOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the reading section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool readingDataNotOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the chapter section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool chapterListSectionValue;

        /// <summary>
        /// Gets or sets a value indicating whether the chapter section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool chapterListOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the chapter section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool chapterListNotOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the author section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorListSectionValue;

        /// <summary>
        /// Gets or sets a value indicating whether the author section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorListOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the author section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorListNotOpen;

        /// <summary>
        /// Gets or sets a value indicating whether to show comments or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showComments;

        /// <summary>
        /// Gets or sets a value indicating whether to show chapters or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showChapters;

        /// <summary>
        /// Gets or sets a value indicating whether to show favorites or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showFavorites;

        /// <summary>
        /// Gets or sets a value indicating whether to show ratings or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showRatings;

        /// <summary>
        /// Gets or sets the chapter list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<ChapterModel>? chapterList;

        /// <summary>
        /// Gets or sets a value indicating whether the book info section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfoSectionValue;

        /// <summary>
        /// Gets or sets a value indicating whether the book info section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfoOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the book info section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfoNotOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the summary section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool summarySectionValue;

        /// <summary>
        /// Gets or sets a value indicating whether the summary section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool summaryOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the summary section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool summaryNotOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the comments section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool commentsSectionValue;

        /// <summary>
        /// Gets or sets a value indicating whether the comments section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool commentsOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the comments section expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool commentsNotOpen;

        /// <summary>
        /// Gets or sets the selected genre of the book.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public GenreModel? selectedGenre;

        /// <summary>
        /// Gets or sets the selected location of the book.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public LocationModel? selectedLocation;

        /// <summary>
        /// Gets or sets the book cover image source.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ImageSource? bookCover;

        /// <summary>
        /// Gets or sets the book publisher list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<string>? bookPublisherList;

        /// <summary>
        /// Gets or sets the book publish year range list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<string>? bookPublishYearList;

        /// <summary>
        /// Gets or sets the book language list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<string>? bookLanguageList;

        /// <summary>
        /// Gets or sets a value indicating whether to show pages or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showPages;

        /// <summary>
        /// Gets or sets a value indicating whether to show time or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookBaseViewModel"/> class.
        /// </summary>
        public BookBaseViewModel()
        {
            this.ShowComments = Preferences.Get("CommentsOn", true /* Default */);
            this.ShowChapters = Preferences.Get("ChaptersOn", true /* Default */);
            this.ShowFavorites = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowRatings = Preferences.Get("RatingsOn", true /* Default */);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden books or not.
        /// </summary>
        public static bool ShowHiddenBooks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden authors or not.
        /// </summary>
        public bool HiddenAuthorsOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show favorite books or not.
        /// </summary>
        public bool ShowFavoriteBooks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show book ratings or not.
        /// </summary>
        public bool ShowBookRatings { get; set; }

        /********************************************************/

        /// <summary>
        /// Gets or sets the favorite books option.
        /// </summary>
        public string? FavoriteBooksOption { get; set; }

        /// <summary>
        /// Gets or sets the book format option.
        /// </summary>
        public string? BookFormatOption { get; set; }

        /// <summary>
        /// Gets or sets the book publisher option.
        /// </summary>
        public string? BookPublisherOption { get; set; }

        /// <summary>
        /// Gets or sets the book publisher year range option.
        /// </summary>
        public string? BookPublishYearOption { get; set; }

        /// <summary>
        /// Gets or sets the book language option.
        /// </summary>
        public string? BookLanguageOption { get; set; }

        /// <summary>
        /// Gets or sets the book rating option.
        /// </summary>
        public string? BookRatingOption { get; set; }

        /// <summary>
        /// Gets or sets the book cover option.
        /// </summary>
        public string? BookCoverOption { get; set; }

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether the book title option is checked or not.
        /// </summary>
        public bool BookTitleChecked { get; set; }

        /// <summary>
        /// Gets or sets the saved book author filter option.
        /// </summary>
        public string? BookAuthorOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the book reading date option is checked or not.
        /// </summary>
        public bool BookReadingDateChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the book read percentage option is checked or not.
        /// </summary>
        public bool BookReadPercentageChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the book publisher option is checked or not.
        /// </summary>
        public bool BookPublisherChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the book publish year option is checked or not.
        /// </summary>
        public bool BookPublishYearChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the author last name option is checked or not.
        /// </summary>
        public bool AuthorLastNameChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the book format option is checked or not.
        /// </summary>
        public bool BookFormatChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page count/book time option is checked or not.
        /// </summary>
        public bool PageCountBookTimeChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the book price option is checked or not.
        /// </summary>
        public bool BookPriceChecked { get; set; }

        /********************************************************/

        /// <summary>
        /// Parse out authors from string.
        /// </summary>
        /// <param name="inputstring">Input string to parse.</param>
        /// <returns>A list of authors parsed out.</returns>
        public static async Task<ObservableCollection<AuthorModel>> ParseOutAuthorsFromstring(string? inputstring)
        {
            var authorList = new ObservableCollection<AuthorModel>();

            if (!string.IsNullOrEmpty(inputstring))
            {
                var list = SplitStringIntoAuthorList(inputstring);

                foreach (var item in list)
                {
                    if (item != null)
                    {
                        AuthorModel? author = null;
                        bool skip = false;

                        author = await BaseViewModel.Database.GetAuthorByNameAsync(item.FirstName, item.LastName);

                        if (!skip)
                        {
                            author ??= new AuthorModel();

                            author.FirstName = item.FirstName;
                            author.LastName = item.LastName;

                            authorList.Add(author);
                        }
                    }
                }
            }

            return authorList;
        }

        /// <summary>
        /// Add book to static lists.
        /// </summary>
        /// <param name="book">Book to add.</param>
        /// <param name="previousViewModel">Previous model to return to.</param>
        /// <returns>A task.</returns>
        public static async Task AddToStaticList(BookModel book, object? previousViewModel = null)
        {
            if (AllBooksViewModel.fullBookList != null)
            {
                AllBooksViewModel.RefreshView = await AddBookToStaticList(book, AllBooksViewModel.fullBookList, AllBooksViewModel.filteredBookList);
            }

            if ((((book.BookPageRead != book.BookPageTotal && book.BookPageRead != 0) ||
                book.UpNext) ||
                (book.BookHourListened != book.BookHoursTotal && book.BookMinuteListened != book.BookMinutesTotal && book.BookHourListened != 0 && book.BookMinuteListened != 0)) &&
                ReadingViewModel.fullBookList != null)
            {
                ReadingViewModel.RefreshView = await AddBookToStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList);
                ToBeReadViewModel.RefreshView = RemoveBookFromStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList);
                ReadViewModel.RefreshView = RemoveBookFromStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList);
            }

            if (book.BookPageRead == 0 &&
                !book.UpNext &&
                (book.BookHourListened == 0 && book.BookMinuteListened == 0) &&
                ToBeReadViewModel.fullBookList != null)
            {
                ToBeReadViewModel.RefreshView = await AddBookToStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList);
                ReadingViewModel.RefreshView = RemoveBookFromStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList);
                ReadViewModel.RefreshView = RemoveBookFromStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList);
            }

            if (((book.BookPageRead == book.BookPageTotal && book.BookPageRead != 0) ||
                (book.BookHourListened == book.BookHoursTotal && book.BookMinuteListened == book.BookMinutesTotal && book.BookHourListened != 0 && book.BookMinuteListened != 0)) &&
                ReadViewModel.fullBookList != null)
            {
                ReadViewModel.RefreshView = await AddBookToStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList);
                ToBeReadViewModel.RefreshView = RemoveBookFromStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList);
                ReadingViewModel.RefreshView = RemoveBookFromStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList);
            }

            if (CollectionsViewModel.fullCollectionList != null)
            {
                if (book.BookCollectionGuid != null)
                {
                    var selected = CollectionsViewModel.fullCollectionList.FirstOrDefault(x => x.CollectionGuid == book.BookCollectionGuid);

                    if (selected != null)
                    {
                        CollectionMainViewModel collectionViewModel;

                        if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Collection"))
                        {
                            collectionViewModel = (CollectionMainViewModel)previousViewModel;

                            if (collectionViewModel.SelectedCollection != selected)
                            {
                                CollectionMainViewModel.RefreshView = RemoveBookFromStaticList(book, collectionViewModel.FullBookList, collectionViewModel.FilteredBookList);
                                collectionViewModel = new CollectionMainViewModel(selected, new ContentPage());
                            }
                        }
                        else
                        {
                            collectionViewModel = new CollectionMainViewModel(selected, new ContentPage());
                        }

                        if (collectionViewModel.FullBookList != null)
                        {
                            CollectionMainViewModel.RefreshView = await AddBookToStaticList(book, collectionViewModel.FullBookList, collectionViewModel.FilteredBookList);
                        }
                    }
                }

                if (book.BookCollectionGuid == null && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Collection"))
                {
                    var collectionViewModel = (CollectionMainViewModel)previousViewModel;
                    CollectionMainViewModel.RefreshView = RemoveBookFromStaticList(book, collectionViewModel.FullBookList, collectionViewModel.FilteredBookList);

                    var existingBooksViewModel = new ExistingBooksViewModel(new CollectionModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        ExistingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList);
                    }
                }
            }

            if (GenresViewModel.fullGenreList != null)
            {
                if (book.BookGenreGuid != null)
                {
                    var selected = GenresViewModel.fullGenreList.FirstOrDefault(x => x.GenreGuid == book.BookGenreGuid);

                    if (selected != null)
                    {
                        GenreMainViewModel genreViewModel;

                        if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Genre"))
                        {
                            genreViewModel = (GenreMainViewModel)previousViewModel;

                            if (genreViewModel.SelectedGenre != selected)
                            {
                                GenreMainViewModel.RefreshView = RemoveBookFromStaticList(book, genreViewModel.FullBookList, genreViewModel.FilteredBookList);
                                genreViewModel = new GenreMainViewModel(selected, new ContentPage());
                            }
                        }
                        else
                        {
                            genreViewModel = new GenreMainViewModel(selected, new ContentPage());
                        }

                        if (genreViewModel.FullBookList != null)
                        {
                            GenreMainViewModel.RefreshView = await AddBookToStaticList(book, genreViewModel.FullBookList, genreViewModel.FilteredBookList);
                        }
                    }
                }

                if (book.BookGenreGuid == null && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Genre"))
                {
                    var genreViewModel = (GenreMainViewModel)previousViewModel;
                    GenreMainViewModel.RefreshView = RemoveBookFromStaticList(book, genreViewModel.FullBookList, genreViewModel.FilteredBookList);

                    var existingBooksViewModel = new ExistingBooksViewModel(new GenreModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        ExistingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList);
                    }
                }
            }

            if (SeriesViewModel.fullSeriesList != null)
            {
                if (book.BookSeriesGuid != null)
                {
                    var selected = SeriesViewModel.fullSeriesList.FirstOrDefault(x => x.SeriesGuid == book.BookSeriesGuid);

                    if (selected != null)
                    {
                        SeriesMainViewModel seriesViewModel;

                        if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Series"))
                        {
                            seriesViewModel = (SeriesMainViewModel)previousViewModel;

                            if (seriesViewModel.SelectedSeries != selected)
                            {
                                SeriesMainViewModel.RefreshView = RemoveBookFromStaticList(book, seriesViewModel.FullBookList, seriesViewModel.FilteredBookList);
                                seriesViewModel = new SeriesMainViewModel(selected, new ContentPage());
                            }
                        }
                        else
                        {
                            seriesViewModel = new SeriesMainViewModel(selected, new ContentPage());
                        }

                        if (seriesViewModel.FullBookList != null)
                        {
                            SeriesMainViewModel.RefreshView = await AddBookToStaticList(book, seriesViewModel.FullBookList, seriesViewModel.FilteredBookList);
                        }
                    }
                }

                if (book.BookSeriesGuid == null && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Series"))
                {
                    var seriesViewModel = (SeriesMainViewModel)previousViewModel;
                    SeriesMainViewModel.RefreshView = RemoveBookFromStaticList(book, seriesViewModel.FullBookList, seriesViewModel.FilteredBookList);

                    var existingBooksViewModel = new ExistingBooksViewModel(new SeriesModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        ExistingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList);
                    }
                }
            }

            if (AuthorsViewModel.fullAuthorList != null)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var authors = SplitStringIntoAuthorList(book.AuthorListString);

                    foreach (var author in authors)
                    {
                        var selected = AuthorsViewModel.fullAuthorList.FirstOrDefault(x => x.FirstName.Equals(author.FirstName) && x.LastName.Equals(author.LastName));

                        if (selected != null)
                        {
                            AuthorMainViewModel authorViewModel;

                            if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Author"))
                            {
                                authorViewModel = (AuthorMainViewModel)previousViewModel;

                                if (authorViewModel.SelectedAuthor != selected)
                                {
                                    AuthorMainViewModel.RefreshView = RemoveBookFromStaticList(book, authorViewModel.FullBookList, authorViewModel.FilteredBookList);
                                    authorViewModel = new AuthorMainViewModel(selected, new ContentPage());
                                }
                            }
                            else
                            {
                                authorViewModel = new AuthorMainViewModel(selected, new ContentPage());
                            }

                            if (authorViewModel.FullBookList != null)
                            {
                                AuthorMainViewModel.RefreshView = await AddBookToStaticList(book, authorViewModel.FullBookList, authorViewModel.FilteredBookList);
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(book.AuthorListString) && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Author"))
                {
                    var authorViewModel = (AuthorMainViewModel)previousViewModel;
                    AuthorMainViewModel.RefreshView = RemoveBookFromStaticList(book, authorViewModel.FullBookList, authorViewModel.FilteredBookList);

                    var existingBooksViewModel = new ExistingBooksViewModel(new AuthorModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        ExistingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList);
                    }
                }
            }

            if (LocationsViewModel.fullLocationList != null)
            {
                if (book.BookLocationGuid != null)
                {
                    var selected = LocationsViewModel.fullLocationList.FirstOrDefault(x => x.LocationGuid == book.BookLocationGuid);

                    if (selected != null)
                    {
                        LocationMainViewModel locationViewModel;

                        if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Location"))
                        {
                            locationViewModel = (LocationMainViewModel)previousViewModel;

                            if (locationViewModel.SelectedLocation != selected)
                            {
                                LocationMainViewModel.RefreshView = RemoveBookFromStaticList(book, locationViewModel.FullBookList, locationViewModel.FilteredBookList);
                                locationViewModel = new LocationMainViewModel(selected, new ContentPage());
                            }
                        }
                        else
                        {
                            locationViewModel = new LocationMainViewModel(selected, new ContentPage());
                        }

                        if (locationViewModel.FullBookList != null)
                        {
                            LocationMainViewModel.RefreshView = await AddBookToStaticList(book, locationViewModel.FullBookList, locationViewModel.FilteredBookList);
                        }
                    }
                }

                if (book.BookLocationGuid == null && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Location"))
                {
                    var locationViewModel = (LocationMainViewModel)previousViewModel;
                    LocationMainViewModel.RefreshView = RemoveBookFromStaticList(book, locationViewModel.FullBookList, locationViewModel.FilteredBookList);

                    var existingBooksViewModel = new ExistingBooksViewModel(new LocationModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        ExistingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList);
                    }
                }
            }

            CollectionsViewModel.RefreshView = true;
            GenresViewModel.RefreshView = true;
            SeriesViewModel.RefreshView = true;
            AuthorsViewModel.RefreshView = true;
            LocationsViewModel.RefreshView = true;
        }

        /// <summary>
        /// Remove book from static lists.
        /// </summary>
        /// <param name="book">Book to remove.</param>
        /// <returns>A task.</returns>
        public static async Task RemoveFromStaticList(BookModel book)
        {
            if (AllBooksViewModel.fullBookList != null)
            {
                AllBooksViewModel.RefreshView = RemoveBookFromStaticList(book, AllBooksViewModel.fullBookList, AllBooksViewModel.filteredBookList);
            }

            if (((book.BookPageRead != book.BookPageTotal && book.BookPageRead != 0) ||
                book.UpNext) ||
                ((book.BookHourListened != book.BookHoursTotal && book.BookMinuteListened != book.BookMinutesTotal) &&
                ReadingViewModel.fullBookList != null))
            {
                ReadingViewModel.RefreshView = RemoveBookFromStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList);
            }

            if (book.BookPageRead == 0 &&
                !book.UpNext &&
                (book.BookHourListened == 0 && book.BookMinuteListened == 0) &&
                ToBeReadViewModel.fullBookList != null)
            {
                ToBeReadViewModel.RefreshView = RemoveBookFromStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList);
            }

            if ((book.BookPageRead == book.BookPageTotal && book.BookPageRead != 0) ||
                ((book.BookHourListened == book.BookHoursTotal && book.BookMinuteListened == book.BookMinutesTotal && book.BookHourListened != 0 && book.BookMinuteListened != 0) &&
                ReadViewModel.fullBookList != null))
            {
                ReadViewModel.RefreshView = RemoveBookFromStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList);
            }

            if (CollectionsViewModel.fullCollectionList != null)
            {
                if (book.BookCollectionGuid != null)
                {
                    var selected = CollectionsViewModel.fullCollectionList.FirstOrDefault(x => x.CollectionGuid == book.BookCollectionGuid);

                    if (selected != null)
                    {
                        await Task.WhenAll(
                        [
                            selected.SetTotalBooks(true),
                            selected.SetTotalCostOfBooks(true),
                        ]);
                    }
                }
            }

            if (GenresViewModel.fullGenreList != null)
            {
                if (book.BookGenreGuid != null)
                {
                    var selected = GenresViewModel.fullGenreList.FirstOrDefault(x => x.GenreGuid == book.BookGenreGuid);

                    if (selected != null)
                    {
                        await Task.WhenAll(
                        [
                            selected.SetTotalBooks(true),
                            selected.SetTotalCostOfBooks(true),
                        ]);
                    }
                }
            }

            if (SeriesViewModel.fullSeriesList != null)
            {
                if (book.BookSeriesGuid != null)
                {
                    var selected = SeriesViewModel.fullSeriesList.FirstOrDefault(x => x.SeriesGuid == book.BookSeriesGuid);

                    if (selected != null)
                    {
                        await Task.WhenAll(
                        [
                            selected.SetTotalBooks(true),
                            selected.SetTotalCostOfBooks(true),
                        ]);
                    }
                }
            }

            if (AuthorsViewModel.fullAuthorList != null)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var authors = SplitStringIntoAuthorList(book.AuthorListString);

                    foreach (var author in authors)
                    {
                        var selected = AuthorsViewModel.fullAuthorList.FirstOrDefault(x => x.FirstName.Equals(author.FirstName) && x.LastName.Equals(author.LastName));

                        if (selected != null)
                        {
                            await Task.WhenAll(
                            [
                                selected.SetTotalBooks(true),
                                selected.SetTotalCostOfBooks(true),
                            ]);
                        }
                    }
                }
            }

            if (LocationsViewModel.fullLocationList != null)
            {
                if (book.BookLocationGuid != null)
                {
                    var selected = LocationsViewModel.fullLocationList.FirstOrDefault(x => x.LocationGuid == book.BookLocationGuid);

                    if (selected != null)
                    {
                        await Task.WhenAll(
                        [
                            selected.SetTotalBooks(true),
                            selected.SetTotalCostOfBooks(true),
                        ]);
                    }
                }
            }
        }

        /// <summary>
        /// Remove book from lists.
        /// </summary>
        /// <param name="book">Book to remove.</param>
        /// <param name="bookList">Main book list to remove book from.</param>
        /// <param name="filteredBookList">Filtered book list to remove book from.</param>
        /// <returns>Refresh view value.</returns>
        public static bool RemoveBookFromStaticList(BookModel book, ObservableCollection<BookModel>? bookList, ObservableCollection<BookModel>? filteredBookList)
        {
            var refresh = false;

            try
            {
                if (bookList != null)
                {
                    var oldBook = bookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                    if (oldBook != null)
                    {
                        bookList.Remove(oldBook);
                        refresh = true;
                    }
                }

                if (filteredBookList != null)
                {
                    var filteredBook = filteredBookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                    if (filteredBook != null)
                    {
                        filteredBookList.Remove(filteredBook);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public override bool GetPreferences()
        {
            return true;
        }

        /// <summary>
        /// Set the view model list.
        /// </summary>
        /// <param name="showHidden">The show hidden list preference.</param>
        /// <returns>A task.</returns>
        public async override Task SetList(bool showHidden)
        {
        }

        /// <summary>
        /// Check if the list is null.
        /// </summary>
        /// <returns>If the list is null.</returns>
        public override bool ListNullCheck()
        {
            return false;
        }

        /// <summary>
        /// Iterate through the list and set necessary data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetListData()
        {
        }

        /// <summary>
        /// Find filters for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetFilters()
        {
        }

        /// <summary>
        /// Find sort values for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetSorts()
        {
        }

        /// <summary>
        /// Set data for view.
        /// </summary>
        public async override void SetViewStrings()
        {
        }

        /// <summary>
        /// Show filter popup.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task FilterPopup()
        {
            if (!string.IsNullOrEmpty(this.ViewTitle))
            {
                var popup = new FilterPopup();
                var viewModel = new FilterPopupViewModel(popup, this.ViewTitle, this.View);
                viewModel = this.SetFilterPopupValues(viewModel);
                viewModel = this.SetFilterPopupLists(viewModel);

                popup.BindingContext = viewModel;

                var result = await this.View.ShowPopupAsync(popup);
                if (!result.WasDismissedByTappingOutsideOfPopup)
                {
                    RefreshView = true;
                    await this.SetViewModelData();
                }
            }
        }

        /// <summary>
        /// Set data for filter popup.
        /// </summary>
        /// <param name="viewModel">Filter popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public abstract FilterPopupViewModel SetFilterPopupValues(FilterPopupViewModel viewModel);

        /// <summary>
        /// Set data for filter popup.
        /// </summary>
        /// <param name="viewModel">Filter popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public abstract FilterPopupViewModel SetFilterPopupLists(FilterPopupViewModel viewModel);

        /// <summary>
        /// Show sort popup.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SortPopup()
        {
            if (!string.IsNullOrEmpty(this.ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, this.ViewTitle);
                viewModel = this.SetSortPopupValues(viewModel);

                popup.BindingContext = viewModel;

                var result = await this.View.ShowPopupAsync(popup);
                if (!result.WasDismissedByTappingOutsideOfPopup)
                {
                    RefreshView = true;
                    await this.SetViewModelData();
                }
            }
        }

        /// <summary>
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public abstract SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel);

        /// <summary>
        /// Navigate to the book main view when book is selected.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookSelectionChanged()
        {
            if (this.SelectedBook != null && !string.IsNullOrEmpty(this.SelectedBook.BookTitle))
            {
                var view = new BookMainView(this.SelectedBook, this.SelectedBook.BookTitle, this);
                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedBook = null;
            }
        }

        /// <summary>
        /// Create a new book and navigate to the book edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddBook()
        {
            this.SetIsBusyTrue();

            var view = new BookEditView(new BookModel(), $"{AppStringResources.AddNewBook}");

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ReadingDataChanged()
        {
            this.ReadingDataOpen = this.ReadingDataSectionValue;
            this.ReadingDataNotOpen = !this.ReadingDataSectionValue;
        }

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ChapterListChanged()
        {
            this.ChapterListOpen = this.ChapterListSectionValue;
            this.ChapterListNotOpen = !this.ChapterListSectionValue;
        }

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AuthorListChanged()
        {
            this.AuthorListOpen = this.AuthorListSectionValue;
            this.AuthorListNotOpen = !this.AuthorListSectionValue;
        }

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookInfoChanged()
        {
            this.BookInfoOpen = this.BookInfoSectionValue;
            this.BookInfoNotOpen = !this.BookInfoSectionValue;
        }

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SummaryChanged()
        {
            this.SummaryOpen = this.SummarySectionValue;
            this.SummaryNotOpen = !this.SummarySectionValue;
        }

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task CommentsChanged()
        {
            this.CommentsOpen = this.CommentsSectionValue;
            this.CommentsNotOpen = !this.CommentsSectionValue;
        }

        /// <summary>
        /// Show book cover popup.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookCoverPopup()
        {
            this.View.ShowPopup(new BookCoverPopup(this.BookCover!));
        }

        private static async Task<bool> AddBookToStaticList(BookModel book, ObservableCollection<BookModel> bookList, ObservableCollection<BookModel>? filteredBookList)
        {
            var refresh = false;

            await Task.WhenAll(
            [
                book.SetReadingProgress(),

                // book.SetAuthorListString(),
                book.SetCoverDisplay(),
            ]);

            try
            {
                var oldBook = bookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                if (oldBook != null)
                {
                    var index = bookList.IndexOf(oldBook);
                    bookList.Remove(oldBook);
                    bookList.Insert(index, book);
                    refresh = true;
                }
                else
                {
                    bookList.Add(book);
                    refresh = true;
                }

                if (filteredBookList != null)
                {
                    var filteredBook = filteredBookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                    if (filteredBook != null)
                    {
                        var index = filteredBookList.IndexOf(filteredBook);
                        filteredBookList.Remove(filteredBook);
                        filteredBookList.Insert(index, book);
                        refresh = true;
                    }
                    else
                    {
                        filteredBookList.Add(book);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }
    }
}
