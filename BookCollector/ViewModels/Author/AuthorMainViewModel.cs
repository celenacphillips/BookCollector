using BookCollector.Data.Models;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Author
{
    public partial class AuthorMainViewModel : AuthorBaseViewModel
    {
        // TO DO
        // Set InfoText string - 11/26/2025
        public AuthorMainViewModel(AuthorModel author, ContentPage view)
        {
            _view = view;

            SelectedAuthor = author;
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
        // Set up AddNewBook - 11/26/2025
        [RelayCommand]
        public async Task AddNewBook()
        {

        }

        // TO DO
        // Set up AddExistingBook - 11/26/2025
        [RelayCommand]
        public async Task AddExistingBook()
        {

        }
    }
}
