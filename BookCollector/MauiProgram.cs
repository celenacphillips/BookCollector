// <copyright file="MauiProgram.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BarcodeScanner.Mobile;
using BookCollector.Data.Database;
using CommunityToolkit.Maui;
using Maui.NullableDateTimePicker;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;

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

            return builder.Build();
        }
    }
}
