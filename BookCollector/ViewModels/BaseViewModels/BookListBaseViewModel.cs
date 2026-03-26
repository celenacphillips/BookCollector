// <copyright file="BookListBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.Models;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Book;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// BookBaseViewModel class.
    /// </summary>
    public abstract partial class BookListBaseViewModel : BookBaseViewModel
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

        /********************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="BookListBaseViewModel"/> class.
        /// </summary>
        public BookListBaseViewModel()
        {
            this.ShowComments = Preferences.Get("CommentsOn", true /* Default */);
            this.ShowChapters = Preferences.Get("ChaptersOn", true /* Default */);
            this.ShowFavorites = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowRatings = Preferences.Get("RatingsOn", true /* Default */);
        }

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden books or not.
        /// </summary>
        public static bool ShowHiddenBooks { get; set; }

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

        /// <summary>
        /// Gets or sets the saved book location filter option.
        /// </summary>
        public string? BookLocationOption { get; set; }

        /// <summary>
        /// Gets or sets the saved book series filter option.
        /// </summary>
        public string? BookSeriesOption { get; set; }

        /// <summary>
        /// Gets or sets the saved book author filter option.
        /// </summary>
        public string? BookAuthorOption { get; set; }

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether the book title option is checked or not.
        /// </summary>
        public bool BookTitleChecked { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether the series or is checked or not.
        /// </summary>
        public bool SeriesOrderChecked { get; set; }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <param name="hiddenBookList">Book list.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            int,
            int,
            string,
            bool,
            ObservableCollection<BookModel>?,
            ObservableCollection<string>?,
            ObservableCollection<string>?,
            ObservableCollection<string>?,
            ObservableCollection<string>?)>
            SetViewModelData(ObservableCollection<BookModel>? hiddenBookList)
        {
            int totalBooksCount = 0, filteredBooksCount = 0;
            var showCollectionViewFooter = false;

            var filteredBookList = new ObservableCollection<BookModel>();
            var bookPublisherList = new ObservableCollection<string>();
            var bookLanguageList = new ObservableCollection<string>();
            var bookPublishYearList = new ObservableCollection<string>();
            var bookAuthorList = new ObservableCollection<string>();

            if (hiddenBookList != null)
            {
                totalBooksCount = hiddenBookList.Count;

                await Task.WhenAll(hiddenBookList.Select(x => x.SetAuthorListStringFromDatabase()));
                await Task.WhenAll(hiddenBookList.Select(x => x.SetCoverDisplay()));

                var authors = FillLists.GetAllAuthorsInBookList(hiddenBookList);
                var bookPublishers = FillLists.GetAllPublishersInBookList(hiddenBookList);
                var bookLanguages = FillLists.GetAllLanguagesInBookList(hiddenBookList);
                var bookPublishYears = FillLists.GetAllPublisherYearsInBookList(hiddenBookList);

                var filteredList = FilterLists.FilterList(
                        hiddenBookList,
                        this.FavoriteBooksOption,
                        this.BookFormatOption,
                        this.BookPublisherOption,
                        this.BookLanguageOption,
                        this.BookRatingOption,
                        this.BookPublishYearOption,
                        this.BookAuthorOption,
                        this.BookCoverOption,
                        this.SearchString);

                await Task.WhenAll(filteredList);

                filteredBookList = filteredList.Result;

                if (filteredBookList != null)
                {
                    filteredBooksCount = filteredBookList.Count;

                    showCollectionViewFooter = filteredBooksCount > 0;

                    await Task.WhenAll(filteredBookList.Select(x => x.SetReadingProgress()));
                    await Task.WhenAll(filteredBookList.Select(x => x.SetBookTotalTime()));

                    filteredBookList = await SortLists.SortList(
                                   filteredBookList!,
                                   this.BookTitleChecked,
                                   this.BookReadingDateChecked,
                                   this.BookReadPercentageChecked,
                                   this.BookPublisherChecked,
                                   this.BookPublishYearChecked,
                                   this.AuthorLastNameChecked,
                                   this.SeriesOrderChecked,
                                   this.BookFormatChecked,
                                   this.BookPriceChecked,
                                   this.PageCountBookTimeChecked,
                                   this.AscendingChecked,
                                   this.DescendingChecked);
                }

                await Task.WhenAll(bookPublishers, bookLanguages, bookPublishYears, authors);

                bookPublisherList = bookPublishers.Result;
                bookLanguageList = bookLanguages.Result;
                bookPublishYearList = bookPublishYears.Result;
                bookAuthorList = authors.Result;
            }

            var totalBooksString = StringManipulation.SetTotalBooksString(filteredBooksCount, totalBooksCount);

            return (totalBooksCount,
                filteredBooksCount,
                totalBooksString,
                showCollectionViewFooter,
                filteredBookList,
                bookPublisherList,
                bookLanguageList,
                bookPublishYearList,
                bookAuthorList);
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <param name="hiddenBookList">Book list.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            int,
            int,
            string,
            bool,
            ObservableCollection<WishlistBookModel>?,
            ObservableCollection<string>?,
            ObservableCollection<string>?,
            ObservableCollection<string>?,
            ObservableCollection<string>?,
            ObservableCollection<string>?,
            ObservableCollection<string>?)>
            SetViewModelData(ObservableCollection<WishlistBookModel>? hiddenBookList)
        {
            int totalBooksCount = 0, filteredBooksCount = 0;
            var showCollectionViewFooter = false;

            var filteredBookList = new ObservableCollection<WishlistBookModel>();
            var bookPublisherList = new ObservableCollection<string>();
            var bookLanguageList = new ObservableCollection<string>();
            var bookPublishYearList = new ObservableCollection<string>();
            var bookAuthorList = new ObservableCollection<string>();
            var bookLocationList = new ObservableCollection<string>();
            var bookSeriesList = new ObservableCollection<string>();

            if (hiddenBookList != null)
            {
                totalBooksCount = hiddenBookList.Count;

                await Task.WhenAll(hiddenBookList.Select(x => x.SetCoverDisplay()));

                var authors = FillLists.GetAllAuthorsInBookList(hiddenBookList);
                var bookPublishers = FillLists.GetAllPublishersInBookList(hiddenBookList);
                var bookLanguages = FillLists.GetAllLanguagesInBookList(hiddenBookList);
                var bookPublishYears = FillLists.GetAllPublisherYearsInBookList(hiddenBookList);
                var bookLocations = FillLists.GetAllLocationsInBookList(hiddenBookList);
                var bookSeries = FillLists.GetAllSeriesInBookList(hiddenBookList);

                var filteredList = FilterLists.FilterList(
                        hiddenBookList,
                        this.BookFormatOption,
                        this.BookPublisherOption,
                        this.BookLanguageOption,
                        this.BookPublishYearOption,
                        this.BookAuthorOption,
                        this.BookLocationOption,
                        this.BookSeriesOption,
                        this.BookCoverOption,
                        this.SearchString);

                await Task.WhenAll(filteredList);

                filteredBookList = filteredList.Result;

                if (filteredBookList != null)
                {
                    filteredBooksCount = filteredBookList.Count;

                    showCollectionViewFooter = filteredBooksCount > 0;
                    await Task.WhenAll(filteredBookList.Select(x => x.SetBookTotalTime()));

                    filteredBookList = await SortLists.SortList(
                                   filteredBookList!,
                                   this.BookTitleChecked,
                                   this.BookPublisherChecked,
                                   this.BookPublishYearChecked,
                                   this.AuthorLastNameChecked,
                                   this.BookFormatChecked,
                                   this.BookPriceChecked,
                                   this.PageCountBookTimeChecked,
                                   this.AscendingChecked,
                                   this.DescendingChecked);
                }

                await Task.WhenAll(bookPublishers, bookLanguages, bookPublishYears, authors, bookLocations, bookSeries);

                bookPublisherList = bookPublishers.Result;
                bookLanguageList = bookLanguages.Result;
                bookPublishYearList = bookPublishYears.Result;
                bookAuthorList = authors.Result;
                bookLocationList = bookLocations.Result;
                bookSeriesList = bookSeries.Result;
            }

            var totalBooksString = StringManipulation.SetTotalBooksString(filteredBooksCount, totalBooksCount);

            return (totalBooksCount,
                filteredBooksCount,
                totalBooksString,
                showCollectionViewFooter,
                filteredBookList,
                bookPublisherList,
                bookLanguageList,
                bookPublishYearList,
                bookAuthorList,
                bookLocationList,
                bookSeriesList);
        }

        /********************************************************/

        /// <summary>
        /// Filter and sort book list.
        /// </summary>
        /// <param name="hiddenFilteredBookList">Book list.</param>
        /// <param name="totalBooksCount">Total book count.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            ObservableCollection<BookModel>?,
            int,
            string)> BookSearch(ObservableCollection<BookModel>? hiddenFilteredBookList, int totalBooksCount)
        {
            var filteredBookList = await FilterLists.FilterList(
                            hiddenFilteredBookList,
                            this.FavoriteBooksOption,
                            this.BookFormatOption,
                            this.BookPublisherOption,
                            this.BookLanguageOption,
                            this.BookRatingOption,
                            this.BookPublishYearOption,
                            this.BookAuthorOption,
                            this.BookCoverOption,
                            this.SearchString);

            var filteredBooksCount = filteredBookList?.Count ?? 0;

            var totalBooksString = StringManipulation.SetTotalBooksString(filteredBooksCount, totalBooksCount);

            filteredBookList = await SortLists.SortList(
                                filteredBookList!,
                                this.BookTitleChecked,
                                this.BookReadingDateChecked,
                                this.BookReadPercentageChecked,
                                this.BookPublisherChecked,
                                this.BookPublishYearChecked,
                                this.AuthorLastNameChecked,
                                this.SeriesOrderChecked,
                                this.BookFormatChecked,
                                this.BookPriceChecked,
                                this.PageCountBookTimeChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            return (filteredBookList, filteredBooksCount, totalBooksString);
        }

        /// <summary>
        /// Filter and sort wishlist book list.
        /// </summary>
        /// <param name="hiddenFilteredBookList">Book list.</param>
        /// <param name="totalBooksCount">Total book count.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            ObservableCollection<WishlistBookModel>?,
            int,
            string)> BookSearch(ObservableCollection<WishlistBookModel>? hiddenFilteredBookList, int totalBooksCount)
        {
            var filteredBookList = await FilterLists.FilterList(
                            hiddenFilteredBookList,
                            this.FavoriteBooksOption,
                            this.BookFormatOption,
                            this.BookPublisherOption,
                            this.BookLanguageOption,
                            this.BookRatingOption,
                            this.BookPublishYearOption,
                            this.BookAuthorOption,
                            this.BookCoverOption,
                            this.SearchString);

            var filteredBooksCount = filteredBookList?.Count ?? 0;

            var totalBooksString = StringManipulation.SetTotalBooksString(filteredBooksCount, totalBooksCount);

            filteredBookList = await SortLists.SortList(
                                   filteredBookList!,
                                   this.BookTitleChecked,
                                   this.BookPublisherChecked,
                                   this.BookPublishYearChecked,
                                   this.AuthorLastNameChecked,
                                   this.BookFormatChecked,
                                   this.BookPriceChecked,
                                   this.PageCountBookTimeChecked,
                                   this.AscendingChecked,
                                   this.DescendingChecked);

            return (filteredBookList, filteredBooksCount, totalBooksString);
        }

        /********************************************************/

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public abstract bool GetPreferences();

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
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public abstract SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel);

        /********************************************************/

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
                    this.SetRefreshView(true);
                    await this.SetViewModelData();
                }
            }
        }

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
                    this.SetRefreshView(true);
                    await this.SetViewModelData();
                }
            }
        }

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
    }
}
