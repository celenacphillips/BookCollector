// <copyright file="TimePopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Popups;

using CommunityToolkit.Maui.Views;

/// <summary>
/// TimePopup class.
/// </summary>
public partial class TimePopup : Popup<TimeSpan>
{
    private bool hoursNotValidField;

    private bool minutesNotValidField;

    private bool saveEnabledField;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimePopup"/> class.
    /// </summary>
    /// <param name="title">Title of the popup.</param>
    /// <param name="popupWidth">Max width of the popup.</param>
    /// <param name="hours">Input hours.</param>
    /// <param name="minutes">Input minutes.</param>
    /// <param name="maxHours">Max hours input cannot go over. Default is null.</param>
    /// <param name="maxMinutes">Max minutes input cannot go over. Default is null.</param>
    public TimePopup(string title, double popupWidth, int hours, int minutes, int? maxHours = null, int? maxMinutes = null)
    {
        this.Title = title;
        this.PopupWidth = popupWidth;
        this.Hours = hours;
        this.Minutes = minutes;
        this.MaxHours = maxHours;
        this.MaxMinutes = maxMinutes;

        this.BindingContext = this;

        this.InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the title of the popup.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the pop width.
    /// </summary>
    public double PopupWidth { get; set; }

    /// <summary>
    /// Gets or sets the input hours.
    /// </summary>
    public int Hours { get; set; }

    /// <summary>
    /// Gets or sets the input minutes.
    /// </summary>
    public int Minutes { get; set; }

    /// <summary>
    /// Gets or sets the max hours.
    /// </summary>
    public int? MaxHours { get; set; }

    /// <summary>
    /// Gets or sets the max minutes.
    /// </summary>
    public int? MaxMinutes { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the input hours is valid.
    /// </summary>
    public bool HoursNotValid
    {
        get => this.hoursNotValidField;
        set
        {
            if (this.hoursNotValidField != value)
            {
                this.hoursNotValidField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the input minutes is valid.
    /// </summary>
    public bool MinutesNotValid
    {
        get => this.minutesNotValidField;
        set
        {
            if (this.minutesNotValidField != value)
            {
                this.minutesNotValidField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the save button is enabled.
    /// </summary>
    public bool SaveEnabled
    {
        get => this.saveEnabledField;
        set
        {
            if (this.saveEnabledField != value)
            {
                this.saveEnabledField = value;
                this.OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Called once the close button is clicked. Closes the popup and returns the timespan value.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public async void OnClose(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        var timespan = new TimeSpan(this.Hours, this.Minutes, 0);

        await this.CloseAsync(timespan, token: cts.Token);
    }

    private void HoursEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (int.TryParse(e.NewTextValue, out var hours))
            {
                if (hours == 0 && e.NewTextValue.Equals("0"))
                {
                    this.Hours = hours;
                    this.HoursNotValid = false;
                }
                else
                    if (hours != 0 && hours > 0)
                    {
                        if (this.MaxHours != null)
                        {
                            var timespan = new TimeSpan(this.Hours, this.Minutes, 0);
                            var maxTimespan = new TimeSpan((int)this.MaxHours, this.MaxMinutes ?? 0, 0);

                            if (timespan <= maxTimespan)
                            {
                                this.Hours = hours;
                                this.HoursNotValid = false;
                                this.MinutesNotValid = false;
                            }
                            else
                            {
                                this.HoursNotValid = true;
                                this.MinutesNotValid = true;
                            }
                        }
                        else
                        {
                            this.Hours = hours;
                            this.HoursNotValid = false;
                        }
                    }
                    else
                    {
                        this.HoursNotValid = true;
                    }
            }
            else
            {
                this.HoursNotValid = true;
            }
        }
        else
        {
            this.HoursNotValid = true;
        }

        this.SaveEnabled = !this.HoursNotValid && !this.MinutesNotValid;
    }

    private void MinutesEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (int.TryParse(e.NewTextValue, out var minutes))
            {
                if (minutes == 0 && e.NewTextValue.Equals("0"))
                {
                    this.Minutes = minutes;
                    this.MinutesNotValid = false;
                }
                else
                    if (minutes != 0 && (minutes > 0 && minutes < 60))
                    {
                        if (this.MaxHours != null)
                        {
                            var timespan = new TimeSpan(this.Hours, this.Minutes, 0);
                            var maxTimespan = new TimeSpan((int)this.MaxHours, this.MaxMinutes ?? 0, 0);

                            if (timespan <= maxTimespan)
                            {
                                this.Minutes = minutes;
                                this.MinutesNotValid = false;
                                this.HoursNotValid = false;
                            }
                            else
                            {
                                this.HoursNotValid = true;
                                this.MinutesNotValid = true;
                            }
                        }
                        else
                        {
                            this.Minutes = minutes;
                            this.MinutesNotValid = false;
                        }
                    }
                    else
                    {
                        this.MinutesNotValid = true;
                    }
            }
            else
            {
                this.MinutesNotValid = true;
            }
        }
        else
        {
            this.MinutesNotValid = true;
        }

        this.SaveEnabled = !this.HoursNotValid && !this.MinutesNotValid;
    }
}