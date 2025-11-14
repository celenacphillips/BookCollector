using BookCollector.Data;
using BookCollector.Data.Enums;
using System.Runtime.ConstrainedExecution;
using System.Windows.Input;

namespace BookCollector.Views.Controls;

public partial class Rating : ContentView
{
    private readonly Rating _view;
    private const int MAX_VALUE = 5;

    public static readonly BindableProperty CurrentValueProperty =
             BindableProperty.Create(
                 nameof(CurrentValue),
                 typeof(int),
                 typeof(Rating),
                 propertyChanged: OnRefreshControl);

    public int CurrentValue
    {
        get => (int)GetValue(CurrentValueProperty);
        set => SetValue(CurrentValueProperty, value);
    }

    public Rating()
    {
        _view = this;
        InitializeComponent();
        SetStars();
    }

    private static void OnRefreshControl(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Rating rating)
        {
            rating.SetStars();
        }

    }

    private void SetStars()
    {
        var starLayout = _view.starLayout;
        starLayout.Children.Clear();

        var intValue = (int)ClampValue(CurrentValue);

        for (int i = 1; i <= MAX_VALUE; i++)
        {
            if (intValue >= i)
            {
                starLayout.Add(CreateButton(StarState.Full, i));
            }
            else
            {
                starLayout.Add(CreateButton(StarState.Empty, i));
            }
        }
    }

    private ImageButton CreateButton(StarState state, int index)
    {
        return new ImageButton()
        {
            Source = CreateStarLabel(state),
            Command = StarsClicked(index),
            Background = Colors.Transparent,
        };
    }

    private ICommand StarsClicked(int index)
    {
        return new Command(() => stars_Clicked(index));
    }

    private ImageSource CreateStarLabel(StarState state)
    {
        return state switch
        {
            StarState.Empty => Application.Current.PlatformAppTheme switch
            {
                AppTheme.Dark => ImageSource.FromFile("Icons/star_icon_empty_dark.svg"),
                _ => ImageSource.FromFile("Icons/star_icon_empty_light.svg"),
            },
            StarState.Full => Application.Current.PlatformAppTheme switch
            {
                AppTheme.Dark => ImageSource.FromFile("Icons/star_icon_full_dark.svg"),
                _ => ImageSource.FromFile("Icons/star_icon_full_light.svg"),
            },
            _ => throw new NotImplementedException(),
        };
    }

    private void stars_Clicked(int index)
    {
        CurrentValue = index;

        SetStars();
    }

    private void stars_Clicked(object sender, EventArgs e)
    {
        CurrentValue = 0;

        SetStars();
    }
    private double ClampValue(double value)
    {
        if (value < 0)
            return 0;

        if (value > MAX_VALUE)
            return MAX_VALUE;

        return value;
    }
}