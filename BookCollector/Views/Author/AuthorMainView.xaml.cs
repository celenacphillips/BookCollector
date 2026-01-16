// <copyright file="AuthorMainView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.Author;

namespace BookCollector.Views.Author;

public partial class AuthorMainView : ContentPage
{
    private AuthorMainViewModel viewModel;

    public AuthorMainView(AuthorModel author, string viewTitle)
    {
        this.viewModel = new AuthorMainViewModel(author, this)
        {
            ViewTitle = viewTitle,
        };
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