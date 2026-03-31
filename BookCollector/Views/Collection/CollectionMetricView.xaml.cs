// <copyright file="CollectionMetricView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Collection;

using BookCollector.Data.Models;

/// <summary>
/// CollectionMetricView class.
/// </summary>
public partial class CollectionMetricView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CollectionMetricView"/> class.
    /// </summary>
    /// <param name="selected">Selected collection.</param>
    /// <param name="viewTitle">View title.</param>
    public CollectionMetricView(CollectionModel selected, string viewTitle)
    {
        this.Collection = selected;
        this.ViewTitle = viewTitle;

        this.BindingContext = this;
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the selected collection.
    /// </summary>
    public CollectionModel Collection { get; set; }

    /// <summary>
    /// Gets or sets the title displayed for the current view.
    /// </summary>
    public string ViewTitle { get; set; }
}