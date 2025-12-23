// <copyright file="LocationEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Location;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Location
{
    public partial class LocationEditViewModel : LocationBaseViewModel
    {
        [ObservableProperty]
        public LocationModel editedLocation;

        [ObservableProperty]
        public bool locationNameValid;

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

                if (!this.LocationNameValid)
                {
                    await DisplayMessage(AppStringResources.LocationNameNotValid, null);
                }
                else
                {
#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    if (TestData.UseTestData)
                    {
                        TestData.UpdateLocation(this.EditedLocation);
                    }
                    else
                    {
                        this.EditedLocation = await Database.SaveLocationAsync(ConvertTo<LocationDatabaseModel>(this.EditedLocation));
                    }

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
            if (string.IsNullOrEmpty(this.EditedLocation.LocationName))
            {
                var locationNameEditor = this.View.FindByName<Editor>("LocationNameEditor");
                locationNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                locationNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                this.LocationNameValid = false;
            }
            else
            {
                var userAppTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme : Application.Current?.UserAppTheme;

                var locationNameEditor = this.View.FindByName<Editor>("LocationNameEditor");
                locationNameEditor.TextColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                locationNameEditor.PlaceholderColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.LocationNameValid = true;
            }
        }
    }
}
