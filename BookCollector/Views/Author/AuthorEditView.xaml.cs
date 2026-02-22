// <copyright file="AuthorEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Author;

using BookCollector.Data.Models;
using BookCollector.ViewModels.Author;

/// <summary>
/// AuthorEditView class.
/// </summary>
public partial class AuthorEditView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorEditView"/> class.
    /// </summary>
    /// <param name="author">Author to add or edit.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    /// <param name="insertMainViewBefore">The value to determine if Main view should be inserted in
    /// stack before this page or not. Default is false.</param>
    public AuthorEditView(AuthorModel author, string viewTitle, bool insertMainViewBefore = false)
    {
        this.ViewModel = new AuthorEditViewModel(author, this)
        {
            ViewTitle = viewTitle,
            InsertMainViewBefore = insertMainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private AuthorEditViewModel ViewModel { get; set; }

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        this.Dispatcher.Dispatch(() =>
        {
            var items = this.ToolbarItems.ToList();
            this.ToolbarItems.Clear();
            foreach (var item in items)
            {
                this.ToolbarItems.Add(item);
            }
        });

        this.ViewModel.SetViewModelData();
    }
}