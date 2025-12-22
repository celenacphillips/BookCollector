// <copyright file="SeriesViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Popups;
using BookCollector.Views.Series;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        }

        private bool ShowHiddenSeries { get; set; }

        private bool SeriesNameChecked { get; set; }

        private bool TotalBooksChecked { get; set; }

        private bool TotalPriceChecked { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();
                var fullList = FillLists.GetAllSeriesList(this.ShowHiddenSeries);

                await Task.WhenAll(fullList);

                this.FullSeriesList = fullList.Result;

                if (this.FullSeriesList != null)
                {
                    this.FilteredSeriesList = this.FullSeriesList;

                    var sortList = SortLists.SortSeriesList(
                            this.FilteredSeriesList,
                            this.SeriesNameChecked,
                            this.TotalBooksChecked,
                            this.TotalPriceChecked,
                            this.AscendingChecked,
                            this.DescendingChecked);

                    this.TotalSeriesCount = this.FullSeriesList.Count;

                    this.FilteredSeriesCount = this.FilteredSeriesList.Count;

                    this.TotalSeriesstring = StringManipulation.SetTotalCollectionsString(this.FilteredSeriesCount, this.TotalSeriesCount);

                    this.ShowCollectionViewFooter = this.FilteredSeriesCount > 0;

                    await Task.WhenAll(sortList);

                    this.FilteredSeriesList = sortList.Result;
                }

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public void SearchOnSeries(string? input)
        {
            this.SetIsBusyTrue();

            this.Searchstring = input;

            if (this.FilteredSeriesList != null)
            {
                if (!string.IsNullOrEmpty(this.Searchstring))
                {
                    this.FilteredSeriesList = this.FilteredSeriesList.Where(x => !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.Contains(this.Searchstring.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
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

                        if (TestData.UseTestData)
                        {
                            TestData.DeleteSeries(selected);
                        }
                        else
                        {
                            await Database.DeleteSeriesAsync(ConvertTo<SeriesDatabaseModel>(selected));
                        }

                        await ConfirmDelete(selected.SeriesName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
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

                await this.View.ShowPopupAsync(popup);
                await this.SetViewModelData();
            }
        }

        private void GetPreferences()
        {
            this.ShowHiddenSeries = Preferences.Get("HiddenSeriesOn", true /* Default */);
            this.ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.SeriesNameChecked = Preferences.Get($"{this.ViewTitle}_SeriesNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }
    }
}
