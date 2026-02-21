// <copyright file="ReadingDataView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.ViewModels.Statistics;

namespace BookCollector.Views.Statistics;

public partial class ReadingDataView : ContentPage
{
    private ReadingDataViewModel viewModel;

    public ReadingDataView()
    {
        this.viewModel = new ReadingDataViewModel(this);
        this.BindingContext = this.viewModel;

        this.InitializeComponent();
    }

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