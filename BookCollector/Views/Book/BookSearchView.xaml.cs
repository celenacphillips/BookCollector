// <copyright file="BookSearchView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Book;

using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Book;

/// <summary>
/// BookSearchView class.
/// </summary>
public partial class BookSearchView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookSearchView"/> class. BookModel version.
    /// </summary>
    /// <param name="inputIsbn">ISBN to send into the search view.</param>
    /// <param name="inputTitle">Title to send into the search view.</param>
    /// <param name="inputAuthorName">Author Name to send into the search view.</param>
    /// <param name="book">The book to add the returned search data to.</param>
    /// <param name="previousViewModel">he previous view model this method has been called from.</param>
    public BookSearchView(string? inputIsbn, string? inputTitle, string? inputAuthorName, BookModel? book, object? previousViewModel)
    {
        var viewModel = new BookSearchViewModel(inputIsbn, inputTitle, inputAuthorName, this)
        {
            ViewTitle = $"{AppStringResources.BookSearch}",
            SelectedBook = book,
            PreviousViewModel = previousViewModel,
        };
        this.BindingContext = viewModel;

        this.InitializeComponent();
        this.FindByName<CollectionView>("bookList").IsVisible = false;
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BookSearchView"/> class. WishlistBookModel version.
    /// </summary>
    /// <param name="inputIsbn">ISBN to send into the search view.</param>
    /// <param name="inputTitle">Title to send into the search view.</param>
    /// <param name="inputAuthorName">Author Name to send into the search view.</param>
    /// <param name="book">The book to add the returned search data to.</param>
    /// <param name="previousViewModel">he previous view model this method has been called from.</param>
    public BookSearchView(string? inputIsbn, string? inputTitle, string? inputAuthorName, WishlistBookModel? book, object? previousViewModel)
    {
        var viewModel = new BookSearchViewModel(inputIsbn, inputTitle, inputAuthorName, this)
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

    private void OnLayoutMeasured(object? sender, EventArgs? e)
    {
        this.Dispatcher.Dispatch(() =>
        {
            // Wait until the label have real heights
            if (this.header.Height <= 0)
            {
                return;
            }

            // Measure the components above the CollectionView
            var headerHeight = this.header.Height;
            var formHeight = -this.form.Height;

            var usableHeight = BaseViewModel.SetCollectionViewHeight(this.rootLayout.Height, headerHeight, formHeight);

            if (usableHeight > 0)
            {
                this.FindByName<CollectionView>("bookList").HeightRequest = usableHeight;
                this.FindByName<CollectionView>("bookList").IsVisible = true;
            }
        });
    }
}