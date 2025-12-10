using CommunityToolkit.Maui.Views;

namespace BookCollector.Views.Popups;

public partial class BookCoverPopup : Popup
{
    public BookCoverPopup(ImageSource? bookCover)
    {
        this.BookCover = bookCover;

        this.BindingContext = this;

        this.InitializeComponent();
    }

    public ImageSource? BookCover { get; set; }
}