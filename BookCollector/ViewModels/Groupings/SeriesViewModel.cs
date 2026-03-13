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
        public static ObservableCollection<SeriesModel>? filteredSeriesList2;

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
            this.CollectionViewHeight = this.DeviceHeight;
            this.InfoText = $"{AppStringResources.SeriesView_InfoText}";
            this.ViewTitle = AppStringResources.Series;
            RefreshView = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static new bool RefreshView { get; set; }

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
        public static async Task SetList(bool showHiddenSeries)
        {
            fullSeriesList ??= await FillLists.GetAllSeriesList();

            hiddenFilteredSeriesList = BaseViewModel.SetList<SeriesModel>(fullSeriesList!, showHiddenSeries).ToObservableCollection();
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

                    await BaseViewModel.UpdateBooksToHide(books);
                }
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await SetList(this.ShowHiddenSeries);

                    if (this.HiddenFilteredSeriesList != null)
                    {
                        this.FilteredSeriesList2 = this.HiddenFilteredSeriesList;

                        this.TotalSeriesCount = this.HiddenFilteredSeriesList.Count;

                        await this.SearchOnSeries(this.SearchString);

                        await Task.WhenAll(this.FilteredSeriesList2.Select(x => x.SetTotalBooks(ShowHiddenBook)));
                        await Task.WhenAll(this.FilteredSeriesList2.Select(x => x.SetTotalCostOfBooks(ShowHiddenBook)));

                        var sortList = SortLists.SortSeriesList(
                                this.FilteredSeriesList2,
                                this.SeriesNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

                        this.FilteredSeriesCount = this.FilteredSeriesList2.Count;

                        this.TotalSeriesString = StringManipulation.SetTotalSeriesString(this.FilteredSeriesCount, this.TotalSeriesCount);

                        this.ShowCollectionViewFooter = this.FilteredSeriesCount > 0;

                        await Task.WhenAll(sortList);

                        this.FilteredSeriesList2 = sortList.Result;
                    }

                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
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

            if (this.FilteredSeriesList2 != null && this.HiddenFilteredSeriesList != null)
            {
                if (!string.IsNullOrEmpty(this.SearchString))
                {
                    this.FilteredSeriesList2 = this.HiddenFilteredSeriesList.Where(x => !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.Contains(this.SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                }
                else
                {
                    this.FilteredSeriesList2 = this.HiddenFilteredSeriesList;
                }

                this.FilteredSeriesCount = this.FilteredSeriesList2 != null ? this.FilteredSeriesList2.Count : 0;

                this.TotalSeriesString = StringManipulation.SetTotalSeriesString(this.FilteredSeriesCount, this.TotalSeriesCount);

                var sortList = SortLists.SortSeriesList(
                                    this.FilteredSeriesList2!,
                                    this.SeriesNameChecked,
                                    this.TotalBooksChecked,
                                    this.TotalPriceChecked,
                                    this.AscendingChecked,
                                    this.DescendingChecked);

                await Task.WhenAll(sortList);

                this.FilteredSeriesList2 = sortList.Result;
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
            if (this.FilteredSeriesList2 != null)
            {
                var selected = this.FilteredSeriesList2.FirstOrDefault(x => x.SeriesGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.SeriesName))
                {
                    var action = await this.PopupMenu(selected.SeriesName);

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
        /// Show sort popup.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SortPopup()
        {
            if (!string.IsNullOrEmpty(this.ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, this.ViewTitle)
                {
                    SeriesNameVisible = true,
                    SeriesNameChecked = this.SeriesNameChecked,
                    TotalBooksVisible = true,
                    TotalBooksChecked = this.TotalBooksChecked,
                    TotalPriceVisible = true,
                    TotalPriceChecked = this.TotalPriceChecked,
                    AscendingChecked = this.AscendingChecked,
                    DescendingChecked = this.DescendingChecked,
                };

                popup.BindingContext = viewModel;

                var result = await this.View.ShowPopupAsync(popup);
                if (!result.WasDismissedByTappingOutsideOfPopup)
                {
                    RefreshView = true;
                    await this.SetViewModelData();
                }
            }
        }

        private static void RemoveFromStaticList(SeriesModel selected)
        {
            if (SeriesViewModel.fullSeriesList != null)
            {
                SeriesViewModel.RefreshView = RemoveSeriesFromStaticList(selected, SeriesViewModel.fullSeriesList, SeriesViewModel.filteredSeriesList2);
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

        private void GetPreferences()
        {
            this.ShowHiddenSeries = Preferences.Get("HiddenSeriesOn", true /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.SeriesNameChecked = Preferences.Get($"{this.ViewTitle}_SeriesNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }
    }
}
