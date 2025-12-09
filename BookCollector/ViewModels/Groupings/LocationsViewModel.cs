using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Location;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Location;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
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

        private bool ShowHiddenLocations { get; set; }
        private bool LocationNameChecked { get; set; }
        private bool TotalBooksChecked { get; set; }
        private bool TotalPriceChecked { get; set; }

        public LocationsViewModel(ContentPage view)
        {
            View = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.LocationView_InfoText}";
            ViewTitle = AppStringResources.Locations;
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                Task.WaitAll(
                [
                    Task.Run (async () => FullLocationList = await FilterLists.GetAllLocationsList(ShowHiddenLocations) ),
                ]);

                if (FullLocationList != null)
                {
                    TotalLocationsCount = FullLocationList.Count;

                    FilteredLocationList = FullLocationList;

                    foreach (var location in FullLocationList)
                    {
                        await location.SetTotalBooks(ShowHiddenBook);
                    }

                    Task.WaitAll(
                    [
                        Task.Run (async () => FilteredLocationList = await FilterLists.SortLocationsList(FilteredLocationList,
                                                                                                         LocationNameChecked,
                                                                                                         TotalBooksChecked,
                                                                                                         TotalPriceChecked,
                                                                                                         AscendingChecked,
                                                                                                         DescendingChecked) ),
                    ]);

                    FilteredLocationsCount = FilteredLocationList.Count;

                    TotalLocationsString = StringManipulation.SetTotalLocationsString(FilteredLocationsCount, TotalLocationsCount);

                    ShowCollectionViewFooter = FilteredLocationsCount > 0;
                }

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public void SearchOnLocation(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (FilteredLocationList != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                    FilteredLocationList = FilteredLocationList.Where(x => !string.IsNullOrEmpty(x.LocationName) && x.LocationName.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                else
                    FilteredLocationList = FullLocationList;

                FilteredLocationsCount = FilteredLocationList != null ? FilteredLocationList.Count : 0;

                TotalLocationsString = StringManipulation.SetTotalLocationsString(FilteredLocationsCount, TotalLocationsCount);
            }

            SetIsBusyFalse();
        }


        [RelayCommand]
        public async Task PopupMenuLocation(Guid? input)
        {
            if (FilteredLocationList != null)
            {
                var selected = FilteredLocationList.FirstOrDefault(x => x.LocationGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.LocationName))
                {
                    var action = await PopupMenu(selected.LocationName);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await EditLocation(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await DeleteLocation(selected);
                    }
                }
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

            var view = new LocationEditView(new LocationModel(), $"{AppStringResources.AddNewLocation}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditLocation(LocationModel selected)
        {
            SetIsBusyTrue();

            var view = new LocationEditView(selected, $"{AppStringResources.EditLocation}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteLocation(LocationModel selected)
        {
            if (!string.IsNullOrEmpty(selected.LocationName))
            {
                var answer = await DeleteCheck(selected.LocationName);

                if (answer)
                {
                    try
                    {
                        SetIsBusyTrue();

                        if (TestData.UseTestData)
                        {
                            TestData.DeleteLocation(selected);
                        }
                        else
                        {

                        }

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

        [RelayCommand]
        public async Task SortPopup()
        {
            if (!string.IsNullOrEmpty(ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, ViewTitle)
                {
                    LocationNameVisible = true,
                    LocationNameChecked = LocationNameChecked,
                    TotalBooksVisible = true,
                    TotalBooksChecked = TotalBooksChecked,
                    TotalPriceVisible = true,
                    TotalPriceChecked = TotalPriceChecked,
                    AscendingChecked = AscendingChecked,
                    DescendingChecked = DescendingChecked,
                };

                popup.BindingContext = viewModel;

                await View.ShowPopupAsync(popup);
                await SetViewModelData();
            }
        }

        private void GetPreferences()
        {
            ShowHiddenLocations = Preferences.Get("HiddenLocationsOn", true  /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true  /* Default */);

            LocationNameChecked = Preferences.Get($"{ViewTitle}_LocationNameSelection", true  /* Default */);
            TotalBooksChecked = Preferences.Get($"{ViewTitle}_TotalBooksSelection", false  /* Default */);
            TotalPriceChecked = Preferences.Get($"{ViewTitle}_TotalPriceSelection", false  /* Default */);

            AscendingChecked = Preferences.Get($"{ViewTitle}_AscendingSelection", true  /* Default */);
            DescendingChecked = Preferences.Get($"{ViewTitle}_DescendingSelection", false  /* Default */);
        }
    }
}
