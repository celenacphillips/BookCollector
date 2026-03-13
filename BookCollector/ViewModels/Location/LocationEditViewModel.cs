// <copyright file="LocationEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Location
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.Views.Location;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// LocationEditViewModel class.
    /// </summary>
    public partial class LocationEditViewModel : LocationsViewModel
    {
        /// <summary>
        /// Gets or sets the location to edit.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public LocationModel editedLocation;

        /// <summary>
        /// Gets or sets a value indicating whether the location name is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool locationNameNotValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationEditViewModel"/> class.
        /// </summary>
        /// <param name="location">Location to edit.</param>
        /// <param name="view">View related to view model.</param>
        public LocationEditViewModel(LocationModel location, ContentPage view)
            : base(view)
        {
            this.View = view;

            this.EditedLocation = (LocationModel)location.Clone();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to insert the main view before or not.
        /// </summary>
        public bool InsertMainViewBefore { get; set; }

        /// <summary>
        /// Add location to the static list in the list view model.
        /// </summary>
        /// <param name="location">Location to add.</param>
        /// <returns>A task.</returns>
        public static async Task AddToStaticList(LocationModel location)
        {
            if (LocationsViewModel.fullLocationList != null)
            {
                LocationsViewModel.RefreshView = await AddLocationToStaticList(location, LocationsViewModel.fullLocationList, LocationsViewModel.filteredLocationList2);
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
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

        /// <summary>
        /// Save location to the database and returns to the previous view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SaveLocation()
        {
            try
            {
                this.SetIsBusyTrue();

                if (this.LocationNameNotValid)
                {
                    await this.DisplayMessage(AppStringResources.LocationNameNotValid, null);
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
                    await AddToStaticList(this.EditedLocation);

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
                await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                this.SetIsBusyFalse();
            }
        }

        /// <summary>
        /// Check if the location name is valid and set the related value.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateLocationName()
        {
            this.ValidateEntry();
        }

        private static async Task<bool> AddLocationToStaticList(LocationModel location, ObservableCollection<LocationModel> locationList, ObservableCollection<LocationModel>? filteredLocationList)
        {
            var refresh = false;

            await Task.WhenAll(
            [
                location.SetTotalBooks(true),
                location.SetTotalCostOfBooks(true),
            ]);

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

        private void ValidateEntry()
        {
            this.LocationNameNotValid = string.IsNullOrEmpty(this.EditedLocation.LocationName);
        }
    }
}
