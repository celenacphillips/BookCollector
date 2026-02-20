// <copyright file="App.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector
{
    using BookCollector.Data.Database;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.Views.Controls;

    /// <summary>
    /// App class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Creates the window for the application. This method is called when the
        /// application starts and can be used to set up the main page or any other
        /// initial UI elements.
        /// </summary>
        /// <param name="activationState">The value of what the window needs to be created.</param>
        /// <returns>The created window.</returns>
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
