using BookCollector.Data.Enums;

namespace BookCollector.Views.Controls;

public partial class Favorite : ContentView
{
    private readonly Favorite _view;

    public static readonly BindableProperty CurrentValueProperty =
             BindableProperty.Create(
                 nameof(CurrentValue),
                 typeof(bool),
                 typeof(Favorite),
                 propertyChanged: OnRefreshControl);

    public bool CurrentValue
    {
        get => (bool)GetValue(CurrentValueProperty);
        set => SetValue(CurrentValueProperty, value);
    }

    public Favorite()
    {
        _view = this;
        InitializeComponent();
        SetHeart();
    }

    private static void OnRefreshControl(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Favorite favorite)
            favorite.SetHeart();
    }

    private void SetHeart()
    {
        var heart = _view.heart;

        if (!CurrentValue)
            heart.Source = CreateHeartLabel(HeartState.Empty);
        else
            heart.Source = CreateHeartLabel(HeartState.Full);
    }

    private ImageSource CreateHeartLabel(HeartState state)
    {
        return state switch
        {
            HeartState.Empty => Application.Current.UserAppTheme switch
            {
                AppTheme.Dark => ImageSource.FromFile("Icons/heart_icon_empty_dark.svg"),
                _ => ImageSource.FromFile("Icons/heart_icon_empty_light.svg")
            },
            HeartState.Full => Application.Current.UserAppTheme switch
            {
                AppTheme.Dark => ImageSource.FromFile("Icons/heart_icon_full_dark.svg"),
                _ => ImageSource.FromFile("Icons/heart_icon_full_light.svg"),
            },
            _ => throw new NotImplementedException(),
        };
    }

    private void heart_Clicked(object sender, EventArgs e)
    {
        if (!CurrentValue)
            CurrentValue = true;
        else
            CurrentValue = false;

        SetHeart();
    }
}