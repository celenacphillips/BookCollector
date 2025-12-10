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

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                // Need a first Task.WaitAll so that anything dependent on this data will have the correct data.
                Task.WaitAll(
               [
                    Task.Run(async () => this.FullBookList = await FilterLists.GetBookWishList(this.ShowHiddenBook)),
                ]);

                if (this.FullBookList != null)
                {
                    this.TotalBooksCount = this.FullBookList.Count;

                    Task.WaitAll(
                   [
                        Task.Run(async () => this.BookPublisherList = await FilterLists.GetAllPublishersInBookList(this.FullBookList)),
                        Task.Run(async () => this.BookLanguageList = await FilterLists.GetAllLanguagesInBookList(this.FullBookList)),
                        Task.Run(async () => this.BookPublishYearList = await FilterLists.GetAllPublisherYearsInBookList(this.FullBookList)),
                        Task.Run(async () => this.BookAuthorList = await FilterLists.GetAllAuthorsInBookList(this.FullBookList)),
                        Task.Run(async () => this.BookLocationList = await FilterLists.GetAllLocationsInBookList(this.FullBookList)),
                        Task.Run(async () => this.BookSeriesList = await FilterLists.GetAllSeriesInBookList(this.FullBookList)),
                        Task.Run(async () => this.FilteredBookList = await FilterLists.FilterBookList(
                            this.FullBookList,
                            null,
                            this.BookFormatOption,
                            this.BookPublisherOption,
                            this.BookLanguageOption,
                            null,
                            this.BookPublishYearOption,
                            this.BookAuthorOption,
                            this.BookLocationOption,
                            this.BookSeriesOption)),
                    ]);

                    if (this.FilteredBookList != null)
                    {
                        Task.WaitAll(
                       [
                            Task.Run(async () => this.FilteredBookList = await FilterLists.SortBookList(
                                this.FilteredBookList,
                                this.BookTitleChecked,
                                this.BookReadingDateChecked,
                                this.BookReadPercentageChecked,
                                this.BookPublisherChecked,
                                this.BookPublishYearChecked,
                                this.AuthorLastNameChecked,
                                this.BookFormatChecked,
                                this.BookPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked)),
                        ]);

                        this.FilteredBooksCount = this.FilteredBookList.Count;

                        this.TotalBooksstring = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                        this.ShowCollectionViewFooter = this.FilteredBooksCount > 0;
                    }
                }

                this.SetIsBusyFalse();
            }
            catch (Exception)
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
            if (this.SelectedBook != null && !string.IsNullOrEmpty(this.SelectedBook.BookTitle))
            {
                var view = new WishListBookMainView(this.SelectedBook, this.SelectedBook.BookTitle);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedBook = null;
            }
        }

        [RelayCommand]
        public async Task AddWishListBook()
        {
            this.SetIsBusyTrue();

            var view = new WishListBookEditView(new BookModel(), $"{AppStringResources.AddNewBook}");

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

        // TO DO
        // Figure out how to share an item - 12/3/2025
        [RelayCommand]
        public async Task ShareList()
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Text = "Text",
                Title = "Test",
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
            this.ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

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
