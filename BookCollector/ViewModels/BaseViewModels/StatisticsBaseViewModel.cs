using BookCollector.Resources.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Color> ColorList { get; set; }

        public bool ShowFavorites { get; set; }
        public bool ShowRatings { get; set; }
        public bool ShowHiddenBooks { get; set; }
        public bool ShowHiddenCollections { get; set; }
        public bool ShowHiddenSeries { get; set; }
        public bool ShowHiddenLocations { get; set; }
        public bool ShowHiddenGenres { get; set; }
        public bool ShowHiddenAuthors { get; set; }

        internal void SetUpPieChart(List<ChartValues> chartValues, string sectionName)
        {
            var textLight = (Color)Application.Current.Resources["TextLight"];
            var textDark = (Color)Application.Current.Resources["TextDark"];

            var section = (ChartView)_view.FindByName(sectionName);

            section.Chart = null;

            List<ChartEntry> entries = new();

            foreach (var chartValue in chartValues)
            {
                entries.Add(
                    new ChartEntry(chartValue.Value)
                    {
                        Label = $"{chartValue.LabelValue}",
                        TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? SKColor.Parse(textDark.ToHex()) : SKColor.Parse(textLight.ToHex()),
                        ValueLabel = $"{chartValue.Value}",
                        ValueLabelColor = Application.Current.UserAppTheme == AppTheme.Dark ? SKColor.Parse(textDark.ToHex()) : SKColor.Parse(textLight.ToHex()),
                        Color = SKColor.Parse($"{chartValue.ColorValue.ToHex()}"),
                    });
            }

            section.Chart = new PieChart
            {
                Entries = entries,
                LabelMode = LabelMode.RightOnly,
                BackgroundColor = SKColor.Empty,
                LabelTextSize = 30,
                GraphPosition = GraphPosition.AutoFill,
            };
        }

        internal void SetUpBarChart(List<ChartValues> chartValues, string sectionName)
        {
            var textLight = (Color)Application.Current.Resources["TextLight"];
            var textDark = (Color)Application.Current.Resources["TextDark"];

            var section = (ChartView)_view.FindByName(sectionName);

            section.Chart = null;

            List<ChartEntry> entries = new();

            foreach (var chartValue in chartValues)
            {
                entries.Add(
                    new ChartEntry(chartValue.Value)
                    {
                        Label = $"{chartValue.LabelValue}",
                        TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? SKColor.Parse(textDark.ToHex()) : SKColor.Parse(textLight.ToHex()),
                        ValueLabel = $"{chartValue.Value}",
                        ValueLabelColor = Application.Current.UserAppTheme == AppTheme.Dark ? SKColor.Parse(textDark.ToHex()) : SKColor.Parse(textLight.ToHex()),
                        Color = SKColor.Parse($"{chartValue.ColorValue.ToHex()}"),
                    });
            }

            section.Chart = new BarChart
            {
                Entries = entries,
                BarAreaAlpha = 0,
                LabelOrientation = Orientation.Vertical,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty,
                LabelTextSize = 30,
                ValueLabelTextSize = 30,
                MaxValue = TotalBooks
            };
        }

        internal void GetColors()
        {
            var color1 = (Color)Application.Current.Resources["Primary"];
            var color2 = (Color)Application.Current.Resources["Color2"];
            var color3 = (Color)Application.Current.Resources["Color3"];
            var color4 = (Color)Application.Current.Resources["Color4"];
            var color5 = (Color)Application.Current.Resources["Color5"];
            var color6 = (Color)Application.Current.Resources["Color6"];

            ColorList = [color1, color2, color3, color4, color5, color6];
        }

        internal void GetPreferences()
        {
            ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true  /* Default */);
            ShowHiddenCollections = Preferences.Get("HiddenCollectionsOn", true  /* Default */);
            ShowHiddenSeries = Preferences.Get("HiddenSeriesOn", true  /* Default */);
            ShowHiddenLocations = Preferences.Get("HiddenLocationsOn", true  /* Default */);
            ShowHiddenGenres = Preferences.Get("HiddenGenresOn", true  /* Default */);
            ShowHiddenAuthors = Preferences.Get("HiddenAuthorsOn", true  /* Default */);
            ShowFavorites = Preferences.Get("FavoritesOn", true  /* Default */);
            ShowRatings = Preferences.Get("RatingsOn", true  /* Default */);
        }
    }

    internal class ChartValues
    {
        internal Color ColorValue { get; set; }

        internal string LabelValue { get; set; }

        internal float Value { get; set; }
    }

    public class CountValues
    {
        internal int Count { get; set; }

        internal string Label { get; set; }
    }
}
