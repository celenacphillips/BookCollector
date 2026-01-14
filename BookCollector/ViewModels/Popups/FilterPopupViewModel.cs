// <copyright file="FilterPopupViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
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
        public ObservableCollection<string>? favoritePicker;

        [ObservableProperty]
        public string? favoriteOption;

        /********************************************************/

        [ObservableProperty]
        public bool formatVisible;

        [ObservableProperty]
        public ObservableCollection<string>? formatPicker;

        [ObservableProperty]
        public string? formatOption;

        /********************************************************/

        [ObservableProperty]
        public bool authorVisible;

        [ObservableProperty]
        public ObservableCollection<string>? authorPicker;

        [ObservableProperty]
        public string? authorOption;

        /********************************************************/

        [ObservableProperty]
        public bool publisherVisible;

        [ObservableProperty]
        public ObservableCollection<string>? publisherPicker;

        [ObservableProperty]
        public string? publisherOption;

        /********************************************************/

        [ObservableProperty]
        public bool publishYearVisible;

        [ObservableProperty]
        public ObservableCollection<string>? publishYearPicker;

        [ObservableProperty]
        public string? publishYearOption;

        /********************************************************/

        [ObservableProperty]
        public bool languageVisible;

        [ObservableProperty]
        public ObservableCollection<string>? languagePicker;

        [ObservableProperty]
        public string? languageOption;

        /********************************************************/

        [ObservableProperty]
        public bool ratingVisible;

        [ObservableProperty]
        public ObservableCollection<string>? ratingPicker;

        [ObservableProperty]
        public string? ratingOption;

        /********************************************************/

        [ObservableProperty]
        public bool locationVisible;

        [ObservableProperty]
        public ObservableCollection<string>? locationPicker;

        [ObservableProperty]
        public string? locationOption;

        /********************************************************/

        [ObservableProperty]
        public bool seriesVisible;

        [ObservableProperty]
        public ObservableCollection<string>? seriesPicker;

        [ObservableProperty]
        public string? seriesOption;

        public FilterPopupViewModel(Popup popup, string viewTitle, ContentPage view)
        {
            this.Popup = popup;
            this.ViewTitle = viewTitle;
            this.View = view;
            this.PopupWidth = this.DeviceWidth - 50;
        }

        public double PopupWidth { get; set; }

        private Popup Popup { get; set; }

        [RelayCommand]
        public async Task Close()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            this.SetPreferences();

            await this.Popup.CloseAsync(token: cts.Token);
        }

        public void SetFavoritePicker()
        {
            this.FavoritePicker = [AppStringResources.Both, AppStringResources.Favorites, AppStringResources.NonFavorites];
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
            this.RatingPicker = [AppStringResources.AllRatings, $"0", $"1", $"2", $"3", $"4", $"5"];
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
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.Favorite,
                    this.FavoritePicker.ToList(),
                    this.FavoriteOption,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.FavoriteOption = result.Result;
                }

                this.Popup.CanBeDismissedByTappingOutsideOfPopup = false;
                this.Popup.CanBeDismissedByTappingOutsideOfPopup = true;
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task AuthorChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.Authors,
                    this.AuthorPicker.ToList(),
                    this.AuthorOption,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.AuthorOption = result.Result;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task PublisherChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.BookPublisher,
                    this.PublisherPicker.ToList(),
                    this.PublisherOption,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.PublisherOption = result.Result;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task PublishYearChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.BookPublishYear,
                    this.PublishYearPicker.ToList(),
                    this.PublishYearOption,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.PublishYearOption = result.Result;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task FormatChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.BookFormat,
                    this.FormatPicker.ToList(),
                    this.FormatOption,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.FormatOption = result.Result;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task LanguageChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.BookLanguage,
                    this.LanguagePicker.ToList(),
                    this.LanguageOption,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.LanguageOption = result.Result;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task RatingChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.BookRating,
                    this.RatingPicker.ToList(),
                    this.RatingOption,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.RatingOption = result.Result;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task LocationChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.BookLocation,
                    this.LocationPicker.ToList(),
                    this.LocationOption,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.LocationOption = result.Result;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task SeriesChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.BookSeries,
                    this.SeriesPicker.ToList(),
                    this.SeriesOption,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.SeriesOption = result.Result;
                }
            }
            catch (Exception ex)
            {
            }
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
