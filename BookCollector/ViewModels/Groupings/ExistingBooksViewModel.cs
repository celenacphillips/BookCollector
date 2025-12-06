using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Groupings
{
    public partial class ExistingBooksViewModel : BookBaseViewModel
    {
        private object SelectedObject {  get; set; }

        private string SelectedObjectType { get; set; }

        private string SelectedObjectName { get; set; }

        public ExistingBooksViewModel(object selected, ContentPage view)
        {
            _view = view;
            SelectedObject = selected;
            SetSelectedObjectType();
            SetSelectedObjectName();

            CollectionViewHeight = DeviceHeight - SingleMenuBar;
            InfoText = $"{AppStringResources.ExistingBooksView_InfoText.Replace("grouping", SelectedObjectName)}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                switch (SelectedObjectType)
                {
                    case "Collection":
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutACollectionList(ShowHiddenBook) ),
                        ]);
                        break;

                    case "Genre":
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutAGenreList(ShowHiddenBook) ),
                        ]);
                        break;

                    case "Series":
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutASeriesList(ShowHiddenBook) ),
                        ]);
                        break;

                    case "Author":
                        var author = (AuthorModel)SelectedObject;
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutAuthorList(author.ReverseFullName ,ShowHiddenBook) ),
                        ]);
                        break;

                    case "Location":
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutALocationList(ShowHiddenBook) ),
                        ]);
                        break;

                    default:
                        break;
                }

                TotalBooksCount = FullBookList.Count;
                FilteredBookList = FullBookList;

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
        public async Task ExistingBooksSelectionChanged()
        {
            var title = $"{AppStringResources.AddBookToGrouping_Question.Replace("grouping", ViewTitle)}";
            bool answer = await DisplayMessage(title, title, null, null);

            if (answer)
            {
                await AddBookToGrouping();
            }
            else
            {
                await CanceledAction();
                SelectedBook = null;
            }
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

        [RelayCommand]
        public async Task SortPopup()
        {
            var popup = new SortPopup();
            SortPopupViewModel viewModel = new SortPopupViewModel(popup, ViewTitle)
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

            await _view.ShowPopupAsync(popup);
            await SetViewModelData();
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

        private async Task AddBookToGrouping()
        {
            SetIsBusyTrue();

            switch (SelectedObjectType)
            {
                case "Collection":
                    var collection = (CollectionModel)SelectedObject;
                    SelectedBook.BookCollectionGuid = collection.CollectionGuid;
                    break;

                case "Genre":
                    var genre = (GenreModel)SelectedObject;
                    SelectedBook.BookGenreGuid = genre.GenreGuid;
                    break;

                case "Series":
                    var series = (SeriesModel)SelectedObject;
                    SelectedBook.BookSeriesGuid = series.SeriesGuid;
                    break;

                case "Author":
                    var author = (AuthorModel)SelectedObject;
                    if (!string.IsNullOrEmpty(SelectedBook.AuthorListString))
                        SelectedBook.AuthorListString += $"; {author.ReverseFullName}";
                    else
                        SelectedBook.AuthorListString = $"{author.ReverseFullName}";
                        if (TestData.UseTestData)
                        {
                            TestData.AddAuthorToBook(author.AuthorGuid, SelectedBook.BookGuid);
                        }
                        else
                        {

                        }
                    break;

                case "Location":
                    var location = (LocationModel)SelectedObject;
                    SelectedBook.BookLocationGuid = location.LocationGuid;
                    break;

                default:
                    break;
            }

            BookMainView view = new BookMainView(SelectedBook, $"{SelectedBook.BookTitle}");
            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();

            await DisplayMessage($"{AppStringResources.BookHasBeenAddedToGrouping.Replace("Book", SelectedBook.BookTitle).Replace("grouping", SelectedObjectName)}", null);
        }

        private void SetSelectedObjectType()
        {
            if (SelectedObject.GetType().ToString().Contains("Collection"))
                SelectedObjectType = "Collection";

            if (SelectedObject.GetType().ToString().Contains("Genre"))
                SelectedObjectType = "Genre";

            if (SelectedObject.GetType().ToString().Contains("Series"))
                SelectedObjectType = "Series";

            if (SelectedObject.GetType().ToString().Contains("Author"))
                SelectedObjectType = "Author";

            if (SelectedObject.GetType().ToString().Contains("Location"))
                SelectedObjectType = "Location";
        }

        private void SetSelectedObjectName()
        {
            switch (SelectedObjectType)
            {
                case "Collection":
                    var collection = (CollectionModel)SelectedObject;
                    SelectedObjectName = collection.CollectionName;
                    break;

                case "Genre":
                    var genre = (GenreModel)SelectedObject;
                    SelectedObjectName = genre.GenreName;
                    break;

                case "Series":
                    var series = (SeriesModel)SelectedObject;
                    SelectedObjectName = series.SeriesName;
                    break;

                case "Author":
                    var author = (AuthorModel)SelectedObject;
                    SelectedObjectName = author.FullName;
                    break;

                case "Location":
                    var location = (LocationModel)SelectedObject;
                    SelectedObjectName = location.LocationName;
                    break;

                default:
                    break;
            }
        }
    }
}
