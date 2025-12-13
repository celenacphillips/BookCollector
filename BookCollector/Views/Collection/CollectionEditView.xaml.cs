using BookCollector.Data.Models;
using BookCollector.ViewModels.Collection;

namespace BookCollector.Views.Collection;

public partial class CollectionEditView : ContentPage
{
    public CollectionEditView(CollectionModel collection, string viewTitle, bool insertMainViewBefore = false)
    {
        this.ViewModel = new CollectionEditViewModel(collection, this)
        {
            ViewTitle = viewTitle,
            InsertMainViewBefore = insertMainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private CollectionEditViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.ViewModel.SetViewModelData();
    }
}