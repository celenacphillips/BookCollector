using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Book;
using BookCollector.ViewModels.Series;
using BookCollector.Views.Book;
using BookCollector.Views.Collection;
using BookCollector.Views.Genre;
using BookCollector.Views.Location;
using BookCollector.Views.Popups;
using BookCollector.Views.Series;
using BookCollector.Views.WishListBook;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace BookCollector.ViewModels.WishListBook
{
    public partial class WishListBookEditViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public BookModel editedBook;

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

        public bool RemoveMainViewBefore { get; set; }

        public WishListBookMainView? MainViewBefore { get; set; }

        public double PopupWidth { get; set; }

        public WishListBookEditViewModel(BookModel book, ContentPage view)
        {
            View = view;

            EditedBook = (BookModel)book.Clone();
            InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{EditedBook.BookTitle}")}";

            PopupWidth = DeviceWidth - 50;
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                ValidateEntry();

                BookInfo1SectionValue = true;
                AuthorListSectionValue = true;
                BookInfoSectionValue = true;
                SummarySectionValue = true;
                CommentsSectionValue = true;

                if (!string.IsNullOrEmpty(EditedBook.BookCoverFileLocation) && EditedBook.BookCover == null)
                {
                    var imageBytes = File.ReadAllBytes(EditedBook.BookCoverFileLocation);
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    EditedBook.BookCover = imageSource;
                }

                if (!string.IsNullOrEmpty(EditedBook.BookCoverUrl) && EditedBook.BookCover == null)
                {
                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        var byteArray = DownloadImage(EditedBook.BookCoverUrl);
                        EditedBook.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                    }
                }

                BookCover = EditedBook.BookCover;

                AuthorList = !string.IsNullOrEmpty(EditedBook.AuthorListString) ? ParseOutAuthorsFromString(EditedBook.AuthorListString) : [];

                Task.WaitAll(
                [
                    Task.Run (async () => EditedBook.SetCoverDisplay() ),
                    Task.Run (async () => await EditedBook.SetBookPrice() ),
                    Task.Run (async () => await BookInfo1Changed() ),
                    Task.Run (async () => await ReadingDataChanged() ),
                    Task.Run (async () => await ChapterListChanged() ),
                    Task.Run (async () => await AuthorListChanged() ),
                    Task.Run (async () => await BookInfoChanged() ),
                    Task.Run (async () => await SummaryChanged() ),
                    Task.Run (async () => await CommentsChanged() ),
                ]);

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
        public async Task BookSearch()
        {
            SetIsBusyTrue();

            var view = new BookSearchView();
            var bindingContext = new BookSearchViewModel(null, view)
            {
                ViewTitle = $"{AppStringResources.BookSearch}",
                SelectedBook = EditedBook
            };
            view.BindingContext = bindingContext;

            await Shell.Current.Navigation.PushModalAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveBook()
        {
            if (!BookTitleValid && BookFormatNotValid)
            {
                if (!BookTitleValid)
                {
                    await DisplayMessage(AppStringResources.BookTitleNotValid, null);
                }

                if (BookFormatNotValid)
                {
                    await DisplayMessage(AppStringResources.BookFormatNotValid, null);
                }
            }
            else
            {
                Task.WaitAll(
                [
                    Task.Run (async () => await EditedBook.SetPartOfSeries() ),
                    Task.Run (async () => EditedBook.SetCoverDisplay() ),
                    Task.Run (async () => await EditedBook.SetBookPrice() ),
                    Task.Run (async () => await EditedBook.SetAuthorListString(AuthorList, false) ),
                ]);

#if ANDROID
                if (Platform.CurrentActivity != null &&
                    Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (!string.IsNullOrEmpty(ViewTitle) && ViewTitle.Equals($"{AppStringResources.AddNewBook}"))
                {
                    if (TestData.UseTestData)
                    {
                        TestData.InsertWishListBook(EditedBook);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (TestData.UseTestData)
                    {
                        TestData.UpdateWishListBook(EditedBook);
                    }
                    else
                    {

                    }
                }

                if (RemoveMainViewBefore)
                {
                    Shell.Current.Navigation.RemovePage(MainViewBefore);
                }

                var view = new WishListBookMainView(EditedBook, $"{EditedBook.BookTitle}");
                Shell.Current.Navigation.InsertPageBefore(view, View);

                await Shell.Current.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        public void BookInfo1Changed()
        {
            BookInfo1Open = BookInfo1SectionValue;
            BookInfo1NotOpen = !BookInfo1SectionValue;
        }

        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {
            var action = await PopupMenu_CoverPhoto();

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.UploadExistingFile))
            {
                SetIsBusyTrue();

                PermissionStatus storageReadStatus = await Permissions.RequestAsync<Permissions.Photos>();
                storageReadStatus = await Permissions.CheckStatusAsync<Permissions.Photos>();

                if (storageReadStatus == PermissionStatus.Granted)
                {
                    MediaPickerOptions pickerOptions = new();

                    try
                    {
                        var photos = await MediaPicker.PickPhotosAsync(pickerOptions);

                        if (photos?.Count > 0)
                        {
                            var firstPhoto = photos.First();
                            BookCover = ImageSource.FromFile(firstPhoto.FullPath);
                            EditedBook.HasBookCover = true;
                            EditedBook.HasNoBookCover = false;

                            var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                            if (!Directory.Exists(directory))
                                Directory.CreateDirectory(directory);

                            var fi = new FileInfo(firstPhoto.FullPath);
                            var filePath = $"{directory}/{fi.Name}";
                            File.Copy(firstPhoto.FullPath, filePath, true);

                            EditedBook.BookCoverFileLocation = filePath;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetIsBusyFalse();
                        await DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                    }

                    if (EditedBook.HasNoBookCover)
                        await DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                }
                else
                {
                    await DisplayMessage(AppStringResources.PleaseAllowPhotoPermissionToAddCover, null);
                }


                SetIsBusyFalse();
            }

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.BookCoverUrl))
            {
                var result = await View.ShowPopupAsync<string>(new BookCoverUrlPopup(PopupWidth, EditedBook.BookCoverUrl));
                var bookCoverUrl = result.Result;

                if (!string.IsNullOrEmpty(bookCoverUrl))
                {
                    SetIsBusyTrue();

                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        try
                        {
                            var byteArray = DownloadImage(bookCoverUrl);
                            BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                            EditedBook.BookCover = BookCover;
                            EditedBook.HasBookCover = true;
                            EditedBook.HasNoBookCover = false;
                            EditedBook.BookCoverUrl = bookCoverUrl;

                            SetIsBusyFalse();
                        }
                        catch (Exception ex)
                        {
                            SetIsBusyFalse();
                            BookCover = null;
                            EditedBook.BookCover = null;
                            EditedBook.HasBookCover = false;
                            EditedBook.HasNoBookCover = true;
                            EditedBook.BookCoverUrl = null;
                            await DisplayMessage(AppStringResources.AnErrorOccurred, AppStringResources.ErrorDownloadingImage);
                        }
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                        SetIsBusyFalse();
                    }

                }
            }
        }

        [RelayCommand]
        public void RemoveCoverPhoto()
        {
            EditedBook.HasBookCover = false;
            EditedBook.HasNoBookCover = true;
            EditedBook.BookCover = null;
            EditedBook.BookCoverUrl = null;
            EditedBook.BookCoverFileLocation = null;
        }

        [RelayCommand]
        public void AddAuthor()
        {
            AuthorList ??= [];

            AuthorList.Add(new AuthorModel());
        }

        [RelayCommand]
        public void RemoveAuthor(AuthorModel author)
        {
            AuthorList?.Remove(author);
        }

        [RelayCommand]
        public void ValidateBookFormat()
        {
            ValidateEntry();
        }

        [RelayCommand]
        public void ValidateBookTitle()
        {
            ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(EditedBook.BookTitle))
            {
                var bookTitleEditor = View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                bookTitleEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                BookTitleValid = false;
            }
            else
            {
                var bookTitleEditor = View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                bookTitleEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                BookTitleValid = true;
            }

            if (string.IsNullOrEmpty(EditedBook.BookFormat))
                BookFormatNotValid = true;
            else
                BookFormatNotValid = false;
        }
    }
}
