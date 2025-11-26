using BookCollector.Data.Models;
using BookCollector.ViewModels.Collection;

namespace BookCollector.Views.Collection;

public partial class CollectionMainView : ContentPage
{
    private CollectionMainViewModel _viewModel;

    public CollectionMainView(CollectionModel collection, string viewTitle)
	{
        CollectionMainViewModel viewModel = new CollectionMainViewModel(collection, this);
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