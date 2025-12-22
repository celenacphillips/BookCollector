// <copyright file="WishListStatisticsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;

namespace BookCollector.ViewModels.Statistics
{
    public partial class WishListStatisticsViewModel : StatisticsBaseViewModel
    {
        public WishListStatisticsViewModel(ContentPage view)
        {
            this.View = view;
            this.MaxListNumber = 5;
        }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
                var cultureInfo = new CultureInfo(cultureCode);
                this.CostBooks = string.Format(cultureInfo, "{0:C}", 0);

                this.GetPreferences();

                var cost = GetCounts.GetPriceOfAllWishListBooks(this.ShowHiddenWishlistBooks);
                var total = GetCounts.GetAllWishListBooksListCount(this.ShowHiddenWishlistBooks);
                var series = GetCounts.GetAllBooksInAllSeriesList(this.ShowHiddenSeries, this.ShowHiddenBooks, this.MaxListNumber);
                var authors = GetCounts.GetAllBooksInAllSeriesList(this.ShowHiddenSeries, this.ShowHiddenBooks, this.MaxListNumber);
                var locations = GetCounts.GetAllBooksInAllLocationsList(this.ShowHiddenLocations, this.ShowHiddenBooks, this.MaxListNumber);
                var formats = GetCounts.GetAllBooksAndBookFormatsList(this.ShowHiddenBooks);
                var formatPrices = GetCounts.GetPriceOfBooksAndBookFormatsList(this.ShowHiddenBooks);

                this.GetColors();

                await Task.WhenAll(
                    cost,
                    total,
                    series,
                    authors,
                    locations,
                    formats,
                    formatPrices);

                this.CostBooks = cost.Result;
                this.TotalBooks = total.Result;
                var seriesCounts = series.Result;
                var authorsCounts = authors.Result;
                var locationsCounts = locations.Result;
                var formatsCounts = formats.Result;
                var formatPricesCounts = formatPrices.Result;

                var loadDataTasks = new Task[]
                {
                    Task.Run(() => this.SetUpSeriesChart(seriesCounts)),
                    Task.Run(() => this.SetUpAuthorsChart(authorsCounts)),
                    Task.Run(() => this.SetUpLocationsChart(locationsCounts)),
                    Task.Run(() => this.SetUpFormatsChart(formatsCounts)),
                    Task.Run(() => this.SetUpFormatPricesChart(formatPricesCounts)),
                };

                await Task.WhenAll(loadDataTasks);

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
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
    }
}
