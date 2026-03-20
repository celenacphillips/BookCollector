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
        public static ObservableCollection<AuthorModel>? filteredAuthorList;

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
            this.CollectionViewHeight = DeviceHeight;
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
        public static async new Task SetList(bool showHiddenAuthors)
        {
            fullAuthorList ??= await FillLists.GetAllAuthorsList();

            hiddenFilteredAuthorList = SetList<AuthorModel>(fullAuthorList!, showHiddenAuthors).ToObservableCollection();
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
            this.ShowHiddenAuthors = Preferences.Get("HiddenAuthorsOn", true /* Default */);
            ShowHiddenBooks = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.AuthorLastNameChecked = Preferences.Get($"{this.ViewTitle}_AuthorLastNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);

            return this.ShowHiddenAuthors;
        }

        /// <summary>
        /// Check if the list is null.
        /// </summary>
        /// <returns>If the list is null.</returns>
        public override bool ListNullCheck()
        {
            return this.HiddenFilteredAuthorList != null;
        }

        /// <summary>
        /// Iterate through the list and set necessary data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetListData()
        {
            this.FilteredAuthorList = this.HiddenFilteredAuthorList;

            await Task.WhenAll(this.FilteredAuthorList!.Select(x => x.SetTotalBooks(ShowHiddenBooks)));
            await Task.WhenAll(this.FilteredAuthorList!.Select(x => x.SetTotalCostOfBooks(ShowHiddenBooks)));
        }

        /// <summary>
        /// Find filters for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetFilters()
        {
            this.FilteredAuthorList = await FilterLists.FilterList(
                                this.HiddenFilteredAuthorList!,
                                this.SearchString);
        }

        /// <summary>
        /// Find sort values for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetSorts()
        {
            var sortList = SortLists.SortAuthorList(
                                this.FilteredAuthorList!,
                                this.AuthorLastNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            await Task.WhenAll(sortList);

            this.FilteredAuthorList = sortList.Result;
        }

        /// <summary>
        /// Set data for view.
        /// </summary>
        public async override void SetViewStrings()
        {
            this.TotalAuthorsCount = this.HiddenFilteredAuthorList?.Count ?? 0;

            this.FilteredAuthorsCount = this.FilteredAuthorList?.Count ?? 0;

            this.TotalAuthorsString = StringManipulation.SetTotalAuthorsString(this.FilteredAuthorsCount, this.TotalAuthorsCount);

            this.ShowCollectionViewFooter = this.FilteredAuthorsCount > 0;
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

            if (this.FilteredAuthorList != null && this.HiddenFilteredAuthorList != null)
            {
                if (!string.IsNullOrEmpty(input))
                {
                    this.FilteredAuthorList = FilterLists.FilterOnSearchString(this.HiddenFilteredAuthorList, input);
                }
                else
                {
                    this.FilteredAuthorList = await FilterLists.FilterList(
                                this.HiddenFilteredAuthorList,
                                this.SearchString);
                }

                this.SetViewStrings();

                await this.SetSorts();
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
            if (this.FilteredAuthorList != null)
            {
                var selected = this.FilteredAuthorList.FirstOrDefault(x => x.AuthorGuid == input);

                if (selected != null)
                {
                    List<string> actions = [AppStringResources.Edit, AppStringResources.Delete];
                    var action = await this.PopupActionMenu(selected.FullName, actions);

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

                    await BaseViewModel.Database.DeleteAuthorAsync(ConvertTo<AuthorDatabaseModel>(selected));
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
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel)
        {
            viewModel.AuthorLastNameVisible = true;
            viewModel.AuthorLastNameChecked = this.AuthorLastNameChecked;
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

        private static void RemoveFromStaticList(AuthorModel selected)
        {
            if (AuthorsViewModel.fullAuthorList != null)
            {
                AuthorsViewModel.RefreshView = RemoveAuthorFromStaticList(selected, AuthorsViewModel.fullAuthorList, AuthorsViewModel.filteredAuthorList);
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
                    await BaseViewModel.Database.DeleteBookAuthorAsync(bookAuthor.AuthorGuid, bookAuthor.BookGuid);

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
    }
}
