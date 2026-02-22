// <copyright file="BookMainView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Book;

using BookCollector.Data.Models;
using BookCollector.ViewModels.Book;

/// <summary>
/// BookMainView class.
/// </summary>
public partial class BookMainView : ContentPage
{
    private BookMainViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="BookMainView"/> class.
    /// </summary>
    /// <param name="book">Book to view.</param>
    /// <param name="viewTitle">The value to display on the menu bar.</param>
    /// <param name="previousViewModel">The previous view model this method has been called from. Default is null.</param>
    public BookMainView(BookModel book, string viewTitle, object? previousViewModel = null)
    {
        this.viewModel = new BookMainViewModel(book, this, previousViewModel)
        {
            ViewTitle = viewTitle,
        };
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    /// <summary>
    /// Called when the view becomes visible.
    /// </summary>
    protected override async void OnAppearing()
    {
        this.Dispatcher.Dispatch(() =>
        {
            var items = this.ToolbarItems.ToList();
            this.ToolbarItems.Clear();
            foreach (var item in items)
            {
                this.ToolbarItems.Add(item);
            }
        });

        await this.viewModel.SetViewModelData();
    }
}