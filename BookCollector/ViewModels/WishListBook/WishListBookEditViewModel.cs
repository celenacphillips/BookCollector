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
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net;

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
            _view = view;

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
                        var byteArray = new WebClient().DownloadData($"{EditedBook.BookCoverUrl}");
                        EditedBook.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                    }
                }

                BookCover = EditedBook.BookCover;

                AuthorList = !string.IsNullOrEmpty(EditedBook.AuthorListString) ? ParseOutAuthorsFromString(EditedBook.AuthorListString) : new ObservableCollection<AuthorModel>();

                Task.WaitAll(
                [
                    Task.Run (async () => await EditedBook.SetCoverDisplay() ),
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

            BookSearchView view = new BookSearchView();
            BookSearchViewModel bindingContext = new BookSearchViewModel(null, view);
            bindingContext.ViewTitle = $"{AppStringResources.BookSearch}";
            bindingContext.SelectedBook = EditedBook;
            view.BindingContext = bindingContext;

            await Shell.Current.Navigation.PushModalAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveBook()
        {
            if (BookTitleValid && !BookFormatNotValid)
            {
                Task.WaitAll(
                [
                    Task.Run (async () => await EditedBook.SetPartOfSeries() ),
                    Task.Run (async () => await EditedBook.SetCoverDisplay() ),
                    Task.Run (async () => await EditedBook.SetBookPrice() ),
                    Task.Run (async () => await EditedBook.SetAuthorListString(AuthorList, false) ),
                ]);

#if ANDROID
                if (Platform.CurrentActivity != null &&
                    Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (ViewTitle.Equals($"{AppStringResources.AddNewBook}"))
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

                WishListBookMainView view = new WishListBookMainView(EditedBook, $"{EditedBook.BookTitle}");
                Shell.Current.Navigation.InsertPageBefore(view, _view);

                await Shell.Current.Navigation.PopAsync();
            }
            else
            {
                if (!BookTitleValid)
                    await Shell.Current.DisplayAlert("Book Title not valid", "Book Title not valid", $"{AppStringResources.OK}");

                if (BookFormatNotValid)
                    await Shell.Current.DisplayAlert("Book Format not valid", "Book Format not valid", $"{AppStringResources.OK}");
            }
        }

        [RelayCommand]
        public async Task BookInfo1Changed()
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
                        var result = await MediaPicker.PickPhotoAsync(pickerOptions);

                        if (result != null)
                        {
                            BookCover = ImageSource.FromFile(result.FullPath);
                            EditedBook.HasBookCover = true;
                            EditedBook.HasNoBookCover = false;

                            var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                            if (!Directory.Exists(directory))
                                Directory.CreateDirectory(directory);

                            var fi = new FileInfo(result.FullPath);
                            var filePath = $"{directory}/{fi.Name}";
                            File.Copy(result.FullPath, filePath, true);

                            EditedBook.BookCoverFileLocation = filePath;
                        }
                    }
                    catch (Exception ex)
                    {
                        SetIsBusyFalse();
                        await Shell.Current.DisplayAlert(null, AppStringResources.PickingCoverCanceled, AppStringResources.OK);
                    }

                    if (EditedBook.HasNoBookCover)
                        await Shell.Current.DisplayAlert(null, AppStringResources.PickingCoverCanceled, AppStringResources.OK);
                }
                else
                {
                    await Shell.Current.DisplayAlert(null, AppStringResources.PleaseAllowPhotoPermissionToAddCover, AppStringResources.OK);
                }


                SetIsBusyFalse();
            }

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.BookCoverUrl))
            {
                var result = await _view.ShowPopupAsync(new BookCoverUrlPopup(PopupWidth, EditedBook.BookCoverUrl));
                var bookCoverUrl = (string?)result;

                if (!string.IsNullOrEmpty(bookCoverUrl))
                {
                    SetIsBusyTrue();

                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        try
                        {
                            var byteArray = new WebClient().DownloadData($"{bookCoverUrl}");
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
        public async Task RemoveCoverPhoto()
        {
            EditedBook.HasBookCover = false;
            EditedBook.HasNoBookCover = true;
            EditedBook.BookCover = null;
            EditedBook.BookCoverUrl = null;
            EditedBook.BookCoverFileLocation = null;
        }

        [RelayCommand]
        public async Task AddAuthor()
        {
            if (AuthorList == null)
                AuthorList = [];

            AuthorList.Add(new AuthorModel());
        }

        [RelayCommand]
        public async Task RemoveAuthor(AuthorModel author)
        {
            AuthorList.Remove(author);
        }

        [RelayCommand]
        public async Task ValidateBookFormat()
        {
            ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(EditedBook.BookTitle))
                BookTitleValid = false;
            else
                BookTitleValid = true;

            if (string.IsNullOrEmpty(EditedBook.BookFormat))
                BookFormatNotValid = true;
            else
                BookFormatNotValid = false;
        }
    }
}
