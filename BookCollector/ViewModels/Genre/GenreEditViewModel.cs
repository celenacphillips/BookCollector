using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Genre;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Genre
{
    public partial class GenreEditViewModel : GenreBaseViewModel
    {
        [ObservableProperty]
        public GenreModel editedGenre;

        [ObservableProperty]
        public bool genreNameValid;

        public bool InsertMainViewBefore { get; set; }

        public GenreEditViewModel(GenreModel genre, ContentPage view)
        {
            View = view;

            EditedGenre = (GenreModel)genre.Clone();
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                ValidateEntry();

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SaveGenre()
        {
            if (!GenreNameValid)
            {
                await DisplayMessage(AppStringResources.GenreNameNotValid, null);
            }
            else
            {
#if ANDROID
                if (Platform.CurrentActivity != null &&
                Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (!string.IsNullOrEmpty(ViewTitle) && ViewTitle.Equals($"{AppStringResources.AddNewGenre}"))
                {
                    if (TestData.UseTestData)
                    {
                        TestData.InsertGenre(EditedGenre);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (TestData.UseTestData)
                    {
                        TestData.UpdateGenre(EditedGenre);
                    }
                    else
                    {

                    }
                }

                if (InsertMainViewBefore)
                {
                    var view = new GenreMainView(EditedGenre, $"{EditedGenre.GenreName}");
                    Shell.Current.Navigation.InsertPageBefore(view, View);
                }

                await Shell.Current.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }


        [RelayCommand]
        public void ValidateGenreName()
        {
            ValidateEntry();
        }

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(EditedGenre.GenreName))
            {
                var genreNameEditor = View.FindByName<Editor>("GenreNameEditor");
                genreNameEditor.TextColor = (Color?)Application.Current?.Resources["Warning"];
                genreNameEditor.PlaceholderColor = (Color?)Application.Current?.Resources["Warning"];
                GenreNameValid = false;
            }
            else
            {
                var genreNameEditor = View.FindByName<Editor>("GenreNameEditor");
                genreNameEditor.TextColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                genreNameEditor.PlaceholderColor = Application.Current?.UserAppTheme == AppTheme.Light ? (Color?)Application.Current?.Resources["TextLight"] : (Color?)Application.Current?.Resources["TextDark"];
                GenreNameValid = true;
            }
        }
    }
}
