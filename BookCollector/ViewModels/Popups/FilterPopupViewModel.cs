// <copyright file="FilterPopupViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Popups
{
    using System.Collections.ObjectModel;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.Views.Controls;
    using CommunityToolkit.Maui.Views;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// FilterPopupViewModel class.
    /// </summary>
    public partial class FilterPopupViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show favorites or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool favoriteVisible;

        /// <summary>
        /// Gets or sets the favorites list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? favoritePicker;

        /// <summary>
        /// Gets or sets the favorites option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? favoriteOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show formats or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool formatVisible;

        /// <summary>
        /// Gets or sets the formats list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? formatPicker;

        /// <summary>
        /// Gets or sets the formats option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? formatOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show authors or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorVisible;

        /// <summary>
        /// Gets or sets the authors list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? authorPicker;

        /// <summary>
        /// Gets or sets the authors option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? authorOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show publishers or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool publisherVisible;

        /// <summary>
        /// Gets or sets the publishers list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? publisherPicker;

        /// <summary>
        /// Gets or sets the publishers option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? publisherOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show publish years or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool publishYearVisible;

        /// <summary>
        /// Gets or sets the publish years list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? publishYearPicker;

        /// <summary>
        /// Gets or sets the publish years option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? publishYearOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show languages or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool languageVisible;

        /// <summary>
        /// Gets or sets the languages list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? languagePicker;

        /// <summary>
        /// Gets or sets the languages option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? languageOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show ratings or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool ratingVisible;

        /// <summary>
        /// Gets or sets the ratings list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? ratingPicker;

        /// <summary>
        /// Gets or sets the ratings option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? ratingOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show locations or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool locationVisible;

        /// <summary>
        /// Gets or sets the locations list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? locationPicker;

        /// <summary>
        /// Gets or sets the locations option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? locationOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show series or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool seriesVisible;

        /// <summary>
        /// Gets or sets the series list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? seriesPicker;

        /// <summary>
        /// Gets or sets the series option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? seriesOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show book covers or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookCoverVisible;

        /// <summary>
        /// Gets or sets the book covers list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? bookCoverPicker;

        /// <summary>
        /// Gets or sets the book covers option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? bookCoverOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show reading status or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool readingStatusVisible;

        /// <summary>
        /// Gets or sets the reading status list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? readingStatusPicker;

        /// <summary>
        /// Gets or sets the reading status option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? readingStatusOption;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to show loaned out books or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool loanedOutBooksVisible;

        /// <summary>
        /// Gets or sets the loaned out books list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public List<string>? loanedOutBooksPicker;

        /// <summary>
        /// Gets or sets the loaned out books option.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? loanedOutBooksOption;

        /********************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterPopupViewModel"/> class.
        /// </summary>
        /// <param name="popup">Popup related to the view model.</param>
        /// <param name="viewTitle">Title of the popup.</param>
        /// <param name="view">View calling the popup.</param>
        public FilterPopupViewModel(Popup popup, string viewTitle, ContentPage view)
        {
            this.Popup = popup;
            this.ViewTitle = viewTitle;
            this.View = view;
            this.PopupWidth = DeviceWidth - 30;
            this.PopupHeight = DeviceHeight - 300;

            this.OverlaySection = (Grid)this.Popup.FindByName("overlaySection");
        }

        /********************************************************/

        /// <summary>
        /// Gets or sets the popup overlay.
        /// </summary>
        public Grid OverlaySection { get; set; }

        /********************************************************/

        /// <summary>
        /// Gets or sets the default saved book author filter option.
        /// </summary>
        internal string BookAuthorOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default favorite books option.
        /// </summary>
        internal string FavoriteOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default book format option.
        /// </summary>
        internal string BookFormatOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default book publisher option.
        /// </summary>
        internal string BookPublisherOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default book publisher year range option.
        /// </summary>
        internal string BookPublishYearOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default book language option.
        /// </summary>
        internal string BookLanguageOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default book rating option.
        /// </summary>
        internal string BookRatingOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default book cover option.
        /// </summary>
        internal string BookCoverOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default saved book location filter option.
        /// </summary>
        internal string BookLocationOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default saved book series filter option.
        /// </summary>
        internal string BookSeriesOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default reading status option.
        /// </summary>
        internal string ReadingStatusOptionDefault { get; set; }

        /// <summary>
        /// Gets or sets the default loaned out books option.
        /// </summary>
        internal string LoanedOutBooksOptionDefault { get; set; }

        /********************************************************/

        /// <summary>
        /// Gets or sets the popup.
        /// </summary>
        private Popup Popup { get; set; }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
        }

        /// <summary>
        /// Set whether to refresh view or not.
        /// </summary>
        /// <param name="value">Value to change to.</param>
        public override void SetRefreshView(bool value)
        {
        }

        /********************************************************/

        /// <summary>
        /// Set the selected values as preferences and close popup.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Close()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            this.SetPreferences();

            await this.Popup.CloseAsync(token: cts.Token);
        }

        /// <summary>
        /// Set the selected values as preferences and close popup.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Reset()
        {
            this.ResetDefaults();
        }

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
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

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task BookCoverChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.BookCover,
                this.BookCoverPicker,
                this.BookCoverOption,
                false,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ReadingStatusChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.ReadingStatus,
                this.ReadingStatusPicker,
                this.ReadingStatusOption,
                false,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        /// <summary>
        /// Displays filter overlay with picker list and selected option.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task LoanedOutBooksChanged()
        {
            var filterablePickerOverlay = new FilterablePickerOverlay(
                this,
                AppStringResources.BooksLoanedOut,
                this.LoanedOutBooksPicker,
                this.LoanedOutBooksOption,
                false,
                true);

            this.OverlaySection.Add(filterablePickerOverlay);
        }

        /********************************************************/

        /// <summary>
        /// Set defaults for each picker.
        /// </summary>
        /// <param name="favoriteOptionDefault">Favorite option default.</param>
        /// <param name="bookFormatOptionDefault">Book format option default.</param>
        /// <param name="bookAuthorOptionDefault">Book author option default.</param>
        /// <param name="bookPublisherOptionDefault">Book publisher option default.</param>
        /// <param name="bookPublisherYearOptionDefault">Book publish year option default.</param>
        /// <param name="bookLanguageOptionDefault">Book language option default.</param>
        /// <param name="bookRatingOptionDefault">Book rating option default.</param>
        /// <param name="bookLocationOptionDefault">Book location option default.</param>
        /// <param name="bookSeriesOptionDefault">Book series option default.</param>
        /// <param name="bookCoverOptionDefault">Book cover option default.</param>
        /// <param name="readingStatusOptionDefault">Reading status option default.</param>
        /// <param name="loanedOutBooksOptionDefault">Loaned out books option default.</param>
        public void SetDefaults(
            string? favoriteOptionDefault,
            string? bookFormatOptionDefault,
            string? bookAuthorOptionDefault,
            string? bookPublisherOptionDefault,
            string? bookPublisherYearOptionDefault,
            string? bookLanguageOptionDefault,
            string? bookRatingOptionDefault,
            string? bookLocationOptionDefault,
            string? bookSeriesOptionDefault,
            string? bookCoverOptionDefault,
            string? readingStatusOptionDefault,
            string? loanedOutBooksOptionDefault)
        {
            if (!string.IsNullOrEmpty(favoriteOptionDefault))
            {
                this.FavoriteOptionDefault = favoriteOptionDefault;
            }

            if (!string.IsNullOrEmpty(bookFormatOptionDefault))
            {
                this.BookFormatOptionDefault = bookFormatOptionDefault;
            }

            if (!string.IsNullOrEmpty(bookAuthorOptionDefault))
            {
                this.BookAuthorOptionDefault = bookAuthorOptionDefault;
            }

            if (!string.IsNullOrEmpty(bookPublisherOptionDefault))
            {
                this.BookPublisherOptionDefault = bookPublisherOptionDefault;
            }

            if (!string.IsNullOrEmpty(bookPublisherYearOptionDefault))
            {
                this.BookPublishYearOptionDefault = bookPublisherYearOptionDefault;
            }

            if (!string.IsNullOrEmpty(bookLanguageOptionDefault))
            {
                this.BookLanguageOptionDefault = bookLanguageOptionDefault;
            }

            if (!string.IsNullOrEmpty(bookRatingOptionDefault))
            {
                this.BookRatingOptionDefault = bookRatingOptionDefault;
            }

            if (!string.IsNullOrEmpty(bookLocationOptionDefault))
            {
                this.BookLocationOptionDefault = bookLocationOptionDefault;
            }

            if (!string.IsNullOrEmpty(bookSeriesOptionDefault))
            {
                this.BookSeriesOptionDefault = bookSeriesOptionDefault;
            }

            if (!string.IsNullOrEmpty(bookCoverOptionDefault))
            {
                this.BookCoverOptionDefault = bookCoverOptionDefault;
            }

            if (!string.IsNullOrEmpty(readingStatusOptionDefault))
            {
                this.ReadingStatusOptionDefault = readingStatusOptionDefault;
            }

            if (!string.IsNullOrEmpty(loanedOutBooksOptionDefault))
            {
                this.LoanedOutBooksOptionDefault = loanedOutBooksOptionDefault;
            }
        }

        /// <summary>
        /// Set values of favorite picker.
        /// </summary>
        public void SetFavoritePicker()
        {
            this.FavoritePicker =
            [
                AppStringResources.Both,
                AppStringResources.Favorites,
                AppStringResources.NonFavorites,
            ];
        }

        /// <summary>
        /// Set values of format picker.
        /// </summary>
        /// <param name="formats">Formats list for the picker.</param>
        /// <param name="showAudiobooks">Show audiobooks.</param>
        /// <param name="showEbooks">Show ebooks.</param>
        /// <param name="showHardcovers">Show hardcovers.</param>
        /// <param name="showPaperbacks">Show paperbacks.</param>
        public void SetFormatPicker(
            ObservableCollection<string>? formats,
            bool showAudiobooks,
            bool showEbooks,
            bool showHardcovers,
            bool showPaperbacks)
        {
            this.FormatPicker = formats != null ? [.. formats] : null;

            if (!showAudiobooks)
            {
                this.FormatPicker?.Remove(AppStringResources.Audiobook);
            }

            if (!showEbooks)
            {
                this.FormatPicker?.Remove(AppStringResources.eBook);
            }

            if (!showHardcovers)
            {
                this.FormatPicker?.Remove(AppStringResources.Hardcover);
            }

            if (!showPaperbacks)
            {
                this.FormatPicker?.Remove(AppStringResources.Paperback);
            }

            if (this.FormatPicker == null || this.FormatPicker.Count == 0)
            {
                this.FormatOption = AppStringResources.None;
            }
            else if (this.FormatPicker.Count == 1)
            {
                this.FormatOption = this.FormatPicker[0];
            }
            else
            {
                this.FormatPicker?.Insert(0, AppStringResources.AllFormats);
            }
        }

        /// <summary>
        /// Set values of author picker.
        /// </summary>
        /// <param name="authorNames">Author names list for the picker.</param>
        public void SetAuthorPicker(ObservableCollection<string>? authorNames)
        {
            this.AuthorPicker = authorNames != null ? [.. authorNames] : null;
            this.AuthorPicker?.Insert(0, AppStringResources.AllAuthors);
            this.AuthorPicker?.Insert(1, AppStringResources.NoAuthor);
        }

        /// <summary>
        /// Set values of publisher picker.
        /// </summary>
        /// <param name="publisherNames">Publisher names list for the picker.</param>
        public void SetPublisherPicker(ObservableCollection<string>? publisherNames)
        {
            this.PublisherPicker = publisherNames != null ? [.. publisherNames] : null;
            this.PublisherPicker?.Insert(0, AppStringResources.AllPublishers);
            this.PublisherPicker?.Insert(1, AppStringResources.NoPublisher);
        }

        /// <summary>
        /// Set values of publish year picker.
        /// </summary>
        /// <param name="publishYears">Publish year ranges list for the picker.</param>
        public void SetPublishYearPicker(ObservableCollection<string>? publishYears)
        {
            this.PublishYearPicker = publishYears != null ? [.. publishYears] : null;
            this.PublishYearPicker?.Insert(0, AppStringResources.AllPublishYears);
            this.PublishYearPicker?.Insert(1, AppStringResources.NoPublishYear);
        }

        /// <summary>
        /// Set values of language picker.
        /// </summary>
        /// <param name="languages">Languages list for the picker.</param>
        public void SetLanguagePicker(ObservableCollection<string>? languages)
        {
            this.LanguagePicker = languages != null ? [.. languages] : null;
            this.LanguagePicker?.Insert(0, AppStringResources.AllLanguages);
            this.LanguagePicker?.Insert(1, AppStringResources.NoLanguage);
        }

        /// <summary>
        /// Set values of rating picker.
        /// </summary>
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

        /// <summary>
        /// Set values of location picker.
        /// </summary>
        /// <param name="locations">Locations list for the picker.</param>
        public void SetLocationPicker(ObservableCollection<string>? locations)
        {
            this.LocationPicker = locations != null ? [.. locations] : null;
            this.LocationPicker?.Insert(0, AppStringResources.AllLocations);
            this.LocationPicker?.Insert(1, AppStringResources.NoLocation);
        }

        /// <summary>
        /// Set values of series picker.
        /// </summary>
        /// <param name="series">Series list for the picker.</param>
        public void SetSeriesPicker(ObservableCollection<string>? series)
        {
            this.SeriesPicker = series != null ? [.. series] : null;
            this.SeriesPicker?.Insert(0, AppStringResources.AllSeries);
            this.SeriesPicker?.Insert(1, AppStringResources.NoSeries);
        }

        /// <summary>
        /// Set values of book cover picker.
        /// </summary>
        public void SetBookCoverPicker()
        {
            this.BookCoverPicker =
            [
                AppStringResources.Both,
                AppStringResources.HasABookCover,
                AppStringResources.HasNoBookCover,
            ];
        }

        /// <summary>
        /// Set values of reading status picker.
        /// </summary>
        public void SetReadingStatusPicker()
        {
            this.ReadingStatusPicker =
            [
                AppStringResources.AllReadingStatuses,
                AppStringResources.ToBeRead,
                AppStringResources.Reading,
                AppStringResources.Read,
            ];
        }

        /// <summary>
        /// Set values of loaned out books picker.
        /// </summary>
        public void SetLoanedOutBooksPicker()
        {
            this.LoanedOutBooksPicker =
            [
                AppStringResources.AllBooks,
                AppStringResources.LoanedOut,
                AppStringResources.NotLoanedOut,
            ];
        }

        /********************************************************/

        private void ResetDefaults()
        {
            this.FavoriteOption = this.FavoriteOptionDefault;
            this.FormatOption = this.BookFormatOptionDefault;
            this.AuthorOption = this.BookAuthorOptionDefault;
            this.PublisherOption = this.BookPublisherOptionDefault;
            this.PublishYearOption = this.BookPublishYearOptionDefault;
            this.LanguageOption = this.BookLanguageOptionDefault;
            this.RatingOption = this.BookRatingOptionDefault;
            this.LocationOption = this.BookLocationOptionDefault;
            this.SeriesOption = this.BookSeriesOptionDefault;
            this.BookCoverOption = this.BookCoverOptionDefault;
            this.ReadingStatusOption = this.ReadingStatusOptionDefault;
            this.LoanedOutBooksOption = this.LoanedOutBooksOptionDefault;
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

            if (this.BookCoverVisible)
            {
                Preferences.Set($"{this.ViewTitle}_BookCoverSelection", this.BookCoverOption);
            }

            if (this.ReadingStatusVisible)
            {
                Preferences.Set($"{this.ViewTitle}_ReadingStatusSelection", this.ReadingStatusOption);
            }

            if (this.LoanedOutBooksVisible)
            {
                Preferences.Set($"{this.ViewTitle}_LoanedOutBooksSelection", this.LoanedOutBooksOption);
            }
        }
    }
}
