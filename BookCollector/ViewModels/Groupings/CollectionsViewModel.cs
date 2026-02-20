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
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class CollectionsViewModel : CollectionBaseViewModel
    {
        [ObservableProperty]
        public string? totalCollectionsstring;

        public CollectionsViewModel(ContentPage view)
        {
            this.View = view;

            this.CollectionViewHeight = this.DeviceHeight;
            this.InfoText = $"{AppStringResources.CollectionView_InfoText}";
            this.ViewTitle = AppStringResources.Collections;
            RefreshView = true;
        }

        public static bool RefreshView { get; set; }

        private bool ShowHiddenCollections { get; set; }

        private bool CollectionNameChecked { get; set; }

        private bool TotalBooksChecked { get; set; }

        private bool TotalPriceChecked { get; set; }

        public static async Task SetList(bool showHiddenCollections)
        {
            fullCollectionList ??= await FillLists.GetAllCollectionsList();

            if (!showHiddenCollections)
            {
                filteredCollectionList1 = new ObservableCollection<CollectionModel>(fullCollectionList!.Where(x => !x.HideCollection));
            }
            else
            {
                filteredCollectionList1 = new ObservableCollection<CollectionModel>(fullCollectionList!);
            }
        }

        public static async Task HideBooks(bool showHiddenCollections)
        {
            if (!showHiddenCollections)
            {
                var hideList = new ObservableCollection<CollectionModel>(fullCollectionList!.Where(x => x.HideCollection));

                foreach (var item in hideList)
                {
                    var books = AllBooksViewModel.filteredBookList1?
                        .Where(x => x.BookCollectionGuid == item.CollectionGuid && !x.HideBook)
                        .ToObservableCollection();

                    if (books != null)
                    {
                        foreach (var book in books)
                        {
                            book.HideBook = true;
                            await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                        }
                    }
                }
            }
        }

        public async Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await SetList(this.ShowHiddenCollections);

                    if (this.FilteredCollectionList1 != null)
                    {
                        this.FilteredCollectionList2 = this.FilteredCollectionList1;

                        this.TotalCollectionsCount = this.FilteredCollectionList1 != null ? this.FilteredCollectionList1.Count : 0;

                        await this.SearchOnCollection(this.SearchString);

                        await Task.WhenAll(this.FilteredCollectionList2.Select(x => x.SetTotalBooks(ShowHiddenBook)));
                        await Task.WhenAll(this.FilteredCollectionList2.Select(x => x.SetTotalCostOfBooks(ShowHiddenBook)));

                        var sortList = SortLists.SortCollectionsList(
                                this.FilteredCollectionList2,
                                this.CollectionNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

                        this.FilteredCollectionsCount = this.FilteredCollectionList2.Count;

                        this.TotalCollectionsstring = StringManipulation.SetTotalCollectionsString(this.FilteredCollectionsCount, this.TotalCollectionsCount);

                        this.ShowCollectionViewFooter = this.FilteredCollectionsCount > 0;

                        await Task.WhenAll(sortList);

                        this.FilteredCollectionList2 = sortList.Result;
                    }

                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
        }

        [RelayCommand]
        public async Task SearchOnCollection(string? input)
        {
            this.SearchString = input;

            if (this.FilteredCollectionList2 != null && this.FilteredCollectionList1 != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    this.FilteredCollectionList2 = this.FilteredCollectionList1
                        .Where(x => !string.IsNullOrEmpty(x.CollectionName) && x.CollectionName.Contains(input.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase))
                        .ToObservableCollection();
                }
                else
                {
                    this.FilteredCollectionList2 = this.FilteredCollectionList1;
                }

                this.FilteredCollectionsCount = this.FilteredCollectionList2 != null ? this.FilteredCollectionList2.Count : 0;

                this.TotalCollectionsstring = StringManipulation.SetTotalCollectionsString(this.FilteredCollectionsCount, this.TotalCollectionsCount);

                var sortList = SortLists.SortCollectionsList(
                                this.FilteredCollectionList2!,
                                this.CollectionNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

                await Task.WhenAll(sortList);

                this.FilteredCollectionList2 = sortList.Result;
            }
        }

        [RelayCommand]
        public async Task PopupMenuCollection(Guid? input)
        {
            if (this.FilteredCollectionList2 != null)
            {
                var selected = this.FilteredCollectionList2.FirstOrDefault(x => x.CollectionGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.CollectionName))
                {
                    var action = await PopupMenu(selected.CollectionName);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await this.EditCollection(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await this.DeleteCollection(selected);
                    }
                }
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task AddCollection()
        {
            this.SetIsBusyTrue();

            var view = new CollectionEditView(new CollectionModel(), $"{AppStringResources.AddNewCollection}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditCollection(CollectionModel selected)
        {
            this.SetIsBusyTrue();

            var view = new CollectionEditView(selected, $"{AppStringResources.EditCollection}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteCollection(CollectionModel selected)
        {
            if (!string.IsNullOrEmpty(selected.CollectionName))
            {
                var answer = await DeleteCheck(selected.CollectionName);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        await Database.DeleteCollectionAsync(ConvertTo<CollectionDatabaseModel>(selected));
                        RemoveFromStaticList(selected);
                        await RemoveBookFromGrouping(selected);

                        await ConfirmDelete(selected.CollectionName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                        await CanceledAction();
                    }
                }
                else
                {
                    await CanceledAction();
                }
            }
        }

        [RelayCommand]
        public async Task SortPopup()
        {
            if (!string.IsNullOrEmpty(this.ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, this.ViewTitle)
                {
                    CollectionNameVisible = true,
                    CollectionNameChecked = this.CollectionNameChecked,
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

        private void GetPreferences()
        {
            this.ShowHiddenCollections = Preferences.Get("HiddenCollectionsOn", true /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.CollectionNameChecked = Preferences.Get($"{this.ViewTitle}_CollectionNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }

        private static void RemoveFromStaticList(CollectionModel selected)
        {
            if (CollectionsViewModel.fullCollectionList != null)
            {
                CollectionsViewModel.RefreshView = RemoveCollectionFromStaticList(selected, CollectionsViewModel.fullCollectionList, CollectionsViewModel.filteredCollectionList2);
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
                    await Database.SaveBookAsync(book);
                    await BookBaseViewModel.AddToStaticList(book);
                }
            }
        }
    }
}
