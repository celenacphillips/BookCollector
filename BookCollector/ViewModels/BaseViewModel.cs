using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        public double DeviceHeight { get; set; }
        public double DeviceWidth { get; set; }
        public double CollectionViewHeight { get; set; }
        public double SingleMenuBar { get; set; }
        public double DoubleMenuBar { get; set; }
        public ContentPage _view { get; set; }

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
        public string? searchString;

        public BaseViewModel()
        {
            DeviceHeight = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;
            DeviceWidth = DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;
            DoubleMenuBar = DeviceHeight * 0.2899;
            SingleMenuBar = DeviceHeight * 0.2297;
        }

        [RelayCommand]
        public async Task FilterPopup()
        {
            _view.ShowPopup(new FilterPopup());
        }

        [RelayCommand]
        public async Task SortPopup()
        {
            _view.ShowPopup(new SortPopup());
        }

        [RelayCommand]
        public async Task InfoPopup()
        {
            _view.ShowPopup(new InformationPopup());
        }

        [RelayCommand]
        public static async Task Tap(object input)
        {
            if (!string.IsNullOrEmpty(input.ToString()))
            {
                await Clipboard.SetTextAsync(input.ToString());
                await Toast.Make($"{AppStringResources.TextCopied}").Show();
            }    
        }

        public async void SetIsBusyTrue()
        {
            GC.Collect();
            IsBusy = true;
            IsVisible = false;
        }

        public void SetIsBusyFalse()
        {
            IsBusy = false;
            IsVisible = true;
        }

        public void SetRefreshTrue()
        {
            IsRefreshing = true;
        }

        public void SetRefreshFalse()
        {
            IsRefreshing = false;
        }

        public static async Task<string?> PopupMenu(string title)
        {
            var edit = $"{AppStringResources.Edit}";
            var delete = $"{AppStringResources.Delete}";

            string cancel = $"{AppStringResources.Cancel}";
            string? destruction = null;
            string[] buttons = [edit, delete];
            return await Shell.Current.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public static async Task<bool> DeleteCheck(string item)
        {
            var message = $"{AppStringResources.AreYouSureYouWantToDeleteItem_Question.Replace("item", item)}";

            var action = await DisplayMessage($"{AppStringResources.AreYouSure_Question}", message, null, null);
            return action;
        }

        public static async Task<bool> DisplayMessage(string inputTitle, string? inputMessage = null, string? inputConfirm = null, string? inputDeny = null)
        {
            if (inputConfirm == null)
                inputConfirm = $"{AppStringResources.Yes}";

            if (inputDeny == null)
                inputDeny = $"{AppStringResources.No}";

            if (inputMessage == null)
                inputMessage = $"{AppStringResources.AreYouSure_Question}";

            var action = await Shell.Current.DisplayAlert(inputTitle, inputMessage, inputConfirm, inputDeny);
            return action;
        }

        public static async Task DisplayMessage(string inputTitle, string? inputMessage = null, string? inputConfirm = null)
        {
            if (inputConfirm == null)
                inputConfirm = $"{AppStringResources.OK}";

            if (inputMessage == null)
                inputMessage = $"{AppStringResources.ActionCanceled}";

            await Shell.Current.DisplayAlert(inputTitle, inputMessage, inputConfirm);
        }

        public static async Task CanceledAction()
        {
            await DisplayMessage($"{AppStringResources.ActionCanceled}", null, null);
        }

        public static async Task ConfirmDelete(string item)
        {
            var title = $"{AppStringResources.ItemDeleted.Replace("Item", item)}.";
            var message = $"{AppStringResources.ItemWasDeleted.Replace("Item", item)}";

            await DisplayMessage(title, message, null);
        }
    }
}
