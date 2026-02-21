// <copyright file="BaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>
#if ANDROID
using AndroidX.Core.View;
using Android.OS;
#endif
using BookCollector.CustomPermissions;
using BookCollector.Data;
using BookCollector.Data.Database;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Groupings;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Main;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        public bool isRefreshing;

        [ObservableProperty]
        public bool isBusy;

        [ObservableProperty]
        public bool isVisible;

        [ObservableProperty]
        public bool isEnabled;

        [ObservableProperty]
        public string? viewTitle;

        [ObservableProperty]
        public string infoText;

        [ObservableProperty]
        public string? searchString;

        internal static BookCollectorDatabase Database;

        [ObservableProperty]
        internal static bool showCollectionViewFooter;

        [ObservableProperty]
        internal SelectionMode collectionViewSelectionMode;

        public BaseViewModel()
        {
            this.DeviceHeight = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;
            this.DeviceWidth = DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;
            this.InfoText = string.Empty;
            this.View = new ContentPage();
        }

        public double DeviceHeight { get; set; }

        public double DeviceWidth { get; set; }

        public double CollectionViewHeight { get; set; }

        public ContentPage View { get; set; }

        public bool AscendingChecked { get; set; }

        public bool DescendingChecked { get; set; }

        [RelayCommand]
        public static async Task Tap(object input)
        {
            if (!string.IsNullOrEmpty(input.ToString()))
            {
                await Clipboard.SetTextAsync(input.ToString());
                await Toast.Make($"{AppStringResources.TextCopied}").Show();
            }
        }

        public async Task<string?> PopupMenu(string title)
        {
            var edit = $"{AppStringResources.Edit}";
            var delete = $"{AppStringResources.Delete}";

            var answer = await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(this.DeviceWidth - 50, title, string.Empty, edit, delete, "Options"));

            return answer.Result;
        }

        public async Task<string?> PopupMenu_CoverPhoto()
        {
            var title = AppStringResources.AddOrReplaceCoverPhoto;
            var file = AppStringResources.UploadExistingFile;
            var url = AppStringResources.BookCoverUrl;

            var answer = await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(this.DeviceWidth - 50, title, string.Empty, file, url, "Options"));

            return answer.Result;
        }

        public async Task<bool> DeleteCheck(string item)
        {
            var message = $"{AppStringResources.AreYouSureYouWantToDeleteItem_Question.Replace("item", item)}";

            var action = await DisplayMessage($"{AppStringResources.AreYouSure_Question}", message, null, null);
            return action;
        }

        public async Task<bool> DisplayMessage(string inputTitle, string? inputMessage = null, string? inputConfirm = null, string? inputDeny = null)
        {
            inputConfirm ??= $"{AppStringResources.Yes}";

            inputDeny ??= $"{AppStringResources.No}";

            inputMessage ??= $"{AppStringResources.AreYouSure_Question}";

            var answer = await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(this.DeviceWidth - 50, inputTitle, inputMessage, inputConfirm, inputDeny, "Commands"));

            if (!string.IsNullOrEmpty(answer.Result) && answer.Result.Equals(inputConfirm))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task DisplayMessage(string inputTitle, string? inputMessage = null)
        {
            inputMessage ??= inputTitle;

            var inputConfirm = $"{AppStringResources.OK}";

            await this.View.ShowPopupAsync<string>(new ChoiceDialogPopup(this.DeviceWidth - 50, inputTitle, inputMessage, inputConfirm, null, "Commands"));
        }

        public async Task CanceledAction()
        {
            await DisplayMessage($"{AppStringResources.ActionCanceled}", null);
        }

        public async Task ConfirmDelete(string item)
        {
            var title = $"{AppStringResources.ItemDeleted.Replace("Item", item)}.";
            var message = $"{AppStringResources.ItemWasDeleted.Replace("Item", item)}";

            await DisplayMessage(title, message);
        }

        public static byte[] DownloadImage(string imageURL)
        {
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30),
            };

            return client.GetByteArrayAsync(imageURL).Result;
        }

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

        [RelayCommand]
        public void InfoPopup()
        {
            this.View.ShowPopup(new InformationPopup(this.DeviceWidth - 50, this.InfoText));
        }

        public void SetIsBusyTrue()
        {
            this.IsBusy = true;
            this.IsVisible = true;
            this.IsEnabled = false;
            this.CollectionViewSelectionMode = SelectionMode.None;
        }

        public void SetIsBusyFalse()
        {
            this.IsBusy = false;
            this.IsVisible = true;
            this.IsEnabled = true;
            this.CollectionViewSelectionMode = SelectionMode.Single;
        }

        public void SetRefreshTrue()
        {
            this.IsRefreshing = true;
        }

        public void SetRefreshFalse()
        {
            this.IsRefreshing = false;
        }

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

        public static void ClearAllLists()
        {
            ReadingViewModel.fullBookList?.Clear();
            ReadingViewModel.filteredBookList1?.Clear();
            ReadingViewModel.filteredBookList2?.Clear();
            ReadingViewModel.RefreshView = true;

            ToBeReadViewModel.fullBookList?.Clear();
            ToBeReadViewModel.filteredBookList1?.Clear();
            ToBeReadViewModel.filteredBookList2?.Clear();
            ToBeReadViewModel.RefreshView = true;

            ReadViewModel.fullBookList?.Clear();
            ReadViewModel.filteredBookList1?.Clear();
            ReadViewModel.filteredBookList2?.Clear();
            ReadViewModel.RefreshView = true;

            AllBooksViewModel.fullBookList?.Clear();
            AllBooksViewModel.filteredBookList1?.Clear();
            AllBooksViewModel.filteredBookList2?.Clear();
            AllBooksViewModel.RefreshView = true;

            CollectionsViewModel.fullCollectionList?.Clear();
            CollectionsViewModel.filteredCollectionList1?.Clear();
            CollectionsViewModel.filteredCollectionList2?.Clear();
            CollectionsViewModel.RefreshView = true;

            GenresViewModel.fullGenreList?.Clear();
            GenresViewModel.filteredGenreList1?.Clear();
            GenresViewModel.filteredGenreList2?.Clear();
            GenresViewModel.RefreshView = true;

            SeriesViewModel.fullSeriesList?.Clear();
            SeriesViewModel.filteredSeriesList1?.Clear();
            SeriesViewModel.filteredSeriesList2?.Clear();
            SeriesViewModel.RefreshView = true;

            AuthorsViewModel.fullAuthorList?.Clear();
            AuthorsViewModel.filteredAuthorList1?.Clear();
            AuthorsViewModel.filteredAuthorList2?.Clear();
            AuthorsViewModel.RefreshView = true;

            LocationsViewModel.fullLocationList?.Clear();
            LocationsViewModel.filteredLocationList1?.Clear();
            LocationsViewModel.filteredLocationList2?.Clear();
            LocationsViewModel.RefreshView = true;

            WishListViewModel.fullWishlistBookList?.Clear();
            WishListViewModel.filteredWishlistBookList1?.Clear();
            WishListViewModel.filteredWishlistBookList2?.Clear();
            WishListViewModel.RefreshView = true;
        }

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
    }
}
