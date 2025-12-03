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
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                double bookItemsPrice = 0;
                //var loadDataTasks = new Task[]
                //{
                //    Task.Run(async () => bookItemsPrice = await _database.GetWishListBookItemsCostAsync()),
                //    Task.Run(async () => TotalBooks = await _database.GetWishListBookItemsCountAsync()),
                //};

                //CostBooks = SetPriceString(bookItemsPrice);

                var primaryHasValue = Application.Current.Resources.TryGetValue("Primary", out object primaryColor);

                if (primaryHasValue)
                {
                    Color primary = (Color)primaryColor;
                    ColorList = [primary];

                    var appTheme = Application.Current.PlatformAppTheme;

                    await Task.WhenAll([
                        SetUpFormatsChart(primary, appTheme),
                        SetUpSeriesChart(primary, appTheme),
                        SetUpAuthorsChart(primary, appTheme),
                        SetUpLocationsChart(primary, appTheme),
                    ]);
                }

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
        private async Task<List<CountValues>> SetShowSeries()
        {
            //List<CountValues> counts = await _database.GetSeriesAndWishListBookCountsAsync(5);

            List<CountValues> counts = null;

            if (counts.Any(x => x.Count > 0))
            {
                ShowSeries = true;
            }
            else
                ShowSeries = false;

            return counts;
        }

        private async Task SetUpSeriesChart(Color primary, AppTheme appTheme)
        {
            List<CountValues> counts = await SetShowSeries();

            if (ShowSeries)
            {
                CalculateColors(primary, appTheme, counts.Count - 1);

                List<ChartValues> values = new List<ChartValues>();

                for (int i = 0; i < counts.Count; i++)
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
        private async Task<List<CountValues>> SetShowAuthors()
        {
            //List<CountValues> counts = await _database.GetAuthorsAndWishListBookCountsAsync(5);

            List<CountValues> counts = null;

            if (counts.Any(x => x.Count > 0))
            {
                ShowAuthors = true;
            }
            else
                ShowAuthors = false;

            return counts;
        }

        private async Task SetUpAuthorsChart(Color primary, AppTheme appTheme)
        {
            List<CountValues> counts = await SetShowAuthors();

            if (ShowAuthors)
            {
                CalculateColors(primary, appTheme, counts.Count - 1);

                List<ChartValues> values = new List<ChartValues>();

                for (int i = 0; i < counts.Count; i++)
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
        private async Task<List<CountValues>> SetShowLocations()
        {
            //List<CountValues> counts = await _database.GetLocationsAndWishListBookCountsAsync(5);

            List<CountValues> counts = null;

            if (counts.Any(x => x.Count > 0))
            {
                ShowLocations = true;
            }
            else
                ShowLocations = false;

            return counts;
        }

        private async Task SetUpLocationsChart(Color primary, AppTheme appTheme)
        {
            List<CountValues> counts = await SetShowLocations();

            if (ShowLocations)
            {
                CalculateColors(primary, appTheme, counts.Count - 1);

                List<ChartValues> values = new List<ChartValues>();

                for (int i = 0; i < counts.Count; i++)
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
        private async Task<List<CountValues>> SetShowFormats()
        {
            //List<CountValues> counts = await _database.GetFormatsAndWishListBookCountsAsync(5);

            List<CountValues> counts = null;

            if (counts.Any(x => x.Count > 0))
            {
                ShowFormats = true;
            }
            else
                ShowFormats = false;

            return counts;
        }

        private async Task SetUpFormatsChart(Color primary, AppTheme appTheme)
        {
            List<CountValues> counts = await SetShowFormats();

            if (ShowFormats)
            {
                CalculateColors(primary, appTheme, counts.Count - 1);

                List<ChartValues> values = new List<ChartValues>();

                for (int i = 0; i < counts.Count; i++)
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
    }
}
