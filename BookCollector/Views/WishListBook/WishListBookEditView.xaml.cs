// <copyright file="WishListBookEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.WishListBook;

namespace BookCollector.Views.WishListBook;

public partial class WishListBookEditView : ContentPage
{
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

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override async void OnAppearing()
    {
        await this.ViewModel.SetViewModelData();
    }
}