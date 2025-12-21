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
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Author
{
    public partial class AuthorMainViewModel : AuthorBaseViewModel
    {
        public AuthorMainViewModel(AuthorModel author, ContentPage view)
        {
            this.View = view;

            this.SelectedAuthor = author;
            this.CollectionViewHeight = this.DeviceHeight - this.SingleMenuBar;
            this.InfoText = $"{AppStringResources.AuthorMainView_InfoText.Replace("author", $"{this.SelectedAuthor.FullName}")}";
        }

        public void SetViewModelData()
        {
            if (this.SelectedAuthor != null)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    Task.WaitAll(
                    [
                        Task.Run(async () => this.FullBookList = await FillLists.GetAllBooksInAuthorList(this.SelectedAuthor.AuthorGuid, this.ShowHiddenBook)),
                    ]);

                    if (this.FullBookList != null)
                    {
                        Task.WaitAll(
                        [
                            Task.Run(async () => this.BookPublisherList = await FillLists.GetAllPublishersInBookList(this.FullBookList)),
                            Task.Run(async () => this.BookLanguageList = await FillLists.GetAllLanguagesInBookList(this.FullBookList)),
                            Task.Run(async () => this.BookPublishYearList = await FillLists.GetAllPublisherYearsInBookList(this.FullBookList)),
                            Task.Run(async () => this.FilteredBookList = await FilterLists.FilterBookList(
                                this.FullBookList,
                                this.FavoriteBooksOption,
                                this.BookFormatOption,
                                this.BookPublisherOption,
                                this.BookLanguageOption,
                                this.BookRatingOption,
                                this.BookPublishYearOption)),
                        ]);

                        if (this.FilteredBookList != null)
                        {
                            Task.WaitAll(
                            [
                                Task.Run(async () => this.FilteredBookList = await SortLists.SortBookList(
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
                                    this.DescendingChecked)),
                            ]);
                        }
                    }

                    this.TotalBooksCount = this.FullBookList != null ? this.FullBookList.Count : 0;

                    this.FilteredBookList.ToList().ForEach(x => x.SetCoverDisplay());
                    this.FilteredBookList.ToList().ForEach(x => x.SetReadingProgress());

                    this.FilteredBooksCount = this.FilteredBookList != null ? this.FilteredBookList.Count : 0;

                    this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                    this.ShowCollectionViewFooter = this.FilteredBooksCount > 0;

                    this.SetIsBusyFalse();
                }
                catch (Exception ex)
                {
                    this.SetIsBusyFalse();
                }
            }
        }

        [RelayCommand]
        public void Refresh()
        {
            this.SetRefreshTrue();
            this.SetViewModelData();
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
                    SelectedAuthor = this.SelectedAuthor,
                };

                var view = new BookEditView(newBook, $"{AppStringResources.AddNewBook}");

                await Shell.Current.Navigation.PushAsync(view);

                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task AddExistingBook()
        {
            if (this.SelectedAuthor != null && !string.IsNullOrWhiteSpace(this.ViewTitle))
            {
                var view = new ExistingBooksView(this.SelectedAuthor, this.ViewTitle);

                await Shell.Current.Navigation.PushAsync(view);
            }
        }

        [RelayCommand]
        public async Task FilterPopup()
        {
            if (!string.IsNullOrEmpty(this.ViewTitle))
            {
                var popup = new FilterPopup();
                var viewModel = new FilterPopupViewModel(popup, this.ViewTitle)
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

                await this.View.ShowPopupAsync(popup);
                this.SetViewModelData();
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
                    PageCountVisible = true,
                    PageCountChecked = this.PageCountChecked,
                    BookPriceVisible = true,
                    BookPriceChecked = this.BookPriceChecked,
                    AscendingChecked = this.AscendingChecked,
                    DescendingChecked = this.DescendingChecked,
                };

                popup.BindingContext = viewModel;

                await this.View.ShowPopupAsync(popup);
                this.SetViewModelData();
            }
        }

        private void GetPreferences()
        {
            this.ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);
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
            this.PageCountChecked = Preferences.Get($"{this.ViewTitle}_PageCountSelection", false /* Default */);
            this.BookPriceChecked = Preferences.Get($"{this.ViewTitle}_BookPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }
    }
}
