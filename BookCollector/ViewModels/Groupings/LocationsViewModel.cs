// <copyright file="LocationsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Location;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Groupings
{
    public partial class LocationsViewModel : LocationBaseViewModel
    {
        [ObservableProperty]
        public string? totalLocationsstring;

        public LocationsViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight;
            this.InfoText = $"{AppStringResources.LocationView_InfoText}";
            this.ViewTitle = AppStringResources.Locations;
            RefreshView = true;
        }

        private bool ShowHiddenLocations { get; set; }

        private bool LocationNameChecked { get; set; }

        private bool TotalBooksChecked { get; set; }

        private bool TotalPriceChecked { get; set; }

        public static bool RefreshView { get; set; }

        public static async Task SetList(bool showHiddenLocations)
        {
            if (fullLocationList == null)
            {
                fullLocationList = await FillLists.GetAllLocationsList();
            }

            if (!showHiddenLocations)
            {
                filteredLocationList1 = new ObservableCollection<LocationModel>(fullLocationList!.Where(x => !x.HideLocation));
            }
            else
            {
                filteredLocationList1 = new ObservableCollection<LocationModel>(fullLocationList!);
            }
        }

        public static async Task HideBooks(bool showHiddenLocations)
        {
            if (!showHiddenLocations)
            {
                var hideList = new ObservableCollection<LocationModel>(fullLocationList!.Where(x => x.HideLocation));

                foreach (var item in hideList)
                {
                    var books = AllBooksViewModel.filteredBookList1?
                        .Where(x => x.BookLocationGuid == item.LocationGuid && !x.HideBook)
                        .ToObservableCollection();

                    foreach (var book in books)
                    {
                        book.HideBook = true;
                        await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                    }
                }
            }
        }

        public async Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await SetList(this.ShowHiddenLocations);

                    if (this.FilteredLocationList1 != null)
                    {
                        this.FilteredLocationList2 = this.FilteredLocationList1;

                        this.TotalLocationsCount = this.FilteredLocationList1 != null ? this.FilteredLocationList1.Count : 0;

                        this.SearchOnLocation(this.SearchString);

                        await Task.WhenAll(this.FilteredLocationList2.Select(x => x.SetTotalBooks(ShowHiddenBook)));
                        await Task.WhenAll(this.FilteredLocationList2.Select(x => x.SetTotalCostOfBooks(ShowHiddenBook)));

                        var sortList = SortLists.SortLocationsList(
                                this.FilteredLocationList2,
                                this.LocationNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

                        this.FilteredLocationsCount = this.FilteredLocationList2.Count;

                        this.TotalLocationsstring = StringManipulation.SetTotalLocationsString(this.FilteredLocationsCount, this.TotalLocationsCount);

                        this.ShowCollectionViewFooter = this.FilteredLocationsCount > 0;

                        await Task.WhenAll(sortList);

                        this.FilteredLocationList2 = sortList.Result;
                    }

                    this.SetIsBusyFalse();
                    RefreshView = false;
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
                    RefreshView = false;
                }
            }
        }

        [RelayCommand]
        public async void SearchOnLocation(string? input)
        {
            this.SearchString = input;

            if (this.FilteredLocationList2 != null && this.FilteredLocationList1 != null)
            {
                if (!string.IsNullOrEmpty(this.SearchString))
                {
                    this.FilteredLocationList2 = this.FilteredLocationList1.Where(x => !string.IsNullOrEmpty(x.LocationName) && x.LocationName.Contains(this.SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                }
                else
                {
                    this.FilteredLocationList2 = this.FilteredLocationList1;
                }

                this.FilteredLocationsCount = this.FilteredLocationList2 != null ? this.FilteredLocationList2.Count : 0;

                this.TotalLocationsstring = StringManipulation.SetTotalLocationsString(this.FilteredLocationsCount, this.TotalLocationsCount);
            }

            var sortList = SortLists.SortLocationsList(
                                this.FilteredLocationList2,
                                this.LocationNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            await Task.WhenAll(sortList);

            this.FilteredLocationList2 = sortList.Result;
        }

        [RelayCommand]
        public async Task PopupMenuLocation(Guid? input)
        {
            if (this.FilteredLocationList2 != null)
            {
                var selected = this.FilteredLocationList2.FirstOrDefault(x => x.LocationGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.LocationName))
                {
                    var action = await PopupMenu(selected.LocationName);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await this.EditLocation(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await this.DeleteLocation(selected);
                    }
                }
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task AddLocation()
        {
            this.SetIsBusyTrue();

            var view = new LocationEditView(new LocationModel(), $"{AppStringResources.AddNewLocation}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditLocation(LocationModel selected)
        {
            this.SetIsBusyTrue();

            var view = new LocationEditView(selected, $"{AppStringResources.EditLocation}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
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
                        this.SetIsBusyTrue();

                        await Database.DeleteLocationAsync(ConvertTo<LocationDatabaseModel>(selected));
                        this.RemoveFromStaticList(selected);
                        await this.RemoveBookFromGrouping(selected);

                        await ConfirmDelete(selected.LocationName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
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
            if (!string.IsNullOrEmpty(this.ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, this.ViewTitle)
                {
                    LocationNameVisible = true,
                    LocationNameChecked = this.LocationNameChecked,
                    TotalBooksVisible = true,
                    TotalBooksChecked = this.TotalBooksChecked,
                    TotalPriceVisible = true,
                    TotalPriceChecked = this.TotalPriceChecked,
                    AscendingChecked = this.AscendingChecked,
                    DescendingChecked = this.DescendingChecked,
                };

                popup.BindingContext = viewModel;

                var result = await this.View.ShowPopupAsync(popup);
                if (!result.WasDismissedByTappingOutsideOfPopup)
                {
                    RefreshView = true;
                    await this.SetViewModelData();
                }
            }
        }

        private void GetPreferences()
        {
            this.ShowHiddenLocations = Preferences.Get("HiddenLocationsOn", true /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.LocationNameChecked = Preferences.Get($"{this.ViewTitle}_LocationNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }

        private void RemoveFromStaticList(LocationModel selected)
        {
            if (LocationsViewModel.fullLocationList != null)
            {
                LocationsViewModel.RefreshView = this.RemoveLocationFromStaticList(selected, LocationsViewModel.fullLocationList, LocationsViewModel.filteredLocationList2);
            }
        }

        private bool RemoveLocationFromStaticList(LocationModel selected, ObservableCollection<LocationModel> locationList, ObservableCollection<LocationModel>? filteredLocationList)
        {
            var refresh = false;

            try
            {
                var oldLocation = locationList.FirstOrDefault(x => x.LocationGuid == selected.LocationGuid);

                if (oldLocation != null)
                {
                    locationList.Remove(oldLocation);
                    refresh = true;
                }

                if (filteredLocationList != null)
                {
                    var filteredLocation = filteredLocationList.FirstOrDefault(x => x.LocationGuid == selected.LocationGuid);

                    if (filteredLocation != null)
                    {
                        filteredLocationList.Remove(filteredLocation);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        private async Task RemoveBookFromGrouping(LocationModel location)
        {
            var books = AllBooksViewModel.fullBookList?.Where(x => x.BookLocationGuid == location.LocationGuid).ToList();

            foreach (var book in books)
            {
                book.BookLocationGuid = null;
                await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                await BookBaseViewModel.AddToStaticList(book);
            }
        }
    }
}
