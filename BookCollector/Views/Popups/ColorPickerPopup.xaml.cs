// <copyright file="ColorPickerPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using CommunityToolkit.Maui.Views;
using Maui.ColorPicker;

/// <summary>
/// ColorPickerPopup class.
/// </summary>
public partial class ColorPickerPopup : Popup<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPickerPopup"/> class.
    /// </summary>
    public ColorPickerPopup(Color selectedColor)
    {
        var deviceHeight = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;
        this.PopupHeight = deviceHeight - 200;
        this.ColorPickerHeight = this.PopupHeight - 400;

        this.BindingContext = this;
        this.InitializeComponent();

        var picker = this.FindByName<ColorPicker>("ColorPicker");
        picker.PickedColor = selectedColor;
    }

    public double PopupHeight { get; set; }

    public double ColorPickerHeight { get; set; }

    private Color SelectedColor { get; set; }

    private void ColorPicker_PickedColorChanged(object sender, PickedColorChangedEventArgs e)
    {
        var newColor = e.NewPickedColorValue;

        if (e.OldPickedColorValue != null)
        {
            var hexCode = newColor.ToHex();
            Data.Colors.SetPreviewColors(hexCode);
            this.SelectedColor = newColor;
        }
    }

    private void OnCloseButton_Clicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        this.CloseAsync(this.SelectedColor.ToHex(), token: cts.Token);
    }
}