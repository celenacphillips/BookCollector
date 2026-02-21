using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Popups;

namespace BookCollector.Views.Controls;

public partial class FilterablePickerOverlay : ContentView
{
    public List<string>? Items { get; set; }

    public string? SelectedItem { get; set; }

    public bool ShowFilter { get; set; }

    public string Title { get; set; }

    public bool IsOverlayVisible { get; set; }

    public FilterPopupViewModel ViewModel { get; set; }

    public FilterablePickerOverlay(FilterPopupViewModel viewModel, string title, List<string>? items, string? selectedItem, bool showFilter, bool isOverlayVisible)
    {
        this.Items = items;
        this.SelectedItem = selectedItem;
        this.Title = title;
        this.ShowFilter = showFilter;
        this.IsOverlayVisible = isOverlayVisible;
        this.ViewModel = viewModel;

        this.InitializeComponent();
    }

    private async void OnOverlayTapped(object sender, EventArgs e)
    {
        this.ViewModel.OverlaySection.Remove(this);
        this.IsOverlayVisible = false;
    }

    public void SelectionChanged(string previous, string current)
    {
        if (!current.Equals(previous))
        {
            if (this.Title.Equals(AppStringResources.Favorite))
            {
                this.ViewModel.FavoriteOption = current;
            }

            if (this.Title.Equals(AppStringResources.Authors))
            {
                this.ViewModel.AuthorOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookPublisher))
            {
                this.ViewModel.PublisherOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookPublishYear))
            {
                this.ViewModel.PublishYearOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookFormat))
            {
                this.ViewModel.FormatOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookLanguage))
            {
                this.ViewModel.LanguageOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookRating))
            {
                this.ViewModel.RatingOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookLocation))
            {
                this.ViewModel.LocationOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookSeries))
            {
                this.ViewModel.SeriesOption = current;
            }

            if (this.Title.Equals(AppStringResources.BookCover))
            {
                this.ViewModel.BookCoverOption = current;
            }

            this.OnOverlayTapped(null, null);
        }
    }
}