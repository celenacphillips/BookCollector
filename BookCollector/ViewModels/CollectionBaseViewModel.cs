using BookCollector.Data.Models;
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
    public partial class CollectionBaseViewModel : BaseViewModel
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

        // TO DO
        [RelayCommand]
        public async Task CollectionSelectionChanged()
        {
            if (SelectedCollection != null)
            {
                //CollectionView view = new CollectionView(SelectedCollection);

                //await Shell.Current.Navigation.PushAsync(view);
                SelectedCollection = null;
            }
        }
    }
}
