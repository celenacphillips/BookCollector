using BookCollector.Data.Models;
using BookCollector.Views.Location;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class LocationBaseViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public int totalLocationsCount;

        [ObservableProperty]
        public int filteredLocationsCount;

        [ObservableProperty]
        public static ObservableCollection<LocationModel>? fullLocationList;

        [ObservableProperty]
        public static ObservableCollection<LocationModel>? filteredLocationList;

        [RelayCommand]
        public async Task LocationSelectionChanged()
        {
            if (SelectedLocation != null && !string.IsNullOrEmpty(SelectedLocation.LocationName))
            {
                var view = new LocationMainView(SelectedLocation, SelectedLocation.LocationName);

                await Shell.Current.Navigation.PushAsync(view);
                SelectedLocation = null;
            }
        }
    }
}
