using BookCollector.Data.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels
{
    public partial class LocationBaseViewModel : BaseViewModel
    {
        [ObservableProperty]
        public int totalLocationsCount;

        [ObservableProperty]
        public int filteredLocationsCount;

        [ObservableProperty]
        public static ObservableCollection<LocationModel>? fullLocationList;

        [ObservableProperty]
        public static ObservableCollection<LocationModel>? filteredLocationList;

        [ObservableProperty]
        public LocationModel? selectedLocation;

        // TO DO
        [RelayCommand]
        public async Task LocationSelectionChanged()
        {

        }
    }
}
