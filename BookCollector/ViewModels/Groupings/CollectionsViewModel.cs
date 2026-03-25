// <copyright file="CollectionsViewModel.cs" company="Castle Software">
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
    using BookCollector.Views.Collection;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// CollectionsViewModel class.
    /// </summary>
    public partial class CollectionsViewModel : GroupingBaseViewModel
    {
        /// <summary>
        /// Gets or sets the full collection list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<CollectionModel>? fullCollectionList;

        /// <summary>
        /// Gets or sets the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<CollectionModel>? hiddenFilteredCollectionList;

        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<CollectionModel>? filteredCollectionList;

        /// <summary>
        /// Gets or sets the total collections string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalCollectionsString;

        /// <summary>
        /// Gets or sets the total count of collections, based on the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int totalCollectionsCount;

        /// <summary>
        /// Gets or sets the total count of collections, based on the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int filteredCollectionsCount;

        /// <summary>
        /// Gets or sets the selected collection.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public CollectionModel? selectedCollection;

        /********************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionsViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public CollectionsViewModel(ContentPage view)
        {
            this.View = view;

            this.CollectionViewHeight = DeviceHeight;
            this.InfoText = $"{AppStringResources.CollectionView_InfoText}";
            this.ViewTitle = AppStringResources.Collections;
            this.SetRefreshView(true);
        }

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static bool RefreshView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden collections or not.
        /// </summary>
        private bool ShowHiddenCollections { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether collection name is checked or not.
        /// </summary>
        private bool CollectionNameChecked { get; set; }

        /********************************************************/

        /// <summary>
        /// Set the first filtered list based on the full collection list and the show hidden collections preference.
        /// </summary>
        /// <param name="showHiddenCollections">Show hidden collection.</param>
        /// <returns>A task.</returns>
        public static async Task SetList(bool showHiddenCollections)
        {
            fullCollectionList ??= await FillLists.GetAllCollectionsList();

            hiddenFilteredCollectionList = showHiddenCollections ? fullCollectionList : fullCollectionList!.Where(x => !x.HideCollection).ToObservableCollection();
        }

        /// <summary>
        /// Set books to hide books that are related to the collection.
        /// </summary>
        /// <param name="showHiddenCollections">Show hidden collection.</param>
        /// <returns>A task.</returns>
        public static async Task HideBooks(bool showHiddenCollections)
        {
            if (!showHiddenCollections)
            {
                var hideList = new ObservableCollection<CollectionModel>(fullCollectionList!.Where(x => x.HideCollection));

                foreach (var item in hideList)
                {
                    var books = AllBooksViewModel.hiddenFilteredBookList?
                        .Where(x => x.BookCollectionGuid == item.CollectionGuid && !x.HideBook)
                        .ToObservableCollection();

                    await UpdateBooksToHide(books);
                }
            }
        }

        /********************************************************/

        /// <summary>
        /// Search the list based on the collection name.
        /// </summary>
        /// <param name="input">Input string to find.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SearchOnCollection(string? input)
        {
            this.SearchString = input;

            (this.FilteredCollectionList, this.FilteredCollectionsCount, this.TotalCollectionsString) = await this.Search(this.HiddenFilteredCollectionList, this.TotalCollectionsCount, this.CollectionNameChecked);
        }

        /// <summary>
        /// Show popup with options to interact with the selected collection object.
        /// </summary>
        /// <param name="input">Collection guid to interact with.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task PopupMenuCollection(Guid? input)
        {
            var selected = this.FilteredCollectionList?.FirstOrDefault(x => x.CollectionGuid == input);

            await this.PopupMenu(selected, selected?.CollectionName);
        }

        /// <summary>
        /// Changes the view based on the selected collection.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task CollectionSelectionChanged()
        {
            if (this.SelectedCollection != null && !string.IsNullOrEmpty(this.SelectedCollection.CollectionName))
            {
                var view = new CollectionMainView(this.SelectedCollection, this.SelectedCollection.CollectionName);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedCollection = null;
            }
        }

        /// <summary>
        /// Create a new collection and navigate to the collection edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddCollection()
        {
            this.SetIsBusyTrue();

            var view = new CollectionEditView(new CollectionModel(), $"{AppStringResources.AddNewCollection}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.GetPreferences();

                    await SetList(this.ShowHiddenCollections);

                    (this.TotalCollectionsCount,
                        this.FilteredCollectionsCount,
                        this.TotalCollectionsString,
                        this.ShowCollectionViewFooter,
                        this.FilteredCollectionList) = await this.SetViewModelData(this.HiddenFilteredCollectionList, this.CollectionNameChecked);
                }
                catch (Exception ex)
                {
                    await this.ViewModelCatch(ex);
                    this.SetRefreshView(false);
                }
            }
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public override bool GetPreferences()
        {
            this.ShowHiddenCollections = Preferences.Get("HiddenCollectionsOn", true /* Default */);
            ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.CollectionNameChecked = Preferences.Get($"{this.ViewTitle}_CollectionNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);

            return this.ShowHiddenCollections;
        }

        /// <summary>
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel)
        {
            viewModel.CollectionNameVisible = true;
            viewModel.CollectionNameChecked = this.CollectionNameChecked;
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

        /// <summary>
        /// Show edit view.
        /// </summary>
        /// <param name="selected">Selected object.</param>
        /// <returns>A task.</returns>
        public override async Task Edit(object selected)
        {
            this.SetIsBusyTrue();

            var view = new CollectionEditView((CollectionModel)selected, $"{AppStringResources.EditCollection}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Delete grouping from database.
        /// </summary>
        /// <param name="selected">Selected object.</param>
        /// <returns>A task.</returns>
        public override async Task DeleteGrouping(object selected)
        {
            await Database.DeleteCollectionAsync(ConvertTo<CollectionDatabaseModel>(selected));
            RemoveFromStaticList((CollectionModel)selected);
            await RemoveBookFromGrouping((CollectionModel)selected);
        }

        /// <summary>
        /// Set whether to refresh view or not.
        /// </summary>
        /// <param name="value">Value to change to.</param>
        public override void SetRefreshView(bool value)
        {
            RefreshView = value;
        }

        /********************************************************/

        private static void RemoveFromStaticList(CollectionModel selected)
        {
            if (CollectionsViewModel.fullCollectionList != null)
            {
                CollectionsViewModel.RefreshView = RemoveCollectionFromStaticList(selected, CollectionsViewModel.fullCollectionList, CollectionsViewModel.filteredCollectionList);
            }
        }

        private static bool RemoveCollectionFromStaticList(CollectionModel selected, ObservableCollection<CollectionModel> collectionList, ObservableCollection<CollectionModel>? filteredCollectionList)
        {
            var refresh = false;

            try
            {
                var oldCollection = collectionList.FirstOrDefault(x => x.CollectionGuid == selected.CollectionGuid);

                if (oldCollection != null)
                {
                    collectionList.Remove(oldCollection);
                    refresh = true;
                }

                if (filteredCollectionList != null)
                {
                    var filteredCollection = filteredCollectionList.FirstOrDefault(x => x.CollectionGuid == selected.CollectionGuid);

                    if (filteredCollection != null)
                    {
                        filteredCollectionList.Remove(filteredCollection);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        private static async Task RemoveBookFromGrouping(CollectionModel collection)
        {
            var books = AllBooksViewModel.fullBookList?.Where(x => x.BookCollectionGuid == collection.CollectionGuid).ToList();

            if (books != null)
            {
                foreach (var book in books)
                {
                    book.BookCollectionGuid = null;
                    await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                    await BookBaseViewModel.AddToStaticList(book);
                }
            }
        }
    }
}
