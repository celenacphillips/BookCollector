// <copyright file="GenresView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class GenresView : ContentPage
{
    public GenresView()
    {
        this.ViewModel = new GenresViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private GenresViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.ViewModel.SetViewModelData();
    }
}