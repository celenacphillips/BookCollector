using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
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
            SetIsBusyTrue();

            Task.WaitAll(
            [
                Task.Run (async () => FullGenreList = await FilterLists.GetAllGenresList(TestData.GenreList) ),
            ]);

            TotalGenresCount = FullGenreList.Count;

            FilteredGenreList = FullGenreList;
            FilteredGenresCount = FilteredGenreList.Count;

            TotalGenresString = StringManipulation.SetTotalGenresString(FilteredGenresCount, TotalGenresCount);

            SetIsBusyFalse();
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
            string? action = await PopupMenu(selected.GenreName);

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

        // TO DO
        [RelayCommand]
        public async Task AddGenre()
        {

        }

        // TO DO
        [RelayCommand]
        public async Task EditGenre(GenreModel selected)
        {

        }

        // TO DO
        [RelayCommand]
        public async Task DeleteGenre(GenreModel selected)
        {

        }
    }
}
