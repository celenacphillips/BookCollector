// <copyright file="GenreEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Genre;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
                }

                if (this.InsertMainViewBefore)
                {
                    var view = new GenreMainView(this.EditedGenre, $"{this.EditedGenre.GenreName}");
                    Shell.Current.Navigation.InsertPageBefore(view, this.View);
                }

                await Shell.Current.Navigation.PopAsync();
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
    }
}
