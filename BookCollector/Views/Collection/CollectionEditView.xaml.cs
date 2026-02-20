// <copyright file="CollectionEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Collection;

using BookCollector.Data.Models;
using BookCollector.ViewModels.Collection;

/// <summary>
/// CollectionEditView class.
/// </summary>
public partial class CollectionEditView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionEditView"/> class.
    /// </summary>
    /// <param name="collection">Collection to add or edit.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    /// <param name="insertMainViewBefore">The value to determine if Main view should be inserted in
    /// stack before this page or not. Default is false.</param>
    public CollectionEditView(CollectionModel collection, string viewTitle, bool insertMainViewBefore = false)
    {
        this.ViewModel = new CollectionEditViewModel(collection, this)
        {
            ViewTitle = viewTitle,
            InsertMainViewBefore = insertMainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private CollectionEditViewModel ViewModel { get; set; }

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        await this.ViewModel.SetViewModelData();
    }
}