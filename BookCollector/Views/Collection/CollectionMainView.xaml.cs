using BookCollector.Data.Models;
using BookCollector.ViewModels.Collection;

namespace BookCollector.Views.Collection;

public partial class CollectionMainView : ContentPage
{
    private CollectionMainViewModel ViewModel;

    public CollectionMainView(CollectionModel collection, string viewTitle)
	{
        var viewModel = new CollectionMainViewModel(collection, this)
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