// <copyright file="AuthorMainView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Author;

using BookCollector.Data.Models;
using BookCollector.ViewModels.Author;
using BookCollector.ViewModels.BaseViewModels;

/// <summary>
/// AuthorMainView class.
/// </summary>
public partial class AuthorMainView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorMainView"/> class.
    /// </summary>
    /// <param name="author">Author to view.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    public AuthorMainView(AuthorModel author, string viewTitle)
    {
        this.ViewModel = new AuthorMainViewModel(author, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
        this.bookCollectionList.IsVisible = false;
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private AuthorMainViewModel ViewModel { get; set; }

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        await this.ViewModel.SetViewModelData();
    }

    private void OnLayoutMeasured(object? sender, EventArgs? e)
    {
        this.Dispatcher.Dispatch(() =>
        {
            // Wait until the label AND search bar have real heights
            if (this.totalString.Height <= 0 || this.searchBar.Height <= 0)
            {
                return;
            }

            // Measure the components above the CollectionView
            var headerHeight = this.totalString.Height;
            var searchHeight = this.searchBar.Height;

            var usableHeight = BaseViewModel.SetCollectionViewHeight(this.rootLayout.Height, headerHeight, searchHeight);

            if (usableHeight > 0)
            {
                this.bookCollectionList.FindByName<CollectionView>("bookList").HeightRequest = usableHeight;
                this.ViewModel.ShowCollectionViewFooter = this.ViewModel.FilteredBooksCount > 0;
                this.bookCollectionList.IsVisible = true;
            }
        });
    }
}