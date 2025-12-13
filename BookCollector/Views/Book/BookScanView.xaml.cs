using BarcodeScanner.Mobile;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Book;
using CommunityToolkit.Maui.Alerts;

namespace BookCollector.Views.Book;

public partial class BookScanView : ContentPage
{
    public BookScanView()
    {
        this.BindingContext = this;

        this.InitializeComponent();
    }

    public BookSearchViewModel? ReturnViewModel { get; set; }

    public string? Inputstring { get; set; }

    public async void CameraView_OnDetected(object sender, OnDetectedEventArg e)
    {
        await Shell.Current.DisplaySnackbar(AppStringResources.BarcodeScanned);

        var result = e.BarcodeResults.Where(x => x.BarcodeType == BarcodeTypes.Isbn);

        if (result != null && result.Count() == 1)
        {
            await Shell.Current.Navigation.PopModalAsync();
            this.Inputstring = result.FirstOrDefault()?.RawValue;
            await this.ScanSearch();
        }
        else
        {
            await Shell.Current.DisplaySnackbar(AppStringResources.PleaseScanABookBarcode);

            this.barcodeScannerView.IsScanning = true;
        }
    }

    public async Task ScanSearch()
    {
        if (this.ReturnViewModel != null)
        {
            this.ReturnViewModel.Input = this.Inputstring;
            await this.ReturnViewModel.Search();
        }
    }
}