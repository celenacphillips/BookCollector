// <copyright file="LocationEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Location;

using BookCollector.Data.Models;
using BookCollector.ViewModels.Location;

/// <summary>
/// LocationEditView class.
/// </summary>
public partial class LocationEditView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LocationEditView"/> class.
    /// </summary>
    /// <param name="location">Location to add or edit.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    /// <param name="insertMainViewBefore">The value to determine if Main view should be inserted in
    /// stack before this page or not. Default is false.</param>
    public LocationEditView(LocationModel location, string viewTitle, bool insertMainViewBefore = false)
    {
        this.ViewModel = new LocationEditViewModel(location, this)
        {
            ViewTitle = viewTitle,
            InsertMainViewBefore = insertMainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private LocationEditViewModel ViewModel { get; set; }

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        await this.ViewModel.SetViewModelData();
    }
}