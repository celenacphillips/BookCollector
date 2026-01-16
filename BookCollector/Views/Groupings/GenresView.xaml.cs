// <copyright file="GenresView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class GenresView : ContentPage
{
    private GenresViewModel viewModel;

    public GenresView()
    {
        this.viewModel = new GenresViewModel(this);
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