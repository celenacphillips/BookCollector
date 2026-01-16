// <copyright file="ExistingBooksView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class ExistingBooksView : ContentPage
{
    private ExistingBooksViewModel viewModel;

    public ExistingBooksView(object selected, string viewTitle, object previousViewModel)
    {
        this.viewModel = new ExistingBooksViewModel(selected, this, previousViewModel)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    // Need this to make sure books populate when you
    // navigate back to the view.
    protected override async void OnAppearing()
    {
        await this.viewModel.SetViewModelData();
    }
}