// <copyright file="FilterPopupViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Controls;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Popups
{
    public partial class FilterPopupViewModel : BaseViewModel
    {
        [ObservableProperty]
        public bool favoriteVisible;

        [ObservableProperty]
        public List<string>? favoritePicker;

        [ObservableProperty]
        public string? favoriteOption;

        /********************************************************/

        [ObservableProperty]
        public bool formatVisible;

        [ObservableProperty]
        public List<string>? formatPicker;

        [ObservableProperty]
        public string? formatOption;

        /********************************************************/

        [ObservableProperty]
        public bool authorVisible;

        [ObservableProperty]
        public List<string>? authorPicker;

        [ObservableProperty]
        public string? authorOption;

        /********************************************************/

        [ObservableProperty]
        public bool publisherVisible;

        [ObservableProperty]
        public List<string>? publisherPicker;

        [ObservableProperty]
        public string? publisherOption;

        /********************************************************/

        [ObservableProperty]
        public bool publishYearVisible;

        [ObservableProperty]
        public List<string>? publishYearPicker;

        [ObservableProperty]
        public string? publishYearOption;

        /********************************************************/

        [ObservableProperty]
        public bool languageVisible;

        [ObservableProperty]
        public List<string>? languagePicker;

        [ObservableProperty]
        public string? languageOption;

        /********************************************************/

        [ObservableProperty]
        public bool ratingVisible;

        [ObservableProperty]
        public List<string>? ratingPicker;

        [ObservableProperty]
        public string? ratingOption;

        /********************************************************/

        [ObservableProperty]
        public bool locationVisible;

        [ObservableProperty]
        public List<string>? locationPicker;

        [ObservableProperty]
        public string? locationOption;

        /********************************************************/

        [ObservableProperty]
        public bool seriesVisible;

        [ObservableProperty]
        public List<string>? seriesPicker;

        [ObservableProperty]
        public string? seriesOption;

        /********************************************************/

        public FilterPopupViewModel(Popup popup, string viewTitle, ContentPage view)
        {
            this.Popup = popup;
            this.ViewTitle = viewTitle;
            this.View = view;
            this.PopupWidth = this.DeviceWidth - 30;

            this.OverlaySection = (Grid)this.Popup.FindByName("overlaySection");
        }

        public double PopupWidth { get; set; }

        private Popup Popup { get; set; }

        public Grid OverlaySection { get; set; }

        [RelayCommand]
        public async Task Close()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            this.SetPreferences();

            await this.Popup.CloseAsync(token: cts.Token);
        }

        public void SetFavoritePicker()
        {
            this.FavoritePicker =
            [
                AppStringResources.Both,
                AppStringResources.Favorites,
                AppStringResources.NonFavorites,
            ];
        }

        public void SetFormatPicker(ObservableCollection<string>? formats)
        {
            this.FormatPicker = formats != null ? [..formats] : null;
            this.FormatPicker?.Insert(0, AppStringResources.AllFormats);
        }

        public void SetAuthorPicker(ObservableCollection<string>? authorNames)
        {
            this.AuthorPicker = authorNames != null ? [..authorNames] : null;
            this.AuthorPicker?.Insert(0, AppStringResources.AllAuthors);
            this.AuthorPicker?.Insert(1, AppStringResources.NoAuthor);
        }

        public void SetPublisherPicker(ObservableCollection<string>? publisherNames)
        {
            this.PublisherPicker = publisherNames != null ? [..publisherNames] : null;
            this.PublisherPicker?.Insert(0, AppStringResources.AllPublishers);
            this.PublisherPicker?.Insert(1, AppStringResources.NoPublisher);
        }

        public void SetPublishYearPicker(ObservableCollection<string>? publishYears)
        {
            this.PublishYearPicker = publishYears != null ? [.. publishYears] : null;
            this.PublishYearPicker?.Insert(0, AppStringResources.AllPublishYears);
            this.PublishYearPicker?.Insert(1, AppStringResources.NoPublishYear);
        }

        public void SetLanguagePicker(ObservableCollection<string>? languages)
        {
            this.LanguagePicker = languages != null ? [..languages] : null;
            this.LanguagePicker?.Insert(0, AppStringResources.AllLanguages);
            this.LanguagePicker?.Insert(1, AppStringResources.NoLanguage);
        }

        public void SetRatingPicker()
        {
            this.RatingPicker =
            [
                AppStringResources.AllRatings,
                AppStringResources.ZeroStars,
                AppStringResources.OneStar,
                AppStringResources.TwoStars,
                AppStringResources.ThreeStars,
                AppStringResources.FourStars,
                AppStringResources.FiveStars,
            ];
        }

        public void SetLocationPicker(ObservableCollection<string>? locations)
        {
            this.LocationPicker = locations != null ? [.. locations] : null;
            this.LocationPicker?.Insert(0, AppStringResources.AllLocations);
            this.LocationPicker?.Insert(1, AppStringResources.NoLocation);
        }

        public void SetSeriesPicker(ObservableCollection<string>? series)
        {
            this.SeriesPicker = series != null ? [.. series] : null;
            this.SeriesPicker?.Insert(0, AppStringResources.AllSeries);
            this.SeriesPicker?.Insert(1, AppStringResources.NoSeries);
        }

        [RelayCommand]
        public async Task FavoriteChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.Favorite,
                this.FavoritePicker,
                this.FavoriteOption,
                false,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        [RelayCommand]
        public async Task AuthorChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.Authors,
                this.AuthorPicker,
                this.AuthorOption,
                true,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        [RelayCommand]
        public async Task PublisherChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.BookPublisher,
                this.PublisherPicker,
                this.PublisherOption,
                true,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        [RelayCommand]
        public async Task PublishYearChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.BookPublishYear,
                this.PublishYearPicker,
                this.PublishYearOption,
                true,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        [RelayCommand]
        public async Task FormatChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.BookFormat,
                this.FormatPicker,
                this.FormatOption,
                false,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        [RelayCommand]
        public async Task LanguageChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.BookLanguage,
                this.LanguagePicker,
                this.LanguageOption,
                true,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        [RelayCommand]
        public async Task RatingChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.BookRating,
                this.RatingPicker,
                this.RatingOption,
                false,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        [RelayCommand]
        public async Task LocationChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.BookLocation,
                this.LocationPicker,
                this.LocationOption,
                true,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        [RelayCommand]
        public async Task SeriesChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.BookSeries,
                this.SeriesPicker,
                this.SeriesOption,
                true,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        private void SetPreferences()
        {
            if (this.FavoriteVisible)
            {
                Preferences.Set($"{this.ViewTitle}_FavoriteSelection", this.FavoriteOption);
            }

            if (this.FormatVisible)
            {
                Preferences.Set($"{this.ViewTitle}_FormatSelection", this.FormatOption);
            }

            if (this.AuthorVisible)
            {
                Preferences.Set($"{this.ViewTitle}_AuthorSelection", this.AuthorOption);
            }

            if (this.PublisherVisible)
            {
                Preferences.Set($"{this.ViewTitle}_PublisherSelection", this.PublisherOption);
            }

            if (this.PublishYearVisible)
            {
                Preferences.Set($"{this.ViewTitle}_PublishYearSelection", this.PublishYearOption);
            }

            if (this.LanguageVisible)
            {
                Preferences.Set($"{this.ViewTitle}_LanguageSelection", this.LanguageOption);
            }

            if (this.RatingVisible)
            {
                Preferences.Set($"{this.ViewTitle}_RatingSelection", this.RatingOption);
            }

            if (this.LocationVisible)
            {
                Preferences.Set($"{this.ViewTitle}_LocationSelection", this.LocationOption);
            }

            if (this.SeriesVisible)
            {
                Preferences.Set($"{this.ViewTitle}_SeriesSelection", this.SeriesOption);
            }
        }
    }
}
