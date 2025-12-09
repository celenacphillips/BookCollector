using BookCollector.Data;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Library
{
    public partial class ToBeReadViewModel : BookBaseViewModel
    {
        public ToBeReadViewModel(ContentPage view)
        {
            View = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.ToBeReadView_InfoText}";
            ViewTitle = AppStringResources.ToBeRead;
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
                    Task.Run (async () => FullBookList = await FilterLists.GetToBeReadBooksList(ShowHiddenBook) ),
                ]);

                if (FullBookList != null)
                {
                    TotalBooksCount = FullBookList.Count;

                    Task.WaitAll(
                    [
                        Task.Run (async () => BookPublisherList = await FilterLists.GetAllPublishersInBookList(FullBookList) ),
                        Task.Run (async () => BookLanguageList = await FilterLists.GetAllLanguagesInBookList(FullBookList) ),
                        Task.Run (async () => BookPublishYearList = await FilterLists.GetAllPublisherYearsInBookList(FullBookList) ),
                        Task.Run (async () => FilteredBookList = await FilterLists.FilterBookList(FullBookList,
                                                                                                  FavoriteBooksOption,
                                                                                                  BookFormatOption,
                                                                                                  BookPublisherOption,
                                                                                                  BookLanguageOption,
                                                                                                  BookRatingOption,
                                                                                                  BookPublishYearOption) ),
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
        public async Task FilterPopup()
        {
            if (!string.IsNullOrEmpty(ViewTitle))
            {
                var popup = new FilterPopup();
                var viewModel = new FilterPopupViewModel(popup, ViewTitle)
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
                    BookReadingDateVisible = true,
                    BookReadingDateChecked = BookReadingDateChecked,
                    BookReadPercentageVisible = true,
                    BookReadPercentageChecked = BookReadPercentageChecked,
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

        private void GetPreferences()
        {
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true  /* Default */);
            ShowFavoriteBooks = Preferences.Get("FavoritesOn", true  /* Default */);
            ShowBookRatings = Preferences.Get("RatingsOn", true  /* Default */);

            FavoriteBooksOption = Preferences.Get($"{ViewTitle}_FavoriteSelection", AppStringResources.Both  /* Default */);
            BookFormatOption = Preferences.Get($"{ViewTitle}_FormatSelection", AppStringResources.AllFormats  /* Default */);
            BookPublisherOption = Preferences.Get($"{ViewTitle}_PublisherSelection", AppStringResources.AllPublishers  /* Default */);
            BookPublishYearOption = Preferences.Get($"{ViewTitle}_PublishYearSelection", AppStringResources.AllPublishYears  /* Default */);
            BookLanguageOption = Preferences.Get($"{ViewTitle}_LanguageSelection", AppStringResources.AllLanguages  /* Default */);
            BookRatingOption = Preferences.Get($"{ViewTitle}_RatingSelection", AppStringResources.AllRatings  /* Default */);

            BookTitleChecked = Preferences.Get($"{ViewTitle}_BookTitleSelection", true  /* Default */);
            BookReadingDateChecked = Preferences.Get($"{ViewTitle}_BookReadingDateSelection", false  /* Default */);
            BookReadPercentageChecked = Preferences.Get($"{ViewTitle}_BookReadPercentageSelection", false  /* Default */);
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
