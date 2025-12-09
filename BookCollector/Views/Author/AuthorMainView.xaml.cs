using BookCollector.Data.Models;
using BookCollector.ViewModels.Author;

namespace BookCollector.Views.Author;

public partial class AuthorMainView : ContentPage
{
    private AuthorMainViewModel ViewModel;

    public AuthorMainView(AuthorModel author, string viewTitle)
	{
        var viewModel = new AuthorMainViewModel(author, this)
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
        ViewModel.SetViewModelData();
    }
}