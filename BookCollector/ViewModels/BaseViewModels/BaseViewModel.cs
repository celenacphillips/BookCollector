// <copyright file="BaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Database;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
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
        public string? viewTitle;

        [ObservableProperty]
        public string infoText;

        [ObservableProperty]
        public string? searchstring;

        internal static BookCollectorDatabase Database;

        [ObservableProperty]
        internal static bool showCollectionViewFooter;

        public BaseViewModel()
        {
            this.DeviceHeight = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;
            this.DeviceWidth = DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;
            this.DoubleMenuBar = this.DeviceHeight * 0.2899;
            this.SingleMenuBar = this.DeviceHeight * 0.2297;
            this.InfoText = string.Empty;
            this.View = new ContentPage();
        }

        public double DeviceHeight { get; set; }

        public double DeviceWidth { get; set; }

        public double CollectionViewHeight { get; set; }

        public double SingleMenuBar { get; set; }

        public double DoubleMenuBar { get; set; }

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

        public static async Task<string?> PopupMenu(string title)
        {
            var edit = $"{AppStringResources.Edit}";
            var delete = $"{AppStringResources.Delete}";

            string cancel = $"{AppStringResources.Cancel}";
            string? destruction = null;
            string[] buttons = [edit, delete];
            return await Shell.Current.DisplayActionSheetAsync(title, cancel, destruction, buttons);
        }

        public static async Task<string?> PopupMenu_CoverPhoto()
        {
            var title = AppStringResources.AddOrReplaceCoverPhoto;
            var file = AppStringResources.UploadExistingFile;
            var url = AppStringResources.BookCoverUrl;

            string cancel = AppStringResources.Cancel;
            string? destruction = null;
            string[] buttons = [file, url];
            return await Shell.Current.DisplayActionSheetAsync(title, cancel, destruction, buttons);
        }

        public static async Task<bool> DeleteCheck(string item)
        {
            var message = $"{AppStringResources.AreYouSureYouWantToDeleteItem_Question.Replace("item", item)}";

            var action = await DisplayMessage($"{AppStringResources.AreYouSure_Question}", message, null, null);
            return action;
        }

        public static async Task<bool> DisplayMessage(string inputTitle, string? inputMessage = null, string? inputConfirm = null, string? inputDeny = null)
        {
            inputConfirm ??= $"{AppStringResources.Yes}";

            inputDeny ??= $"{AppStringResources.No}";

            inputMessage ??= $"{AppStringResources.AreYouSure_Question}";

            var action = await Shell.Current.DisplayAlertAsync(inputTitle, inputMessage, inputConfirm, inputDeny);
            return action;
        }

        public static async Task DisplayMessage(string inputTitle, string? inputMessage = null)
        {
            inputMessage ??= inputTitle;

            var inputConfirm = $"{AppStringResources.OK}";

            await Shell.Current.DisplayAlertAsync(inputTitle, inputMessage, inputConfirm);
        }

        public static async Task CanceledAction()
        {
            await DisplayMessage($"{AppStringResources.ActionCanceled}", null);
        }

        public static async Task ConfirmDelete(string item)
        {
            var title = $"{AppStringResources.ItemDeleted.Replace("Item", item)}.";
            var message = $"{AppStringResources.ItemWasDeleted.Replace("Item", item)}";

            await DisplayMessage(title, message);
        }

        public static byte[] DownloadImage(string imageURL)
        {
            return new HttpClient().GetByteArrayAsync(imageURL).Result;
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
            // GC.Collect();
            this.IsBusy = true;
            this.IsVisible = true;
        }

        public void SetIsBusyFalse()
        {
            this.IsBusy = false;
            this.IsVisible = true;
            GC.Collect();
        }

        public void SetRefreshTrue()
        {
            this.IsRefreshing = true;
        }

        public void SetRefreshFalse()
        {
            this.IsRefreshing = false;
        }

        public static void SetBookCover(BookModel book)
        {
            if (!string.IsNullOrEmpty(book.BookCoverFileLocation) && book.BookCover == null)
            {
                var imageBytes = File.ReadAllBytes(book.BookCoverFileLocation);
                var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                book.BookCover = imageSource;
            }

            if (!string.IsNullOrEmpty(book.BookCoverUrl) && book.BookCover == null)
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var byteArray = DownloadImage(book.BookCoverUrl);
                    book.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                }
                else
                {
                    book.HasBookCover = false;
                    book.HasNoBookCover = true;
                }
            }
        }

        public void SetBookCover(WishlistBookModel book)
        {
            if (!string.IsNullOrEmpty(book.BookCoverFileLocation) && book.BookCover == null)
            {
                var imageBytes = File.ReadAllBytes(book.BookCoverFileLocation);
                var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                book.BookCover = imageSource;
            }

            if (!string.IsNullOrEmpty(book.BookCoverUrl) && book.BookCover == null)
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var byteArray = DownloadImage(book.BookCoverUrl);
                    book.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                }
                else
                {
                    book.HasBookCover = false;
                    book.HasNoBookCover = true;
                }
            }
        }
    }
}
