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
