// <copyright file="FilterableListPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.Views.Popups;

public partial class FilterableListPopup : Popup<string?>
{
    public List<string>? Items { get; set; }

    public string? SelectedItem { get; set; }

    public bool ShowFilter { get; set; }

    public string? Title { get; set; }

    public Popup PopupView { get; set; }

    public FilterableListPopup(string title, List<string>? items, string? selectedItem, bool showFilter)
    {
        this.Items = items;
        this.SelectedItem = selectedItem;
        this.Title = title;
        this.BindingContext = this;
        this.ShowFilter = showFilter;
        this.InitializeComponent();
    }

    public async void OnClose(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.SelectedItem, token: cts.Token);
    }
}