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
        public ObservableCollection<WishlistBookModel>? fullWishlistBookList;

        [ObservableProperty]
        public ObservableCollection<WishlistBookModel>? filteredWishlistBookList;

        [ObservableProperty]
        public WishlistBookModel? selectedWishlistBook;

        [ObservableProperty]
        public ObservableCollection<string>? bookAuthorList;

        [ObservableProperty]
        public ObservableCollection<string>? bookLocationList;

        [ObservableProperty]
        public ObservableCollection<string>? bookSeriesList;

        public WishListViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight - this.SingleMenuBar;
            this.InfoText = AppStringResources.WishListView_InfoText;
            this.ViewTitle = AppStringResources.Wishlist;
        }

        public string? BookAuthorOption { get; set; }

        public string? BookLocationOption { get; set; }

        public string? BookSeriesOption { get; set; }

        public bool ShowHiddenWishlistBooks { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                var fullList = FillLists.GetBookWishList(this.ShowHiddenWishlistBooks);

                await Task.WhenAll(fullList);

                this.FullWishlistBookList = fullList.Result;

                if (this.FullWishlistBookList != null)
                {
                    this.TotalBooksCount = this.FullWishlistBookList.Count;

                    var bookPublishers = FillLists.GetAllPublishersInWishlistBookList(this.FullWishlistBookList);
                    var bookLanguages = FillLists.GetAllLanguagesInWishlistBookList(this.FullWishlistBookList);
                    var bookPublishYears = FillLists.GetAllPublisherYearsInWishlistBookList(this.FullWishlistBookList);
                    var authors = FillLists.GetAllAuthorsInWishlistBookList(this.FullWishlistBookList);
                    var locations = FillLists.GetAllLocationsInWishlistBookList(this.FullWishlistBookList);
                    var series = FillLists.GetAllSeriesInWishlistBookList(this.FullWishlistBookList);
                    var filteredList = FilterLists.FilterWishlistBookList(
                            this.FullWishlistBookList,
                            this.BookFormatOption,
                            this.BookPublisherOption,
                            this.BookLanguageOption,
                            this.BookPublishYearOption,
                            this.BookAuthorOption,
                            this.BookLocationOption,
                            this.BookSeriesOption);

                    await Task.WhenAll(filteredList);

                    this.FilteredWishlistBookList = filteredList.Result;

                    if (this.FilteredWishlistBookList != null)
                    {
                        var sortList = SortLists.SortBookList(
                                this.FilteredBookList,
                                this.BookTitleChecked,
                                this.BookReadingDateChecked,
                                this.BookReadPercentageChecked,
                                this.BookPublisherChecked,
                                this.BookPublishYearChecked,
                                this.AuthorLastNameChecked,
                                this.BookFormatChecked,
                                this.BookPriceChecked,
                                this.PageCountChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

                        var loadDataTasks = new Task[]
                        {
                            Task.Run(() => this.FilteredBookList.ToList().ForEach(x => x.SetCoverDisplay())),
                            Task.Run(() => this.FilteredBookList.ToList().ForEach(x => x.SetReadingProgress())),
                        };

                        this.FilteredBooksCount = this.FilteredWishlistBookList.Count;

                        this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                        this.ShowCollectionViewFooter = this.FilteredBooksCount > 0;

                        await Task.WhenAll(sortList);
                        await Task.WhenAll(loadDataTasks);
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
            }
            catch (Exception ex)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
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
                this.SelectedBook = null;
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
                var viewModel = new FilterPopupViewModel(popup, this.ViewTitle)
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

                await this.View.ShowPopupAsync(popup);
                await this.SetViewModelData();
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
                    PageCountVisible = true,
                    PageCountChecked = this.PageCountChecked,
                    BookPriceVisible = true,
                    BookPriceChecked = this.BookPriceChecked,
                    AscendingChecked = this.AscendingChecked,
                    DescendingChecked = this.DescendingChecked,
                };

                popup.BindingContext = viewModel;

                await this.View.ShowPopupAsync(popup);
                await this.SetViewModelData();
            }
        }

        [RelayCommand]
        public async Task ShareList()
        {
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
            this.PageCountChecked = Preferences.Get($"{this.ViewTitle}_PageCountSelection", false /* Default */);
            this.BookPriceChecked = Preferences.Get($"{this.ViewTitle}_BookPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }
    }
}
