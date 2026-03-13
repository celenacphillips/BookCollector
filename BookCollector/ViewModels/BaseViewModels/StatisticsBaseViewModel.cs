// <copyright file="StatisticsBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using BookCollector.Data;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microcharts;
    using Microcharts.Maui;
    using SkiaSharp;

    /// <summary>
    /// StatisticsBaseViewModel class.
    /// </summary>
    public partial class StatisticsBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the string for the total cost of books.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? costBooks;

        /// <summary>
        /// Gets or sets the string for the total count of books.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int totalBooks;

        /// <summary>
        /// Get or sets the total count of books as a string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalBooksString;

        /// <summary>
        /// Gets or sets a value indicating whether to show the series chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showSeries;

        /// <summary>
        /// Gets or sets a value indicating whether to show the authors chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showAuthors;

        /// <summary>
        /// Gets or sets a value indicating whether to show the locations chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showLocations;

        /// <summary>
        /// Gets or sets a value indicating whether to show the formats chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showFormats;

        /// <summary>
        /// Gets or sets a value indicating whether to show the format prices chart or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showFormatPrices;

        /// <summary>
        /// Gets or sets the string for the top X series, where X is a number defined by the max limit or the max number of series.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? topXSeries;

        /// <summary>
        /// Gets or sets the string for the top X authors, where X is a number defined by the max limit or the max number of authors.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? topXAuthors;

        /// <summary>
        /// Gets or sets the string for the top X locations, where X is a number defined by the max limit or the max number of locations.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? topXLocations;

        /// <summary>
        /// Gets or sets the list of colors to use in the charts.
        /// </summary>
        public List<Color?>? ColorList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the favorites in the charts or not.
        /// </summary>
        public bool ShowFavorites { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the ratings in the charts or not.
        /// </summary>
        public bool ShowRatings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the hidden books in the charts or not.
        /// </summary>
        public bool ShowHiddenBooks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the hidden collections in the charts or not.
        /// </summary>
        public bool ShowHiddenCollections { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the hidden series in the charts or not.
        /// </summary>
        public bool ShowHiddenSeries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the hidden locations in the charts or not.
        /// </summary>
        public bool ShowHiddenLocations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the hidden genres in the charts or not.
        /// </summary>
        public bool ShowHiddenGenres { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the hidden authors in the charts or not.
        /// </summary>
        public bool ShowHiddenAuthors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the hidden wishlist books in the charts or not.
        /// </summary>
        public bool ShowHiddenWishlistBooks { get; set; }

        /// <summary>
        /// Gets or sets the max number of items to show in the lists.
        /// </summary>
        public int MaxListNumber { get; set; }

        /// <summary>
        /// Converts the font size from Maui units (dp) to SkiaSharp units (pixels) based on the device density.
        /// </summary>
        /// <param name="mauiFontSizeDp">Font size.</param>
        /// <returns>Pixel size.</returns>
        public static float ConvertMauiFontToSkiaPixels(float mauiFontSizeDp)
        {
            float density = (float)DeviceDisplay.MainDisplayInfo.Density;
            return mauiFontSizeDp * density;
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
        }

        /// <summary>
        /// Creates the entries for the charts and sets up the pie chart with the given values and section name.
        /// </summary>
        /// <param name="chartValues">Chart values to display.</param>
        /// <param name="sectionName">Section name to create the chart for.</param>
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

        /// <summary>
        /// Creates the entries for the charts and sets up the bar chart with the given values and section name.
        /// </summary>
        /// <param name="chartValues">Chart values to display.</param>
        /// <param name="sectionName">Section name to create the chart for.</param>
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

        /// <summary>
        /// Get the colors from the resources and sets up the color list to use in the charts.
        /// </summary>
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

        /// <summary>
        /// Get the user preferences from device settings.
        /// </summary>
        public void GetPreferences()
        {
            this.ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);
            this.ShowHiddenCollections = Preferences.Get("HiddenCollectionsOn", true /* Default */);
            this.ShowHiddenSeries = Preferences.Get("HiddenSeriesOn", true /* Default */);
            this.ShowHiddenLocations = Preferences.Get("HiddenLocationsOn", true /* Default */);
            this.ShowHiddenGenres = Preferences.Get("HiddenGenresOn", true /* Default */);
            this.ShowHiddenAuthors = Preferences.Get("HiddenAuthorsOn", true /* Default */);
            this.ShowHiddenWishlistBooks = Preferences.Get("HiddenWishlistBooksOn", true /* Default */);
            this.ShowFavorites = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowRatings = Preferences.Get("RatingsOn", true /* Default */);
        }

        /*********************** Series Methods ***********************/

        /// <summary>
        /// Set show series chart or not based on the count values.
        /// </summary>
        /// <param name="counts">Chart values.</param>
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

        /// <summary>
        /// Create series chart.
        /// </summary>
        /// <param name="counts">Chart values.</param>
        internal void SetUpSeriesChart(List<CountModel> counts)
        {
            this.SetShowSeries(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            counts = [.. counts.Where(x => x.Count > 0)];

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

        /// <summary>
        /// Set show author chart or not based on the count values.
        /// </summary>
        /// <param name="counts">Chart values.</param>
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

        /// <summary>
        /// Create authors chart.
        /// </summary>
        /// <param name="counts">Chart values.</param>
        internal void SetUpAuthorsChart(List<CountModel> counts)
        {
            this.SetShowAuthors(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            counts = [.. counts.Where(x => x.Count > 0)];

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

        /// <summary>
        /// Set show location chart or not based on the count values.
        /// </summary>
        /// <param name="counts">Chart values.</param>
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

        /// <summary>
        /// Create locations chart.
        /// </summary>
        /// <param name="counts">Chart values.</param>
        internal void SetUpLocationsChart(List<CountModel> counts)
        {
            this.SetShowLocations(counts);

            counts = [.. counts.OrderByDescending(x => x.Count)];

            counts = [.. counts.Where(x => x.Count > 0)];

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

        /// <summary>
        /// Set show format chart or not based on the count values.
        /// </summary>
        /// <param name="counts">Chart values.</param>
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

        /// <summary>
        /// Create formats chart.
        /// </summary>
        /// <param name="counts">Chart values.</param>
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

        /// <summary>
        /// Set show format prices chart or not based on the count values.
        /// </summary>
        /// <param name="counts">Chart values.</param>
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

        /// <summary>
        /// Create format prices chart.
        /// </summary>
        /// <param name="counts">Chart values.</param>
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
