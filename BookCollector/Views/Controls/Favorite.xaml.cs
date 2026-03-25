// <copyright file="Favorite.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Controls;

using BookCollector.Data.Enums;

/// <summary>
/// Favorite class.
/// </summary>
public partial class Favorite : ContentView
{
    /// <summary>
    /// Gets or sets a value indicating whether the value is on or not.
    /// </summary>
    public static readonly BindableProperty CurrentValueProperty =
             BindableProperty.Create(
                 nameof(CurrentValue),
                 typeof(bool),
                 typeof(Favorite),
                 propertyChanged: OnRefreshControl);

    private readonly Favorite view;

    /// <summary>
    /// Initializes a new instance of the <see cref="Favorite"/> class.
    /// </summary>
    public Favorite()
    {
        this.view = this;
        this.InitializeComponent();
        this.SetHeart();
    }

    /// <summary>
    /// Gets or sets a value indicating whether the value is on or not.
    /// </summary>
    public bool CurrentValue
    {
        get => (bool)this.GetValue(CurrentValueProperty);
        set => this.SetValue(CurrentValueProperty, value);
    }

    private static void OnRefreshControl(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Favorite favorite)
        {
            favorite.SetHeart();
        }
    }

    private static FontImageSource CreateHeartLabel(HeartState state)
    {
        return state switch
        {
            HeartState.Empty => new FontImageSource
            {
                Glyph = "\U000F0A57",
                FontFamily = "MaterialDesignIcons",
                Size = 18,
                Color = Color.FromArgb("#ff0000"),
            },
            HeartState.Full => new FontImageSource
            {
                Glyph = "\U000F0A56",
                FontFamily = "MaterialDesignIcons",
                Size = 18,
                Color = Color.FromArgb("#ff0000"),
            },
            _ => throw new NotImplementedException(),
        };
    }

    private void SetHeart()
    {
        var heart = this.view.heart;

        if (!this.CurrentValue)
        {
            heart.Source = CreateHeartLabel(HeartState.Empty);
        }
        else
        {
            heart.Source = CreateHeartLabel(HeartState.Full);
        }
    }

    private void Heart_Clicked(object sender, EventArgs e)
    {
        if (!this.CurrentValue)
        {
            this.CurrentValue = true;
        }
        else
        {
            this.CurrentValue = false;
        }

        this.SetHeart();
    }
}