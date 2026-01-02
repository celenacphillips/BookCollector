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
        public bool bookTitleValid;

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

        public WishListBookEditViewModel(WishlistBookModel book, ContentPage view)
        {
            this.View = view;

            this.EditedWishlistBook = (WishlistBookModel)book.Clone();
            this.InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{this.EditedWishlistBook.BookTitle}")}";

            this.PopupWidth = this.DeviceWidth - 50;
        }

        public bool RemoveMainViewBefore { get; set; }

        public WishListBookMainView? MainViewBefore { get; set; }

        public double PopupWidth { get; set; }

        public async Task SetViewModelData()
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

                if (!string.IsNullOrEmpty(this.EditedWishlistBook.BookCoverFileLocation))
                {
                    this.EditedWishlistBook.BookCover = ImageSource.FromFile(this.EditedWishlistBook.BookCoverFileLocation);
                }

                if (!string.IsNullOrEmpty(this.EditedWishlistBook.BookCoverUrl))
                {
                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        this.EditedWishlistBook.BookCover = new UriImageSource
                        {
                            Uri = new Uri(this.EditedWishlistBook.BookCoverUrl),
                            CachingEnabled = true,
                            CacheValidity = TimeSpan.FromDays(1),
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
                };

                await Task.WhenAll(authors);

                this.AuthorList = authors.Result;

                await Task.WhenAll(loadDataTasks);

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

                if (!this.BookTitleValid || this.BookFormatNotValid)
                {
                    if (!this.BookTitleValid)
                    {
                        await DisplayMessage(AppStringResources.BookTitleNotValid, null);
                    }

                    if (this.BookFormatNotValid)
                    {
                        await DisplayMessage(AppStringResources.BookFormatNotValid, null);
                    }
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

                    if (TestData.UseTestData)
                    {
                        TestData.UpdateWishListBook(this.EditedWishlistBook);
                    }
                    else
                    {
                        this.EditedWishlistBook = await Database.SaveWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.EditedWishlistBook));
                        AddToStaticList(this.EditedWishlistBook);
                    }

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

                            this.EditedWishlistBook.BookCoverFileLocation = filePath;
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
                                CacheValidity = TimeSpan.FromDays(1),
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
            this.EditedWishlistBook.BookCoverFileLocation = null;
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
            if (string.IsNullOrEmpty(this.EditedWishlistBook.BookTitle))
            {
                var bookTitleEditor = this.View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                bookTitleEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                this.BookTitleValid = false;
            }
            else
            {
                var userAppTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme : Application.Current?.UserAppTheme;

                var bookTitleEditor = this.View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                bookTitleEditor.PlaceholderColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.BookTitleValid = true;
            }

            if (string.IsNullOrEmpty(this.EditedWishlistBook.BookFormat))
            {
                this.BookFormatNotValid = true;
            }
            else
            {
                this.BookFormatNotValid = false;
            }
        }

        public static void AddToStaticList(WishlistBookModel book)
        {
            if (WishListViewModel.fullWishlistBookList != null)
            {
                WishListViewModel.RefreshView = AddWishListBookToStaticList(book, WishListViewModel.fullWishlistBookList, WishListViewModel.filteredWishlistBookList);
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
