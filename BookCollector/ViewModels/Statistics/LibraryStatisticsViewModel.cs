using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Statistics
{
    public partial class LibraryStatisticsViewModel : StatisticsBaseViewModel
    {
        [ObservableProperty]
        public bool showReadingStatus;

        [ObservableProperty]
        public bool showFavoritesStatus;

        [ObservableProperty]
        public bool showRatingStatus;

        [ObservableProperty]
        public bool showCollections;

        [ObservableProperty]
        public bool showGenres;

        [ObservableProperty]
        public string booksReadString;

        [ObservableProperty]
        public int booksReadCount;

        [ObservableProperty]
        public string pagesReadString;

        [ObservableProperty]
        public int pagesReadCount;

        public LibraryStatisticsViewModel(ContentPage view)
        {
            _view = view;
            BooksReadString = AppStringResources.BooksReadThisYear.Replace("yyyy", DateTime.Now.Year.ToString());
            PagesReadString = AppStringResources.PagesReadThisYear.Replace("yyyy", DateTime.Now.Year.ToString());
        }

        public async Task SetViewModelData()
        {
            GetPreferences();

            try
            {
                SetIsBusyTrue();

                double bookItemsPrice = 0;
                //var loadDataTasks = new Task[]
                //{
                //    Task.Run(async () => bookItemsPrice = await _database.GetBookItemsCostAsync()),
                //    Task.Run(async () => TotalBooks = await _database.GetBookItemsCountAsync()),
                //    Task.Run(async () => BooksReadCount = await _database.GetBookItemsCountReadInYearAsync(DateTime.Now.Year)),
                //    Task.Run(async () => PagesReadCount = await _database.GetBookItemsCountPagesReadInYearAsync(DateTime.Now.Year))
                //};

                //CostBooks = SetPriceString(bookItemsPrice);

                var primaryHasValue = Application.Current.Resources.TryGetValue("Primary", out object primaryColor);

                if (primaryHasValue)
                {
                    Color primary = (Color)primaryColor;
                    ColorList = [primary];

                    var appTheme = Application.Current.PlatformAppTheme;

                    await Task.WhenAll([
                        SetUpReadingStatusChart(primary, appTheme),
                        SetUpFavoritesChart(primary, appTheme),
                        SetUpRatingsChart(primary, appTheme),
                        SetUpFormatsChart(primary, appTheme),
                        SetUpCollectionsChart(primary, appTheme),
                        SetUpGenresChart(primary, appTheme),
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

        #region Reading Status
        private async Task<List<CountValues>> SetShowReadingStatus()
        {
            List<CountValues> counts = new List<CountValues>();

            int unread = 0, reading = 0, read = 0;

            //var loadDataTasks = new Task[]
            //{
            //    Task.Run(async () => unread = await _database.GetBookItemsUnreadCountAsync()),
            //    Task.Run(async () => reading = await _database.GetBookItemsReadingCountAsync()),
            //    Task.Run(async () => read = await _database.GetBookItemsReadCountAsync()),
            //};

            if (unread > 0 ||
                reading > 0 ||
                read > 0)
            {
                ShowReadingStatus = true;

                counts.Add(new CountValues()
                {
                    Count = unread,
                    Label = AppStringResources.ToBeRead
                });
                counts.Add(new CountValues()
                {
                    Count = reading,
                    Label = AppStringResources.Reading
                });
                counts.Add(new CountValues()
                {
                    Count = read,
                    Label = AppStringResources.Read
                });
            }
            else
                ShowReadingStatus = false;

            return counts;
        }

        private async Task SetUpReadingStatusChart(Color primary, AppTheme appTheme)
        {
            List<CountValues> counts = await SetShowReadingStatus();

            if (ShowReadingStatus)
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

                SetUpPieChart(values, "readingStatus");
            }
        }
        #endregion

        #region Favorites
        private async Task<List<CountValues>> SetShowFavorites()
        {
            List<CountValues> counts = new List<CountValues>();

            if (ShowFavorites)
            {
                int favorite = 0, notFavorite = 0;

                //var loadDataTasks = new Task[]
                //{
                //    Task.Run(async () => favorite = await _database.GetBookItemsFavoriteCountAsync()),
                //    Task.Run(async () => notFavorite = await _database.GetBookItemsNotFavoriteCountAsync()),
                //};

                if (favorite > 0 ||
                    notFavorite > 0)
                {
                    ShowFavoritesStatus = true;

                    counts.Add(new CountValues()
                    {
                        Count = favorite,
                        Label = AppStringResources.Favorite
                    });
                    counts.Add(new CountValues()
                    {
                        Count = notFavorite,
                        Label = AppStringResources.NonFavorite
                    });
                }
                else
                    ShowFavoritesStatus = false;
            }
            else
                ShowFavoritesStatus = false;

            return counts;
        }

        private async Task SetUpFavoritesChart(Color primary, AppTheme appTheme)
        {
            List<CountValues> counts = await SetShowFavorites();

            if (ShowFavoritesStatus)
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

                SetUpPieChart(values, "favorites");
            }
        }
        #endregion

        #region Ratings
        private async Task<List<CountValues>> SetShowRatings()
        {
            List<CountValues> counts = new List<CountValues>();

            if (ShowRatings)
            {
                int zero = 0, one = 0, two = 0, three = 0, four = 0, five = 0;

                //var loadDataTasks = new Task[]
                //{
                //    Task.Run(async () => zero = await _database.GetBookItemsZeroStarRatingCountAsync()),
                //    Task.Run(async () => one = await _database.GetBookItemsOneStarRatingCountAsync()),
                //    Task.Run(async () => two = await _database.GetBookItemsTwoStarRatingCountAsync()),
                //    Task.Run(async () => three = await _database.GetBookItemsThreeStarRatingCountAsync()),
                //    Task.Run(async () => four = await _database.GetBookItemsFourStarRatingCountAsync()),
                //    Task.Run(async () => five = await _database.GetBookItemsFiveStarRatingCountAsync()),
                //};

                if (zero > 0 ||
                   one > 0 ||
                   two > 0 ||
                   three > 0 ||
                   four > 0 ||
                   five > 0)
                {
                    ShowRatingStatus = true;

                    counts.Add(new CountValues()
                    {
                        Count = zero,
                        Label = AppStringResources.ZeroStars
                    });
                    counts.Add(new CountValues()
                    {
                        Count = one,
                        Label = AppStringResources.OneStar
                    });
                    counts.Add(new CountValues()
                    {
                        Count = two,
                        Label = AppStringResources.TwoStars
                    });
                    counts.Add(new CountValues()
                    {
                        Count = three,
                        Label = AppStringResources.ThreeStars
                    });
                    counts.Add(new CountValues()
                    {
                        Count = four,
                        Label = AppStringResources.FourStars
                    });
                    counts.Add(new CountValues()
                    {
                        Count = five,
                        Label = AppStringResources.FiveStars
                    });
                }
                else
                    ShowRatingStatus = false;
            }
            else
                ShowRatingStatus = false;

            return counts;
        }

        private async Task SetUpRatingsChart(Color primary, AppTheme appTheme)
        {
            List<CountValues> counts = await SetShowRatings();

            if (ShowRatingStatus)
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

                SetUpPieChart(values, "ratings");
            }
        }
        #endregion

        #region Collections
        private async Task<List<CountValues>> SetShowCollections()
        {
            //List<CountValues> counts = await _database.GetCollectionItemsAndBookCountsAsync(5);

            List<CountValues> counts = null;

            if (counts.Any(x => x.Count > 0))
            {
                ShowCollections = true;
            }
            else
                ShowCollections = false;

            return counts;
        }

        private async Task SetUpCollectionsChart(Color primary, AppTheme appTheme)
        {
            List<CountValues> counts = await SetShowCollections();

            if (ShowCollections)
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

                SetUpBarChart(values, "collections");
            }
        }
        #endregion

        #region Genres
        private async Task<List<CountValues>> SetShowGenres()
        {
            //List<CountValues> counts = await _database.GetGenreItemsAndBookCountsAsync(5);

            List<CountValues> counts = null;

            if (counts.Any(x => x.Count > 0))
            {
                ShowGenres = true;
            }
            else
                ShowGenres = false;

            return counts;
        }

        private async Task SetUpGenresChart(Color primary, AppTheme appTheme)
        {
            List<CountValues> counts = await SetShowGenres();

            if (ShowGenres)
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

                SetUpBarChart(values, "genres");
            }
        }
        #endregion

        #region Series
        private async Task<List<CountValues>> SetShowSeries()
        {
            //List<CountValues> counts = await _database.GetSeriesItemsAndBookCountsAsync(5);

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
            //List<CountValues> counts = await _database.GetAuthorItemsAndBookCountsAsync(5);

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
            //List<CountValues> counts = await _database.GetLocationsAndBookCountsAsync(5);

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
            //List<CountValues> counts = await _database.GetFormatsAndBookCountsAsync(5);

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
