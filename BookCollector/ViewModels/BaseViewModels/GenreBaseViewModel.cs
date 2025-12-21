// <copyright file="GenreBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data.Models;
using BookCollector.Views.Genre;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class GenreBaseViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<GenreModel>? fullGenreList;

        [ObservableProperty]
        public static ObservableCollection<GenreModel>? filteredGenreList;

        [ObservableProperty]
        public int totalGenresCount;

        [ObservableProperty]
        public int filteredGenresCount;

        [RelayCommand]
        public async Task GenreSelectionChanged()
        {
            if (this.SelectedGenre != null && !string.IsNullOrEmpty(this.SelectedGenre.GenreName))
            {
                var view = new GenreMainView(this.SelectedGenre, this.SelectedGenre.GenreName);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedGenre = null;
            }
        }
    }
}
