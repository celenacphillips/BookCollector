// <copyright file="WishListBookMainViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.WishListBook
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.Author;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Main;
    using BookCollector.ViewModels.Series;
    using BookCollector.Views.WishListBook;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// WishListBookMainViewModel class.
    /// </summary>
    public partial class WishListBookMainViewModel : BookMainBaseViewModel
    {
        /// <summary>
        /// Gets or sets the selected book.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public WishlistBookModel? selectedWishlistBook;

        /********************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="WishListBookMainViewModel"/> class.
        /// </summary>
        /// <param name="book">Book to view.</param>
        /// <param name="view">View related to view model.</param>
        public WishListBookMainViewModel(WishlistBookModel book, ContentPage view)
        {
            this.View = view;

            this.SelectedWishlistBook = book;
            this.InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book", $"{this.SelectedWishlistBook.BookTitle}")}";
            this.AuthorList = [];
            this.SetRefreshView(true);
        }

        /********************************************************/

        /// <summary>
        /// Move selected book to library and remove from wishlist.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddToLibrary()
        {
            if (this.SelectedWishlistBook != null)
            {
                var answer = await this.DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureYouWantToMoveBookToYourLibrary_Question.Replace("book", this.SelectedWishlistBook.BookTitle), null, null);

                if (answer)
                {
                    try
                    {
                        await this.SetIsBusyTrue();

                        if (!string.IsNullOrEmpty(this.SelectedWishlistBook.BookSeries))
                        {
                            await this.SaveSeries();
                        }

                        await this.MoveBook();

                        if (!string.IsNullOrEmpty(this.SelectedWishlistBook.AuthorListString))
                        {
                            await this.SaveAuthors();
                        }

                        WishListViewModel.RefreshView = true;

                        await this.DisplayMessage(AppStringResources.AddToLibrary, AppStringResources.BookWasAddedToLibrary);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
                        await this.ViewModelCatch(ex);
                        this.SetRefreshView(false);
                    }
                }
                else
                {
                    await this.CanceledAction();
                }
            }
        }

        /********************************************************/

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        public override void GetPreferences()
        {
        }

        /// <summary>
        /// Set the view model lists.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetLists()
        {
            var authors = StringManipulation.SplitAuthorListStringIntoAuthorList(this.SelectedWishlistBook!.AuthorListString!);

            await Task.WhenAll(authors);

            this.AuthorList = authors.Result.ToObservableCollection();
        }

        /// <summary>
        /// Set section values.
        /// </summary>
        public override void SetSectionValues()
        {
            this.AuthorListSectionValue = true;
            this.BookInfoSectionValue = true;
            this.SummarySectionValue = true;
            this.CommentsSectionValue = true;
        }

        /// <summary>
        /// Get book data for other methods.
        /// </summary>
        /// <param name="returnData">Return type.</param>
        /// <returns>An object of book data.</returns>
        public override object? GetBookData(string? returnData)
        {
            if (returnData != null && returnData.Equals("strings"))
            {
                return (List<string?>)[this.SelectedWishlistBook!.BookCoverFileName, this.SelectedWishlistBook!.BookCoverUrl];
            }

            return this.SelectedWishlistBook;
        }

        /// <summary>
        /// Check book format and set values.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task CheckBookFormat()
        {
            if (!this.SelectedWishlistBook!.BookFormat!.Equals(AppStringResources.Audiobook))
            {
                this.ShowPages = true;
                this.ShowTime = false;
            }
            else
            {
                this.ShowPages = false;
                this.ShowTime = true;
            }
        }

        /// <summary>
        /// Set other view data.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetViewData()
        {
            var loadDataTasks = new Task[]
            {
                Task.Run(() => this.AuthorListChanged()),
                Task.Run(() => this.BookInfoChanged()),
                Task.Run(() => this.SummaryChanged()),
                Task.Run(() => this.CommentsChanged()),
                Task.Run(() => this.SelectedWishlistBook!.SetCoverDisplay()),
                Task.Run(() => this.SelectedWishlistBook!.SetPartOfSeries()),
                Task.Run(() => this.SelectedWishlistBook!.SetBookPrice()),
                Task.Run(() => this.SelectedWishlistBook!.TotalTimeSpan = SetTime(this.SelectedWishlistBook.BookHoursTotal, this.SelectedWishlistBook.BookMinutesTotal)),
            };

            await Task.WhenAll(loadDataTasks);
        }

        /// <summary>
        /// Set edit view.
        /// </summary>
        /// <returns>Page to navigate to.</returns>
        public override ContentPage SetEditView()
        {
            return new WishListBookEditView(this.SelectedWishlistBook!, $"{AppStringResources.EditBook}", true, (WishListBookMainView)this.View);
        }

        /// <summary>
        /// Delete book data.
        /// </summary>
        /// <returns>An task.</returns>
        public override async Task DeleteData()
        {
            await Database.DeleteWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.SelectedWishlistBook!));
            this.RemoveFromStaticList();
        }

        /// <summary>
        /// Set whether to refresh view or not.
        /// </summary>
        /// <param name="value">Value to change to.</param>
        public override void SetRefreshView(bool value)
        {
            RefreshView = value;
        }

        /********************************************************/

        private void RemoveFromStaticList()
        {
            if (WishListViewModel.fullWishlistBookList != null)
            {
                WishListViewModel.RefreshView = this.RemoveWishListBookFromStaticList(WishListViewModel.fullWishlistBookList, WishListViewModel.filteredWishlistBookList);
            }
        }

        private bool RemoveWishListBookFromStaticList(ObservableCollection<WishlistBookModel> bookList, ObservableCollection<WishlistBookModel>? filteredBookList)
        {
            var refresh = false;

            try
            {
                var oldBook = bookList.FirstOrDefault(x => x.BookGuid == this.SelectedWishlistBook!.BookGuid);

                if (oldBook != null)
                {
                    bookList.Remove(oldBook);
                    refresh = true;
                }

                if (filteredBookList != null)
                {
                    var filteredBook = filteredBookList.FirstOrDefault(x => x.BookGuid == this.SelectedWishlistBook!.BookGuid);

                    if (filteredBook != null)
                    {
                        filteredBookList.Remove(filteredBook);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        private async Task SaveSeries()
        {
            SeriesModel? series = null;

            series = await Database.GetSeriesByNameAsync(this.SelectedWishlistBook!.BookSeries);

            if (series == null || series.SeriesGuid == null)
            {
                series = new SeriesModel()
                {
                    SeriesName = this.SelectedWishlistBook.BookSeries,
                };

                series = await Database.SaveSeriesAsync(ConvertTo<SeriesDatabaseModel>(series));
                await SeriesEditViewModel.AddToStaticList(series);
                SeriesViewModel.RefreshView = true;
            }

            this.SelectedWishlistBook.BookSeriesGuid = series.SeriesGuid;
            this.SelectedWishlistBook.BookSeries = null;
        }

        private async Task SaveAuthors()
        {
            var authorList = await StringManipulation.SplitAuthorListStringIntoAuthorList(this.SelectedWishlistBook!.AuthorListString!);

            foreach (var author in authorList)
            {
                var addAuthor = author;

                var existingAuthor = await Database.GetAuthorByNameAsync(addAuthor.FirstName, addAuthor.LastName);

                if (existingAuthor != null && existingAuthor.AuthorGuid != null)
                {
                    addAuthor = existingAuthor;
                    await Database.AddAuthorToBookAsync(addAuthor.AuthorGuid, this.SelectedWishlistBook.BookGuid);
                }
                else
                {
                    addAuthor.AuthorGuid = Guid.NewGuid();
                    await Database.InsertAuthorAsync(ConvertTo<AuthorDatabaseModel>(addAuthor), this.SelectedWishlistBook.BookGuid);
                    await AuthorEditViewModel.AddToStaticList(addAuthor);
                    AuthorsViewModel.RefreshView = true;
                }
            }
        }

        private async Task MoveBook()
        {
            this.SelectedBook = ConvertTo<BookModel>(await BaseViewModel.Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedWishlistBook!)));
            await AddToStaticList(ConvertTo<BookModel>(this.SelectedWishlistBook!));

            await BaseViewModel.Database.DeleteWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.SelectedWishlistBook!));
            this.RemoveFromStaticList();
        }
    }
}
