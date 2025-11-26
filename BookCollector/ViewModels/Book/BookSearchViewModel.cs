using BarcodeScanner.Mobile;
using BookCollector.Data.BookAPI;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Book
{
    public partial class BookSearchViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public string? input;

        [ObservableProperty]
        public string totalItemsString;

        [ObservableProperty]
        public int totalItems;

        [ObservableProperty]
        public ObservableCollection<Item>? isbnItems;

        [ObservableProperty]
        public Item? selectedISBNItem;

        [ObservableProperty]
        public bool showAddISBN;

        public BookSearchViewModel(string? inputIsbn, ContentPage view)
        {
            _view = view;
            Input = inputIsbn;
            TotalItemsString = $"{AppStringResources.TotalItems}: ";
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await Search();
            SetRefreshFalse();
        }

        // TO DO
        // Add check for internet connection - 11/20/2025
        [RelayCommand]
        public async Task Search()
        {
            SetIsBusyTrue();

            TotalItems = 0;
            IsbnItems = null;

            if (string.IsNullOrEmpty(Input))
            {
                SetIsBusyFalse();
                await Shell.Current.DisplayAlert(null, AppStringResources.NoISBNEntered, AppStringResources.OK);

                return;
            }

            Input = Input.Trim().Replace("-", "").Replace(" ", "");

            var (items, totalItems) = await GoogleBooksAPI.SearchAsync(Input);

            SetIsBusyFalse();

            if (items == null ||
                items.Count == 0 ||
                totalItems == 0)
            {
                await Shell.Current.DisplayAlert(null, AppStringResources.ErrorSearchingForBook, AppStringResources.OK);
                ShowAddISBN = true;
            }
            else
            {
                IsbnItems = items;
                TotalItems = totalItems;
            }
        }

        [RelayCommand]
        public async Task Scan()
        {
#if ANDROID
            Methods.SetSupportBarcodeFormat(BarcodeFormats.All);

            bool allowed = await Methods.AskForRequiredPermission();

            if (allowed)
            {
                BookScanView view = new();
                view.ReturnViewModel = this;

                await Shell.Current.Navigation.PushModalAsync(view);
            }
            else
                await Shell.Current.DisplayAlert(null, AppStringResources.PleaseAllowCameraPermissionToScanBarcodes, AppStringResources.OK);
#endif
        }

        [RelayCommand]
        public async Task Save()
        {
            try
            {
                SetData();
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(null, AppStringResources.ErrorSavingBook, AppStringResources.OK);
            }
        }

        [RelayCommand]
        public async Task BookSearch_BookCoverPopup(ImageSource imageSource)
        {
            BookCover = imageSource;
            _view.ShowPopup(new BookCoverPopup());
        }

        private void SetData()
        {
            if (ShowAddISBN)
            {
                if (SelectedBook != null)
                {
                    if (string.IsNullOrEmpty(SelectedBook.BookIdentifier))
                    {
                        SelectedBook.BookIdentifier = Input;
                    }
                }

                return;
            }

            if (SelectedBook != null && SelectedISBNItem != null)
            {
                if (SelectedBook.BookCoverBytes == null)
                {
                    if (SelectedISBNItem.VolumeInfo.ImageLinks != null)
                    {
                        SelectedBook.HasBookCover = true;
                        SelectedBook.BookCoverBytes = SelectedISBNItem.VolumeInfo.ImageLinks.ImageByteArray;
                    }
                    else
                        SelectedBook.HasBookCover = false;

                    SelectedBook.HasNoBookCover = !SelectedBook.HasBookCover;
                }

                if (string.IsNullOrEmpty(SelectedBook.BookTitle))
                {
                    SelectedBook.BookTitle = SelectedISBNItem.VolumeInfo.Title;

                    if (!string.IsNullOrEmpty(SelectedISBNItem.VolumeInfo.Subtitle))
                        SelectedBook.BookTitle += $": {SelectedISBNItem.VolumeInfo.Subtitle}";
                }

                if (string.IsNullOrEmpty(SelectedBook.AuthorListString))
                {
                    if (SelectedISBNItem.VolumeInfo.Authors != null)
                    {
                        List<AuthorModel> authorList = new List<AuthorModel>();

                        foreach (var author in SelectedISBNItem.VolumeInfo.Authors)
                        {
                            string firstName = author[..author.LastIndexOf(' ')];
                            string lastName = author[(author.LastIndexOf(" ") + 1)..];

                            authorList.Add(
                                new AuthorModel()
                                {
                                    FirstName = firstName,
                                    LastName = lastName
                                });
                        }

                        SelectedBook.SetAuthorListString(authorList.ToObservableCollection());
                    }
                }

                if (string.IsNullOrEmpty(SelectedBook.BookPublisher))
                {
                    SelectedBook.BookPublisher = SelectedISBNItem.VolumeInfo.Publisher;
                }

                if (string.IsNullOrEmpty(SelectedBook.BookPublishYear))
                {
                    if (SelectedISBNItem.VolumeInfo.PublishedDate.Contains("-"))
                        SelectedBook.BookPublishYear = SelectedISBNItem.VolumeInfo.PublishedDate.Substring(0, SelectedISBNItem.VolumeInfo.PublishedDate.IndexOf("-"));
                    else
                        SelectedBook.BookPublishYear = SelectedISBNItem.VolumeInfo.PublishedDate;
                }

                if (SelectedBook.BookPageTotal <= 0)
                {
                    SelectedBook.BookPageTotal = SelectedISBNItem.VolumeInfo.PageCount;
                }

                if (string.IsNullOrEmpty(SelectedBook.BookSummary))
                {
                    SelectedBook.BookSummary = SelectedISBNItem.VolumeInfo.Description;
                }

                if (string.IsNullOrEmpty(SelectedBook.BookLanguage))
                {
                    if (SelectedISBNItem.VolumeInfo.Language.Equals("en"))
                        SelectedBook.BookLanguage = $"{AppStringResources.English}";
                }

                if (string.IsNullOrEmpty(SelectedBook.BookIdentifier))
                {
                    SelectedBook.BookIdentifier = Input;
                }
            }
        }
    }
}
