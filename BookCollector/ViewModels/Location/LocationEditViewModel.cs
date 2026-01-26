// <copyright file="LocationEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;
using BookCollector.Views.Location;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Location
{
    public partial class LocationEditViewModel : LocationBaseViewModel
    {
        [ObservableProperty]
        public LocationModel editedLocation;

        [ObservableProperty]
        public bool locationNameNotValid;

        public LocationEditViewModel(LocationModel location, ContentPage view)
        {
            this.View = view;

            this.EditedLocation = (LocationModel)location.Clone();
        }

        public bool InsertMainViewBefore { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.ValidateEntry();

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SaveLocation()
        {
            try
            {
                this.SetIsBusyTrue();

                if (this.LocationNameNotValid)
                {
                    await DisplayMessage(AppStringResources.LocationNameNotValid, null);
                    this.SetIsBusyFalse();
                }
                else
                {
#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    this.EditedLocation = await Database.SaveLocationAsync(ConvertTo<LocationDatabaseModel>(this.EditedLocation));
                    AddToStaticList(this.EditedLocation);

                    if (this.InsertMainViewBefore)
                    {
                        var view = new LocationMainView(this.EditedLocation, $"{this.EditedLocation.LocationName}");
                        Shell.Current.Navigation.InsertPageBefore(view, this.View);
                    }

                    await Shell.Current.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public void ValidateLocationName()
        {
            this.ValidateEntry();
        }

        private void ValidateEntry()
        {
            this.LocationNameNotValid = string.IsNullOrEmpty(this.EditedLocation.LocationName);
        }

        public static async Task AddToStaticList(LocationModel location)
        {
            if (LocationsViewModel.fullLocationList != null)
            {
                LocationsViewModel.RefreshView = await AddLocationToStaticList(location, LocationsViewModel.fullLocationList, LocationsViewModel.filteredLocationList2);
            }
        }

        private static async Task<bool> AddLocationToStaticList(LocationModel location, ObservableCollection<LocationModel> locationList, ObservableCollection<LocationModel>? filteredLocationList)
        {
            var refresh = false;

            await Task.WhenAll(new Task[]
            {
                location.SetTotalBooks(true),
                location.SetTotalCostOfBooks(true),
            });

            try
            {
                var oldLocation = locationList.FirstOrDefault(x => x.LocationGuid == location.LocationGuid);

                if (oldLocation != null)
                {
                    var index = locationList.IndexOf(oldLocation);
                    locationList.Remove(oldLocation);
                    locationList.Insert(index, location);
                    refresh = true;
                }
                else
                {
                    locationList.Add(location);
                    refresh = true;
                }

                if (filteredLocationList != null)
                {
                    var filteredLocation = filteredLocationList.FirstOrDefault(x => x.LocationGuid == location.LocationGuid);

                    if (filteredLocation != null)
                    {
                        var index = filteredLocationList.IndexOf(filteredLocation);
                        filteredLocationList.Remove(filteredLocation);
                        filteredLocationList.Insert(index, location);
                        refresh = true;
                    }
                    else
                    {
                        filteredLocationList.Add(location);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }
    }
}
