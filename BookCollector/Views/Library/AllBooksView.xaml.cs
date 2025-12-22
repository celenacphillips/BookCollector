// <copyright file="AllBooksView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class AllBooksView : ContentPage
{
    public AllBooksView()
    {
        this.ViewModel = new AllBooksViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private AllBooksViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        var variable = this.ViewModel.SetViewModelData();
    }
}