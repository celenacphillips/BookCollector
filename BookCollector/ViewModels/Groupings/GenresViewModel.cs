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
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Maui.Extensions;
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
            RefreshView = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden genres or not.
        /// </summary>
        private bool ShowHiddenGenres { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether genre named is checked or not.
        /// </summary>
        private bool GenreNameChecked { get; set; }

        /// <summary>
        /// Set the first filtered list based on the full genre list and the show hidden genres preference.
        /// </summary>
        /// <param name="showHiddenGenres">Show hidden genres.</param>
        /// <returns>A task.</returns>
        public static async new Task SetList(bool showHiddenGenres)
        {
            fullGenreList ??= await FillLists.GetAllGenresList();

            hiddenFilteredGenreList = SetList<GenreModel>(fullGenreList!, showHiddenGenres).ToObservableCollection();
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
        /// Check if the list is null.
        /// </summary>
        /// <returns>If the list is null.</returns>
        public override bool ListNullCheck()
        {
            return this.HiddenFilteredGenreList != null;
        }

        /// <summary>
        /// Iterate through the list and set necessary data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetListData()
        {
            this.FilteredGenreList = this.HiddenFilteredGenreList;

            await Task.WhenAll(this.FilteredGenreList!.Select(x => x.SetTotalBooks(ShowHiddenBooks)));
            await Task.WhenAll(this.FilteredGenreList!.Select(x => x.SetTotalCostOfBooks(ShowHiddenBooks)));
        }

        /// <summary>
        /// Find filters for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetFilters()
        {
            this.FilteredGenreList = await FilterLists.FilterList(
                                this.HiddenFilteredGenreList!,
                                this.SearchString);
        }

        /// <summary>
        /// Find sort values for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetSorts()
        {
            var sortList = SortLists.SortGenresList(
                                this.FilteredGenreList!,
                                this.GenreNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            await Task.WhenAll(sortList);

            this.FilteredGenreList = sortList.Result;
        }

        /// <summary>
        /// Set data for view.
        /// </summary>
        public async override void SetViewStrings()
        {
            this.TotalGenresCount = this.HiddenFilteredGenreList?.Count ?? 0;

            this.FilteredGenresCount = this.FilteredGenreList?.Count ?? 0;

            this.TotalGenresString = StringManipulation.SetTotalGenresString(this.FilteredGenresCount, this.TotalGenresCount);

            this.ShowCollectionViewFooter = this.FilteredGenresCount > 0;
        }

        /// <summary>
        /// Search the list based on the genre name.
        /// </summary>
        /// <param name="input">Input string to find.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SearchOnGenre(string? input)
        {
            this.SearchString = input;

            if (this.FilteredGenreList != null && this.HiddenFilteredGenreList != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    this.FilteredGenreList = FilterLists.FilterOnSearchString(this.HiddenFilteredGenreList, input);
                }
                else
                {
                    this.FilteredGenreList = await FilterLists.FilterList(
                                this.HiddenFilteredGenreList,
                                this.SearchString);
                }

                this.SetViewStrings();

                await this.SetSorts();
            }
        }

        /// <summary>
        /// Show popup with options to interact with the selected genre object.
        /// </summary>
        /// <param name="input">Genre guid to interact with.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task PopupMenuGenre(Guid? input)
        {
            if (this.FilteredGenreList != null)
            {
                var selected = this.FilteredGenreList.FirstOrDefault(x => x.GenreGuid == input);

                if (selected != null && !string.IsNullOrEmpty(selected.GenreName))
                {
                    List<string> actions = [AppStringResources.Edit, AppStringResources.Delete];
                    var action = await this.PopupActionMenu(selected.GenreName, actions);

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
            this.SetIsBusyTrue();

            var view = new GenreEditView(new GenreModel(), $"{AppStringResources.AddNewGenre}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Navigate to genre edit view for selected genre.
        /// </summary>
        /// <param name="selected">Selected genre.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task EditGenre(GenreModel selected)
        {
            this.SetIsBusyTrue();

            var view = new GenreEditView(selected, $"{AppStringResources.EditGenre}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Delete selected genre.
        /// </summary>
        /// <param name="selected">Selected genre.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task DeleteGenre(GenreModel selected)
        {
            if (!string.IsNullOrEmpty(selected.GenreName))
            {
                var answer = await this.DeleteCheck(selected.GenreName);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        await Database.DeleteGenreAsync(ConvertTo<GenreDatabaseModel>(selected));
                        RemoveFromStaticList(selected);
                        await RemoveBookFromGrouping(selected);

                        await this.ConfirmDelete(selected.GenreName);

                        await this.SetViewModelData();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                        await this.CanceledAction();
                    }
                }
                else
                {
                    await this.CanceledAction();
                }
            }
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
