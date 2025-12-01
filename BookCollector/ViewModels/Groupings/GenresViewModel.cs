using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Genre;
using BookCollector.Views.Genre;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Groupings
{
    public partial class GenresViewModel : GenreBaseViewModel
    {
        [ObservableProperty]
        public string? totalGenresString;

        public GenresViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.GenreView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                // Unit test data
                var genreList = TestData.GenreList;

                Task.WaitAll(
                [
                    Task.Run (async () => FullGenreList = await FilterLists.GetAllGenresList(genreList) ),
                ]);

                TotalGenresCount = FullGenreList.Count;

                FilteredGenreList = FullGenreList;
                FilteredGenresCount = FilteredGenreList.Count;

                TotalGenresString = StringManipulation.SetTotalGenresString(FilteredGenresCount, TotalGenresCount);

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SearchOnGenre(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (!string.IsNullOrEmpty(SearchString))
                FilteredGenreList = FilteredGenreList.Where(x => x.GenreName.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            else
                FilteredGenreList = FullGenreList;

            FilteredGenresCount = FilteredGenreList.Count;

            TotalGenresString = StringManipulation.SetTotalGenresString(FilteredGenresCount, TotalGenresCount);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuGenre(Guid? input)
        {
            var selected = FilteredGenreList.FirstOrDefault(x => x.GenreGuid == input);
            string? action = await BaseViewModel.PopupMenu(selected.GenreName);

            switch (action)
            {
                case "Edit":
                    await EditGenre(selected);
                    break;

                case "Delete":
                    await DeleteGenre(selected);
                    break;

                default:
                    break;
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
        public async Task AddGenre()
        {
            SetIsBusyTrue();

            GenreEditView view = new GenreEditView(new GenreModel(), $"{AppStringResources.AddNewGenre}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditGenre(GenreModel selected)
        {
            SetIsBusyTrue();

            GenreEditView view = new GenreEditView(selected, $"{AppStringResources.EditGenre}", true);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteGenre(GenreModel selected)
        {
            bool answer = await DeleteCheck(selected.GenreName);

            if (answer)
            {
                try
                {
                    SetIsBusyTrue();

                    // Unit test data
                    TestData.DeleteGenre(selected);

                    await ConfirmDelete(selected.GenreName);

                    await SetViewModelData();

                    SetIsBusyFalse();
                }
                catch (Exception ex)
                {
                    await CanceledAction();
                }
            }
            else
            {
                await CanceledAction();
            }
        }
    }
}
