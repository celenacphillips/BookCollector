// <copyright file="SortPopupViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Popups
{
    using BookCollector.ViewModels.BaseViewModels;
    using CommunityToolkit.Maui.Views;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// SortPopupViewModel class.
    /// </summary>
    public partial class SortPopupViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether book title is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookTitleVisible;

        /// <summary>
        /// Gets or sets a value indicating whether book title is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookTitleChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether collection name is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool collectionNameVisible;

        /// <summary>
        /// Gets or sets a value indicating whether collection name is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool collectionNameChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether genre name is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool genreNameVisible;

        /// <summary>
        /// Gets or sets a value indicating whether genre name is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool genreNameChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether series name is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool seriesNameVisible;

        /// <summary>
        /// Gets or sets a value indicating whether series name is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool seriesNameChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether author last name is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorLastNameVisible;

        /// <summary>
        /// Gets or sets a value indicating whether author last name is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorLastNameChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether location name is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool locationNameVisible;

        /// <summary>
        /// Gets or sets a value indicating whether location name is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool locationNameChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether book reading date is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookReadingDateVisible;

        /// <summary>
        /// Gets or sets a value indicating whether book reading date is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookReadingDateChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether total books is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool totalBooksVisible;

        /// <summary>
        /// Gets or sets a value indicating whether total books is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool totalBooksChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether book read percentage is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookReadPercentageVisible;

        /// <summary>
        /// Gets or sets a value indicating whether book read percentage is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookReadPercentageChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether book publisher is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookPublisherVisible;

        /// <summary>
        /// Gets or sets a value indicating whether book publisher is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookPublisherChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether book publish year is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookPublishYearVisible;

        /// <summary>
        /// Gets or sets a value indicating whether book publish year is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookPublishYearChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether book format is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookFormatVisible;

        /// <summary>
        /// Gets or sets a value indicating whether book format is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookFormatChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether page count/time is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool pageCountTimeVisible;

        /// <summary>
        /// Gets or sets a value indicating whether page count/time is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool pageCountTimeChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether total price is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool totalPriceVisible;

        /// <summary>
        /// Gets or sets a value indicating whether total price is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool totalPriceChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether book price is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookPriceVisible;

        /// <summary>
        /// Gets or sets a value indicating whether book price is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookPriceChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether series order is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool seriesOrderVisible;

        /// <summary>
        /// Gets or sets a value indicating whether series order is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool seriesOrderChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether ascending is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool ascendingChecked;

        /// <summary>
        /// Gets or sets a value indicating whether descending is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool descendingChecked;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortPopupViewModel"/> class.
        /// </summary>
        /// <param name="popup">Popup related to the view model.</param>
        /// <param name="viewTitle">Title of the popup.</param>
        public SortPopupViewModel(Popup popup, string viewTitle)
        {
            this.Popup = popup;
            this.ViewTitle = viewTitle;
            this.PopupWidth = DeviceWidth - 50;
        }

        private Popup Popup { get; set; }

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

        private void SetPreferences()
        {
            if (this.BookTitleVisible)
            {
                Preferences.Set($"{this.ViewTitle}_BookTitleSelection", this.BookTitleChecked);
            }

            if (this.CollectionNameVisible)
            {
                Preferences.Set($"{this.ViewTitle}_CollectionNameSelection", this.CollectionNameChecked);
            }

            if (this.GenreNameVisible)
            {
                Preferences.Set($"{this.ViewTitle}_GenreNameSelection", this.GenreNameChecked);
            }

            if (this.SeriesNameVisible)
            {
                Preferences.Set($"{this.ViewTitle}_SeriesNameSelection", this.SeriesNameChecked);
            }

            if (this.AuthorLastNameVisible)
            {
                Preferences.Set($"{this.ViewTitle}_AuthorLastNameSelection", this.AuthorLastNameChecked);
            }

            if (this.LocationNameVisible)
            {
                Preferences.Set($"{this.ViewTitle}_LocationNameSelection", this.LocationNameChecked);
            }

            if (this.BookReadingDateVisible)
            {
                Preferences.Set($"{this.ViewTitle}_BookReadingDateSelection", this.BookReadingDateChecked);
            }

            if (this.TotalBooksVisible)
            {
                Preferences.Set($"{this.ViewTitle}_TotalBooksSelection", this.TotalBooksChecked);
            }

            if (this.BookReadPercentageVisible)
            {
                Preferences.Set($"{this.ViewTitle}_BookReadPercentageSelection", this.BookReadPercentageChecked);
            }

            if (this.BookPublisherVisible)
            {
                Preferences.Set($"{this.ViewTitle}_BookPublisherSelection", this.BookPublisherChecked);
            }

            if (this.BookPublishYearVisible)
            {
                Preferences.Set($"{this.ViewTitle}_BookPublishYearSelection", this.BookPublishYearChecked);
            }

            if (this.BookFormatVisible)
            {
                Preferences.Set($"{this.ViewTitle}_BookFormatSelection", this.BookFormatChecked);
            }

            if (this.PageCountTimeVisible)
            {
                Preferences.Set($"{this.ViewTitle}_PageCountBookTimeSelection", this.PageCountTimeChecked);
            }

            if (this.TotalPriceVisible)
            {
                Preferences.Set($"{this.ViewTitle}_TotalPriceSelection", this.TotalPriceChecked);
            }

            if (this.BookPriceVisible)
            {
                Preferences.Set($"{this.ViewTitle}_BookPriceSelection", this.BookPriceChecked);
            }

            if (this.SeriesOrderVisible)
            {
                Preferences.Set($"{this.ViewTitle}_SeriesOrderSelection", this.SeriesOrderChecked);
            }

            Preferences.Set($"{this.ViewTitle}_AscendingSelection", this.AscendingChecked);
            Preferences.Set($"{this.ViewTitle}_DescendingSelection", this.DescendingChecked);
        }
    }
}
