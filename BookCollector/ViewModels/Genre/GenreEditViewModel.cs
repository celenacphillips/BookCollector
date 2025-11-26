using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
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
            _view = view;

            EditedGenre = (GenreModel)genre.Clone();
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            ValidateEntry();

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveGenre()
        {
            if (GenreNameValid)
            {
#if ANDROID
                if (Platform.CurrentActivity != null &&
                Platform.CurrentActivity.Window != null)
                    Platform.CurrentActivity.Window.DecorView.ClearFocus();
#endif

                if (ViewTitle.Equals($"{AppStringResources.AddNewGenre}"))
                {
                    // Unit test data
                    TestData.InsertGenre(EditedGenre);
                }
                else
                {
                    // Unit test data
                    TestData.UpdateGenre(EditedGenre);
                }

                if (InsertMainViewBefore)
                {
                    GenreMainView view = new GenreMainView(EditedGenre, $"{EditedGenre.GenreName}");
                    Shell.Current.Navigation.InsertPageBefore(view, _view);
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

        private void ValidateEntry()
        {
            if (string.IsNullOrEmpty(EditedGenre.GenreName))
                GenreNameValid = false;
            else
                GenreNameValid = true;
        }
    }
}
