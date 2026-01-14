// <copyright file="SeriesViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Popups;
using BookCollector.Views.Series;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Groupings
{
    public partial class SeriesViewModel : SeriesBaseViewModel
    {
        [ObservableProperty]
        public string? totalSeriesstring;

        public SeriesViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight - this.DoubleMenuBar;
            this.InfoText = $"{AppStringResources.SeriesView_InfoText}";
            this.ViewTitle = AppStringResources.Series;
            RefreshView = true;
        }

        private bool ShowHiddenSeries { get; set; }

        private bool SeriesNameChecked { get; set; }

        private bool TotalBooksChecked { get; set; }

        private bool TotalPriceChecked { get; set; }

        public static bool RefreshView { get; set; }

        public static async Task SetList(bool showHiddenSeries)
        {
            if (fullSeriesList == null)
            {
                fullSeriesList = await FillLists.GetAllSeriesList(showHiddenSeries);
            }
        }

        public async Task SetViewModelData()
        {
            if (!RefreshView)
            {
                this.SetIsBusyTrue();

                var temp = this.FilteredSeriesList;
                this.FilteredSeriesList = null;
                this.FilteredSeriesList = temp;

                this.SetIsBusyFalse();
            }

            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await SetList(this.ShowHiddenSeries);

                    if (fullSeriesList != null)
                    {
                        this.FilteredSeriesList = fullSeriesList;

                        this.TotalSeriesCount = fullSeriesList != null ? fullSeriesList.Count : 0;

                        this.SearchOnSeries(this.Searchstring);

                        var sortList = SortLists.SortSeriesList(
                                this.FilteredSeriesList,
                                this.SeriesNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

                        this.FilteredSeriesCount = this.FilteredSeriesList.Count;

                        this.TotalSeriesstring = StringManipulation.SetTotalSeriesString(this.FilteredSeriesCount, this.TotalSeriesCount);

                        this.ShowCollectionViewFooter = this.FilteredSeriesCount > 0;

                        await Task.WhenAll(sortList);

                        this.FilteredSeriesList = sortList.Result;
                    }

                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
        }

        [RelayCommand]
        public void SearchOnSeries(string? input)
        {
            this.SetIsBusyTrue();

            this.Searchstring = input;

            if (this.FilteredSeriesList != null && this.FullSeriesList != null)
            {
                if (!string.IsNullOrEmpty(this.Searchstring))
                {
                    this.FilteredSeriesList = this.FullSeriesList.Where(x => !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.Contains(this.Searchstring.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                }
                else
                {
                    this.FilteredSeriesList = this.FullSeriesList;
                }

                this.FilteredSeriesCount = this.FilteredSeriesList != null ? this.FilteredSeriesList.Count : 0;

                this.TotalSeriesstring = StringManipulation.SetTotalSeriesString(this.FilteredSeriesCount, this.TotalSeriesCount);
            }

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuSeries(Guid? input)
        {
            if (this.FilteredSeriesList != null)
            {
                var selected = this.FilteredSeriesList.FirstOrDefault(x => x.SeriesGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.SeriesName))
                {
                    var action = await PopupMenu(selected.SeriesName);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await this.EditSeries(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await this.DeleteSeries(selected);
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
        public async Task AddSeries()
        {
            this.SetIsBusyTrue();

            var view = new SeriesEditView(new SeriesModel(), $"{AppStringResources.AddNewSeries}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditSeries(SeriesModel selected)
        {
            this.SetIsBusyTrue();

            var view = new SeriesEditView(selected, $"{AppStringResources.EditSeries}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteSeries(SeriesModel selected)
        {
            if (!string.IsNullOrEmpty(selected.SeriesName))
            {
                var answer = await DeleteCheck(selected.SeriesName);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        await Database.DeleteSeriesAsync(ConvertTo<SeriesDatabaseModel>(selected));
                        this.RemoveFromStaticList(selected);

                        await ConfirmDelete(selected.SeriesName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await DisplayMessage("Error!", ex.Message);
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
                    SeriesNameVisible = true,
                    SeriesNameChecked = this.SeriesNameChecked,
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
            this.ShowHiddenSeries = Preferences.Get("HiddenSeriesOn", true /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.SeriesNameChecked = Preferences.Get($"{this.ViewTitle}_SeriesNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }

        private void RemoveFromStaticList(SeriesModel selected)
        {
            if (SeriesViewModel.fullSeriesList != null)
            {
                SeriesViewModel.RefreshView = this.RemoveSeriesFromStaticList(selected, SeriesViewModel.fullSeriesList, SeriesViewModel.filteredSeriesList);
            }
        }

        private bool RemoveSeriesFromStaticList(SeriesModel selected, ObservableCollection<SeriesModel> seriesList, ObservableCollection<SeriesModel>? filteredSeriesList)
        {
            var refresh = false;

            try
            {
                var oldSeries = seriesList.FirstOrDefault(x => x.SeriesGuid == selected.SeriesGuid);

                if (oldSeries != null)
                {
                    seriesList.Remove(oldSeries);
                    refresh = true;
                }

                if (filteredSeriesList != null)
                {
                    var filteredSeries = filteredSeriesList.FirstOrDefault(x => x.SeriesGuid == selected.SeriesGuid);

                    if (filteredSeries != null)
                    {
                        filteredSeriesList.Remove(filteredSeries);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }
    }
}
