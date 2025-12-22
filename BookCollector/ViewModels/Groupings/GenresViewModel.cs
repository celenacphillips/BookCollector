// <copyright file="GenresViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Genre;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Groupings
{
    public partial class GenresViewModel : GenreBaseViewModel
    {
        [ObservableProperty]
        public string? totalGenresstring;

        public GenresViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight - this.DoubleMenuBar;
            this.InfoText = $"{AppStringResources.GenreView_InfoText}";
            this.ViewTitle = AppStringResources.Genres;
        }

        private bool ShowHiddenGenres { get; set; }

        private bool GenreNameChecked { get; set; }

        private bool TotalBooksChecked { get; set; }

        private bool TotalPriceChecked { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                var fullList = FillLists.GetAllGenresList(this.ShowHiddenGenres);

                await Task.WhenAll(fullList);

                this.FullGenreList = fullList.Result;

                if (this.FullGenreList != null)
                {
                    this.FilteredGenreList = this.FullGenreList;

                    var sortList = SortLists.SortGenresList(
                            this.FilteredGenreList,
                            this.GenreNameChecked,
                            this.TotalBooksChecked,
                            this.TotalPriceChecked,
                            this.AscendingChecked,
                            this.DescendingChecked);

                    this.TotalGenresCount = this.FullGenreList.Count;

                    this.FilteredGenresCount = this.FilteredGenreList.Count;

                    this.TotalGenresstring = StringManipulation.SetTotalCollectionsString(this.FilteredGenresCount, this.TotalGenresCount);

                    this.ShowCollectionViewFooter = this.FilteredGenresCount > 0;

                    await Task.WhenAll(sortList);

                    this.FilteredGenreList = sortList.Result;
                }

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public void SearchOnGenre(string? input)
        {
            this.SetIsBusyTrue();

            this.Searchstring = input;

            if (this.FilteredGenreList != null)
            {
                if (!string.IsNullOrEmpty(this.Searchstring))
                {
                    this.FilteredGenreList = this.FilteredGenreList.Where(x => !string.IsNullOrEmpty(x.GenreName) && x.GenreName.Contains(this.Searchstring.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                }
                else
                {
                    this.FilteredGenreList = this.FullGenreList;
                }

                this.FilteredGenresCount = this.FilteredGenreList != null ? this.FilteredGenreList.Count : 0;

                this.TotalGenresstring = StringManipulation.SetTotalGenresString(this.FilteredGenresCount, this.TotalGenresCount);
            }

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuGenre(Guid? input)
        {
            if (this.FilteredGenreList != null)
            {
                var selected = this.FilteredGenreList.FirstOrDefault(x => x.GenreGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.GenreName))
                {
                    var action = await PopupMenu(selected.GenreName);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await this.EditGenre(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await this.DeleteGenre(selected);
                    }
                }
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
        public async Task AddGenre()
        {
            this.SetIsBusyTrue();

            var view = new GenreEditView(new GenreModel(), $"{AppStringResources.AddNewGenre}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditGenre(GenreModel selected)
        {
            this.SetIsBusyTrue();

            var view = new GenreEditView(selected, $"{AppStringResources.EditGenre}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteGenre(GenreModel selected)
        {
            if (!string.IsNullOrEmpty(selected.GenreName))
            {
                var answer = await DeleteCheck(selected.GenreName);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        if (TestData.UseTestData)
                        {
                            TestData.DeleteGenre(selected);
                        }
                        else
                        {
                            await Database.DeleteGenreAsync(ConvertTo<GenreDatabaseModel>(selected));
                        }

                        await ConfirmDelete(selected.GenreName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
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

        [RelayCommand]
        public async Task SortPopup()
        {
            if (!string.IsNullOrEmpty(this.ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, this.ViewTitle)
                {
                    GenreNameVisible = true,
                    GenreNameChecked = this.GenreNameChecked,
                    TotalBooksVisible = true,
                    TotalBooksChecked = this.TotalBooksChecked,
                    TotalPriceVisible = true,
                    TotalPriceChecked = this.TotalPriceChecked,
                    AscendingChecked = this.AscendingChecked,
                    DescendingChecked = this.DescendingChecked,
                };

                popup.BindingContext = viewModel;

                await this.View.ShowPopupAsync(popup);
                await this.SetViewModelData();
            }
        }

        private void GetPreferences()
        {
            this.ShowHiddenGenres = Preferences.Get("HiddenGenresOn", true /* Default */);
            this.ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.GenreNameChecked = Preferences.Get($"{this.ViewTitle}_GenreNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }
    }
}
