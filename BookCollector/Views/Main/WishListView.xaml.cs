// <copyright file="WishListView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Main;

namespace BookCollector.Views.Main;

public partial class WishListView : ContentPage
{
    public WishListView()
    {
        this.ViewModel = new WishListViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private WishListViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        var variable = this.ViewModel.SetViewModelData();
    }
}