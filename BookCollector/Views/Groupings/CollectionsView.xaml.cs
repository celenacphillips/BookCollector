using BookCollector.Data.Models;
using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class CollectionsView : ContentPage
{
    private CollectionsViewModel _viewModel { get; set; }

    public CollectionsView()
	{
        CollectionsViewModel viewModel = new CollectionsViewModel(this);
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