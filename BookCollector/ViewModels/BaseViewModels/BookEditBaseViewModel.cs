// <copyright file="BookEditBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
    using BookCollector.CustomPermissions;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.Views.Book;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// BookEditBaseViewModel class.
    /// </summary>
    public abstract partial class BookEditBaseViewModel : BookBaseViewModel
    {
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
        /// Gets or sets a value indicating whether the first section in book info is not open or not.
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

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static bool RefreshView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove the main view before.
        /// </summary>
        public bool RemoveMainViewBefore { get; set; }

        /// <summary>
        /// Gets or sets the main view before, to return to after closing the popup.
        /// </summary>
        public object? MainViewBefore { get; set; }

        /********************************************************/

        /// <summary>
        /// Set hours.
        /// </summary>
        /// <param name="time">Total time span.</param>
        /// <returns>Hours.</returns>
        public static int SetHours(TimeSpan time)
        {
            return (time.Days * 24) + time.Hours;
        }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
            if (!RefreshView)
            {
                return;
            }

            this.SetRefreshView(false);

            try
            {
                await this.SetIsBusyTrue();

                this.GetPreferences();

                await this.SetLists();

                this.SetSectionValues();

                await this.CheckBookFormat();

                List<string?> bookStrings = (List<string?>)this.GetBookData("strings");

                this.BookCover = await CheckBookCover(bookStrings[0], bookStrings[1]);

                await this.SetViewData();

                await this.SetAuthorData();

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                await this.ViewModelCatch(ex);
            }
        }

        /********************************************************/

        /// <summary>
        /// Save the book to the database and return to the previous view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SaveBook()
        {
            try
            {
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
                    await this.SetIsBusyTrue();

                    await this.SetBookDataForSaving();

#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    await this.SaveData();

                    var view = this.SetReturnView();
                    Shell.Current.Navigation.InsertPageBefore(view, this.View);

                    await Shell.Current.Navigation.PopAsync();

                    this.SetIsBusyFalse();
                }
            }
            catch (Exception ex)
            {
                await this.ViewModelCatch(ex);
                this.SetRefreshView(false);
            }
        }

        /// <summary>
        /// Show book search view to search for a book and fill the book info from the search result.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookSearch()
        {
            await this.SetIsBusyTrue();

            var book = this.GetBookData(null);

            var view = new BookSearchView(null, null, null, book, this);

            await Shell.Current.Navigation.PushModalAsync(view);
            this.SetIsBusyFalse();
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
                var book = this.GetBookData(null);
                string? format = string.Empty;

                if (book.GetType().ToString().Contains("WishlistBookModel"))
                {
                    format = ((WishlistBookModel)book).BookFormat;
                }
                else
                {
                    format = ((BookModel)book).BookFormat;
                }

                var filterablePopup = new FilterableListPopup(
                    AppStringResources.SelectABookFormat,
                    [.. this.BookFormats!],
                    format,
                    false);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.SetBookFormat(result.Result);
                    this.SelectedBookFormat = result.Result ?? AppStringResources.SelectABookFormat;
                    await this.ValidateBookFormat();
                }

                await this.CheckBookFormat();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Show popup with time entries to set the total time, and update the reading progress
        /// values and checkpoints visibility.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task TotalTimePopup()
        {
            var book = this.GetBookData(null);
            int hours = 0;
            int minutes = 0;

            if (book.GetType().ToString().Contains("WishlistBookModel"))
            {
                hours = ((WishlistBookModel)book).BookHoursTotal;
                minutes = ((WishlistBookModel)book).BookMinutesTotal;
            }
            else
            {
                hours = ((BookModel)book).BookHoursTotal;
                minutes = ((BookModel)book).BookMinutesTotal;
            }

            var totalTimePopup = new TimePopup(
                AppStringResources.TotalTime,
                this.PopupWidth,
                hours,
                minutes);
            var result = await this.View.ShowPopupAsync<TimeSpan>(totalTimePopup);

            if (!result.WasDismissedByTappingOutsideOfPopup)
            {
                await this.SetTotalTime(result.Result);
            }
        }

        /// <summary>
        /// Show popup to choose between adding a cover photo by picking an existing file
        /// or by entering an image url, and set the book cover accordingly.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {
            List<string> actions = [AppStringResources.UploadExistingFile, AppStringResources.BookCoverUrl];
            var action = await this.PopupActionMenu(AppStringResources.AddOrReplaceCoverPhoto, actions);

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.UploadExistingFile))
            {
                await this.UploadCover();
            }

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.BookCoverUrl))
            {
                await this.DownloadCover();
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
            this.SetBookCover(null, null, null);
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

        /********************************************************/

        /// <summary>
        /// Validate data entry.
        /// </summary>
        public abstract void ValidateEntry();

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        public abstract void GetPreferences();

        /// <summary>
        /// Set the view model lists.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetLists();

        /// <summary>
        /// Set section values.
        /// </summary>
        public abstract void SetSectionValues();

        /// <summary>
        /// Get book data for other methods.
        /// </summary>
        /// <param name="returnData">Return type.</param>
        /// <returns>An object of book data.</returns>
        public abstract object GetBookData(string? returnData);

        /// <summary>
        /// Check book format and set values.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task CheckBookFormat();

        /// <summary>
        /// Set other view data.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetViewData();

        /// <summary>
        /// Set author data.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetAuthorData();

        /// <summary>
        /// Set book format.
        /// </summary>
        /// <param name="format">Book format.</param>
        public abstract void SetBookFormat(string format);

        /// <summary>
        /// Set total time.
        /// </summary>
        /// <param name="time">Total time span.</param>
        /// <returns>A task.</returns>
        public abstract Task SetTotalTime(TimeSpan time);

        /// <summary>
        /// Set book cover.
        /// </summary>
        /// <param name="imageSource">Book cover image source.</param>
        /// <param name="fileName">Book cover image filename.</param>
        /// <param name="fileUrl">Book cover image url.</param>
        public abstract void SetBookCover(ImageSource? imageSource, string? fileName, string? fileUrl);

        /// <summary>
        /// Set book data for saving.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetBookDataForSaving();

        /// <summary>
        /// Save book data.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SaveData();

        /// <summary>
        /// Set return view.
        /// </summary>
        /// <returns>Page to return to.</returns>
        public abstract ContentPage SetReturnView();

        /********************************************************/

        private async Task UploadCover()
        {
            await this.SetIsBusyTrue();

            PermissionStatus storageReadStatus = await Permissions.CheckStatusAsync<Permissions.Media>();

            if (storageReadStatus != PermissionStatus.Granted)
            {
                storageReadStatus = await Permissions.RequestAsync<Permissions.Media>();
            }

            if (storageReadStatus == PermissionStatus.Granted)
            {
                MediaPickerOptions pickerOptions = new ();

                try
                {
                    var photos = await MediaPicker.PickPhotosAsync(pickerOptions);

                    if (photos?.Count > 0)
                    {
                        var firstPhoto = photos.First();

                        var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        var fi = new FileInfo(firstPhoto.FullPath);
                        var filePath = $"{directory}/{fi.Name}";
                        File.Copy(firstPhoto.FullPath, filePath, true);
                        this.SetBookCover(ImageSource.FromFile(firstPhoto.FullPath), fi.Name, null);
                    }
                }
                catch (Exception ex)
                {
                    this.SetIsBusyFalse();
                    this.SetBookCover(null, null, null);
                    await this.DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                }
            }
            else
            {
                await this.DisplayMessage(AppStringResources.PleaseAllowPhotoPermissionToAddCover, null);
            }

            this.SetIsBusyFalse();
        }

        private async Task DownloadCover()
        {
            var book = this.GetBookData(null);
            string? bookCoverUrlInput = string.Empty;

            if (book.GetType().ToString().Contains("WishlistBookModel"))
            {
                bookCoverUrlInput = ((WishlistBookModel)book).BookCoverUrl;
            }
            else
            {
                bookCoverUrlInput = ((BookModel)book).BookCoverUrl;
            }

            var result = await this.View.ShowPopupAsync<string>(new BookCoverUrlPopup(this.PopupWidth, bookCoverUrlInput));
            var bookCoverUrl = result.Result;

            if (!string.IsNullOrEmpty(bookCoverUrl))
            {
                await this.SetIsBusyTrue();

                PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                if (internetStatus != PermissionStatus.Granted)
                {
                    internetStatus = await Permissions.RequestAsync<InternetPermission>();
                }

                if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    try
                    {
                        this.SetBookCover(
                            new UriImageSource
                            {
                                Uri = new Uri(bookCoverUrl),
                                CachingEnabled = true,
                                CacheValidity = TimeSpan.FromDays(14),
                            },
                            null,
                            bookCoverUrl);

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
                        this.SetIsBusyFalse();
                        this.SetBookCover(null, null, null);
                        await this.DisplayMessage(AppStringResources.AnErrorOccurred, AppStringResources.ErrorDownloadingImage);
                    }
                }
            }
        }
    }
}
