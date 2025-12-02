using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Book;
using BookCollector.Views.Groupings;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Author
{
    public partial class AuthorMainViewModel : AuthorBaseViewModel
    {
        public AuthorMainViewModel(AuthorModel author, ContentPage view)
        {
            _view = view;

            SelectedAuthor = author;
            CollectionViewHeight = DeviceHeight - SingleMenuBar;
            InfoText = $"{AppStringResources.AuthorMainView_InfoText.Replace("author", $"{SelectedAuthor.FullName}")}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                ShowHiddenBook = Preferences.Get("HiddenBooksOn", true  /* Default */);
                ShowFavoriteBooks = Preferences.Get("FavoritesOn", true  /* Default */);
                ShowBookRatings = Preferences.Get("RatingsOn", true  /* Default */);
                FavoriteBooksOption = Preferences.Get($"{ViewTitle}_FavoriteSelection", AppStringResources.Both  /* Default */);
                BookFormatOption = Preferences.Get($"{ViewTitle}_FormatSelection", AppStringResources.AllFormats  /* Default */);
                BookPublisherOption = Preferences.Get($"{ViewTitle}_PublisherSelection", AppStringResources.AllPublishers  /* Default */);
                BookPublishYearOption = Preferences.Get($"{ViewTitle}_PublishYearSelection", AppStringResources.AllPublishYears  /* Default */);
                BookLanguageOption = Preferences.Get($"{ViewTitle}_LanguageSelection", AppStringResources.AllLanguages  /* Default */);
                BookRatingOption = Preferences.Get($"{ViewTitle}_RatingSelection", AppStringResources.AllRatings  /* Default */);

                // Unit test data
                var bookList = TestData.BookList;
                var bookAuthorList = TestData.BookAuthorList;

                // Need a first Task.WaitAll so that anything dependent on this data will have the correct data.
                Task.WaitAll(
                [
                    Task.Run (async () => bookAuthorList = await FilterLists.GetAllBookAuthorsForAuthor(bookAuthorList, SelectedAuthor.AuthorGuid) ),
                ]);

                Task.WaitAll(
                [
                    Task.Run (async () => FullBookList = await FilterLists.GetAllBooksInAuthorList(bookAuthorList, bookList) ),
                ]);

                TotalBooksCount = FullBookList.Count;
                FilteredBookList = FullBookList;

                Task.WaitAll(
                [
                    Task.Run (async () => BookPublisherList = await FilterLists.GetAllPublishersInBookList(FullBookList) ),
                    Task.Run (async () => BookLanguageList = await FilterLists.GetAllLanguagesInBookList(FullBookList) ),
                    Task.Run (async () => BookPublishYearList = await FilterLists.GetAllPublisherYearsInBookList(FullBookList) ),
                    Task.Run (async () => FilteredBookList = await FilterLists.FilterBookList(FullBookList,
                                                                                              ShowHiddenBook,
                                                                                              FavoriteBooksOption,
                                                                                              BookFormatOption,
                                                                                              BookPublisherOption,
                                                                                              BookLanguageOption,
                                                                                              BookRatingOption,
                                                                                              BookPublishYearOption) ),
                ]);

                FilteredBooksCount = FilteredBookList.Count;

                TotalBooksString = StringManipulation.SetTotalBooksString(FilteredBooksCount, TotalBooksCount);

                ShowCollectionViewFooter = FilteredBooksCount > 0;

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        [RelayCommand]
        public async Task AddNewBook()
        {
            SetIsBusyTrue();

            BookModel newBook = new BookModel()
            {
                AuthorListString = SelectedAuthor.ReverseFullName,
            };

            BookEditView view = new BookEditView(newBook, $"{AppStringResources.AddNewBook}");

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task AddExistingBook()
        {
            ExistingBooksView view = new ExistingBooksView(SelectedAuthor, ViewTitle);

            await Shell.Current.Navigation.PushAsync(view);
        }

        [RelayCommand]
        public async Task FilterPopup()
        {
            var popup = new FilterPopup();
            FilterPopupViewModel viewModel = new FilterPopupViewModel(popup, ViewTitle)
            {
                FavoriteVisible = ShowFavoriteBooks,
                FavoriteOption = FavoriteBooksOption,
                FormatVisible = true,
                FormatOption = BookFormatOption,
                PublisherVisible = true,
                PublisherOption = BookPublisherOption,
                PublishYearVisible = true,
                PublishYearOption = BookPublishYearOption,
                LanguageVisible = true,
                LanguageOption = BookLanguageOption,
                RatingVisible = ShowBookRatings,
                RatingOption = BookRatingOption,
            };
            viewModel.SetFavoritePicker();
            viewModel.SetFormatPicker(BookFormats);
            viewModel.SetPublisherPicker(BookPublisherList);
            viewModel.SetPublishYearPicker(BookPublishYearList);
            viewModel.SetLanguagePicker(BookLanguageList);
            viewModel.SetRatingPicker();

            popup.BindingContext = viewModel;

            await _view.ShowPopupAsync(popup);
            await SetViewModelData();
        }
    }
}
