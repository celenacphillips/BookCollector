using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Location;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Groupings
{
    public partial class LocationsViewModel : LocationBaseViewModel
    {
        [ObservableProperty]
        public string? totalLocationsstring;

        public LocationsViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight - this.DoubleMenuBar;
            this.InfoText = $"{AppStringResources.LocationView_InfoText}";
            this.ViewTitle = AppStringResources.Locations;
        }

        private bool ShowHiddenLocations { get; set; }

        private bool LocationNameChecked { get; set; }

        private bool TotalBooksChecked { get; set; }

        private bool TotalPriceChecked { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                Task.WaitAll(
               [
                    Task.Run(async () => this.FullLocationList = await FilterLists.GetAllLocationsList(this.ShowHiddenLocations)),
                ]);

                if (this.FullLocationList != null)
                {
                    this.TotalLocationsCount = this.FullLocationList.Count;

                    this.FilteredLocationList = this.FullLocationList;

                    foreach (var location in this.FullLocationList)
                    {
                        location.SetTotalBooks(this.ShowHiddenBook);
                        location.SetTotalCostOfBooks(this.ShowHiddenBook);
                    }

                    Task.WaitAll(
                   [
                        Task.Run(async () => this.FilteredLocationList = await FilterLists.SortLocationsList(
                            this.FilteredLocationList,
                            this.LocationNameChecked,
                            this.TotalBooksChecked,
                            this.TotalPriceChecked,
                            this.AscendingChecked,
                            this.DescendingChecked)),
                    ]);

                    this.FilteredLocationsCount = this.FilteredLocationList.Count;

                    this.TotalLocationsstring = StringManipulation.SetTotalLocationsString(this.FilteredLocationsCount, this.TotalLocationsCount);

                    this.ShowCollectionViewFooter = this.FilteredLocationsCount > 0;
                }

                this.SetIsBusyFalse();
            }
            catch (Exception)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public void SearchOnLocation(string? input)
        {
            this.SetIsBusyTrue();

            this.Searchstring = input;

            if (this.FilteredLocationList != null)
            {
                if (!string.IsNullOrEmpty(this.Searchstring))
                {
                    this.FilteredLocationList = this.FilteredLocationList.Where(x => !string.IsNullOrEmpty(x.LocationName) && x.LocationName.Contains(this.Searchstring.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                }
                else
                {
                    this.FilteredLocationList = this.FullLocationList;
                }

                this.FilteredLocationsCount = this.FilteredLocationList != null ? this.FilteredLocationList.Count : 0;

                this.TotalLocationsstring = StringManipulation.SetTotalLocationsString(this.FilteredLocationsCount, this.TotalLocationsCount);
            }

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuLocation(Guid? input)
        {
            if (this.FilteredLocationList != null)
            {
                var selected = this.FilteredLocationList.FirstOrDefault(x => x.LocationGuid == input);

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

                        if (TestData.UseTestData)
                        {
                            TestData.DeleteLocation(selected);
                        }
                        else
                        {
                        }

                        await ConfirmDelete(selected.LocationName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception)
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

                await this.View.ShowPopupAsync(popup);
                await this.SetViewModelData();
            }
        }

        private void GetPreferences()
        {
            this.ShowHiddenLocations = Preferences.Get("HiddenLocationsOn", true /* Default */);
            this.ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.LocationNameChecked = Preferences.Get($"{this.ViewTitle}_LocationNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }
    }
}
