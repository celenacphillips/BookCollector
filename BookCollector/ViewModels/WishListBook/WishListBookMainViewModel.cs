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

    public partial class WishListBookMainViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public WishlistBookModel? selectedWishlistBook;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<AuthorModel>? authorList;

        public WishListBookMainViewModel(WishlistBookModel book, ContentPage view)
        {
            this.View = view;

            this.SelectedWishlistBook = book;
            this.InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book", $"{this.SelectedWishlistBook.BookTitle}")}";
            this.AuthorList = [];
        }

        public async Task SetViewModelData()
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

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

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

                        await Database.DeleteWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.SelectedWishlistBook));
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

                            series = await Database.GetSeriesByNameAsync(this.SelectedWishlistBook.BookSeries);

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

                        this.SelectedBook = ConvertTo<BookModel>(await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedWishlistBook)));
                        await AddToStaticList(ConvertTo<BookModel>(this.SelectedWishlistBook));

                        await Database.DeleteWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.SelectedWishlistBook));
                        this.RemoveFromStaticList();

                        if (!string.IsNullOrEmpty(this.SelectedWishlistBook.AuthorListString))
                        {
                            var authorList = SplitStringIntoAuthorList(this.SelectedWishlistBook.AuthorListString);

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
                WishListViewModel.RefreshView = this.RemoveWishListBookFromStaticList(WishListViewModel.fullWishlistBookList, WishListViewModel.filteredWishlistBookList2);
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
