// <copyright file="BookEditView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Book;

using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Book;

/// <summary>
/// BookEditView class.
/// </summary>
public partial class BookEditView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookEditView"/> class.
    /// </summary>
    /// <param name="book">Book to add or edit.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    /// <param name="removeMainViewBefore">The value to determine if Main view should be removed in
    /// stack before this page or not. Default is false.</param>
    /// <param name="mainViewBefore">The main view to insered before this page. Default is null.</param>
    /// <param name="previousViewModel">The previous view model this method has been called from. Default is null.</param>
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

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        await this.ViewModel.SetViewModelData();
    }
}