// <copyright file="ReadingView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Library;

using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Library;

/// <summary>
/// ReadingView class.
/// </summary>
public partial class ReadingView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadingView"/> class.
    /// </summary>
    public ReadingView()
    {
        // Put on first view to set the status bar to whatever color the user wants the app to be.
        var savedColor = Preferences.Get("AppColor", "#336699" /* Default */);

        // https://developer.android.com/about/versions/15/behavior-changes-15#custom-background-protection
        this.ViewModel = new ReadingViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
        this.bookCollectionList.IsVisible = false;
        this.rootLayout.SizeChanged += this.OnLayoutMeasured;
    }

    private ReadingViewModel ViewModel { get; set; }

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