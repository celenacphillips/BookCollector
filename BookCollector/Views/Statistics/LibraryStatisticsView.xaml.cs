// <copyright file="LibraryStatisticsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Statistics;

using BookCollector.ViewModels.Statistics;
using Microcharts.Maui;

/// <summary>
/// LibraryStatisticsView class.
/// </summary>
public partial class LibraryStatisticsView : ContentPage
{
    private LibraryStatisticsViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="LibraryStatisticsView"/> class.
    /// </summary>
    public LibraryStatisticsView()
    {
        this.viewModel = new LibraryStatisticsViewModel(this);
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        this.readingStatus.ClearValue(ChartView.ChartProperty);
        this.favorites.ClearValue(ChartView.ChartProperty);
        this.ratings.ClearValue(ChartView.ChartProperty);
        this.formats.ClearValue(ChartView.ChartProperty);
        this.formatprices.ClearValue(ChartView.ChartProperty);
        this.collections.ClearValue(ChartView.ChartProperty);
        this.genres.ClearValue(ChartView.ChartProperty);
        this.series.ClearValue(ChartView.ChartProperty);
        this.authors.ClearValue(ChartView.ChartProperty);
        this.locations.ClearValue(ChartView.ChartProperty);

        await this.viewModel.SetViewModelData();
    }
}