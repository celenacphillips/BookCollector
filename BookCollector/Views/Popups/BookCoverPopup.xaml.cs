// <copyright file="BookCoverPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using CommunityToolkit.Maui.Views;

/// <summary>
/// BookCoverPopup class.
/// </summary>
public partial class BookCoverPopup : Popup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookCoverPopup"/> class.
    /// </summary>
    /// <param name="bookCover">Book cover to view.</param>
    public BookCoverPopup(ImageSource bookCover)
    {
        this.BookCover = bookCover;

        this.BindingContext = this;

        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the book cover image source.
    /// </summary>
    public ImageSource BookCover { get; set; }
}