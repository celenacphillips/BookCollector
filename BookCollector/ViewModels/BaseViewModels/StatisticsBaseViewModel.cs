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

        internal void SetUpPieChart(List<ChartValues> chartValues, string sectionName)
        {
            var section = (ChartView)_view.FindByName(sectionName);

            section.Chart = null;

            List<ChartEntry> entries = new();

            foreach (var chartValue in chartValues)
            {
                entries.Add(
                    new ChartEntry(chartValue.Value)
                    {
                        Label = $"{chartValue.LabelValue}",
                        TextColor = SKColor.Parse($"{chartValue.ColorValue.ToHex()}"),
                        ValueLabel = $"{chartValue.Value}",
                        ValueLabelColor = SKColor.Parse($"{chartValue.ColorValue.ToHex()}"),
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
            var section = (ChartView)_view.FindByName(sectionName);

            section.Chart = null;

            List<ChartEntry> entries = new();

            foreach (var chartValue in chartValues)
            {
                entries.Add(
                    new ChartEntry(chartValue.Value)
                    {
                        Label = $"{chartValue.LabelValue}",
                        TextColor = SKColor.Parse($"{chartValue.ColorValue.ToHex()}"),
                        ValueLabel = $"{chartValue.Value}",
                        ValueLabelColor = SKColor.Parse($"{chartValue.ColorValue.ToHex()}"),
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

        // TO DO
        // fix color calculations - 12/3/2025
        internal void CalculateColors(Color primaryColor, AppTheme appTheme, int splits)
        {
            float value = (float)1.0 / splits;

            for (int i = 0; i < splits; i++)
            {
                float addValue = (float)value * (i + 1);
                if (appTheme == AppTheme.Light)
                {
                    ColorList.Add(primaryColor.WithHue(addValue));
                }
                else
                {
                    ColorList.Add(primaryColor.WithHue(addValue));
                }
            }
        }

        internal void GetPreferences()
        {
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
