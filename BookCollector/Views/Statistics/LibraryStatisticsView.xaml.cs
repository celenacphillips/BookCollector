// <copyright file="LibraryStatisticsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Statistics;
using Microcharts.Maui;

namespace BookCollector.Views.Statistics;

public partial class LibraryStatisticsView : ContentPage
{
    private LibraryStatisticsViewModel viewModel;

    public LibraryStatisticsView()
    {
        this.viewModel = new LibraryStatisticsViewModel(this);
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

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