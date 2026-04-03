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
    private string selectedColorHexCodeField;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPickerPopup"/> class.
    /// </summary>
    /// <param name="selectedColor">The selected color.</param>
    public ColorPickerPopup(Color selectedColor)
    {
        var deviceHeight = DeviceDisplay.Current.MainDisplayInfo.Height / DeviceDisplay.Current.MainDisplayInfo.Density;
        this.PopupHeight = deviceHeight - 200;
        this.ColorPickerHeight = this.PopupHeight - 400;

        this.BindingContext = this;
        this.InitializeComponent();

        var picker = this.FindByName<ColorPicker>("ColorPicker");
        picker.PickedColor = selectedColor;

        this.SelectedColorHexCode = selectedColor.ToHex();
    }

    /// <summary>
    /// Gets or sets the popup height.
    /// </summary>
    public double PopupHeight { get; set; }

    /// <summary>
    /// Gets or sets the color picker height.
    /// </summary>
    public double ColorPickerHeight { get; set; }

    /// <summary>
    /// Gets or sets the selected color hex code.
    /// </summary>
    public string SelectedColorHexCode
    {
        get => this.selectedColorHexCodeField;
        set
        {
            if (this.selectedColorHexCodeField != value)
            {
                this.selectedColorHexCodeField = value;
                this.OnPropertyChanged();
            }
        }
    }

    private Color SelectedColor { get; set; }

    private void ColorPicker_PickedColorChanged(object sender, PickedColorChangedEventArgs e)
    {
        var newColor = e.NewPickedColorValue;

        if (e.OldPickedColorValue != null)
        {
            var hexCode = newColor.ToHex();
            Data.Colors.SetPreviewColors(hexCode);
            this.SelectedColor = newColor;
            this.SelectedColorHexCode = newColor.ToHex();
        }
    }

    private void OnCloseButton_Clicked(object sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        this.CloseAsync(this.SelectedColor.ToHex(), token: cts.Token);
    }
}