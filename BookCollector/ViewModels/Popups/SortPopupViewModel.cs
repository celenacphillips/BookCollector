using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Popups
{
    public partial class SortPopupViewModel : BaseViewModel
    {
        public double PopupWidth { get; set; }
        private Popup Popup { get; set; }

        [ObservableProperty]
        public bool bookTitleVisible;

        [ObservableProperty]
        public bool bookTitleChecked;


        [ObservableProperty]
        public bool collectionNameVisible;

        [ObservableProperty]
        public bool collectionNameChecked;


        [ObservableProperty]
        public bool genreNameVisible;

        [ObservableProperty]
        public bool genreNameChecked;


        [ObservableProperty]
        public bool seriesNameVisible;

        [ObservableProperty]
        public bool seriesNameChecked;


        [ObservableProperty]
        public bool authorLastNameVisible;

        [ObservableProperty]
        public bool authorLastNameChecked;


        [ObservableProperty]
        public bool locationNameVisible;

        [ObservableProperty]
        public bool locationNameChecked;


        [ObservableProperty]
        public bool bookReadingDateVisible;

        [ObservableProperty]
        public bool bookReadingDateChecked;


        [ObservableProperty]
        public bool totalBooksVisible;

        [ObservableProperty]
        public bool totalBooksChecked;

        [ObservableProperty]
        public bool bookReadPercentageVisible;

        [ObservableProperty]
        public bool bookReadPercentageChecked;


        [ObservableProperty]
        public bool bookPublisherVisible;

        [ObservableProperty]
        public bool bookPublisherChecked;


        [ObservableProperty]
        public bool bookPublishYearVisible;

        [ObservableProperty]
        public bool bookPublishYearChecked;


        [ObservableProperty]
        public bool bookFormatVisible;

        [ObservableProperty]
        public bool bookFormatChecked;


        [ObservableProperty]
        public bool pageCountVisible;

        [ObservableProperty]
        public bool pageCountChecked;


        [ObservableProperty]
        public bool totalPriceVisible;

        [ObservableProperty]
        public bool totalPriceChecked;


        [ObservableProperty]
        public bool bookPriceVisible;

        [ObservableProperty]
        public bool bookPriceChecked;

        [ObservableProperty]
        public bool seriesOrderVisible;

        [ObservableProperty]
        public bool seriesOrderChecked;


        [ObservableProperty]
        public bool ascendingChecked;

        [ObservableProperty]
        public bool descendingChecked;

        public SortPopupViewModel(Popup popup, string viewTitle)
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

        private void SetPreferences()
        {
            if (BookTitleVisible)
                Preferences.Set($"{ViewTitle}_BookTitleSelection", BookTitleChecked);

            if (CollectionNameVisible)
                Preferences.Set($"{ViewTitle}_CollectionNameSelection", CollectionNameChecked);

            if (GenreNameVisible)
                Preferences.Set($"{ViewTitle}_GenreNameSelection", GenreNameChecked);

            if (SeriesNameVisible)
                Preferences.Set($"{ViewTitle}_SeriesNameSelection", SeriesNameChecked);

            if (AuthorLastNameVisible)
                Preferences.Set($"{ViewTitle}_AuthorLastNameSelection", AuthorLastNameChecked);

            if (LocationNameVisible)
                Preferences.Set($"{ViewTitle}_LocationNameSelection", LocationNameChecked);

            if (BookReadingDateVisible)
                Preferences.Set($"{ViewTitle}_BookReadingDateSelection", BookReadingDateChecked);

            if (TotalBooksVisible)
                Preferences.Set($"{ViewTitle}_TotalBooksSelection", TotalBooksChecked);

            if (BookReadPercentageVisible)
                Preferences.Set($"{ViewTitle}_BookReadPercentageSelection", BookReadPercentageChecked);

            if (BookPublisherVisible)
                Preferences.Set($"{ViewTitle}_BookPublisherSelection", BookPublisherChecked);

            if (BookPublishYearVisible)
                Preferences.Set($"{ViewTitle}_BookPublishYearSelection", BookPublishYearChecked);

            if (BookFormatVisible)
                Preferences.Set($"{ViewTitle}_BookFormatSelection", BookFormatChecked);

            if (PageCountVisible)
                Preferences.Set($"{ViewTitle}_PageCountSelection", PageCountChecked);

            if (TotalPriceVisible)
                Preferences.Set($"{ViewTitle}_TotalPriceSelection", TotalPriceChecked);

            if (BookPriceVisible)
                Preferences.Set($"{ViewTitle}_BookPriceSelection", BookPriceChecked);

            if (SeriesOrderVisible)
                Preferences.Set($"{ViewTitle}_SeriesOrderSelection", SeriesOrderChecked);

            Preferences.Set($"{ViewTitle}_AscendingSelection", AscendingChecked);
            Preferences.Set($"{ViewTitle}_DescendingSelection", DescendingChecked);
        }
    }
}
