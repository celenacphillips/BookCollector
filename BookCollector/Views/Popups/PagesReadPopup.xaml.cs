// <copyright file="PagesReadPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Maui.Views;

namespace BookCollector.Views.Popups;

public partial class PagesReadPopup : Popup<int>
{
    public PagesReadPopup(double popupWidth, int pagesRead, int pageTotal)
    {
        this.PopupWidth = popupWidth;
        this.PagesRead = pagesRead;
        this.PageTotal = pageTotal;

        this.BindingContext = this;

        this.InitializeComponent();

        var label = this.FindByName<Label>("PagesReadLabel");
        label.Text = $"{this.PagesRead}";
    }

    public double PopupWidth { get; set; }

    public int PagesRead { get; set; }

    public int PageTotal { get; set; }

    public async void OnClose(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.PagesRead, token: cts.Token);
    }

    public void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        int value = (int)Math.Floor(args.NewValue);
        var label = this.FindByName<Label>("PagesReadLabel");
        label.Text = $"{value}";
    }
}