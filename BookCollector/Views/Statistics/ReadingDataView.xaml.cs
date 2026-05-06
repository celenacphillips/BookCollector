// <copyright file="ReadingDataView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Statistics;

using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Statistics;

/// <summary>
/// ReadingDataView class.
/// </summary>
public partial class ReadingDataView : ContentPage
{
    private readonly ReadingDataViewModel viewModel;

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

        this.CreateCollectionView();
    }

    private void CreateCollectionView()
    {
        var headerGrid = new Grid
        {
            Margin = new Thickness(20, 20, 20, 10),
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            },
        };

        if (this.viewModel.PageBooksShow)
        {
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        if (this.viewModel.ShowAudiobooks)
        {
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        var yearLabel = new Label
        {
            Text = AppStringResources.Year,
            Style = Application.Current?.Resources["Header"] as Style,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.End,
        };
        Grid.SetRow(yearLabel, 0);
        Grid.SetColumn(yearLabel, 0);
        headerGrid.Add(yearLabel);

        var booksLabel = new Label
        {
            Text = AppStringResources.BooksRead,
            Style = Application.Current?.Resources["Header"] as Style,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.End,
        };
        Grid.SetRow(booksLabel, 0);
        Grid.SetColumn(booksLabel, 1);
        headerGrid.Add(booksLabel);

        if (this.viewModel.PageBooksShow)
        {
            var pagesLabel = new Label
            {
                Text = AppStringResources.PagesRead,
                Style = Application.Current?.Resources["Header"] as Style,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
            };
            Grid.SetRow(pagesLabel, 0);
            Grid.SetColumn(pagesLabel, 2);
            headerGrid.Add(pagesLabel);
        }

        if (this.viewModel.ShowAudiobooks)
        {
            var audiobookLabel = new Label
            {
                Text = AppStringResources.AudiobookHours,
                Style = Application.Current?.Resources["Header"] as Style,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
            };
            Grid.SetRow(audiobookLabel, 0);
            Grid.SetColumn(audiobookLabel, 3);
            headerGrid.Add(audiobookLabel);
        }

        var dataTemplate = new DataTemplate(() =>
        {
            var grid = new Grid
            {
                Margin = new Thickness(20, 20, 20, 10),
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                },
            };

            if (this.viewModel.PageBooksShow)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            if (this.viewModel.ShowAudiobooks)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            var yearDataLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                Style = Application.Current?.Resources["Header"] as Style,
            };
            yearDataLabel.SetBinding(Label.TextProperty, "Year");
            Grid.SetRow(yearDataLabel, 0);
            Grid.SetColumn(yearDataLabel, 0);
            grid.Add(yearDataLabel);

            var booksDataLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
            };
            booksDataLabel.SetBinding(Label.TextProperty, new Binding("BooksReadCount", stringFormat: "{0:N0}"));
            Grid.SetRow(booksDataLabel, 0);
            Grid.SetColumn(booksDataLabel, 1);
            grid.Add(booksDataLabel);

            if (this.viewModel.PageBooksShow)
            {
                var pagesDataLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                pagesDataLabel.SetBinding(Label.TextProperty, new Binding("PagesReadCount", stringFormat: "{0:N0}"));
                Grid.SetRow(pagesDataLabel, 0);
                Grid.SetColumn(pagesDataLabel, 2);
                grid.Add(pagesDataLabel);
            }

            if (this.viewModel.ShowAudiobooks)
            {
                var audiobookDataLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                };
                audiobookDataLabel.SetBinding(Label.TextProperty, new Binding("AudiobookTime", stringFormat: "{0:#,##0.00}"));
                Grid.SetRow(audiobookDataLabel, 0);
                Grid.SetColumn(audiobookDataLabel, 3);
                grid.Add(audiobookDataLabel);
            }

            return grid;
        });

        var collectionView = new CollectionView
        {
            ItemsSource = this.viewModel.ReadingDataList,
            Header = headerGrid,
            ItemTemplate = dataTemplate,
        };
        collectionView.SetBinding(ItemsView.ItemsSourceProperty, "ReadingDataList");

        var readingDataStack = (VerticalStackLayout)this.FindByName("readingDataStack");
        readingDataStack.Children.Clear();
        readingDataStack.Children.Add(collectionView);
    }
}