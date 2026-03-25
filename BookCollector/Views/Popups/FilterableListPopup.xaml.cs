// <copyright file="FilterableListPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using CommunityToolkit.Maui.Views;

/// <summary>
/// FilterableListPopup class.
/// </summary>
public partial class FilterableListPopup : Popup<string?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FilterableListPopup"/> class.
    /// </summary>
    /// <param name="title">The title of the picker.</param>
    /// <param name="items">The list of items for the picker.</param>
    /// <param name="selectedItem">The selected item for the picker.</param>
    /// <param name="showFilter">The value if the filter option of the picker should show.</param>
    public FilterableListPopup(string title, List<string>? items, string? selectedItem, bool showFilter)
    {
        this.Items = items;
        this.SelectedItem = selectedItem;
        this.Title = title;
        this.BindingContext = this;
        this.ShowFilter = showFilter;
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
    public string? Title { get; set; }

    /// <summary>
    /// Called when the popup is closed. Closes the popup and returns the selected item.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public async void OnClose(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.SelectedItem, token: cts.Token);
    }
}