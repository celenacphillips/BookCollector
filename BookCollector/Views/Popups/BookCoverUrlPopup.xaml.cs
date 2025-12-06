using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace BookCollector.Views.Popups;

public partial class BookCoverUrlPopup : Popup
{
	public double PopupWidth { get; set; }

	public string? BookCoverUrl { get; set; }

	public BookCoverUrlPopup(double popupWidth, string? bookCoverUrl)
	{
		this.PopupWidth = popupWidth;
		this.BookCoverUrl = bookCoverUrl;

        BindingContext = this;

        InitializeComponent();
    }

	async void OnClose(object sender, EventArgs e)
	{
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await this.CloseAsync(this.BookCoverUrl, token: cts.Token);
    }
}