using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Popups
{
    public partial class FilterPopupViewModel : BaseViewModel
    {
        public double PopupWidth { get; set; }
        private Popup Popup { get; set; }

        [ObservableProperty]
        public bool favoriteVisible;

        [ObservableProperty]
        public ObservableCollection<string>? favoritePicker;

        [ObservableProperty]
        public string? favoriteOption;


        [ObservableProperty]
        public bool formatVisible;

        [ObservableProperty]
        public ObservableCollection<string>? formatPicker;

        [ObservableProperty]
        public string? formatOption;


        [ObservableProperty]
        public bool authorVisible;

        [ObservableProperty]
        public ObservableCollection<string>? authorPicker;

        [ObservableProperty]
        public string? authorOption;


        [ObservableProperty]
        public bool publisherVisible;

        [ObservableProperty]
        public ObservableCollection<string>? publisherPicker;

        [ObservableProperty]
        public string? publisherOption;


        [ObservableProperty]
        public bool publishYearVisible;

        [ObservableProperty]
        public ObservableCollection<string>? publishYearPicker;

        [ObservableProperty]
        public string? publishYearOption;


        [ObservableProperty]
        public bool languageVisible;

        [ObservableProperty]
        public ObservableCollection<string>? languagePicker;

        [ObservableProperty]
        public string? languageOption;


        [ObservableProperty]
        public bool ratingVisible;

        [ObservableProperty]
        public ObservableCollection<string>? ratingPicker;

        [ObservableProperty]
        public string? ratingOption;


        [ObservableProperty]
        public bool locationVisible;

        [ObservableProperty]
        public ObservableCollection<string>? locationPicker;

        [ObservableProperty]
        public string? locationOption;


        [ObservableProperty]
        public bool seriesVisible;

        [ObservableProperty]
        public ObservableCollection<string>? seriesPicker;

        [ObservableProperty]
        public string? seriesOption;


        public FilterPopupViewModel(Popup popup, string viewTitle)
        {
            Popup = popup;
            ViewTitle = viewTitle;
            PopupWidth = DeviceWidth - 50;
        }

        [RelayCommand]
        public async Task Close()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            SetPreferences();

            await Popup.CloseAsync(token: cts.Token);
        }

        public void SetFavoritePicker()
        {
            FavoritePicker = [AppStringResources.Both, AppStringResources.Favorites, AppStringResources.NonFavorites];
        }

        public void SetFormatPicker(ObservableCollection<string>? formats)
        {
            FormatPicker = formats != null ? [..formats] : null;
            FormatPicker?.Insert(0, AppStringResources.AllFormats);
        }

        public void SetAuthorPicker(ObservableCollection<string>? authorNames)
        {
            AuthorPicker = authorNames != null ? [..authorNames] : null;
            AuthorPicker?.Insert(0, AppStringResources.AllAuthors);
            AuthorPicker?.Insert(1, AppStringResources.NoAuthor);
        }

        public void SetPublisherPicker(ObservableCollection<string>? publisherNames)
        {
            PublisherPicker = publisherNames != null ? [..publisherNames] : null;
            PublisherPicker?.Insert(0, AppStringResources.AllPublishers);
            PublisherPicker?.Insert(1, AppStringResources.NoPublisher);
        }

        public void SetPublishYearPicker(ObservableCollection<string>? publishYears)
        {
            PublishYearPicker = publishYears != null ? [.. publishYears] : null;
            PublishYearPicker?.Insert(0, AppStringResources.AllPublishYears);
            PublishYearPicker?.Insert(1, AppStringResources.NoPublishYear);
        }

        public void SetLanguagePicker(ObservableCollection<string>? languages)
        {
            LanguagePicker = languages != null ? [..languages] : null;
            LanguagePicker?.Insert(0, AppStringResources.AllLanguages);
            LanguagePicker?.Insert(1, AppStringResources.NoLanguage);
        }

        public void SetRatingPicker()
        {
            RatingPicker = [AppStringResources.AllRatings, $"0", $"1", $"2", $"3", $"4", $"5"];
        }

        public void SetLocationPicker(ObservableCollection<string>? locations)
        {
            LocationPicker = locations != null ? [.. locations] : null;
            LocationPicker?.Insert(0, AppStringResources.AllLocations);
            LocationPicker?.Insert(1, AppStringResources.NoLocation);
        }

        public void SetSeriesPicker(ObservableCollection<string>? series)
        {
            SeriesPicker = series != null ? [.. series] : null;
            SeriesPicker?.Insert(0, AppStringResources.AllSeries);
            SeriesPicker?.Insert(1, AppStringResources.NoSeries);
        }

        private void SetPreferences()
        {
            if (FavoriteVisible)
                Preferences.Set($"{ViewTitle}_FavoriteSelection", FavoriteOption);

            if (FormatVisible)
                Preferences.Set($"{ViewTitle}_FormatSelection", FormatOption);

            if (AuthorVisible)
                Preferences.Set($"{ViewTitle}_AuthorSelection", AuthorOption);

            if (PublisherVisible)
                Preferences.Set($"{ViewTitle}_PublisherSelection", PublisherOption);

            if (PublishYearVisible)
                Preferences.Set($"{ViewTitle}_PublishYearSelection", PublishYearOption);

            if (LanguageVisible)
                Preferences.Set($"{ViewTitle}_LanguageSelection", LanguageOption);

            if (RatingVisible)
                Preferences.Set($"{ViewTitle}_RatingSelection", RatingOption);

            if (LocationVisible)
                Preferences.Set($"{ViewTitle}_LocationSelection", LocationOption);

            if (SeriesVisible)
                Preferences.Set($"{ViewTitle}_SeriesSelection", SeriesOption);
        }
    }
}
