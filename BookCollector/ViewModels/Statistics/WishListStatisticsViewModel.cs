// <copyright file="WishListStatisticsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Statistics
{
    using System.Globalization;
    using BookCollector.Data;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Main;
    using CommunityToolkit.Mvvm.Input;

    public partial class WishListStatisticsViewModel : StatisticsBaseViewModel
    {
        public WishListStatisticsViewModel(ContentPage view)
        {
            this.View = view;
            this.MaxListNumber = 5;
        }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                if (string.IsNullOrEmpty(this.CostBooks))
                {
                    var cultureCode = Preferences.Get("CultureCode", "en-US" /* Default */);
                    var cultureInfo = new CultureInfo(cultureCode);
                    this.CostBooks = string.Format(cultureInfo, "{0:C}", 0);
                }

                this.GetPreferences();

                if (WishListViewModel.filteredWishlistBookList1 == null || WishListViewModel.RefreshView)
                {
                    await WishListViewModel.SetList(this.ShowHiddenWishlistBooks);
                }

                var cost = GetCounts.GetPriceOfAllWishListBooks();
                var series = GetCounts.GetAllWishListBooksAndSeriesList(this.MaxListNumber);
                var authors = GetCounts.GetAllWishListBooksAndAuthorList(this.MaxListNumber);
                var locations = GetCounts.GetAllWishListBooksAndLocationList(this.MaxListNumber);
                var formats = GetCounts.GetAllWishListBooksAndBookFormatsList();
                var formatPrices = GetCounts.GetPriceOfWishListBooksAndBookFormatsList();

                this.GetColors();

                await Task.WhenAll(
                    cost,
                    series,
                    authors,
                    locations,
                    formats,
                    formatPrices);

                this.CostBooks = cost.Result;
                this.TotalBooks = WishListViewModel.filteredWishlistBookList1!.Count;
                var seriesCounts = series.Result;
                var authorsCounts = authors.Result;
                var locationsCounts = locations.Result;
                var formatsCounts = formats.Result;
                var formatPricesCounts = formatPrices.Result;

                this.SetUpSeriesChart(seriesCounts);
                this.SetUpAuthorsChart(authorsCounts);
                this.SetUpLocationsChart(locationsCounts);
                this.SetUpFormatsChart(formatsCounts);
                this.SetUpFormatPricesChart(formatPricesCounts);

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
#if DEBUG
                await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }
    }
}
