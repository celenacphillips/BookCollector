// <copyright file="ChoiceDialogPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using BookCollector.Data.Enums;
using CommunityToolkit.Maui.Views;

/// <summary>
/// ChoiceDialogPopup class.
/// </summary>
public partial class ChoiceDialogPopup : Popup<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChoiceDialogPopup"/> class.
    /// </summary>
    /// <param name="popupWidth">Popup width.</param>
    /// <param name="dialogTitle">Title of popup.</param>
    /// <param name="dialogMessage">Popup message.</param>
    /// <param name="confirm">Confirm button text.</param>
    /// <param name="deny">Deny button text.</param>
    /// <param name="state">Type of popup to display: Commands or Options.</param>
    public ChoiceDialogPopup(double popupWidth, string dialogTitle, string dialogMessage, string confirm, string? deny, DialogState state)
    {
        var deviceHeight = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;
        this.PopupHeight = deviceHeight - 200;
        this.ScrollViewHeight = this.PopupHeight - 400;

        this.PopupWidth = popupWidth;
        this.Title = dialogTitle;
        this.Message = dialogMessage;
        this.Confirm = confirm;
        this.Deny = !string.IsNullOrEmpty(deny) ? deny : string.Empty;
        this.DenyVisible = !string.IsNullOrEmpty(deny);

        this.OptionsVisible = state == DialogState.Options;
        this.CommandsVisible = state == DialogState.Choice;

        this.BindingContext = this;

        this.InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChoiceDialogPopup"/> class.
    /// </summary>
    /// <param name="popupWidth">Popup width.</param>
    /// <param name="dialogTitle">Title of popup.</param>
    /// <param name="dialogMessage">Popup message.</param>
    /// <param name="actions">List of actions that can be performed.</param>
    /// <param name="state">Type of popup to display: Commands or Options.</param>
    public ChoiceDialogPopup(double popupWidth, string dialogTitle, string dialogMessage, List<string> actions, DialogState state)
    {
        var deviceHeight = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;
        this.PopupHeight = deviceHeight - 200;
        this.ScrollViewHeight = this.PopupHeight - 400;

        this.PopupWidth = popupWidth;
        this.Title = dialogTitle;
        this.Message = dialogMessage;
        this.Choices = actions;

        this.OptionsVisible = state == DialogState.Options;
        this.CommandsVisible = state == DialogState.Choice;

        this.BindingContext = this;

        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the popup height.
    /// </summary>
    public double PopupHeight { get; set; }

    /// <summary>
    /// Gets or sets the popup width.
    /// </summary>
    public double PopupWidth { get; set; }

    /// <summary>
    /// Gets or sets the scroll view height.
    /// </summary>
    public double ScrollViewHeight { get; set; }

    /// <summary>
    /// Gets or sets the popup title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the popup message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the confirm button text.
    /// </summary>
    public string Confirm { get; set; }

    /// <summary>
    /// Gets or sets the deny button text.
    /// </summary>
    public string Deny { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the deny button is visible or not.
    /// </summary>
    public bool DenyVisible { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the options button setup is visible or not.
    /// </summary>
    public bool OptionsVisible { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the commands button setup is visible or not.
    /// </summary>
    public bool CommandsVisible { get; set; }

    /// <summary>
    /// Gets or sets the choice actions.
    /// </summary>
    public List<string> Choices { get; set; }

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

    private async void OnChoiceButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(button.Text, token: cts.Token);
    }
}