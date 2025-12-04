using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
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
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                GetColors();

                int toBeRead = 0, reading = 0, read = 0;
                int favorite = 0, nonFavorite = 0;
                int zero = 0, one = 0, two = 0, three = 0, four = 0, five = 0;
                List<CountModel> collectionCounts = new List<CountModel>();
                List<CountModel> genreCounts = new List<CountModel>();
                List<CountModel> seriesCounts = new List<CountModel>();
                List<CountModel> authorCounts = new List<CountModel>();
                List<CountModel> locationCounts = new List<CountModel>();
                List<CountModel> formatCounts = new List<CountModel>();

                Task.WaitAll(
                [
                    Task.Run (async () => BooksReadCount = await FilterLists.GetBookCountReadInYear(DateTime.Now.Year, ShowHiddenBooks) ),
                    Task.Run (async () => PagesReadCount = await FilterLists.GetBookPageCountReadInYear(DateTime.Now.Year, ShowHiddenBooks) ),
                    Task.Run (async () => CostBooks = await FilterLists.GetPriceOfAllBooks(ShowHiddenBooks) ),
                    Task.Run (async () => TotalBooks = await FilterLists.GetAllBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => toBeRead = await FilterLists.GetToBeReadBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => reading = await FilterLists.GetReadingBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => read = await FilterLists.GetReadBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => favorite = await FilterLists.GetFavoriteBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => nonFavorite = await FilterLists.GetNonFavoriteBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => zero = await FilterLists.GetZeroStarBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => one = await FilterLists.GetOneStarBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => two = await FilterLists.GetTwoStarBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => three = await FilterLists.GetThreeStarBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => four = await FilterLists.GetFourStarBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => five = await FilterLists.GetFiveStarBooksListCount(TestData.BookList, ShowHiddenBooks) ),
                    Task.Run (async () => collectionCounts = await FilterLists.GetAllBooksInAllCollectionsList(TestData.CollectionList, ShowHiddenCollections, ShowHiddenBooks) ),
                    Task.Run (async () => genreCounts = await FilterLists.GetAllBooksInAllGenresList(TestData.GenreList, ShowHiddenGenres, ShowHiddenBooks) ),
                    Task.Run (async () => seriesCounts = await FilterLists.GetAllBooksInAllSeriesList(TestData.SeriesList, ShowHiddenSeries, ShowHiddenBooks) ),
                    Task.Run (async () => authorCounts = await FilterLists.GetAllBooksInAllAuthorsList(TestData.AuthorList, ShowHiddenAuthors, ShowHiddenBooks) ),
                    Task.Run (async () => locationCounts = await FilterLists.GetAllBooksInAllLocationsList(TestData.LocationList, ShowHiddenLocations, ShowHiddenBooks) ),
                    Task.Run (() => formatCounts = FilterLists.GetAllBooksAndBookFormatsList(ShowHiddenBooks) ),
                ]);

                SetUpReadingStatusChart(toBeRead, reading, read);
                SetUpFavoritesChart(favorite, nonFavorite);
                SetUpRatingsChart(zero, one, two, three, four, five);
                SetUpCollectionsChart(collectionCounts);
                SetUpGenresChart(genreCounts);
                SetUpSeriesChart(seriesCounts);
                SetUpAuthorsChart(authorCounts);
                SetUpLocationsChart(locationCounts);
                SetUpFormatsChart(formatCounts);

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
        private List<CountValues> SetShowReadingStatus(int toBeRead, int reading, int read)
        {
            List<CountValues> counts = new List<CountValues>();

            if (toBeRead > 0 ||
                reading > 0 ||
                read > 0)
            {
                ShowReadingStatus = true;

                counts.Add(new CountValues()
                {
                    Count = toBeRead,
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

        private void SetUpReadingStatusChart(int toBeRead, int reading, int read)
        {
            List<CountValues> counts = SetShowReadingStatus(toBeRead, reading ,read);

            if (ShowReadingStatus)
            {
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
        private List<CountValues> SetShowFavorites(int favorite, int nonFavorite)
        {
            List<CountValues> counts = new List<CountValues>();

            if (ShowFavorites)
            {
                if (favorite > 0 ||
                    nonFavorite > 0)
                {
                    ShowFavoritesStatus = true;

                    counts.Add(new CountValues()
                    {
                        Count = favorite,
                        Label = AppStringResources.Favorite
                    });
                    counts.Add(new CountValues()
                    {
                        Count = nonFavorite,
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

        private void SetUpFavoritesChart(int favorite, int nonFavorite)
        {
            List<CountValues> counts = SetShowFavorites(favorite, nonFavorite);

            if (ShowFavoritesStatus)
            {
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
        private List<CountValues> SetShowRatings(int zero, int one, int two, int three, int four, int five)
        {
            List<CountValues> counts = new List<CountValues>();

            if (ShowRatings)
            {
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

        private void SetUpRatingsChart(int zero, int one, int two, int three, int four, int five)
        {
            List<CountValues> counts = SetShowRatings(zero, one, two, three, four, five);

            if (ShowRatings)
            {
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
        private void SetShowCollections(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                ShowCollections = true;
            }
            else
                ShowCollections = false;
        }

        private void SetUpCollectionsChart(List<CountModel> counts)
        {
            SetShowCollections(counts);

            counts = counts.OrderByDescending(x => x.Count).ToList();

            if (ShowCollections)
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

                SetUpBarChart(values, "collections");
            }
        }
        #endregion

        #region Genres
        private void SetShowGenres(List<CountModel> counts)
        {
            if (counts.Any(x => x.Count > 0))
            {
                ShowGenres = true;
            }
            else
                ShowGenres = false;
        }

        private void SetUpGenresChart(List<CountModel> counts)
        {
            SetShowGenres(counts);

            counts = counts.OrderByDescending(x => x.Count).ToList();

            if (ShowGenres)
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

                SetUpBarChart(values, "genres");
            }
        }
        #endregion

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

                var max = 5;

                if (counts.Count < max)
                    max = counts.Count;

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
