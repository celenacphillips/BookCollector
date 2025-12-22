// <copyright file="CollectionsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class CollectionsView : ContentPage
{
    public CollectionsView()
    {
        this.ViewModel = new CollectionsViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private CollectionsViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        var variable = this.ViewModel.SetViewModelData();
    }
}