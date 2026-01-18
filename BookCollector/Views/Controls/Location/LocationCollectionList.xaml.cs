// <copyright file="LocationCollectionList.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Controls.Location;

public partial class LocationCollectionList : ContentView
{
    public LocationCollectionList()
    {
        this.InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        // Reset visual state when the cell is reused
        VisualStateManager.GoToState(this, "Normal");
    }
}