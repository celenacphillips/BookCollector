// <copyright file="AuthorsViewModel.cs" company="Castle Software">
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
    using BookCollector.Views.Author;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// AuthorsViewModel class.
    /// </summary>
    public partial class AuthorsViewModel : GroupingBaseViewModel
    {
        /// <summary>
        /// Gets or sets the full author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<AuthorModel>? fullAuthorList;

        /// <summary>
        /// Gets or sets the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<AuthorModel>? hiddenFilteredAuthorList;

        /// <summary>
        /// Gets or sets the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Observable Property")]
        public static ObservableCollection<AuthorModel>? filteredAuthorList2;

        /// <summary>
        /// Gets or sets the total authors string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalAuthorsString;

        /// <summary>
        /// Gets or sets the total count of authors, based on the first filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int totalAuthorsCount;

        /// <summary>
        /// Gets or sets the total count of authors, based on the second filtered list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public int filteredAuthorsCount;

        /// <summary>
        /// Gets or sets the selected author.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public AuthorModel? selectedAuthor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorsViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public AuthorsViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight;
            this.InfoText = $"{AppStringResources.AuthorView_InfoText}";
            this.ViewTitle = AppStringResources.Authors;
            RefreshView = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden authors or not.
        /// </summary>
        private bool ShowHiddenAuthors { get; set; }

        /// <summary>
        /// Set the first filtered list based on the full author list and the show hidden authors preference.
        /// </summary>
        /// <param name="showHiddenAuthors">Show hidden authors.</param>
        /// <returns>A task.</returns>
        public static async Task SetList(bool showHiddenAuthors)
        {
            fullAuthorList ??= await FillLists.GetAllAuthorsList();

            hiddenFilteredAuthorList = BaseViewModel.SetList<AuthorModel>(fullAuthorList!, showHiddenAuthors).ToObservableCollection();
        }

        /// <summary>
        /// Set books to hide books that are related to the author.
        /// </summary>
        /// <param name="showHiddenAuthors">Show hidden authors.</param>
        /// <returns>A task.</returns>
        public static async Task HideBooks(bool showHiddenAuthors)
        {
            if (!showHiddenAuthors)
            {
                var hideList = new ObservableCollection<AuthorModel>(fullAuthorList!.Where(x => x.HideAuthor));

                foreach (var item in hideList)
                {
                    var books = await FillLists.GetAllBooksInAuthorList(item.AuthorGuid, true);

                    books = books?.Where(x => !x.HideBook).ToObservableCollection();

                    await BaseViewModel.UpdateBooksToHide(books);
                }
            }
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await SetList(this.ShowHiddenAuthors);

                    if (this.HiddenFilteredAuthorList != null)
                    {
                        this.FilteredAuthorList2 = this.HiddenFilteredAuthorList;

                        this.TotalAuthorsCount = this.HiddenFilteredAuthorList != null ? this.HiddenFilteredAuthorList.Count : 0;

                        await this.SearchOnAuthor(this.SearchString);

                        await Task.WhenAll(this.FilteredAuthorList2.Select(x => x.SetTotalBooks(ShowHiddenBook)));
                        await Task.WhenAll(this.FilteredAuthorList2.Select(x => x.SetTotalCostOfBooks(ShowHiddenBook)));

                        var sortList = SortLists.SortAuthorList(
                                this.FilteredAuthorList2,
                                this.AuthorLastNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

                        this.FilteredAuthorsCount = this.FilteredAuthorList2.Count;

                        this.TotalAuthorsString = StringManipulation.SetTotalAuthorsString(this.FilteredAuthorsCount, this.TotalAuthorsCount);

                        this.ShowCollectionViewFooter = this.FilteredAuthorsCount > 0;

                        await Task.WhenAll(sortList);

                        this.FilteredAuthorList2 = sortList.Result;
                    }

                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
        }

        /// <summary>
        /// Search the list based on the author name.
        /// </summary>
        /// <param name="input">Input string to find.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SearchOnAuthor(string? input)
        {
            this.SearchString = input;

            if (this.FilteredAuthorList2 != null && this.HiddenFilteredAuthorList != null)
            {
                if (!string.IsNullOrEmpty(this.SearchString))
                {
                    this.FilteredAuthorList2 = this.HiddenFilteredAuthorList.Where(x => x.FullName.Contains(this.SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                }
                else
                {
                    this.FilteredAuthorList2 = this.HiddenFilteredAuthorList;
                }

                this.FilteredAuthorsCount = this.FilteredAuthorList2 != null ? this.FilteredAuthorList2.Count : 0;

                this.TotalAuthorsString = StringManipulation.SetTotalAuthorsString(this.FilteredAuthorsCount, this.TotalAuthorsCount);

                var sortList = SortLists.SortAuthorList(
                                    this.FilteredAuthorList2!,
                                    this.AuthorLastNameChecked,
                                    this.TotalBooksChecked,
                                    this.TotalPriceChecked,
                                    this.AscendingChecked,
                                    this.DescendingChecked);

                await Task.WhenAll(sortList);

                this.FilteredAuthorList2 = sortList.Result;
            }
        }

        /// <summary>
        /// Show popup with options to interact with the selected author object.
        /// </summary>
        /// <param name="input">Author guid to interact with.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task PopupMenuAuthor(Guid? input)
        {
            if (this.FilteredAuthorList2 != null)
            {
                var selected = this.FilteredAuthorList2.FirstOrDefault(x => x.AuthorGuid == input);

                if (selected != null)
                {
                    var action = await this.PopupMenu(selected.FullName);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await this.EditAuthor(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await this.DeleteAuthor(selected);
                    }
                }
            }
        }

        /// <summary>
        /// Changes the view based on the selected author.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AuthorSelectionChanged()
        {
            if (this.SelectedAuthor != null)
            {
                var view = new AuthorMainView(this.SelectedAuthor, this.SelectedAuthor.FullName);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedAuthor = null;
            }
        }

        /// <summary>
        /// Create a new author and navigate to the author edit view.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task AddAuthor()
        {
            this.SetIsBusyTrue();

            var view = new AuthorEditView(new AuthorModel(), $"{AppStringResources.AddNewAuthor}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Navigate to author edit view for selected author.
        /// </summary>
        /// <param name="selected">Selected author.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task EditAuthor(AuthorModel selected)
        {
            this.SetIsBusyTrue();

            var view = new AuthorEditView(selected, selected.FullName, true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Delete selected author.
        /// </summary>
        /// <param name="selected">Selected author.</param>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task DeleteAuthor(AuthorModel selected)
        {
            var answer = await this.DeleteCheck(selected.FullName);

            if (answer)
            {
                try
                {
                    this.SetIsBusyTrue();

                    await Database.DeleteAuthorAsync(ConvertTo<AuthorDatabaseModel>(selected));
                    RemoveFromStaticList(selected);
                    await RemoveBookFromGrouping(selected);

                    await this.ConfirmDelete(selected.FullName);

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

        /// <summary>
        /// Show sort popup.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task SortPopup()
        {
            if (!string.IsNullOrEmpty(this.ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, this.ViewTitle)
                {
                    AuthorLastNameVisible = true,
                    AuthorLastNameChecked = this.AuthorLastNameChecked,
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

        private static void RemoveFromStaticList(AuthorModel selected)
        {
            if (AuthorsViewModel.fullAuthorList != null)
            {
                AuthorsViewModel.RefreshView = RemoveAuthorFromStaticList(selected, AuthorsViewModel.fullAuthorList, AuthorsViewModel.filteredAuthorList2);
            }
        }

        private static bool RemoveAuthorFromStaticList(AuthorModel selected, ObservableCollection<AuthorModel> authorList, ObservableCollection<AuthorModel>? filteredAuthorList)
        {
            var refresh = false;

            try
            {
                var oldAuthor = authorList.FirstOrDefault(x => x.AuthorGuid == selected.AuthorGuid);

                if (oldAuthor != null)
                {
                    authorList.Remove(oldAuthor);
                    refresh = true;
                }

                if (filteredAuthorList != null)
                {
                    var filteredAuthor = filteredAuthorList.FirstOrDefault(x => x.AuthorGuid == selected.AuthorGuid);

                    if (filteredAuthor != null)
                    {
                        filteredAuthorList.Remove(filteredAuthor);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        private static async Task RemoveBookFromGrouping(AuthorModel author)
        {
            var bookAuthors = await FillLists.GetAllBookAuthorsForAuthor(author.AuthorGuid);

            if (bookAuthors != null)
            {
                foreach (var bookAuthor in bookAuthors)
                {
                    await Database.DeleteBookAuthorAsync(bookAuthor.AuthorGuid, bookAuthor.BookGuid);

                    var book = AllBooksViewModel.fullBookList?.FirstOrDefault(x => x.BookGuid == bookAuthor.BookGuid);

                    if (book != null)
                    {
                        // Author string is already re-evaluated when loading the book lists,
                        // so no need to update anything else here.
                        await BookBaseViewModel.AddToStaticList(book);
                    }
                }
            }
        }

        private void GetPreferences()
        {
            this.ShowHiddenAuthors = Preferences.Get("HiddenAuthorsOn", true /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.AuthorLastNameChecked = Preferences.Get($"{this.ViewTitle}_AuthorLastNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }
    }
}
