// <copyright file="PagesReadPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Views;

namespace BookCollector.Views.Popups;

public partial class SliderPopup : Popup<int>
{
    public SliderPopup(string title, double popupWidth, int inputValue, int maxSliderValue)
    {
        this.Title = title;
        this.PopupWidth = popupWidth;
        this.InputValue = inputValue;
        this.MaxSliderValue = maxSliderValue;

        this.BindingContext = this;

        this.InitializeComponent();

        var label = this.FindByName<Label>("InputValueLabel");
        label.Text = $"{this.InputValue}";
    }

    public string Title { get; set; }

    public double PopupWidth { get; set; }

    public int InputValue { get; set; }

    public int MaxSliderValue { get; set; }

    public async void OnClose(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.InputValue, token: cts.Token);
    }

    public void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (this.Title.Equals(AppStringResources.PagesRead))
        {
            var value = (int)Math.Floor(args.NewValue);
            var label = this.FindByName<Label>("InputValueLabel");
            label.Text = $"{value}";
            this.InputValue = value;
        }
    }
}