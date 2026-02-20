// <copyright file="WishListStatisticsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Statistics;

using BookCollector.ViewModels.Statistics;
using Microcharts.Maui;

/// <summary>
/// WishListStatisticsView class.
/// </summary>
public partial class WishListStatisticsView : ContentPage
{
    private WishListStatisticsViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="WishListStatisticsView"/> class.
    /// </summary>
    public WishListStatisticsView()
    {
        this.viewModel = new WishListStatisticsViewModel(this);
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        this.formats.ClearValue(ChartView.ChartProperty);
        this.formatprices.ClearValue(ChartView.ChartProperty);
        this.series.ClearValue(ChartView.ChartProperty);
        this.authors.ClearValue(ChartView.ChartProperty);
        this.locations.ClearValue(ChartView.ChartProperty);

        await this.viewModel.SetViewModelData();
    }
}