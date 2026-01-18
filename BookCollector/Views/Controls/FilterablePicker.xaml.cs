// <copyright file="FilterablePicker.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BookCollector.Views.Controls;

public partial class FilterablePicker : ContentView
{
    public static readonly BindableProperty SelectedItemProperty =
             BindableProperty.Create(
                 nameof(SelectedItem),
                 typeof(string),
                 typeof(FilterablePicker),
                 propertyChanged: OnRefreshControl);

    public static readonly BindableProperty TotalItemsProperty =
             BindableProperty.Create(
                 nameof(TotalItemsString),
                 typeof(string),
                 typeof(FilterablePicker));

    public static readonly BindableProperty ItemsProperty =
             BindableProperty.Create(
                 nameof(Items),
                 typeof(List<string>),
                 typeof(FilterablePicker),
                 propertyChanged: OnRefreshControl);

    public static readonly BindableProperty ShowFilterProperty =
             BindableProperty.Create(
                 nameof(ShowFilter),
                 typeof(bool),
                 typeof(FilterablePicker),
                 propertyChanged: OnRefreshControl);

    public static readonly BindableProperty TotalItemsColorProperty =
             BindableProperty.Create(
                 nameof(TotalItemsColor),
                 typeof(Color),
                 typeof(FilterablePicker),
                 propertyChanged: OnRefreshControl);

    public static readonly BindableProperty MainTextColorProperty =
             BindableProperty.Create(
                 nameof(MainTextColor),
                 typeof(Color),
                 typeof(FilterablePicker),
                 propertyChanged: OnRefreshControl);

    public static readonly BindableProperty SelectedItemColorProperty =
             BindableProperty.Create(
                 nameof(SelectedItemColor),
                 typeof(Color),
                 typeof(FilterablePicker),
                 propertyChanged: OnRefreshControl);

    private readonly FilterablePicker view;

    public FilterablePicker()
    {
        this.FilteredItems = [];
        this.CollectionViewHeight = 300;

        this.InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        // Reset visual state when the cell is reused
        VisualStateManager.GoToState(this, "Normal");
    }

    public string? SelectedItem
    {
        get => (string?)this.GetValue(SelectedItemProperty);
        set => this.SetValue(SelectedItemProperty, value);
    }

    public List<string>? Items
    {
        get => (List<string>?)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    private ObservableCollection<string>? filteredItemsField;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ObservableCollection<string>? FilteredItems
    {
        get => this.filteredItemsField;
        set
        {
            this.filteredItemsField = value;
            this.OnPropertyChanged();
        }
    }

    public string? TotalItemsString
    {
        get => (string?)this.GetValue(TotalItemsProperty);
        set => this.SetValue(TotalItemsProperty, value);
    }

    public bool ShowFilter
    {
        get => (bool)this.GetValue(ShowFilterProperty);
        set => this.SetValue(ShowFilterProperty, value);
    }

    public Color MainTextColor
    {
        get => (Color)this.GetValue(MainTextColorProperty);
        set => this.SetValue(MainTextColorProperty, value);
    }

    public Color TotalItemsColor
    {
        get => (Color)this.GetValue(TotalItemsColorProperty);
        set => this.SetValue(TotalItemsColorProperty, value);
    }

    public Color SelectedItemColor
    {
        get => (Color)this.GetValue(SelectedItemColorProperty);
        set => this.SetValue(SelectedItemColorProperty, value);
    }

    public int CollectionViewHeight { get; set; }

    private static void OnRefreshControl(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FilterablePicker filterablePicker)
        {
            if (filterablePicker.Items != null && filterablePicker.FilteredItems.Count == 0)
            {
                foreach (var item in filterablePicker.Items)
                {
                    filterablePicker.FilteredItems ??= [];
                    filterablePicker.FilteredItems.Add(item);
                }

                filterablePicker.TotalItemsString = AppStringResources.Blank1OfBlank2Items.Replace("Blank1", filterablePicker.FilteredItems?.Count.ToString()).Replace("Blank2", filterablePicker.Items?.Count.ToString());
            }

            if (filterablePicker.SelectedItem != null)
            {
                filterablePicker.collectionView.Loaded += (s, e) =>
                {
                    filterablePicker.collectionView.ScrollTo(filterablePicker.SelectedItem, position: ScrollToPosition.Center, animate: false);
                };
            }
        }
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchString = e.NewTextValue;

        List<string>? filtered = this.Items;
        this.FilteredItems?.Clear();

        if (filtered != null)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                filtered = filtered.Where(x =>
                    x.Contains(searchString.Trim(), StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            foreach (var item in filtered)
            {
                this.FilteredItems ??= [];
                this.FilteredItems.Add(item);
            }
        }

        if (this.FilteredItems.Contains(this.SelectedItem))
        {
            this.collectionView.ScrollTo(this.SelectedItem, position: ScrollToPosition.Center, animate: false);
        }

        this.TotalItemsString = AppStringResources.Blank1OfBlank2Items.Replace("Blank1", this.FilteredItems?.Count.ToString()).Replace("Blank2", this.Items?.Count.ToString());
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.SelectedItem != null && e.CurrentSelection.Count > 0)
        {
            this.SelectedItem = (string)e.CurrentSelection[0];

            try
            {
                var view = (Popup<string?>)this.BindingContext;

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                await view.CloseAsync(this.SelectedItem, token: cts.Token);
            }
            catch (Exception ex)
            {

            }

            try
            {
                var view = (FilterablePickerOverlay)this.BindingContext;

                view.SelectionChanged((string)e.PreviousSelection[0], (string)e.CurrentSelection[0]);
            }
            catch (Exception ex)
            {

            }
        }
    }
}