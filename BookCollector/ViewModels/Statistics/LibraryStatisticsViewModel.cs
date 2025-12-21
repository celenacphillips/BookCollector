// <copyright file="LibraryStatisticsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        public string booksReadstring;

        [ObservableProperty]
        public int booksReadCount;

        [ObservableProperty]
        public string pagesReadstring;

        [ObservableProperty]
        public int pagesReadCount;

        [ObservableProperty]
        public string? topXCollections;

        [ObservableProperty]
        public string? topXGenres;

        public LibraryStatisticsViewModel(ContentPage view)
        {
            this.View = view;
            this.BooksReadstring = AppStringResources.BooksReadThisYear.Replace("yyyy", DateTime.Now.Year.ToString());
            this.PagesReadstring = AppStringResources.PagesReadThisYear.Replace("yyyy", DateTime.Now.Year.ToString());
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
                    Task.Run(async () => this.BooksReadCount = await GetCounts.GetBookCountReadInYear(DateTime.Now.Year, this.ShowHiddenBooks)),
                    Task.Run(async () => this.PagesReadCount = await GetCounts.GetBookPageCountReadInYear(DateTime.Now.Year, this.ShowHiddenBooks)),
                    Task.Run(async () => this.CostBooks = await GetCounts.GetPriceOfAllBooks(this.ShowHiddenBooks)),
                    Task.Run(async () => this.TotalBooks = await GetCounts.GetAllBooksListCount(this.ShowHiddenBooks)),
                ]);

                this.SetUpReadingStatusChart();
                this.SetUpFavoritesChart();
                this.SetUpRatingsChart();
                this.SetUpCollectionsChart();
                this.SetUpGenresChart();
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

        /*********************** Reading Status Methods ***********************/
        private List<CountModel> SetShowReadingStatus()
        {
            int toBeRead = 0;
            int reading = 0;
            int read = 0;

            Task.WaitAll(
            [
                Task.Run(async () => toBeRead = await GetCounts.GetToBeReadBooksListCount(this.ShowHiddenBooks)),
                Task.Run(async () => reading = await GetCounts.GetReadingBooksListCount(this.ShowHiddenBooks)),
                Task.Run(async () => read = await GetCounts.GetReadBooksListCount(this.ShowHiddenBooks)),
            ]);

            List<CountModel> counts = [];

            if (toBeRead > 0 ||
                reading > 0 ||
                read > 0)
            {
                this.ShowReadingStatus = true;

                counts.Add(new CountModel()
                {
                    Count = toBeRead,
                    Label = AppStringResources.ToBeRead,
                });
                counts.Add(new CountModel()
                {
                    Count = reading,
                    Label = AppStringResources.Reading,
                });
                counts.Add(new CountModel()
                {
                    Count = read,
                    Label = AppStringResources.Read,
                });
            }
            else
            {
                this.ShowReadingStatus = false;
            }

            return counts;
        }

        private async void SetUpReadingStatusChart()
        {
            List<CountModel> counts = this.SetShowReadingStatus();

            if (this.ShowReadingStatus)
            {
                List<ChartValues> values = [];

                for (int i = 0; i < counts.Count; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = this.ColorList?[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count,
                        });
                }

                this.SetUpPieChart(values, "readingStatus");
            }
        }

        /*********************** Reading Status Methods ***********************/

        /*********************** Favorites Methods ***********************/
        private List<CountModel> SetShowFavorites()
        {
            int favorite = 0;
            int nonFavorite = 0;

            Task.WaitAll(
            [
                Task.Run(async () => favorite = await GetCounts.GetBooksListCountByFavorite(this.ShowHiddenBooks, true)),
                Task.Run(async () => nonFavorite = await GetCounts.GetBooksListCountByFavorite(this.ShowHiddenBooks, false)),
            ]);

            List<CountModel> counts = [];

            if (this.ShowFavorites)
            {
                if (favorite > 0 ||
                    nonFavorite > 0)
                {
                    this.ShowFavoritesStatus = true;

                    counts.Add(new CountModel()
                    {
                        Count = favorite,
                        Label = AppStringResources.Favorite,
                    });
                    counts.Add(new CountModel()
                    {
                        Count = nonFavorite,
                        Label = AppStringResources.NonFavorite,
                    });
                }
                else
                {
                    this.ShowFavoritesStatus = false;
                }
            }
            else
            {
                this.ShowFavoritesStatus = false;
            }

            return counts;
        }

        private async void SetUpFavoritesChart()
        {
            List<CountModel> counts = this.SetShowFavorites();

            if (this.ShowFavoritesStatus)
            {
                List<ChartValues> values = [];

                for (int i = 0; i < counts.Count; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = this.ColorList?[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count,
                        });
                }

                this.SetUpPieChart(values, "favorites");
            }
        }

        /*********************** Favorites Methods ***********************/

        /*********************** Ratings Methods ***********************/
        private List<CountModel> SetShowRatings()
        {
            int zero = 0;
            int one = 0;
            int two = 0;
            int three = 0;
            int four = 0;
            int five = 0;

            Task.WaitAll(
            [
                Task.Run(async () => zero = await GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 0)),
                Task.Run(async () => one = await GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 1)),
                Task.Run(async () => two = await GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 2)),
                Task.Run(async () => three = await GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 3)),
                Task.Run(async () => four = await GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 4)),
                Task.Run(async () => five = await GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 5)),
            ]);

            List<CountModel> counts = [];

            if (this.ShowRatings)
            {
                if (zero > 0 ||
                   one > 0 ||
                   two > 0 ||
                   three > 0 ||
                   four > 0 ||
                   five > 0)
                {
                    this.ShowRatingStatus = true;

                    counts.Add(new CountModel()
                    {
                        Count = zero,
                        Label = AppStringResources.ZeroStars,
                    });
                    counts.Add(new CountModel()
                    {
                        Count = one,
                        Label = AppStringResources.OneStar,
                    });
                    counts.Add(new CountModel()
                    {
                        Count = two,
                        Label = AppStringResources.TwoStars,
                    });
                    counts.Add(new CountModel()
                    {
                        Count = three,
                        Label = AppStringResources.ThreeStars,
                    });
                    counts.Add(new CountModel()
                    {
                        Count = four,
                        Label = AppStringResources.FourStars,
                    });
                    counts.Add(new CountModel()
                    {
                        Count = five,
                        Label = AppStringResources.FiveStars,
                    });
                }
                else
                {
                    this.ShowRatingStatus = false;
                }
            }
            else
            {
                this.ShowRatingStatus = false;
            }

            return counts;
        }

        private async void SetUpRatingsChart()
        {
            List<CountModel> counts = this.SetShowRatings();

            if (this.ShowRatings)
            {
                List<ChartValues> values = [];

                for (int i = 0; i < counts.Count; i++)
                {
                    values.Add(
                        new ChartValues()
                        {
                            ColorValue = this.ColorList?[i],
                            LabelValue = counts[i].Label,
                            Value = counts[i].Count,
                        });
                }

                this.SetUpPieChart(values, "ratings");
            }
        }

        /*********************** Ratings Methods ***********************/

        /*********************** Collections Methods ***********************/
        private void SetShowCollections(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                this.ShowCollections = true;
            }
            else
            {
                this.ShowCollections = false;
            }
        }

        private async void SetUpCollectionsChart()
        {
            List<CountModel> counts = [];

            Task.WaitAll(
            [
                Task.Run(async () => counts = await GetCounts.GetAllBooksInAllCollectionsList(this.ShowHiddenCollections, this.ShowHiddenBooks, this.MaxListNumber)),
            ]);

            this.SetShowCollections(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            counts = counts.Where(x => x.Count > 0).ToList();

            if (this.ShowCollections)
            {
                List<ChartValues> values = [];

                var max = this.MaxListNumber;

                if (counts.Count < max)
                {
                    max = counts.Count;
                }

                if (max != 1)
                {
                    this.TopXCollections = AppStringResources.TopXCollections.Replace("x", $"{max}");
                }
                else
                {
                    this.TopXCollections = AppStringResources.TopCollection;
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

                this.SetUpBarChart(values, "collections");
            }
        }

        /*********************** Collections Methods ***********************/

        /*********************** Genres Methods ***********************/
        private void SetShowGenres(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                this.ShowGenres = true;
            }
            else
            {
                this.ShowGenres = false;
            }
        }

        private async void SetUpGenresChart()
        {
            List<CountModel> counts = [];

            Task.WaitAll(
            [
                Task.Run(async () => counts = await GetCounts.GetAllBooksInAllGenresList(this.ShowHiddenGenres, this.ShowHiddenBooks, this.MaxListNumber)),
            ]);

            this.SetShowGenres(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            counts = counts.Where(x => x.Count > 0).ToList();

            if (this.ShowGenres)
            {
                List<ChartValues> values = [];

                var max = this.MaxListNumber;

                if (counts.Count < max)
                {
                    max = counts.Count;
                }

                if (max != 1)
                {
                    this.TopXGenres = AppStringResources.TopXGenres.Replace("x", $"{max}");
                }
                else
                {
                    this.TopXGenres = AppStringResources.TopGenre;
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

                this.SetUpBarChart(values, "genres");
            }
        }

        /*********************** Genres Methods ***********************/

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
                Task.Run(async () => counts = await GetCounts.GetAllBooksInAllSeriesList(this.ShowHiddenSeries, this.ShowHiddenBooks, this.MaxListNumber)),
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
                Task.Run(async () => counts = await GetCounts.GetAllBooksInAllAuthorsList(this.ShowHiddenAuthors, this.ShowHiddenBooks, this.MaxListNumber)),
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
                Task.Run(async () => counts = await GetCounts.GetAllBooksInAllLocationsList(this.ShowHiddenLocations, this.ShowHiddenBooks, this.MaxListNumber)),
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
                Task.Run(async () => counts = await GetCounts.GetAllBooksAndBookFormatsList(this.ShowHiddenBooks)),
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
                Task.Run(async () => counts = await GetCounts.GetPriceOfBooksAndBookFormatsList(this.ShowHiddenBooks)),
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
