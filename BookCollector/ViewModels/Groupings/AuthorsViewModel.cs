using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Author;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Popups;
using BookCollector.Views.Author;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
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
        private bool AuthorLastNameChecked { get; set; }
        private bool TotalBooksChecked { get; set; }
        private bool TotalPriceChecked { get; set; }

        public AuthorsViewModel(ContentPage view)
        {
            _view = view;
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

                TotalAuthorsCount = FullAuthorList.Count;

                FilteredAuthorList = FullAuthorList;

                foreach (var author in FullAuthorList)
                {
                    author.SetTotalBooks(ShowHiddenBook);
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

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task SearchOnAuthor(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (!string.IsNullOrEmpty(SearchString))
                FilteredAuthorList = FilteredAuthorList.Where(x => x.FullName.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            else
                FilteredAuthorList = FullAuthorList;

            FilteredAuthorsCount = FilteredAuthorList.Count;

            TotalAuthorsString = StringManipulation.SetTotalAuthorsString(FilteredAuthorsCount, TotalAuthorsCount);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task PopupMenuAuthor(Guid? input)
        {
            var selected = FilteredAuthorList.FirstOrDefault(x => x.AuthorGuid == input);
            string? action = await PopupMenu(selected.FullName);

            switch (action)
            {
                case "Edit":
                    await EditAuthor(selected);
                    break;

                case "Delete":
                    await DeleteAuthor(selected);
                    break;

                default:
                    break;
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

            AuthorEditView view = new AuthorEditView(new AuthorModel(), $"{AppStringResources.AddNewAuthor}");

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task EditAuthor(AuthorModel selected)
        {
            SetIsBusyTrue();

            AuthorEditView view = new AuthorEditView(selected, selected.FullName);
            AuthorEditViewModel bindingContext = new AuthorEditViewModel(selected, view);
            bindingContext.ViewTitle = $"{AppStringResources.EditAuthor}";
            view.BindingContext = bindingContext;

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteAuthor(AuthorModel selected)
        {
            bool answer = await DeleteCheck(selected.FullName);

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
            var popup = new SortPopup();
            SortPopupViewModel viewModel = new SortPopupViewModel(popup, ViewTitle)
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

            await _view.ShowPopupAsync(popup);
            await SetViewModelData();
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
