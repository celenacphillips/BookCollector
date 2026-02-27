// <copyright file="Rating.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Controls;

using BookCollector.Data.Enums;
using DocumentFormat.OpenXml.Drawing;
using Colors = Microsoft.Maui.Graphics.Colors;

/// <summary>
/// Rating class.
/// </summary>
public partial class Rating : ContentView
{
    /// <summary>
    /// Gets or sets the value selected.
    /// </summary>
    public static readonly BindableProperty CurrentValueProperty =
             BindableProperty.Create(
                 nameof(CurrentValue),
                 typeof(int),
                 typeof(Rating),
                 propertyChanged: OnRefreshControl);

    private const int MAXVALUE = 5;
    private readonly Rating view;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rating"/> class.
    /// </summary>
    public Rating()
    {
        this.view = this;
        this.InitializeComponent();
        this.SetStars();
    }

    /// <summary>
    /// Gets or sets the value selected.
    /// </summary>
    public int CurrentValue
    {
        get => (int)this.GetValue(CurrentValueProperty);
        set => this.SetValue(CurrentValueProperty, value);
    }

    private static void OnRefreshControl(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Rating rating)
        {
            rating.SetStars();
        }
    }

    private static FontImageSource CreateStarLabel(StarState state)
    {
        return state switch
        {
            StarState.Empty => new FontImageSource
            {
                Glyph = "\U000F04D2",
                FontFamily = "MaterialDesignIcons",
                Size = 22,
                Color = Color.FromArgb("#cc9900"),
            },
            StarState.Half => new FontImageSource
            {
                Glyph = "\U000F04D0",
                FontFamily = "MaterialDesignIcons",
                Size = 22,
                Color = Color.FromArgb("#cc9900"),
            },
            StarState.Full => new FontImageSource
            {
                Glyph = "\U000F04CE",
                FontFamily = "MaterialDesignIcons",
                Size = 22,
                Color = Color.FromArgb("#cc9900"),
            },
            _ => throw new NotImplementedException(),
        };
    }

    private static double ClampValue(double value)
    {
        if (value < 0)
        {
            return 0;
        }

        if (value > MAXVALUE)
        {
            return MAXVALUE;
        }

        return value;
    }

    private void SetStars()
    {
        var starLayout = this.view.starLayout;
        starLayout.Children.Clear();

        var intValue = (int)ClampValue(this.CurrentValue);

        for (int i = 0; i <= MAXVALUE; i++)
        {
            if (i == 0)
            {
                starLayout.Add(this.CreateButton(i), i, 0);
            }
            else
            {
                if (intValue >= i)
                {
                    starLayout.Add(this.CreateButton(StarState.Full, i), i, 0);
                }
                else
                {
                    starLayout.Add(this.CreateButton(StarState.Empty, i), i, 0);
                }
            }
        }
    }

    private Button CreateButton(int index)
    {
        var button = new Button
        {
            Text = string.Empty,
            Command = this.StarsClicked(index),
            Background = Colors.Transparent,
            MaximumWidthRequest = 35,
        };

        return button;
    }

    private ImageButton CreateButton(StarState state, int index)
    {
        var button = new ImageButton()
        {
            Source = CreateStarLabel(state),
            Command = this.StarsClicked(index),
            Background = Colors.Transparent,
        };

        button.SetDynamicResource(Button.StyleProperty, "RatingButton");

        return button;
    }

    private Command StarsClicked(int index)
    {
        return new Command(() => this.Stars_Clicked(index));
    }

    private void Stars_Clicked(int index)
    {
        this.CurrentValue = index;

        this.SetStars();
    }

    private void Stars_Clicked(object sender, EventArgs e)
    {
        this.CurrentValue = 0;

        this.SetStars();
    }
}