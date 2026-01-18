// <copyright file="BookCollectionList.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Controls.Book;

public partial class BookCollectionList : ContentView
{
    public BookCollectionList()
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