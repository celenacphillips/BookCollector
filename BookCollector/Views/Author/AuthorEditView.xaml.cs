using BookCollector.Data.Models;
using BookCollector.ViewModels.Author;

namespace BookCollector.Views.Author;

public partial class AuthorEditView : ContentPage
{
    public AuthorEditView(AuthorModel author, string viewTitle)
    {
        this.ViewModel = new AuthorEditViewModel(author, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private AuthorEditViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        this.ViewModel.SetViewModelData();
    }
}