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
    public partial class GenreBaseViewModel : BaseViewModel
    {
        [ObservableProperty]
        public int totalGenresCount;

        [ObservableProperty]
        public int filteredGenresCount;

        [ObservableProperty]
        public static ObservableCollection<GenreModel>? fullGenreList;

        [ObservableProperty]
        public static ObservableCollection<GenreModel>? filteredGenreList;

        [ObservableProperty]
        public GenreModel? selectedGenre;

        // TO DO
        [RelayCommand]
        public async Task GenreSelectionChanged()
        {

        }
    }
}
