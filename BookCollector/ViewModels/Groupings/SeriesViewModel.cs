// <copyright file="SeriesViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Groupings
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Library;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Popups;
    using BookCollector.Views.Series;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// SeriesViewModel class.
    /// </summary>
    public partial class SeriesViewModel : GroupingBaseViewModel
    {
        /// <summary>
        /// Gets or sets the full series list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<SeriesModel>? fullSeriesList;

        /// <summary>
        /// Gets or sets the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<SeriesModel>? hiddenFilteredSeriesList;

        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<SeriesModel>? filteredSeriesList;

        /// <summary>
        /// Gets or sets the total series string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalSeriesString;

        /// <summary>
        /// Gets or sets the total count of series, based on the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int totalSeriesCount;

        /// <summary>
        /// Gets or sets the total count of series, based on the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int filteredSeriesCount;

        /// <summary>
        /// Gets or sets the selected series.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public SeriesModel? selectedSeries;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public SeriesViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = DeviceHeight;
            this.InfoText = $"{AppStringResources.SeriesView_InfoText}";
            this.ViewTitle = AppStringResources.Series;
            RefreshView = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden series or not.
        /// </summary>
        private bool ShowHiddenSeries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether series name is checked or not.
        /// </summary>
        private bool SeriesNameChecked { get; set; }

        /// <summary>
        /// Set the first filtered list based on the full series list and the show hidden series preference.
        /// </summary>
        /// <param name="showHiddenSeries">Show hidden series.</param>
        /// <returns>A task.</returns>
        public static async new Task SetList(bool showHiddenSeries)
        {
            fullSeriesList ??= await FillLists.GetAllSeriesList();

            hiddenFilteredSeriesList = SetList<SeriesModel>(fullSeriesList!, showHiddenSeries).ToObservableCollection();
        }

        /// <summary>
        /// Set books to hide books that are related to the series.
        /// </summary>
        /// <param name="showHiddenSeries">Show hidden series.</param>
        /// <returns>A task.</returns>
        public static async Task HideBooks(bool showHiddenSeries)
        {
            if (!showHiddenSeries)
            {
                var hideList = new ObservableCollection<SeriesModel>(fullSeriesList!.Where(x => x.HideSeries));

                foreach (var item in hideList)
                {
                    var books = AllBooksViewModel.hiddenFilteredBookList?
                        .Where(x => x.BookSeriesGuid == item.SeriesGuid && !x.HideBook)
                        .ToObservableCollection();

                    await UpdateBooksToHide(books);
                }
            }
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public override bool GetPreferences()
        {
            this.ShowHiddenSeries = Preferences.Get("HiddenSeriesOn", true /* Default */);
            ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.SeriesNameChecked = Preferences.Get($"{this.ViewTitle}_SeriesNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);

            return this.ShowHiddenSeries;
        }

        /// <summary>
        /// Check if the list is null.
        /// </summary>
        /// <returns>If the list is null.</returns>
        public override bool ListNullCheck()
        {
            return this.HiddenFilteredSeriesList != null;
        }

        /// <summary>
        /// Iterate through the list and set necessary data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetListData()
        {
            this.FilteredSeriesList = this.HiddenFilteredSeriesList;

            await Task.WhenAll(this.FilteredSeriesList!.Select(x => x.SetTotalBooks(ShowHiddenBooks)));
            await Task.WhenAll(this.FilteredSeriesList!.Select(x => x.SetTotalCostOfBooks(ShowHiddenBooks)));
        }

        /// <summary>
        /// Find filters for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetFilters()
        {
            this.FilteredSeriesList = await FilterLists.FilterList(
                                this.HiddenFilteredSeriesList!,
                                this.SearchString);
        }

        /// <summary>
        /// Find sort values for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetSorts()
        {
            var sortList = SortLists.SortSeriesList(
                                this.FilteredSeriesList!,
                                this.SeriesNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            await Task.WhenAll(sortList);

            this.FilteredSeriesList = sortList.Result;
        }

        /// <summary>
        /// Set data for view.
        /// </summary>
        public async override void SetViewStrings()
        {
            this.TotalSeriesCount = this.HiddenFilteredSeriesList?.Count ?? 0;

            this.FilteredSeriesCount = this.FilteredSeriesList?.Count ?? 0;

            this.TotalSeriesString = StringManipulation.SetTotalSeriesString(this.FilteredSeriesCount, this.TotalSeriesCount);

            this.ShowCollectionViewFooter = this.FilteredSeriesCount > 0;
        }

        /// <summary>
        /// Search the list based on the series name.
        /// </summary>
        /// <param name="input">Input string to find.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SearchOnSeries(string? input)
        {
            this.SearchString = input;

            if (this.FilteredSeriesList != null && this.HiddenFilteredSeriesList != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    this.FilteredSeriesList = FilterLists.FilterOnSearchString(this.HiddenFilteredSeriesList, input);
                }
                else
                {
                    this.FilteredSeriesList = await FilterLists.FilterList(
                                this.HiddenFilteredSeriesList,
                                this.SearchString);
                }

                this.SetViewStrings();

                await this.SetSorts();
            }
        }

        /// <summary>
        /// Show popup with options to interact with the selected series object.
        /// </summary>
        /// <param name="input">Series guid to interact with.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task PopupMenuSeries(Guid? input)
        {
            if (this.FilteredSeriesList != null)
            {
                var selected = this.FilteredSeriesList.FirstOrDefault(x => x.SeriesGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.SeriesName))
                {
                    List<string> actions = [AppStringResources.Edit, AppStringResources.Delete];
                    var action = await this.PopupActionMenu(selected.SeriesName, actions);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await this.EditSeries(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await this.DeleteSeries(selected);
                    }
                }
            }
        }

        /// <summary>
        /// Changes the view based on the selected series.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SeriesSelectionChanged()
        {
            if (this.SelectedSeries != null && !string.IsNullOrEmpty(this.SelectedSeries.SeriesName))
            {
                var view = new SeriesMainView(this.SelectedSeries, this.SelectedSeries.SeriesName);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedSeries = null;
            }
        }

        /// <summary>
        /// Create a new series and navigate to the series edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddSeries()
        {
            this.SetIsBusyTrue();

            var view = new SeriesEditView(new SeriesModel(), $"{AppStringResources.AddNewSeries}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Navigate to series edit view for selected series.
        /// </summary>
        /// <param name="selected">Selected series.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task EditSeries(SeriesModel selected)
        {
            this.SetIsBusyTrue();

            var view = new SeriesEditView(selected, $"{AppStringResources.EditSeries}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Delete selected series.
        /// </summary>
        /// <param name="selected">Selected series.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task DeleteSeries(SeriesModel selected)
        {
            if (!string.IsNullOrEmpty(selected.SeriesName))
            {
                var answer = await this.DeleteCheck(selected.SeriesName);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        await Database.DeleteSeriesAsync(ConvertTo<SeriesDatabaseModel>(selected));
                        RemoveFromStaticList(selected);
                        await RemoveBookFromGrouping(selected);

                        await this.ConfirmDelete(selected.SeriesName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                        await this.CanceledAction();
                    }
                }
                else
                {
                    await this.CanceledAction();
                }
            }
        }

        /// <summary>
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel)
        {
            viewModel.SeriesNameVisible = true;
            viewModel.SeriesNameChecked = this.SeriesNameChecked;
            /******************************/
            viewModel.TotalBooksVisible = true;
            viewModel.TotalBooksChecked = this.TotalBooksChecked;
            /******************************/
            viewModel.TotalPriceVisible = true;
            viewModel.TotalPriceChecked = this.TotalPriceChecked;
            /******************************/
            viewModel.AscendingChecked = this.AscendingChecked;
            viewModel.DescendingChecked = this.DescendingChecked;

            return viewModel;
        }

        private static void RemoveFromStaticList(SeriesModel selected)
        {
            if (SeriesViewModel.fullSeriesList != null)
            {
                SeriesViewModel.RefreshView = RemoveSeriesFromStaticList(selected, SeriesViewModel.fullSeriesList, SeriesViewModel.filteredSeriesList);
            }
        }

        private static bool RemoveSeriesFromStaticList(SeriesModel selected, ObservableCollection<SeriesModel> seriesList, ObservableCollection<SeriesModel>? filteredSeriesList)
        {
            var refresh = false;

            try
            {
                var oldSeries = seriesList.FirstOrDefault(x => x.SeriesGuid == selected.SeriesGuid);

                if (oldSeries != null)
                {
                    seriesList.Remove(oldSeries);
                    refresh = true;
                }

                if (filteredSeriesList != null)
                {
                    var filteredSeries = filteredSeriesList.FirstOrDefault(x => x.SeriesGuid == selected.SeriesGuid);

                    if (filteredSeries != null)
                    {
                        filteredSeriesList.Remove(filteredSeries);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        private static async Task RemoveBookFromGrouping(SeriesModel series)
        {
            var books = AllBooksViewModel.fullBookList?.Where(x => x.BookSeriesGuid == series.SeriesGuid).ToList();

            if (books != null)
            {
                foreach (var book in books)
                {
                    book.BookSeriesGuid = null;
                    await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                    await BookBaseViewModel.AddToStaticList(book);
                }
            }
        }
    }
}
