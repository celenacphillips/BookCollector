// <copyright file="LibraryStatisticsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Drawing;
using System.Globalization;

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

                if (string.IsNullOrEmpty(this.CostBooks))
                {
                    var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
                    var cultureInfo = new CultureInfo(cultureCode);
                    this.CostBooks = string.Format(cultureInfo, "{0:C}", 0);
                }

                this.GetPreferences();

                var cost = GetCounts.GetPriceOfAllBooks(this.ShowHiddenBooks);
                var total = GetCounts.GetAllBooksListCount(this.ShowHiddenBooks);
                var bookReadCount = GetCounts.GetBookCountReadInYear(DateTime.Now.Year, this.ShowHiddenBooks);
                var pageReadCount = GetCounts.GetBookPageCountReadInYear(DateTime.Now.Year, this.ShowHiddenBooks);
                var toBeReadCount = GetCounts.GetToBeReadBooksListCount(this.ShowHiddenBooks);
                var readingCount = GetCounts.GetReadingBooksListCount(this.ShowHiddenBooks);
                var readCount = GetCounts.GetReadBooksListCount(this.ShowHiddenBooks);
                var favoriteCount = GetCounts.GetBooksListCountByFavorite(this.ShowHiddenBooks, true);
                var nonFavoriteCount = GetCounts.GetBooksListCountByFavorite(this.ShowHiddenBooks, false);
                var zeroCount = GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 0);
                var oneCount = GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 1);
                var twoCount = GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 2);
                var threeCount = GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 3);
                var fourCount = GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 4);
                var fiveCount = GetCounts.GetBooksListCountByRating(this.ShowHiddenBooks, 5);
                var collections = GetCounts.GetAllBooksInAllCollectionsList(this.ShowHiddenCollections, this.ShowHiddenBooks, this.MaxListNumber);
                var genres = GetCounts.GetAllBooksInAllGenresList(this.ShowHiddenGenres, this.ShowHiddenBooks, this.MaxListNumber);
                var series = GetCounts.GetAllBooksInAllSeriesList(this.ShowHiddenSeries, this.ShowHiddenBooks, this.MaxListNumber);
                var authors = GetCounts.GetAllBooksInAllAuthorsList(this.ShowHiddenAuthors, this.ShowHiddenBooks, this.MaxListNumber);
                var locations = GetCounts.GetAllBooksInAllLocationsList(this.ShowHiddenLocations, this.ShowHiddenBooks, this.MaxListNumber);
                var formats = GetCounts.GetAllBooksAndBookFormatsList(this.ShowHiddenBooks);
                var formatPrices = GetCounts.GetPriceOfBooksAndBookFormatsList(this.ShowHiddenBooks);

                this.GetColors();

                await Task.WhenAll(
                    cost,
                    total,
                    bookReadCount,
                    pageReadCount,
                    toBeReadCount,
                    readingCount,
                    readCount,
                    favoriteCount,
                    nonFavoriteCount,
                    zeroCount,
                    oneCount,
                    twoCount,
                    threeCount,
                    fourCount,
                    fiveCount,
                    collections,
                    genres,
                    series,
                    authors,
                    locations,
                    formats,
                    formatPrices);

                await Task.Delay(1);

                this.CostBooks = cost.Result;
                this.TotalBooks = total.Result;
                this.BooksReadCount = bookReadCount.Result;
                this.PagesReadCount = pageReadCount.Result;
                var toBeRead = toBeReadCount.Result;
                var reading = readingCount.Result;
                var read = readCount.Result;
                var favorite = favoriteCount.Result;
                var nonFavorite = nonFavoriteCount.Result;
                var zero = zeroCount.Result;
                var one = oneCount.Result;
                var two = twoCount.Result;
                var three = threeCount.Result;
                var four = fourCount.Result;
                var five = fiveCount.Result;
                var collectionCounts = collections.Result;
                var genresCounts = genres.Result;
                var seriesCounts = series.Result;
                var authorsCounts = authors.Result;
                var locationsCounts = locations.Result;
                var formatsCounts = formats.Result;
                var formatPricesCounts = formatPrices.Result;

                this.SetUpReadingStatusChart(toBeRead, reading, read);
                this.SetUpFavoritesChart(favorite, nonFavorite);
                this.SetUpRatingsChart(zero, one, two, three, four, five);
                this.SetUpCollectionsChart(collectionCounts);
                this.SetUpGenresChart(genresCounts);
                this.SetUpSeriesChart(seriesCounts);
                this.SetUpAuthorsChart(authorsCounts);
                this.SetUpLocationsChart(locationsCounts);
                this.SetUpFormatsChart(formatsCounts);
                this.SetUpFormatPricesChart(formatPricesCounts);

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
#if DEBUG
                await DisplayMessage("Error!", ex.Message);
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

        /*********************** Reading Status Methods ***********************/
        private List<CountModel> SetShowReadingStatus(int toBeRead, int reading, int read)
        {
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

        private void SetUpReadingStatusChart(int toBeRead, int reading, int read)
        {
            List<CountModel> counts = this.SetShowReadingStatus(toBeRead, reading, read);

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
        private List<CountModel> SetShowFavorites(int favorite, int nonFavorite)
        {
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

        private void SetUpFavoritesChart(int favorite, int nonFavorite)
        {
            List<CountModel> counts = this.SetShowFavorites(favorite, nonFavorite);

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
        private List<CountModel> SetShowRatings(int zero, int one, int two, int three, int four, int five)
        {
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

        private void SetUpRatingsChart(int zero, int one, int two, int three, int four, int five)
        {
            List<CountModel> counts = this.SetShowRatings(zero, one, two, three, four, five);

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

        private void SetUpCollectionsChart(List<CountModel> counts)
        {
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

        private void SetUpGenresChart(List<CountModel> counts)
        {
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
    }
}
