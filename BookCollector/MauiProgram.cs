// <copyright file="MauiProgram.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector
{
#if ANDROID
    using BookCollector.CustomPicker;
#endif
    using System.Reflection;
    using BarcodeScanner.Mobile;
    using BookCollector.Data.BookAPI;
    using BookCollector.Data.Database;
    using CommunityToolkit.Maui;
    using Maui.NullableDateTimePicker;
    using Microcharts.Maui;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// MauiProgram class.
    /// </summary>
    public static class MauiProgram
    {
        /// <summary>
        /// Creates the MauiApp.
        /// </summary>
        /// <returns>The created MauiApp.</returns>
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureNullableDateTimePicker()
                .UseMicrocharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialDesignIcons");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    // Add the handlers
                    handlers.AddBarcodeScannerHandler();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<BookCollectorDatabase>();

#if ANDROID
            builder.Services.AddSingleton<IAndroidImagePicker, AndroidImagePicker>();
#endif

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("BookCollector.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream!)
                        .Build();

            builder.Configuration.AddConfiguration(config);

            GoogleBooksAPI.Initialize(builder.Configuration);

            return builder.Build();
        }
    }
}
