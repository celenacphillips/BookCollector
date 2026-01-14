using CommunityToolkit.Maui.Storage;

namespace BookCollector.Views.Support;

public partial class SupportView : ContentPage
{
    public SupportView()
    {
        this.InitializeComponent();
        this.BindingContext = this;
    }

    public async void OnEmailButtonClicked(object sender, EventArgs e)
    {
        if (Email.Default.IsComposeSupported)
        {
            string subject = "Book Collector Support";
            string body = $"AppVersion: {AppInfo.VersionString}\n";
            string[] recipients = new[] { "bookcollector.support@protonmail.com" };

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string>(recipients),
            };

            await Email.Default.ComposeAsync(message);
        }
    }
}