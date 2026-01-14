// <copyright file="WishListViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Popups;
using BookCollector.Views.WishListBook;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Main
{
    public partial class WishListViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<WishlistBookModel>? fullWishlistBookList;

        [ObservableProperty]
        public static ObservableCollection<WishlistBookModel>? filteredWishlistBookList;

        [ObservableProperty]
        public WishlistBookModel? selectedWishlistBook;

        [ObservableProperty]
        public ObservableCollection<string>? bookAuthorList;

        [ObservableProperty]
        public ObservableCollection<string>? bookLocationList;

        [ObservableProperty]
        public ObservableCollection<string>? bookSeriesList;

        [ObservableProperty]
        public static int totalBooksCount;

        [ObservableProperty]
        public static int filteredBooksCount;

        public static bool RefreshView { get; set; }

        public WishListViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight - this.SingleMenuBar;
            this.InfoText = AppStringResources.WishListView_InfoText;
            this.ViewTitle = AppStringResources.Wishlist;
            RefreshView = true;
        }

        public string? BookAuthorOption { get; set; }

        public string? BookLocationOption { get; set; }

        public string? BookSeriesOption { get; set; }

        public bool ShowHiddenWishlistBooks { get; set; }

        public static async Task SetList(bool showHiddenBooks)
        {
            if (fullWishlistBookList == null)
            {
                fullWishlistBookList = await FillLists.GetBookWishList(showHiddenBooks);
            }
        }

        public async Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await SetList(this.ShowHiddenWishlistBooks);

                    if (fullWishlistBookList != null)
                    {
                        this.TotalBooksCount = fullWishlistBookList.Count;

                        var bookPublishers = FillLists.GetAllPublishersInWishlistBookList(fullWishlistBookList);
                        var bookLanguages = FillLists.GetAllLanguagesInWishlistBookList(fullWishlistBookList);
                        var bookPublishYears = FillLists.GetAllPublisherYearsInWishlistBookList(fullWishlistBookList);
                        var authors = FillLists.GetAllAuthorsInWishlistBookList(fullWishlistBookList);
                        var locations = FillLists.GetAllLocationsInWishlistBookList(fullWishlistBookList);
                        var series = FillLists.GetAllSeriesInWishlistBookList(fullWishlistBookList);
                        var filteredList = FilterLists.FilterWishlistBookList(
                                fullWishlistBookList,
                                this.BookFormatOption,
                                this.BookPublisherOption,
                                this.BookLanguageOption,
                                this.BookPublishYearOption,
                                this.BookAuthorOption,
                                this.BookLocationOption,
                                this.BookSeriesOption,
                                this.Searchstring);

                        await Task.WhenAll(filteredList);

                        this.FilteredWishlistBookList = filteredList.Result;

                        await Task.WhenAll(fullWishlistBookList.Select(x => x.SetCoverDisplay()));
                        await Task.WhenAll(fullWishlistBookList.Select(x => x.SetBookTotalTime()));

                        if (this.FilteredWishlistBookList != null)
                        {
                            var sortList = SortLists.SortWishlistBookList(
                                    fullWishlistBookList,
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

                            this.FilteredBooksCount = this.FilteredWishlistBookList.Count;

                            this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                            this.ShowCollectionViewFooter = this.FilteredBooksCount > 0;

                            await Task.WhenAll(sortList);

                            this.FullWishlistBookList = sortList.Result;
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
                    await DisplayMessage("Error!", ex.Message);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
        }

        [RelayCommand]
        public async void BookSearchOnTitle(string? input)
        {
            this.SetIsBusyTrue();

            this.Searchstring = input;

            if (this.FilteredWishlistBookList != null && this.FullWishlistBookList != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    this.FilteredWishlistBookList = FilterLists.FilterOnSearchString(this.FilteredWishlistBookList, input);
                }
                else
                {
                    this.FilteredWishlistBookList = await FilterLists.FilterWishlistBookList(
                                this.FullWishlistBookList,
                                this.BookFormatOption,
                                this.BookPublisherOption,
                                this.BookLanguageOption,
                                this.BookPublishYearOption,
                                this.BookAuthorOption,
                                this.BookLocationOption,
                                this.BookSeriesOption,
                                this.Searchstring);
                }

                this.FilteredBooksCount = this.FilteredWishlistBookList != null ? this.FilteredWishlistBookList.Count : 0;

                this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);
            }

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

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

        [RelayCommand]
        public async Task AddWishListBook()
        {
            this.SetIsBusyTrue();

            var view = new WishListBookEditView(new WishlistBookModel(), $"{AppStringResources.AddNewBook}");

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

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

            //File.Delete(filePath);

            this.SetIsBusyFalse();
        }

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
                        var authorList = await ParseOutAuthorsFromstring(book.AuthorListString);

                        text = $"{AppStringResources.BookTitleByAuthorName.Replace("Book Title", book.BookTitle).Replace("Author Name", authorList[0].FullName)}";

                        if (authorList.Count > 1)
                        {
                            text += $", {AppStringResources.EtAl}";
                        }
                    }
                    else
                    {
                        text = $"{AppStringResources.BookTitle.Replace("Book Title", book.BookTitle)}";
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
