// <copyright file="WishListStatisticsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Statistics;

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

    protected override void OnAppearing()
    {
        using var variable = this.viewModel.SetViewModelData();
    }
}