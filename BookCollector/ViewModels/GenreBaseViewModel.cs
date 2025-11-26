using BookCollector.Data.Models;
using BookCollector.Views.Genre;
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
    public partial class GenreBaseViewModel : BookBaseViewModel
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

        [RelayCommand]
        public async Task GenreSelectionChanged()
        {
            if (SelectedGenre != null)
            {
                GenreMainView view = new GenreMainView(SelectedGenre, SelectedGenre.GenreName);

                await Shell.Current.Navigation.PushAsync(view);
                SelectedGenre = null;
            }
        }
    }
}
