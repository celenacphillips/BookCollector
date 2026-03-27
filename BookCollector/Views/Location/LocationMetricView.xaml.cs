// <copyright file="LocationMetricView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Location;

using BookCollector.Data.Models;

/// <summary>
/// LocationMetricView class.
/// </summary>
public partial class LocationMetricView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LocationMetricView"/> class.
    /// </summary>
    /// <param name="selected">Selected location.</param>
    /// <param name="viewTitle">View title.</param>
    public LocationMetricView(LocationModel selected, string viewTitle)
    {
        this.Location = selected;
        this.ViewTitle = viewTitle;

        this.BindingContext = this;
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the selected location.
    /// </summary>
    public LocationModel Location { get; set; }

    /// <summary>
    /// Gets or sets the title displayed for the current view.
    /// </summary>
    public string ViewTitle { get; set; }
}