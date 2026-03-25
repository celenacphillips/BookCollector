// <copyright file="WishListBookEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.WishListBook;

using BookCollector.Data.Models;
using BookCollector.ViewModels.WishListBook;

/// <summary>
/// WishListBookEditView class.
/// </summary>
public partial class WishListBookEditView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WishListBookEditView"/> class.
    /// </summary>
    /// <param name="book">Book to add or edit.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    /// <param name="removeMainViewBefore">The value to determine if Main view should be removed in
    /// stack before this page or not. Default is false.</param>
    /// <param name="mainViewBefore">The main view to insered before this page. Default is null.</param>
    public WishListBookEditView(WishlistBookModel book, string viewTitle, bool removeMainViewBefore = false, WishListBookMainView? mainViewBefore = null)
    {
        this.ViewModel = new WishListBookEditViewModel(book, this)
        {
            ViewTitle = viewTitle,
            RemoveMainViewBefore = removeMainViewBefore,
            MainViewBefore = mainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private WishListBookEditViewModel ViewModel { get; set; }

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

        await this.ViewModel.SetViewModelData();
    }
}