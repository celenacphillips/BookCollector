// <copyright file="GenresViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Groupings
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Library;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Genre;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// GenresViewModel class.
    /// </summary>
    public partial class GenresViewModel : GroupingBaseViewModel
    {
        /// <summary>
        /// Gets or sets the full genre list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<GenreModel>? fullGenreList;

        /// <summary>
        /// Gets or sets the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<GenreModel>? hiddenFilteredGenreList;

        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<GenreModel>? filteredGenreList;

        /// <summary>
        /// Gets or sets the total genres string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalGenresString;

        /// <summary>
        /// Gets or sets the total count of genres, based on the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int totalGenresCount;

        /// <summary>
        /// Gets or sets the total count of genres, based on the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int filteredGenresCount;

        /********************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="GenresViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public GenresViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = DeviceHeight;
            this.InfoText = $"{AppStringResources.GenreView_InfoText}";
            this.ViewTitle = AppStringResources.Genres;
            this.SetRefreshView(true);
        }

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static bool RefreshView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden genres or not.
        /// </summary>
        private bool ShowHiddenGenres { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether genre named is checked or not.
        /// </summary>
        private bool GenreNameChecked { get; set; }

        /********************************************************/

        /// <summary>
        /// Set the first filtered list based on the full genre list and the show hidden genres preference.
        /// </summary>
        /// <param name="showHiddenGenres">Show hidden genres.</param>
        /// <returns>A task.</returns>
        public static async Task SetList(bool showHiddenGenres)
        {
            fullGenreList ??= await FillLists.GetAllGenresList();

            hiddenFilteredGenreList = showHiddenGenres ? fullGenreList : fullGenreList!.Where(x => !x.HideGenre).ToObservableCollection();
        }

        /// <summary>
        /// Set books to hide books that are related to the genre.
        /// </summary>
        /// <param name="showHiddenGenres">Show hidden genres.</param>
        /// <returns>A task.</returns>
        public static async Task HideBooks(bool showHiddenGenres)
        {
            if (!showHiddenGenres)
            {
                var hideList = new ObservableCollection<GenreModel>(fullGenreList!.Where(x => x.HideGenre));

                foreach (var item in hideList)
                {
                    var books = AllBooksViewModel.hiddenFilteredBookList?
                        .Where(x => x.BookGenreGuid == item.GenreGuid && !x.HideBook)
                        .ToObservableCollection();

                    await UpdateBooksToHide(books);
                }
            }
        }

        /********************************************************/

        /// <summary>
        /// Search the list based on the genre name.
        /// </summary>
        /// <param name="input">Input string to find.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SearchOnGenre(string? input)
        {
            this.SearchString = input;

            (this.FilteredGenreList, this.FilteredGenresCount, this.TotalGenresString) = await this.Search(this.HiddenFilteredGenreList, this.TotalGenresCount, this.GenreNameChecked);
        }

        /// <summary>
        /// Show popup with options to interact with the selected genre object.
        /// </summary>
        /// <param name="input">Genre guid to interact with.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task PopupMenuGenre(Guid? input)
        {
            var selected = this.FilteredGenreList?.FirstOrDefault(x => x.GenreGuid == input);

            await this.PopupMenu(selected, selected?.GenreName);
        }

        /// <summary>
        /// Changes the view based on the selected genre.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task GenreSelectionChanged()
        {
            if (this.SelectedGenre != null && !string.IsNullOrEmpty(this.SelectedGenre.GenreName))
            {
                var view = new GenreMainView(this.SelectedGenre, this.SelectedGenre.GenreName);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedGenre = null;
            }
        }

        /// <summary>
        /// Create a new genre and navigate to the genre edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddGenre()
        {
            await this.SetIsBusyTrue();

            var view = new GenreEditView(new GenreModel(), $"{AppStringResources.AddNewGenre}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetViewModelData()
        {
            if (!RefreshView)
            {
                return;
            }

            this.SetRefreshView(false);

            await this.SetIsBusyTrue(true);

            try
            {
                this.GetPreferences();

                await SetList(this.ShowHiddenGenres);

                (this.TotalGenresCount,
                    this.FilteredGenresCount,
                    this.TotalGenresString,
                    this.ShowCollectionViewFooter,
                    this.FilteredGenreList) = await this.SetViewModelData(this.HiddenFilteredGenreList, this.GenreNameChecked);

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                await this.ViewModelCatch(ex);
            }
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public override bool GetPreferences()
        {
            this.ShowHiddenGenres = Preferences.Get("HiddenGenresOn", true /* Default */);
            ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.GenreNameChecked = Preferences.Get($"{this.ViewTitle}_GenreNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);

            return this.ShowHiddenGenres;
        }

        /// <summary>
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel)
        {
            viewModel.GenreNameVisible = true;
            viewModel.GenreNameChecked = this.GenreNameChecked;
            /******************************/
            viewModel.TotalBooksVisible = true;
            viewModel.TotalBooksChecked = this.TotalBooksChecked;
            /******************************/
            viewModel.TotalPriceVisible = true;
            viewModel.TotalPriceChecked = this.TotalPriceChecked;
            /******************************/
            viewModel.AscendingChecked = this.AscendingChecked;
            viewModel.DescendingChecked = this.DescendingChecked;

            return viewModel;
        }

        /// <summary>
        /// Show edit view.
        /// </summary>
        /// <param name="selected">Selected object.</param>
        /// <returns>A task.</returns>
        public override async Task Edit(object selected)
        {
            await this.SetIsBusyTrue();

            var view = new GenreEditView((GenreModel)selected, $"{AppStringResources.EditGenre}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Delete grouping from database.
        /// </summary>
        /// <param name="selected">Selected object.</param>
        /// <returns>A task.</returns>
        public override async Task DeleteGrouping(object selected)
        {
            await Database.DeleteGenreAsync(ConvertTo<GenreDatabaseModel>(selected));
            RemoveFromStaticList((GenreModel)selected);
            await RemoveBookFromGrouping((GenreModel)selected);
        }

        /// <summary>
        /// Set whether to refresh view or not.
        /// </summary>
        /// <param name="value">Value to change to.</param>
        public override void SetRefreshView(bool value)
        {
            RefreshView = value;
        }

        /********************************************************/

        private static void RemoveFromStaticList(GenreModel selected)
        {
            if (GenresViewModel.fullGenreList != null)
            {
                GenresViewModel.RefreshView = RemoveGenreFromStaticList(selected, GenresViewModel.fullGenreList, GenresViewModel.filteredGenreList);
            }
        }

        private static bool RemoveGenreFromStaticList(GenreModel selected, ObservableCollection<GenreModel> genreList, ObservableCollection<GenreModel>? filteredGenreList)
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

        private static async Task RemoveBookFromGrouping(GenreModel genre)
        {
            var books = AllBooksViewModel.fullBookList?.Where(x => x.BookGenreGuid == genre.GenreGuid).ToList();

            if (books != null)
            {
                foreach (var book in books)
                {
                    book.BookGenreGuid = null;
                    await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                    await BookBaseViewModel.AddToStaticList(book);
                }
            }
        }
    }
}
