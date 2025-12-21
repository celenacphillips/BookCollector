// <copyright file="BookEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;

namespace BookCollector.Views.Book;

public partial class BookEditView : ContentPage
{
    public BookEditView(BookModel book, string viewTitle, bool removeMainViewBefore = false, BookMainView? mainViewBefore = null)
    {
        this.ViewModel = new BookEditViewModel(book, this)
        {
            ViewTitle = viewTitle,
            RemoveMainViewBefore = removeMainViewBefore,
            MainViewBefore = mainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();
    }

    private BookEditViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        using var variable = this.ViewModel.SetViewModelData();
    }
}