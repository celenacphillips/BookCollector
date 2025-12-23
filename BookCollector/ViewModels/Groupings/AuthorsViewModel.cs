// <copyright file="AuthorsViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Author;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Author;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        }

        private bool ShowHiddenAuthors { get; set; }

        private bool TotalBooksChecked { get; set; }

        private bool TotalPriceChecked { get; set; }

        public async Task SetViewModelData()
        {
            try
            {
                this.SetIsBusyTrue();

                this.GetPreferences();

                var fullList = FillLists.GetAllAuthorsList(this.ShowHiddenAuthors);

                await Task.WhenAll(fullList);

                this.FullAuthorList = fullList.Result;

                if (this.FullAuthorList != null)
                {
                    this.FilteredAuthorList = this.FullAuthorList;

                    this.SearchOnAuthor(this.Searchstring);

                    var loadDataTasks = new Task[]
                    {
                        Task.Run(() => this.FilteredAuthorList.ToList().ForEach(x => x.SetTotalBooks(this.ShowHiddenBook))),
                        Task.Run(() => this.FilteredAuthorList.ToList().ForEach(x => x.SetTotalCostOfBooks(this.ShowHiddenBook))),
                    };

                    await Task.WhenAll(loadDataTasks);

                    var sortList = SortLists.SortAuthorList(
                            this.FilteredAuthorList,
                            this.AuthorLastNameChecked,
                            this.TotalBooksChecked,
                            this.TotalPriceChecked,
                            this.AscendingChecked,
                            this.DescendingChecked);

                    this.TotalAuthorsCount = this.FullAuthorList.Count;

                    this.FilteredAuthorsCount = this.FilteredAuthorList.Count;

                    this.TotalAuthorsstring = StringManipulation.SetTotalAuthorsString(this.FilteredAuthorsCount, this.TotalAuthorsCount);

                    this.ShowCollectionViewFooter = this.FilteredAuthorsCount > 0;

                    await Task.WhenAll(sortList);

                    this.FilteredAuthorList = sortList.Result;
                }

                this.SetIsBusyFalse();
            }
            catch (Exception ex)
            {
#if DEBUG
                await DisplayMessage("Error!", ex.Message);
#endif
                this.SetIsBusyFalse();
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

                    if (TestData.UseTestData)
                    {
                        TestData.DeleteAuthor(selected);
                    }
                    else
                    {
                        await Database.DeleteAuthorAsync(ConvertTo<AuthorDatabaseModel>(selected));
                    }

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

                await this.View.ShowPopupAsync(popup);
                await this.SetViewModelData();
            }
        }

        private void GetPreferences()
        {
            this.ShowHiddenAuthors = Preferences.Get("HiddenAuthorsOn", true /* Default */);
            this.ShowHiddenBook = Preferences.Get("HiddenBooksOn", true /* Default */);

            this.AuthorLastNameChecked = Preferences.Get($"{this.ViewTitle}_AuthorLastNameSelection", true /* Default */);
            this.TotalBooksChecked = Preferences.Get($"{this.ViewTitle}_TotalBooksSelection", false /* Default */);
            this.TotalPriceChecked = Preferences.Get($"{this.ViewTitle}_TotalPriceSelection", false /* Default */);

            this.AscendingChecked = Preferences.Get($"{this.ViewTitle}_AscendingSelection", true /* Default */);
            this.DescendingChecked = Preferences.Get($"{this.ViewTitle}_DescendingSelection", false /* Default */);
        }
    }
}
