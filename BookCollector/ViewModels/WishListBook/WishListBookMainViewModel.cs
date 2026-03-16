// <copyright file="WishListBookMainViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.WishListBook
{
    using System.Collections.ObjectModel;
    using BookCollector.CustomPermissions;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.Author;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Main;
    using BookCollector.ViewModels.Series;
    using BookCollector.Views.WishListBook;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// WishListBookMainViewModel class.
    /// </summary>
    public partial class WishListBookMainViewModel : BookBaseViewModel
    {
        /// <summary>
        /// Gets or sets the selected book.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public WishlistBookModel? selectedWishlistBook;

        /// <summary>
        /// Gets or sets the author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<AuthorModel>? authorList;

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
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async new Task SetViewModelData()
        {
            if (this.SelectedWishlistBook != null)
            {
                try
                {
                    this.SetIsBusyTrue();

                    var authors = ParseOutAuthorsFromstring(this.SelectedWishlistBook.AuthorListString);

                    this.AuthorListSectionValue = true;
                    this.BookInfoSectionValue = true;
                    this.SummarySectionValue = true;
                    this.CommentsSectionValue = true;

                    if (!this.SelectedWishlistBook.BookFormat!.Equals(AppStringResources.Audiobook))
                    {
                        this.ShowPages = true;
                        this.ShowTime = false;
                    }
                    else
                    {
                        this.ShowPages = false;
                        this.ShowTime = true;
                    }

                    if (!string.IsNullOrEmpty(this.SelectedWishlistBook.BookCoverFileName))
                    {
                        var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                        this.SelectedWishlistBook.BookCover = ImageSource.FromFile($"{directory}/{this.SelectedWishlistBook.BookCoverFileName}");
                    }

                    if (!string.IsNullOrEmpty(this.SelectedWishlistBook.BookCoverUrl))
                    {
                        PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                        if (internetStatus != PermissionStatus.Granted)
                        {
                            internetStatus = await Permissions.RequestAsync<InternetPermission>();
                        }

                        if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                        {
                            this.SelectedWishlistBook.BookCover = new UriImageSource
                            {
                                Uri = new Uri(this.SelectedWishlistBook.BookCoverUrl),
                                CachingEnabled = true,
                                CacheValidity = TimeSpan.FromDays(14),
                            };
                        }
                    }

                    this.BookCover = this.SelectedWishlistBook.BookCover;

                    var loadDataTasks = new Task[]
                    {
                        Task.Run(() => this.AuthorListChanged()),
                        Task.Run(() => this.BookInfoChanged()),
                        Task.Run(() => this.SummaryChanged()),
                        Task.Run(() => this.CommentsChanged()),
                        Task.Run(() => this.SelectedWishlistBook.SetCoverDisplay()),
                        Task.Run(() => this.SelectedWishlistBook.SetPartOfSeries()),
                        Task.Run(() => this.SelectedWishlistBook.SetBookPrice()),
                        Task.Run(() => this.SelectedWishlistBook.TotalTimeSpan = WishlistBookModel.SetTime(this.SelectedWishlistBook.BookHoursTotal, this.SelectedWishlistBook.BookMinutesTotal)),
                    };

                    await Task.WhenAll(authors);

                    this.AuthorList = authors.Result;

                    await Task.WhenAll(loadDataTasks);

                    this.SetIsBusyFalse();
                }
                catch (Exception ex)
                {
#if DEBUG
                    await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                }
            }
        }

        /// <summary>
        /// Show edit wishlist book view with selected book.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task EditBook()
        {
            if (this.SelectedWishlistBook != null)
            {
                this.SetIsBusyTrue();

                var view = new WishListBookEditView(this.SelectedWishlistBook, $"{AppStringResources.EditBook}", true, (WishListBookMainView)this.View);

                await Shell.Current.Navigation.PushAsync(view);

                this.SetIsBusyFalse();
            }
        }

        /// <summary>
        /// Delete selected book.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task DeleteBook()
        {
            if (this.SelectedWishlistBook != null && !string.IsNullOrEmpty(this.SelectedWishlistBook.BookTitle))
            {
                var answer = await this.DeleteCheck(this.SelectedWishlistBook.BookTitle);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        await BaseViewModel.Database.DeleteWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.SelectedWishlistBook));
                        this.RemoveFromStaticList();

                        await this.ConfirmDelete(this.SelectedWishlistBook.BookTitle);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                        await this.CanceledAction();
                    }
                }
                else
                {
                    await this.CanceledAction();
                }
            }
        }

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
                        this.SetIsBusyTrue();

                        if (!string.IsNullOrEmpty(this.SelectedWishlistBook.BookSeries))
                        {
                            SeriesModel? series = null;

                            series = await BaseViewModel.Database.GetSeriesByNameAsync(this.SelectedWishlistBook.BookSeries);

                            if (series == null || series.SeriesGuid == null)
                            {
                                series = new SeriesModel()
                                {
                                    SeriesName = this.SelectedWishlistBook.BookSeries,
                                };

                                series = await BaseViewModel.Database.SaveSeriesAsync(ConvertTo<SeriesDatabaseModel>(series));
                                await SeriesEditViewModel.AddToStaticList(series);
                                SeriesViewModel.RefreshView = true;
                            }

                            this.SelectedWishlistBook.BookSeriesGuid = series.SeriesGuid;
                            this.SelectedWishlistBook.BookSeries = null;
                        }

                        this.SelectedBook = ConvertTo<BookModel>(await BaseViewModel.Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedWishlistBook)));
                        await AddToStaticList(ConvertTo<BookModel>(this.SelectedWishlistBook));

                        await BaseViewModel.Database.DeleteWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.SelectedWishlistBook));
                        this.RemoveFromStaticList();

                        if (!string.IsNullOrEmpty(this.SelectedWishlistBook.AuthorListString))
                        {
                            var authorList = SplitStringIntoAuthorList(this.SelectedWishlistBook.AuthorListString);

                            foreach (var author in authorList)
                            {
                                var addAuthor = author;

                                var existingAuthor = await BaseViewModel.Database.GetAuthorByNameAsync(addAuthor.FirstName, addAuthor.LastName);

                                if (existingAuthor != null && existingAuthor.AuthorGuid != null)
                                {
                                    addAuthor = existingAuthor;
                                    await BaseViewModel.Database.AddAuthorToBookAsync(addAuthor.AuthorGuid, this.SelectedWishlistBook.BookGuid);
                                }
                                else
                                {
                                    addAuthor.AuthorGuid = Guid.NewGuid();
                                    await BaseViewModel.Database.InsertAuthorAsync(ConvertTo<AuthorDatabaseModel>(addAuthor), this.SelectedWishlistBook.BookGuid);
                                    await AuthorEditViewModel.AddToStaticList(addAuthor);
                                    AuthorsViewModel.RefreshView = true;
                                }
                            }
                        }

                        WishListViewModel.RefreshView = true;

                        await this.DisplayMessage(AppStringResources.AddToLibrary, AppStringResources.BookWasAddedToLibrary);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                        this.SetIsBusyFalse();
                    }
                }
                else
                {
                    await this.CanceledAction();
                }
            }
        }

        /// <summary>
        /// Share book information.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ShareList()
        {
            if (this.SelectedWishlistBook != null)
            {
                var title = this.SelectedWishlistBook.BookTitle;

                string? text;
                if (this.AuthorList != null && this.AuthorList.Count > 0)
                {
                    text = $"{AppStringResources.BookTitleByAuthorName.Replace("Book Title", this.SelectedWishlistBook.BookTitle).Replace("Author Name", this.AuthorList[0].FullName)}";

                    if (this.AuthorList.Count > 1)
                    {
                        text += $", {AppStringResources.EtAl}";
                    }
                }
                else
                {
                    text = $"{AppStringResources.BookTitle_Replace.Replace("Book Title", this.SelectedWishlistBook.BookTitle)}";
                }

                if (!string.IsNullOrEmpty(this.SelectedWishlistBook.BookURL))
                {
                    text += $" ({this.SelectedWishlistBook.BookURL})";
                }

                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = text,
                    Title = title,
                });
            }
        }

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
    }
}
