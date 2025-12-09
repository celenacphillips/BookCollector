using BookCollector.Data.Models;
using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class CollectionsView : ContentPage
{
    private CollectionsViewModel ViewModel { get; set; }

    public CollectionsView()
	{
        var viewModel = new CollectionsViewModel(this);
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