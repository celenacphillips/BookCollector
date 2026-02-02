// <copyright file="GenresViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Genre;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Groupings
{
    public partial class GenresViewModel : GenreBaseViewModel
    {
        [ObservableProperty]
        public string? totalGenresstring;

        public GenresViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight;
            this.InfoText = $"{AppStringResources.GenreView_InfoText}";
            this.ViewTitle = AppStringResources.Genres;
            RefreshView = true;
        }

        private bool ShowHiddenGenres { get; set; }

        private bool GenreNameChecked { get; set; }

        private bool TotalBooksChecked { get; set; }

        private bool TotalPriceChecked { get; set; }

        public static bool RefreshView { get; set; }

        public static async Task SetList(bool showHiddenGenres)
        {
            if (fullGenreList == null)
            {
                fullGenreList = await FillLists.GetAllGenresList();
            }

            if (!showHiddenGenres)
            {
                filteredGenreList1 = new ObservableCollection<GenreModel>(fullGenreList!.Where(x => !x.HideGenre));
            }
            else
            {
                filteredGenreList1 = new ObservableCollection<GenreModel>(fullGenreList!);
            }
        }

        public static async Task HideBooks(bool showHiddenGenres)
        {
            if (!showHiddenGenres)
            {
                var hideList = new ObservableCollection<GenreModel>(fullGenreList!.Where(x => x.HideGenre));

                foreach (var item in hideList)
                {
                    var books = AllBooksViewModel.filteredBookList1?
                        .Where(x => x.BookGenreGuid == item.GenreGuid && !x.HideBook)
                        .ToObservableCollection();

                    foreach (var book in books)
                    {
                        book.HideBook = true;
                        await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                    }
                }
            }
        }

        public async Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await SetList(this.ShowHiddenGenres);

                    if (this.FilteredGenreList1 != null)
                    {
                        this.FilteredGenreList2 = this.FilteredGenreList1;

                        this.TotalGenresCount = this.FilteredGenreList1 != null ? this.FilteredGenreList1.Count : 0;

                        this.SearchOnGenre(this.Searchstring);

                        await Task.WhenAll(this.FilteredGenreList2.Select(x => x.SetTotalBooks(ShowHiddenBook)));
                        await Task.WhenAll(this.FilteredGenreList2.Select(x => x.SetTotalCostOfBooks(ShowHiddenBook)));

                        var sortList = SortLists.SortGenresList(
                                this.FilteredGenreList2,
                                this.GenreNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

                        this.FilteredGenresCount = this.FilteredGenreList2.Count;

                        this.TotalGenresstring = StringManipulation.SetTotalGenresString(this.FilteredGenresCount, this.TotalGenresCount);

                        this.ShowCollectionViewFooter = this.FilteredGenresCount > 0;

                        await Task.WhenAll(sortList);

                        this.FilteredGenreList2 = sortList.Result;
                    }

                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
        }

        [RelayCommand]
        public async void SearchOnGenre(string? input)
        {
            this.Searchstring = input;

            if (this.FilteredGenreList2 != null && this.FilteredGenreList1 != null)
            {
                if (!string.IsNullOrEmpty(this.Searchstring))
                {
                    this.FilteredGenreList2 = this.FilteredGenreList1.Where(x => !string.IsNullOrEmpty(x.GenreName) && x.GenreName.Contains(this.Searchstring.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                }
                else
                {
                    this.FilteredGenreList2 = this.FilteredGenreList1;
                }

                this.FilteredGenresCount = this.FilteredGenreList2 != null ? this.FilteredGenreList2.Count : 0;

                this.TotalGenresstring = StringManipulation.SetTotalGenresString(this.FilteredGenresCount, this.TotalGenresCount);
            }

            var sortList = SortLists.SortGenresList(
                                this.FilteredGenreList2,
                                this.GenreNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            await Task.WhenAll(sortList);

            this.FilteredGenreList2 = sortList.Result;
        }

        [RelayCommand]
        public async Task PopupMenuGenre(Guid? input)
        {
            if (this.FilteredGenreList2 != null)
            {
                var selected = this.FilteredGenreList2.FirstOrDefault(x => x.GenreGuid == input);

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
            RefreshView = true;
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

                        await Database.DeleteGenreAsync(ConvertTo<GenreDatabaseModel>(selected));
                        this.RemoveFromStaticList(selected);
                        this.RemoveBookFromGrouping(selected);

                        await ConfirmDelete(selected.GenreName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
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

                var result = await this.View.ShowPopupAsync(popup);
                if (!result.WasDismissedByTappingOutsideOfPopup)
                {
                    RefreshView = true;
                    await this.SetViewModelData();
                }
            }
        }

        private void GetPreferences()
        {
            this.ShowHiddenGenres = Preferences.Get("HiddenGenresOn", true /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.GenreNameChecked = Preferences.Get($"{this.ViewTitle}_GenreNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }

        private void RemoveFromStaticList(GenreModel selected)
        {
            if (GenresViewModel.fullGenreList != null)
            {
                GenresViewModel.RefreshView = this.RemoveGenreFromStaticList(selected, GenresViewModel.fullGenreList, GenresViewModel.filteredGenreList2);
            }
        }

        private bool RemoveGenreFromStaticList(GenreModel selected, ObservableCollection<GenreModel> genreList, ObservableCollection<GenreModel>? filteredGenreList)
        {
            var refresh = false;

            try
            {
                var oldGenre = genreList.FirstOrDefault(x => x.GenreGuid == selected.GenreGuid);

                if (oldGenre != null)
                {
                    genreList.Remove(oldGenre);
                    refresh = true;
                }

                if (filteredGenreList != null)
                {
                    var filteredGenre = filteredGenreList.FirstOrDefault(x => x.GenreGuid == selected.GenreGuid);

                    if (filteredGenre != null)
                    {
                        filteredGenreList.Remove(filteredGenre);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        private void RemoveBookFromGrouping(GenreModel genre)
        {
            var books = AllBooksViewModel.fullBookList?.Where(x => x.BookGenreGuid == genre.GenreGuid).ToList();

            foreach (var book in books)
            {
                book.BookGenreGuid = null;
                Database.SaveBookAsync(book);
                BookBaseViewModel.AddToStaticList(book);
            }
        }
    }
}
