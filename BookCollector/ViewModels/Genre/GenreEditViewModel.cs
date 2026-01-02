// <copyright file="GenreEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;
using BookCollector.Views.Genre;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Genre
{
    public partial class GenreEditViewModel : GenreBaseViewModel
    {
        [ObservableProperty]
        public GenreModel editedGenre;

        [ObservableProperty]
        public bool genreNameValid;

        public GenreEditViewModel(GenreModel genre, ContentPage view)
        {
            this.View = view;

            this.EditedGenre = (GenreModel)genre.Clone();
        }

        public bool InsertMainViewBefore { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.ValidateEntry();

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SaveGenre()
        {
            try
            {
                this.SetIsBusyTrue();

                if (!this.GenreNameValid)
                {
                    await DisplayMessage(AppStringResources.GenreNameNotValid, null);
                }
                else
                {
#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    if (TestData.UseTestData)
                    {
                        TestData.UpdateGenre(this.EditedGenre);
                    }
                    else
                    {
                        this.EditedGenre = await Database.SaveGenreAsync(ConvertTo<GenreDatabaseModel>(this.EditedGenre));
                        AddToStaticList(this.EditedGenre);
                    }

                    if (this.InsertMainViewBefore)
                    {
                        var view = new GenreMainView(this.EditedGenre, $"{this.EditedGenre.GenreName}");
                        Shell.Current.Navigation.InsertPageBefore(view, this.View);
                    }

                    await Shell.Current.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                await DisplayMessage("Error!", ex.Message);
#endif
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public void ValidateGenreName()
        {
            this.ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(this.EditedGenre.GenreName))
            {
                var genreNameEditor = this.View.FindByName<Editor>("GenreNameEditor");
                genreNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                genreNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                this.GenreNameValid = false;
            }
            else
            {
                var userAppTheme = Application.Current?.UserAppTheme == AppTheme.Unspecified ? Application.Current?.PlatformAppTheme : Application.Current?.UserAppTheme;

                var genreNameEditor = this.View.FindByName<Editor>("GenreNameEditor");
                genreNameEditor.TextColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                genreNameEditor.PlaceholderColor = userAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                this.GenreNameValid = true;
            }
        }

        public static async Task AddToStaticList(GenreModel genre)
        {
            if (GenresViewModel.fullGenreList != null)
            {
                GenresViewModel.RefreshView = await AddGenreToStaticList(genre, GenresViewModel.fullGenreList, GenresViewModel.filteredGenreList);
            }
        }

        private static async Task<bool> AddGenreToStaticList(GenreModel genre, ObservableCollection<GenreModel> genreList, ObservableCollection<GenreModel>? filteredGenreList)
        {
            var refresh = false;

            await Task.WhenAll(new Task[]
            {
                genre.SetTotalBooks(true),
                genre.SetTotalCostOfBooks(true),
            });

            try
            {
                var oldGenre = genreList.FirstOrDefault(x => x.GenreGuid == genre.GenreGuid);

                if (oldGenre != null)
                {
                    var index = genreList.IndexOf(oldGenre);
                    genreList.Remove(oldGenre);
                    genreList.Insert(index, genre);
                    refresh = true;
                }
                else
                {
                    genreList.Add(genre);
                    refresh = true;
                }

                if (filteredGenreList != null)
                {
                    var filteredGenre = filteredGenreList.FirstOrDefault(x => x.GenreGuid == genre.GenreGuid);

                    if (filteredGenre != null)
                    {
                        var index = filteredGenreList.IndexOf(filteredGenre);
                        filteredGenreList.Remove(filteredGenre);
                        filteredGenreList.Insert(index, genre);
                        refresh = true;
                    }
                    else
                    {
                        filteredGenreList.Add(genre);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }
    }
}
