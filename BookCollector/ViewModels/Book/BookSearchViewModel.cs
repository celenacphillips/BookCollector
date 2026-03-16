// <copyright file="BookSearchViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Book
{
#if ANDROID
    using BarcodeScanner.Mobile;
#endif
    using System.Collections.ObjectModel;
    using BookCollector.CustomPermissions;
    using BookCollector.Data.BookAPI;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Popups;
    using BookCollector.ViewModels.WishListBook;
    using BookCollector.Views.Book;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// BookSearchViewModel class.
    /// </summary>
    public partial class BookSearchViewModel : BookBaseViewModel
    {
        /// <summary>
        /// Gets or sets the ISBN to search.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? isbnInput;

        /// <summary>
        /// Gets or sets the title to search.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? titleInput;

        /// <summary>
        /// Gets or sets the author name to search.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? authorInput;

        /// <summary>
        /// Gets or sets a string parse for total items found.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string totalItemsstring;

        /// <summary>
        /// Gets or sets the total items found.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int totalItems;

        /// <summary>
        /// Gets or sets response items from the search.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<Item>? isbnItems;

        /// <summary>
        /// Gets or sets selected response item.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public Item? selectedISBNItem;

        /// <summary>
        /// Gets or sets a value indicating whether to show the ISBN button.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showAddISBN;

        /// <summary>
        /// Gets or sets the selected wishlist book.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public WishlistBookModel? selectedWishListBook;

        /// <summary>
        /// Gets or sets a value indicating whether the search form section is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool searchFormSectionValue;

        /// <summary>
        /// Gets or sets a value indicating whether the search form section is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool searchFormOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the search form section is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool searchFormNotOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookSearchViewModel"/> class.
        /// </summary>
        /// <param name="inputIsbn">Input ISBN to search.</param>
        /// <param name="inputTitle">Input title to search.</param>
        /// <param name="inputAuthorName">Input author name to search.</param>
        /// <param name="view">View related to view model.</param>
        public BookSearchViewModel(string? inputIsbn, string? inputTitle, string? inputAuthorName, ContentPage view)
        {
            this.View = view;
            this.IsbnInput = inputIsbn;
            this.TitleInput = inputTitle;
            this.AuthorInput = inputAuthorName;
            this.TotalItemsstring = $"{AppStringResources.TotalItems}: ";
            this.CollectionViewHeight = DeviceHeight;
            this.ShowCollectionViewFooter = false;

            this.SearchFormSectionValue = false;
            var result = this.SearchFormChanged();
        }

        /// <summary>
        /// Gets or sets previous view model to return to after closing the popup or saving the book.
        /// </summary>
        public object? PreviousViewModel { get; set; }

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SearchFormChanged()
        {
            this.SearchFormOpen = this.SearchFormSectionValue;
            this.SearchFormNotOpen = !this.SearchFormSectionValue;
        }

        /// <summary>
        /// Search api for books based on input search terms.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Search()
        {
            this.SetIsBusyTrue();

            this.SearchFormSectionValue = false;

            this.TotalItems = 0;
            this.IsbnItems = null;

            PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

            if (internetStatus != PermissionStatus.Granted)
            {
                internetStatus = await Permissions.RequestAsync<InternetPermission>();
            }

            if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    List<Item>? items = [];
                    int totalItems = 0;

                    var (combinedItems, totalCombinedItems) = await GoogleBooksAPI.CombinedSearch(this.IsbnInput, this.TitleInput, this.AuthorInput);

                    if (combinedItems != null)
                    {
                        items.AddRange(combinedItems);
                    }

                    totalItems += totalCombinedItems;

                    this.SetIsBusyFalse();

                    if (items == null ||
                        items.Count == 0 ||
                        totalItems == 0)
                    {
                        await this.DisplayMessage(AppStringResources.UnableToFindBook.Replace("api", "Google Books API"), null);
                        this.SetIsBusyFalse();
                        this.ShowAddISBN = !string.IsNullOrEmpty(this.IsbnInput);
                    }
                    else
                    {
                        this.IsbnItems = items.ToObservableCollection();
                        this.TotalItems = totalItems;
                    }

                    this.ShowCollectionViewFooter = totalItems > 0;
                }
                catch (Exception ex)
                {
                    await this.DisplayMessage($"{AppStringResources.ErrorSearchingForBook}", null);
#if DEBUG
                    await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    this.ShowAddISBN = true;
                }
            }
            else
            {
                await this.DisplayMessage($"{AppStringResources.PleaseConnectToInternetToSearch}", null);
                this.SetIsBusyFalse();
            }
        }

        /// <summary>
        /// Show popup to scan barcodes.
        /// </summary>
        /// <returns>A task.</returns>
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
                await this.DisplayMessage($"{AppStringResources.ActionCanceled}", $"{AppStringResources.PleaseAllowCameraPermissionToScanBarcodes}");
            }
#else
            BookScanView view = new ()
            {
                ReturnViewModel = this,
            };

            await Shell.Current.Navigation.PushModalAsync(view);
#endif
        }

        /// <summary>
        /// Save selected search result and return to the book edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Save()
        {
            try
            {
                this.SetData();

                if (this.PreviousViewModel != null && this.PreviousViewModel.GetType().ToString().Contains("WishListBookEditViewModel"))
                {
                    var previous = (WishListBookEditViewModel)this.PreviousViewModel;
                    WishListBookEditViewModel.RefreshView = true;
                }

                if (this.PreviousViewModel != null &&
                    this.PreviousViewModel.GetType().ToString().Contains("BookEditViewModel") &&
                    !this.PreviousViewModel.GetType().ToString().Contains("WishListBookEditViewModel"))
                {
                    var previous = (BookEditViewModel)this.PreviousViewModel;
                    BookEditViewModel.RefreshView = true;
                }

                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayMessage($"{AppStringResources.ErrorSavingBook}", null);
#if DEBUG
                await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
            }
        }

        /// <summary>
        /// Show book cover popup.
        /// </summary>
        /// <returns>A task.</returns>
        /// <param name="imageSource">The image to display.</param>
        [RelayCommand]
        public async Task BookSearch_BookCoverPopup(ImageSource imageSource)
        {
            this.BookCover = imageSource;
            this.View.ShowPopup(new BookCoverPopup(this.BookCover));
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

        private void SetData()
        {
            if (this.ShowAddISBN)
            {
                if (this.SelectedBook != null)
                {
                    if (string.IsNullOrEmpty(this.SelectedBook.BookIdentifier))
                    {
                        this.SelectedBook.BookIdentifier = this.IsbnInput;
                    }
                }

                if (this.SelectedWishListBook != null)
                {
                    if (string.IsNullOrEmpty(this.SelectedWishListBook.BookIdentifier))
                    {
                        this.SelectedWishListBook.BookIdentifier = this.IsbnInput;
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
                    this.SelectedBook.BookPageTotal = this.SelectedISBNItem.VolumeInfo?.PageCount ?? 0;
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

                if (string.IsNullOrEmpty(this.SelectedBook.BookIdentifier) && !string.IsNullOrEmpty(this.IsbnInput))
                {
                    this.SelectedBook.BookIdentifier = this.IsbnInput;
                }

                if (string.IsNullOrEmpty(this.SelectedBook.BookIdentifier) && string.IsNullOrEmpty(this.IsbnInput))
                {
                    this.SelectedBook.BookIdentifier = this.SelectedISBNItem?.VolumeInfo?.industryIdentifiers.FirstOrDefault(x => x.type.Equals("ISBN_13"))?.identifier;
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
                        string lastName = author[(author.LastIndexOf(' ') + 1) ..];

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
                    this.SelectedWishListBook.BookPageTotal = this.SelectedISBNItem.VolumeInfo?.PageCount ?? 0;
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

                if (string.IsNullOrEmpty(this.SelectedWishListBook.BookIdentifier) && !string.IsNullOrEmpty(this.IsbnInput))
                {
                    this.SelectedWishListBook.BookIdentifier = this.IsbnInput;
                }

                if (string.IsNullOrEmpty(this.SelectedWishListBook.BookIdentifier) && string.IsNullOrEmpty(this.IsbnInput))
                {
                    this.SelectedWishListBook.BookIdentifier = this.SelectedISBNItem?.VolumeInfo?.industryIdentifiers.FirstOrDefault(x => x.type.Equals("ISBN_13"))?.identifier;
                }
            }
        }
    }
}
