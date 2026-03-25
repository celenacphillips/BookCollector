// <copyright file="ExportImportView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Main;

using BookCollector.ViewModels.Main;

/// <summary>
/// ExportImportView class.
/// </summary>
public partial class ExportImportView : ContentPage
{
    private ExportImportViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportImportView"/> class.
    /// </summary>
    public ExportImportView()
    {
        this.viewModel = new ExportImportViewModel(this);
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