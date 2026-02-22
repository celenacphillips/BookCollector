// <copyright file="ExistingBooksViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Groupings
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Book;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class ExistingBooksViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public ObservableCollection<BookModel>? fullBookList;

        [ObservableProperty]
        public ObservableCollection<BookModel>? filteredBookList1;

        [ObservableProperty]
        public ObservableCollection<BookModel>? filteredBookList2;

        [ObservableProperty]
        public ObservableCollection<string>? bookAuthorList;

        [ObservableProperty]
        public int totalBooksCount;

        [ObservableProperty]
        public int filteredBooksCount;

        public ExistingBooksViewModel(object selected, ContentPage view, object? previousViewModel)
        {
            this.View = view;
            this.SelectedObject = selected;
            this.PreviousViewModel = previousViewModel;
            this.SetSelectedObjectType();
            this.SetSelectedObjectName();
            this.CollectionViewHeight = this.DeviceHeight;
            this.InfoText = $"{AppStringResources.ExistingBooksView_InfoText.Replace("grouping", this.SelectedObjectName)}";
            this.ViewTitle = AppStringResources.ExistingBooks_Object.Replace("Object", this.SelectedObjectName);
            this.RefreshView = true;
        }

        public string? BookAuthorOption { get; set; }

        public bool RefreshView { get; set; }

        private object? SelectedObject { get; set; }

        private string? SelectedObjectType { get; set; }

        private string? SelectedObjectName { get; set; }

        private object? PreviousViewModel { get; set; }

        public async Task SetCollectionList(bool showHiddenBooks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksWithoutACollectionList(showHiddenBooks);

            if (!showHiddenBooks)
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!.Where(x => !x.HideBook));
            }
            else
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!);
            }
        }

        public async Task SetGenreList(bool showHiddenBooks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksWithoutAGenreList(showHiddenBooks);

            if (!showHiddenBooks)
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!.Where(x => !x.HideBook));
            }
            else
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!);
            }
        }

        public async Task SetSeriesList(bool showHiddenBooks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksWithoutASeriesList(showHiddenBooks);

            if (!showHiddenBooks)
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!.Where(x => !x.HideBook));
            }
            else
            {
                this.FilteredBookList1 = new ObservableCollection<BookModel>(this.FullBookList!);
            }
        }

        public async Task SetAuthorList(bool showHiddenBooks)
        {
            var author = (AuthorModel?)this.SelectedObject;

            if (this.FullBookList == null && author != null)
            {
                this.FullBookList = await FillLists.GetAllBooksWithoutAuthorList(author.ReverseFullName, ShowHiddenBook);
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

        public async Task SetLocationList(bool showHiddenBooks)
        {
            this.FullBookList ??= await FillLists.GetAllBooksWithoutALocationList(showHiddenBooks);

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
            if (this.RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    switch (this.SelectedObjectType)
                    {
                        case "Collection":
                            await this.SetCollectionList(ShowHiddenBook);
                            break;

                        case "Genre":
                            await this.SetGenreList(ShowHiddenBook);
                            break;

                        case "Series":
                            await this.SetSeriesList(ShowHiddenBook);
                            break;

                        case "Author":
                            await this.SetAuthorList(ShowHiddenBook);
                            break;

                        case "Location":
                            await this.SetLocationList(ShowHiddenBook);
                            break;

                        default:
                            break;
                    }

                    if (this.FilteredBookList1 != null)
                    {
                        this.TotalBooksCount = this.FilteredBookList1.Count;
                        this.FilteredBookList2 = this.FilteredBookList1;

                        await Task.WhenAll(this.FilteredBookList1.Select(x => x.SetAuthorListString()));
                        await Task.WhenAll(this.FilteredBookList1.Select(x => x.SetCoverDisplay()));

                        var authors = FillLists.GetAllAuthorsInBookList(this.FilteredBookList1);
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
                                this.BookAuthorOption,
                                this.BookCoverOption,
                                this.SearchString);

                        await Task.WhenAll(filteredList);

                        this.FilteredBookList2 = filteredList.Result;

                        await Task.WhenAll(this.FilteredBookList1.Select(x => x.SetReadingProgress()));

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

                        await Task.WhenAll(bookPublishers, bookLanguages, bookPublishYears, authors);

                        this.BookPublisherList = bookPublishers.Result;
                        this.BookLanguageList = bookLanguages.Result;
                        this.BookPublishYearList = bookPublishYears.Result;
                        this.BookAuthorList = authors.Result;
                    }

                    this.SetIsBusyFalse();
                    this.RefreshView = false;
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
                    this.RefreshView = false;
                }
            }
        }

        [RelayCommand]
        public async Task BookSearchOnTitle(string? input)
        {
            this.SearchString = input;

            if (this.FilteredBookList2 != null && this.FilteredBookList1 != null)
            {
                this.FilteredBookList2 = await FilterLists.FilterBookList(
                                this.FilteredBookList1,
                                this.FavoriteBooksOption,
                                this.BookFormatOption,
                                this.BookPublisherOption,
                                this.BookLanguageOption,
                                this.BookRatingOption,
                                this.BookPublishYearOption,
                                this.BookAuthorOption,
                                this.BookCoverOption,
                                this.SearchString);

                this.FilteredBooksCount = this.FilteredBookList2 != null ? this.FilteredBookList2.Count : 0;

                this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                var sortList = SortLists.SortBookList(
                                        this.FilteredBookList2!,
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
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            this.RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task ExistingBooksSelectionChanged()
        {
            if (this.SelectedBook != null)
            {
                var title = $"{AppStringResources.AddBookToGrouping_Question.Replace("grouping", this.ViewTitle)}";
                var answer = await DisplayMessage(title, title, null, null);

                if (answer)
                {
                    await this.AddBookToGrouping();
                    this.SelectedBook = null;
                }
                else
                {
                    await CanceledAction();
                    this.SelectedBook = null;
                }
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
                    AuthorVisible = true,
                    AuthorOption = this.BookAuthorOption,
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
                    BookCoverVisible = true,
                    BookCoverOption = this.BookCoverOption,
                };
                viewModel.SetAuthorPicker(this.BookAuthorList);
                viewModel.SetFavoritePicker();
                viewModel.SetFormatPicker(this.BookFormats);
                viewModel.SetPublisherPicker(this.BookPublisherList);
                viewModel.SetPublishYearPicker(this.BookPublishYearList);
                viewModel.SetLanguagePicker(this.BookLanguageList);
                viewModel.SetRatingPicker();
                viewModel.SetBookCoverPicker();

                popup.BindingContext = viewModel;

                var result = await this.View.ShowPopupAsync(popup);
                if (!result.WasDismissedByTappingOutsideOfPopup)
                {
                    this.RefreshView = true;
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
                    this.RefreshView = true;
                    await this.SetViewModelData();
                }
            }
        }

        private void GetPreferences()
        {
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);
            this.ShowFavoriteBooks = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowBookRatings = Preferences.Get("RatingsOn", true /* Default */);

            this.BookAuthorOption = Preferences.Get($"{this.ViewTitle}_AuthorSelection", AppStringResources.AllAuthors /* Default */);
            this.FavoriteBooksOption = Preferences.Get($"{this.ViewTitle}_FavoriteSelection", AppStringResources.Both /* Default */);
            this.BookFormatOption = Preferences.Get($"{this.ViewTitle}_FormatSelection", AppStringResources.AllFormats /* Default */);
            this.BookPublisherOption = Preferences.Get($"{this.ViewTitle}_PublisherSelection", AppStringResources.AllPublishers /* Default */);
            this.BookPublishYearOption = Preferences.Get($"{this.ViewTitle}_PublishYearSelection", AppStringResources.AllPublishYears /* Default */);
            this.BookLanguageOption = Preferences.Get($"{this.ViewTitle}_LanguageSelection", AppStringResources.AllLanguages /* Default */);
            this.BookRatingOption = Preferences.Get($"{this.ViewTitle}_RatingSelection", AppStringResources.AllRatings /* Default */);
            this.BookCoverOption = Preferences.Get($"{this.ViewTitle}_BookCoverSelection", AppStringResources.Both /* Default */);

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

        private async Task AddBookToGrouping()
        {
            try
            {
                this.SetIsBusyTrue();

                if (this.SelectedBook != null)
                {
                    switch (this.SelectedObjectType)
                    {
                        case "Collection":
                            var collection = (CollectionModel?)this.SelectedObject;
                            this.SelectedBook.BookCollectionGuid = collection?.CollectionGuid;
                            break;

                        case "Genre":
                            var genre = (GenreModel?)this.SelectedObject;
                            this.SelectedBook.BookGenreGuid = genre?.GenreGuid;
                            break;

                        case "Series":
                            var series = (SeriesModel?)this.SelectedObject;
                            this.SelectedBook.BookSeriesGuid = series?.SeriesGuid;
                            break;

                        case "Author":
                            var author = (AuthorModel?)this.SelectedObject;

                            this.SelectedBook.SelectedAuthors ??= [];
                            this.SelectedBook.SelectedAuthors.Add(author);

                            await Database.AddAuthorToBookAsync(author?.AuthorGuid, this.SelectedBook.BookGuid);

                            await this.SelectedBook.SetAuthorListString();

                            break;

                        case "Location":
                            var location = (LocationModel?)this.SelectedObject;
                            this.SelectedBook.BookLocationGuid = location?.LocationGuid;
                            break;

                        default:
                            break;
                    }

                    await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedBook));

                    this.RemoveFromStaticList(this.SelectedBook);
                    await AddToStaticList(this.SelectedBook, this.PreviousViewModel);

                    var view = new BookMainView(this.SelectedBook, $"{this.SelectedBook.BookTitle}", this.PreviousViewModel);
                    await Shell.Current.Navigation.PushAsync(view);

                    await DisplayMessage($"{AppStringResources.BookHasBeenAddedToGrouping.Replace("Book", this.SelectedBook.BookTitle).Replace("grouping", this.SelectedObjectName)}", null);
                }

                this.SetIsBusyFalse();
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
            }
        }

        private void SetSelectedObjectType()
        {
            if (this.SelectedObject != null)
            {
                if (this.SelectedObject.GetType().ToString().Contains("Collection"))
                {
                    this.SelectedObjectType = "Collection";
                }

                if (this.SelectedObject.GetType().ToString().Contains("Genre"))
                {
                    this.SelectedObjectType = "Genre";
                }

                if (this.SelectedObject.GetType().ToString().Contains("Series"))
                {
                    this.SelectedObjectType = "Series";
                }

                if (this.SelectedObject.GetType().ToString().Contains("Author"))
                {
                    this.SelectedObjectType = "Author";
                }

                if (this.SelectedObject.GetType().ToString().Contains("Location"))
                {
                    this.SelectedObjectType = "Location";
                }
            }
        }

        private void SetSelectedObjectName()
        {
            switch (this.SelectedObjectType)
            {
                case "Collection":
                    var collection = (CollectionModel?)this.SelectedObject;
                    this.SelectedObjectName = collection?.CollectionName;
                    break;

                case "Genre":
                    var genre = (GenreModel?)this.SelectedObject;
                    this.SelectedObjectName = genre?.GenreName;
                    break;

                case "Series":
                    var series = (SeriesModel?)this.SelectedObject;
                    this.SelectedObjectName = series?.SeriesName;
                    break;

                case "Author":
                    var author = (AuthorModel?)this.SelectedObject;
                    this.SelectedObjectName = author?.FullName;
                    break;

                case "Location":
                    var location = (LocationModel?)this.SelectedObject;
                    this.SelectedObjectName = location?.LocationName;
                    break;

                default:
                    break;
            }
        }

        private new void RemoveFromStaticList(BookModel book)
        {
            if (this.FullBookList != null)
            {
                this.RefreshView = RemoveBookFromStaticList(book, this.FullBookList, this.FilteredBookList2);
            }
        }
    }
}
