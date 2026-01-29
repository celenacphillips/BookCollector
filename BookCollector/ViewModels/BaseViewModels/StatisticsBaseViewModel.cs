// <copyright file="StatisticsBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class StatisticsBaseViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string? costBooks;

        [ObservableProperty]
        public int totalBooks;

        [ObservableProperty]
        public string? totalBooksString;

        [ObservableProperty]
        public bool showSeries;

        [ObservableProperty]
        public bool showAuthors;

        [ObservableProperty]
        public bool showLocations;

        [ObservableProperty]
        public bool showFormats;

        [ObservableProperty]
        public bool showFormatPrices;

        [ObservableProperty]
        public string? topXSeries;

        [ObservableProperty]
        public string? topXAuthors;

        [ObservableProperty]
        public string? topXLocations;

        public List<Color?>? ColorList { get; set; }

        public bool ShowFavorites { get; set; }

        public bool ShowRatings { get; set; }

        public bool ShowHiddenBooks { get; set; }

        public bool ShowHiddenCollections { get; set; }

        public bool ShowHiddenSeries { get; set; }

        public bool ShowHiddenLocations { get; set; }

        public bool ShowHiddenGenres { get; set; }

        public bool ShowHiddenAuthors { get; set; }

        public bool ShowHiddenWishlistBooks { get; set; }

        public int MaxListNumber { get; set; }

        public void SetUpPieChart(List<ChartValues> chartValues, string sectionName)
        {
            var textLight = (Color?)Application.Current?.Resources["TextLight"];
            var textDark = (Color?)Application.Current?.Resources["TextDark"];

            var section = (ChartView)this.View.FindByName(sectionName);

            List<ChartEntry> entries = [];

            foreach (var chartValue in chartValues)
            {
                entries.Add(
                    new ChartEntry(chartValue.Value)
                    {
                        Label = $"{chartValue.LabelValue}",
                        TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? SKColor.Parse(textDark?.ToHex()) : SKColor.Parse(textLight?.ToHex()),
                        ValueLabel = $"{chartValue.Value}",
                        ValueLabelColor = Application.Current?.UserAppTheme == AppTheme.Dark ? SKColor.Parse(textDark?.ToHex()) : SKColor.Parse(textLight?.ToHex()),
                        Color = SKColor.Parse($"{chartValue?.ColorValue?.ToHex()}"),
                    });
            }

            section.Chart = new PieChart
            {
                Entries = entries,
                LabelMode = LabelMode.RightOnly,
                BackgroundColor = SKColor.Empty,
                LabelTextSize = ConvertMauiFontToSkiaPixels(14f),
                GraphPosition = GraphPosition.AutoFill,
            };
        }

        public void SetUpBarChart(List<ChartValues> chartValues, string sectionName)
        {
            var textLight = (Color?)Application.Current?.Resources["TextLight"];
            var textDark = (Color?)Application.Current?.Resources["TextDark"];

            var section = (ChartView)this.View.FindByName(sectionName);

            List<ChartEntry> entries = [];

            foreach (var chartValue in chartValues)
            {
                entries.Add(
                    new ChartEntry(chartValue.Value)
                    {
                        Label = $"{chartValue.LabelValue}",
                        TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? SKColor.Parse(textDark?.ToHex()) : SKColor.Parse(textLight?.ToHex()),
                        ValueLabel = $"{chartValue.Value}",
                        ValueLabelColor = Application.Current?.UserAppTheme == AppTheme.Dark ? SKColor.Parse(textDark?.ToHex()) : SKColor.Parse(textLight?.ToHex()),
                        Color = SKColor.Parse($"{chartValue?.ColorValue?.ToHex()}"),
                    });
            }

            section.Chart = new BarChart
            {
                Entries = entries,
                BarAreaAlpha = 2,
                LabelOrientation = Orientation.Vertical,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty,
                LabelTextSize = ConvertMauiFontToSkiaPixels(14f),
                ValueLabelTextSize = ConvertMauiFontToSkiaPixels(14f),
                MaxValue = this.TotalBooks,
            };
        }

        public void GetColors()
        {
            var color1 = (Color?)Application.Current?.Resources["Primary"];
            var color2 = (Color?)Application.Current?.Resources["Color2"];
            var color3 = (Color?)Application.Current?.Resources["Color3"];
            var color4 = (Color?)Application.Current?.Resources["Color4"];
            var color5 = (Color?)Application.Current?.Resources["Color5"];
            var color6 = (Color?)Application.Current?.Resources["Color6"];

            this.ColorList = [color1, color2, color3, color4, color5, color6];
        }

        public void GetPreferences()
        {
            ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);
            this.ShowHiddenCollections = Preferences.Get("HiddenCollectionsOn", true /* Default */);
            this.ShowHiddenSeries = Preferences.Get("HiddenSeriesOn", true /* Default */);
            this.ShowHiddenLocations = Preferences.Get("HiddenLocationsOn", true /* Default */);
            this.ShowHiddenGenres = Preferences.Get("HiddenGenresOn", true /* Default */);
            this.ShowHiddenAuthors = Preferences.Get("HiddenAuthorsOn", true /* Default */);
            this.ShowHiddenWishlistBooks = Preferences.Get("HiddenWishlistBooksOn", true /* Default */);
            this.ShowFavorites = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowRatings = Preferences.Get("RatingsOn", true /* Default */);
        }

        public static float ConvertMauiFontToSkiaPixels(float mauiFontSizeDp)
        {
            float density = (float)DeviceDisplay.MainDisplayInfo.Density;
            return mauiFontSizeDp * density;
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

        internal void SetUpSeriesChart(List<CountModel> counts)
        {
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

        internal void SetUpAuthorsChart(List<CountModel> counts)
        {
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

        internal void SetUpLocationsChart(List<CountModel> counts)
        {
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

        internal void SetUpFormatsChart(List<CountModel> counts)
        {
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

        internal void SetUpFormatPricesChart(List<CountModel> counts)
        {
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
