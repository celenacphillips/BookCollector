// <copyright file="WishListBookMainView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.WishListBook;

namespace BookCollector.Views.WishListBook;

public partial class WishListBookMainView : ContentPage
{
    private WishListBookMainViewModel viewModel;

    public WishListBookMainView(WishlistBookModel book, string viewTitle)
    {
        this.viewModel = new WishListBookMainViewModel(book, this)
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
        var variable = this.viewModel.SetViewModelData();
    }
}