using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Book;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using BookCollector.Views.WishListBook;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        public WishListBookEditViewModel(BookModel book, ContentPage view)
        {
            this.View = view;

            this.EditedBook = (BookModel)book.Clone();
            this.InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{this.EditedBook.BookTitle}")}";

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

                this.ValidateEntry();

                this.BookInfo1SectionValue = true;
                this.AuthorListSectionValue = true;
                this.BookInfoSectionValue = true;
                this.SummarySectionValue = true;
                this.CommentsSectionValue = true;

                if (!string.IsNullOrEmpty(this.EditedBook.BookCoverFileLocation) && this.EditedBook.BookCover == null)
                {
                    var imageBytes = File.ReadAllBytes(this.EditedBook.BookCoverFileLocation);
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    this.EditedBook.BookCover = imageSource;
                }

                if (!string.IsNullOrEmpty(this.EditedBook.BookCoverUrl) && this.EditedBook.BookCover == null)
                {
                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        var byteArray = DownloadImage(this.EditedBook.BookCoverUrl);
                        this.EditedBook.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                    }
                }

                this.BookCover = this.EditedBook.BookCover;

                this.AuthorList = !string.IsNullOrEmpty(this.EditedBook.AuthorListstring) ? ParseOutAuthorsFromstring(this.EditedBook.AuthorListstring) : [];

                Task.WaitAll(
               [
                    Task.Run(async () => this.EditedBook.SetCoverDisplay()),
                    Task.Run(async () => await this.EditedBook.SetBookPrice()),
                    Task.Run(() => this.BookInfo1Changed()),
                    Task.Run(() => this.ReadingDataChanged()),
                    Task.Run(() => this.ChapterListChanged()),
                    Task.Run(() => this.AuthorListChanged()),
                    Task.Run(() => this.BookInfoChanged()),
                    Task.Run(() => this.SummaryChanged()),
                    Task.Run(() => this.CommentsChanged()),
                ]);

                this.SetIsBusyFalse();
            }
            catch (Exception)
            {
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
                SelectedBook = this.EditedBook,
            };
            view.BindingContext = bindingContext;

            await Shell.Current.Navigation.PushModalAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveBook()
        {
            if (!this.BookTitleValid && this.BookFormatNotValid)
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
                Task.WaitAll(
               [
                    Task.Run(async () => await this.EditedBook.SetPartOfSeries()),
                    Task.Run(async () => this.EditedBook.SetCoverDisplay()),
                    Task.Run(async () => await this.EditedBook.SetBookPrice()),
                    Task.Run(async () => await this.EditedBook.SetAuthorListstring(this.AuthorList, false)),
                ]);

#if ANDROID
                if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                {
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
                }
#endif

                if (!string.IsNullOrEmpty(this.ViewTitle) && this.ViewTitle.Equals($"{AppStringResources.AddNewBook}"))
                {
                    if (TestData.UseTestData)
                    {
                        TestData.InsertWishListBook(this.EditedBook);
                    }
                    else
                    {
                    }
                }
                else
                {
                    if (TestData.UseTestData)
                    {
                        TestData.UpdateWishListBook(this.EditedBook);
                    }
                    else
                    {
                    }
                }

                if (this.RemoveMainViewBefore)
                {
                    Shell.Current.Navigation.RemovePage(this.MainViewBefore);
                }

                var view = new WishListBookMainView(this.EditedBook, $"{this.EditedBook.BookTitle}");
                Shell.Current.Navigation.InsertPageBefore(view, this.View);

                await Shell.Current.Navigation.PopAsync();
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
                            this.EditedBook.HasBookCover = true;
                            this.EditedBook.HasNoBookCover = false;

                            var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            var fi = new FileInfo(firstPhoto.FullPath);
                            var filePath = $"{directory}/{fi.Name}";
                            File.Copy(firstPhoto.FullPath, filePath, true);

                            this.EditedBook.BookCoverFileLocation = filePath;
                        }
                    }
                    catch (Exception)
                    {
                        this.SetIsBusyFalse();
                        await DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                    }

                    if (this.EditedBook.HasNoBookCover)
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
                var result = await this.View.ShowPopupAsync<string>(new BookCoverUrlPopup(this.PopupWidth, this.EditedBook.BookCoverUrl));
                var bookCoverUrl = result.Result;

                if (!string.IsNullOrEmpty(bookCoverUrl))
                {
                    this.SetIsBusyTrue();

                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        try
                        {
                            var byteArray = DownloadImage(bookCoverUrl);
                            this.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                            this.EditedBook.BookCover = this.BookCover;
                            this.EditedBook.HasBookCover = true;
                            this.EditedBook.HasNoBookCover = false;
                            this.EditedBook.BookCoverUrl = bookCoverUrl;

                            this.SetIsBusyFalse();
                        }
                        catch (Exception)
                        {
                            this.SetIsBusyFalse();
                            this.BookCover = null;
                            this.EditedBook.BookCover = null;
                            this.EditedBook.HasBookCover = false;
                            this.EditedBook.HasNoBookCover = true;
                            this.EditedBook.BookCoverUrl = null;
                            await DisplayMessage(AppStringResources.AnErrorOccurred, AppStringResources.ErrorDownloadingImage);
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
            this.EditedBook.HasBookCover = false;
            this.EditedBook.HasNoBookCover = true;
            this.EditedBook.BookCover = null;
            this.EditedBook.BookCoverUrl = null;
            this.EditedBook.BookCoverFileLocation = null;
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
            if (string.IsNullOrEmpty(this.EditedBook.BookTitle))
            {
                var bookTitleEditor = this.View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                bookTitleEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                this.BookTitleValid = false;
            }
            else
            {
                var bookTitleEditor = this.View.FindByName<Editor>("BookTitleEditor");
                bookTitleEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                bookTitleEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.BookTitleValid = true;
            }

            if (string.IsNullOrEmpty(this.EditedBook.BookFormat))
            {
                this.BookFormatNotValid = true;
            }
            else
            {
                this.BookFormatNotValid = false;
            }
        }
    }
}
