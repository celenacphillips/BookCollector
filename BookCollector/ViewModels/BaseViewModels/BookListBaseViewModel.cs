// <copyright file="BookListBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
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
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    var showHidden = this.GetPreferences();

                    await this.SetList(showHidden);

                    var listNotNull = this.ListNullCheck();

                    if (listNotNull)
                    {
                        await this.SetListData();

                        await this.SetFilters();

                        await this.SetSorts();
                    }

                    this.SetViewStrings();

                    this.SetIsBusyFalse();
                    RefreshView = false;
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
                    RefreshView = false;
                }
            }
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public abstract bool GetPreferences();

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
        public abstract bool ListNullCheck();

        /// <summary>
        /// Iterate through the list and set necessary data.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetListData();

        /// <summary>
        /// Find filters for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetFilters();

        /// <summary>
        /// Find sort values for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetSorts();

        /// <summary>
        /// Set data for view.
        /// </summary>
        public abstract void SetViewStrings();

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
    }
}
