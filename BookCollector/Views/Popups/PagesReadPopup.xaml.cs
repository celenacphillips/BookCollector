using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace BookCollector.Views.Popups;

public partial class PagesReadPopup : Popup<int>
{
	public double PopupWidth { get; set; }

    public int PagesRead { get; set; }

    public int PageTotal { get; set; }

    public PagesReadPopup(double popupWidth, int pagesRead, int pageTotal)
	{
		this.PopupWidth = popupWidth;
		this.PagesRead = pagesRead;
		this.PageTotal = pageTotal;

        BindingContext = this;

        InitializeComponent();

        var label = this.FindByName<Label>("PagesReadLabel");
        label.Text = $"{this.PagesRead}";
    }

	public async void OnClose(object? sender, EventArgs e)
	{
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await CloseAsync(this.PagesRead, token: cts.Token);
    }

    public void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
    {
        int value = (int)args.NewValue;
        var label = this.FindByName<Label>("PagesReadLabel");
        label.Text = $"{value}";
    }
}