// <copyright file="ReadingDataView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Statistics;

namespace BookCollector.Views.Statistics;

public partial class ReadingDataView : ContentPage
{
    private ReadingDataViewModel viewModel;

    public ReadingDataView()
    {
        this.viewModel = new ReadingDataViewModel(this);
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    protected override void OnAppearing()
    {
        var variable = this.viewModel.SetViewModelData();
    }
}