using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Core.Extensions;
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

        public CollectionsViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.CollectionView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            Task.WaitAll(
            [
                Task.Run (async () => FullCollectionList = await FilterLists.GetAllCollectionsList(TestData.CollectionList) ),
            ]);

            TotalCollectionsCount = FullCollectionList.Count;

            FilteredCollectionList = FullCollectionList;
            FilteredCollectionsCount = FilteredCollectionList.Count;

            TotalCollectionsString = StringManipulation.SetTotalCollectionsString(FilteredCollectionsCount, TotalCollectionsCount);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SearchOnCollection(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (!string.IsNullOrEmpty(SearchString))
                FilteredCollectionList = FilteredCollectionList.Where(x => x.CollectionName.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            else
                FilteredCollectionList = FullCollectionList;

            FilteredCollectionsCount = FilteredCollectionList.Count;

            TotalCollectionsString = StringManipulation.SetTotalCollectionsString(FilteredCollectionsCount, TotalCollectionsCount);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuCollection(Guid? input)
        {
            var selected = FilteredCollectionList.FirstOrDefault(x => x.CollectionGuid == input);
            string? action = await PopupMenu(selected.CollectionName);

            switch (action)
            {
                case "Edit":
                    await EditCollection(selected);
                    break;

                case "Delete":
                    await DeleteCollection(selected);
                    break;

                default:
                    break;
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        // TO DO:
        // Fix Add Collection to not add to main list without save - 11/25/2025
        [RelayCommand]
        public async Task AddCollection()
        {
            SetIsBusyTrue();

            CollectionModel newCollection = new CollectionModel();

            //BookMainView view = new BookMainView(newBook, $"{AppStringResources.AddNewBook}");
            TestData.InsertCollection(newCollection);

            //await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        // TO DO
        [RelayCommand]
        public async Task EditCollection(CollectionModel selected)
        {
            SetIsBusyTrue();

            //CollectionEditView view = new CollectionEditView();
            //CollectionEditViewModel bindingContext = new CollectionEditViewModel(selected, view);
            //bindingContext.ViewTitle = $"{AppResources.EditCollection}";
            //view.BindingContext = bindingContext;

            //await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        // TO DO
        // Add checks for deleting collection - 11/25/2025
        [RelayCommand]
        public async Task DeleteCollection(CollectionModel selected)
        {
            SetIsBusyTrue();

            TestData.DeleteCollection(selected);

            await SetViewModelData();

            SetIsBusyFalse();
        }
    }
}
