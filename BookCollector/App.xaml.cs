// <copyright file="App.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Database;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Main;
using BookCollector.Views.Controls;

namespace BookCollector
{
    /// <summary>
    /// App class.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window;

            BookBaseViewModel.bookFormats ??= [$"{AppStringResources.eBook}", $"{AppStringResources.Paperback}", $"{AppStringResources.Hardcover}", $"{AppStringResources.Audiobook}"];

            BaseViewModel.Database ??= new BookCollectorDatabase();

            App.GetAppTheme();
            App.GetColor();

            window = new Window(new FakeSplashScreen());

            return window;
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

        internal static async Task PreLoadData()
        {
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

        private static bool UseFakeSplashScreen()
        {
#if ANDROID
            var brand = Android.OS.Build.Brand?.ToLowerInvariant() ?? string.Empty;
            var fingerprint = Android.OS.Build.Fingerprint?.ToLowerInvariant() ?? string.Empty;

            if (brand.Contains("graphene") ||
                fingerprint.Contains("graphene") ||
                fingerprint.Contains("panther"))
            {
                return true;
            }
            else
            {
                return false;
            }
#else
        return false;
#endif
        }
    }
}
