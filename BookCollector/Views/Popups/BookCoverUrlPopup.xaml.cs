// <copyright file="BookCoverUrlPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using CommunityToolkit.Maui.Views;

/// <summary>
/// BookCoverUrlPopup class.
/// </summary>
public partial class BookCoverUrlPopup : Popup<string?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookCoverUrlPopup"/> class.
    /// </summary>
    /// <param name="popupWidth">Max width of popup.</param>
    /// <param name="bookCoverUrl">Book cover url.</param>
    public BookCoverUrlPopup(double popupWidth, string? bookCoverUrl)
    {
        this.PopupWidth = popupWidth;
        this.BookCoverUrl = bookCoverUrl;

        this.BindingContext = this;

        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the popup width.
    /// </summary>
    public double PopupWidth { get; set; }

    /// <summary>
    /// Gets or sets the book cover URL.
    /// </summary>
    public string? BookCoverUrl { get; set; }

    /// <summary>
    /// Called when the popup is closed, passing the book cover URL back to the caller.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public async void OnClose(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.BookCoverUrl, token: cts.Token);
    }
}