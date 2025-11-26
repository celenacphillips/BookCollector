using BookCollector.Data.Models;
using BookCollector.ViewModels.Author;

namespace BookCollector.Views.Author;

public partial class AuthorMainView : ContentPage
{
    private AuthorMainViewModel _viewModel;

    public AuthorMainView(AuthorModel author, string viewTitle)
	{
        AuthorMainViewModel viewModel = new AuthorMainViewModel(author, this);
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