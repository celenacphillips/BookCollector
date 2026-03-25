// <copyright file="SliderPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Views;

/// <summary>
/// SliderPopup class.
/// </summary>
public partial class SliderPopup : Popup<int>
{
    private bool inputNotValidField;

    /// <summary>
    /// Initializes a new instance of the <see cref="SliderPopup"/> class.
    /// </summary>
    /// <param name="title">Title of the popup.</param>
    /// <param name="popupWidth">Max width of the popup.</param>
    /// <param name="inputValue">Value to default the slider to.</param>
    /// <param name="maxSliderValue">Max value of the slider.</param>
    public SliderPopup(string title, double popupWidth, int inputValue, int maxSliderValue)
    {
        this.Title = title;
        this.PopupWidth = popupWidth;
        this.InputValue = inputValue;
        this.MaxSliderValue = maxSliderValue;
        this.InputNotValid = false;

        this.BindingContext = this;

        this.InitializeComponent();

        var editor = this.FindByName<Editor>("InputValueEditor");
        editor.Text = $"{this.InputValue}";
    }

    /// <summary>
    /// Gets or sets the title of the popup.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the popup width.
    /// </summary>
    public double PopupWidth { get; set; }

    /// <summary>
    /// Gets or sets the input value for the slider.
    /// </summary>
    public int InputValue { get; set; }

    /// <summary>
    /// Gets or sets the max value for the slider.
    /// </summary>
    public int MaxSliderValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the current input is not valid.
    /// </summary>
    public bool InputNotValid
    {
        get => this.inputNotValidField;
        set
        {
            if (this.inputNotValidField != value)
            {
                this.inputNotValidField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Called once the close button is clicked. Closes the popup and returns the input value.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public async void OnClose(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.InputValue, token: cts.Token);
    }

    /// <summary>
    /// Called when the slider value has changed. Updates the label to show the current value and sets the input value to the new value.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The event.</param>
    public void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (this.Title.Equals(AppStringResources.PagesRead))
        {
            var value = (int)Math.Floor(args.NewValue);
            var editor = this.FindByName<Editor>("InputValueEditor");
            editor.Text = $"{value}";
            this.InputValue = value;
        }
    }

    private void InputValueEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (this.Title.Equals(AppStringResources.PagesRead))
        {
            var tryIntParse = int.TryParse(e.NewTextValue, out var value);
            var slider = this.FindByName<Slider>("InputSlider");

            if (tryIntParse)
            {
                if (value >= 0 && value <= this.MaxSliderValue)
                {
                    this.InputNotValid = false;
                    slider.Value = value;
                    this.InputValue = value;
                }
                else
                {
                    this.InputNotValid = true;
                }
            }
        }
    }
}