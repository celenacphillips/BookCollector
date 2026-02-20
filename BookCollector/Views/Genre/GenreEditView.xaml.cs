// <copyright file="GenreEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Genre;

using BookCollector.Data.Models;
using BookCollector.ViewModels.Genre;

/// <summary>
/// GenreEditView class.
/// </summary>
public partial class GenreEditView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenreEditView"/> class.
    /// </summary>
    /// <param name="genre">Genre to add or edit.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    /// <param name="insertMainViewBefore">The value to determine if Main view should be inserted in
    /// stack before this page or not. Default is false.</param>
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

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        await this.ViewModel.SetViewModelData();
    }
}