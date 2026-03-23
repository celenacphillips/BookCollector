// <copyright file="CollectionEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Collection
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.Views.Collection;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// CollectionEditViewModel class.
    /// </summary>
    public partial class CollectionEditViewModel : CollectionsViewModel
    {
        /// <summary>
        /// Gets or sets the collection to edit.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public CollectionModel editedCollection;

        /// <summary>
        /// Gets or sets a value indicating whether the collection name is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool collectionNameNotValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionEditViewModel"/> class.
        /// </summary>
        /// <param name="collection">Collection to edit.</param>
        /// <param name="view">View related to view model.</param>
        public CollectionEditViewModel(CollectionModel collection, ContentPage view)
            : base(view)
        {
            this.View = view;

            this.EditedCollection = (CollectionModel)collection.Clone();
        }

        /// <summary>
        /// Add collection to the static list in the list view model.
        /// </summary>
        /// <param name="collection">Collection to add.</param>
        /// <returns>A task.</returns>
        public static async Task AddToStaticList(CollectionModel collection)
        {
            if (CollectionsViewModel.fullCollectionList != null)
            {
                CollectionsViewModel.RefreshView = await AddCollectionToStaticList(collection, CollectionsViewModel.fullCollectionList, CollectionsViewModel.filteredCollectionList);
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async new Task SetViewModelData()
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

        /// <summary>
        /// Save collection to the database and returns to the previous view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SaveCollection()
        {
            try
            {
                this.SetIsBusyTrue();

                if (this.CollectionNameNotValid)
                {
                    await this.DisplayMessage(AppStringResources.CollectionNameNotValid, null);
                    this.SetIsBusyFalse();
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
                    await AddToStaticList(this.EditedCollection);

                    if (this.InsertMainViewBefore)
                    {
                        CollectionMainView view = new (this.EditedCollection, $"{this.EditedCollection.CollectionName}");
                        Shell.Current.Navigation.InsertPageBefore(view, this.View);
                    }

                    await Shell.Current.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await this.ViewModelCatch(ex);
                RefreshView = false;
            }
        }

        /// <summary>
        /// Check if the collection name is valid and set the related value.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateCollectionName()
        {
            this.ValidateEntry();
        }

        private static async Task<bool> AddCollectionToStaticList(CollectionModel collection, ObservableCollection<CollectionModel> collectionList, ObservableCollection<CollectionModel>? filteredCollectionList)
        {
            var refresh = false;

            await Task.WhenAll(
            [
                collection.SetTotalBooks(true),
                collection.SetTotalCostOfBooks(true),
            ]);

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

        private void ValidateEntry()
        {
            this.CollectionNameNotValid = string.IsNullOrEmpty(this.EditedCollection.CollectionName);
        }
    }
}
