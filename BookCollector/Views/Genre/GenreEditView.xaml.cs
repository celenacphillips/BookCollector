using BookCollector.Data.Models;
using BookCollector.ViewModels.Genre;

namespace BookCollector.Views.Genre;

public partial class GenreEditView : ContentPage
{
    private GenreEditViewModel _viewModel { get; set; }

    public GenreEditView(GenreModel genre, string viewTitle)
	{
        GenreEditViewModel viewModel = new GenreEditViewModel(genre, this);
        viewModel.ViewTitle = viewTitle;
        _viewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();
	}

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        _viewModel.SetViewModelData();
    }
}