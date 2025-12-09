using BookCollector.Data.Models;
using BookCollector.ViewModels.Genre;

namespace BookCollector.Views.Genre;

public partial class GenreEditView : ContentPage
{
    private GenreEditViewModel ViewModel { get; set; }

    public GenreEditView(GenreModel genre, string viewTitle, bool insertMainViewBefore = false)
	{
        var viewModel = new GenreEditViewModel(genre, this)
        {
            ViewTitle = viewTitle,
            InsertMainViewBefore = insertMainViewBefore
        };
        ViewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var _ = ViewModel.SetViewModelData();
    }
}