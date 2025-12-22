// <copyright file="LocationsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class LocationsView : ContentPage
{
    public LocationsView()
    {
        this.ViewModel = new LocationsViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private LocationsViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        var variable = this.ViewModel.SetViewModelData();
    }
}