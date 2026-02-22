// <copyright file="LocationsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Groupings;

using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;

/// <summary>
/// LocationsView class.
/// </summary>
public partial class LocationsView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LocationsView"/> class.
    /// </summary>
    public LocationsView()
    {
        this.ViewModel = new LocationsViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
        this.locationCollectionList.IsVisible = false;
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private LocationsViewModel ViewModel { get; set; }

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
                this.locationCollectionList.FindByName<CollectionView>("locationList").HeightRequest = usableHeight;
                this.ViewModel.ShowCollectionViewFooter = this.ViewModel.FilteredLocationsCount > 0;
                this.locationCollectionList.IsVisible = true;
            }
        });
    }
}