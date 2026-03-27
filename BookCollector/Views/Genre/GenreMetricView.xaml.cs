// <copyright file="GenreMetricView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Genre;

using BookCollector.Data.Models;

/// <summary>
/// GenreMetricView class.
/// </summary>
public partial class GenreMetricView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenreMetricView"/> class.
    /// </summary>
    /// <param name="selected">Selected genre.</param>
    /// <param name="viewTitle">View title.</param>
    public GenreMetricView(GenreModel selected, string viewTitle)
    {
        this.Genre = selected;
        this.ViewTitle = viewTitle;

        this.BindingContext = this;
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the selected genre.
    /// </summary>
    public GenreModel Genre { get; set; }

    /// <summary>
    /// Gets or sets the title displayed for the current view.
    /// </summary>
    public string ViewTitle { get; set; }
}