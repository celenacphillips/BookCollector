// <copyright file="BookScanView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Book;

using BarcodeScanner.Mobile;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Book;
using CommunityToolkit.Maui.Alerts;

/// <summary>
/// BookScanView class.
/// </summary>
public partial class BookScanView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookScanView"/> class.
    /// </summary>
    public BookScanView()
    {
        this.BindingContext = this;

        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the view model to return the search result to.
    /// </summary>
    public BookSearchViewModel? ReturnViewModel { get; set; }

    /// <summary>
    /// Gets or sets input string to search.
    /// </summary>
    public string? Inputstring { get; set; }

    /// <summary>
    /// Determines the value of the barcode scanner.
    /// </summary>
    /// <param name="sender">The object sender.</param>
    /// <param name="e">The event value.</param>
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

    /// <summary>
    /// Returns the search result to the previous view model.
    /// </summary>
    /// <returns>A task.</returns>
    public async Task ScanSearch()
    {
        if (this.ReturnViewModel != null)
        {
            this.ReturnViewModel.IsbnInput = this.Inputstring;
            await this.ReturnViewModel.Search();
        }
    }
}