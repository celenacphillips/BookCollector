// <copyright file="SeriesEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Series
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.Views.Series;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class SeriesEditViewModel : SeriesBaseViewModel
    {
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public SeriesModel editedSeries;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool seriesNameNotValid;

        public SeriesEditViewModel(SeriesModel series, ContentPage view)
        {
            this.View = view;

            this.EditedSeries = (SeriesModel)series.Clone();
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
        public async Task SaveSeries()
        {
            try
            {
                this.SetIsBusyTrue();

                if (this.SeriesNameNotValid)
                {
                    await this.DisplayMessage(AppStringResources.SeriesNameNotValid, null);
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

                    this.EditedSeries = await Database.SaveSeriesAsync(ConvertTo<SeriesDatabaseModel>(this.EditedSeries));
                    await AddToStaticList(this.EditedSeries);

                    if (this.InsertMainViewBefore)
                    {
                        var view = new SeriesMainView(this.EditedSeries, $"{this.EditedSeries.SeriesName}");
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

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public void ValidateSeriesName()
        {
            this.ValidateEntry();
        }

        public static async Task AddToStaticList(SeriesModel series)
        {
            if (SeriesViewModel.fullSeriesList != null)
            {
                SeriesViewModel.RefreshView = await AddSeriesToStaticList(series, SeriesViewModel.fullSeriesList, SeriesViewModel.filteredSeriesList2);
            }
        }

        private static async Task<bool> AddSeriesToStaticList(SeriesModel series, ObservableCollection<SeriesModel> seriesList, ObservableCollection<SeriesModel>? filteredSeriesList)
        {
            var refresh = false;

            await Task.WhenAll(
            [
                series.SetTotalBooks(true),
                series.SetTotalCostOfBooks(true),
            ]);

            try
            {
                var oldSeries = seriesList.FirstOrDefault(x => x.SeriesGuid == series.SeriesGuid);

                if (oldSeries != null)
                {
                    var index = seriesList.IndexOf(oldSeries);
                    seriesList.Remove(oldSeries);
                    seriesList.Insert(index, series);
                    refresh = true;
                }
                else
                {
                    seriesList.Add(series);
                    refresh = true;
                }

                if (filteredSeriesList != null)
                {
                    var filteredSeries = filteredSeriesList.FirstOrDefault(x => x.SeriesGuid == series.SeriesGuid);

                    if (filteredSeries != null)
                    {
                        var index = filteredSeriesList.IndexOf(filteredSeries);
                        filteredSeriesList.Remove(filteredSeries);
                        filteredSeriesList.Insert(index, series);
                        refresh = true;
                    }
                    else
                    {
                        filteredSeriesList.Add(series);
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
            this.SeriesNameNotValid = string.IsNullOrEmpty(this.EditedSeries.SeriesName);
        }
    }
}
