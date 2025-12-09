using BarcodeScanner.Mobile;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Book;
using CommunityToolkit.Maui.Alerts;

namespace BookCollector.Views.Book;

public partial class BookScanView : ContentPage
{
    public BookSearchViewModel? ReturnViewModel { get; set; }

    public string? InputString { get; set; }

    public BookScanView()
    {
        BindingContext = this;

        InitializeComponent();
    }

    public async void CameraView_OnDetected(object sender, OnDetectedEventArg e)
    {
        await Shell.Current.DisplaySnackbar(AppStringResources.BarcodeScanned);

        var result = e.BarcodeResults.Where(x => x.BarcodeType == BarcodeTypes.Isbn);

        if (result != null && result.Count() == 1)
        {
            await Shell.Current.Navigation.PopModalAsync();
            InputString = result.FirstOrDefault()?.RawValue;
            await ScanSearch();
        }
        else
        {
            await Shell.Current.DisplaySnackbar(AppStringResources.PleaseScanABookBarcode);

            barcodeScannerView.IsScanning = true;
        }
    }

    public async Task ScanSearch()
    {
        if (ReturnViewModel != null)
        {
            ReturnViewModel.Input = InputString;
            await ReturnViewModel.Search();
        }
    }
}