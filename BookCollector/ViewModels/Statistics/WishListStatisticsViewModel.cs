// <copyright file="WishListStatisticsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.Input;

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

                this.GetPreferences();

                this.GetColors();

                Task.WaitAll(
                [
                    Task.Run(async () => this.CostBooks = await GetCounts.GetPriceOfAllWishListBooks(this.ShowHiddenWishlistBooks)),
                    Task.Run(async () => this.TotalBooks = await GetCounts.GetAllWishListBooksListCount(this.ShowHiddenWishlistBooks)),
                ]);

                this.SetUpSeriesChart();
                this.SetUpAuthorsChart();
                this.SetUpLocationsChart();
                this.SetUpFormatsChart();
                this.SetUpFormatPricesChart();

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

        /*********************** Series Methods ***********************/
        internal void SetShowSeries(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                this.ShowSeries = true;
            }
            else
            {
                this.ShowSeries = false;
            }
        }

        internal async void SetUpSeriesChart()
        {
            List<CountModel> counts = [];

            Task.WaitAll(
            [
                Task.Run(async () => counts = await GetCounts.GetAllWishListBooksAndSeriesList(this.ShowHiddenWishlistBooks, this.MaxListNumber)),
            ]);

            this.SetShowSeries(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            counts = counts.Where(x => x.Count > 0).ToList();

            if (this.ShowSeries)
            {
                List<ChartValues> values = [];

                var max = this.MaxListNumber;

                if (counts.Count < max)
                {
                    max = counts.Count;
                }

                if (max != 1)
                {
                    this.TopXSeries = AppStringResources.TopXSeries.Replace("x", $"{max}");
                }
                else
                {
                    this.TopXSeries = AppStringResources.TopSeries;
                }

                for (int i = 0; i < max; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = this.ColorList?[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count,
                        });
                }

                this.SetUpBarChart(values, "series");
            }
        }

        /*********************** Series Methods ***********************/

        /*********************** Authors Methods ***********************/
        internal void SetShowAuthors(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                this.ShowAuthors = true;
            }
            else
            {
                this.ShowAuthors = false;
            }
        }

        internal async void SetUpAuthorsChart()
        {
            List<CountModel> counts = [];

            Task.WaitAll(
            [
                Task.Run(async () => counts = await GetCounts.GetAllWishListBooksAndAuthorList(this.ShowHiddenWishlistBooks, this.MaxListNumber)),
            ]);

            this.SetShowAuthors(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            counts = counts.Where(x => x.Count > 0).ToList();

            if (this.ShowAuthors)
            {
                List<ChartValues> values = [];

                var max = this.MaxListNumber;

                if (counts.Count < max)
                {
                    max = counts.Count;
                }

                if (max != 1)
                {
                    this.TopXAuthors = AppStringResources.TopXAuthors.Replace("x", $"{max}");
                }
                else
                {
                    this.TopXAuthors = AppStringResources.TopAuthor;
                }

                for (int i = 0; i < max; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = this.ColorList?[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count,
                        });
                }

                this.SetUpBarChart(values, "authors");
            }
        }

        /*********************** Authors Methods ***********************/

        /*********************** Locations Methods ***********************/
        internal void SetShowLocations(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                this.ShowLocations = true;
            }
            else
            {
                this.ShowLocations = false;
            }
        }

        internal async void SetUpLocationsChart()
        {
            List<CountModel> counts = [];

            Task.WaitAll(
            [
                Task.Run(async () => counts = await GetCounts.GetAllWishListBooksAndLocationList(this.ShowHiddenWishlistBooks, this.MaxListNumber)),
            ]);

            this.SetShowLocations(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            counts = counts.Where(x => x.Count > 0).ToList();

            if (this.ShowLocations)
            {
                List<ChartValues> values = [];

                var max = this.MaxListNumber;

                if (counts.Count < max)
                {
                    max = counts.Count;
                }

                if (max != 1)
                {
                    this.TopXLocations = AppStringResources.TopXLocations.Replace("x", $"{max}");
                }
                else
                {
                    this.TopXLocations = AppStringResources.TopLocation;
                }

                for (int i = 0; i < max; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = this.ColorList?[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count,
                        });
                }

                this.SetUpBarChart(values, "locations");
            }
        }

        /*********************** Locations Methods ***********************/

        /*********************** Formats Methods ***********************/
        internal void SetShowFormats(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                this.ShowFormats = true;
            }
            else
            {
                this.ShowFormats = false;
            }
        }

        internal async void SetUpFormatsChart()
        {
            List<CountModel> counts = [];

            Task.WaitAll(
            [
                Task.Run(async () => counts = await GetCounts.GetAllWishListBooksAndBookFormatsList(this.ShowHiddenWishlistBooks)),
            ]);

            this.SetShowFormats(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            if (this.ShowFormats)
            {
                List<ChartValues> values = [];

                var max = this.MaxListNumber;

                if (counts.Count < max)
                {
                    max = counts.Count;
                }

                for (int i = 0; i < max; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = this.ColorList?[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count,
                        });
                }

                this.SetUpPieChart(values, "formats");
            }
        }

        /*********************** Formats Methods ***********************/

        /*********************** Format Prices Methods ***********************/
        internal void SetShowFormatPrices(List<CountModel> counts)
        {
            if (counts.Any(x => x.CountDouble > 0))
            {
                this.ShowFormatPrices = true;
            }
            else
            {
                this.ShowFormatPrices = false;
            }
        }

        internal async void SetUpFormatPricesChart()
        {
            List<CountModel> counts = [];

            Task.WaitAll(
            [
                Task.Run(async () => counts = await GetCounts.GetPriceOfWishListBooksAndBookFormatsList(this.ShowHiddenWishlistBooks)),
            ]);

            this.SetShowFormatPrices(counts);

            counts = [.. counts.OrderByDescending(x => x.CountDouble)];

            if (this.ShowFormatPrices)
            {
                List<ChartValues> values = [];

                for (int i = 0; i < counts.Count; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = this.ColorList?[i],
                            LabelValue = counts[i].Label,
                            Value = (float)counts[i].CountDouble,
                        });
                }

                this.SetUpPieChart(values, "formatprices");
            }
        }

        /*********************** Format Prices Methods ***********************/
    }
}
