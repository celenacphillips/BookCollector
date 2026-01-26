// <copyright file="BookSearchViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BarcodeScanner.Mobile;
using BookCollector.CustomPermissions;
using BookCollector.Data.BookAPI;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.WishListBook;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
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
        public string totalItemsstring;

        [ObservableProperty]
        public int totalItems;

        [ObservableProperty]
        public ObservableCollection<Item>? isbnItems;

        [ObservableProperty]
        public Item? selectedISBNItem;

        [ObservableProperty]
        public bool showAddISBN;

        [ObservableProperty]
        public WishlistBookModel? selectedWishListBook;

        public BookSearchViewModel(string? inputIsbn, ContentPage view)
        {
            this.View = view;
            this.Input = inputIsbn;
            this.TotalItemsstring = $"{AppStringResources.TotalItems}: ";
            this.CollectionViewHeight = this.DeviceHeight;
        }

        public object? PreviousViewModel { get; set; }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.Search();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task Search()
        {
            this.SetIsBusyTrue();

            this.TotalItems = 0;
            this.IsbnItems = null;

            if (string.IsNullOrEmpty(this.Input))
            {
                this.SetIsBusyFalse();
                await DisplayMessage($"{AppStringResources.NoISBNEntered}", null);

                return;
            }

            this.Input = this.Input.Trim().Replace("-", string.Empty).Replace(" ", string.Empty);

            PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

            if (internetStatus != PermissionStatus.Granted)
            {
                internetStatus = await Permissions.RequestAsync<InternetPermission>();
            }

            if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    var (items, totalItems) = await GoogleBooksAPI.Search(this.Input);

                    this.SetIsBusyFalse();

                    if (items == null ||
                        items.Count == 0 ||
                        totalItems == 0)
                    {
                        await DisplayMessage(AppStringResources.UnableToFindBook.Replace("api", "Google Books API"), null);
                        this.SetIsBusyFalse();
                        this.ShowAddISBN = true;
                    }
                    else
                    {
                        this.IsbnItems = items;
                        this.TotalItems = totalItems;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayMessage($"{AppStringResources.ErrorSearchingForBook}", null);
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    this.ShowAddISBN = true;
                }
            }
            else
            {
                await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToSearch}", null);
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Scan()
        {
#if ANDROID
            Methods.SetSupportBarcodeFormat(BarcodeFormats.All);

            var allowed = await Methods.AskForRequiredPermission();

            if (allowed)
            {
                BookScanView view = new ()
                {
                    ReturnViewModel = this,
                };

                await Shell.Current.Navigation.PushModalAsync(view);
            }
            else
            {
                await DisplayMessage($"{AppStringResources.ActionCanceled}", $"{AppStringResources.PleaseAllowCameraPermissionToScanBarcodes}");
            }
#endif
        }

        [RelayCommand]
        public async Task Save()
        {
            try
            {
                this.SetData();

                if (this.PreviousViewModel.GetType().ToString().Contains("WishListBookEditViewModel"))
                {
                    var previous = (WishListBookEditViewModel)this.PreviousViewModel;
                    previous.RefreshView = true;
                }

                if (this.PreviousViewModel.GetType().ToString().Contains("BookEditViewModel") &&
                    !this.PreviousViewModel.GetType().ToString().Contains("WishListBookEditViewModel"))
                {
                    var previous = (BookEditViewModel)this.PreviousViewModel;
                    previous.RefreshView = true;
                }

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayMessage($"{AppStringResources.ErrorSavingBook}", null);
#if DEBUG
                await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
            }
        }

        [RelayCommand]
        public void BookSearch_BookCoverPopup(ImageSource imageSource)
        {
            this.BookCover = imageSource;
            this.View.ShowPopup(new BookCoverPopup(this.BookCover));
        }

        private void SetData()
        {
            if (this.ShowAddISBN)
            {
                if (this.SelectedBook != null)
                {
                    if (string.IsNullOrEmpty(this.SelectedBook.BookIdentifier))
                    {
                        this.SelectedBook.BookIdentifier = this.Input;
                    }
                }

                if (this.SelectedWishListBook != null)
                {
                    if (string.IsNullOrEmpty(this.SelectedWishListBook.BookIdentifier))
                    {
                        this.SelectedWishListBook.BookIdentifier = this.Input;
                    }
                }

                return;
            }

            if (this.SelectedBook != null && this.SelectedISBNItem != null)
            {
                if (string.IsNullOrEmpty(this.SelectedBook.BookCoverFileName) || string.IsNullOrEmpty(this.SelectedBook.BookCoverUrl))
                {
                    if (this.SelectedISBNItem.VolumeInfo?.ImageLinks != null)
                    {
                        this.SelectedBook.HasBookCover = true;

                        this.SelectedBook.BookCoverUrl = $"{this.SelectedISBNItem.VolumeInfo.ImageLinks.thumbnail}.jpg";
                    }
                    else
                    {
                        this.SelectedBook.HasBookCover = false;
                    }

                    this.SelectedBook.HasNoBookCover = !this.SelectedBook.HasBookCover;
                }

                if (string.IsNullOrEmpty(this.SelectedBook.BookTitle))
                {
                    this.SelectedBook.BookTitle = this.SelectedISBNItem.VolumeInfo?.Title;

                    if (!string.IsNullOrEmpty(this.SelectedISBNItem.VolumeInfo?.Subtitle))
                    {
                        this.SelectedBook.BookTitle += $": {this.SelectedISBNItem.VolumeInfo.Subtitle}";
                    }
                }

                if (this.SelectedISBNItem.VolumeInfo?.Authors != null)
                {
                    List<AuthorModel> authorList = [];

                    foreach (var author in this.SelectedISBNItem.VolumeInfo.Authors)
                    {
                        string firstName = author[..author.LastIndexOf(' ')];
                        string lastName = author[(author.LastIndexOf(' ') + 1) ..];

                        authorList.Add(
                            new AuthorModel()
                            {
                                FirstName = firstName,
                                LastName = lastName,
                            });
                    }

                    this.SelectedBook.SelectedAuthors ??= [];
                    this.SelectedBook.SelectedAuthors.AddRange(authorList);
                }

                if (string.IsNullOrEmpty(this.SelectedBook.BookPublisher))
                {
                    this.SelectedBook.BookPublisher = this.SelectedISBNItem.VolumeInfo?.Publisher;
                }

                if (string.IsNullOrEmpty(this.SelectedBook.BookPublishYear))
                {
                    if (!string.IsNullOrEmpty(this.SelectedISBNItem.VolumeInfo?.PublishedDate) && this.SelectedISBNItem.VolumeInfo.PublishedDate.Contains('-'))
                    {
                        this.SelectedBook.BookPublishYear = this.SelectedISBNItem.VolumeInfo.PublishedDate[..this.SelectedISBNItem.VolumeInfo.PublishedDate.IndexOf('-')];
                    }
                    else
                    {
                        this.SelectedBook.BookPublishYear = this.SelectedISBNItem.VolumeInfo?.PublishedDate;
                    }
                }

                if (this.SelectedBook.BookPageTotal <= 0 && this.SelectedISBNItem != null && this.SelectedISBNItem.VolumeInfo != null)
                {
                    this.SelectedBook.BookPageTotal = (int)this.SelectedISBNItem.VolumeInfo?.PageCount;
                }

                if (string.IsNullOrEmpty(this.SelectedBook.BookSummary) && this.SelectedISBNItem != null && this.SelectedISBNItem.VolumeInfo != null)
                {
                    this.SelectedBook.BookSummary = this.SelectedISBNItem.VolumeInfo?.Description;
                }

                if (string.IsNullOrEmpty(this.SelectedBook.BookLanguage) && this.SelectedISBNItem != null && this.SelectedISBNItem.VolumeInfo != null)
                {
                    if (!string.IsNullOrEmpty(this.SelectedISBNItem.VolumeInfo?.Language) && this.SelectedISBNItem.VolumeInfo.Language.Equals("en"))
                    {
                        this.SelectedBook.BookLanguage = $"{AppStringResources.English}";
                    }
                }

                if (string.IsNullOrEmpty(this.SelectedBook.BookIdentifier))
                {
                    this.SelectedBook.BookIdentifier = this.Input;
                }
            }

            if (this.SelectedWishListBook != null && this.SelectedISBNItem != null)
            {
                if (string.IsNullOrEmpty(this.SelectedWishListBook.BookCoverFileName) || string.IsNullOrEmpty(this.SelectedWishListBook.BookCoverUrl))
                {
                    if (this.SelectedISBNItem.VolumeInfo?.ImageLinks != null)
                    {
                        this.SelectedWishListBook.HasBookCover = true;

                        this.SelectedWishListBook.BookCoverUrl = $"{this.SelectedISBNItem.VolumeInfo.ImageLinks.thumbnail}.jpg";
                    }
                    else
                    {
                        this.SelectedWishListBook.HasBookCover = false;
                    }

                    this.SelectedWishListBook.HasNoBookCover = !this.SelectedWishListBook.HasBookCover;
                }

                if (string.IsNullOrEmpty(this.SelectedWishListBook.BookTitle))
                {
                    this.SelectedWishListBook.BookTitle = this.SelectedISBNItem.VolumeInfo?.Title;

                    if (!string.IsNullOrEmpty(this.SelectedISBNItem.VolumeInfo?.Subtitle))
                    {
                        this.SelectedWishListBook.BookTitle += $": {this.SelectedISBNItem.VolumeInfo.Subtitle}";
                    }
                }

                if (this.SelectedISBNItem.VolumeInfo?.Authors != null)
                {
                    List<AuthorModel> authorList = [];

                    foreach (var author in this.SelectedISBNItem.VolumeInfo.Authors)
                    {
                        string firstName = author[..author.LastIndexOf(' ')];
                        string lastName = author[(author.LastIndexOf(' ') + 1)..];

                        authorList.Add(
                            new AuthorModel()
                            {
                                FirstName = firstName,
                                LastName = lastName,
                            });
                    }

                    this.SelectedWishListBook.SelectedAuthors ??= [];
                    this.SelectedWishListBook.SelectedAuthors.AddRange(authorList);
                }

                if (string.IsNullOrEmpty(this.SelectedWishListBook.BookPublisher))
                {
                    this.SelectedWishListBook.BookPublisher = this.SelectedISBNItem.VolumeInfo?.Publisher;
                }

                if (string.IsNullOrEmpty(this.SelectedWishListBook.BookPublishYear))
                {
                    if (!string.IsNullOrEmpty(this.SelectedISBNItem.VolumeInfo?.PublishedDate) && this.SelectedISBNItem.VolumeInfo.PublishedDate.Contains('-'))
                    {
                        this.SelectedWishListBook.BookPublishYear = this.SelectedISBNItem.VolumeInfo.PublishedDate[..this.SelectedISBNItem.VolumeInfo.PublishedDate.IndexOf('-')];
                    }
                    else
                    {
                        this.SelectedWishListBook.BookPublishYear = this.SelectedISBNItem.VolumeInfo?.PublishedDate;
                    }
                }

                if (this.SelectedWishListBook.BookPageTotal <= 0 && this.SelectedISBNItem != null && this.SelectedISBNItem.VolumeInfo != null)
                {
                    this.SelectedWishListBook.BookPageTotal = (int)this.SelectedISBNItem.VolumeInfo?.PageCount;
                }

                if (string.IsNullOrEmpty(this.SelectedWishListBook.BookSummary) && this.SelectedISBNItem != null && this.SelectedISBNItem.VolumeInfo != null)
                {
                    this.SelectedWishListBook.BookSummary = this.SelectedISBNItem.VolumeInfo?.Description;
                }

                if (string.IsNullOrEmpty(this.SelectedWishListBook.BookLanguage) && this.SelectedISBNItem != null && this.SelectedISBNItem.VolumeInfo != null)
                {
                    if (!string.IsNullOrEmpty(this.SelectedISBNItem.VolumeInfo?.Language))
                    {
                        this.SelectedWishListBook.BookLanguage = this.SelectedISBNItem.VolumeInfo.Language.Equals("en") ? $"{AppStringResources.English}" : this.SelectedISBNItem.VolumeInfo.Language;
                    }
                }

                if (string.IsNullOrEmpty(this.SelectedWishListBook.BookIdentifier))
                {
                    this.SelectedWishListBook.BookIdentifier = this.Input;
                }
            }
        }
    }
}
