// <copyright file="WishListViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Main
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Popups;
    using BookCollector.Views.WishListBook;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Maui.Extensions;
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
        /// <returns>A task.</returns>
        public static async Task SetList(bool showHiddenBooks)
        {
            fullWishlistBookList ??= await FillLists.GetBookWishList();

            hiddenFilteredWishlistBookList = SetHiddenFilteredList<WishlistBookModel>(fullWishlistBookList!, showHiddenBooks).ToObservableCollection();
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
            this.SetIsBusyTrue();

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
            this.SetIsBusyTrue();

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
            if (RefreshView)
            {
                try
                {
                    this.GetPreferences();

                    await SetList(this.ShowHiddenWishlistBooks);

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
                }
                catch (Exception ex)
                {
                    await this.ViewModelCatch(ex);
                    this.SetRefreshView(false);
                }
            }
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public override bool GetPreferences()
        {
            this.ShowHiddenWishlistBooks = Preferences.Get("HiddenWishlistBooksOn", true /* Default */);

            this.BookFormatOption = Preferences.Get($"{this.ViewTitle}_FormatSelection", AppStringResources.AllFormats /* Default */);
            this.BookPublisherOption = Preferences.Get($"{this.ViewTitle}_PublisherSelection", AppStringResources.AllPublishers /* Default */);
            this.BookPublishYearOption = Preferences.Get($"{this.ViewTitle}_PublishYearSelection", AppStringResources.AllPublishYears /* Default */);
            this.BookLanguageOption = Preferences.Get($"{this.ViewTitle}_LanguageSelection", AppStringResources.AllLanguages /* Default */);
            this.BookAuthorOption = Preferences.Get($"{this.ViewTitle}_AuthorSelection", AppStringResources.AllAuthors /* Default */);
            this.BookSeriesOption = Preferences.Get($"{this.ViewTitle}_SeriesSelection", AppStringResources.AllSeries /* Default */);
            this.BookLocationOption = Preferences.Get($"{this.ViewTitle}_LocationSelection", AppStringResources.AllLocations /* Default */);
            this.BookCoverOption = Preferences.Get($"{this.ViewTitle}_BookCoverSelection", AppStringResources.Both /* Default */);

            this.BookTitleChecked = Preferences.Get($"{this.ViewTitle}_BookTitleSelection", true /* Default */);
            this.BookPublisherChecked = Preferences.Get($"{this.ViewTitle}_BookPublisherSelection", false /* Default */);
            this.BookPublishYearChecked = Preferences.Get($"{this.ViewTitle}_BookPublishYearSelection", false /* Default */);
            this.AuthorLastNameChecked = Preferences.Get($"{this.ViewTitle}_AuthorLastNameSelection", false /* Default */);
            this.BookFormatChecked = Preferences.Get($"{this.ViewTitle}_BookFormatSelection", false /* Default */);
            this.PageCountBookTimeChecked = Preferences.Get($"{this.ViewTitle}_PageCountBookTimeSelection", false /* Default */);
            this.BookPriceChecked = Preferences.Get($"{this.ViewTitle}_BookPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);

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
            viewModel.SetFormatPicker(this.BookFormats);
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
    }
}
