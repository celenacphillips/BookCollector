// <copyright file="InformationPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Maui.Views;

namespace BookCollector.Views.Popups;

public partial class InformationPopup : Popup
{
    public double PopupHeight = 190;

    public InformationPopup(double popupWidth, string infoText)
    {
        this.PopupWidth = popupWidth;
        this.InfoText = infoText;

        this.BindingContext = this;

        this.InitializeComponent();
    }

    public double PopupWidth { get; set; }

    public string InfoText { get; set; }
}