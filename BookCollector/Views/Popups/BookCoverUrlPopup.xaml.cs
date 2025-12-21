// <copyright file="BookCoverUrlPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Maui.Views;

namespace BookCollector.Views.Popups;

public partial class BookCoverUrlPopup : Popup<string?>
{
    public BookCoverUrlPopup(double popupWidth, string? bookCoverUrl)
    {
        this.PopupWidth = popupWidth;
        this.BookCoverUrl = bookCoverUrl;

        this.BindingContext = this;

        this.InitializeComponent();
    }

    public double PopupWidth { get; set; }

    public string? BookCoverUrl { get; set; }

    public async void OnClose(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.BookCoverUrl, token: cts.Token);
    }
}