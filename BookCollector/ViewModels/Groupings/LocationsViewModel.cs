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
    public partial class LocationsViewModel : GroupingBaseViewModel
    {
        /// <summary>
        /// Gets or sets the full location list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<LocationModel>? fullLocationList;

        /// <summary>
        /// Gets or sets the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<LocationModel>? hiddenFilteredLocationList;

        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<LocationModel>? filteredLocationList;

        /// <summary>
        /// Gets or sets the total locations string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalLocationsString;

        /// <summary>
        /// Gets or sets the total count of locations, based on the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int totalLocationsCount;

        /// <summary>
        /// Gets or sets the total count of locations, based on the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int filteredLocationsCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public LocationsViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = DeviceHeight;
            this.InfoText = $"{AppStringResources.LocationView_InfoText}";
            this.ViewTitle = AppStringResources.Locations;
            RefreshView = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden locations or not.
        /// </summary>
        private bool ShowHiddenLocations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether location name is checked or not.
        /// </summary>
        private bool LocationNameChecked { get; set; }

        /// <summary>
        /// Set the first filtered list based on the full location list and the show hidden locations preference.
        /// </summary>
        /// <param name="showHiddenLocations">Show hidden locations.</param>
        /// <returns>A task.</returns>
        public static async new Task SetList(bool showHiddenLocations)
        {
            fullLocationList ??= await FillLists.GetAllLocationsList();

            hiddenFilteredLocationList = SetList<LocationModel>(fullLocationList!, showHiddenLocations).ToObservableCollection();
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
                    var books = AllBooksViewModel.hiddenFilteredBookList?
                        .Where(x => x.BookLocationGuid == item.LocationGuid && !x.HideBook)
                        .ToObservableCollection();

                    await UpdateBooksToHide(books);
                }
            }
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public override bool GetPreferences()
        {
            this.ShowHiddenLocations = Preferences.Get("HiddenLocationsOn", true /* Default */);
            ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.LocationNameChecked = Preferences.Get($"{this.ViewTitle}_LocationNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);

            return this.ShowHiddenLocations;
        }

        /// <summary>
        /// Check if the list is null.
        /// </summary>
        /// <returns>If the list is null.</returns>
        public override bool ListNullCheck()
        {
            return this.HiddenFilteredLocationList != null;
        }

        /// <summary>
        /// Iterate through the list and set necessary data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetListData()
        {
            this.FilteredLocationList = this.HiddenFilteredLocationList;

            await Task.WhenAll(this.FilteredLocationList!.Select(x => x.SetTotalBooks(ShowHiddenBooks)));
            await Task.WhenAll(this.FilteredLocationList!.Select(x => x.SetTotalCostOfBooks(ShowHiddenBooks)));
        }

        /// <summary>
        /// Find filters for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetFilters()
        {
            this.FilteredLocationList = await FilterLists.FilterList(
                                this.HiddenFilteredLocationList!,
                                this.SearchString);
        }

        /// <summary>
        /// Find sort values for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetSorts()
        {
            var sortList = SortLists.SortLocationsList(
                                this.FilteredLocationList!,
                                this.LocationNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            await Task.WhenAll(sortList);

            this.FilteredLocationList = sortList.Result;
        }

        /// <summary>
        /// Set data for view.
        /// </summary>
        public async override void SetViewStrings()
        {
            this.TotalLocationsCount = this.HiddenFilteredLocationList?.Count ?? 0;

            this.FilteredLocationsCount = this.FilteredLocationList?.Count ?? 0;

            this.TotalLocationsString = StringManipulation.SetTotalLocationsString(this.FilteredLocationsCount, this.TotalLocationsCount);

            this.ShowCollectionViewFooter = this.FilteredLocationsCount > 0;
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

            if (this.FilteredLocationList != null && this.HiddenFilteredLocationList != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    this.FilteredLocationList = FilterLists.FilterOnSearchString(this.HiddenFilteredLocationList, input);
                }
                else
                {
                    this.FilteredLocationList = await FilterLists.FilterList(
                                this.HiddenFilteredLocationList,
                                this.SearchString);
                }

                this.SetViewStrings();

                await this.SetSorts();
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
            if (this.FilteredLocationList != null)
            {
                var selected = this.FilteredLocationList.FirstOrDefault(x => x.LocationGuid == input);

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
        /// Changes the view based on the selected location.
        /// </summary>
        /// <returns>A task.</returns>
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
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel)
        {
            viewModel.LocationNameVisible = true;
            viewModel.LocationNameChecked = this.LocationNameChecked;
            /******************************/
            viewModel.TotalBooksVisible = true;
            viewModel.TotalBooksChecked = this.TotalBooksChecked;
            /******************************/
            viewModel.TotalPriceVisible = true;
            viewModel.TotalPriceChecked = this.TotalPriceChecked;
            /******************************/
            viewModel.AscendingChecked = this.AscendingChecked;
            viewModel.DescendingChecked = this.DescendingChecked;

            return viewModel;
        }

        private static void RemoveFromStaticList(LocationModel selected)
        {
            if (LocationsViewModel.fullLocationList != null)
            {
                LocationsViewModel.RefreshView = RemoveLocationFromStaticList(selected, LocationsViewModel.fullLocationList, LocationsViewModel.filteredLocationList);
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
    }
}
