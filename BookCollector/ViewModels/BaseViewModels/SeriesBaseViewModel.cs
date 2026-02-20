// <copyright file="SeriesBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.Models;
    using BookCollector.Views.Series;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class SeriesBaseViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<SeriesModel>? fullSeriesList;

        [ObservableProperty]
        public static ObservableCollection<SeriesModel>? filteredSeriesList1;

        [ObservableProperty]
        public static ObservableCollection<SeriesModel>? filteredSeriesList2;

        [ObservableProperty]
        public int totalSeriesCount;

        [ObservableProperty]
        public int filteredSeriesCount;

        [ObservableProperty]
        public SeriesModel? selectedSeries;

        [RelayCommand]
        public async Task SeriesSelectionChanged()
        {
            if (this.SelectedSeries != null && !string.IsNullOrEmpty(this.SelectedSeries.SeriesName))
            {
                var view = new SeriesMainView(this.SelectedSeries, this.SelectedSeries.SeriesName);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedSeries = null;
            }
        }
    }
}
