using CommunityToolkit.Maui.Views;

namespace BookCollector.Views.Popups;

public partial class BookCoverPopup : Popup
{
	public ImageSource? BookCover { get; set; }
    public BookCoverPopup(ImageSource? bookCover)
	{
		this.BookCover = bookCover;

        BindingContext = this;

        InitializeComponent();
	}
}