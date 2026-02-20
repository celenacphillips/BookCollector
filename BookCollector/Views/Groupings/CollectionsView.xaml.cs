// <copyright file="CollectionsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Groupings;

using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;

/// <summary>
/// CollectionsView class.
/// </summary>
public partial class CollectionsView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionsView"/> class.
    /// </summary>
    public CollectionsView()
    {
        this.ViewModel = new CollectionsViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
        this.collectionCollectionList.IsVisible = false;
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private CollectionsViewModel ViewModel { get; set; }

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
                this.collectionCollectionList.FindByName<CollectionView>("collectionList").HeightRequest = usableHeight;
                this.ViewModel.ShowCollectionViewFooter = this.ViewModel.FilteredCollectionsCount > 0;
                this.collectionCollectionList.IsVisible = true;
            }
        });
    }
}