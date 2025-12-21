// <copyright file="LibraryStatisticsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Statistics;

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

    protected override void OnAppearing()
    {
        using var variable = this.viewModel.SetViewModelData();
    }
}