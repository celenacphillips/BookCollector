// <copyright file="FilterablePickerOverlay.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Controls;

using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Popups;

/// <summary>
/// FilterablePickerOverlay class.
/// </summary>
public partial class FilterablePickerOverlay : ContentView
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FilterablePickerOverlay"/> class.
    /// </summary>
    /// <param name="viewModel">The previous view model.</param>
    /// <param name="title">The title of the picker.</param>
    /// <param name="items">The list of items for the picker.</param>
    /// <param name="selectedItem">The selected item for the picker.</param>
    /// <param name="showFilter">The value if the filter option of the picker should show.</param>
    /// <param name="isOverlayVisible">The value if the overlay should show.</param>
    public FilterablePickerOverlay(FilterPopupViewModel viewModel, string title, List<string>? items, string? selectedItem, bool showFilter, bool isOverlayVisible)
    {
        this.Items = items;
        this.SelectedItem = selectedItem;
        this.Title = title;
        this.ShowFilter = showFilter;
        this.IsOverlayVisible = isOverlayVisible;
        this.ViewModel = viewModel;

        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the list of items for the picker.
    /// </summary>
    public List<string>? Items { get; set; }

    /// <summary>
    /// Gets or sets the selected item for the picker.
    /// </summary>
    public string? SelectedItem { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the filter should show.
    /// </summary>
    public bool ShowFilter { get; set; }

    /// <summary>
    /// Gets or sets the title of the picker.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the overlay should show.
    /// </summary>
    public bool IsOverlayVisible { get; set; }

    /// <summary>
    /// Gets or sets the previous view model to return to.
    /// </summary>
    public FilterPopupViewModel ViewModel { get; set; }

    /// <summary>
    /// Called when the selected item of the picker changes. Sets the appropriate option in the view model based on the title of the picker and then closes the overlay.
    /// </summary>
    /// <param name="previous">Previous selected value.</param>
    /// <param name="current">Current selected value.</param>
    public void SelectionChanged(string previous, string current)
    {
        if (!current.Equals(previous))
        {
            if (this.Title.Equals(AppStringResources.Favorite))
            {
                this.ViewModel.FavoriteOption = current;
            }

            if (this.Title.Equals(AppStringResources.Authors))
            {
                this.ViewModel.AuthorOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookPublisher))
            {
                this.ViewModel.PublisherOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookPublishYear))
            {
                this.ViewModel.PublishYearOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookFormat))
            {
                this.ViewModel.FormatOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookLanguage))
            {
                this.ViewModel.LanguageOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookRating))
            {
                this.ViewModel.RatingOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookLocation))
            {
                this.ViewModel.LocationOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookSeries))
            {
                this.ViewModel.SeriesOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookCover))
            {
                this.ViewModel.BookCoverOption = current;
            }

            this.OnOverlayTapped(null, null);
        }
    }

    private async void OnOverlayTapped(object? sender, EventArgs? e)
    {
        this.ViewModel.OverlaySection.Remove(this);
        this.IsOverlayVisible = false;
    }
}