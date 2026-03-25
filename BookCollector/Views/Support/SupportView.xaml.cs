// <copyright file="SupportView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Support;

/// <summary>
/// SupportView class.
/// </summary>
public partial class SupportView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SupportView"/> class.
    /// </summary>
    public SupportView()
    {
        this.InitializeComponent();
        this.BindingContext = this;
    }

    /// <summary>
    /// Formats an email to support with the app version and opens the default email client to send the email.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event.</param>
    public async void OnEmailButtonClicked(object sender, EventArgs e)
    {
        if (Email.Default.IsComposeSupported)
        {
            string subject = "Book Collector Support";
            string body = $"AppVersion: {AppInfo.VersionString}\n";
            string[] recipients = ["bookcollector.support@protonmail.com"];

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = [.. recipients],
            };

            await Email.Default.ComposeAsync(message);
        }
    }
}