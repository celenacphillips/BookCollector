// <copyright file="ExportImportView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Main;

namespace BookCollector.Views.Main;

public partial class ExportImportView : ContentPage
{
    private ExportImportViewModel viewModel;

    public ExportImportView()
    {
        this.viewModel = new ExportImportViewModel(this);
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

    // Need this to make sure new info populates when you
    // navigate back to the view.
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