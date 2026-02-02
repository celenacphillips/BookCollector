// <copyright file="CollectionBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data.Models;
using BookCollector.Views.Collection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class CollectionBaseViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<CollectionModel>? fullCollectionList;

        [ObservableProperty]
        public static ObservableCollection<CollectionModel>? filteredCollectionList1;

        [ObservableProperty]
        public static ObservableCollection<CollectionModel>? filteredCollectionList2;

        [ObservableProperty]
        public int totalCollectionsCount;

        [ObservableProperty]
        public int filteredCollectionsCount;

        [ObservableProperty]
        public CollectionModel? selectedCollection;

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
    }
}
