// <copyright file="FakeSplashScreen.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector
{
    using BookCollector.Data;
    using BookCollector.Data.Database;
    using BookCollector.Data.Enums;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Library;
    using BookCollector.ViewModels.Main;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Core.Extensions;
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
            this.BackgroundColor = Color.FromArgb(DevicePreferenceDefaults.AppColorDefault);

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
                Color = Microsoft.Maui.Graphics.Colors.White,
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
            Colors.SetColors(DevicePreferences.AppColorValue);

            Application.Current?.Windows[0].Page = new AppShell();

            await ShowChangeLogPopup();

            // await ShowMajorChangeLogPopup();
        }

        private static void GetAppTheme()
        {
            if (DevicePreferences.AppThemeValue.Equals(DevicePreferenceDefaults.AppThemeDefault))
            {
                DevicePreferences.AppThemeValue =
                    Application.Current!.UserAppTheme == AppTheme.Unspecified ?
                    Application.Current!.PlatformAppTheme.ToString() :
                    Application.Current!.UserAppTheme.ToString();
            }

            Application.Current?.UserAppTheme = DevicePreferences.AppThemeValue switch
            {
                "Light" => AppTheme.Light,
                "Dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified // Follows system
            };
        }

        private static void PreLoadPreferences()
        {
            DevicePreferences.AppThemeValue = Preferences.Get(DevicePreferences.AppTheme, DevicePreferenceDefaults.AppThemeDefault /* Default */);
            DevicePreferences.AppColorValue = Preferences.Get(DevicePreferences.AppColor, DevicePreferenceDefaults.AppColorDefault /* Default */);
            DevicePreferences.AppLanguageValue = Preferences.Get(DevicePreferences.AppLanguage, DevicePreferenceDefaults.AppLanguageDefault /* Default */);
            DevicePreferences.AppCurrencyValue = Preferences.Get(DevicePreferences.AppCurrency, DevicePreferenceDefaults.AppCurrencyDefault /* Default */);
            DevicePreferences.AppExportLocationValue = Preferences.Get(DevicePreferences.AppExportLocation, AppStringResources.DefaultExportLocation /* Default */);
            DevicePreferences.AppCultureCodeValue = Preferences.Get(DevicePreferences.AppCultureCode, DevicePreferenceDefaults.CultureCodeDefault /* Default */);

            DevicePreferences.ShowHiddenCollectionsValue = Preferences.Get(DevicePreferences.HiddenCollectionsShow, DevicePreferenceDefaults.HiddenCollectionShowDefault /* Default */);
            DevicePreferences.ShowHiddenGenresValue = Preferences.Get(DevicePreferences.HiddenGenresShow, DevicePreferenceDefaults.HiddenGenresShowDefault /* Default */);
            DevicePreferences.ShowHiddenSeriesValue = Preferences.Get(DevicePreferences.HiddenSeriesShow, DevicePreferenceDefaults.HiddenSeriesShowDefault /* Default */);
            DevicePreferences.ShowHiddenAuthorsValue = Preferences.Get(DevicePreferences.HiddenAuthorsShow, DevicePreferenceDefaults.HiddenAuthorsShowDefault /* Default */);
            DevicePreferences.ShowHiddenLocationsValue = Preferences.Get(DevicePreferences.HiddenLocationsShow, DevicePreferenceDefaults.HiddenLocationsShowDefault /* Default */);
            DevicePreferences.ShowHiddenBooksValue = Preferences.Get(DevicePreferences.HiddenBooksShow, DevicePreferenceDefaults.HiddenBookShowDefault /* Default */);
            DevicePreferences.ShowHiddenWishlistBooksValue = Preferences.Get(DevicePreferences.HiddenWishlistBooksShow, DevicePreferenceDefaults.HiddenWishlistBooksShowDefault /* Default */);

            DevicePreferences.ShowAudiobooksValue = Preferences.Get(DevicePreferences.AudiobookShow, DevicePreferenceDefaults.AudiobookShowDefault /* Default */);
            DevicePreferences.ShoweBooksValue = Preferences.Get(DevicePreferences.eBookShow, DevicePreferenceDefaults.EbookShowDefault /* Default */);
            DevicePreferences.ShowHardcoversValue = Preferences.Get(DevicePreferences.HardcoverShow, DevicePreferenceDefaults.HardcoverShowDefault /* Default */);
            DevicePreferences.ShowPaperbacksValue = Preferences.Get(DevicePreferences.PaperbackShow, DevicePreferenceDefaults.PaperbackShowDefault /* Default */);

            DevicePreferences.CommentsShowValue = Preferences.Get(DevicePreferences.CommentsFeatureShow, DevicePreferenceDefaults.CommentsShowDefault /* Default */);
            DevicePreferences.ChaptersShowValue = Preferences.Get(DevicePreferences.ChaptersFeatureShow, DevicePreferenceDefaults.ChaptersShowDefault /* Default */);
            DevicePreferences.FavoritesShowValue = Preferences.Get(DevicePreferences.FavoriteFeatureShow, DevicePreferenceDefaults.FavoritesShowDefault /* Default */);
            DevicePreferences.RatingsShowValue = Preferences.Get(DevicePreferences.RatingFeatureShow, DevicePreferenceDefaults.RatingsShowDefault /* Default */);
            DevicePreferences.LoanedOutBooksShowValue = Preferences.Get(DevicePreferences.LoanedOutBooksShow, DevicePreferenceDefaults.LoanedOutBooksShowDefault /* Default */);
            DevicePreferences.BorrowedBooksShowValue = Preferences.Get(DevicePreferences.BorrowedBooksShow, DevicePreferenceDefaults.BorrowedBooksShowDefault /* Default */);

            DevicePreferences.ReadingViewShowValue = Preferences.Get(DevicePreferences.ReadingViewShow, DevicePreferenceDefaults.ReadingViewShowDefault /* Default */);
            DevicePreferences.ToBeReadViewShowValue = Preferences.Get(DevicePreferences.ToBeReadViewShow, DevicePreferenceDefaults.ToBeReadViewShowDefault /* Default */);
            DevicePreferences.ReadViewShowValue = Preferences.Get(DevicePreferences.ReadViewShow, DevicePreferenceDefaults.ReadViewShowDefault /* Default */);
            DevicePreferences.AllBooksViewShowValue = Preferences.Get(DevicePreferences.AllBooksViewShow, DevicePreferenceDefaults.AllBooksViewShowDefault /* Default */);

            DevicePreferences.CollectionsViewShowValue = Preferences.Get(DevicePreferences.CollectionsViewShow, DevicePreferenceDefaults.CollectionsViewShowDefault /* Default */);
            DevicePreferences.GenresViewShowValue = Preferences.Get(DevicePreferences.GenresViewShow, DevicePreferenceDefaults.GenresViewShowDefault /* Default */);
            DevicePreferences.SeriesViewShowValue = Preferences.Get(DevicePreferences.SeriesViewShow, DevicePreferenceDefaults.SeriesViewShowDefault /* Default */);
            DevicePreferences.AuthorsViewShowValue = Preferences.Get(DevicePreferences.AuthorsViewShow, DevicePreferenceDefaults.AuthorsViewShowDefault /* Default */);
            DevicePreferences.LocationsViewShowValue = Preferences.Get(DevicePreferences.LocationsViewShow, DevicePreferenceDefaults.LocationsViewShowDefault /* Default */);

            DevicePreferences.LibraryTabViewsOrderValue = Preferences.Get(
                DevicePreferences.LibraryTabViewsOrder,
                DevicePreferenceDefaults.LibraryTabViewsOrderDefault /* Default */);

            DevicePreferences.GroupingsTabViewOrderValue = Preferences.Get(
                DevicePreferences.GroupingsTabViewsOrder,
                DevicePreferenceDefaults.GroupingsTabViewsOrderDefault /* Default */);
        }

        private static async Task PreLoadData()
        {
            BookBaseViewModel.bookFormats ??= [
                $"{AppStringResources.eBook}",
                $"{AppStringResources.Paperback}",
                $"{AppStringResources.Hardcover}",
                $"{AppStringResources.Audiobook}"
            ];

            BookBaseViewModel.bookFormats = BookBaseViewModel.bookFormats.Order().ToObservableCollection();

            BaseViewModel.Database ??= new BookCollectorDatabase();

            var minimumSeconds = 5;
            var start = DateTime.Now;

            PreLoadPreferences();

            await AllBooksViewModel.SetList(
                DevicePreferences.ShowHiddenBooksValue,
                DevicePreferences.ShowAudiobooksValue,
                DevicePreferences.ShoweBooksValue,
                DevicePreferences.ShowHardcoversValue,
                DevicePreferences.ShowPaperbacksValue);

            List<Task> taskList =
            [
                CollectionsViewModel.SetList(DevicePreferences.ShowHiddenCollectionsValue),
                GenresViewModel.SetList(DevicePreferences.ShowHiddenGenresValue),
                SeriesViewModel.SetList(DevicePreferences.ShowHiddenSeriesValue),
                AuthorsViewModel.SetList(DevicePreferences.ShowHiddenAuthorsValue),
                AuthorsViewModel.SetBookAuthorList(),
                LocationsViewModel.SetList(DevicePreferences.ShowHiddenLocationsValue),

                ToBeReadViewModel.SetList(
                    DevicePreferences.ShowHiddenBooksValue,
                    DevicePreferences.ShowAudiobooksValue,
                    DevicePreferences.ShoweBooksValue,
                    DevicePreferences.ShowHardcoversValue,
                    DevicePreferences.ShowPaperbacksValue),
                ReadViewModel.SetList(
                    DevicePreferences.ShowHiddenBooksValue,
                    DevicePreferences.ShowAudiobooksValue,
                    DevicePreferences.ShoweBooksValue,
                    DevicePreferences.ShowHardcoversValue,
                    DevicePreferences.ShowPaperbacksValue),
                ReadingViewModel.SetList(
                    DevicePreferences.ShowHiddenBooksValue,
                    DevicePreferences.ShowAudiobooksValue,
                    DevicePreferences.ShoweBooksValue,
                    DevicePreferences.ShowHardcoversValue,
                    DevicePreferences.ShowPaperbacksValue),
                WishListViewModel.SetList(
                    DevicePreferences.ShowHiddenWishlistBooksValue,
                    DevicePreferences.ShowAudiobooksValue,
                    DevicePreferences.ShoweBooksValue,
                    DevicePreferences.ShowHardcoversValue,
                    DevicePreferences.ShowPaperbacksValue),
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
                    ShowPopupAsync<string>(new ChoiceDialogPopup(popupWidth, AppStringResources.ChangeLog, AppStringResources.ChangeLogEntry, AppStringResources.OK, null, DialogState.Choice));

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
                    ShowPopupAsync<string>(new ChoiceDialogPopup(popupWidth, AppStringResources.Warning_, AppStringResources.MajorChangeMessage, AppStringResources.OK, null, DialogState.Choice));

                Preferences.Set($"ShowMajorChangeLogDialog{AppInfo.Version.Major}", false);
            }
        }
    }
}
