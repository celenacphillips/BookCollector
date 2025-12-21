// <copyright file="GenreMainView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.Genre;

namespace BookCollector.Views.Genre;

public partial class GenreMainView : ContentPage
{
    private GenreMainViewModel viewModel;

    public GenreMainView(GenreModel genre, string viewTitle)
    {
        this.viewModel = new GenreMainViewModel(genre, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.viewModel.SetViewModelData();
    }
}