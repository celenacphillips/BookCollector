// <copyright file="AuthorMetricView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Author;

using BookCollector.Data.Models;

/// <summary>
/// AuthorMetricView class.
/// </summary>
public partial class AuthorMetricView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorMetricView"/> class.
    /// </summary>
    /// <param name="selected">Selected author.</param>
    /// <param name="viewTitle">View title.</param>
    public AuthorMetricView(AuthorModel selected, string viewTitle)
    {
        this.Author = selected;
        this.ViewTitle = viewTitle;

        this.BindingContext = this;
        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the selected author.
    /// </summary>
    public AuthorModel Author { get; set; }

    /// <summary>
    /// Gets or sets the title displayed for the current view.
    /// </summary>
    public string ViewTitle { get; set; }
}