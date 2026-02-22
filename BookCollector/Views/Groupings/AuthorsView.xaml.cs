// <copyright file="AuthorsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>
namespace BookCollector.Views.Groupings;

using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;

/// <summary>
/// AuthorsView class.
/// </summary>
public partial class AuthorsView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorsView"/> class.
    /// </summary>
    public AuthorsView()
    {
        this.ViewModel = new AuthorsViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
        this.authorCollectionList.IsVisible = false;
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private AuthorsViewModel ViewModel { get; set; }

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
                this.authorCollectionList.FindByName<CollectionView>("authorList").HeightRequest = usableHeight;
                this.ViewModel.ShowCollectionViewFooter = this.ViewModel.FilteredAuthorsCount > 0;
                this.authorCollectionList.IsVisible = true;
            }
        });
    }
}