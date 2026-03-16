// <copyright file="BaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
#if ANDROID
    using Android.OS;
    using AndroidX.Core.View;
#endif
    using System.Collections;
    using System.Collections.ObjectModel;
    using BookCollector.CustomPermissions;
    using BookCollector.Data.Database;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Library;
    using BookCollector.ViewModels.Main;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Alerts;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// BaseViewModel class.
    /// </summary>
    public abstract partial class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether the view is refreshing or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool isRefreshing;

        /// <summary>
        /// Gets or sets a value indicating whether the view is busy or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool isBusy;

        /// <summary>
        /// Gets or sets a value indicating whether the view is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool isVisible;

        /// <summary>
        /// Gets or sets a value indicating whether the view is enabled or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool isEnabled;

        /// <summary>
        /// Gets or sets the view title.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? viewTitle;

        /// <summary>
        /// Gets or sets the view info text.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string infoText;

        /// <summary>
        /// Gets or sets the search input string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? searchString;

        /// <summary>
        /// Gets or sets the BaseViewModel.Database.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Database connection")]
        internal static BookCollectorDatabase Database;

        /// <summary>
        /// Gets or sets a value indicating whether to show the collection view footer or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        internal static bool showCollectionViewFooter;

        /// <summary>
        /// Gets or sets the selection mode of a collection view.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        internal SelectionMode collectionViewSelectionMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel"/> class.
        /// </summary>
        public BaseViewModel()
        {
            DeviceHeight = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;
            DeviceWidth = DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;
            this.InfoText = string.Empty;
            this.View = new ContentPage();
            MaxViewWidth = DeviceWidth - 20;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static bool RefreshView { get; set; }

        /// <summary>
        /// Gets or sets the device height.
        /// </summary>
        public static double DeviceHeight { get; set; }

        /// <summary>
        /// Gets or sets the device width.
        /// </summary>
        public static double DeviceWidth { get; set; }

        /// <summary>
        /// Gets or sets the max view width.
        /// </summary>
        public static double MaxViewWidth { get; set; }

        /// <summary>
        /// Gets or sets collection view height.
        /// </summary>
        public double CollectionViewHeight { get; set; }

        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        public ContentPage View { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ascending is checked or not.
        /// </summary>
        public bool AscendingChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether descending is checked or not.
        /// </summary>
        public bool DescendingChecked { get; set; }

        /// <summary>
        /// Download an image from a URL and return byte array.
        /// </summary>
        /// <param name="imageURL">Image URL to download.</param>
        /// <returns>Byte arrray of downloaded image.</returns>
        public static byte[] DownloadImage(string imageURL)
        {
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30),
            };

            return client.GetByteArrayAsync(imageURL).Result;
        }

        /// <summary>
        /// Convert one object to another.
        /// </summary>
        /// <param name="source">Object to convert.</param>
        /// <typeparam name="T">Object to convert to.</typeparam>
        /// <returns>Converted object.</returns>
        public static T ConvertTo<T>(object source)
            where T : new()
        {
            var destination = new T();
            if (source != null)
            {
                var sourceProps = source.GetType().GetProperties();
                var destProps = typeof(T).GetProperties();

                foreach (var sourceProp in sourceProps)
                {
                    var destProp = destProps.FirstOrDefault(p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
                    if (destProp != null && destProp.SetMethod != null)
                    {
                        destProp.SetValue(destination, sourceProp.GetValue(source));
                    }
                }
            }

            return destination;
        }

        /// <summary>
        /// Set the hidden items list based on the full list and the show hidden preference.
        /// </summary>
        /// <param name="source">Full list to filter.</param>
        /// <param name="showHidden">Show hidden.</param>
        /// <typeparam name="T">Object to convert to.</typeparam>
        /// <returns>A list filtered based on the hidden parameter.</returns>
        public static List<T> SetList<T>(object source, bool showHidden)
            where T : new()
        {
            var destination = new List<T>();
            if (source != null)
            {
                var listType = source.GetType();
                var elementType = listType.GetGenericArguments()[0];

                var sourceProps = elementType.GetProperties();
                var destProps = typeof(T).GetProperties();

                var hideProp = elementType
                        .GetProperties()
                        .FirstOrDefault(p => p.Name.StartsWith("Hide", StringComparison.OrdinalIgnoreCase)
                                             && p.PropertyType == typeof(bool));

                foreach (var item in (IEnumerable)source)
                {
                    if (!showHidden && hideProp != null)
                    {
                        var isHidden = (bool)(hideProp.GetValue(item) ?? false);
                        if (isHidden)
                        {
                            continue; // skip hidden items
                        }
                    }

                    var dest = new T();

                    foreach (var sourceProp in sourceProps)
                    {
                        var destProp = destProps.FirstOrDefault(p =>
                            p.Name == sourceProp.Name &&
                            p.PropertyType == sourceProp.PropertyType);

                        if (destProp != null && destProp.SetMethod != null)
                        {
                            var value = sourceProp.GetValue(item);
                            destProp.SetValue(dest, value);
                        }
                    }

                    destination.Add(dest);
                }
            }

            return destination;
        }

        /// <summary>
        /// Update books in list to hide.
        /// </summary>
        /// <param name="books">Books to hide.</param>
        /// <returns>A task.</returns>
        public static async Task UpdateBooksToHide(ObservableCollection<BookModel>? books)
        {
            if (books != null)
            {
                foreach (var book in books)
                {
                    book.HideBook = true;
                    await BaseViewModel.Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                }
            }
        }

        /// <summary>
        /// Parse author list string to author list.
        /// </summary>
        /// <param name="input">String to parse.</param>
        /// <returns>A list of authors.</returns>
        public static List<AuthorModel> SplitStringIntoAuthorList(string input)
        {
            var list = new List<AuthorModel>();

            string[] authorNames = input.Split(";");

            foreach (var authorName in authorNames)
            {
                if (!string.IsNullOrEmpty(authorName.Trim()))
                {
                    string[] name = authorName.Split(",");

                    AuthorModel author1 = new ()
                    {
                        FirstName = name[1].Trim(),
                        LastName = name[0].Trim(),
                    };

                    list.Add(author1);
                }
            }

            return list;
        }

        /// <summary>
        /// Copy the tapped text.
        /// </summary>
        /// <param name="input">Text to copy to clipboard.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public static async Task Tap(object input)
        {
            if (!string.IsNullOrEmpty(input.ToString()))
            {
                await Clipboard.SetTextAsync(input.ToString());
                await Toast.Make($"{AppStringResources.TextCopied}").Show();
            }
        }

        /// <summary>
        /// Set the book cover image source of a book.
        /// </summary>
        /// <param name="book">Book to set cover for.</param>
        public static async void SetBookCover(BookModel book)
        {
            if (!string.IsNullOrEmpty(book.BookCoverFileName))
            {
                var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                if (!File.Exists($"{directory}/{book.BookCoverFileName}"))
                {
                    book.HasBookCover = false;
                    book.HasNoBookCover = true;
                }
                else
                {
                    book.BookCover = ImageSource.FromFile($"{directory}/{book.BookCoverFileName}");
                }
            }

            if (!string.IsNullOrEmpty(book.BookCoverUrl))
            {
                PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                if (internetStatus != PermissionStatus.Granted)
                {
                    internetStatus = await Permissions.RequestAsync<InternetPermission>();
                }

                if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    book.BookCover = new UriImageSource
                    {
                        Uri = new Uri(book.BookCoverUrl),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(14),
                    };
                }
                else
                {
                    book.HasBookCover = false;
                    book.HasNoBookCover = true;
                }
            }
        }

        /// <summary>
        /// Set the book cover image source of a book.
        /// </summary>
        /// <param name="book">Book to set cover for.</param>
        public static async void SetBookCover(WishlistBookModel book)
        {
            if (!string.IsNullOrEmpty(book.BookCoverFileName))
            {
                var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                book.BookCover = ImageSource.FromFile($"{directory}/{book.BookCoverFileName}");
            }

            if (!string.IsNullOrEmpty(book.BookCoverUrl))
            {
                PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                if (internetStatus != PermissionStatus.Granted)
                {
                    internetStatus = await Permissions.RequestAsync<InternetPermission>();
                }

                if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    book.BookCover = new UriImageSource
                    {
                        Uri = new Uri(book.BookCoverUrl),
                        CachingEnabled = true,
                        CacheValidity = TimeSpan.FromDays(14),
                    };
                }
                else
                {
                    book.HasBookCover = false;
                    book.HasNoBookCover = true;
                }
            }
        }

        /// <summary>
        /// Clear all static lists.
        /// </summary>
        public static void ClearAllLists()
        {
            ReadingViewModel.fullBookList?.Clear();
            ReadingViewModel.hiddenFilteredBookList?.Clear();
            ReadingViewModel.filteredBookList?.Clear();
            ReadingViewModel.RefreshView = true;

            ToBeReadViewModel.fullBookList?.Clear();
            ToBeReadViewModel.hiddenFilteredBookList?.Clear();
            ToBeReadViewModel.filteredBookList?.Clear();
            ToBeReadViewModel.RefreshView = true;

            ReadViewModel.fullBookList?.Clear();
            ReadViewModel.hiddenFilteredBookList?.Clear();
            ReadViewModel.filteredBookList?.Clear();
            ReadViewModel.RefreshView = true;

            AllBooksViewModel.fullBookList?.Clear();
            AllBooksViewModel.hiddenFilteredBookList?.Clear();
            AllBooksViewModel.filteredBookList?.Clear();
            AllBooksViewModel.RefreshView = true;

            CollectionsViewModel.fullCollectionList?.Clear();
            CollectionsViewModel.hiddenFilteredCollectionList?.Clear();
            CollectionsViewModel.filteredCollectionList?.Clear();
            CollectionsViewModel.RefreshView = true;

            GenresViewModel.fullGenreList?.Clear();
            GenresViewModel.hiddenFilteredGenreList?.Clear();
            GenresViewModel.filteredGenreList?.Clear();
            GenresViewModel.RefreshView = true;

            SeriesViewModel.fullSeriesList?.Clear();
            SeriesViewModel.hiddenFilteredSeriesList?.Clear();
            SeriesViewModel.filteredSeriesList?.Clear();
            SeriesViewModel.RefreshView = true;

            AuthorsViewModel.fullAuthorList?.Clear();
            AuthorsViewModel.hiddenFilteredAuthorList?.Clear();
            AuthorsViewModel.filteredAuthorList?.Clear();
            AuthorsViewModel.RefreshView = true;

            LocationsViewModel.fullLocationList?.Clear();
            LocationsViewModel.hiddenFilteredLocationList?.Clear();
            LocationsViewModel.filteredLocationList?.Clear();
            LocationsViewModel.RefreshView = true;

            WishListViewModel.fullWishlistBookList?.Clear();
            WishListViewModel.hiddenFilteredWishlistBookList?.Clear();
            WishListViewModel.filteredWishlistBookList?.Clear();
            WishListViewModel.RefreshView = true;
        }

        /// <summary>
        /// Calculate and set height of collection view.
        /// </summary>
        /// <param name="viewHeight">Main view height.</param>
        /// <param name="headerHeight">Header height above collection view.</param>
        /// <param name="searchHeight">Search height above collection view.</param>
        /// <returns>Remaining space for collection view.</returns>
        public static double SetCollectionViewHeight(double viewHeight, double headerHeight, double searchHeight)
        {
            var padding = 20;
            double extraSpacing = 0;
            var navBarHeight = 0.0;

#if ANDROID
            var activity = Platform.CurrentActivity;
            var rootView = activity?.Window?.DecorView;

            if (rootView != null)
            {
                var insetsCompat = ViewCompat.GetRootWindowInsets(rootView);

                if (insetsCompat != null)
                {
                    // System bars = status bar + nav bar
                    var systemBars = insetsCompat.GetInsets(WindowInsetsCompat.Type.SystemBars());
                    var bottomPx = systemBars!.Bottom;

                    navBarHeight = bottomPx / activity!.Resources!.DisplayMetrics!.Density;
                }
            }

            var api = (int)Build.VERSION.SdkInt;

            // NOTE: Maybe need to add Device Height if there are too many complaints?
            if (api <= 31)
            {
                extraSpacing = 10;
                headerHeight = 0;
                searchHeight = 0;
            }

            if (api > 31)
            {
                padding = 0;
                navBarHeight /= 2;
            }
#endif

            double usableHeight =
                viewHeight
                - headerHeight
                - searchHeight
                - padding
                - extraSpacing
                - navBarHeight;

            return usableHeight;
        }

        /// <summary>
        /// Set refreshing values and reset the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    var showHidden = this.GetPreferences();

                    await this.SetList(showHidden);

                    var listNotNull = this.ListNullCheck();

                    if (listNotNull)
                    {
                        await this.SetListData();

                        await this.SetFilters();

                        await this.SetSorts();
                    }

                    this.SetViewStrings();

                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public abstract bool GetPreferences();

        /// <summary>
        /// Set the view model list.
        /// </summary>
        /// <param name="showHidden">The show hidden list preference.</param>
        /// <returns>The list show hidden preference.</returns>
        public abstract Task SetList(bool showHidden);

        /// <summary>
        /// Check if the list is null.
        /// </summary>
        /// <returns>If the list is null.</returns>
        public abstract bool ListNullCheck();

        /// <summary>
        /// Iterate through the list and set necessary data.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetListData();

        /// <summary>
        /// Find filters for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetFilters();

        /// <summary>
        /// Find sort values for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetSorts();

        /// <summary>
        /// Set data for view.
        /// </summary>
        public abstract void SetViewStrings();

        /// <summary>
        /// Show popup to edit or delete object.
        /// </summary>
        /// <param name="title">Title of popup.</param>
        /// <returns>A task.</returns>
        public async Task<string?> PopupMenu(string title)
        {
            var edit = $"{AppStringResources.Edit}";
            var delete = $"{AppStringResources.Delete}";

            var answer = await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(DeviceWidth - 50, title, string.Empty, edit, delete, "Options"));

            return answer.Result;
        }

        /// <summary>
        /// Show popup to replace book cover photo.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task<string?> PopupMenu_CoverPhoto()
        {
            var title = AppStringResources.AddOrReplaceCoverPhoto;
            var file = AppStringResources.UploadExistingFile;
            var url = AppStringResources.BookCoverUrl;

            var answer = await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(DeviceWidth - 50, title, string.Empty, file, url, "Options"));

            return answer.Result;
        }

        /// <summary>
        /// Show popup to check if user is sure they want to delete item.
        /// </summary>
        /// <param name="item">Item name to delete.</param>
        /// <returns>A task.</returns>
        public async Task<bool> DeleteCheck(string item)
        {
            var message = $"{AppStringResources.AreYouSureYouWantToDeleteItem_Question.Replace("item", item)}";

            var action = await this.DisplayMessage($"{AppStringResources.AreYouSure_Question}", message, null, null);
            return action;
        }

        /// <summary>
        /// Show popup to get user input.
        /// </summary>
        /// <param name="inputTitle">Title to show on popup.</param>
        /// <param name="inputMessage">Message to show on popup.</param>
        /// <param name="inputConfirm">Input confirm text.</param>
        /// <param name="inputDeny">Input deny text.</param>
        /// <returns>A task.</returns>
        public async Task<bool> DisplayMessage(string inputTitle, string? inputMessage = null, string? inputConfirm = null, string? inputDeny = null)
        {
            inputConfirm ??= $"{AppStringResources.Yes}";

            inputDeny ??= $"{AppStringResources.No}";

            inputMessage ??= $"{AppStringResources.AreYouSure_Question}";

            var answer = await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(DeviceWidth - 50, inputTitle, inputMessage, inputConfirm, inputDeny, "Commands"));

            if (!string.IsNullOrEmpty(answer.Result) && answer.Result.Equals(inputConfirm))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Show popup to confirm message to user.
        /// </summary>
        /// <param name="inputTitle">Title to show on popup.</param>
        /// <param name="inputMessage">Message to show on popup.</param>
        /// <returns>A task.</returns>
        public async Task DisplayMessage(string inputTitle, string? inputMessage = null)
        {
            inputMessage ??= inputTitle;

            var inputConfirm = $"{AppStringResources.OK}";

            await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(DeviceWidth - 50, inputTitle, inputMessage, inputConfirm, null, "Commands"));
        }

        /// <summary>
        /// Show cancel action popup.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task CanceledAction()
        {
            await this.DisplayMessage($"{AppStringResources.ActionCanceled}", null);
        }

        /// <summary>
        /// Show delete item confirmation popup.
        /// </summary>
        /// <param name="item">Item name to delete.</param>
        /// <returns>A task.</returns>
        public async Task ConfirmDelete(string item)
        {
            var title = $"{AppStringResources.ItemDeleted.Replace("Item", item)}.";
            var message = $"{AppStringResources.ItemWasDeleted.Replace("Item", item)}";

            await this.DisplayMessage(title, message);
        }

        /// <summary>
        /// Show info popup.
        /// </summary>
        [RelayCommand]
        public void InfoPopup()
        {
            this.View.ShowPopup(new InformationPopup(DeviceWidth - 50, this.InfoText));
        }

        /// <summary>
        /// Set values when view is busy.
        /// </summary>
        public void SetIsBusyTrue()
        {
            this.IsBusy = true;
            this.IsVisible = true;
            this.IsEnabled = false;
            this.CollectionViewSelectionMode = SelectionMode.None;
        }

        /// <summary>
        /// Set values when view is not busy.
        /// </summary>
        public void SetIsBusyFalse()
        {
            this.IsBusy = false;
            this.IsVisible = true;
            this.IsEnabled = true;
            this.CollectionViewSelectionMode = SelectionMode.Single;
        }

        /// <summary>
        /// Set values when view is refreshing.
        /// </summary>
        public void SetRefreshTrue()
        {
            this.IsRefreshing = true;
        }

        /// <summary>
        /// Set values when view is not refreshing.
        /// </summary>
        public void SetRefreshFalse()
        {
            this.IsRefreshing = false;
        }

        /// <summary>
        /// Get current date and parse into string.
        /// </summary>
        /// <returns>Parsed date string.</returns>
        internal static string GetDate()
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            return $"{year}{month.ToString().PadLeft(2, '0')}{day.ToString().PadLeft(2, '0')}";
        }
    }
}
