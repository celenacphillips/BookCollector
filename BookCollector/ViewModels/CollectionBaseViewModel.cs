using BookCollector.Data.Models;
using BookCollector.Views.Collection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels
{
    public partial class CollectionBaseViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public int totalCollectionsCount;

        [ObservableProperty]
        public int filteredCollectionsCount;

        [ObservableProperty]
        public static ObservableCollection<CollectionModel>? fullCollectionList;

        [ObservableProperty]
        public static ObservableCollection<CollectionModel>? filteredCollectionList;

        [ObservableProperty]
        public CollectionModel? selectedCollection;

        [RelayCommand]
        public async Task CollectionSelectionChanged()
        {
            if (SelectedCollection != null)
            {
                CollectionMainView view = new CollectionMainView(SelectedCollection, SelectedCollection.CollectionName);

                await Shell.Current.Navigation.PushAsync(view);
                SelectedCollection = null;
            }
        }
    }
}
