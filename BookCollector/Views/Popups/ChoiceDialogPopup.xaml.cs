// <copyright file="ChoiceDialogPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using CommunityToolkit.Maui.Views;

public partial class ChoiceDialogPopup : Popup<string>
{
    public ChoiceDialogPopup(double popupWidth, string dialogTitle, string dialogMessage, string confirm, string? deny, string type)
    {
        this.PopupWidth = popupWidth;
        this.Title = dialogTitle;
        this.Message = dialogMessage;
        this.Confirm = confirm;
        this.Deny = deny;
        this.DenyVisible = !string.IsNullOrEmpty(deny);

        this.OptionsVisible = type.Equals("Options");
        this.CommandsVisible = type.Equals("Commands");

        this.BindingContext = this;

        this.InitializeComponent();
    }

    public double PopupWidth { get; set; }

    public string Title { get; set; }

    public string Message { get; set; }

    public string Confirm { get; set; }

    public string Deny { get; set; }

    public bool DenyVisible { get; set; }

    public bool OptionsVisible { get; set; }

    public bool CommandsVisible { get; set; }

    private async void OnDenyButton_Clicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.Deny, token: cts.Token);
    }

    private async void OnConfirmButton_Clicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.Confirm, token: cts.Token);
    }
}