// <copyright file="AuthorMainViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

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
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Author
{
    public partial class AuthorMainViewModel : AuthorBaseViewModel
    {
        [ObservableProperty]
        public ObservableCollection<BookModel>? fullBookList;

        [ObservableProperty]
        public ObservableCollection<BookModel>? filteredBookList1;

        [ObservableProperty]
        public ObservableCollection<BookModel>? filteredBookList2;

        [ObservableProperty]
        public int totalBooksCount;

        [ObservableProperty]
        public int filteredBooksCount;

        public bool RefreshView { get; set; }

        public AuthorMainViewModel(AuthorModel author, ContentPage view)
        {
            this.View = view;
            this.SelectedAuthor = author;
            this.CollectionViewHeight = this.DeviceHeight - this.SingleMenuBar;
            this.InfoText = $"{AppStringResources.AuthorMainView_InfoText.Replace("author", $"{this.SelectedAuthor.FullName}")}";
            RefreshView = true;
        }

        public async Task SetList(bool showHiddenBooks)
        {
            if (this.FullBookList == null)
            {
                this.FullBookList = await FillLists.GetAllBooksInAuthorList(this.SelectedAuthor.AuthorGuid, ShowHiddenBook);
            }

            if (!showHiddenBooks)
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!.Where(x => !x.HideBook));
            }
            else
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!);
            }
        }

        public async Task SetViewModelData()
        {
            if (RefreshView && this.SelectedAuthor != null)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await this.SetList(ShowHiddenBook);

                    if (this.FilteredBookList1 != null)
                    {
                        this.TotalBooksCount = this.FilteredBookList1.Count;

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
                                this.Searchstring);

                        await Task.WhenAll(filteredList);

                        this.FilteredBookList2 = filteredList.Result;

                        if (this.FilteredBookList2 != null)
                        {
                            await Task.WhenAll(this.FilteredBookList2.Select(x => x.SetReadingProgress()));
                            await Task.WhenAll(this.FilteredBookList2.Select(x => x.SetAuthorListString()));
                            await Task.WhenAll(this.FilteredBookList2.Select(x => x.SetCoverDisplay()));
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

                        await Task.WhenAll(bookPublishers, bookLanguages, bookPublishYears);

                        this.BookPublisherList = bookPublishers.Result;
                        this.BookLanguageList = bookLanguages.Result;
                        this.BookPublishYearList = bookPublishYears.Result;
                    }

                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
        }

        [RelayCommand]
        public async void BookSearchOnTitle(string? input)
        {
            this.Searchstring = input;

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
                                this.Searchstring);
                }

                this.FilteredBooksCount = this.FilteredBookList2 != null ? this.FilteredBookList2.Count : 0;

                this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);
            }

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

            await Task.WhenAll(sortList);

            this.FilteredBookList2 = sortList.Result;
        }

        [RelayCommand]
        public async void Refresh()
        {
            this.SetRefreshTrue();
            RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task AddNewBook()
        {
            if (this.SelectedAuthor != null)
            {
                this.SetIsBusyTrue();

                var newBook = new BookModel()
                {
                    SelectedAuthors = [],
                };
                newBook.SelectedAuthors.Add(this.SelectedAuthor);

                var view = new BookEditView(newBook, $"{AppStringResources.AddNewBook}", false, null, this);

                await Shell.Current.Navigation.PushAsync(view);

                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task AddExistingBook()
        {
            if (this.SelectedAuthor != null && !string.IsNullOrWhiteSpace(this.ViewTitle))
            {
                var view = new ExistingBooksView(this.SelectedAuthor, this.ViewTitle, this);

                await Shell.Current.Navigation.PushAsync(view);
            }
        }

        [RelayCommand]
        public async Task FilterPopup()
        {
            if (!string.IsNullOrEmpty(this.ViewTitle))
            {
                var popup = new FilterPopup();
                var viewModel = new FilterPopupViewModel(popup, this.ViewTitle, this.View)
                {
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
                };
                viewModel.SetFavoritePicker();
                viewModel.SetFormatPicker(this.BookFormats);
                viewModel.SetPublisherPicker(this.BookPublisherList);
                viewModel.SetPublishYearPicker(this.BookPublishYearList);
                viewModel.SetLanguagePicker(this.BookLanguageList);
                viewModel.SetRatingPicker();

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
                    RefreshView = true;
                    await this.SetViewModelData();
                }
            }
        }

        private void GetPreferences()
        {
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);
            this.ShowFavoriteBooks = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowBookRatings = Preferences.Get("RatingsOn", true /* Default */);

            this.FavoriteBooksOption = Preferences.Get($"{this.ViewTitle}_FavoriteSelection", AppStringResources.Both /* Default */);
            this.BookFormatOption = Preferences.Get($"{this.ViewTitle}_FormatSelection", AppStringResources.AllFormats /* Default */);
            this.BookPublisherOption = Preferences.Get($"{this.ViewTitle}_PublisherSelection", AppStringResources.AllPublishers /* Default */);
            this.BookPublishYearOption = Preferences.Get($"{this.ViewTitle}_PublishYearSelection", AppStringResources.AllPublishYears /* Default */);
            this.BookLanguageOption = Preferences.Get($"{this.ViewTitle}_LanguageSelection", AppStringResources.AllLanguages /* Default */);
            this.BookRatingOption = Preferences.Get($"{this.ViewTitle}_RatingSelection", AppStringResources.AllRatings /* Default */);

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
