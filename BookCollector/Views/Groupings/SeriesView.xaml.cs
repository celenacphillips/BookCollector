// <copyright file="SeriesView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class SeriesView : ContentPage
{
    private SeriesViewModel viewModel;

    public SeriesView()
    {
        this.viewModel = new SeriesViewModel(this);
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        var variable = this.viewModel.SetViewModelData();
    }
}