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

                List<CountModel> seriesCounts = [];
                List<CountModel> authorCounts = [];
                List<CountModel> locationCounts = [];
                List<CountModel> formatCounts = [];
                List<CountModel> formatPriceCounts = [];

                Task.WaitAll(
               [
                    Task.Run(async () => this.CostBooks = await FilterLists.GetPriceOfAllWishListBooks(this.ShowHiddenWishlistBooks)),
                    Task.Run(async () => this.TotalBooks = await FilterLists.GetAllWishListBooksListCount(this.ShowHiddenWishlistBooks)),
                    Task.Run(async () => seriesCounts = await FilterLists.GetAllWishListBooksAndSeriesList(this.ShowHiddenWishlistBooks, this.MaxListNumber)),
                    Task.Run(async () => authorCounts = await FilterLists.GetAllWishListBooksAndAuthorList(this.ShowHiddenWishlistBooks, this.MaxListNumber)),
                    Task.Run(async () => locationCounts = await FilterLists.GetAllWishListBooksAndLocationList(this.ShowHiddenWishlistBooks, this.MaxListNumber)),
                    Task.Run(() => formatCounts = FilterLists.GetAllWishListBooksAndBookFormatsList(this.ShowHiddenWishlistBooks)),
                    Task.Run(() => formatPriceCounts = FilterLists.GetPriceOfWishListBooksAndBookFormatsList(this.ShowHiddenWishlistBooks)),
                ]);

                this.SetUpSeriesChart(seriesCounts);
                this.SetUpAuthorsChart(authorCounts);
                this.SetUpLocationsChart(locationCounts);
                this.SetUpFormatsChart(formatCounts);
                this.SetUpFormatPricesChart(formatPriceCounts);

                this.SetIsBusyFalse();
            }
            catch (Exception)
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
        private void SetShowSeries(List<CountModel> counts)
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

        private void SetUpSeriesChart(List<CountModel> counts)
        {
            this.SetShowSeries(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

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
        private void SetShowAuthors(List<CountModel> counts)
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

        private void SetUpAuthorsChart(List<CountModel> counts)
        {
            this.SetShowAuthors(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

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

        /*********************** Series Methods ***********************/

        /*********************** Locations Methods ***********************/
        private void SetShowLocations(List<CountModel> counts)
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

        private void SetUpLocationsChart(List<CountModel> counts)
        {
            this.SetShowLocations(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

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
        private void SetShowFormats(List<CountModel> counts)
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

        private void SetUpFormatsChart(List<CountModel> counts)
        {
            this.SetShowFormats(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            if (this.ShowFormats)
            {
                List<ChartValues> values = [];

                var max = 5;

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
        private void SetShowFormatPrices(List<CountModel> counts)
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

        private void SetUpFormatPricesChart(List<CountModel> counts)
        {
            this.SetShowFormatPrices(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

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
