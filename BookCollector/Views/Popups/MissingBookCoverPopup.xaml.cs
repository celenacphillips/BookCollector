// <copyright file="MissingBookCoverPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Views;

public partial class MissingBookCoverPopup : Popup<string>
{
    public MissingBookCoverPopup(string bookTitle)
    {
        this.MissingMessage = AppStringResources.MissingBookCover.Replace("Book", bookTitle);

        this.BindingContext = this;

        this.InitializeComponent();
    }

    public string MissingMessage { get; set; }

    private async void OnSkipAllButton_Clicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(AppStringResources.SkipAll, token: cts.Token);
    }

    private async void OnNoButton_Clicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(AppStringResources.No, token: cts.Token);
    }

    private async void OnYesButton_Clicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(AppStringResources.Yes, token: cts.Token);
    }
}