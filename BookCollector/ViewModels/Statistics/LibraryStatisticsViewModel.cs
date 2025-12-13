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

                int toBeRead = 0, reading = 0, read = 0;
                int favorite = 0, nonFavorite = 0;
                int zero = 0, one = 0, two = 0, three = 0, four = 0, five = 0;
                List<CountModel> collectionCounts = [];
                List<CountModel> genreCounts = [];
                List<CountModel> seriesCounts = [];
                List<CountModel> authorCounts = [];
                List<CountModel> locationCounts = [];
                List<CountModel> formatCounts = [];
                List<CountModel> formatPriceCounts = [];

                Task.WaitAll(
               [
                    Task.Run(async () => this.BooksReadCount = await FilterLists.GetBookCountReadInYear(DateTime.Now.Year, this.ShowHiddenBooks)),
                    Task.Run(async () => this.PagesReadCount = await FilterLists.GetBookPageCountReadInYear(DateTime.Now.Year, this.ShowHiddenBooks)),
                    Task.Run(async () => this.CostBooks = await FilterLists.GetPriceOfAllBooks(this.ShowHiddenBooks)),
                    Task.Run(async () => this.TotalBooks = await FilterLists.GetAllBooksListCount(this.ShowHiddenBooks)),
                    Task.Run(async () => toBeRead = await FilterLists.GetToBeReadBooksListCount(this.ShowHiddenBooks)),
                    Task.Run(async () => reading = await FilterLists.GetReadingBooksListCount(this.ShowHiddenBooks)),
                    Task.Run(async () => read = await FilterLists.GetReadBooksListCount(this.ShowHiddenBooks)),
                    Task.Run(async () => favorite = await FilterLists.GetBooksListCountByFavorite(this.ShowHiddenBooks, true)),
                    Task.Run(async () => nonFavorite = await FilterLists.GetBooksListCountByFavorite(this.ShowHiddenBooks, false)),
                    Task.Run(async () => zero = await FilterLists.GetBooksListCountByRating(this.ShowHiddenBooks, 0)),
                    Task.Run(async () => one = await FilterLists.GetBooksListCountByRating(this.ShowHiddenBooks, 1)),
                    Task.Run(async () => two = await FilterLists.GetBooksListCountByRating(this.ShowHiddenBooks, 2)),
                    Task.Run(async () => three = await FilterLists.GetBooksListCountByRating(this.ShowHiddenBooks, 3)),
                    Task.Run(async () => four = await FilterLists.GetBooksListCountByRating(this.ShowHiddenBooks, 4)),
                    Task.Run(async () => five = await FilterLists.GetBooksListCountByRating(this.ShowHiddenBooks, 5)),
                    Task.Run(async () => collectionCounts = await FilterLists.GetAllBooksInAllCollectionsList(this.ShowHiddenCollections, this.ShowHiddenBooks, this.MaxListNumber)),
                    Task.Run(async () => genreCounts = await FilterLists.GetAllBooksInAllGenresList(this.ShowHiddenGenres, this.ShowHiddenBooks, this.MaxListNumber)),
                    Task.Run(async () => seriesCounts = await FilterLists.GetAllBooksInAllSeriesList(this.ShowHiddenSeries, this.ShowHiddenBooks, this.MaxListNumber)),
                    Task.Run(async () => authorCounts = await FilterLists.GetAllBooksInAllAuthorsList(this.ShowHiddenAuthors, this.ShowHiddenBooks, this.MaxListNumber)),
                    Task.Run(async () => locationCounts = await FilterLists.GetAllBooksInAllLocationsList(this.ShowHiddenLocations, this.ShowHiddenBooks, this.MaxListNumber)),
                    Task.Run(() => formatCounts = FilterLists.GetAllBooksAndBookFormatsList(this.ShowHiddenBooks)),
                    Task.Run(() => formatPriceCounts = FilterLists.GetPriceOfBooksAndBookFormatsList(this.ShowHiddenBooks)),
                ]);

                this.SetUpReadingStatusChart(toBeRead, reading, read);
                this.SetUpFavoritesChart(favorite, nonFavorite);
                this.SetUpRatingsChart(zero, one, two, three, four, five);
                this.SetUpCollectionsChart(collectionCounts);
                this.SetUpGenresChart(genreCounts);
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

        /*********************** Authors Methods ***********************/

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
