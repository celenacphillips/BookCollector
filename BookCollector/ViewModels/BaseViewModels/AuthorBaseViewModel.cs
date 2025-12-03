using BookCollector.Data.Models;
using BookCollector.Views.Author;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class AuthorBaseViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public int totalAuthorsCount;

        [ObservableProperty]
        public int filteredAuthorsCount;

        [ObservableProperty]
        public static ObservableCollection<AuthorModel>? fullAuthorList;

        [ObservableProperty]
        public static ObservableCollection<AuthorModel>? filteredAuthorList;

        [ObservableProperty]
        public AuthorModel? selectedAuthor;

        [RelayCommand]
        public async Task AuthorSelectionChanged()
        {
            if (SelectedAuthor != null)
            {
                AuthorMainView view = new AuthorMainView(SelectedAuthor, SelectedAuthor.FullName);

                await Shell.Current.Navigation.PushAsync(view);
                SelectedAuthor = null;
            }
        }
    }
}
