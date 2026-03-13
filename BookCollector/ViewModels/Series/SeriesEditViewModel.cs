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

    /// <summary>
    /// SeriesEditViewModel class.
    /// </summary>
    public partial class SeriesEditViewModel : SeriesViewModel
    {
        /// <summary>
        /// Gets or sets the series to edit.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public SeriesModel editedSeries;

        /// <summary>
        /// Gets or sets a value indicating whether the series name is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool seriesNameNotValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesEditViewModel"/> class.
        /// </summary>
        /// <param name="series">Series to edit.</param>
        /// <param name="view">View related to view model.</param>
        public SeriesEditViewModel(SeriesModel series, ContentPage view)
            : base(view)
        {
            this.View = view;

            this.EditedSeries = (SeriesModel)series.Clone();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to insert the main view before or not.
        /// </summary>
        public bool InsertMainViewBefore { get; set; }

        /// <summary>
        /// Add series to the static list in the list view model.
        /// </summary>
        /// <param name="series">Series to add.</param>
        /// <returns>A task.</returns>
        public static async Task AddToStaticList(SeriesModel series)
        {
            if (SeriesViewModel.fullSeriesList != null)
            {
                SeriesViewModel.RefreshView = await AddSeriesToStaticList(series, SeriesViewModel.fullSeriesList, SeriesViewModel.filteredSeriesList2);
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
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

        /// <summary>
        /// Save series to the database and returns to the previous view.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Check if the series name is valid and set the related value.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateSeriesName()
        {
            this.ValidateEntry();
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
