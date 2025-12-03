using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Genre;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Genre;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
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

        private bool ShowHiddenGenres { get; set; }
        private bool GenreNameChecked { get; set; }
        private bool TotalBooksChecked { get; set; }
        private bool TotalPriceChecked { get; set; }

        public GenresViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.GenreView_InfoText}";
            ViewTitle = AppStringResources.Genres;
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                // Unit test data
                var genreList = TestData.GenreList;

                Task.WaitAll(
                [
                    Task.Run (async () => FullGenreList = await FilterLists.GetAllGenresList(genreList, ShowHiddenGenres) ),
                ]);

                TotalGenresCount = FullGenreList.Count;

                FilteredGenreList = FullGenreList;

                foreach (var genre in FullGenreList)
                {
                    genre.SetTotalBooks(ShowHiddenBook);
                }

                Task.WaitAll(
                [
                    Task.Run (async () => FilteredGenreList = await FilterLists.SortGenresList(FilteredGenreList,
                                                                                               GenreNameChecked,
                                                                                               TotalBooksChecked,
                                                                                               TotalPriceChecked,
                                                                                               AscendingChecked,
                                                                                               DescendingChecked) ),
                ]);

                FilteredGenresCount = FilteredGenreList.Count;

                TotalGenresString = StringManipulation.SetTotalGenresString(FilteredGenresCount, TotalGenresCount);

                ShowCollectionViewFooter = FilteredGenresCount > 0;

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

        [RelayCommand]
        public async Task SortPopup()
        {
            var popup = new SortPopup();
            SortPopupViewModel viewModel = new SortPopupViewModel(popup, ViewTitle)
            {
                GenreNameVisible = true,
                GenreNameChecked = GenreNameChecked,
                TotalBooksVisible = true,
                TotalBooksChecked = TotalBooksChecked,
                TotalPriceVisible = true,
                TotalPriceChecked = TotalPriceChecked,
                AscendingChecked = AscendingChecked,
                DescendingChecked = DescendingChecked,
            };

            popup.BindingContext = viewModel;

            await _view.ShowPopupAsync(popup);
            await SetViewModelData();
        }

        private void GetPreferences()
        {
            ShowHiddenGenres = Preferences.Get("HiddenGenresOn", true  /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true  /* Default */);

            GenreNameChecked = Preferences.Get($"{ViewTitle}_GenreNameSelection", true  /* Default */);
            TotalBooksChecked = Preferences.Get($"{ViewTitle}_TotalBooksSelection", false  /* Default */);
            TotalPriceChecked = Preferences.Get($"{ViewTitle}_TotalPriceSelection", false  /* Default */);

            AscendingChecked = Preferences.Get($"{ViewTitle}_AscendingSelection", true  /* Default */);
            DescendingChecked = Preferences.Get($"{ViewTitle}_DescendingSelection", false  /* Default */);
        }
    }
}
