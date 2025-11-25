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
        public static ObservableCollection<CollectionModel>? fullCollectionList;

        [ObservableProperty]
        public static ObservableCollection<CollectionModel>? filteredCollectionList;

        [ObservableProperty]
        public CollectionModel? selectedCollection;

        [RelayCommand]
        public async Task CollectionSelectionChanged()
        {
            
        }
    }
}
