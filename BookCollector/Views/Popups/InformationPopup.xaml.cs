// <copyright file="InformationPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using CommunityToolkit.Maui.Views;

/// <summary>
/// InformationPopup class.
/// </summary>
public partial class InformationPopup : Popup
{
    /// <summary>
    /// Gets or sets the version number.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "View Property")]
    public double PopupHeight = 190;

    /// <summary>
    /// Initializes a new instance of the <see cref="InformationPopup"/> class.
    /// </summary>
    /// <param name="popupWidth">The max width of the popup.</param>
    /// <param name="infoText">The text to display in the popup.</param>
    public InformationPopup(double popupWidth, string infoText)
    {
        this.PopupWidth = popupWidth;
        this.InfoText = infoText;

        this.BindingContext = this;

        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the popup width.
    /// </summary>
    public double PopupWidth { get; set; }

    /// <summary>
    /// Gets or sets the info text to display.
    /// </summary>
    public string InfoText { get; set; }
}