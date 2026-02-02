// <copyright file="BookCollectionList.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>
#if ANDROID
using Android.Views;
using AndroidX.Core.View;
#endif
using BookCollector.ViewModels.BaseViewModels;

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