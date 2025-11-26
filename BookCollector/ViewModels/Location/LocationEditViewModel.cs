using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views.Location;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Location
{
    public partial class LocationEditViewModel : LocationBaseViewModel
    {
        [ObservableProperty]
        public LocationModel editedLocation;

        [ObservableProperty]
        public bool locationNameValid;

        public bool InsertMainViewBefore { get; set; }

        public LocationEditViewModel(LocationModel location, ContentPage view)
        {
            _view = view;

            EditedLocation = (LocationModel)location.Clone();
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            ValidateEntry();

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveLocation()
        {
            if (LocationNameValid)
            {
#if ANDROID
                if (Platform.CurrentActivity != null &&
                Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (ViewTitle.Equals($"{AppStringResources.AddNewLocation}"))
                {
                    // Unit test data
                    TestData.InsertLocation(EditedLocation);
                }
                else
                {
                    // Unit test data
                    TestData.UpdateLocation(EditedLocation);
                }

                if (InsertMainViewBefore)
                {
                    LocationMainView view = new LocationMainView(EditedLocation, $"{EditedLocation.LocationName}");
                    Shell.Current.Navigation.InsertPageBefore(view, _view);

                }

                await Shell.Current.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(EditedLocation.LocationName))
                LocationNameValid = false;
            else
                LocationNameValid = true;
        }

    }
}
