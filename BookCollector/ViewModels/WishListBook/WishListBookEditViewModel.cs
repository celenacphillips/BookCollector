// <copyright file="WishListBookEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.WishListBook
{
    using System.Collections.ObjectModel;
    using BookCollector.CustomPermissions;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Main;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Book;
    using BookCollector.Views.Popups;
    using BookCollector.Views.WishListBook;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// WishListBookEditViewModel class.
    /// </summary>
    public partial class WishListBookEditViewModel : BookBaseViewModel
    {
        /// <summary>
        /// Gets or sets the book to edit.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public WishlistBookModel editedWishlistBook;

        /// <summary>
        /// Gets or sets a value indicating whether the book title is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookTitleNotValid;

        /// <summary>
        /// Gets or sets a value indicating whether the book format is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookFormatNotValid;

        /// <summary>
        /// Gets or sets a value indicating whether the first section in book info is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfo1SectionValue;

        /// <summary>
        /// Gets or sets a value indicating whether the first section in book info is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfo1Open;

        /// <summary>
        /// Gets or sets a value indicating whether the first section in book info is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfo1NotOpen;

        /// <summary>
        /// Gets or sets the author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<AuthorModel>? authorList;

        /// <summary>
        /// Gets or sets the selected book format.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string selectedBookFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishListBookEditViewModel"/> class.
        /// </summary>
        /// <param name="book">Book to edit.</param>
        /// <param name="view">View related to view model.</param>
        public WishListBookEditViewModel(WishlistBookModel book, ContentPage view)
        {
            this.View = view;

            this.EditedWishlistBook = (WishlistBookModel)book.Clone();
            this.InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{this.EditedWishlistBook.BookTitle}")}";
            this.SelectedBookFormat = this.EditedWishlistBook.BookFormat ?? AppStringResources.SelectABookFormat;
            this.PopupWidth = DeviceWidth - 50;
            RefreshView = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to remove the main view before.
        /// </summary>
        public bool RemoveMainViewBefore { get; set; }

        /// <summary>
        /// Gets or sets the main view before, to return to after closing the popup.
        /// </summary>
        public WishListBookMainView? MainViewBefore { get; set; }

        /// <summary>
        /// Gets or sets the popup width.
        /// </summary>
        public double PopupWidth { get; set; }

        /// <summary>
        /// Add book to static list.
        /// </summary>
        /// <param name="book">Book to add.</param>
        /// <returns>A task.</returns>
        public static async Task AddToStaticList(WishlistBookModel book)
        {
            if (WishListViewModel.fullWishlistBookList != null)
            {
                WishListViewModel.RefreshView = AddWishListBookToStaticList(book, WishListViewModel.fullWishlistBookList, WishListViewModel.filteredWishlistBookList);
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async new Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    var authors = ParseOutAuthorsFromstring(this.EditedWishlistBook.AuthorListString);

                    this.BookInfo1SectionValue = true;
                    this.AuthorListSectionValue = true;
                    this.BookInfoSectionValue = true;
                    this.SummarySectionValue = true;
                    this.CommentsSectionValue = true;

                    if (this.EditedWishlistBook.BookFormat == null || !this.EditedWishlistBook.BookFormat.Equals(AppStringResources.Audiobook))
                    {
                        this.ShowPages = true;
                        this.ShowTime = false;
                    }
                    else
                    {
                        this.ShowTime = true;
                        this.ShowPages = false;
                    }

                    if (!string.IsNullOrEmpty(this.EditedWishlistBook.BookCoverFileName))
                    {
                        var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                        this.EditedWishlistBook.BookCover = ImageSource.FromFile($"{directory}/{this.EditedWishlistBook.BookCoverFileName}");
                    }

                    if (!string.IsNullOrEmpty(this.EditedWishlistBook.BookCoverUrl))
                    {
                        PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                        if (internetStatus != PermissionStatus.Granted)
                        {
                            internetStatus = await Permissions.RequestAsync<InternetPermission>();
                        }

                        if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                        {
                            this.EditedWishlistBook.BookCover = new UriImageSource
                            {
                                Uri = new Uri(this.EditedWishlistBook.BookCoverUrl),
                                CachingEnabled = true,
                                CacheValidity = TimeSpan.FromDays(14),
                            };
                        }
                    }

                    this.BookCover = this.EditedWishlistBook.BookCover;

                    var loadDataTasks = new Task[]
                    {
                        Task.Run(() => this.ValidateEntry()),
                        Task.Run(() => this.EditedWishlistBook.SetBookPrice()),
                        Task.Run(() => this.EditedWishlistBook.SetCoverDisplay()),
                        Task.Run(() => this.BookInfo1Changed()),
                        Task.Run(() => this.ReadingDataChanged()),
                        Task.Run(() => this.ChapterListChanged()),
                        Task.Run(() => this.AuthorListChanged()),
                        Task.Run(() => this.BookInfoChanged()),
                        Task.Run(() => this.SummaryChanged()),
                        Task.Run(() => this.CommentsChanged()),
                        Task.Run(() => this.EditedWishlistBook.TotalTimeSpan = WishlistBookModel.SetTime(this.EditedWishlistBook.BookHoursTotal, this.EditedWishlistBook.BookMinutesTotal)),
                    };

                    await Task.WhenAll(authors);

                    this.AuthorList = authors.Result;

                    this.AuthorList ??= [];

                    if (this.EditedWishlistBook.SelectedAuthors != null && this.EditedWishlistBook.SelectedAuthors.Count > 0)
                    {
                        foreach (var selectedAuthor in this.EditedWishlistBook.SelectedAuthors)
                        {
                            if (this.AuthorList != null && selectedAuthor != null)
                            {
                                var author = new AuthorModel()
                                {
                                    FirstName = selectedAuthor.FirstName,
                                    LastName = selectedAuthor.LastName,
                                };

                                this.AuthorList.Add(author);
                            }
                        }
                    }

                    await Task.WhenAll(loadDataTasks);

                    this.SetIsBusyFalse();
                    RefreshView = false;
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
                    RefreshView = false;
                }
            }
        }

        /// <summary>
        /// Show book search view to search for a book and fill the book info from the search result.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookSearch()
        {
            this.SetIsBusyTrue();

            var view = new BookSearchView(null, null, null, this.EditedWishlistBook, this);

            await Shell.Current.Navigation.PushModalAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Save the book to the database and return to the previous view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SaveBook()
        {
            try
            {
                this.SetIsBusyTrue();

                if (this.BookTitleNotValid || this.BookFormatNotValid)
                {
                    if (this.BookTitleNotValid)
                    {
                        await this.DisplayMessage(AppStringResources.BookTitleNotValid, null);
                    }

                    if (this.BookFormatNotValid)
                    {
                        await this.DisplayMessage(AppStringResources.BookFormatNotValid, null);
                    }

                    this.SetIsBusyFalse();
                }
                else
                {
                    this.SetIsBusyTrue();

                    var dataTasks = new Task[]
                    {
                        Task.Run(() => this.EditedWishlistBook.SetCoverDisplay()),
                        Task.Run(() => this.EditedWishlistBook.SetPartOfSeries()),
                        Task.Run(() => this.EditedWishlistBook.SetBookPrice()),
                        Task.Run(() => this.EditedWishlistBook.SetAuthorListString(this.AuthorList, false)),
                    };

                    await Task.WhenAll(dataTasks);

#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    this.EditedWishlistBook = await Database.SaveWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.EditedWishlistBook));
                    await AddToStaticList(this.EditedWishlistBook);

                    if (this.RemoveMainViewBefore)
                    {
                        Shell.Current.Navigation.RemovePage(this.MainViewBefore);
                    }

                    var view = new WishListBookMainView(this.EditedWishlistBook, $"{this.EditedWishlistBook.BookTitle}");
                    Shell.Current.Navigation.InsertPageBefore(view, this.View);

                    await Shell.Current.Navigation.PopAsync();

                    this.SetIsBusyFalse();
                }
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

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookInfo1Changed()
        {
            this.BookInfo1Open = this.BookInfo1SectionValue;
            this.BookInfo1NotOpen = !this.BookInfo1SectionValue;
        }

        /// <summary>
        /// Show popup to choose between adding a cover photo by picking an existing file
        /// or by entering an image url, and set the book cover accordingly.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {
            var action = await this.PopupMenu_CoverPhoto();

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.UploadExistingFile))
            {
                this.SetIsBusyTrue();

                PermissionStatus storageReadStatus = await Permissions.RequestAsync<Permissions.Photos>();
                storageReadStatus = await Permissions.CheckStatusAsync<Permissions.Photos>();

                if (storageReadStatus == PermissionStatus.Granted)
                {
                    MediaPickerOptions pickerOptions = new ();

                    try
                    {
                        var photos = await MediaPicker.PickPhotosAsync(pickerOptions);

                        if (photos?.Count > 0)
                        {
                            var firstPhoto = photos.First();
                            this.BookCover = ImageSource.FromFile(firstPhoto.FullPath);
                            this.EditedWishlistBook.HasBookCover = true;
                            this.EditedWishlistBook.HasNoBookCover = false;

                            var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            var fi = new FileInfo(firstPhoto.FullPath);
                            var filePath = $"{directory}/{fi.Name}";
                            File.Copy(firstPhoto.FullPath, filePath, true);

                            this.EditedWishlistBook.BookCoverFileName = fi.Name;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.SetIsBusyFalse();
                        await this.DisplayMessage(AppStringResources.PickingCoverCanceled, null);
#if DEBUG
                        await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    }

                    if (this.EditedWishlistBook.HasNoBookCover)
                    {
                        await this.DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                    }
                }
                else
                {
                    await this.DisplayMessage(AppStringResources.PleaseAllowPhotoPermissionToAddCover, null);
                }

                this.SetIsBusyFalse();
            }

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.BookCoverUrl))
            {
                var result = await this.View.ShowPopupAsync<string>(new BookCoverUrlPopup(this.PopupWidth, this.EditedWishlistBook.BookCoverUrl));
                var bookCoverUrl = result.Result;

                if (!string.IsNullOrEmpty(bookCoverUrl))
                {
                    this.SetIsBusyTrue();

                    PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                    if (internetStatus != PermissionStatus.Granted)
                    {
                        internetStatus = await Permissions.RequestAsync<InternetPermission>();
                    }

                    if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        try
                        {
                            this.BookCover = new UriImageSource
                            {
                                Uri = new Uri(bookCoverUrl),
                                CachingEnabled = true,
                                CacheValidity = TimeSpan.FromDays(14),
                            };
                            this.EditedWishlistBook.BookCover = this.BookCover;
                            this.EditedWishlistBook.HasBookCover = true;
                            this.EditedWishlistBook.HasNoBookCover = false;
                            this.EditedWishlistBook.BookCoverUrl = bookCoverUrl;

                            this.SetIsBusyFalse();
                        }
                        catch (Exception ex)
                        {
                            this.SetIsBusyFalse();
                            this.BookCover = null;
                            this.EditedWishlistBook.BookCover = null;
                            this.EditedWishlistBook.HasBookCover = false;
                            this.EditedWishlistBook.HasNoBookCover = true;
                            this.EditedWishlistBook.BookCoverUrl = null;
                            await this.DisplayMessage(AppStringResources.AnErrorOccurred, AppStringResources.ErrorDownloadingImage);
#if DEBUG
                            await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                            await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                        }
                    }

                    this.SetIsBusyFalse();
                }
            }
        }

        /// <summary>
        /// Remove the book cover and delete the related file if exists, and set the
        /// related book properties accordingly.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task RemoveCoverPhoto()
        {
            this.EditedWishlistBook.HasBookCover = false;
            this.EditedWishlistBook.HasNoBookCover = true;
            this.EditedWishlistBook.BookCover = null;
            this.EditedWishlistBook.BookCoverUrl = null;
            this.EditedWishlistBook.BookCoverFileName = null;
        }

        /// <summary>
        /// Add a new author to the author list.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddAuthor()
        {
            this.AuthorList ??= [];

            this.AuthorList.Add(new AuthorModel());
        }

        /// <summary>
        /// Remove author from the author list, and add it to the authors to delete list.
        /// </summary>
        /// <param name="author">Author to remove.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task RemoveAuthor(AuthorModel author)
        {
            this.AuthorList?.Remove(author);
        }

        /// <summary>
        /// Validate book format.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateBookFormat()
        {
            this.ValidateEntry();
        }

        /// <summary>
        /// Validate book title.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateBookTitle()
        {
            this.ValidateEntry();
        }

        /// <summary>
        /// Show popup with time entries to set the total time.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task TotalTimePopup()
        {
            try
            {
                var totalTimePopup = new TimePopup(AppStringResources.TotalTime, this.PopupWidth, this.EditedWishlistBook.BookHoursTotal, this.EditedWishlistBook.BookMinutesTotal);
                var result = await this.View.ShowPopupAsync<TimeSpan>(totalTimePopup);
                this.EditedWishlistBook.TotalTimeSpan = result.Result;

                this.EditedWishlistBook.BookHoursTotal = result.Result.Hours;
                this.EditedWishlistBook.BookMinutesTotal = result.Result.Minutes;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Show filter list popup for book formats.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookFormatChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.SelectABookFormat,
                    [.. this.BookFormats!],
                    this.EditedWishlistBook.BookFormat,
                    false);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.EditedWishlistBook.BookFormat = result.Result;
                    this.SelectedBookFormat = this.EditedWishlistBook.BookFormat ?? AppStringResources.SelectABookFormat;
                    await this.ValidateBookFormat();
                }

                if (!this.SelectedBookFormat.Equals(AppStringResources.Audiobook))
                {
                    this.ShowPages = true;
                    this.ShowTime = false;
                }

                if (this.SelectedBookFormat.Equals(AppStringResources.Audiobook))
                {
                    this.ShowPages = false;
                    this.ShowTime = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Set data for filter popup.
        /// </summary>
        /// <param name="viewModel">Filter popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override FilterPopupViewModel SetFilterPopupValues(FilterPopupViewModel viewModel)
        {
            return viewModel;
        }

        /// <summary>
        /// Set data for filter popup.
        /// </summary>
        /// <param name="viewModel">Filter popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override FilterPopupViewModel SetFilterPopupLists(FilterPopupViewModel viewModel)
        {
            return viewModel;
        }

        /// <summary>
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel)
        {
            return viewModel;
        }

        private static bool AddWishListBookToStaticList(WishlistBookModel book, ObservableCollection<WishlistBookModel> bookList, ObservableCollection<WishlistBookModel>? filteredBookList)
        {
            var refresh = false;

            try
            {
                var oldBook = bookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                if (oldBook != null)
                {
                    var index = bookList.IndexOf(oldBook);
                    bookList.Remove(oldBook);
                    bookList.Insert(index, book);
                    refresh = true;
                }
                else
                {
                    bookList.Add(book);
                    refresh = true;
                }

                if (filteredBookList != null)
                {
                    var filteredBook = filteredBookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                    if (filteredBook != null)
                    {
                        var index = filteredBookList.IndexOf(filteredBook);
                        filteredBookList.Remove(filteredBook);
                        filteredBookList.Insert(index, book);
                        refresh = true;
                    }
                    else
                    {
                        filteredBookList.Add(book);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        private void ValidateEntry()
        {
            this.BookTitleNotValid = string.IsNullOrEmpty(this.EditedWishlistBook.BookTitle);
            this.BookFormatNotValid = string.IsNullOrEmpty(this.EditedWishlistBook.BookFormat);
        }
    }
}
