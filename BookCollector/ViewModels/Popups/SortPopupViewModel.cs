// <copyright file="SortPopupViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Popups
{
    using BookCollector.ViewModels.BaseViewModels;
    using CommunityToolkit.Maui.Views;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class SortPopupViewModel : BaseViewModel
    {
        [ObservableProperty]
        public bool bookTitleVisible;

        [ObservableProperty]
        public bool bookTitleChecked;

        /********************************************************/

        [ObservableProperty]
        public bool collectionNameVisible;

        [ObservableProperty]
        public bool collectionNameChecked;

        /********************************************************/

        [ObservableProperty]
        public bool genreNameVisible;

        [ObservableProperty]
        public bool genreNameChecked;

        /********************************************************/

        [ObservableProperty]
        public bool seriesNameVisible;

        [ObservableProperty]
        public bool seriesNameChecked;

        /********************************************************/

        [ObservableProperty]
        public bool authorLastNameVisible;

        [ObservableProperty]
        public bool authorLastNameChecked;

        /********************************************************/

        [ObservableProperty]
        public bool locationNameVisible;

        [ObservableProperty]
        public bool locationNameChecked;

        /********************************************************/

        [ObservableProperty]
        public bool bookReadingDateVisible;

        [ObservableProperty]
        public bool bookReadingDateChecked;

        /********************************************************/

        [ObservableProperty]
        public bool totalBooksVisible;

        [ObservableProperty]
        public bool totalBooksChecked;

        /********************************************************/

        [ObservableProperty]
        public bool bookReadPercentageVisible;

        [ObservableProperty]
        public bool bookReadPercentageChecked;

        /********************************************************/

        [ObservableProperty]
        public bool bookPublisherVisible;

        [ObservableProperty]
        public bool bookPublisherChecked;

        /********************************************************/

        [ObservableProperty]
        public bool bookPublishYearVisible;

        [ObservableProperty]
        public bool bookPublishYearChecked;

        /********************************************************/

        [ObservableProperty]
        public bool bookFormatVisible;

        [ObservableProperty]
        public bool bookFormatChecked;

        /********************************************************/

        [ObservableProperty]
        public bool pageCountTimeVisible;

        [ObservableProperty]
        public bool pageCountTimeChecked;

        /********************************************************/

        [ObservableProperty]
        public bool totalPriceVisible;

        [ObservableProperty]
        public bool totalPriceChecked;

        /********************************************************/

        [ObservableProperty]
        public bool bookPriceVisible;

        [ObservableProperty]
        public bool bookPriceChecked;

        /********************************************************/

        [ObservableProperty]
        public bool seriesOrderVisible;

        [ObservableProperty]
        public bool seriesOrderChecked;

        /********************************************************/

        [ObservableProperty]
        public bool ascendingChecked;

        [ObservableProperty]
        public bool descendingChecked;

        public SortPopupViewModel(Popup popup, string viewTitle)
        {
            this.Popup = popup;
            this.ViewTitle = viewTitle;
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
