// <copyright file="AuthorBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data.Models;
using BookCollector.Views.Author;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class AuthorBaseViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<AuthorModel>? fullAuthorList;

        [ObservableProperty]
        public static ObservableCollection<AuthorModel>? filteredAuthorList1;

        [ObservableProperty]
        public static ObservableCollection<AuthorModel>? filteredAuthorList2;

        [ObservableProperty]
        public int totalAuthorsCount;

        [ObservableProperty]
        public int filteredAuthorsCount;

        [ObservableProperty]
        public AuthorModel? selectedAuthor;

        [RelayCommand]
        public async Task AuthorSelectionChanged()
        {
            if (this.SelectedAuthor != null)
            {
                var view = new AuthorMainView(this.SelectedAuthor, this.SelectedAuthor.FullName);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedAuthor = null;
            }
        }
    }
}
