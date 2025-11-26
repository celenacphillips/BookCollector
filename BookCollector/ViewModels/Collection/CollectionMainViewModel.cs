using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views.Collection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Collection
{
    public partial class CollectionMainViewModel : CollectionBaseViewModel
    {
        // TO DO
        // Set InfoText string - 11/25/2025
        public CollectionMainViewModel(CollectionModel collection, ContentPage view)
        {
            _view = view;

            SelectedCollection = collection;
            //InfoText = string.Empty;
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();


            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        // TO DO
        // Set up AddNewBook - 11/25/2025
        [RelayCommand]
        public async Task AddNewBook ()
        {

        }

        // TO DO
        // Set up AddExistingBook - 11/25/2025
        [RelayCommand]
        public async Task AddExistingBook()
        {

        }
    }
}
