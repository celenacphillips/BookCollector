using BookCollector.Data.Models;
using BookCollector.ViewModels.Series;

namespace BookCollector.Views.Series;

public partial class SeriesEditView : ContentPage
{
    private SeriesEditViewModel _viewModel { get; set; }

    public SeriesEditView(SeriesModel series, string viewTitle, bool insertMainViewBefore = false)
	{
        SeriesEditViewModel viewModel = new SeriesEditViewModel(series, this);
        viewModel.ViewTitle = viewTitle;
        viewModel.InsertMainViewBefore = insertMainViewBefore;
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