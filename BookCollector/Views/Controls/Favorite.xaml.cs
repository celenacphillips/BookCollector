// <copyright file="Favorite.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Enums;

namespace BookCollector.Views.Controls;

public partial class Favorite : ContentView
{
    public static readonly BindableProperty CurrentValueProperty =
             BindableProperty.Create(
                 nameof(CurrentValue),
                 typeof(bool),
                 typeof(Favorite),
                 propertyChanged: OnRefreshControl);

    private readonly Favorite view;

    public Favorite()
    {
        this.view = this;
        this.InitializeComponent();
        this.SetHeart();
    }

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

    private static ImageSource CreateHeartLabel(HeartState state)
    {
        return state switch
        {
            HeartState.Empty => Application.Current?.UserAppTheme switch
            {
                AppTheme.Dark => ImageSource.FromFile("Icons/heart_icon_empty_dark.svg"),
                _ => ImageSource.FromFile("Icons/heart_icon_empty_light.svg")
            },
            HeartState.Full => Application.Current?.UserAppTheme switch
            {
                AppTheme.Dark => ImageSource.FromFile("Icons/heart_icon_full_dark.svg"),
                _ => ImageSource.FromFile("Icons/heart_icon_full_light.svg"),
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