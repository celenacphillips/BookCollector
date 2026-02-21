// <copyright file="WishListStatisticsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Statistics;
using Microcharts.Maui;

namespace BookCollector.Views.Statistics;

public partial class WishListStatisticsView : ContentPage
{
    private WishListStatisticsViewModel viewModel;

    public WishListStatisticsView()
    {
        this.viewModel = new WishListStatisticsViewModel(this);
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        this.Dispatcher.Dispatch(() =>
        {
            var items = this.ToolbarItems.ToList();
            this.ToolbarItems.Clear();
            foreach (var item in items)
            {
                this.ToolbarItems.Add(item);
            }
        });

        this.formats.ClearValue(ChartView.ChartProperty);
        this.formatprices.ClearValue(ChartView.ChartProperty);
        this.series.ClearValue(ChartView.ChartProperty);
        this.authors.ClearValue(ChartView.ChartProperty);
        this.locations.ClearValue(ChartView.ChartProperty);

        await this.viewModel.SetViewModelData();
    }
}