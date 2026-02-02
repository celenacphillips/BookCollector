// <copyright file="LocationBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data.Models;
using BookCollector.Views.Location;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class LocationBaseViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<LocationModel>? fullLocationList;

        [ObservableProperty]
        public static ObservableCollection<LocationModel>? filteredLocationList1;

        [ObservableProperty]
        public static ObservableCollection<LocationModel>? filteredLocationList2;

        [ObservableProperty]
        public int totalLocationsCount;

        [ObservableProperty]
        public int filteredLocationsCount;

        [RelayCommand]
        public async Task LocationSelectionChanged()
        {
            if (this.SelectedLocation != null && !string.IsNullOrEmpty(this.SelectedLocation.LocationName))
            {
                var view = new LocationMainView(this.SelectedLocation, this.SelectedLocation.LocationName);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedLocation = null;
            }
        }
    }
}
