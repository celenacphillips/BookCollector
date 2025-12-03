using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.ViewModels.Series;
using BookCollector.Views.Popups;
using BookCollector.Views.Series;
using CommunityToolkit.Maui.Core.Extensions;
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
            _view = view;
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

                // Unit test data
                var seriesList = TestData.SeriesList;

                Task.WaitAll(
                [
                    Task.Run (async () => FullSeriesList = await FilterLists.GetAllSeriesList(seriesList, ShowHiddenSeries) ),
                ]);

                TotalSeriesCount = FullSeriesList.Count;

                FilteredSeriesList = FullSeriesList;

                foreach (var series in fullSeriesList)
                {
                    series.SetTotalBooks(ShowHiddenBook);
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

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SearchOnSeries(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (!string.IsNullOrEmpty(SearchString))
                FilteredSeriesList = FilteredSeriesList.Where(x => x.SeriesName.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            else
                FilteredSeriesList = FullSeriesList;

            FilteredSeriesCount = FilteredSeriesList.Count;

            TotalSeriesString = StringManipulation.SetTotalSeriesString(FilteredSeriesCount, TotalSeriesCount);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuSeries(Guid? input)
        {
            var selected = FilteredSeriesList.FirstOrDefault(x => x.SeriesGuid == input);
            string? action = await PopupMenu(selected.SeriesName);

            switch (action)
            {
                case "Edit":
                    await EditSeries(selected);
                    break;

                case "Delete":
                    await DeleteSeries(selected);
                    break;

                default:
                    break;
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

            SeriesEditView view = new SeriesEditView(new SeriesModel(), $"{AppStringResources.AddNewSeries}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditSeries(SeriesModel selected)
        {
            SetIsBusyTrue();

            SeriesEditView view = new SeriesEditView(selected, $"{AppStringResources.EditSeries}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteSeries(SeriesModel selected)
        {
            bool answer = await DeleteCheck(selected.SeriesName);

            if (answer)
            {
                try
                {
                    SetIsBusyTrue();

                    // Unit test data
                    TestData.DeleteSeries(selected);

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

        [RelayCommand]
        public async Task SortPopup()
        {
            var popup = new SortPopup();
            SortPopupViewModel viewModel = new SortPopupViewModel(popup, ViewTitle)
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

            await _view.ShowPopupAsync(popup);
            await SetViewModelData();
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
