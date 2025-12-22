// <copyright file="SeriesView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class SeriesView : ContentPage
{
    public SeriesView()
    {
        this.ViewModel = new SeriesViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private SeriesViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        var variable = this.ViewModel.SetViewModelData();
    }
}