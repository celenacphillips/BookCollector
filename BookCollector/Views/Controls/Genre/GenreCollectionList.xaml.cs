// <copyright file="GenreCollectionList.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Controls.Genre;

public partial class GenreCollectionList : ContentView
{
    public GenreCollectionList()
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