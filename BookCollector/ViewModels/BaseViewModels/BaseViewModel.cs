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
        /// Gets or sets previous view model to return to after closing the popup or saving the book.
        /// </summary>
        public object? PreviousViewModel { get; set; }

        /// <summary>
        /// Gets or sets the popup width.
        /// </summary>
        public double PopupWidth { get; set; }

        /// <summary>
        /// Gets or sets the popup height.
        /// </summary>
        public double PopupHeight { get; set; }

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
        /// Sets the time span for the book based on the book format and updates the total time span property accordingly.
        /// </summary>
        /// <param name="hour">Hour to set.</param>
        /// <param name="minute">Minute to set.</param>
        /// <returns>New time span created.</returns>
        public static TimeSpan SetTime(int hour, int minute)
        {
            return new TimeSpan(hour, minute, 0);
        }

        /// <summary>
        /// Set refreshing values and reset the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        /// <summary>
        /// Show popup to edit or delete object.
        /// </summary>
        /// <param name="title">Title of popup.</param>
        /// <param name="actions">List of actions that can be performed.</param>
        /// <returns>A task.</returns>
        public async Task<string?> PopupActionMenu(string title, List<string> actions)
        {
            var answer = await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(DeviceWidth - 50, title, string.Empty, actions, "Options"));

            return answer.Result;
        }

        /// <summary>
        /// Set the view model catch data.
        /// </summary>
        /// <param name="ex">Exception.</param>
        /// <returns>A task.</returns>
        public async Task ViewModelCatch(Exception ex)
        {
#if DEBUG
            await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
            await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
            await this.CanceledAction();

            this.SetIsBusyFalse();
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

        /********************************************************/

        /// <summary>
        /// Show info popup.
        /// </summary>
        [RelayCommand]
        public void InfoPopup()
        {
            this.View.ShowPopup(new InformationPopup(DeviceWidth - 50, this.InfoText));
        }

        /********************************************************/

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
            this.SetRefreshView(true);
        }

        /// <summary>
        /// Set values when view is not refreshing.
        /// </summary>
        public void SetRefreshFalse()
        {
            this.IsRefreshing = false;
        }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public abstract Task SetViewModelData();

        /// <summary>
        /// Set whether to refresh view or not.
        /// </summary>
        /// <param name="value">Value to change to.</param>
        public abstract void SetRefreshView(bool value);

        /********************************************************/

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
