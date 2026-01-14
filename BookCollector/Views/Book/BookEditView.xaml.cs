// <copyright file="BookEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Book;

namespace BookCollector.Views.Book;

public partial class BookEditView : ContentPage
{
    public BookEditView(BookModel book, string viewTitle, bool removeMainViewBefore = false, BookMainView? mainViewBefore = null, object? previousViewModel = null)
    {
        this.ViewModel = new BookEditViewModel(book, this, previousViewModel)
        {
            ViewTitle = viewTitle,
            RemoveMainViewBefore = removeMainViewBefore,
            MainViewBefore = mainViewBefore,
        };
        this.BindingContext = this.ViewModel;

        this.InitializeComponent();

        if (book.BookFormat == null ||
            (book.BookFormat != null && !book.BookFormat!.Equals(AppStringResources.Audiobook)))
        {
            // Need this to make sure the stepper doesn't set pages read
            // to 100 if over 100. 100 is default.
            var stepper = (Stepper)this.FindByName("PageReadStepper");
            stepper.Maximum = (int)book.BookPageTotal;
            stepper.Value = book.BookPageRead;
        }
    }

    private BookEditViewModel ViewModel { get; set; }

    // Need this to make sure new info populates when you
    // navigate back to the view.
    protected override void OnAppearing()
    {
        var variable = this.ViewModel.SetViewModelData();
    }
}