using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Author;
using BookCollector.Views.Author;
using CommunityToolkit.Maui.Core.Extensions;
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

        public AuthorsViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.AuthorView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                // Unit test data
                var authorList = TestData.AuthorList;

                Task.WaitAll(
                [
                    Task.Run (async () => FullAuthorList = await FilterLists.GetAllAuthorsList(authorList) ),
                ]);

                TotalAuthorsCount = FullAuthorList.Count;

                FilteredAuthorList = FullAuthorList;
                FilteredAuthorsCount = FilteredAuthorList.Count;

                TotalAuthorsString = StringManipulation.SetTotalAuthorsString(FilteredAuthorsCount, TotalAuthorsCount);

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
            string? action = await BaseViewModel.PopupMenu(selected.FullName);

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

                    // Unit test data
                    TestData.DeleteAuthor(selected);

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
    }
}
