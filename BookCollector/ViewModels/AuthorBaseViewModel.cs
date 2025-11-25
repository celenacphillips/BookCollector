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
    public partial class AuthorBaseViewModel : BaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<AuthorModel>? fullAuthorList;

        [ObservableProperty]
        public static ObservableCollection<AuthorModel>? filteredAuthorList;

        [ObservableProperty]
        public AuthorModel? selectedAuthor;

        [RelayCommand]
        public async Task AuthorSelectionChanged()
        {

        }
    }
}
