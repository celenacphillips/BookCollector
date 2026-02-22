// <copyright file="ReadingDataView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Statistics;

using BookCollector.ViewModels.Statistics;

/// <summary>
/// ReadingDataView class.
/// </summary>
public partial class ReadingDataView : ContentPage
{
    private ReadingDataViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadingDataView"/> class.
    /// </summary>
    public ReadingDataView()
    {
        this.viewModel = new ReadingDataViewModel(this);
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