using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Statistics
{
    public partial class WishListStatisticsViewModel : StatisticsBaseViewModel
    {
        public WishListStatisticsViewModel(ContentPage view)
        {
            _view = view;
            MaxListNumber = 5;
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                GetColors();

                List<CountModel> seriesCounts = new List<CountModel>();
                List<CountModel> authorCounts = new List<CountModel>();
                List<CountModel> locationCounts = new List<CountModel>();
                List<CountModel> formatCounts = new List<CountModel>();
                List<CountModel> formatPriceCounts = new List<CountModel>();

                Task.WaitAll(
                [
                    Task.Run (async () => CostBooks = await FilterLists.GetPriceOfAllWishListBooks(ShowHiddenBooks) ),
                    Task.Run (async () => TotalBooks = await FilterLists.GetAllWishListBooksListCount(ShowHiddenBooks) ),
                    Task.Run (async () => seriesCounts = await FilterLists.GetAllWishListBooksAndSeriesList(ShowHiddenBooks, MaxListNumber) ),
                    Task.Run (async () => authorCounts = await FilterLists.GetAllWishListBooksAndAuthorList(ShowHiddenBooks, MaxListNumber) ),
                    Task.Run (async () => locationCounts = await FilterLists.GetAllWishListBooksAndLocationList(ShowHiddenBooks, MaxListNumber) ),
                    Task.Run (() => formatCounts = FilterLists.GetAllWishListBooksAndBookFormatsList(ShowHiddenBooks) ),
                    Task.Run (() => formatPriceCounts = FilterLists.GetPriceOfWishListBooksAndBookFormatsList(ShowHiddenBooks) ),
                ]);

                SetUpSeriesChart(seriesCounts);
                SetUpAuthorsChart(authorCounts);
                SetUpLocationsChart(locationCounts);
                SetUpFormatsChart(formatCounts);
                SetUpFormatPricesChart(formatPriceCounts);

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }


        #region Series
        private void SetShowSeries(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                ShowSeries = true;
            }
            else
                ShowSeries = false;
        }

        private void SetUpSeriesChart(List<CountModel> counts)
        {
            SetShowSeries(counts);

            counts = counts.OrderByDescending(x => x.Count).ToList();

            if (ShowSeries)
            {
                List<ChartValues> values = new List<ChartValues>();

                var max = MaxListNumber;

                if (counts.Count < max)
                {
                    max = counts.Count;
                }

                if (max != 1)
                    TopXSeries = AppStringResources.TopXSeries.Replace("x", $"{max}");
                else
                    TopXSeries = AppStringResources.TopSeries;

                for (int i = 0; i < max; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = ColorList[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count
                        });
                }

                SetUpBarChart(values, "series");
            }
        }
        #endregion

        #region Author
        private void SetShowAuthors(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                ShowAuthors = true;
            }
            else
                ShowAuthors = false;
        }

        private void SetUpAuthorsChart(List<CountModel> counts)
        {
            SetShowAuthors(counts);

            counts = counts.OrderByDescending(x => x.Count).ToList();

            if (ShowAuthors)
            {
                List<ChartValues> values = new List<ChartValues>();

                var max = MaxListNumber;

                if (counts.Count < max)
                {
                    max = counts.Count;
                }

                if (max != 1)
                    TopXAuthors = AppStringResources.TopXAuthors.Replace("x", $"{max}");
                else
                    TopXAuthors = AppStringResources.TopAuthor;

                for (int i = 0; i < max; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = ColorList[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count
                        });
                }

                SetUpBarChart(values, "authors");
            }
        }
        #endregion

        #region Location
        private void SetShowLocations(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                ShowLocations = true;
            }
            else
                ShowLocations = false;
        }

        private void SetUpLocationsChart(List<CountModel> counts)
        {
            SetShowLocations(counts);

            counts = counts.OrderByDescending(x => x.Count).ToList();

            if (ShowLocations)
            {
                List<ChartValues> values = new List<ChartValues>();

                var max = MaxListNumber;

                if (counts.Count < max)
                {
                    max = counts.Count;
                }

                if (max != 1)
                    TopXLocations = AppStringResources.TopXLocations.Replace("x", $"{max}");
                else
                    TopXLocations = AppStringResources.TopLocation;

                for (int i = 0; i < max; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = ColorList[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count
                        });
                }

                SetUpBarChart(values, "locations");
            }
        }

        #endregion

        #region Format
        private void SetShowFormats(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                ShowFormats = true;
            }
            else
                ShowFormats = false;
        }

        private void SetUpFormatsChart(List<CountModel> counts)
        {
            SetShowFormats(counts);

            counts = counts.OrderByDescending(x => x.Count).ToList();

            if (ShowFormats)
            {
                List<ChartValues> values = new List<ChartValues>();

                var max = 5;

                if (counts.Count < max)
                    max = counts.Count;

                for (int i = 0; i < max; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = ColorList[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count
                        });
                }

                SetUpPieChart(values, "formats");
            }
        }
        #endregion

        #region Format Price
        private void SetShowFormatPrices(List<CountModel> counts)
        {
            if (counts.Any(x => x.CountDouble > 0))
            {
                ShowFormatPrices = true;
            }
            else
                ShowFormatPrices = false;
        }

        private void SetUpFormatPricesChart(List<CountModel> counts)
        {
            SetShowFormatPrices(counts);

            counts = counts.OrderByDescending(x => x.Count).ToList();

            if (ShowFormatPrices)
            {
                List<ChartValues> values = new List<ChartValues>();

                for (int i = 0; i < counts.Count; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = ColorList[i],
                            LabelValue = counts[i].Label,
                            Value = (float)counts[i].CountDouble
                        });
                }

                SetUpPieChart(values, "formatprices");
            }
        }
        #endregion
    }
}
