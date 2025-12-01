using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Location;
using BookCollector.Views.Location;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Groupings
{
    public partial class LocationsViewModel : LocationBaseViewModel
    {
        [ObservableProperty]
        public string? totalLocationsString;

        public LocationsViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.LocationView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                // Unit test data
                var locationList = TestData.LocationList;

                Task.WaitAll(
                [
                    Task.Run (async () => FullLocationList = await FilterLists.GetAllLocationsList(locationList) ),
                ]);

                TotalLocationsCount = FullLocationList.Count;

                FilteredLocationList = FullLocationList;
                FilteredLocationsCount = FilteredLocationList.Count;

                TotalLocationsString = StringManipulation.SetTotalLocationsString(FilteredLocationsCount, TotalLocationsCount);

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SearchOnLocation(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (!string.IsNullOrEmpty(SearchString))
                FilteredLocationList = FilteredLocationList.Where(x => x.LocationName.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            else
                FilteredLocationList = FullLocationList;

            FilteredLocationsCount = FilteredLocationList.Count;

            TotalLocationsString = StringManipulation.SetTotalLocationsString(FilteredLocationsCount, TotalLocationsCount);

            SetIsBusyFalse();
        }


        [RelayCommand]
        public async Task PopupMenuLocation(Guid? input)
        {
            var selected = FilteredLocationList.FirstOrDefault(x => x.LocationGuid == input);
            string? action = await BaseViewModel.PopupMenu(selected.LocationName);

            switch (action)
            {
                case "Edit":
                    await EditLocation(selected);
                    break;

                case "Delete":
                    await DeleteLocation(selected);
                    break;

                default:
                    break;
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        [RelayCommand]
        public async Task AddLocation()
        {
            SetIsBusyTrue();

            LocationEditView view = new LocationEditView(new LocationModel(), $"{AppStringResources.AddNewLocation}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditLocation(LocationModel selected)
        {
            SetIsBusyTrue();

            LocationEditView view = new LocationEditView(selected, $"{AppStringResources.EditLocation}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteLocation(LocationModel selected)
        {
            bool answer = await DeleteCheck(selected.LocationName);

            if (answer)
            {
                try
                {
                    SetIsBusyTrue();

                    // Unit test data
                    TestData.DeleteLocation(selected);

                    await ConfirmDelete(selected.LocationName);

                    await SetViewModelData();

                    SetIsBusyFalse();

                }
                catch (Exception ex)
                {
                    await CanceledAction();
                }
            }
            else
            {
                await CanceledAction();
            }
        }
    }
}
