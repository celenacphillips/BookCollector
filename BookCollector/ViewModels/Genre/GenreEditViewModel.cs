// <copyright file="GenreEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Genre
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.Views.Genre;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// GenreEditViewModel class.
    /// </summary>
    public partial class GenreEditViewModel : GenresViewModel
    {
        /// <summary>
        /// Gets or sets the genre to edit.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public GenreModel editedGenre;

        /// <summary>
        /// Gets or sets a value indicating whether the genre name is valid or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool genreNameNotValid;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenreEditViewModel"/> class.
        /// </summary>
        /// <param name="genre">Genre to edit.</param>
        /// <param name="view">View related to view model.</param>
        public GenreEditViewModel(GenreModel genre, ContentPage view)
            : base(view)
        {
            this.View = view;

            this.EditedGenre = (GenreModel)genre.Clone();
        }

        /// <summary>
        /// Add genre to the static list in the list view model.
        /// </summary>
        /// <param name="genre">Genre to add.</param>
        /// <returns>A task.</returns>
        public static async Task AddToStaticList(GenreModel genre)
        {
            if (GenresViewModel.fullGenreList != null)
            {
                GenresViewModel.RefreshView = await AddGenreToStaticList(genre, GenresViewModel.fullGenreList, GenresViewModel.filteredGenreList);
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async new Task SetViewModelData()
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

        /// <summary>
        /// Save genre to the database and returns to the previous view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SaveGenre()
        {
            try
            {
                this.SetIsBusyTrue();

                if (this.GenreNameNotValid)
                {
                    await this.DisplayMessage(AppStringResources.GenreNameNotValid, null);
                    this.SetIsBusyFalse();
                }
                else
                {
#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    this.EditedGenre = await BaseViewModel.Database.SaveGenreAsync(ConvertTo<GenreDatabaseModel>(this.EditedGenre));
                    await AddToStaticList(this.EditedGenre);

                    if (this.InsertMainViewBefore)
                    {
                        var view = new GenreMainView(this.EditedGenre, $"{this.EditedGenre.GenreName}");
                        Shell.Current.Navigation.InsertPageBefore(view, this.View);
                    }

                    await Shell.Current.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await this.ViewModelCatch(ex);
                this.SetRefreshView(false);
            }
        }

        /// <summary>
        /// Check if the genre name is valid and set the related value.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ValidateGenreName()
        {
            this.ValidateEntry();
        }

        private static async Task<bool> AddGenreToStaticList(GenreModel genre, ObservableCollection<GenreModel> genreList, ObservableCollection<GenreModel>? filteredGenreList)
        {
            var refresh = false;

            try
            {
                var oldGenre = genreList.FirstOrDefault(x => x.GenreGuid == genre.GenreGuid);

                if (oldGenre != null)
                {
                    var index = genreList.IndexOf(oldGenre);
                    genreList.Remove(oldGenre);
                    genreList.Insert(index, genre);
                    refresh = true;
                }
                else
                {
                    genreList.Add(genre);
                    refresh = true;
                }

                if (filteredGenreList != null)
                {
                    var filteredGenre = filteredGenreList.FirstOrDefault(x => x.GenreGuid == genre.GenreGuid);

                    if (filteredGenre != null)
                    {
                        var index = filteredGenreList.IndexOf(filteredGenre);
                        filteredGenreList.Remove(filteredGenre);
                        filteredGenreList.Insert(index, genre);
                        refresh = true;
                    }
                    else
                    {
                        filteredGenreList.Add(genre);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        private void ValidateEntry()
        {
            this.GenreNameNotValid = string.IsNullOrEmpty(this.EditedGenre.GenreName);
        }
    }
}
