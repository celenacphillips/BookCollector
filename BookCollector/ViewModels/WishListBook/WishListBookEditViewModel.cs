// <copyright file="WishListBookEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Book;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Main;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using BookCollector.Views.WishListBook;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.WishListBook
{
    public partial class WishListBookEditViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public WishlistBookModel editedWishlistBook;

        [ObservableProperty]
        public bool bookTitleNotValid;

        [ObservableProperty]
        public bool bookFormatNotValid;

        [ObservableProperty]
        public bool bookInfo1SectionValue;

        [ObservableProperty]
        public bool bookInfo1Open;

        [ObservableProperty]
        public bool bookInfo1NotOpen;

        [ObservableProperty]
        public ObservableCollection<AuthorModel>? authorList;

        [ObservableProperty]
        public string selectedBookFormat;

        public WishListBookEditViewModel(WishlistBookModel book, ContentPage view)
        {
            this.View = view;

            this.EditedWishlistBook = (WishlistBookModel)book.Clone();
            this.InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{this.EditedWishlistBook.BookTitle}")}";
            this.SelectedBookFormat = this.EditedWishlistBook.BookFormat ?? AppStringResources.SelectABookFormat;
            this.PopupWidth = this.DeviceWidth - 50;
            this.RefreshView = true;
        }

        public bool RemoveMainViewBefore { get; set; }

        public WishListBookMainView? MainViewBefore { get; set; }

        public double PopupWidth { get; set; }

        public bool RefreshView { get; set; }

        public async Task SetViewModelData()
        {
            if (this.RefreshView)
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
                        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                        {
                            this.EditedWishlistBook.BookCover = new UriImageSource
                            {
                                Uri = new Uri(this.EditedWishlistBook.BookCoverUrl),
                                CachingEnabled = true,
                                CacheValidity = TimeSpan.FromDays(14),
                            };
                        }
                        else
                        {
                            await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
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
                        Task.Run(() => this.EditedWishlistBook.TotalTimeSpan = this.EditedWishlistBook.SetTime(this.EditedWishlistBook.BookHoursTotal, this.EditedWishlistBook.BookMinutesTotal)),
                    };

                    await Task.WhenAll(authors);

                    this.AuthorList = authors.Result;

                    await Task.WhenAll(loadDataTasks);

                    this.SetIsBusyFalse();
                    this.RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif
                    this.SetIsBusyFalse();
                    this.RefreshView = false;
                }
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
        public async Task BookSearch()
        {
            this.SetIsBusyTrue();

            var view = new BookSearchView();
            var bindingContext = new BookSearchViewModel(null, view)
            {
                ViewTitle = $"{AppStringResources.BookSearch}",
                SelectedBook = ConvertTo<BookModel>(this.EditedWishlistBook),
            };
            view.BindingContext = bindingContext;

            await Shell.Current.Navigation.PushModalAsync(view);

            this.SetIsBusyFalse();
        }

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
                        await DisplayMessage(AppStringResources.BookTitleNotValid, null);
                    }

                    if (this.BookFormatNotValid)
                    {
                        await DisplayMessage(AppStringResources.BookFormatNotValid, null);
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
                    AddToStaticList(this.EditedWishlistBook);

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
                await DisplayMessage("Error!", ex.Message);
#endif
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public void BookInfo1Changed()
        {
            this.BookInfo1Open = this.BookInfo1SectionValue;
            this.BookInfo1NotOpen = !this.BookInfo1SectionValue;
        }

        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {
            var action = await PopupMenu_CoverPhoto();

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
                        await DisplayMessage(AppStringResources.PickingCoverCanceled, null);
#if DEBUG
                        await DisplayMessage("Error!", ex.Message);
#endif
                    }

                    if (this.EditedWishlistBook.HasNoBookCover)
                    {
                        await DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                    }
                }
                else
                {
                    await DisplayMessage(AppStringResources.PleaseAllowPhotoPermissionToAddCover, null);
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

                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
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
                            await DisplayMessage(AppStringResources.AnErrorOccurred, AppStringResources.ErrorDownloadingImage);
#if DEBUG
                            await DisplayMessage("Error!", ex.Message);
#endif
                        }
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                        this.SetIsBusyFalse();
                    }
                }
            }
        }

        [RelayCommand]
        public void RemoveCoverPhoto()
        {
            this.EditedWishlistBook.HasBookCover = false;
            this.EditedWishlistBook.HasNoBookCover = true;
            this.EditedWishlistBook.BookCover = null;
            this.EditedWishlistBook.BookCoverUrl = null;
            this.EditedWishlistBook.BookCoverFileName = null;
        }

        [RelayCommand]
        public void AddAuthor()
        {
            this.AuthorList ??= [];

            this.AuthorList.Add(new AuthorModel());
        }

        [RelayCommand]
        public void RemoveAuthor(AuthorModel author)
        {
            this.AuthorList?.Remove(author);
        }

        [RelayCommand]
        public void ValidateBookFormat()
        {
            this.ValidateEntry();
        }

        [RelayCommand]
        public void ValidateBookTitle()
        {
            this.ValidateEntry();
        }

        private void ValidateEntry()
        {
            this.BookTitleNotValid = string.IsNullOrEmpty(this.EditedWishlistBook.BookTitle);
            this.BookFormatNotValid = string.IsNullOrEmpty(this.EditedWishlistBook.BookFormat);
        }

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

        [RelayCommand]
        public async Task BookFormatChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.SelectABookFormat,
                    BookFormats.ToList(),
                    this.EditedWishlistBook.BookFormat,
                    false);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.EditedWishlistBook.BookFormat = result.Result;
                    this.SelectedBookFormat = this.EditedWishlistBook.BookFormat ?? AppStringResources.SelectABookFormat;
                    this.ValidateBookFormat();
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

        public static void AddToStaticList(WishlistBookModel book)
        {
            if (WishListViewModel.fullWishlistBookList != null)
            {
                WishListViewModel.RefreshView = AddWishListBookToStaticList(book, WishListViewModel.fullWishlistBookList, WishListViewModel.filteredWishlistBookList2);
            }
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
    }
}
