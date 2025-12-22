// <copyright file="ExistingBooksView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Groupings;

namespace BookCollector.Views.Groupings;

public partial class ExistingBooksView : ContentPage
{
    private ExistingBooksViewModel viewModel;

    public ExistingBooksView(object selected, string viewTitle)
    {
        this.viewModel = new ExistingBooksViewModel(selected, this)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    // Need this to make sure books populate when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        var variable = this.viewModel.SetViewModelData();
    }
}