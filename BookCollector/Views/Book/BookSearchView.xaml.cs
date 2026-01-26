// <copyright file="BookSearchView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Book;

namespace BookCollector.Views.Book;

public partial class BookSearchView : ContentPage
{
    public BookSearchView(string? inputIsbn, BookModel? book, object? previousViewModel)
    {
        var viewModel = new BookSearchViewModel(inputIsbn, this)
        {
            ViewTitle = $"{AppStringResources.BookSearch}",
            SelectedBook = book,
            PreviousViewModel = previousViewModel,
        };
        this.BindingContext = viewModel;

        this.InitializeComponent();
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    public BookSearchView(string? inputIsbn, WishlistBookModel? book, object? previousViewModel)
    {
        var viewModel = new BookSearchViewModel(inputIsbn, this)
        {
            ViewTitle = $"{AppStringResources.BookSearch}",
            SelectedWishListBook = book,
            PreviousViewModel = previousViewModel,
        };
        this.BindingContext = viewModel;

        this.InitializeComponent();
        this.FindByName<CollectionView>("bookList").IsVisible = false;
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private void OnLayoutMeasured(object sender, EventArgs e)
    {
        this.Dispatcher.Dispatch(() =>
        {
            // Wait until the label have real heights
            if (this.totalString.Height <= 0)
            {
                return;
            }

            // Measure the components above the CollectionView
            var headerHeight = this.totalString.Height;

            var usableHeight = BaseViewModel.SetCollectionViewHeight(this.rootLayout.Height, headerHeight, 0);

            if (usableHeight > 0)
            {
                this.FindByName<CollectionView>("bookList").HeightRequest = usableHeight;
                this.FindByName<CollectionView>("bookList").IsVisible = true;
            }
        });
    }
}