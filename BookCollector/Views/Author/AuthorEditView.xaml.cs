using BookCollector.Data.Models;
using BookCollector.ViewModels.Author;

namespace BookCollector.Views.Author;

public partial class AuthorEditView : ContentPage
{
    private AuthorEditViewModel _viewModel { get; set; }

    public AuthorEditView(AuthorModel author, string viewTitle)
	{
        AuthorEditViewModel viewModel = new AuthorEditViewModel(author, this);
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