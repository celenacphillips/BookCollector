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
    public partial class WishListViewModel : BookBaseViewModel
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
        public static ObservableCollection<WishlistBookModel>? filteredWishlistBookList1;

        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<WishlistBookModel>? filteredWishlistBookList2;

        /// <summary>
        /// Gets or sets the total count of books, based on the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static int totalBooksCount;

        /// <summary>
        /// Gets or sets the total count of books, based on the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static int filteredBooksCount;

        /// <summary>
        /// Gets or sets the selected book.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public WishlistBookModel? selectedWishlistBook;

        /// <summary>
        /// Gets or sets the book author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<string>? bookAuthorList;

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
        /// Initializes a new instance of the <see cref="WishListViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public WishListViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight;
            this.InfoText = AppStringResources.WishListView_InfoText;
            this.ViewTitle = AppStringResources.Wishlist;
            RefreshView = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static new bool RefreshView { get; set; }

        /// <summary>
        /// Gets or sets the saved book author filter option.
        /// </summary>
        public string? BookAuthorOption { get; set; }

        /// <summary>
        /// Gets or sets the saved book location filter option.
        /// </summary>
        public string? BookLocationOption { get; set; }

        /// <summary>
        /// Gets or sets the saved book series filter option.
        /// </summary>
        public string? BookSeriesOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden wishlist books or not.
        /// </summary>
        public bool ShowHiddenWishlistBooks { get; set; }

        /// <summary>
        /// Set the first filtered list based on the full book list and the show hidden books preference.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
        public static async Task SetList(bool showHiddenBooks)
        {
            fullWishlistBookList ??= await FillLists.GetBookWishList();

            filteredWishlistBookList1 = BaseViewModel.SetList<WishlistBookModel>(fullWishlistBookList!, showHiddenBooks).ToObservableCollection();
        }

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

                    this.GetPreferences();

                    await SetList(this.ShowHiddenWishlistBooks);

                    if (this.FilteredWishlistBookList1 != null)
                    {
                        this.TotalBooksCount = this.FilteredWishlistBookList1.Count;

                        var bookPublishers = FillLists.GetAllPublishersInWishlistBookList(this.FilteredWishlistBookList1);
                        var bookLanguages = FillLists.GetAllLanguagesInWishlistBookList(this.FilteredWishlistBookList1);
                        var bookPublishYears = FillLists.GetAllPublisherYearsInWishlistBookList(this.FilteredWishlistBookList1);
                        var authors = FillLists.GetAllAuthorsInWishlistBookList(this.FilteredWishlistBookList1);
                        var locations = FillLists.GetAllLocationsInWishlistBookList(this.FilteredWishlistBookList1);
                        var series = FillLists.GetAllSeriesInWishlistBookList(this.FilteredWishlistBookList1);
                        var filteredList = FilterLists.FilterWishlistBookList(
                                this.FilteredWishlistBookList1,
                                this.BookFormatOption,
                                this.BookPublisherOption,
                                this.BookLanguageOption,
                                this.BookPublishYearOption,
                                this.BookAuthorOption,
                                this.BookLocationOption,
                                this.BookSeriesOption,
                                this.SearchString);

                        await Task.WhenAll(filteredList);

                        this.FilteredWishlistBookList2 = filteredList.Result;

                        await Task.WhenAll(this.FilteredWishlistBookList2.Select(x => x.SetCoverDisplay()));
                        await Task.WhenAll(this.FilteredWishlistBookList2.Select(x => x.SetBookTotalTime()));

                        if (this.FilteredWishlistBookList2 != null)
                        {
                            var sortList = SortLists.SortWishlistBookList(
                                    this.FilteredWishlistBookList2,
                                    this.BookTitleChecked,
                                    this.BookPublisherChecked,
                                    this.BookPublishYearChecked,
                                    this.AuthorLastNameChecked,
                                    this.BookFormatChecked,
                                    this.BookPriceChecked,
                                    this.PageCountBookTimeChecked,
                                    this.AscendingChecked,
                                    this.DescendingChecked);

                            this.FilteredBooksCount = this.FilteredWishlistBookList2.Count;

                            this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                            this.ShowCollectionViewFooter = this.FilteredBooksCount > 0;

                            await Task.WhenAll(sortList);

                            this.FilteredWishlistBookList2 = sortList.Result;
                        }

                        await Task.WhenAll(bookPublishers, bookLanguages, bookPublishYears, authors, locations, series);

                        this.BookPublisherList = bookPublishers.Result;
                        this.BookLanguageList = bookLanguages.Result;
                        this.BookPublishYearList = bookPublishYears.Result;
                        this.BookAuthorList = authors.Result;
                        this.BookLocationList = locations.Result;
                        this.BookSeriesList = series.Result;
                    }

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
        /// Search the list based on the book title.
        /// </summary>
        /// <param name="input">Input string to find.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookSearchOnTitle(string? input)
        {
            this.SearchString = input;

            if (this.FilteredWishlistBookList2 != null && this.FilteredWishlistBookList1 != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    this.FilteredWishlistBookList2 = FilterLists.FilterOnSearchString(this.FilteredWishlistBookList1, input);
                }
                else
                {
                    this.FilteredWishlistBookList2 = await FilterLists.FilterWishlistBookList(
                                this.FilteredWishlistBookList1,
                                this.BookFormatOption,
                                this.BookPublisherOption,
                                this.BookLanguageOption,
                                this.BookPublishYearOption,
                                this.BookAuthorOption,
                                this.BookLocationOption,
                                this.BookSeriesOption,
                                this.SearchString);
                }

                this.FilteredBooksCount = this.FilteredWishlistBookList2 != null ? this.FilteredWishlistBookList2.Count : 0;

                this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                var sortList = SortLists.SortWishlistBookList(
                                    this.FilteredWishlistBookList2!,
                                    this.BookTitleChecked,
                                    this.BookPublisherChecked,
                                    this.BookPublishYearChecked,
                                    this.AuthorLastNameChecked,
                                    this.BookFormatChecked,
                                    this.BookPriceChecked,
                                    this.PageCountBookTimeChecked,
                                    this.AscendingChecked,
                                    this.DescendingChecked);

                await Task.WhenAll(sortList);

                this.FilteredWishlistBookList2 = sortList.Result;
            }
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
                    FormatVisible = true,
                    FormatOption = this.BookFormatOption,
                    PublisherVisible = true,
                    PublisherOption = this.BookPublisherOption,
                    PublishYearVisible = true,
                    PublishYearOption = this.BookPublishYearOption,
                    LanguageVisible = true,
                    LanguageOption = this.BookLanguageOption,
                    SeriesVisible = true,
                    SeriesOption = this.BookSeriesOption,
                    LocationVisible = true,
                    LocationOption = this.BookLocationOption,
                };
                viewModel.SetFavoritePicker();
                viewModel.SetFormatPicker(this.BookFormats);
                viewModel.SetPublisherPicker(this.BookPublisherList);
                viewModel.SetPublishYearPicker(this.BookPublishYearList);
                viewModel.SetLanguagePicker(this.BookLanguageList);
                viewModel.SetAuthorPicker(this.BookAuthorList);
                viewModel.SetLocationPicker(this.BookLocationList);
                viewModel.SetSeriesPicker(this.BookSeriesList);

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
                    RefreshView = true;
                    await this.SetViewModelData();
                }
            }
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

        private async Task<List<string>> CreateShareWishList()
        {
            var wishList = new List<string>();
            if (this.FilteredWishlistBookList2 != null)
            {
                foreach (var book in this.FilteredWishlistBookList2)
                {
                    string? text;

                    if (!string.IsNullOrEmpty(book.AuthorListString))
                    {
                        var authorList = await ParseOutAuthorsFromstring(book.AuthorListString);

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

        private void GetPreferences()
        {
            this.ShowHiddenWishlistBooks = Preferences.Get("HiddenWishlistBooksOn", true /* Default */);

            this.BookFormatOption = Preferences.Get($"{this.ViewTitle}_FormatSelection", AppStringResources.AllFormats /* Default */);
            this.BookPublisherOption = Preferences.Get($"{this.ViewTitle}_PublisherSelection", AppStringResources.AllPublishers /* Default */);
            this.BookPublishYearOption = Preferences.Get($"{this.ViewTitle}_PublishYearSelection", AppStringResources.AllPublishYears /* Default */);
            this.BookLanguageOption = Preferences.Get($"{this.ViewTitle}_LanguageSelection", AppStringResources.AllLanguages /* Default */);
            this.BookAuthorOption = Preferences.Get($"{this.ViewTitle}_AuthorSelection", AppStringResources.AllAuthors /* Default */);
            this.BookSeriesOption = Preferences.Get($"{this.ViewTitle}_SeriesSelection", AppStringResources.AllSeries /* Default */);
            this.BookLocationOption = Preferences.Get($"{this.ViewTitle}_LocationSelection", AppStringResources.AllLocations /* Default */);

            this.BookTitleChecked = Preferences.Get($"{this.ViewTitle}_BookTitleSelection", true /* Default */);
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
