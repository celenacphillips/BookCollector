using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ReadView : ContentPage
{
    public ReadView()
    {
        this.ViewModel = new ReadViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private ReadViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.ViewModel.SetViewModelData();
    }
}