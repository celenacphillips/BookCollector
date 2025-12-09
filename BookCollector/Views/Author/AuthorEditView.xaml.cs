using BookCollector.Data.Models;
using BookCollector.ViewModels.Author;

namespace BookCollector.Views.Author;

public partial class AuthorEditView : ContentPage
{
    private AuthorEditViewModel ViewModel { get; set; }

    public AuthorEditView(AuthorModel author, string viewTitle)
	{
        var viewModel = new AuthorEditViewModel(author, this)
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