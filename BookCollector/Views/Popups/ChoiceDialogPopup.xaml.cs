// <copyright file="ChoiceDialogPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

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
    /// <param name="type">Type of popup to display: Commands or Options.</param>
    public ChoiceDialogPopup(double popupWidth, string dialogTitle, string dialogMessage, string confirm, string? deny, string type)
    {
        this.PopupWidth = popupWidth;
        this.Title = dialogTitle;
        this.Message = dialogMessage;
        this.Confirm = confirm;
        this.Deny = !string.IsNullOrEmpty(deny) ? deny : string.Empty;
        this.DenyVisible = !string.IsNullOrEmpty(deny);

        this.OptionsVisible = type.Equals("Options");
        this.CommandsVisible = type.Equals("Commands");

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
    /// <param name="type">Type of popup to display: Commands or Options.</param>
    public ChoiceDialogPopup(double popupWidth, string dialogTitle, string dialogMessage, List<string> actions, string type)
    {
        this.PopupWidth = popupWidth;
        this.Title = dialogTitle;
        this.Message = dialogMessage;
        this.Choices = actions;

        this.OptionsVisible = type.Equals("Options");
        this.CommandsVisible = type.Equals("Commands");

        this.BindingContext = this;

        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the popup width.
    /// </summary>
    public double PopupWidth { get; set; }

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

    /// <summary>
    /// Gets or sets the selected choice.
    /// </summary>
    public string SelectedChoice { get; set; }

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

        await this.CloseAsync(button.BindingContext.ToString(), token: cts.Token);
    }
}