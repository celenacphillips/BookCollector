using BarcodeScanner.Mobile;
using BookCollector.Views.Controls;
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

            return builder.Build();
        }
    }
}
