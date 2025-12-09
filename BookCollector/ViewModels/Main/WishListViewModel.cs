using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Popups;
using BookCollector.Views.WishListBook;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Main
{
    public partial class WishListViewModel : BookBaseViewModel
    {
        public string? BookAuthorOption { get; set; }
        public string? BookLocationOption { get; set; }
        public string? BookSeriesOption { get; set; }

        [ObservableProperty]
        public ObservableCollection<string>? bookAuthorList;

        [ObservableProperty]
        public ObservableCollection<string>? bookLocationList;

        [ObservableProperty]
        public ObservableCollection<string>? bookSeriesList;

        public WishListViewModel(ContentPage view)
        {
            View = view;
            CollectionViewHeight = DeviceHeight - SingleMenuBar;
            InfoText = AppStringResources.WishListView_InfoText;
            ViewTitle = AppStringResources.Wishlist;
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                // Need a first Task.WaitAll so that anything dependent on this data will have the correct data.
                Task.WaitAll(
                [
                    Task.Run (async () => FullBookList = await FilterLists.GetBookWishList(ShowHiddenBook) ),
                ]);

                if (FullBookList != null)
                {
                    TotalBooksCount = FullBookList.Count;

                    Task.WaitAll(
                    [
                        Task.Run (async () => BookPublisherList = await FilterLists.GetAllPublishersInBookList(FullBookList) ),
                        Task.Run (async () => BookLanguageList = await FilterLists.GetAllLanguagesInBookList(FullBookList) ),
                        Task.Run (async () => BookPublishYearList = await FilterLists.GetAllPublisherYearsInBookList(FullBookList) ),
                        Task.Run (async () => BookAuthorList = await FilterLists.GetAllAuthorsInBookList(FullBookList) ),
                        Task.Run (async () => BookLocationList = await FilterLists.GetAllLocationsInBookList(FullBookList) ),
                        Task.Run (async () => BookSeriesList = await FilterLists.GetAllSeriesInBookList(FullBookList) ),
                        Task.Run (async () => FilteredBookList = await FilterLists.FilterBookList(FullBookList,
                                                                                                  null,
                                                                                                  BookFormatOption,
                                                                                                  BookPublisherOption,
                                                                                                  BookLanguageOption,
                                                                                                  null,
                                                                                                  BookPublishYearOption,
                                                                                                  BookAuthorOption,
                                                                                                  BookLocationOption,
                                                                                                  BookSeriesOption) ),
                    ]);

                    if (FilteredBookList != null)
                    {
                        Task.WaitAll(
                        [
                            Task.Run (async () => FilteredBookList = await FilterLists.SortBookList(FilteredBookList,
                                                                                                    BookTitleChecked,
                                                                                                    BookReadingDateChecked,
                                                                                                    BookReadPercentageChecked,
                                                                                                    BookPublisherChecked,
                                                                                                    BookPublishYearChecked,
                                                                                                    AuthorLastNameChecked,
                                                                                                    BookFormatChecked,
                                                                                                    BookPriceChecked,
                                                                                                    AscendingChecked,
                                                                                                    DescendingChecked) ),
                        ]);

                        FilteredBooksCount = FilteredBookList.Count;

                        TotalBooksString = StringManipulation.SetTotalBooksString(FilteredBooksCount, TotalBooksCount);

                        ShowCollectionViewFooter = FilteredBooksCount > 0;
                    }
                }

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
        public async Task WishListBookSelectionChanged()
        {
            if (SelectedBook != null && !string.IsNullOrEmpty(SelectedBook.BookTitle))
            {
                var view = new WishListBookMainView(SelectedBook, SelectedBook.BookTitle);

                await Shell.Current.Navigation.PushAsync(view);
                SelectedBook = null;
            }
        }

        [RelayCommand]
        public async Task AddWishListBook()
        {
            SetIsBusyTrue();

            var view = new WishListBookEditView(new BookModel(), $"{AppStringResources.AddNewBook}");

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }
     
        [RelayCommand]
        public async Task FilterPopup()
        {
            if (!string.IsNullOrEmpty(ViewTitle))
            {
                var popup = new FilterPopup();
                var viewModel = new FilterPopupViewModel(popup, ViewTitle)
                {
                    AuthorVisible = true,
                    AuthorOption = BookAuthorOption,
                    FormatVisible = true,
                    FormatOption = BookFormatOption,
                    PublisherVisible = true,
                    PublisherOption = BookPublisherOption,
                    PublishYearVisible = true,
                    PublishYearOption = BookPublishYearOption,
                    LanguageVisible = true,
                    LanguageOption = BookLanguageOption,
                    SeriesVisible = true,
                    SeriesOption = BookSeriesOption,
                    LocationVisible = true,
                    LocationOption = BookLocationOption,
                };
                viewModel.SetFavoritePicker();
                viewModel.SetFormatPicker(BookFormats);
                viewModel.SetPublisherPicker(BookPublisherList);
                viewModel.SetPublishYearPicker(BookPublishYearList);
                viewModel.SetLanguagePicker(BookLanguageList);
                viewModel.SetAuthorPicker(BookAuthorList);
                viewModel.SetLocationPicker(BookLocationList);
                viewModel.SetSeriesPicker(BookSeriesList);

                popup.BindingContext = viewModel;

                await View.ShowPopupAsync(popup);
                await SetViewModelData();
            }
        }

        [RelayCommand]
        public async Task SortPopup()
        {
            if (!string.IsNullOrEmpty(ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, ViewTitle)
                {
                    BookTitleVisible = true,
                    BookTitleChecked = BookTitleChecked,
                    BookPublisherVisible = true,
                    BookPublisherChecked = BookPublisherChecked,
                    BookPublishYearVisible = true,
                    BookPublishYearChecked = BookPublishYearChecked,
                    AuthorLastNameVisible = true,
                    AuthorLastNameChecked = AuthorLastNameChecked,
                    BookFormatVisible = true,
                    BookFormatChecked = BookFormatChecked,
                    PageCountVisible = true,
                    PageCountChecked = PageCountChecked,
                    BookPriceVisible = true,
                    BookPriceChecked = BookPriceChecked,
                    AscendingChecked = AscendingChecked,
                    DescendingChecked = DescendingChecked,
                };

                popup.BindingContext = viewModel;

                await View.ShowPopupAsync(popup);
                await SetViewModelData();
            }
        }

        // TO DO
        // Figure out how to share an item - 12/3/2025
        [RelayCommand]
        public async Task ShareList()
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = "Text",
                Title = "Test"
            });

            /*
             var bookList = string.Join("\n", books); // books is your List<string>
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = bookList,
                Title = "Share Book List"
            });

            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Share Screenshot",
                File = new ShareFile(filePath)
            });
             */
        }

        private void GetPreferences()
        {
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true  /* Default */);

            BookFormatOption = Preferences.Get($"{ViewTitle}_FormatSelection", AppStringResources.AllFormats  /* Default */);
            BookPublisherOption = Preferences.Get($"{ViewTitle}_PublisherSelection", AppStringResources.AllPublishers  /* Default */);
            BookPublishYearOption = Preferences.Get($"{ViewTitle}_PublishYearSelection", AppStringResources.AllPublishYears  /* Default */);
            BookLanguageOption = Preferences.Get($"{ViewTitle}_LanguageSelection", AppStringResources.AllLanguages  /* Default */);
            BookAuthorOption = Preferences.Get($"{ViewTitle}_AuthorSelection", AppStringResources.AllAuthors  /* Default */);
            BookSeriesOption = Preferences.Get($"{ViewTitle}_SeriesSelection", AppStringResources.AllSeries  /* Default */);
            BookLocationOption = Preferences.Get($"{ViewTitle}_LocationSelection", AppStringResources.AllLocations  /* Default */);

            BookTitleChecked = Preferences.Get($"{ViewTitle}_BookTitleSelection", true  /* Default */);
            BookPublisherChecked = Preferences.Get($"{ViewTitle}_BookPublisherSelection", false  /* Default */);
            BookPublishYearChecked = Preferences.Get($"{ViewTitle}_BookPublishYearSelection", false  /* Default */);
            AuthorLastNameChecked = Preferences.Get($"{ViewTitle}_AuthorLastNameSelection", false  /* Default */);
            BookFormatChecked = Preferences.Get($"{ViewTitle}_BookFormatSelection", false  /* Default */);
            PageCountChecked = Preferences.Get($"{ViewTitle}_PageCountSelection", false  /* Default */);
            BookPriceChecked = Preferences.Get($"{ViewTitle}_BookPriceSelection", false  /* Default */);

            AscendingChecked = Preferences.Get($"{ViewTitle}_AscendingSelection", true  /* Default */);
            DescendingChecked = Preferences.Get($"{ViewTitle}_DescendingSelection", false  /* Default */);
        }
    }
}
