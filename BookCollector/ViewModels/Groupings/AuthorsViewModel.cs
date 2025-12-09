using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Author;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Author;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
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
    public partial class AuthorsViewModel : AuthorBaseViewModel
    {
        [ObservableProperty]
        public string? totalAuthorsString;

        private bool ShowHiddenAuthors { get; set; }
        private bool TotalBooksChecked { get; set; }
        private bool TotalPriceChecked { get; set; }

        public AuthorsViewModel(ContentPage view)
        {
            View = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.AuthorView_InfoText}";
            ViewTitle = AppStringResources.Authors;
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                GetPreferences();

                Task.WaitAll(
                [
                    Task.Run (async () => FullAuthorList = await FilterLists.GetAllAuthorsList(ShowHiddenAuthors) ),
                ]);

                if (FullAuthorList != null)
                {
                    TotalAuthorsCount = FullAuthorList.Count;

                    FilteredAuthorList = FullAuthorList;

                    foreach (var author in FullAuthorList)
                    {
                        await author.SetTotalBooks(ShowHiddenBook);
                    }

                    Task.WaitAll(
                    [
                        Task.Run (async () => FilteredAuthorList = await FilterLists.SortAuthorList(FilteredAuthorList,
                                                                                                    AuthorLastNameChecked,
                                                                                                    TotalBooksChecked,
                                                                                                    TotalPriceChecked,
                                                                                                    AscendingChecked,
                                                                                                    DescendingChecked) ),
                    ]);

                    FilteredAuthorsCount = FilteredAuthorList.Count;

                    TotalAuthorsString = StringManipulation.SetTotalAuthorsString(FilteredAuthorsCount, TotalAuthorsCount);

                    ShowCollectionViewFooter = FilteredAuthorsCount > 0;
                }

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public void SearchOnAuthor(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (FilteredAuthorList != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                    FilteredAuthorList = FilteredAuthorList.Where(x => x.FullName.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                else
                    FilteredAuthorList = FullAuthorList;

                FilteredAuthorsCount = FilteredAuthorList != null ? FilteredAuthorList.Count : 0;

                TotalAuthorsString = StringManipulation.SetTotalAuthorsString(FilteredAuthorsCount, TotalAuthorsCount);
            }

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuAuthor(Guid? input)
        {
            if (FilteredAuthorList != null)
            {
                var selected = FilteredAuthorList.FirstOrDefault(x => x.AuthorGuid == input);

                if (selected != null)
                {
                    var action = await PopupMenu(selected.FullName);

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                    {
                        await EditAuthor(selected);
                    }

                    if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                    {
                        await DeleteAuthor(selected);
                    }
                }
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
        public async Task AddAuthor()
        {
            SetIsBusyTrue();

            var view = new AuthorEditView(new AuthorModel(), $"{AppStringResources.AddNewAuthor}");

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditAuthor(AuthorModel selected)
        {
            SetIsBusyTrue();

            var view = new AuthorEditView(selected, selected.FullName);
            var bindingContext = new AuthorEditViewModel(selected, view)
            {
                ViewTitle = $"{AppStringResources.EditAuthor}"
            };
            view.BindingContext = bindingContext;

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteAuthor(AuthorModel selected)
        {
            var answer = await DeleteCheck(selected.FullName);

            if (answer)
            {
                try
                {
                    SetIsBusyTrue();

                    if (TestData.UseTestData)
                    {
                        TestData.DeleteAuthor(selected);
                    }
                    else
                    {

                    }

                    await ConfirmDelete(selected.FullName);

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
            if (!string.IsNullOrEmpty(ViewTitle))
            {
                var popup = new SortPopup();
                var viewModel = new SortPopupViewModel(popup, ViewTitle)
                {
                    AuthorLastNameVisible = true,
                    AuthorLastNameChecked = AuthorLastNameChecked,
                    TotalBooksVisible = true,
                    TotalBooksChecked = TotalBooksChecked,
                    TotalPriceVisible = true,
                    TotalPriceChecked = TotalPriceChecked,
                    AscendingChecked = AscendingChecked,
                    DescendingChecked = DescendingChecked,
                };

                popup.BindingContext = viewModel;

                await View.ShowPopupAsync(popup);
                await SetViewModelData();
            }
        }

        private void GetPreferences()
        {
            ShowHiddenAuthors = Preferences.Get("HiddenAuthorsOn", true  /* Default */);
            ShowHiddenBook = Preferences.Get("HiddenBooksOn", true  /* Default */);

            AuthorLastNameChecked = Preferences.Get($"{ViewTitle}_AuthorLastNameSelection", true  /* Default */);
            TotalBooksChecked = Preferences.Get($"{ViewTitle}_TotalBooksSelection", false  /* Default */);
            TotalPriceChecked = Preferences.Get($"{ViewTitle}_TotalPriceSelection", false  /* Default */);

            AscendingChecked = Preferences.Get($"{ViewTitle}_AscendingSelection", true  /* Default */);
            DescendingChecked = Preferences.Get($"{ViewTitle}_DescendingSelection", false  /* Default */);
        }
    }
}
