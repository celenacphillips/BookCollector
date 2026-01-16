// <copyright file="LocationsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class LocationsView : ContentPage
{
    private LocationsViewModel viewModel;

    public LocationsView()
    {
        this.viewModel = new LocationsViewModel(this);
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override async void OnAppearing()
    {
        await this.viewModel.SetViewModelData();
    }
}