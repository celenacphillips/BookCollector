// <copyright file="SeriesMetricView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Series;

using BookCollector.Data.Models;

/// <summary>
/// SeriesMetricView class.
/// </summary>
public partial class SeriesMetricView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SeriesMetricView"/> class.
    /// </summary>
    /// <param name="selected">Selected series.</param>
    /// <param name="viewTitle">View title.</param>
    public SeriesMetricView(SeriesModel selected, string viewTitle)
    {
        this.Series = selected;
        this.ViewTitle = viewTitle;

        this.BindingContext = this;
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the selected series.
    /// </summary>
    public SeriesModel Series { get; set; }

    /// <summary>
    /// Gets or sets the title displayed for the current view.
    /// </summary>
    public string ViewTitle { get; set; }
}