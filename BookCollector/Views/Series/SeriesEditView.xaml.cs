// <copyright file="SeriesEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Series;

using BookCollector.Data.Models;
using BookCollector.ViewModels.Series;

/// <summary>
/// SeriesEditView class.
/// </summary>
public partial class SeriesEditView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SeriesEditView"/> class.
    /// </summary>
    /// <param name="series">Series to add or edit.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    /// <param name="insertMainViewBefore">The value to determine if Main view should be inserted in
    /// stack before this page or not. Default is false.</param>
    public SeriesEditView(SeriesModel series, string viewTitle, bool insertMainViewBefore = false)
    {
        this.ViewModel = new SeriesEditViewModel(series, this)
        {
            ViewTitle = viewTitle,
            InsertMainViewBefore = insertMainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private SeriesEditViewModel ViewModel { get; set; }

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        await this.ViewModel.SetViewModelData();
    }
}