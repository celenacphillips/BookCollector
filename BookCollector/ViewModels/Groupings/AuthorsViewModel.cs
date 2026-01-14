// <copyright file="AuthorsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Author;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Author;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Groupings
{
    public partial class AuthorsViewModel : AuthorBaseViewModel
    {
        [ObservableProperty]
        public string? totalAuthorsstring;

        public AuthorsViewModel(ContentPage view)
        {
            this.View = view;
            this.CollectionViewHeight = this.DeviceHeight - this.DoubleMenuBar;
            this.InfoText = $"{AppStringResources.AuthorView_InfoText}";
            this.ViewTitle = AppStringResources.Authors;
            RefreshView = true;
        }

        private bool ShowHiddenAuthors { get; set; }

        private bool TotalBooksChecked { get; set; }

        private bool TotalPriceChecked { get; set; }

        public static bool RefreshView { get; set; }

        public static async Task SetList(bool showHiddenAuthors)
        {
            if (fullAuthorList == null)
            {
                fullAuthorList = await FillLists.GetAllAuthorsList(showHiddenAuthors);
            }
        }

        public async Task SetViewModelData()
        {
            if (!RefreshView)
            {
                this.SetIsBusyTrue();

                var temp = this.FilteredAuthorList;
                this.FilteredAuthorList = null;
                this.FilteredAuthorList = temp;

                this.SetIsBusyFalse();
            }

            if (RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    await SetList(this.ShowHiddenAuthors);

                    if (fullAuthorList != null)
                    {
                        this.FilteredAuthorList = fullAuthorList;

                        this.TotalAuthorsCount = fullAuthorList != null ? fullAuthorList.Count : 0;

                        this.SearchOnAuthor(this.Searchstring);

                        var sortList = SortLists.SortAuthorList(
                                this.FilteredAuthorList,
                                this.AuthorLastNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

                        this.FilteredAuthorsCount = this.FilteredAuthorList.Count;

                        this.TotalAuthorsstring = StringManipulation.SetTotalAuthorsString(this.FilteredAuthorsCount, this.TotalAuthorsCount);

                        this.ShowCollectionViewFooter = this.FilteredAuthorsCount > 0;

                        await Task.WhenAll(sortList);

                        this.FilteredAuthorList = sortList.Result;
                    }

                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif
                    this.SetIsBusyFalse();
                    RefreshView = false;
                }
            }
        }

        [RelayCommand]
        public void SearchOnAuthor(string? input)
        {
            this.SetIsBusyTrue();

            this.Searchstring = input;

            if (this.FilteredAuthorList != null && this.FullAuthorList != null)
            {
                if (!string.IsNullOrEmpty(this.Searchstring))
                {
                    this.FilteredAuthorList = this.FullAuthorList.Where(x => x.FullName.Contains(this.Searchstring.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                }
                else
                {
                    this.FilteredAuthorList = this.FullAuthorList;
                }

                this.FilteredAuthorsCount = this.FilteredAuthorList != null ? this.FilteredAuthorList.Count : 0;

                this.TotalAuthorsstring = StringManipulation.SetTotalAuthorsString(this.FilteredAuthorsCount, this.TotalAuthorsCount);
            }

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuAuthor(Guid? input)
        {
            if (this.FilteredAuthorList != null)
            {
                var selected = this.FilteredAuthorList.FirstOrDefault(x => x.AuthorGuid == input);

                if (selected != null)
                {
                    var action = await PopupMenu(selected.FullName);

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

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task AddAuthor()
        {
            this.SetIsBusyTrue();

            var view = new AuthorEditView(new AuthorModel(), $"{AppStringResources.AddNewAuthor}", true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditAuthor(AuthorModel selected)
        {
            this.SetIsBusyTrue();

            var view = new AuthorEditView(selected, selected.FullName, true);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteAuthor(AuthorModel selected)
        {
            var answer = await DeleteCheck(selected.FullName);

            if (answer)
            {
                try
                {
                    this.SetIsBusyTrue();

                    await Database.DeleteAuthorAsync(ConvertTo<AuthorDatabaseModel>(selected));
                    this.RemoveFromStaticList(selected);

                    await ConfirmDelete(selected.FullName);

                    await this.SetViewModelData();

                    this.SetIsBusyFalse();
                }
                catch (Exception ex)
                {
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif
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

        private void RemoveFromStaticList(AuthorModel selected)
        {
            if (AuthorsViewModel.fullAuthorList != null)
            {
                AuthorsViewModel.RefreshView = this.RemoveAuthorFromStaticList(selected, AuthorsViewModel.fullAuthorList, AuthorsViewModel.filteredAuthorList);
            }
        }

        private bool RemoveAuthorFromStaticList(AuthorModel selected, ObservableCollection<AuthorModel> authorList, ObservableCollection<AuthorModel>? filteredAuthorList)
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
    }
}
