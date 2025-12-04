using CommunityToolkit.Maui.Views;

namespace BookCollector.Views.Popups;

public partial class InformationPopup : Popup
{
    public double PopupWidth { get; set; }
	public string InfoText { get; set; }
	public double PopupHeight = 190;

    public InformationPopup(double popupWidth, string infoText)
	{
		this.PopupWidth = popupWidth;
		this.InfoText = infoText;

		BindingContext = this;

		InitializeComponent();
		
	}
}