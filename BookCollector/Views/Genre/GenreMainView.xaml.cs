using BookCollector.Data.Models;
using BookCollector.ViewModels.Genre;

namespace BookCollector.Views.Genre;

public partial class GenreMainView : ContentPage
{
    private GenreMainViewModel ViewModel;

    public GenreMainView(GenreModel genre, string viewTitle)
	{
        var viewModel = new GenreMainViewModel(genre, this)
        {
            ViewTitle = viewTitle
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