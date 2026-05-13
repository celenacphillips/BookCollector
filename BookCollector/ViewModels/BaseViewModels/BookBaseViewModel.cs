// <copyright file="BookBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using BookCollector.CustomPermissions;
    using BookCollector.Data;
    using BookCollector.Data.Enums;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.Author;
    using BookCollector.ViewModels.Collection;
    using BookCollector.ViewModels.Genre;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Library;
    using BookCollector.ViewModels.Location;
    using BookCollector.ViewModels.Main;
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
        /// Gets or sets a value indicating whether to show loan out books option or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool loanOutBooks;

        /// <summary>
        /// Gets or sets a value indicating whether to show borrow books option or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool borrowBooks;

        /********************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="BookBaseViewModel"/> class.
        /// </summary>
        public BookBaseViewModel()
        {
            this.ShowComments = DevicePreferences.CommentsShowValue;
            this.ShowChapters = DevicePreferences.ChaptersShowValue;
            this.ShowFavorites = DevicePreferences.FavoritesShowValue;
            this.ShowRatings = DevicePreferences.RatingsShowValue;
        }

        /********************************************************/

        /// <summary>
        /// Sets the book total time based on the book format and updates the book total time property accordingly.
        /// </summary>
        /// <param name="format">Book format.</param>
        /// <param name="hourTotal">Hour total.</param>
        /// <param name="minuteTotal">Minute total.</param>
        /// <returns>The formatted total time, or null.</returns>
        public static double? SetBookTotalTime(string? format, int hourTotal, int minuteTotal)
        {
            return
                !string.IsNullOrEmpty(format) && format.Equals(AppStringResources.Audiobook) ? (double)hourTotal + ((double)minuteTotal / 60) : null;
        }

        /// <summary>
        /// Creates a parsed string with the input values.
        /// </summary>
        /// <param name="bookPrice">Book price.</param>
        /// <returns>Parsed string.</returns>
        public static double SetBookPriceValue(string? bookPrice)
        {
            if (string.IsNullOrEmpty(bookPrice))
            {
                return 0;
            }

            bookPrice = bookPrice.Trim();

            var firstCharacter = bookPrice[0];

            if (!char.IsDigit(firstCharacter))
            {
                bookPrice = bookPrice[1..].Trim();
            }

            if (double.TryParse(bookPrice, out double price))
            {
                return price;
            }

            return 0;
        }

        /// <summary>
        /// Set the book cover display.
        /// </summary>
        /// <param name="bookCoverFileName">Book cover file name.</param>
        /// <param name="bookCoverUrl">Book cover URL.</param>
        /// <param name="bookCover">Book cover image source.</param>
        /// <returns>Two booleans (has a book cover and not) and an image source of the book cover.</returns>
        public static async Task<(bool, bool, ImageSource?)> SetCoverDisplay(string? bookCoverFileName, string? bookCoverUrl, ImageSource? bookCover)
        {
            var bookCoverImageSource = await CheckBookCover(bookCoverFileName, bookCoverUrl);

            var hasBookCover = bookCoverImageSource != null || bookCover != null;
            var hasNoBookCover = !hasBookCover;

            return (hasBookCover, hasNoBookCover, bookCoverImageSource);
        }

        /// <summary>
        /// Set author list string.
        /// </summary>
        /// <param name="authorList">Author list to parse through.</param>
        /// <returns>The formatted author list string.</returns>
        public static string? SetAuthorListStringFromInputList(ObservableCollection<AuthorModel>? authorList)
        {
            var authorListString = string.Empty;

            if (authorList != null)
            {
                for (int i = 0; i < authorList.Count; i++)
                {
                    var author = authorList[i];
                    if (author != null && !string.IsNullOrEmpty(author.FirstName) && !string.IsNullOrEmpty(author.LastName))
                    {
                        authorListString += author.ReverseFullName;
                        if (i != authorList.Count - 1)
                        {
                            authorListString += "; ";
                        }
                    }
                }
            }

            return authorListString;
        }

        /// <summary>
        /// Add book to static lists.
        /// </summary>
        /// <param name="book">Book to add.</param>
        /// <param name="previousViewModel">Previous model to return to.</param>
        /// <returns>A task.</returns>
        public static async Task AddToStaticList(BookModel book, object? previousViewModel = null)
        {
            await SetAllBooksViewModelList(book, ActionState.Add);

            await SetReadingViewModelList(book, ActionState.Add);

            await SetToBeReadViewModelList(book, ActionState.Add);

            await SetReadViewModelList(book, ActionState.Add);

            await SetLoanedOutBooksViewModelList(book, ActionState.Add);

            await SetBorrowedBooksViewModelList(book, ActionState.Add);

            await SetCollectionsViewModelBookList(book, ActionState.Add, previousViewModel);

            await SetGenresViewModelBookList(book, ActionState.Add, previousViewModel);

            await SetSeriesViewModelBookList(book, ActionState.Add, previousViewModel);

            await SetAuthorsViewModelBookList(book, ActionState.Add, previousViewModel);

            await SetLocationsViewModelBookList(book, ActionState.Add, previousViewModel);

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
        /// <param name="previousViewModel">Previous model to return to.</param>
        /// <returns>A task.</returns>
        public static async Task RemoveFromStaticList(BookModel book, object? previousViewModel = null)
        {
            await SetAllBooksViewModelList(book, ActionState.Remove);

            await SetReadingViewModelList(book, ActionState.Remove);

            await SetToBeReadViewModelList(book, ActionState.Remove);

            await SetReadViewModelList(book, ActionState.Remove);

            await SetLoanedOutBooksViewModelList(book, ActionState.Remove);

            await SetBorrowedBooksViewModelList(book, ActionState.Remove);

            await SetCollectionsViewModelBookList(book, ActionState.Remove, previousViewModel);

            await SetGenresViewModelBookList(book, ActionState.Remove, previousViewModel);

            await SetSeriesViewModelBookList(book, ActionState.Remove, previousViewModel);

            await SetAuthorsViewModelBookList(book, ActionState.Remove, previousViewModel);

            await SetLocationsViewModelBookList(book, ActionState.Remove, previousViewModel);

            CollectionsViewModel.RefreshView = true;
            GenresViewModel.RefreshView = true;
            SeriesViewModel.RefreshView = true;
            AuthorsViewModel.RefreshView = true;
            LocationsViewModel.RefreshView = true;
        }

        /// <summary>
        /// Remove book from lists.
        /// </summary>
        /// <param name="book">Book to remove.</param>
        /// <param name="bookList">Main book list to remove book from.</param>
        /// <param name="filteredBookList">Filtered book list to remove book from.</param>
        /// <returns>Refresh view value.</returns>
        public static async Task<bool> RemoveBookFromStaticList(BookModel book, ObservableCollection<BookModel>? bookList, ObservableCollection<BookModel>? filteredBookList)
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
        /// Check book cover and set values.
        /// </summary>
        /// <param name="fileName">Book cover file name.</param>
        /// <param name="coverUrl">Book cover url.</param>
        /// <returns>Image source of book cover.</returns>
        public static async Task<ImageSource?> CheckBookCover(string? fileName, string? coverUrl)
        {
            ImageSource? imageSource = null;

            if (!string.IsNullOrEmpty(fileName))
            {
                var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                var file = $"{directory}/{fileName}";

                if (File.Exists(file))
                {
                    imageSource = ImageSource.FromFile(file);
                }
            }

            if (!string.IsNullOrEmpty(coverUrl))
            {
                PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                if (internetStatus != PermissionStatus.Granted)
                {
                    internetStatus = await Permissions.RequestAsync<InternetPermission>();
                }

                if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    imageSource = new UriImageSource
                    {
                        Uri = new Uri(coverUrl),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(14),
                    };
                }
            }

            return imageSource;
        }

        /********************************************************/

        /// <summary>
        /// Show book edit view.
        /// </summary>
        /// <param name="book">Book to show on the edit view.</param>
        /// <returns>A task.</returns>
        public async Task ShowBookEditView(BookModel book)
        {
            await this.SetIsBusyTrue();

            var view = new BookEditView(book, $"{AppStringResources.AddNewBook}", false, null, this);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /********************************************************/

        /// <summary>
        /// Create a new book and navigate to the book edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddBook()
        {
            await this.ShowBookEditView(new BookModel());
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

        /********************************************************/

        private static async Task<bool> AddBookToStaticList(BookModel book, ObservableCollection<BookModel> bookList, ObservableCollection<BookModel>? filteredBookList)
        {
            var refresh = false;

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

        private static async Task SetAllBooksViewModelList(BookModel book, ActionState action)
        {
            if (AllBooksViewModel.fullBookList != null)
            {
                if (action == ActionState.Add)
                {
                    AllBooksViewModel.RefreshView = await AddBookToStaticList(book, AllBooksViewModel.fullBookList, AllBooksViewModel.filteredBookList);
                }

                if (action == ActionState.Remove)
                {
                    AllBooksViewModel.RefreshView = await RemoveBookFromStaticList(book, AllBooksViewModel.fullBookList, AllBooksViewModel.filteredBookList);
                }
            }
        }

        private static async Task SetReadingViewModelList(BookModel book, ActionState action)
        {
            if (ReadingViewModel.fullBookList != null)
            {
                if (action == ActionState.Add)
                {
                    if (((book.BookPageRead != book.BookPageTotal && book.BookPageRead != 0) ||
                        book.UpNext) ||
                        (book.BookHourListened != book.BookHoursTotal && book.BookMinuteListened != book.BookMinutesTotal && book.BookHourListened != 0 && book.BookMinuteListened != 0) ||
                        (!string.IsNullOrEmpty(book.BookStartDate) && string.IsNullOrEmpty(book.BookEndDate)))
                    {
                        ReadingViewModel.RefreshView = await AddBookToStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList);
                        ToBeReadViewModel.RefreshView = await RemoveBookFromStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList);
                        ReadViewModel.RefreshView = await RemoveBookFromStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList);
                    }
                }

                if (action == ActionState.Remove)
                {
                    if ((book.BookPageRead != book.BookPageTotal && book.BookPageRead != 0) ||
                        book.UpNext ||
                        (book.BookHourListened != book.BookHoursTotal && book.BookMinuteListened != book.BookMinutesTotal))
                    {
                        ReadingViewModel.RefreshView = await RemoveBookFromStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList);
                    }
                }
            }
        }

        private static async Task SetToBeReadViewModelList(BookModel book, ActionState action)
        {
            if (ToBeReadViewModel.fullBookList != null)
            {
                if (action == ActionState.Add)
                {
                    if (book.BookPageRead == 0 &&
                        !book.UpNext &&
                        (book.BookHourListened == 0 && book.BookMinuteListened == 0) &&
                        string.IsNullOrEmpty(book.BookStartDate) && string.IsNullOrEmpty(book.BookEndDate))
                    {
                        ToBeReadViewModel.RefreshView = await AddBookToStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList);
                        ReadingViewModel.RefreshView = await RemoveBookFromStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList);
                        ReadViewModel.RefreshView = await RemoveBookFromStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList);
                    }
                }

                if (action == ActionState.Remove)
                {
                    if (book.BookPageRead == 0 &&
                        !book.UpNext &&
                        book.BookHourListened == 0 && book.BookMinuteListened == 0)
                    {
                        ToBeReadViewModel.RefreshView = await RemoveBookFromStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList);
                    }
                }
            }
        }

        private static async Task SetReadViewModelList(BookModel book, ActionState action)
        {
            if (ReadViewModel.fullBookList != null)
            {
                if (action == ActionState.Add)
                {
                    if ((book.BookPageRead == book.BookPageTotal && book.BookPageRead != 0) ||
                        (book.BookHourListened == book.BookHoursTotal && book.BookMinuteListened == book.BookMinutesTotal && book.BookHourListened != 0 && book.BookMinuteListened != 0) ||
                        (!string.IsNullOrEmpty(book.BookStartDate) && !string.IsNullOrEmpty(book.BookEndDate)))
                    {
                        ReadViewModel.RefreshView = await AddBookToStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList);
                        ToBeReadViewModel.RefreshView = await RemoveBookFromStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList);
                        ReadingViewModel.RefreshView = await RemoveBookFromStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList);
                    }
                }

                if (action == ActionState.Remove)
                {
                    if ((book.BookPageRead == book.BookPageTotal && book.BookPageRead != 0) ||
                        (book.BookHourListened == book.BookHoursTotal && book.BookMinuteListened == book.BookMinutesTotal && book.BookHourListened != 0 && book.BookMinuteListened != 0))
                    {
                        ReadViewModel.RefreshView = await RemoveBookFromStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList);
                    }
                }
            }
        }

        private static async Task SetLoanedOutBooksViewModelList(BookModel book, ActionState action)
        {
            if (LoanedOutBooksViewModel.fullBookList != null)
            {
                if (action == ActionState.Add)
                {
                    if (!string.IsNullOrEmpty(book.LoanedTo) || !string.IsNullOrEmpty(book.BookLoanedOutOn))
                    {
                        LoanedOutBooksViewModel.RefreshView = await AddBookToStaticList(book, LoanedOutBooksViewModel.fullBookList, LoanedOutBooksViewModel.filteredBookList);
                    }
                }

                if (action == ActionState.Remove ||
                    (string.IsNullOrEmpty(book.LoanedTo) && string.IsNullOrEmpty(book.BookLoanedOutOn)))
                {
                    LoanedOutBooksViewModel.RefreshView = await RemoveBookFromStaticList(book, LoanedOutBooksViewModel.fullBookList, LoanedOutBooksViewModel.filteredBookList);
                }
            }
        }

        private static async Task SetBorrowedBooksViewModelList(BookModel book, ActionState action)
        {
            if (BorrowedBooksViewModel.fullBookList != null)
            {
                if (action == ActionState.Add)
                {
                    if (!string.IsNullOrEmpty(book.BorrowedFrom) || !string.IsNullOrEmpty(book.BookBorrowedOn))
                    {
                        BorrowedBooksViewModel.RefreshView = await AddBookToStaticList(book, BorrowedBooksViewModel.fullBookList, BorrowedBooksViewModel.filteredBookList);
                    }
                }

                if (action == ActionState.Remove ||
                    (string.IsNullOrEmpty(book.BorrowedFrom) && string.IsNullOrEmpty(book.BookBorrowedOn)))
                {
                    BorrowedBooksViewModel.RefreshView = await RemoveBookFromStaticList(book, BorrowedBooksViewModel.fullBookList, BorrowedBooksViewModel.filteredBookList);
                }
            }
        }

        private static async Task SetCollectionsViewModelBookList(BookModel book, ActionState action, object? previousViewModel = null)
        {
            if (CollectionsViewModel.fullCollectionList != null &&
                previousViewModel != null &&
                previousViewModel is CollectionMainViewModel collectionViewModel)
            {
                if (action == ActionState.Add &&
                    book.BookCollectionGuid != null)
                {
                    var selected = CollectionsViewModel.fullCollectionList.FirstOrDefault(x => x.CollectionGuid == book.BookCollectionGuid);

                    if (selected != null)
                    {
                        if (collectionViewModel.FullBookList != null)
                        {
                            CollectionMainViewModel.RefreshView = await AddBookToStaticList(book, collectionViewModel.FullBookList, collectionViewModel.FilteredBookList);
                        }
                    }
                }

                if (action == ActionState.Remove ||
                    book.BookCollectionGuid == null)
                {
                    CollectionMainViewModel.RefreshView = await RemoveBookFromStaticList(book, collectionViewModel.FullBookList, collectionViewModel.FilteredBookList);
                }
            }
        }

        private static async Task SetGenresViewModelBookList(BookModel book, ActionState action, object? previousViewModel = null)
        {
            if (GenresViewModel.fullGenreList != null &&
                previousViewModel != null &&
                previousViewModel is GenreMainViewModel genreViewModel)
            {
                if (action == ActionState.Add &&
                    book.BookGenreGuid != null)
                {
                    var selected = GenresViewModel.fullGenreList.FirstOrDefault(x => x.GenreGuid == book.BookGenreGuid);

                    if (selected != null)
                    {
                        if (genreViewModel.FullBookList != null)
                        {
                            GenreMainViewModel.RefreshView = await AddBookToStaticList(book, genreViewModel.FullBookList, genreViewModel.FilteredBookList);
                        }
                    }
                }

                if (action == ActionState.Remove ||
                    book.BookGenreGuid == null)
                {
                    GenreMainViewModel.RefreshView = await RemoveBookFromStaticList(book, genreViewModel.FullBookList, genreViewModel.FilteredBookList);
                }
            }
        }

        private static async Task SetSeriesViewModelBookList(BookModel book, ActionState action, object? previousViewModel = null)
        {
            if (SeriesViewModel.fullSeriesList != null &&
                previousViewModel != null &&
                previousViewModel is SeriesMainViewModel seriesViewModel)
            {
                if (action == ActionState.Add &&
                    book.BookSeriesGuid != null)
                {
                    var selected = SeriesViewModel.fullSeriesList.FirstOrDefault(x => x.SeriesGuid == book.BookSeriesGuid);

                    if (selected != null)
                    {
                        if (seriesViewModel.FullBookList != null)
                        {
                            SeriesMainViewModel.RefreshView = await AddBookToStaticList(book, seriesViewModel.FullBookList, seriesViewModel.FilteredBookList);
                        }
                    }
                }

                if (action == ActionState.Remove ||
                    book.BookSeriesGuid == null)
                {
                    SeriesMainViewModel.RefreshView = await RemoveBookFromStaticList(book, seriesViewModel.FullBookList, seriesViewModel.FilteredBookList);
                }
            }
        }

        private static async Task SetAuthorsViewModelBookList(BookModel book, ActionState action, object? previousViewModel = null)
        {
            if (AuthorsViewModel.fullAuthorList != null &&
                previousViewModel != null &&
                previousViewModel is AuthorMainViewModel authorViewModel)
            {
                if (action == ActionState.Add &&
                    !string.IsNullOrEmpty(book.AuthorListString))
                {
                    var authors = await StringManipulation.SplitAuthorListStringIntoAuthorList(book.AuthorListString);

                    foreach (var author in authors)
                    {
                        var selected = AuthorsViewModel.fullAuthorList.FirstOrDefault(x => x.FirstName.Equals(author.FirstName) && x.LastName.Equals(author.LastName));

                        if (selected != null)
                        {
                            if (authorViewModel.FullBookList != null)
                            {
                                AuthorMainViewModel.RefreshView = await AddBookToStaticList(book, authorViewModel.FullBookList, authorViewModel.FilteredBookList);
                            }
                        }
                    }
                }

                if (action == ActionState.Remove ||
                    string.IsNullOrEmpty(book.AuthorListString))
                {
                    AuthorMainViewModel.RefreshView = await RemoveBookFromStaticList(book, authorViewModel.FullBookList, authorViewModel.FilteredBookList);
                }
            }
        }

        private static async Task SetLocationsViewModelBookList(BookModel book, ActionState action, object? previousViewModel = null)
        {
            if (LocationsViewModel.fullLocationList != null &&
                previousViewModel != null &&
                previousViewModel is LocationMainViewModel locationViewModel)
            {
                if (action == ActionState.Add &&
                    book.BookLocationGuid != null)
                {
                    var selected = LocationsViewModel.fullLocationList.FirstOrDefault(x => x.LocationGuid == book.BookLocationGuid);

                    if (selected != null)
                    {
                        if (locationViewModel.FullBookList != null)
                        {
                            LocationMainViewModel.RefreshView = await AddBookToStaticList(book, locationViewModel.FullBookList, locationViewModel.FilteredBookList);
                        }
                    }
                }

                if (action == ActionState.Remove ||
                    book.BookLocationGuid == null)
                {
                    LocationMainViewModel.RefreshView = await RemoveBookFromStaticList(book, locationViewModel.FullBookList, locationViewModel.FilteredBookList);
                }
            }
        }
    }
}
