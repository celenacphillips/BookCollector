// <copyright file="BookSearchView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.BaseViewModels;

namespace BookCollector.Views.Book;

public partial class BookSearchView : ContentPage
{
    public BookSearchView()
    {
        this.InitializeComponent();
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private void OnLayoutMeasured(object sender, EventArgs e)
    {
        // Wait until the label AND search bar have real heights
        if (this.totalString.Height <= 0)
        {
            return;
        }

        // Measure the components above the CollectionView
        var headerHeight = this.totalString.Height;
        var searchHeight = 0;

        var usableHeight = BaseViewModel.SetCollectionViewHeight(this.rootLayout.Height, headerHeight, searchHeight);

        if (usableHeight > 0)
        {
            this.FindByName<CollectionView>("bookList").HeightRequest = usableHeight;
        }
    }
}