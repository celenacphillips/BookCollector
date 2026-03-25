// <copyright file="WishListBookMainView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.WishListBook;

using BookCollector.Data.Models;
using BookCollector.ViewModels.WishListBook;

/// <summary>
/// WishListBookMainView class.
/// </summary>
public partial class WishListBookMainView : ContentPage
{
    private WishListBookMainViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="WishListBookMainView"/> class.
    /// </summary>
    /// <param name="book">Book to view.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    public WishListBookMainView(WishlistBookModel book, string viewTitle)
    {
        this.viewModel = new WishListBookMainViewModel(book, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

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

        await this.viewModel.SetViewModelData();
    }
}