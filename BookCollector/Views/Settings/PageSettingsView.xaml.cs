// <copyright file="PageSettingsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Settings;

using BookCollector.Data;
using BookCollector.Data.Enums;
using BookCollector.Resources.Localization;
using BookCollector.Views.Controls;
using System.Collections.ObjectModel;

/// <summary>
/// PageSettingsView class.
/// </summary>
public partial class PageSettingsView : ContentPage
{
    private readonly AppShell appShell;

    /********************************************************/

    private bool readingOnField;

    private bool toBeReadOnField;

    private bool readOnField;

    private bool allBooksOnField;

    private bool collectionsOnField;

    private bool genresOnField;

    private bool seriesOnField;

    private bool authorsOnField;

    private bool locationsOnField;

    /********************************************************/

    /// <summary>
    /// Initializes a new instance of the <see cref="PageSettingsView"/> class.
    /// </summary>
    public PageSettingsView()
    {
        this.ReadingOn = DevicePreferences.ReadingViewShowValue;
        this.ToBeReadOn = DevicePreferences.ToBeReadViewShowValue;
        this.ReadOn = DevicePreferences.ReadViewShowValue;
        this.AllBooksOn = DevicePreferences.AllBooksViewShowValue;

        this.CollectionsOn = DevicePreferences.CollectionsViewShowValue;
        this.GenresOn = DevicePreferences.GenresViewShowValue;
        this.SeriesOn = DevicePreferences.SeriesViewShowValue;
        this.AuthorsOn = DevicePreferences.AuthorsViewShowValue;
        this.LocationsOn = DevicePreferences.LocationsViewShowValue;

        this.appShell = (AppShell)Application.Current?.Windows[0].Page!;

        this.AddLibraryTabViews();
        this.AddGroupingTabViews();

        this.InitializeComponent();
        this.BindingContext = this;
    }

    /********************************************************/

    /// <summary>
    /// Gets or sets the library tab views.
    /// </summary>
    public ObservableCollection<HorizontalStackLayout> LibraryTabViews { get; set; }

    /// <summary>
    /// Gets or sets the grouping tab views.
    /// </summary>
    public ObservableCollection<HorizontalStackLayout> GroupingTabViews { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the reading toggle is on.
    /// </summary>
    public bool ReadingOn
    {
        get => this.readingOnField;
        set
        {
            if (this.readingOnField != value)
            {
                this.readingOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the to be read toggle is on.
    /// </summary>
    public bool ToBeReadOn
    {
        get => this.toBeReadOnField;
        set
        {
            if (this.toBeReadOnField != value)
            {
                this.toBeReadOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the read toggle is on.
    /// </summary>
    public bool ReadOn
    {
        get => this.readOnField;
        set
        {
            if (this.readOnField != value)
            {
                this.readOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the all books toggle is on.
    /// </summary>
    public bool AllBooksOn
    {
        get => this.allBooksOnField;
        set
        {
            if (this.allBooksOnField != value)
            {
                this.allBooksOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the collections toggle is on.
    /// </summary>
    public bool CollectionsOn
    {
        get => this.collectionsOnField;
        set
        {
            if (this.collectionsOnField != value)
            {
                this.collectionsOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the genres toggle is on.
    /// </summary>
    public bool GenresOn
    {
        get => this.genresOnField;
        set
        {
            if (this.genresOnField != value)
            {
                this.genresOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the series toggle is on.
    /// </summary>
    public bool SeriesOn
    {
        get => this.seriesOnField;
        set
        {
            if (this.seriesOnField != value)
            {
                this.seriesOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the authors toggle is on.
    /// </summary>
    public bool AuthorsOn
    {
        get => this.authorsOnField;
        set
        {
            if (this.authorsOnField != value)
            {
                this.authorsOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the locations toggle is on.
    /// </summary>
    public bool LocationsOn
    {
        get => this.locationsOnField;
        set
        {
            if (this.locationsOnField != value)
            {
                this.locationsOnField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /********************************************************/

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnReadingToggled(object? sender, ToggledEventArgs? e)
    {
        if (e != null)
        {
            Preferences.Set(DevicePreferences.ReadingViewShow, e.Value);
            DevicePreferences.ReadingViewShowValue = e.Value;
            this.appShell.ResetLibraryTab();
        }
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnToBeReadToggled(object? sender, ToggledEventArgs? e)
    {
        if (e != null)
        {
            Preferences.Set(DevicePreferences.ToBeReadViewShow, e.Value);
            DevicePreferences.ToBeReadViewShowValue = e.Value;
            this.appShell.ResetLibraryTab();
        }
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnReadToggled(object? sender, ToggledEventArgs? e)
    {
        if (e != null)
        {
            Preferences.Set(DevicePreferences.ReadViewShow, e.Value);
            DevicePreferences.ReadViewShowValue = e.Value;
            this.appShell.ResetLibraryTab();
        }
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnAllBooksToggled(object? sender, ToggledEventArgs? e)
    {
        if (e != null)
        {
            Preferences.Set(DevicePreferences.AllBooksViewShow, e.Value);
            DevicePreferences.AllBooksViewShowValue = e.Value;
            this.appShell.ResetLibraryTab();
        }
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnCollectionsToggled(object? sender, ToggledEventArgs? e)
    {
        if (e != null)
        {
            Preferences.Set(DevicePreferences.CollectionsViewShow, e.Value);
            DevicePreferences.CollectionsViewShowValue = e.Value;
            this.appShell.ResetGroupingsTab();
        }
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnGenresToggled(object? sender, ToggledEventArgs? e)
    {
        if (e != null)
        {
            Preferences.Set(DevicePreferences.GenresViewShow, e.Value);
            DevicePreferences.GenresViewShowValue = e.Value;
            this.appShell.ResetGroupingsTab();
        }
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnSeriesToggled(object? sender, ToggledEventArgs? e)
    {
        if (e != null)
        {
            Preferences.Set(DevicePreferences.SeriesViewShow, e.Value);
            DevicePreferences.SeriesViewShowValue = e.Value;
            this.appShell.ResetGroupingsTab();
        }
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnAuthorsToggled(object? sender, ToggledEventArgs? e)
    {
        if (e != null)
        {
            Preferences.Set(DevicePreferences.AuthorsViewShow, e.Value);
            DevicePreferences.AuthorsViewShowValue = e.Value;
            this.appShell.ResetGroupingsTab();
        }
    }

    /// <summary>
    /// Saves the value of the toggle to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnLocationsToggled(object? sender, ToggledEventArgs? e)
    {
        if (e != null)
        {
            Preferences.Set(DevicePreferences.LocationsViewShow, e.Value);
            DevicePreferences.LocationsViewShowValue = e.Value;
            this.appShell.ResetGroupingsTab();
        }
    }

    /// <summary>
    /// Resets the toggles values.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnLibraryTabResetButton_Clicked(object sender, EventArgs e)
    {
        this.ReadingOn = DevicePreferenceDefaults.ReadingViewShowDefault;
        Preferences.Set(DevicePreferences.ReadingViewShow, DevicePreferenceDefaults.ReadingViewShowDefault);
        DevicePreferences.ReadingViewShowValue = DevicePreferenceDefaults.ReadingViewShowDefault;

        this.ToBeReadOn = DevicePreferenceDefaults.ToBeReadViewShowDefault;
        Preferences.Set(DevicePreferences.ToBeReadViewShow, DevicePreferenceDefaults.ToBeReadViewShowDefault);
        DevicePreferences.ToBeReadViewShowValue = DevicePreferenceDefaults.ToBeReadViewShowDefault;

        this.ReadOn = DevicePreferenceDefaults.ReadViewShowDefault;
        Preferences.Set(DevicePreferences.ReadViewShow, DevicePreferenceDefaults.ReadViewShowDefault);
        DevicePreferences.ReadViewShowValue = DevicePreferenceDefaults.ReadViewShowDefault;

        this.AllBooksOn = DevicePreferenceDefaults.AllBooksViewShowDefault;
        Preferences.Set(DevicePreferences.AllBooksViewShow, DevicePreferenceDefaults.AllBooksViewShowDefault);
        DevicePreferences.AllBooksViewShowValue = DevicePreferenceDefaults.AllBooksViewShowDefault;

        Preferences.Set(DevicePreferences.LibraryTabViewsOrder, DevicePreferenceDefaults.LibraryTabViewsOrderDefault);
        DevicePreferences.LibraryTabViewsOrderValue = DevicePreferenceDefaults.LibraryTabViewsOrderDefault;

        this.LibraryTabViews.Clear();
        this.AddLibraryTabViews();
        this.libraryViewsDragAndDropContainer.DragAndDropItems = this.LibraryTabViews;
        this.appShell.ResetLibraryTab();
    }

    /// <summary>
    /// Resets the toggles values.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnGroupingsTabResetButton_Clicked(object sender, EventArgs e)
    {
        this.CollectionsOn = DevicePreferenceDefaults.CollectionsViewShowDefault;
        Preferences.Set(DevicePreferences.CollectionsViewShow, DevicePreferenceDefaults.CollectionsViewShowDefault);
        DevicePreferences.CollectionsViewShowValue = DevicePreferenceDefaults.CollectionsViewShowDefault;

        this.GenresOn = DevicePreferenceDefaults.GenresViewShowDefault;
        Preferences.Set(DevicePreferences.GenresViewShow, DevicePreferenceDefaults.GenresViewShowDefault);
        DevicePreferences.GenresViewShowValue = DevicePreferenceDefaults.GenresViewShowDefault;

        this.SeriesOn = DevicePreferenceDefaults.SeriesViewShowDefault;
        Preferences.Set(DevicePreferences.SeriesViewShow, DevicePreferenceDefaults.SeriesViewShowDefault);
        DevicePreferences.SeriesViewShowValue = DevicePreferenceDefaults.SeriesViewShowDefault;

        this.AuthorsOn = DevicePreferenceDefaults.AuthorsViewShowDefault;
        Preferences.Set(DevicePreferences.AuthorsViewShow, DevicePreferenceDefaults.AuthorsViewShowDefault);
        DevicePreferences.AuthorsViewShowValue = DevicePreferenceDefaults.AuthorsViewShowDefault;

        this.LocationsOn = DevicePreferenceDefaults.LocationsViewShowDefault;
        Preferences.Set(DevicePreferences.LocationsViewShow, DevicePreferenceDefaults.LocationsViewShowDefault);
        DevicePreferences.LocationsViewShowValue = DevicePreferenceDefaults.LocationsViewShowDefault;

        Preferences.Set(DevicePreferences.GroupingsTabViewsOrder, DevicePreferenceDefaults.GroupingsTabViewsOrderDefault);
        DevicePreferences.GroupingsTabViewOrderValue = DevicePreferenceDefaults.GroupingsTabViewsOrderDefault;

        this.GroupingTabViews.Clear();
        this.AddGroupingTabViews();
        this.groupingsViewsDragAndDropContainer.DragAndDropItems = this.GroupingTabViews;
        this.appShell.ResetGroupingsTab();
    }

    /// <summary>
    /// Handles the event when the library tab views are updated and saves the new order to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnLibraryTabViewsUpdated(object sender, EventArgs e)
    {
        var control = (DragAndDropContainer)sender;

        var tabOrder = string.Empty;

        foreach (HorizontalStackLayout item in control.DragAndDropItems)
        {
            var label = (Label)item.Children[0];
            tabOrder += $"{label.Text},";
        }

        Preferences.Set(DevicePreferences.LibraryTabViewsOrder, tabOrder);
        DevicePreferences.LibraryTabViewsOrderValue = tabOrder;

        this.appShell.ResetLibraryTab();
    }

    /// <summary>
    /// Handles the event when the groupings tab views are updated and saves the new order to preferences.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public void OnGroupingsTabViewsUpdated(object sender, EventArgs e)
    {
        var control = (DragAndDropContainer)sender;

        var tabOrder = string.Empty;

        foreach (HorizontalStackLayout item in control.DragAndDropItems)
        {
            var label = (Label)item.Children[0];
            tabOrder += $"{label.Text},";
        }

        Preferences.Set(DevicePreferences.GroupingsTabViewsOrder, tabOrder);
        DevicePreferences.GroupingsTabViewOrderValue = tabOrder;

        this.appShell.ResetGroupingsTab();
    }

    /********************************************************/

    private static HorizontalStackLayout SetUpTabs(string labelText, bool isToggled, EventHandler<ToggledEventArgs>? toggled)
    {
        var label = new Label
        {
            Text = labelText,
            VerticalTextAlignment = TextAlignment.Center,
        };

        var toggle = new Switch
        {
            VerticalOptions = LayoutOptions.Start,
            Margin = new Thickness(0, -5, 0, -5),
            IsToggled = isToggled,
        };

        toggle.Toggled += toggled;

        var horizontalStackLayout = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
        };

        horizontalStackLayout.Children.Add(label);
        horizontalStackLayout.Children.Add(toggle);

        return horizontalStackLayout;
    }

    private void AddLibraryTabViews()
    {
        this.LibraryTabViews ??= [];

        var libraryTabViews = DevicePreferences.LibraryTabViewsOrderValue.Split(",");

        foreach (var libraryTabView in libraryTabViews)
        {
            if (string.IsNullOrEmpty(libraryTabView))
            {
                continue;
            }

            bool isToggled = false;
            EventHandler<ToggledEventArgs>? toggled = null;

            if (libraryTabView.Equals(AppStringResources.Reading))
            {
                isToggled = this.ReadingOn;
                toggled = this.OnReadingToggled;
            }

            if (libraryTabView.Equals(AppStringResources.ToBeRead))
            {
                isToggled = this.ToBeReadOn;
                toggled = this.OnToBeReadToggled;
            }

            if (libraryTabView.Equals(AppStringResources.Read))
            {
                isToggled = this.ReadOn;
                toggled = this.OnReadToggled;
            }

            if (libraryTabView.Equals(AppStringResources.AllBooks))
            {
                isToggled = this.AllBooksOn;
                toggled = this.OnAllBooksToggled;
            }

            this.LibraryTabViews.Add(SetUpTabs(libraryTabView, isToggled, toggled));
        }
    }

    private void AddGroupingTabViews()
    {
        this.GroupingTabViews ??= [];

        var groupingsTabViews = DevicePreferences.GroupingsTabViewOrderValue.Split(",");

        foreach (var groupingsTabView in groupingsTabViews)
        {
            if (string.IsNullOrEmpty(groupingsTabView))
            {
                continue;
            }

            bool isToggled = false;
            EventHandler<ToggledEventArgs>? toggled = null;

            if (groupingsTabView.Equals(AppStringResources.Collections))
            {
                isToggled = this.CollectionsOn;
                toggled = this.OnCollectionsToggled;
            }

            if (groupingsTabView.Equals(AppStringResources.Genres))
            {
                isToggled = this.GenresOn;
                toggled = this.OnGenresToggled;
            }

            if (groupingsTabView.Equals(AppStringResources.Series))
            {
                isToggled = this.SeriesOn;
                toggled = this.OnSeriesToggled;
            }

            if (groupingsTabView.Equals(AppStringResources.Authors))
            {
                isToggled = this.AuthorsOn;
                toggled = this.OnAuthorsToggled;
            }

            if (groupingsTabView.Equals(AppStringResources.Locations))
            {
                isToggled = this.LocationsOn;
                toggled = this.OnLocationsToggled;
            }

            this.GroupingTabViews.Add(SetUpTabs(groupingsTabView, isToggled, toggled));
        }
    }
}