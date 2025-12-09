using BookCollector.Data.Models;
using BookCollector.ViewModels.Collection;

namespace BookCollector.Views.Collection;

public partial class CollectionEditView : ContentPage
{
	private CollectionEditViewModel ViewModel {  get; set; }

    public CollectionEditView(CollectionModel collection, string viewTitle, bool insertMainViewBefore = false)
	{
        var viewModel = new CollectionEditViewModel(collection, this)
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