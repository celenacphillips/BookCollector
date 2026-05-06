// <copyright file="DragAndDropContainer.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Controls;

using System.Collections;
using System.Collections.Specialized;

/// <summary>
/// DragAndDropContainer class.
/// </summary>
public partial class DragAndDropContainer : ContentView
{
    /// <summary>
    /// Gets or sets the collection of items to display in the control.
    /// </summary>
    public static readonly BindableProperty DragAndDropItemsProperty =
             BindableProperty.Create(
                 nameof(DragAndDropItems),
                 typeof(IEnumerable),
                 typeof(DragAndDropContainer),
                 defaultValue: null,
                 propertyChanged: OnRefreshControl);

    private View? draggedItem;

    private int draggedIndex;

    /// <summary>
    /// Initializes a new instance of the <see cref="DragAndDropContainer"/> class.
    /// </summary>
    public DragAndDropContainer()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// The passed-in event to occur when the drag and drop items have been updated.
    /// </summary>
    public event EventHandler? OnDragAndDropItemsUpdated;

    /// <summary>
    /// Gets or sets the collection of items to display in the control.
    /// </summary>
    public IEnumerable DragAndDropItems
    {
        get => (IEnumerable)this.GetValue(DragAndDropItemsProperty);
        set => this.SetValue(DragAndDropItemsProperty, value);
    }

    private static void OnRefreshControl(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is DragAndDropContainer dragAndDropContainer)
        {
            dragAndDropContainer.LoadDragAndDropItems();

            if (dragAndDropContainer.DragAndDropItems is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged -= (s, e) => dragAndDropContainer.LoadDragAndDropItems();
                notifyCollection.CollectionChanged += (s, e) => dragAndDropContainer.LoadDragAndDropItems();
            }
        }
    }

    private void LoadDragAndDropItems()
    {
        var itemLayout = this.ItemsStackLayout;
        itemLayout.Children.Clear();

        if (this.DragAndDropItems != null)
        {
            foreach (var item in this.DragAndDropItems)
            {
                if (item is not View itemView)
                {
                    continue;
                }

                if (itemView.Parent is Grid parentGrid)
                {
                    parentGrid.Children.Remove(itemView);
                }

                var dragImage = new Image
                {
                    Source = new FontImageSource
                    {
                        FontFamily = "MaterialDesignIcons",
                        Glyph = char.ConvertFromUtf32(0xF12F0),
                        Color = Colors.White,
                        Size = 24,
                    },
                    WidthRequest = 24,
                    HeightRequest = 24,
                    HorizontalOptions = LayoutOptions.End,
                };

                var grid = new Grid();

                if (itemView.HorizontalOptions == LayoutOptions.Center)
                {
                    grid.ColumnDefinitions =
                    [
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    ];
                }
                else
                {
                    grid.ColumnDefinitions =
                    [
                        new ColumnDefinition { Width = GridLength.Auto },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    ];
                }

                grid.Add(itemView, grid.ColumnDefinitions.Count - 2, 0);
                grid.Add(dragImage, grid.ColumnDefinitions.Count - 1, 0);

                var dragGesture = new DragGestureRecognizer();
                dragGesture.DragStarting += (s, e) => this.DragGestureRecognizer(itemView);

                var dropGesture = new DropGestureRecognizer();
                dropGesture.Drop += (s, e) => this.DropGestureRecognizer(itemView);

                dragImage.GestureRecognizers.Add(dragGesture);
                dragImage.GestureRecognizers.Add(dropGesture);

                itemLayout.Add(grid);
            }
        }
    }

    private void DragGestureRecognizer(View item)
    {
        this.draggedItem = item;
        this.draggedIndex = ((IList)this.DragAndDropItems).IndexOf(this.draggedItem);
    }

    private void DropGestureRecognizer(View targetItem)
    {
        if (this.draggedItem == null)
        {
            return;
        }

        var targetIndex = ((IList)this.DragAndDropItems).IndexOf(targetItem);

        var list = new ArrayList((IList)this.DragAndDropItems);

        if (this.draggedIndex != targetIndex)
        {
            list.RemoveAt(this.draggedIndex);
            list.Insert(targetIndex, this.draggedItem);
        }

        this.SetValue(DragAndDropItemsProperty, list);

        this.draggedItem = null;
        this.draggedIndex = -1;

        this.OnDragAndDropItemsUpdated?.Invoke(this, EventArgs.Empty);
    }
}