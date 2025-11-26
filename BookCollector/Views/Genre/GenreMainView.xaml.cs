using BookCollector.Data.Models;
using BookCollector.ViewModels.Genre;

namespace BookCollector.Views.Genre;

public partial class GenreMainView : ContentPage
{
    private GenreMainViewModel _viewModel;

    public GenreMainView(GenreModel genre, string viewTitle)
	{
        GenreMainViewModel viewModel = new GenreMainViewModel(genre, this);
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