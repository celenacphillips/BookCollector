// <copyright file="BookMainView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;

namespace BookCollector.Views.Book;

public partial class BookMainView : ContentPage
{
    private BookMainViewModel viewModel;

    public BookMainView(BookModel book, string viewTitle)
    {
        this.viewModel = new BookMainViewModel(book, this)
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