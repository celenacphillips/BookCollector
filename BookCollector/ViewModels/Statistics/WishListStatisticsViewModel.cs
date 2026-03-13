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

    /// <summary>
    /// WishListStatisticsViewModel class.
    /// </summary>
    public partial class WishListStatisticsViewModel : StatisticsBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WishListStatisticsViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public WishListStatisticsViewModel(ContentPage view)
        {
            this.View = view;
            this.MaxListNumber = 5;
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
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
    }
}
