// <copyright file="GenreMainView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Genre;

using BookCollector.Data.Models;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Genre;

/// <summary>
/// GenreMainView class.
/// </summary>
public partial class GenreMainView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenreMainView"/> class.
    /// </summary>
    /// <param name="genre">Genre to view.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    public GenreMainView(GenreModel genre, string viewTitle)
    {
        this.ViewModel = new GenreMainViewModel(genre, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
        this.bookCollectionList.IsVisible = false;
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private GenreMainViewModel ViewModel { get; set; }

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