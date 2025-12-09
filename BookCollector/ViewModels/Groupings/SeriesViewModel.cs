using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.ViewModels.Series;
using BookCollector.Views.Popups;
using BookCollector.Views.Series;
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
    public partial class SeriesViewModel : SeriesBaseViewModel
    {
        [ObservableProperty]
        public string? totalSeriesString;

        private bool ShowHiddenSeries {  get; set; }
        private bool SeriesNameChecked { get; set; }
        private bool TotalBooksChecked { get; set; }
        private bool TotalPriceChecked { get; set; }

        public SeriesViewModel(ContentPage view)
        {
            View = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.SeriesView_InfoText}";
            ViewTitle = AppStringResources.Series;
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                Task.WaitAll(
                [
                    Task.Run (async () => FullSeriesList = await FilterLists.GetAllSeriesList(ShowHiddenSeries) ),
                ]);

                if (FullSeriesList != null)
                {
                    TotalSeriesCount = FullSeriesList.Count;

                    FilteredSeriesList = FullSeriesList;

                    foreach (var series in FullSeriesList)
                    {
                        await series.SetTotalBooks(ShowHiddenBook);
                    }

                    Task.WaitAll(
                    [
                        Task.Run (async () => FilteredSeriesList = await FilterLists.SortSeriesList(FilteredSeriesList,
                                                                                                    SeriesNameChecked,
                                                                                                    TotalBooksChecked,
                                                                                                    TotalPriceChecked,
                                                                                                    AscendingChecked,
                                                                                                    DescendingChecked) ),
                    ]);

                    FilteredSeriesCount = FilteredSeriesList.Count;

                    TotalSeriesString = StringManipulation.SetTotalSeriesString(FilteredSeriesCount, TotalSeriesCount);

                    ShowCollectionViewFooter = FilteredSeriesCount > 0;
                }

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public void SearchOnSeries(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (FilteredSeriesList != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                    FilteredSeriesList = FilteredSeriesList.Where(x => !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                else
                    FilteredSeriesList = FullSeriesList;

                FilteredSeriesCount = FilteredSeriesList != null ? FilteredSeriesList.Count : 0;

                TotalSeriesString = StringManipulation.SetTotalSeriesString(FilteredSeriesCount, TotalSeriesCount);
            }

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuSeries(Guid? input)
        {
            if (FilteredSeriesList != null)
            {
                var selected = FilteredSeriesList.FirstOrDefault(x => x.SeriesGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.SeriesName))
                {
                    var action = await PopupMenu(selected.SeriesName);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await EditSeries(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await DeleteSeries(selected);
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
        public async Task AddSeries()
        {
            SetIsBusyTrue();

            var view = new SeriesEditView(new SeriesModel(), $"{AppStringResources.AddNewSeries}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditSeries(SeriesModel selected)
        {
            SetIsBusyTrue();

            var view = new SeriesEditView(selected, $"{AppStringResources.EditSeries}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
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
                        SetIsBusyTrue();

                        if (TestData.UseTestData)
                        {
                            TestData.DeleteSeries(selected);
                        }
                        else
                        {

                        }

                        await ConfirmDelete(selected.SeriesName);

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
                    SeriesNameVisible = true,
                    SeriesNameChecked = SeriesNameChecked,
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
            ShowHiddenSeries = Preferences.Get("HiddenSeriesOn", true  /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true  /* Default */);

            SeriesNameChecked = Preferences.Get($"{ViewTitle}_SeriesNameSelection", true  /* Default */);
            TotalBooksChecked = Preferences.Get($"{ViewTitle}_TotalBooksSelection", false  /* Default */);
            TotalPriceChecked = Preferences.Get($"{ViewTitle}_TotalPriceSelection", false  /* Default */);

            AscendingChecked = Preferences.Get($"{ViewTitle}_AscendingSelection", true  /* Default */);
            DescendingChecked = Preferences.Get($"{ViewTitle}_DescendingSelection", false  /* Default */);
        }
    }
}
