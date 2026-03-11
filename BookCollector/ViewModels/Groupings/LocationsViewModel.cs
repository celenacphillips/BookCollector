// <copyright file="LocationsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Groupings
{
    using System.Collections.ObjectModel;
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

    /// <summary>
    /// LocationsViewModel class.
    /// </summary>
    public partial class LocationsViewModel : LocationBaseViewModel
    {
        /// <summary>
        /// Gets or sets the total locations string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalLocationsstring;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public LocationsViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight;
            this.InfoText = $"{AppStringResources.LocationView_InfoText}";
            this.ViewTitle = AppStringResources.Locations;
            RefreshView = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static bool RefreshView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden locations or not.
        /// </summary>
        private bool ShowHiddenLocations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether location name is checked or not.
        /// </summary>
        private bool LocationNameChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether total books is checked or not.
        /// </summary>
        private bool TotalBooksChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether total price is checked or not.
        /// </summary>
        private bool TotalPriceChecked { get; set; }

        /// <summary>
        /// Set the first filtered list based on the full location list and the show hidden locations preference.
        /// </summary>
        /// <param name="showHiddenLocations">Show hidden locations.</param>
        /// <returns>A task.</returns>
        public static async Task SetList(bool showHiddenLocations)
        {
            fullLocationList ??= await FillLists.GetAllLocationsList();

            if (!showHiddenLocations)
            {
                filteredLocationList1 = new ObservableCollection<LocationModel>(fullLocationList!.Where(x => !x.HideLocation));
            }
            else
            {
                filteredLocationList1 = new ObservableCollection<LocationModel>(fullLocationList!);
            }
        }

        /// <summary>
        /// Set books to hide books that are related to the location.
        /// </summary>
        /// <param name="showHiddenLocations">Show hidden locations.</param>
        /// <returns>A task.</returns>
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

                    if (books != null)
                    {
                        foreach (var book in books)
                        {
                            book.HideBook = true;
                            await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
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

                        this.TotalLocationsCount = this.FilteredLocationList1.Count;

                        await this.SearchOnLocation(this.SearchString);

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
                    await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
        }

        /// <summary>
        /// Search the list based on the location name.
        /// </summary>
        /// <param name="input">Input string to find.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SearchOnLocation(string? input)
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

                var sortList = SortLists.SortLocationsList(
                                    this.FilteredLocationList2!,
                                    this.LocationNameChecked,
                                    this.TotalBooksChecked,
                                    this.TotalPriceChecked,
                                    this.AscendingChecked,
                                    this.DescendingChecked);

                await Task.WhenAll(sortList);

                this.FilteredLocationList2 = sortList.Result;
            }
        }

        /// <summary>
        /// Show popup with options to interact with the selected location object.
        /// </summary>
        /// <param name="input">Location guid to interact with.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task PopupMenuLocation(Guid? input)
        {
            if (this.FilteredLocationList2 != null)
            {
                var selected = this.FilteredLocationList2.FirstOrDefault(x => x.LocationGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.LocationName))
                {
                    var action = await this.PopupMenu(selected.LocationName);

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

        /// <summary>
        /// Set refreshing values and reset the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        /// <summary>
        /// Create a new location and navigate to the location edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddLocation()
        {
            this.SetIsBusyTrue();

            var view = new LocationEditView(new LocationModel(), $"{AppStringResources.AddNewLocation}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Navigate to location edit view for selected location.
        /// </summary>
        /// <param name="selected">Selected location.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task EditLocation(LocationModel selected)
        {
            this.SetIsBusyTrue();

            var view = new LocationEditView(selected, $"{AppStringResources.EditLocation}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Delete selected location.
        /// </summary>
        /// <param name="selected">Selected location.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task DeleteLocation(LocationModel selected)
        {
            if (!string.IsNullOrEmpty(selected.LocationName))
            {
                var answer = await this.DeleteCheck(selected.LocationName);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        await Database.DeleteLocationAsync(ConvertTo<LocationDatabaseModel>(selected));
                        RemoveFromStaticList(selected);
                        await RemoveBookFromGrouping(selected);

                        await this.ConfirmDelete(selected.LocationName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                        await this.CanceledAction();
                    }
                }
                else
                {
                    await this.CanceledAction();
                }
            }
        }

        /// <summary>
        /// Show sort popup.
        /// </summary>
        /// <returns>A task.</returns>
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

        private static void RemoveFromStaticList(LocationModel selected)
        {
            if (LocationsViewModel.fullLocationList != null)
            {
                LocationsViewModel.RefreshView = RemoveLocationFromStaticList(selected, LocationsViewModel.fullLocationList, LocationsViewModel.filteredLocationList2);
            }
        }

        private static bool RemoveLocationFromStaticList(LocationModel selected, ObservableCollection<LocationModel> locationList, ObservableCollection<LocationModel>? filteredLocationList)
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

        private static async Task RemoveBookFromGrouping(LocationModel location)
        {
            var books = AllBooksViewModel.fullBookList?.Where(x => x.BookLocationGuid == location.LocationGuid).ToList();

            if (books != null)
            {
                foreach (var book in books)
                {
                    book.BookLocationGuid = null;
                    await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                    await BookBaseViewModel.AddToStaticList(book);
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
    }
}
