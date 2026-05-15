// <copyright file="ExistingBooksViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Groupings
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Enums;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Book;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// ExistingBooksViewModel class.
    /// </summary>
    public partial class ExistingBooksViewModel : BookListBaseViewModel
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

        /********************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="ExistingBooksViewModel"/> class.
        /// </summary>
        /// <param name="selected">Selected grouping object.</param>
        /// <param name="view">View related to view model.</param>
        /// <param name="previousViewModel">Previous view model to return to.</param>
        public ExistingBooksViewModel(object selected, ContentPage view, object? previousViewModel)
        {
            this.View = view;
            this.SelectedObject = selected;
            this.PreviousViewModel = previousViewModel;
            this.SetSelectedObjectName();
            this.CollectionViewHeight = DeviceHeight;
            this.SetInfoText();
            this.ViewTitle = AppStringResources.ExistingBooks_Object.Replace("Object", this.SelectedObjectName);
            this.SetRefreshView(true);

            this.SetFilterPopupDefaults();
            this.SetSortPopupDefaults();
        }

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static bool RefreshView { get; set; }

        /// <summary>
        /// Gets or sets the selected grouping object.
        /// </summary>
        private object? SelectedObject { get; set; }

        /// <summary>
        /// Gets or sets the selected grouping object name.
        /// </summary>
        private string? SelectedObjectName { get; set; }

        /********************************************************/

        /// <summary>
        /// Search the list based on the book title.
        /// </summary>
        /// <param name="input">Input string to find.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookSearchOnTitle(string? input)
        {
            this.SearchString = input;

            (this.FilteredBookList, this.FilteredBooksCount, this.TotalBooksString) = await this.BookSearch(this.HiddenFilteredBookList, this.TotalBooksCount);
        }

        /// <summary>
        /// Add selected book to selected grouping.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ExistingBooksSelectionChanged()
        {
            if (this.SelectedBook != null)
            {
                var title = $"{AppStringResources.AddBookToGrouping_Question.Replace("grouping", this.ViewTitle)}";
                var answer = await this.DisplayMessage(title, title, null, null);

                if (answer)
                {
                    await this.AddBookToGrouping();
                    this.SelectedBook = null;
                }
                else
                {
                    await this.CanceledAction();
                    this.SelectedBook = null;
                }
            }
        }

        /********************************************************/

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        public async Task SetList(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.FullBookList = null;

            if (this.SelectedObject is CollectionModel)
            {
                await this.SetCollectionList(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
            }

            if (this.SelectedObject is GenreModel)
            {
                await this.SetGenreList(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
            }

            if (this.SelectedObject is SeriesModel)
            {
                await this.SetSeriesList(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
            }

            if (this.SelectedObject is AuthorModel)
            {
                await this.SetAuthorList(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
            }

            if (this.SelectedObject is LocationModel)
            {
                await this.SetLocationList(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
            }

            if (this.SelectedObject is string)
            {
                if (!string.IsNullOrEmpty(this.SelectedObjectName) &&
                    this.SelectedObjectName.Equals(AppStringResources.BooksLoanedOut))
                {
                    await this.SetLoanedOutList(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
                }

                if (!string.IsNullOrEmpty(this.SelectedObjectName) &&
                    this.SelectedObjectName.Equals(AppStringResources.BooksBorrowed))
                {
                    await this.SetBorrowedList(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
                }
            }
        }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetViewModelData()
        {
            if (!RefreshView)
            {
                return;
            }

            this.SetRefreshView(false);

            await this.SetIsBusyTrue(true);

            try
            {
                this.GetPreferences();

                await this.SetList(
                    DevicePreferences.ShowHiddenBooksValue,
                    DevicePreferences.ShowAudiobooksValue,
                    DevicePreferences.ShoweBooksValue,
                    DevicePreferences.ShowHardcoversValue,
                    DevicePreferences.ShowPaperbacksValue);

                (this.TotalBooksCount,
                    this.FilteredBooksCount,
                    this.TotalBooksString,
                    this.ShowCollectionViewFooter,
                    this.FilteredBookList,
                    this.BookPublisherList,
                    this.BookLanguageList,
                    this.BookPublishYearList,
                    this.BookAuthorList) = await this.SetViewModelData(this.HiddenFilteredBookList);

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                await this.ViewModelCatch(ex);
            }
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public override bool GetPreferences()
        {
            this.BookAuthorOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.AuthorFilterSelection}", this.BookAuthorOptionDefault /* Default */);
            this.FavoritesOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.FavoriteFilterSelection}", this.FavoriteOptionDefault /* Default */);
            this.BookFormatOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.FormatFilterSelection}", this.BookFormatOptionDefault /* Default */);
            this.BookPublisherOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.PublisherFilterSelection}", this.BookPublisherOptionDefault /* Default */);
            this.BookPublishYearOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.PublishYearFilterSelection}", this.BookPublishYearOptionDefault /* Default */);
            this.BookLanguageOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.LanguageFilterSelection}", this.BookLanguageOptionDefault /* Default */);
            this.BookRatingOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.RatingFilterSelection}", this.BookRatingOptionDefault /* Default */);
            this.BookCoverOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookCoverFilterSelection}", this.BookCoverOptionDefault /* Default */);

            this.BookTitleChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookTitleSortSelection}", (bool)this.BookTitleCheckedDefault! /* Default */);
            this.BookReadingDateChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookReadingDateSortSelection}", (bool)this.BookReadingDateCheckedDefault! /* Default */);
            this.BookReadPercentageChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookReadPercentageSortSelection}", (bool)this.BookReadPercentageCheckedDefault! /* Default */);
            this.BookPublisherChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookPublisherSortSelection}", (bool)this.BookPublisherCheckedDefault! /* Default */);
            this.BookPublishYearChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookPublishYearSortSelection}", (bool)this.BookPublishYearCheckedDefault! /* Default */);
            this.AuthorLastNameChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.AuthorLastNameSortSelection}", (bool)this.AuthorLastNameCheckedDefault! /* Default */);
            this.BookFormatChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookFormatSortSelection}", (bool)this.BookFormatCheckedDefault! /* Default */);
            this.PageCountBookTimeChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.PageCountBookTimeSortSelection}", (bool)this.PageCountBookTimeCheckedDefault! /* Default */);
            this.BookPriceChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookPriceSortSelection}", (bool)this.BookPriceCheckedDefault! /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.AscendingSortSelection}", this.AscendingCheckedDefault /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.DescendingSortSelection}", this.DescendingCheckedDefault /* Default */);

            return DevicePreferences.ShowHiddenBooksValue;
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
            viewModel.FavoriteVisible = DevicePreferences.FavoritesShowValue;
            viewModel.FavoriteOption = this.FavoritesOption;
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
            viewModel.RatingVisible = DevicePreferences.RatingsShowValue;
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
            viewModel.SetFormatPicker(
                this.BookFormats,
                DevicePreferences.ShowAudiobooksValue,
                DevicePreferences.ShoweBooksValue,
                DevicePreferences.ShowHardcoversValue,
                DevicePreferences.ShowPaperbacksValue);
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

        /// <summary>
        /// Set whether to refresh view or not.
        /// </summary>
        /// <param name="value">Value to change to.</param>
        public override void SetRefreshView(bool value)
        {
            RefreshView = value;
        }

        /********************************************************/

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        private async Task SetCollectionList(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksWithoutACollectionList(showHiddenBooks);

            this.FilterBooks(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
        }

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        private async Task SetGenreList(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksWithoutAGenreList(showHiddenBooks);

            this.FilterBooks(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
        }

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        private async Task SetSeriesList(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksWithoutASeriesList(showHiddenBooks);

            this.FilterBooks(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
        }

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        private async Task SetAuthorList(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            var author = (AuthorModel?)this.SelectedObject;

            if (this.FullBookList == null && author != null)
            {
                this.FullBookList = await FillLists.GetAllBooksWithoutAuthorList(author.ReverseFullName, showHiddenBooks);
            }

            this.FilterBooks(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
        }

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        private async Task SetLocationList(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksWithoutALocationList(showHiddenBooks);

            this.FilterBooks(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
        }

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        private async Task SetLoanedOutList(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksNotLoanedOutList(showHiddenBooks);

            this.FilterBooks(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
        }

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        /// <returns>A task.</returns>
        private async Task SetBorrowedList(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksNotBorrowedList(showHiddenBooks);

            this.FilterBooks(showHiddenBooks, showAudiobooks, showEbooks, showHardcovers, showPaperbacks);
        }

        private async Task AddBookToGrouping()
        {
            try
            {
                await this.SetIsBusyTrue();

                if (this.SelectedBook != null)
                {
                    if (this.SelectedObject is string)
                    {
                        var view = new BookEditView(this.SelectedBook, $"{this.SelectedBook.BookTitle}", false, null, this.PreviousViewModel);
                        await Shell.Current.Navigation.PushAsync(view);
                        this.SetRefreshView(true);
                    }
                    else
                    {
                        this.SelectedBook.BookCollectionGuid = this.SelectedObject is CollectionModel ? ((CollectionModel?)this.SelectedObject)?.CollectionGuid : this.SelectedBook.BookCollectionGuid;

                        this.SelectedBook.BookGenreGuid = this.SelectedObject is GenreModel ? ((GenreModel?)this.SelectedObject)?.GenreGuid : this.SelectedBook.BookGenreGuid;

                        this.SelectedBook.BookSeriesGuid = this.SelectedObject is SeriesModel ? ((SeriesModel?)this.SelectedObject)?.SeriesGuid : this.SelectedBook.BookSeriesGuid;

                        if (this.SelectedObject is AuthorModel)
                        {
                            var author = (AuthorModel?)this.SelectedObject;

                            this.SelectedBook.SelectedAuthors ??= [];
                            this.SelectedBook.SelectedAuthors.Add(author);

                            await Database.AddAuthorToBookAsync(author?.AuthorGuid, this.SelectedBook.BookGuid);

                            await this.SelectedBook.SetAuthorListStringFromDatabase();
                        }

                        this.SelectedBook.BookLocationGuid = this.SelectedObject is LocationModel ? ((LocationModel?)this.SelectedObject)?.LocationGuid : this.SelectedBook.BookLocationGuid;

                        await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedBook));

                        await this.RemoveFromStaticList(this.SelectedBook);
                        await AddToStaticList(this.SelectedBook, this.PreviousViewModel);

                        var view = new BookMainView(this.SelectedBook, $"{this.SelectedBook.BookTitle}", this.PreviousViewModel);
                        await Shell.Current.Navigation.PushAsync(view);

                        await this.DisplayMessage($"{AppStringResources.BookHasBeenAddedToGrouping.Replace("Book", this.SelectedBook.BookTitle).Replace("grouping", this.SelectedObjectName)}", null);
                    }
                }

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                await this.ViewModelCatch(ex);
                this.SetRefreshView(false);
            }
        }

        private void SetSelectedObjectName()
        {
            this.SelectedObjectName = this.SelectedObject is CollectionModel ? ((CollectionModel?)this.SelectedObject)?.CollectionName : this.SelectedObjectName;

            this.SelectedObjectName = this.SelectedObject is GenreModel ? ((GenreModel?)this.SelectedObject)?.GenreName : this.SelectedObjectName;

            this.SelectedObjectName = this.SelectedObject is SeriesModel ? ((SeriesModel?)this.SelectedObject)?.SeriesName : this.SelectedObjectName;

            this.SelectedObjectName = this.SelectedObject is AuthorModel ? ((AuthorModel?)this.SelectedObject)?.FullName : this.SelectedObjectName;

            this.SelectedObjectName = this.SelectedObject is LocationModel ? ((LocationModel?)this.SelectedObject)?.LocationName : this.SelectedObjectName;

            if (this.SelectedObject is string name)
            {
                this.SelectedObjectName = name;
            }
        }

        private void SetInfoText()
        {
            if (this.SelectedObject is string name)
            {
                this.InfoText = $"{AppStringResources.ExistingBooksView_InfoText1.Replace("grouping", this.SelectedObjectName)}";
            }
            else
            {
                this.InfoText = $"{AppStringResources.ExistingBooksView_InfoText.Replace("grouping", this.SelectedObjectName)}";
            }
        }

        private void FilterBooks(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.HiddenFilteredBookList = showHiddenBooks ? this.FullBookList : this.FullBookList!.Where(x => !x.HideBook).ToObservableCollection();

            this.HiddenFilteredBookList = showAudiobooks ? this.HiddenFilteredBookList : this.HiddenFilteredBookList!.Where(x => !x.BookFormat!.Equals(AppStringResources.Audiobook)).ToObservableCollection();

            this.HiddenFilteredBookList = showEbooks ? this.HiddenFilteredBookList : this.HiddenFilteredBookList!.Where(x => !x.BookFormat!.Equals(AppStringResources.eBook)).ToObservableCollection();

            this.HiddenFilteredBookList = showHardcovers ? this.HiddenFilteredBookList : this.HiddenFilteredBookList!.Where(x => !x.BookFormat!.Equals(AppStringResources.Hardcover)).ToObservableCollection();

            this.HiddenFilteredBookList = showPaperbacks ? this.HiddenFilteredBookList : this.HiddenFilteredBookList!.Where(x => !x.BookFormat!.Equals(AppStringResources.Paperback)).ToObservableCollection();
        }

        private async Task RemoveFromStaticList(BookModel book)
        {
            if (this.FullBookList != null)
            {
                RefreshView = await RemoveBookFromStaticList(book, this.FullBookList, this.FilteredBookList);
            }
        }

        private void SetFilterPopupDefaults()
        {
            this.BookAuthorOptionDefault = AppStringResources.AllAuthors;
            this.FavoriteOptionDefault = AppStringResources.Both;
            this.BookFormatOptionDefault = AppStringResources.AllFormats;
            this.BookPublisherOptionDefault = AppStringResources.AllPublishers;
            this.BookPublishYearOptionDefault = AppStringResources.AllPublishYears;
            this.BookLanguageOptionDefault = AppStringResources.AllLanguages;
            this.BookRatingOptionDefault = AppStringResources.AllRatings;
            this.BookCoverOptionDefault = AppStringResources.Both;
        }

        private void SetSortPopupDefaults()
        {
            this.BookTitleCheckedDefault = true;
            this.BookReadingDateCheckedDefault = false;
            this.BookReadPercentageCheckedDefault = false;
            this.BookPublisherCheckedDefault = false;
            this.BookPublishYearCheckedDefault = false;
            this.AuthorLastNameCheckedDefault = false;
            this.BookFormatCheckedDefault = false;
            this.PageCountBookTimeCheckedDefault = false;
            this.BookPriceCheckedDefault = false;

            this.AscendingCheckedDefault = true;
            this.DescendingCheckedDefault = false;
        }
    }
}
