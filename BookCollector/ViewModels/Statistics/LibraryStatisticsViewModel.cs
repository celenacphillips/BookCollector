// <copyright file="LibraryStatisticsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Statistics
{
    using System.Globalization;
    using BookCollector.Data;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Library;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// LibraryStatisticsViewModel class.
    /// </summary>
    public partial class LibraryStatisticsViewModel : StatisticsBaseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show the reading status chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showReadingStatus;

        /// <summary>
        /// Gets or sets a value indicating whether to show the favorites chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showFavoritesStatus;

        /// <summary>
        /// Gets or sets a value indicating whether to show the ratings chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showRatingStatus;

        /// <summary>
        /// Gets or sets a value indicating whether to show the collections chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showCollections;

        /// <summary>
        /// Gets or sets a value indicating whether to show the genres chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showGenres;

        /// <summary>
        /// Gets or sets the string for the top X collections, where X is a number defined by the max limit or the max number of collections.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? topXCollections;

        /// <summary>
        /// Gets or sets the string for the top X genres, where X is a number defined by the max limit or the max number of genres.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? topXGenres;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryStatisticsViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public LibraryStatisticsViewModel(ContentPage view)
        {
            this.View = view;
            this.MaxListNumber = 5;
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async new Task SetViewModelData()
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

                List<Task> taskList = [];
                List<Task<int>> dataTasks = [];

                if (ToBeReadViewModel.hiddenFilteredBookList == null || ToBeReadViewModel.RefreshView)
                {
                    taskList.Add(ToBeReadViewModel.SetList(this.ShowHiddenBooks));
                }

                if (ReadViewModel.hiddenFilteredBookList == null || ReadViewModel.RefreshView)
                {
                    taskList.Add(ReadViewModel.SetList(this.ShowHiddenBooks));
                }

                if (ReadingViewModel.hiddenFilteredBookList == null || ReadingViewModel.RefreshView)
                {
                    taskList.Add(ReadingViewModel.SetList(this.ShowHiddenBooks));
                }

                if (CollectionsViewModel.hiddenFilteredCollectionList == null || CollectionsViewModel.RefreshView)
                {
                    taskList.Add(CollectionsViewModel.SetList(this.ShowHiddenCollections));
                }

                if (GenresViewModel.hiddenFilteredGenreList == null || GenresViewModel.RefreshView)
                {
                    taskList.Add(GenresViewModel.SetList(this.ShowHiddenGenres));
                }

                if (SeriesViewModel.hiddenFilteredSeriesList == null || SeriesViewModel.RefreshView)
                {
                    taskList.Add(SeriesViewModel.SetList(this.ShowHiddenSeries));
                }

                if (AuthorsViewModel.hiddenFilteredAuthorList == null || AuthorsViewModel.RefreshView)
                {
                    taskList.Add(AuthorsViewModel.SetList(this.ShowHiddenAuthors));
                }

                if (LocationsViewModel.hiddenFilteredLocationList == null || LocationsViewModel.RefreshView)
                {
                    taskList.Add(LocationsViewModel.SetList(this.ShowHiddenLocations));
                }

                if (AllBooksViewModel.hiddenFilteredBookList == null || AllBooksViewModel.RefreshView)
                {
                    await AllBooksViewModel.SetList(this.ShowHiddenBooks);
                }

                var cost = GetCounts.GetPriceOfAllBooks();
                var formats = GetCounts.GetAllBooksAndBookFormatsList();
                var formatPrices = GetCounts.GetPriceOfBooksAndBookFormatsList();

                Task<int>? favoriteCount = null;
                Task<int>? nonFavoriteCount = null;

                if (this.ShowFavorites)
                {
                    favoriteCount = GetCounts.GetBooksListCountByFavorite(true);
                    nonFavoriteCount = GetCounts.GetBooksListCountByFavorite(false);

                    dataTasks.Add(favoriteCount);
                    dataTasks.Add(nonFavoriteCount);
                }

                Task<int>? zeroCount = null;
                Task<int>? oneCount = null;
                Task<int>? twoCount = null;
                Task<int>? threeCount = null;
                Task<int>? fourCount = null;
                Task<int>? fiveCount = null;

                if (this.ShowRatings)
                {
                    zeroCount = GetCounts.GetBooksListCountByRating(0);
                    oneCount = GetCounts.GetBooksListCountByRating(1);
                    twoCount = GetCounts.GetBooksListCountByRating(2);
                    threeCount = GetCounts.GetBooksListCountByRating(3);
                    fourCount = GetCounts.GetBooksListCountByRating(4);
                    fiveCount = GetCounts.GetBooksListCountByRating(5);

                    dataTasks.Add(zeroCount);
                    dataTasks.Add(oneCount);
                    dataTasks.Add(twoCount);
                    dataTasks.Add(threeCount);
                    dataTasks.Add(fourCount);
                    dataTasks.Add(fiveCount);
                }

                await Task.WhenAll(taskList);

                var collections = GetCounts.GetAllBooksInAllCollectionsList(this.ShowHiddenBooks, this.MaxListNumber);
                var genres = GetCounts.GetAllBooksInAllGenresList(this.ShowHiddenBooks, this.MaxListNumber);
                var series = GetCounts.GetAllBooksInAllSeriesList(this.ShowHiddenBooks, this.MaxListNumber);
                var authors = GetCounts.GetAllBooksInAllAuthorsList(this.ShowHiddenBooks, this.MaxListNumber);
                var locations = GetCounts.GetAllBooksInAllLocationsList(this.ShowHiddenBooks, this.MaxListNumber);

                this.GetColors();

                await Task.WhenAll(
                    cost,
                    collections,
                    genres,
                    series,
                    authors,
                    locations,
                    formats,
                    formatPrices);

                await Task.WhenAll(dataTasks);

                this.CostBooks = cost.Result;
                this.TotalBooks = AllBooksViewModel.hiddenFilteredBookList!.Count;
                var toBeRead = ToBeReadViewModel.hiddenFilteredBookList!.Count;
                var reading = ReadingViewModel.hiddenFilteredBookList!.Count;
                var read = ReadViewModel.hiddenFilteredBookList!.Count;
                var favorite = favoriteCount != null ? favoriteCount.Result : 0;
                var nonFavorite = nonFavoriteCount != null ? nonFavoriteCount.Result : 0;
                var zero = zeroCount != null ? zeroCount.Result : 0;
                var one = oneCount != null ? oneCount.Result : 0;
                var two = twoCount != null ? twoCount.Result : 0;
                var three = threeCount != null ? threeCount.Result : 0;
                var four = fourCount != null ? fourCount.Result : 0;
                var five = fiveCount != null ? fiveCount.Result : 0;
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
                await this.ViewModelCatch(ex);
            }
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

            counts = [.. counts.Where(x => x.Count > 0)];

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

            counts = [.. counts.Where(x => x.Count > 0)];

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
