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

        public BaseViewModel()
        {
            DeviceHeight = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;
            DeviceWidth = DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;
            DoubleMenuBar = DeviceHeight * 0.2899;
            SingleMenuBar = DeviceHeight * 0.2297;
        }

        // TO DO:
        // Set up refresh command for all views - 11/19/2025
        [RelayCommand]
        public async Task Refresh()
        {
            IsRefreshing = true;

            IsRefreshing = false;
        }

        // TO DO:
        // Set up filter pop up - 11/19/2025
        [RelayCommand]
        public async Task FilterPopup()
        {

        }

        // TO DO:
        // Set up sort pop up - 11/19/2025
        [RelayCommand]
        public async Task SortPopup()
        {

        }

        // TO DO:
        // Set up info pop up - 11/19/2025
        [RelayCommand]
        public async Task InfoPopup()
        {

        }

        // TO DO:
        // Set up cover image pop up - 11/19/2025
        [RelayCommand]
        public async Task BookCoverPopup()
        {
            _view.ShowPopup(new BookCoverPopup());
        }

        // TO DO:
        // Include Tap command on all labels of the BookMainView - 11/19/2025
        [RelayCommand]
        public static async Task Tap(string input)
        {
            await Clipboard.SetTextAsync(input);
            await Toast.Make($"{AppStringResources.TextCopied}").Show();
        }

        public void SetIsBusyTrue()
        {
            IsBusy = true;
            IsVisible = false;
        }

        public void SetIsBusyFalse()
        {
            IsBusy = false;
            IsVisible = true;
        }
    }
}
