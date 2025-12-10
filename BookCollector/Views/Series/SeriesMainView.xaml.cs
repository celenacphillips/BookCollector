using BookCollector.Data.Models;
using BookCollector.ViewModels.Series;

namespace BookCollector.Views.Series;

public partial class SeriesMainView : ContentPage
{
    private SeriesMainViewModel viewModel;

    public SeriesMainView(SeriesModel series, string viewTitle)
    {
        this.viewModel = new SeriesMainViewModel(series, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.viewModel.SetViewModelData();
    }
}