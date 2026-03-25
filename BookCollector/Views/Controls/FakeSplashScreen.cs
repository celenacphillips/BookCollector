// <copyright file="FakeSplashScreen.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Controls
{
    using BookCollector.Data.Database;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Library;
    using BookCollector.ViewModels.Main;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Extensions;

    /// <summary>
    /// FakeSplashScreen class.
    /// </summary>
    public class FakeSplashScreen : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeSplashScreen"/> class.
        /// </summary>
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

        /// <summary>
        /// Called when the view becomes visible.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var preload = PreLoadData();

            await Task.WhenAll(preload);

            GetAppTheme();
            GetColor();

            Application.Current?.Windows[0].Page = new AppShell();

            await ShowChangeLogPopup();

            // await ShowMajorChangeLogPopup();
        }

        private static void GetAppTheme()
        {
            var savedTheme = Preferences.Get("AppTheme", "System" /* Default */);

            if (savedTheme.Equals("System"))
            {
                savedTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme.ToString() : Application.Current?.UserAppTheme.ToString();
            }

            Application.Current?.UserAppTheme = savedTheme switch
            {
                "Light" => AppTheme.Light,
                "Dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified // Follows system
            };
        }

        private static void GetColor()
        {
            var savedColorHexCode = Preferences.Get("AppColor", "#336699" /* Default */);

            Data.Colors.SetColors(savedColorHexCode);
        }

        private static async Task PreLoadData()
        {
            BookBaseViewModel.bookFormats ??= [$"{AppStringResources.eBook}", $"{AppStringResources.Paperback}", $"{AppStringResources.Hardcover}", $"{AppStringResources.Audiobook}"];

            BaseViewModel.Database ??= new BookCollectorDatabase();

            var minimumSeconds = 5;
            var start = DateTime.Now;

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

            var end = DateTime.Now;

            var elapsed = end - start;

            if (elapsed.Seconds < minimumSeconds)
            {
                var remainingSeconds = minimumSeconds - elapsed.Seconds;

                await Task.Delay(remainingSeconds * 1000);
            }
        }

        private static async Task ShowChangeLogPopup()
        {
            var show = Preferences.Get($"ShowChangeLogDialog{AppInfo.VersionString}", true /* Default */);

            if (show)
            {
                await Task.Delay(500);

                var popupWidth = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density) - 50;
                Application.Current?.Windows[0]?.Page?.
                    ShowPopupAsync<string>(new ChoiceDialogPopup(popupWidth, AppStringResources.ChangeLog, AppStringResources.ChangeLogEntry, AppStringResources.OK, null, "Commands"));

                Preferences.Set($"ShowChangeLogDialog{AppInfo.VersionString}", false);
            }
        }

        private static async Task ShowMajorChangeLogPopup()
        {
            var show = Preferences.Get($"ShowMajorChangeLogDialog{AppInfo.Version.Major}", true /* Default */);

            if (show)
            {
                await Task.Delay(500);

                var popupWidth = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density) - 50;
                Application.Current?.Windows[0]?.Page?.
                    ShowPopupAsync<string>(new ChoiceDialogPopup(popupWidth, AppStringResources.Warning_, AppStringResources.MajorChangeMessage, AppStringResources.OK, null, "Commands"));

                Preferences.Set($"ShowMajorChangeLogDialog{AppInfo.Version.Major}", false);
            }
        }
    }
}
