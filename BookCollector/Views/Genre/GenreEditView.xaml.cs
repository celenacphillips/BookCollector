// <copyright file="GenreEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.Genre;

namespace BookCollector.Views.Genre;

public partial class GenreEditView : ContentPage
{
    public GenreEditView(GenreModel genre, string viewTitle, bool insertMainViewBefore = false)
    {
        this.ViewModel = new GenreEditViewModel(genre, this)
        {
            ViewTitle = viewTitle,
            InsertMainViewBefore = insertMainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private GenreEditViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.ViewModel.SetViewModelData();
    }
}