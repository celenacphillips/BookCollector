using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Core.Extensions;
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

        public SeriesViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.SeriesView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            Task.WaitAll(
            [
                Task.Run (async () => FullSeriesList = await FilterLists.GetAllSeriesList(TestData.SeriesList) ),
            ]);

            TotalSeriesCount = FullSeriesList.Count;

            FilteredSeriesList = FullSeriesList;
            FilteredSeriesCount = FilteredSeriesList.Count;

            TotalSeriesString = StringManipulation.SetTotalSeriesString(FilteredSeriesCount, TotalSeriesCount);

            SetIsBusyFalse();
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

        // TO DO
        [RelayCommand]
        public async Task AddSeries()
        {

        }

        // TO DO
        [RelayCommand]
        public async Task EditSeries(SeriesModel selected)
        {

        }

        // TO DO
        [RelayCommand]
        public async Task DeleteSeries(SeriesModel selected)
        {

        }
    }
}
