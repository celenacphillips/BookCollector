// <copyright file="WishListViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Main
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.Enums;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.WishListBook;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// WishListViewModel class.
    /// </summary>
    public partial class WishListViewModel : BookListBaseViewModel
    {
        /// <summary>
        /// Gets or sets the full book list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<WishlistBookModel>? fullWishlistBookList;

        /// <summary>
        /// Gets or sets the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<WishlistBookModel>? hiddenFilteredWishlistBookList;

        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<WishlistBookModel>? filteredWishlistBookList;

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
        /// Gets or sets the selected book.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public WishlistBookModel? selectedWishlistBook;

        /// <summary>
        /// Gets or sets the book location list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<string>? bookLocationList;

        /// <summary>
        /// Gets or sets the book series list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<string>? bookSeriesList;

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
        /// Initializes a new instance of the <see cref="WishListViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public WishListViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = DeviceHeight;
            this.InfoText = AppStringResources.WishListView_InfoText;
            this.ViewTitle = AppStringResources.Wishlist;
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
        /// Gets or sets a value indicating whether to show hidden wishlist books or not.
        /// </summary>
        public bool ShowHiddenWishlistBooks { get; set; }

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
        public static async Task SetList(
            bool showHiddenBooks,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            fullWishlistBookList ??= await FillLists.GetBookWishList();

            hiddenFilteredWishlistBookList = showHiddenBooks ? fullWishlistBookList : fullWishlistBookList!.Where(x => !x.HideBook).ToObservableCollection();

            hiddenFilteredWishlistBookList = showAudiobooks ? hiddenFilteredWishlistBookList : hiddenFilteredWishlistBookList!.Where(x => !x.BookFormat!.Equals(AppStringResources.Audiobook)).ToObservableCollection();

            hiddenFilteredWishlistBookList = showEbooks ? hiddenFilteredWishlistBookList : hiddenFilteredWishlistBookList!.Where(x => !x.BookFormat!.Equals(AppStringResources.eBook)).ToObservableCollection();

            hiddenFilteredWishlistBookList = showHardcovers ? hiddenFilteredWishlistBookList : hiddenFilteredWishlistBookList!.Where(x => !x.BookFormat!.Equals(AppStringResources.Hardcover)).ToObservableCollection();

            hiddenFilteredWishlistBookList = showPaperbacks ? hiddenFilteredWishlistBookList : hiddenFilteredWishlistBookList!.Where(x => !x.BookFormat!.Equals(AppStringResources.Paperback)).ToObservableCollection();
        }

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

            (this.FilteredWishlistBookList, this.FilteredBooksCount, this.TotalBooksString) = await this.BookSearch(this.HiddenFilteredWishlistBookList, this.TotalBooksCount);
        }

        /// <summary>
        /// Navigate to the wishlist book main view when book is selected.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task WishListBookSelectionChanged()
        {
            if (this.SelectedWishlistBook != null && !string.IsNullOrEmpty(this.SelectedWishlistBook.BookTitle))
            {
                var view = new WishListBookMainView(this.SelectedWishlistBook, this.SelectedWishlistBook.BookTitle);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedWishlistBook = null;
            }
        }

        /// <summary>
        /// Create a new wishlist book and navigate to the wishlist book edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddWishListBook()
        {
            await this.SetIsBusyTrue();

            var view = new WishListBookEditView(new WishlistBookModel(), $"{AppStringResources.AddNewBook}");

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Share wishlist information.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ShareList()
        {
            await this.SetIsBusyTrue();

            var data = await this.CreateShareWishList();
            var filePath = $"{FileSystem.CacheDirectory}/{AppStringResources.Wishlist}.txt";
            var title = AppStringResources.Wishlist;

            File.WriteAllLines(filePath, data);

            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = title,
                File = new ShareFile(filePath),
            });

            // File.Delete(filePath);
            this.SetIsBusyFalse();
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

                await SetList(this.ShowHiddenWishlistBooks, DevicePreferences.ShowAudiobooksValue, DevicePreferences.ShoweBooksValue, DevicePreferences.ShowHardcoversValue, DevicePreferences.ShowPaperbacksValue);

                (this.TotalBooksCount,
                    this.FilteredBooksCount,
                    this.TotalBooksString,
                    this.ShowCollectionViewFooter,
                    this.FilteredWishlistBookList,
                    this.BookPublisherList,
                    this.BookLanguageList,
                    this.BookPublishYearList,
                    this.BookAuthorList,
                    this.BookLocationList,
                    this.BookSeriesList) = await this.SetViewModelData(this.HiddenFilteredWishlistBookList);

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
            this.ShowHiddenWishlistBooks = DevicePreferences.ShowHiddenWishlistBooksValue;

            DevicePreferences.ShowAudiobooksValue = DevicePreferences.ShowAudiobooksValue;
            DevicePreferences.ShoweBooksValue = DevicePreferences.ShoweBooksValue;
            DevicePreferences.ShowHardcoversValue = DevicePreferences.ShowHardcoversValue;
            DevicePreferences.ShowPaperbacksValue = DevicePreferences.ShowPaperbacksValue;

            this.BookFormatOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.FormatFilterSelection}", this.BookFormatOptionDefault /* Default */);
            this.BookPublisherOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.PublisherFilterSelection}", this.BookPublisherOptionDefault /* Default */);
            this.BookPublishYearOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.PublishYearFilterSelection}", this.BookPublishYearOptionDefault /* Default */);
            this.BookLanguageOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.LanguageFilterSelection}", this.BookLanguageOptionDefault /* Default */);
            this.BookAuthorOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.AuthorFilterSelection}", this.BookAuthorOptionDefault /* Default */);
            this.BookSeriesOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.SeriesFilterSelection}", this.BookSeriesOptionDefault /* Default */);
            this.BookLocationOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.LocationFilterSelection}", this.BookLocationOptionDefault /* Default */);
            this.BookCoverOption = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookCoverFilterSelection}", this.BookCoverOptionDefault /* Default */);

            this.BookTitleChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookTitleSortSelection}", (bool)this.BookTitleCheckedDefault! /* Default */);
            this.BookPublisherChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookPublisherSortSelection}", (bool)this.BookPublisherCheckedDefault! /* Default */);
            this.BookPublishYearChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookPublishYearSortSelection}", (bool)this.BookPublishYearCheckedDefault! /* Default */);
            this.AuthorLastNameChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.AuthorLastNameSortSelection}", (bool)this.AuthorLastNameCheckedDefault! /* Default */);
            this.BookFormatChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookFormatSortSelection}", (bool)this.BookFormatCheckedDefault! /* Default */);
            this.PageCountBookTimeChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.PageCountBookTimeSortSelection}", (bool)this.PageCountBookTimeCheckedDefault! /* Default */);
            this.BookPriceChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.BookPriceSortSelection}", (bool)this.BookPriceCheckedDefault! /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.AscendingSortSelection}", this.AscendingCheckedDefault /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_{DevicePreferences.DescendingSortSelection}", this.DescendingCheckedDefault /* Default */);

            return this.ShowHiddenWishlistBooks;
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
            viewModel.SeriesVisible = true;
            viewModel.SeriesOption = this.BookSeriesOption;
            /******************************/
            viewModel.LocationVisible = true;
            viewModel.LocationOption = this.BookLocationOption;
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
            viewModel.SetFavoritePicker();
            viewModel.SetFormatPicker(this.BookFormats, DevicePreferences.ShowAudiobooksValue, DevicePreferences.ShoweBooksValue, DevicePreferences.ShowHardcoversValue, DevicePreferences.ShowPaperbacksValue);
            viewModel.SetPublisherPicker(this.BookPublisherList);
            viewModel.SetPublishYearPicker(this.BookPublishYearList);
            viewModel.SetLanguagePicker(this.BookLanguageList);
            viewModel.SetAuthorPicker(this.BookAuthorList);
            viewModel.SetLocationPicker(this.BookLocationList);
            viewModel.SetSeriesPicker(this.BookSeriesList);
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

        private async Task<List<string>> CreateShareWishList()
        {
            var wishList = new List<string>();
            if (this.FilteredWishlistBookList != null)
            {
                foreach (var book in this.FilteredWishlistBookList)
                {
                    string? text;

                    if (!string.IsNullOrEmpty(book.AuthorListString))
                    {
                        var authorList = await StringManipulation.SplitAuthorListStringIntoAuthorList(book.AuthorListString);

                        text = $"{AppStringResources.BookTitleByAuthorName.Replace("Book Title", book.BookTitle).Replace("Author Name", authorList[0].FullName)}";

                        if (authorList.Count > 1)
                        {
                            text += $", {AppStringResources.EtAl}";
                        }
                    }
                    else
                    {
                        text = $"{AppStringResources.BookTitle_Replace.Replace("Book Title", book.BookTitle)}";
                    }

                    if (!string.IsNullOrEmpty(book.BookWhereToBuy))
                    {
                        text += $" [{book.BookWhereToBuy}]";
                    }

                    if (!string.IsNullOrEmpty(book.BookURL))
                    {
                        text += $" ({book.BookURL})";
                    }

                    wishList.Add(text);
                }
            }

            return wishList;
        }

        private void SetFilterPopupDefaults()
        {
            this.BookFormatOptionDefault = AppStringResources.AllFormats;
            this.BookPublisherOptionDefault = AppStringResources.AllPublishers;
            this.BookPublishYearOptionDefault = AppStringResources.AllPublishYears;
            this.BookLanguageOptionDefault = AppStringResources.AllLanguages;
            this.BookAuthorOptionDefault = AppStringResources.AllAuthors;
            this.BookSeriesOptionDefault = AppStringResources.AllSeries;
            this.BookLocationOptionDefault = AppStringResources.AllLocations;
            this.BookCoverOptionDefault = AppStringResources.Both;
        }

        private void SetSortPopupDefaults()
        {
            this.BookTitleCheckedDefault = true;
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
