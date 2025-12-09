using BookCollector.Data.Models;
using BookCollector.Views.Series;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class SeriesBaseViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public int totalSeriesCount;

        [ObservableProperty]
        public int filteredSeriesCount;

        [ObservableProperty]
        public static ObservableCollection<SeriesModel>? fullSeriesList;

        [ObservableProperty]
        public static ObservableCollection<SeriesModel>? filteredSeriesList;

        [ObservableProperty]
        public SeriesModel? selectedSeries;

        [RelayCommand]
        public async Task SeriesSelectionChanged()
        {
            if (SelectedSeries != null && !string.IsNullOrEmpty(SelectedSeries.SeriesName))
            {
                var view = new SeriesMainView(SelectedSeries, SelectedSeries.SeriesName);

                await Shell.Current.Navigation.PushAsync(view);
                SelectedSeries = null;
            }
        }
    }
}
