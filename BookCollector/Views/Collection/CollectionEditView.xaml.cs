using BookCollector.Data.Models;
using BookCollector.ViewModels.Collection;

namespace BookCollector.Views.Collection;

public partial class CollectionEditView : ContentPage
{
	private CollectionEditViewModel _viewModel {  get; set; }

    public CollectionEditView(CollectionModel collection, string viewTitle)
	{
        CollectionEditViewModel viewModel = new CollectionEditViewModel(collection, this);
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