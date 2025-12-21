// <copyright file="ToBeReadView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Library;

namespace BookCollector.Views.Library;

public partial class ToBeReadView : ContentPage
{
    public ToBeReadView()
    {
        this.ViewModel = new ToBeReadViewModel(this);
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private ToBeReadViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.ViewModel.SetViewModelData();
    }
}