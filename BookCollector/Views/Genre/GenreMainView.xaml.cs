// <copyright file="GenreMainView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Genre;

namespace BookCollector.Views.Genre;

public partial class GenreMainView : ContentPage
{
    public GenreMainView(GenreModel genre, string viewTitle)
    {
        this.ViewModel = new GenreMainViewModel(genre, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private void OnLayoutMeasured(object sender, EventArgs e)
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
        }
    }

    private GenreMainViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override async void OnAppearing()
    {
        await this.ViewModel.SetViewModelData();
    }
}