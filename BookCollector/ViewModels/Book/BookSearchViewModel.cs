// <copyright file="BookSearchViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BarcodeScanner.Mobile;
using BookCollector.Data.BookAPI;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        public BookSearchViewModel(string? inputIsbn, ContentPage view)
        {
            this.View = view;
            this.Input = inputIsbn;
            this.TotalItemsstring = $"{AppStringResources.TotalItems}: ";
            this.CollectionViewHeight = this.DeviceHeight - this.DoubleMenuBar;
        }

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

            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
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
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayMessage($"{AppStringResources.ErrorSavingBook}", null);
#if DEBUG
                await DisplayMessage("Error!", ex.Message);
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

                return;
            }

            if (this.SelectedBook != null && this.SelectedISBNItem != null)
            {
                if (string.IsNullOrEmpty(this.SelectedBook.BookCoverFileLocation) || string.IsNullOrEmpty(this.SelectedBook.BookCoverFileLocation))
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

                if (string.IsNullOrEmpty(this.SelectedBook.AuthorListString))
                {
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

                        var variable = this.SelectedBook.SetAuthorListString(authorList.ToObservableCollection(), false);
                    }
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
        }
    }
}
