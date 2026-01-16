using BookCollector.ViewModels.Groupings;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookCollector.Views.Controls
{
    public class FakeSplashScreen : ContentPage
    {
        public FakeSplashScreen()
        {
            this.BackgroundColor = Color.FromArgb("#336699");

            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                },
            };

            var logo = new Image
            {
                Source = "Images/splash_image.svg",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
                WidthRequest = 200,
                HeightRequest = 200,
            };

            var indicator = new ActivityIndicator
            {
                Color = Colors.White,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                IsRunning = true,
                IsVisible = true,
            };

            grid.Add(logo, 0, 0);
            grid.Add(indicator, 0, 1);

            this.Content = grid;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var preload = PreLoadData();

            await Task.WhenAll(preload);

            Application.Current?.Windows[0].Page = new AppShell();
        }

        private static async Task PreLoadData()
        {
            await Task.Delay(1000);

            var showHiddenCollections = Preferences.Get("HiddenCollectionsOn", true /* Default */);
            var showHiddenGenres = Preferences.Get("HiddenGenresOn", true /* Default */);
            var showHiddenSeries = Preferences.Get("HiddenSeriesOn", true /* Default */);
            var showHiddenAuthors = Preferences.Get("HiddenAuthorsOn", true /* Default */);
            var showHiddenLocations = Preferences.Get("HiddenLocationsOn", true /* Default */);
            var showHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);
            var showHiddenWishlistBooks = Preferences.Get("HiddenWishlistBooksOn", true /* Default */);

            await AllBooksViewModel.SetList(showHiddenBooks);

            List<Task> taskList =
            [
                CollectionsViewModel.SetList(showHiddenCollections),
                GenresViewModel.SetList(showHiddenGenres),
                SeriesViewModel.SetList(showHiddenSeries),
                AuthorsViewModel.SetList(showHiddenAuthors),
                LocationsViewModel.SetList(showHiddenLocations),
                ToBeReadViewModel.SetList(showHiddenBooks),
                ReadViewModel.SetList(showHiddenBooks),
                ReadingViewModel.SetList(showHiddenBooks),
                WishListViewModel.SetList(showHiddenWishlistBooks),
            ];

            await Task.WhenAll(taskList);
        }
    }
}
