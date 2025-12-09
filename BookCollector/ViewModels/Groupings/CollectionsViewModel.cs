using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Collection;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Collection;
using BookCollector.Views.Groupings;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Groupings
{
    public partial class CollectionsViewModel : CollectionBaseViewModel
    {
        [ObservableProperty]
        public string? totalCollectionsString;

        private bool ShowHiddenCollections { get; set; }
        private bool CollectionNameChecked { get; set; }
        private bool TotalBooksChecked { get; set; }
        private bool TotalPriceChecked { get; set; }

        public CollectionsViewModel(ContentPage view)
        {
            View = view;

            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.CollectionView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                Task.WaitAll(
                [
                    Task.Run (async () => FullCollectionList = await FilterLists.GetAllCollectionsList(ShowHiddenCollections) ),
                ]);

                if (FullCollectionList != null)
                {
                    TotalCollectionsCount = FullCollectionList.Count;

                    FilteredCollectionList = FullCollectionList;

                    foreach (var collection in FullCollectionList)
                    {
                        await collection.SetTotalBooks(ShowHiddenBook);
                    }

                    Task.WaitAll(
                    [
                        Task.Run (async () => FilteredCollectionList = await FilterLists.SortCollectionsList(FilteredCollectionList,
                                                                                                             CollectionNameChecked,
                                                                                                             TotalBooksChecked,
                                                                                                             TotalPriceChecked,
                                                                                                             AscendingChecked,
                                                                                                             DescendingChecked) ),
                    ]);

                    FilteredCollectionsCount = FilteredCollectionList.Count;

                    TotalCollectionsString = StringManipulation.SetTotalCollectionsString(FilteredCollectionsCount, TotalCollectionsCount);

                    ShowCollectionViewFooter = FilteredCollectionsCount > 0;
                }

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public void SearchOnCollection(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (FilteredCollectionList != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                    FilteredCollectionList = FilteredCollectionList.Where(x => !string.IsNullOrEmpty(x.CollectionName) && x.CollectionName.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                else
                    FilteredCollectionList = FullCollectionList;

                FilteredCollectionsCount = FilteredCollectionList != null ? FilteredCollectionList.Count : 0;

                TotalCollectionsString = StringManipulation.SetTotalCollectionsString(FilteredCollectionsCount, TotalCollectionsCount);
            }

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuCollection(Guid? input)
        {
            if (FilteredCollectionList != null)
            {
                var selected = FilteredCollectionList.FirstOrDefault(x => x.CollectionGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.CollectionName))
                {
                    var action = await PopupMenu(selected.CollectionName);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await EditCollection(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await DeleteCollection(selected);
                    }
                }
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        [RelayCommand]
        public async Task AddCollection()
        {
            SetIsBusyTrue();

            var view = new CollectionEditView(new CollectionModel(), $"{AppStringResources.AddNewCollection}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }


        [RelayCommand]
        public async Task EditCollection(CollectionModel selected)
        {
            SetIsBusyTrue();

            var view = new CollectionEditView(selected, $"{AppStringResources.EditCollection}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
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
                        SetIsBusyTrue();

                        if (TestData.UseTestData)
                        {
                            TestData.DeleteCollection(selected);
                        }
                        else
                        {

                        }

                        await ConfirmDelete(selected.CollectionName);

                        await SetViewModelData();

                        SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
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
            if (!string.IsNullOrEmpty(ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, ViewTitle)
                {
                    CollectionNameVisible = true,
                    CollectionNameChecked = CollectionNameChecked,
                    TotalBooksVisible = true,
                    TotalBooksChecked = TotalBooksChecked,
                    TotalPriceVisible = true,
                    TotalPriceChecked = TotalPriceChecked,
                    AscendingChecked = AscendingChecked,
                    DescendingChecked = DescendingChecked,
                };

                popup.BindingContext = viewModel;

                await View.ShowPopupAsync(popup);
                await SetViewModelData();
            }
        }

        private void GetPreferences()
        {
            ShowHiddenCollections = Preferences.Get("HiddenCollectionsOn", true  /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true  /* Default */);

            CollectionNameChecked = Preferences.Get($"{ViewTitle}_CollectionNameSelection", true  /* Default */);
            TotalBooksChecked = Preferences.Get($"{ViewTitle}_TotalBooksSelection", false  /* Default */);
            TotalPriceChecked = Preferences.Get($"{ViewTitle}_TotalPriceSelection", false  /* Default */);

            AscendingChecked = Preferences.Get($"{ViewTitle}_AscendingSelection", true  /* Default */);
            DescendingChecked = Preferences.Get($"{ViewTitle}_DescendingSelection", false  /* Default */);
        }
    }
}
