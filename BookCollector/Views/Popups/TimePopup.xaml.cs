// <copyright file="PagesReadPopup.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Views;
using DocumentFormat.OpenXml.Bibliography;

namespace BookCollector.Views.Popups;

public partial class TimePopup : Popup<TimeSpan>
{
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

    public string Title { get; set; }

    public double PopupWidth { get; set; }

    public int Hours { get; set; }

    public int Minutes { get; set; }

    public int? MaxHours { get; set; }

    public int? MaxMinutes { get; set; }

    private bool hoursNotValidField;

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

    private bool minutesNotValidField;

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

    private bool saveEnabledField;

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
            int.TryParse(e.NewTextValue, out var hours);

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
                    var maxTimespan = new TimeSpan((int)this.MaxHours, (int)this.MaxMinutes, 0);

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

        this.SaveEnabled = !this.HoursNotValid && !this.MinutesNotValid;
    }

    private void MinutesEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            int.TryParse(e.NewTextValue, out var minutes);

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
                    var maxTimespan = new TimeSpan((int)this.MaxHours, (int)this.MaxMinutes, 0);

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

        this.SaveEnabled = !this.HoursNotValid && !this.MinutesNotValid;
    }
}