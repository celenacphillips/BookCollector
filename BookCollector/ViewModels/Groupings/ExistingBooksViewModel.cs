// <copyright file="ExistingBooksViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Groupings
{
    public partial class ExistingBooksViewModel : BookBaseViewModel
    {
        public ExistingBooksViewModel(object selected, ContentPage view)
        {
            this.View = view;
            this.SelectedObject = selected;
            this.SetSelectedObjectType();
            this.SetSelectedObjectName();

            this.CollectionViewHeight = this.DeviceHeight - this.SingleMenuBar;
            this.InfoText = $"{AppStringResources.ExistingBooksView_InfoText.Replace("grouping", this.SelectedObjectName)}";
            this.ViewTitle = AppStringResources.ExistingBooks_Object.Replace("Object", this.SelectedObjectName);
        }

        private object? SelectedObject { get; set; }

        private string? SelectedObjectType { get; set; }

        private string? SelectedObjectName { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.FilteredBookList = null;

                this.GetPreferences();

                switch (this.SelectedObjectType)
                {
                    case "Collection":
                        var fullList1 = FillLists.GetAllBooksWithoutACollectionList(this.ShowHiddenBook);
                        await Task.WhenAll(fullList1);
                        this.FullBookList = fullList1.Result;
                        break;

                    case "Genre":
                        var fullList2 = FillLists.GetAllBooksWithoutAGenreList(this.ShowHiddenBook);
                        await Task.WhenAll(fullList2);
                        this.FullBookList = fullList2.Result;
                        break;

                    case "Series":
                        var fullList3 = FillLists.GetAllBooksWithoutASeriesList(this.ShowHiddenBook);
                        await Task.WhenAll(fullList3);
                        this.FullBookList = fullList3.Result;
                        break;

                    case "Author":
                        var author = (AuthorModel?)this.SelectedObject;

                        if (author != null)
                        {
                            var fullList4 = FillLists.GetAllBooksWithoutAuthorList(author.ReverseFullName, this.ShowHiddenBook);
                            await Task.WhenAll(fullList4);
                            this.FullBookList = fullList4.Result;
                        }

                        break;

                    case "Location":
                        var fullList5 = FillLists.GetAllBooksWithoutALocationList(this.ShowHiddenBook);
                        await Task.WhenAll(fullList5);
                        this.FullBookList = fullList5.Result;
                        break;

                    default:
                        break;
                }

                if (this.FullBookList != null)
                {
                    this.TotalBooksCount = this.FullBookList.Count;
                    this.FilteredBookList = this.FullBookList;

                    var bookPublishers = FillLists.GetAllPublishersInBookList(this.FullBookList);
                    var bookLanguages = FillLists.GetAllLanguagesInBookList(this.FullBookList);
                    var bookPublishYears = FillLists.GetAllPublisherYearsInBookList(this.FullBookList);
                    var filteredList = FilterLists.FilterBookList(
                            this.FullBookList,
                            this.FavoriteBooksOption,
                            this.BookFormatOption,
                            this.BookPublisherOption,
                            this.BookLanguageOption,
                            this.BookRatingOption,
                            this.BookPublishYearOption,
                            this.Searchstring);

                    await Task.WhenAll(filteredList);

                    this.FilteredBookList = filteredList.Result;

                    var sortTasks = new Task[]
                    {
                        Task.Run(() => this.FilteredBookList.ToList().ForEach(x => x.SetReadingProgress())),
                        Task.Run(() => this.FilteredBookList.ToList().ForEach(x => x.SetAuthorListString())),
                    };

                    var loadDataTasks = new Task[]
                    {
                        Task.Run(() => this.FilteredBookList.ToList().ForEach(x => x.SetCoverDisplay())),
                    };

                    await Task.WhenAll(sortTasks);

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

                    this.FilteredBooksCount = this.FilteredBookList.Count;

                    this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);

                    this.ShowCollectionViewFooter = this.FilteredBooksCount > 0;

                    await Task.WhenAll(sortList);

                    this.FilteredBookList = sortList.Result;

                    await Task.WhenAll(loadDataTasks);

                    await Task.WhenAll(bookPublishers, bookLanguages, bookPublishYears);

                    this.BookPublisherList = bookPublishers.Result;
                    this.BookLanguageList = bookLanguages.Result;
                    this.BookPublishYearList = bookPublishYears.Result;
                }

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
#if DEBUG
                await DisplayMessage("Error!", ex.Message);
#endif
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
        public async Task ExistingBooksSelectionChanged()
        {
            var title = $"{AppStringResources.AddBookToGrouping_Question.Replace("grouping", this.ViewTitle)}";
            var answer = await DisplayMessage(title, title, null, null);

            if (answer)
            {
                await this.AddBookToGrouping();
            }
            else
            {
                await CanceledAction();
                this.SelectedBook = null;
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
                await this.SetViewModelData();
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

                            this.SelectedBook.SelectedAuthor = author;

                            if (TestData.UseTestData)
                            {
                                TestData.AddAuthorToBook(author?.AuthorGuid, this.SelectedBook.BookGuid);
                            }
                            else
                            {
                                await Database.AddAuthorToBookAsync(author?.AuthorGuid, this.SelectedBook.BookGuid);
                            }

                            await this.SelectedBook.SetAuthorListString();

                            break;

                        case "Location":
                            var location = (LocationModel?)this.SelectedObject;
                            this.SelectedBook.BookLocationGuid = location?.LocationGuid;
                            break;

                        default:
                            break;
                    }

                    if (TestData.UseTestData)
                    {
                    }
                    else
                    {
                        await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedBook));
                    }

                    var view = new BookMainView(this.SelectedBook, $"{this.SelectedBook.BookTitle}");
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
    }
}
