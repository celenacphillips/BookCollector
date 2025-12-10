using BookCollector.Data;
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
        public string? totalBooksstring;

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

        public int MaxListNumber { get; set; }

        public void SetUpPieChart(List<ChartValues> chartValues, string sectionName)
        {
            var textLight = (Color?)Application.Current?.Resources["TextLight"];
            var textDark = (Color?)Application.Current?.Resources["TextDark"];

            var section = (ChartView)this.View.FindByName(sectionName);

            section.Chart = null;

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
                LabelTextSize = 30,
                GraphPosition = GraphPosition.AutoFill,
            };
        }

        public void SetUpBarChart(List<ChartValues> chartValues, string sectionName)
        {
            var textLight = (Color?)Application.Current?.Resources["TextLight"];
            var textDark = (Color?)Application.Current?.Resources["TextDark"];

            var section = (ChartView)this.View.FindByName(sectionName);

            section.Chart = null;

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
                BarAreaAlpha = 0,
                LabelOrientation = Orientation.Vertical,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Empty,
                LabelTextSize = 30,
                ValueLabelTextSize = 30,
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
            this.ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);
            this.ShowHiddenCollections = Preferences.Get("HiddenCollectionsOn", true /* Default */);
            this.ShowHiddenSeries = Preferences.Get("HiddenSeriesOn", true /* Default */);
            this.ShowHiddenLocations = Preferences.Get("HiddenLocationsOn", true /* Default */);
            this.ShowHiddenGenres = Preferences.Get("HiddenGenresOn", true /* Default */);
            this.ShowHiddenAuthors = Preferences.Get("HiddenAuthorsOn", true /* Default */);
            this.ShowFavorites = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowRatings = Preferences.Get("RatingsOn", true /* Default */);
        }
    }
}
