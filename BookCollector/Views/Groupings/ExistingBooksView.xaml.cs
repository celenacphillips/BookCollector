// <copyright file="ExistingBooksView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Groupings;

using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;

/// <summary>
/// ExistingBooksView class.
/// </summary>
public partial class ExistingBooksView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExistingBooksView"/> class.
    /// </summary>
    /// <param name="selected">Selected grouping.</param>
    /// <param name="viewTitle">Selected grouping name.</param>
    /// <param name="previousViewModel">Selected grouping main page.</param>
    public ExistingBooksView(object selected, string viewTitle, object previousViewModel)
    {
        this.ViewModel = new ExistingBooksViewModel(selected, this, previousViewModel)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
        this.bookCollectionList.IsVisible = false;
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private ExistingBooksViewModel ViewModel { get; set; }

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