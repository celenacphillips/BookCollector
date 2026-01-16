// <copyright file="AuthorsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class AuthorsView : ContentPage
{
    private AuthorsViewModel viewModel;

    public AuthorsView()
    {
        this.viewModel = new AuthorsViewModel(this);
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