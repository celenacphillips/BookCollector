using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Statistics
{
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

                this.GetPreferences();

                this.GetColors();

                List<CountModel> seriesCounts = [];
                List<CountModel> authorCounts = [];
                List<CountModel> locationCounts = [];
                List<CountModel> formatCounts = [];
                List<CountModel> formatPriceCounts = [];

                Task.WaitAll(
                [
                    Task.Run(async () => this.CostBooks = await FilterLists.GetPriceOfAllWishListBooks(this.ShowHiddenWishlistBooks)),
                    Task.Run(async () => this.TotalBooks = await FilterLists.GetAllWishListBooksListCount(this.ShowHiddenWishlistBooks)),
                    Task.Run(async () => seriesCounts = await FilterLists.GetAllWishListBooksAndSeriesList(this.ShowHiddenWishlistBooks, this.MaxListNumber)),
                    Task.Run(async () => authorCounts = await FilterLists.GetAllWishListBooksAndAuthorList(this.ShowHiddenWishlistBooks, this.MaxListNumber)),
                    Task.Run(async () => locationCounts = await FilterLists.GetAllWishListBooksAndLocationList(this.ShowHiddenWishlistBooks, this.MaxListNumber)),
                    Task.Run(() => formatCounts = FilterLists.GetAllWishListBooksAndBookFormatsList(this.ShowHiddenWishlistBooks)),
                    Task.Run(() => formatPriceCounts = FilterLists.GetPriceOfWishListBooksAndBookFormatsList(this.ShowHiddenWishlistBooks)),
                ]);

                this.SetUpSeriesChart(seriesCounts);
                this.SetUpAuthorsChart(authorCounts);
                this.SetUpLocationsChart(locationCounts);
                this.SetUpFormatsChart(formatCounts);
                this.SetUpFormatPricesChart(formatPriceCounts);

                this.SetIsBusyFalse();
            }
            catch (Exception)
            {
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
