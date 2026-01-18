// <copyright file="CollectionEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;
using BookCollector.ViewModels.Library;
using BookCollector.Views.Collection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Collection
{
    public partial class CollectionEditViewModel : CollectionBaseViewModel
    {
        [ObservableProperty]
        public CollectionModel editedCollection;

        [ObservableProperty]
        public bool collectionNameNotValid;

        public CollectionEditViewModel(CollectionModel collection, ContentPage view)
        {
            this.View = view;

            this.EditedCollection = (CollectionModel)collection.Clone();
        }

        public bool InsertMainViewBefore { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.ValidateEntry();

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SaveCollection()
        {
            try
            {
                this.SetIsBusyTrue();

                if (this.CollectionNameNotValid)
                {
                    await DisplayMessage(AppStringResources.CollectionNameNotValid, null);
                }
                else
                {
#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    this.EditedCollection = await Database.SaveCollectionAsync(ConvertTo<CollectionDatabaseModel>(this.EditedCollection));
                    AddToStaticList(this.EditedCollection);

                    if (this.InsertMainViewBefore)
                    {
                        CollectionMainView view = new CollectionMainView(this.EditedCollection, $"{this.EditedCollection.CollectionName}");
                        Shell.Current.Navigation.InsertPageBefore(view, this.View);
                    }

                    await Shell.Current.Navigation.PopAsync();
                }
            }
            catch(Exception ex)
            {
#if DEBUG
                await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public void ValidateCollectionName()
        {
            this.ValidateEntry();
        }

        private void ValidateEntry()
        {
            this.CollectionNameNotValid = string.IsNullOrEmpty(this.EditedCollection.CollectionName);
        }

        public static async Task AddToStaticList(CollectionModel collection)
        {
            if (CollectionsViewModel.fullCollectionList != null)
            {
                CollectionsViewModel.RefreshView = await AddCollectionToStaticList(collection, CollectionsViewModel.fullCollectionList, CollectionsViewModel.filteredCollectionList2);
            }
        }

        private static async Task<bool> AddCollectionToStaticList(CollectionModel collection, ObservableCollection<CollectionModel> collectionList, ObservableCollection<CollectionModel>? filteredCollectionList)
        {
            var refresh = false;

            await Task.WhenAll(new Task[]
            {
                collection.SetTotalBooks(true),
                collection.SetTotalCostOfBooks(true),
            });

            try
            {
                var oldCollection = collectionList.FirstOrDefault(x => x.CollectionGuid == collection.CollectionGuid);

                if (oldCollection != null)
                {
                    var index = collectionList.IndexOf(oldCollection);
                    collectionList.Remove(oldCollection);
                    collectionList.Insert(index, collection);
                    refresh = true;
                }
                else
                {
                    collectionList.Add(collection);
                    refresh = true;
                }

                if (filteredCollectionList != null)
                {
                    var filteredCollection = filteredCollectionList.FirstOrDefault(x => x.CollectionGuid == collection.CollectionGuid);

                    if (filteredCollection != null)
                    {
                        var index = filteredCollectionList.IndexOf(filteredCollection);
                        filteredCollectionList.Remove(filteredCollection);
                        filteredCollectionList.Insert(index, collection);
                        refresh = true;
                    }
                    else
                    {
                        filteredCollectionList.Add(collection);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }
    }
}
