// <copyright file="MauiProgram.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BarcodeScanner.Mobile;
using BookCollector.Data.BookAPI;
using BookCollector.Data.Database;
using CommunityToolkit.Maui;
using Maui.NullableDateTimePicker;
using Microcharts.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace BookCollector
{
    public static class MauiProgram
    {
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

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("BookCollector.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Configuration.AddConfiguration(config);

            GoogleBooksAPI.Initialize(builder.Configuration);

            return builder.Build();
        }
    }
}
