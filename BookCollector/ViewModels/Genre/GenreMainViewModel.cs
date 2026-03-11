// <copyright file="GenreMainViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Genre
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Book;
    using BookCollector.Views.Groupings;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// GenreMainViewModel class.
    /// </summary>
    public partial class GenreMainViewModel : GenreBaseViewModel
    {
        /// <summary>
        /// Gets or sets the full book list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<BookModel>? fullBookList;

        /// <summary>
        /// Gets or sets the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<BookModel>? filteredBookList1;

        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public new ObservableCollection<BookModel>? filteredBookList2;

        /// <summary>
        /// Gets or sets the book author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<string>? bookAuthorList;

        /// <summary>
        /// Gets or sets the total count of books, based on the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int totalBooksCount;

        /// <summary>
        /// Gets or sets the total count of books, based on the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int filteredBooksCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenreMainViewModel"/> class.
        /// </summary>
        /// <param name="genre">Genre to view.</param>
        /// <param name="view">View related to view model.</param>
        public GenreMainViewModel(GenreModel genre, ContentPage view)
        {
            this.View = view;
            this.SelectedGenre = genre;
            this.CollectionViewHeight = this.DeviceHeight;
            this.InfoText = $"{AppStringResources.GenreMainView_InfoText.Replace("genre", $"{this.SelectedGenre.GenreName}")}";
            this.RefreshView = true;
        }

        /// <summary>
        /// Gets or sets the book author option.
        /// </summary>
        public string? BookAuthorOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public bool RefreshView { get; set; }

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
        public async Task SetList(bool showHiddenBooks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksInGenreList(this.SelectedGenre?.GenreGuid, ShowHiddenBook);

            if (!showHiddenBooks)
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!.Where(x => !x.HideBook));
            }
            else
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!);
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task SetViewModelData()
        {
            if (this.RefreshView && this.SelectedGenre != null)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await this.SetList(ShowHiddenBook);

                    if (this.FilteredBookList1 != null)
                    {
                        this.TotalBooksCount = this.FilteredBookList1.Count;

                        await Task.WhenAll(this.FilteredBookList1.Select(x => x.SetAuthorListString()));
                        await Task.WhenAll(this.FilteredBookList1.Select(x => x.SetCoverDisplay()));

                        var authors = FillLists.GetAllAuthorsInBookList(this.FilteredBookList1);
                        var bookPublishers = FillLists.GetAllPublishersInBookList(this.FilteredBookList1);
                        var bookLanguages = FillLists.GetAllLanguagesInBookList(this.FilteredBookList1);
                        var bookPublishYears = FillLists.GetAllPublisherYearsInBookList(this.FilteredBookList1);
                        var filteredList = FilterLists.FilterBookList(
                                this.FilteredBookList1,
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

                        this.FilteredBookList2 = filteredList.Result;

                        if (this.FilteredBookList2 != null)
                        {
                            await Task.WhenAll(this.FilteredBookList2.Select(x => x.SetReadingProgress()));
                            await Task.WhenAll(this.FilteredBookList2.Select(x => x.SetBookTotalTime()));

                            var sortList = SortLists.SortBookList(
                                    this.FilteredBookList2,
                                    this.BookTitleChecked,
                                    this.BookReadingDateChecked,
                                    this.BookReadPercentageChecked,
                                    this.BookPublisherChecked,
                                    this.BookPublishYearChecked,
                                    this.AuthorLastNameChecked,
                                    this.BookFormatChecked,
                                    this.BookPriceChecked,
                                    this.PageCountBookTimeChecked,
                                    this.AscendingChecked,
                                    this.DescendingChecked);

                            this.FilteredBooksCount = this.FilteredBookList2.Count;

                            this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                            this.ShowCollectionViewFooter = this.FilteredBooksCount > 0;

                            await Task.WhenAll(sortList);

                            this.FilteredBookList2 = sortList.Result;
                        }

                        await Task.WhenAll(bookPublishers, bookLanguages, bookPublishYears, authors);

                        this.BookPublisherList = bookPublishers.Result;
                        this.BookLanguageList = bookLanguages.Result;
                        this.BookPublishYearList = bookPublishYears.Result;
                        this.BookAuthorList = authors.Result;
                    }

                    this.SetIsBusyFalse();
                    this.RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    this.RefreshView = false;
                }
            }
        }

        /// <summary>
        /// Search the list based on the book title.
        /// </summary>
        /// <param name="input">Input string to find.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookSearchOnTitle(string? input)
        {
            this.SearchString = input;

            if (this.FilteredBookList2 != null && this.FilteredBookList1 != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    this.FilteredBookList2 = FilterLists.FilterOnSearchString(this.FilteredBookList1, input);
                }
                else
                {
                    this.FilteredBookList2 = await FilterLists.FilterBookList(
                                this.FilteredBookList1,
                                this.FavoriteBooksOption,
                                this.BookFormatOption,
                                this.BookPublisherOption,
                                this.BookLanguageOption,
                                this.BookRatingOption,
                                this.BookPublishYearOption,
                                this.BookAuthorOption,
                                this.BookCoverOption,
                                this.SearchString);
                }

                this.FilteredBooksCount = this.FilteredBookList2 != null ? this.FilteredBookList2.Count : 0;

                this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                var sortList = SortLists.SortBookList(
                                        this.FilteredBookList2!,
                                        this.BookTitleChecked,
                                        this.BookReadingDateChecked,
                                        this.BookReadPercentageChecked,
                                        this.BookPublisherChecked,
                                        this.BookPublishYearChecked,
                                        this.AuthorLastNameChecked,
                                        this.BookFormatChecked,
                                        this.BookPriceChecked,
                                        this.PageCountBookTimeChecked,
                                        this.AscendingChecked,
                                        this.DescendingChecked);

                await Task.WhenAll(sortList);

                this.FilteredBookList2 = sortList.Result;
            }
        }

        /// <summary>
        /// Set refreshing values and reset the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            this.RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        /// <summary>
        /// Create a new book and navigate to the book edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddNewBook()
        {
            if (this.SelectedGenre != null)
            {
                this.SetIsBusyTrue();

                var newBook = new BookModel()
                {
                    BookGenreGuid = this.SelectedGenre.GenreGuid,
                };

                var view = new BookEditView(newBook, $"{AppStringResources.AddNewBook}", false, null, this);

                await Shell.Current.Navigation.PushAsync(view);

                this.SetIsBusyFalse();
            }
        }

        /// <summary>
        /// Show the existing books view to add an existing book to the genre.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddExistingBook()
        {
            if (this.SelectedGenre != null && !string.IsNullOrEmpty(this.ViewTitle))
            {
                var view = new ExistingBooksView(this.SelectedGenre, this.ViewTitle, this);

                await Shell.Current.Navigation.PushAsync(view);
            }
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
                var viewModel = new FilterPopupViewModel(popup, this.ViewTitle, this.View)
                {
                    AuthorVisible = true,
                    AuthorOption = this.BookAuthorOption,
                    FavoriteVisible = this.ShowFavoriteBooks,
                    FavoriteOption = this.FavoriteBooksOption,
                    FormatVisible = true,
                    FormatOption = this.BookFormatOption,
                    PublisherVisible = true,
                    PublisherOption = this.BookPublisherOption,
                    PublishYearVisible = true,
                    PublishYearOption = this.BookPublishYearOption,
                    LanguageVisible = true,
                    LanguageOption = this.BookLanguageOption,
                    RatingVisible = this.ShowBookRatings,
                    RatingOption = this.BookRatingOption,
                    BookCoverVisible = true,
                    BookCoverOption = this.BookCoverOption,
                };
                viewModel.SetAuthorPicker(this.BookAuthorList);
                viewModel.SetFavoritePicker();
                viewModel.SetFormatPicker(this.BookFormats);
                viewModel.SetPublisherPicker(this.BookPublisherList);
                viewModel.SetPublishYearPicker(this.BookPublishYearList);
                viewModel.SetLanguagePicker(this.BookLanguageList);
                viewModel.SetRatingPicker();
                viewModel.SetBookCoverPicker();

                popup.BindingContext = viewModel;

                var result = await this.View.ShowPopupAsync(popup);
                if (!result.WasDismissedByTappingOutsideOfPopup)
                {
                    this.RefreshView = true;
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
                var viewModel = new SortPopupViewModel(popup, this.ViewTitle)
                {
                    BookTitleVisible = true,
                    BookTitleChecked = this.BookTitleChecked,
                    BookReadingDateVisible = true,
                    BookReadingDateChecked = this.BookReadingDateChecked,
                    BookReadPercentageVisible = true,
                    BookReadPercentageChecked = this.BookReadPercentageChecked,
                    BookPublisherVisible = true,
                    BookPublisherChecked = this.BookPublisherChecked,
                    BookPublishYearVisible = true,
                    BookPublishYearChecked = this.BookPublishYearChecked,
                    AuthorLastNameVisible = true,
                    AuthorLastNameChecked = this.AuthorLastNameChecked,
                    BookFormatVisible = true,
                    BookFormatChecked = this.BookFormatChecked,
                    PageCountTimeVisible = true,
                    PageCountTimeChecked = this.PageCountBookTimeChecked,
                    BookPriceVisible = true,
                    BookPriceChecked = this.BookPriceChecked,
                    AscendingChecked = this.AscendingChecked,
                    DescendingChecked = this.DescendingChecked,
                };

                popup.BindingContext = viewModel;

                var result = await this.View.ShowPopupAsync(popup);
                if (!result.WasDismissedByTappingOutsideOfPopup)
                {
                    this.RefreshView = true;
                    await this.SetViewModelData();
                }
            }
        }

        private void GetPreferences()
        {
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);
            this.ShowFavoriteBooks = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowBookRatings = Preferences.Get("RatingsOn", true /* Default */);

            this.BookAuthorOption = Preferences.Get($"{this.ViewTitle}_AuthorSelection", AppStringResources.AllAuthors /* Default */);
            this.FavoriteBooksOption = Preferences.Get($"{this.ViewTitle}_FavoriteSelection", AppStringResources.Both /* Default */);
            this.BookFormatOption = Preferences.Get($"{this.ViewTitle}_FormatSelection", AppStringResources.AllFormats /* Default */);
            this.BookPublisherOption = Preferences.Get($"{this.ViewTitle}_PublisherSelection", AppStringResources.AllPublishers /* Default */);
            this.BookPublishYearOption = Preferences.Get($"{this.ViewTitle}_PublishYearSelection", AppStringResources.AllPublishYears /* Default */);
            this.BookLanguageOption = Preferences.Get($"{this.ViewTitle}_LanguageSelection", AppStringResources.AllLanguages /* Default */);
            this.BookRatingOption = Preferences.Get($"{this.ViewTitle}_RatingSelection", AppStringResources.AllRatings /* Default */);
            this.BookCoverOption = Preferences.Get($"{this.ViewTitle}_BookCoverSelection", AppStringResources.Both /* Default */);

            this.BookTitleChecked = Preferences.Get($"{this.ViewTitle}_BookTitleSelection", true /* Default */);
            this.BookReadingDateChecked = Preferences.Get($"{this.ViewTitle}_BookReadingDateSelection", false /* Default */);
            this.BookReadPercentageChecked = Preferences.Get($"{this.ViewTitle}_BookReadPercentageSelection", false /* Default */);
            this.BookPublisherChecked = Preferences.Get($"{this.ViewTitle}_BookPublisherSelection", false /* Default */);
            this.BookPublishYearChecked = Preferences.Get($"{this.ViewTitle}_BookPublishYearSelection", false /* Default */);
            this.AuthorLastNameChecked = Preferences.Get($"{this.ViewTitle}_AuthorLastNameSelection", false /* Default */);
            this.BookFormatChecked = Preferences.Get($"{this.ViewTitle}_BookFormatSelection", false /* Default */);
            this.PageCountBookTimeChecked = Preferences.Get($"{this.ViewTitle}_PageCountBookTimeSelection", false /* Default */);
            this.BookPriceChecked = Preferences.Get($"{this.ViewTitle}_BookPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }
    }
}
