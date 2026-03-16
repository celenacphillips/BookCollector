// <copyright file="LocationMainViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Location
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Book;
    using BookCollector.Views.Groupings;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// LocationMainViewModel class.
    /// </summary>
    public partial class LocationMainViewModel : LocationsViewModel
    {
        /// <summary>
        /// Gets or sets the full book list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<BookModel>? fullBookList;

        /// <summary>
        /// Gets or sets the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<BookModel>? hiddenFilteredBookList;

        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static new ObservableCollection<BookModel>? filteredBookList;

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
        /// Gets or sets the book author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<string>? bookAuthorList;

        /// <summary>
        /// Gets or sets the total books string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalBooksString;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationMainViewModel"/> class.
        /// </summary>
        /// <param name="location">Location to view.</param>
        /// <param name="view">View related to view model.</param>
        public LocationMainViewModel(LocationModel location, ContentPage view)
            : base(view)
        {
            this.View = view;
            this.SelectedLocation = location;
            this.CollectionViewHeight = DeviceHeight;
            this.InfoText = $"{AppStringResources.LocationMainView_InfoText.Replace("location", $"{this.SelectedLocation.LocationName}")}";
            RefreshView = true;
        }

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
        public async new Task SetList(bool showHiddenBooks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksInLocationList(this.SelectedLocation?.LocationGuid, ShowHiddenBooks);

            this.HiddenFilteredBookList = SetList<BookModel>(this.FullBookList!, showHiddenBooks).ToObservableCollection();
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public override bool GetPreferences()
        {
            ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);
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

            return ShowHiddenBooks;
        }

        /// <summary>
        /// Check if the list is null.
        /// </summary>
        /// <returns>If the list is null.</returns>
        public override bool ListNullCheck()
        {
            return this.HiddenFilteredBookList != null;
        }

        /// <summary>
        /// Iterate through the list and set necessary data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetListData()
        {
            await Task.WhenAll(this.HiddenFilteredBookList!.Select(x => x.SetAuthorListString()));
            await Task.WhenAll(this.HiddenFilteredBookList!.Select(x => x.SetCoverDisplay()));
            await Task.WhenAll(this.HiddenFilteredBookList!.Select(x => x.SetReadingProgress()));
            await Task.WhenAll(this.HiddenFilteredBookList!.Select(x => x.SetBookTotalTime()));
        }

        /// <summary>
        /// Find filters for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetFilters()
        {
            var authors = FillLists.GetAllAuthorsInBookList(this.HiddenFilteredBookList!);
            var bookPublishers = FillLists.GetAllPublishersInBookList(this.HiddenFilteredBookList!);
            var bookLanguages = FillLists.GetAllLanguagesInBookList(this.HiddenFilteredBookList!);
            var bookPublishYears = FillLists.GetAllPublisherYearsInBookList(this.HiddenFilteredBookList!);

            var filteredList = FilterLists.FilterList(
                    this.HiddenFilteredBookList!,
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

            this.FilteredBookList = filteredList.Result;

            await Task.WhenAll(bookPublishers, bookLanguages, bookPublishYears, authors);

            this.BookPublisherList = bookPublishers.Result;
            this.BookLanguageList = bookLanguages.Result;
            this.BookPublishYearList = bookPublishYears.Result;
            this.BookAuthorList = authors.Result;
        }

        /// <summary>
        /// Find sort values for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetSorts()
        {
            var sortList = SortLists.SortBookList(
                                    this.FilteredBookList!,
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

            this.FilteredBookList = sortList.Result;
        }

        /// <summary>
        /// Set data for view.
        /// </summary>
        public async override void SetViewStrings()
        {
            this.TotalBooksCount = this.HiddenFilteredBookList?.Count ?? 0;

            this.FilteredBooksCount = this.FilteredBookList?.Count ?? 0;

            this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

            this.ShowCollectionViewFooter = this.FilteredBooksCount > 0;
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

            if (this.FilteredBookList != null && this.HiddenFilteredBookList != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    this.FilteredBookList = FilterLists.FilterOnSearchString(this.HiddenFilteredBookList, input);
                }
                else
                {
                    this.FilteredBookList = await FilterLists.FilterList(
                                this.HiddenFilteredBookList,
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

                this.SetViewStrings();

                await this.SetSorts();
            }
        }

        /// <summary>
        /// Create a new book and navigate to the book edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddNewBook()
        {
            if (this.SelectedLocation != null)
            {
                this.SetIsBusyTrue();

                var newBook = new BookModel()
                {
                    BookLocationGuid = this.SelectedLocation.LocationGuid,
                };

                var view = new BookEditView(newBook, $"{AppStringResources.AddNewBook}", false, null, this);

                await Shell.Current.Navigation.PushAsync(view);

                this.SetIsBusyFalse();
            }
        }

        /// <summary>
        /// Show the existing books view to add an existing book to the location.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddExistingBook()
        {
            if (this.SelectedLocation != null && !string.IsNullOrEmpty(this.ViewTitle))
            {
                var view = new ExistingBooksView(this.SelectedLocation, this.ViewTitle, this);

                await Shell.Current.Navigation.PushAsync(view);
            }
        }

        /// <summary>
        /// Set data for filter popup.
        /// </summary>
        /// <param name="viewModel">Filter popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override FilterPopupViewModel SetFilterPopupValues(FilterPopupViewModel viewModel)
        {
            viewModel.AuthorVisible = true;
            viewModel.AuthorOption = this.BookAuthorOption;
            /******************************/
            viewModel.FavoriteVisible = this.ShowFavoriteBooks;
            viewModel.FavoriteOption = this.FavoriteBooksOption;
            /******************************/
            viewModel.FormatVisible = true;
            viewModel.FormatOption = this.BookFormatOption;
            /******************************/
            viewModel.PublisherVisible = true;
            viewModel.PublisherOption = this.BookPublisherOption;
            /******************************/
            viewModel.PublishYearVisible = true;
            viewModel.PublishYearOption = this.BookPublishYearOption;
            /******************************/
            viewModel.LanguageVisible = true;
            viewModel.LanguageOption = this.BookLanguageOption;
            /******************************/
            viewModel.RatingVisible = this.ShowBookRatings;
            viewModel.RatingOption = this.BookRatingOption;
            /******************************/
            viewModel.BookCoverVisible = true;
            viewModel.BookCoverOption = this.BookCoverOption;

            return viewModel;
        }

        /// <summary>
        /// Set data for filter popup.
        /// </summary>
        /// <param name="viewModel">Filter popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override FilterPopupViewModel SetFilterPopupLists(FilterPopupViewModel viewModel)
        {
            viewModel.SetAuthorPicker(this.BookAuthorList);
            viewModel.SetFavoritePicker();
            viewModel.SetFormatPicker(this.BookFormats);
            viewModel.SetPublisherPicker(this.BookPublisherList);
            viewModel.SetPublishYearPicker(this.BookPublishYearList);
            viewModel.SetLanguagePicker(this.BookLanguageList);
            viewModel.SetRatingPicker();
            viewModel.SetBookCoverPicker();

            return viewModel;
        }

        /// <summary>
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel)
        {
            viewModel.BookTitleVisible = true;
            viewModel.BookTitleChecked = this.BookTitleChecked;
            /******************************/
            viewModel.BookReadingDateVisible = true;
            viewModel.BookReadingDateChecked = this.BookReadingDateChecked;
            /******************************/
            viewModel.BookReadPercentageVisible = true;
            viewModel.BookReadPercentageChecked = this.BookReadPercentageChecked;
            /******************************/
            viewModel.BookPublisherVisible = true;
            viewModel.BookPublisherChecked = this.BookPublisherChecked;
            /******************************/
            viewModel.BookPublishYearVisible = true;
            viewModel.BookPublishYearChecked = this.BookPublishYearChecked;
            /******************************/
            viewModel.AuthorLastNameVisible = true;
            viewModel.AuthorLastNameChecked = this.AuthorLastNameChecked;
            /******************************/
            viewModel.BookFormatVisible = true;
            viewModel.BookFormatChecked = this.BookFormatChecked;
            /******************************/
            viewModel.PageCountTimeVisible = true;
            viewModel.PageCountTimeChecked = this.PageCountBookTimeChecked;
            /******************************/
            viewModel.BookPriceVisible = true;
            viewModel.BookPriceChecked = this.BookPriceChecked;
            /******************************/
            viewModel.AscendingChecked = this.AscendingChecked;
            viewModel.DescendingChecked = this.DescendingChecked;

            return viewModel;
        }
    }
}
