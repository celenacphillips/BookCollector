using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Renderscripts.ScriptGroup;

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
            SetIsBusyTrue();

            Task.WaitAll(
            [
                Task.Run (async () => FullAuthorList = await FilterLists.GetAllAuthorsList(TestData.AuthorList) ),
            ]);

            TotalAuthorsCount = FullAuthorList.Count;

            FilteredAuthorList = FullAuthorList;
            FilteredAuthorsCount = FilteredAuthorList.Count;

            TotalAuthorsString = StringManipulation.SetTotalAuthorsString(FilteredAuthorsCount, TotalAuthorsCount);

            SetIsBusyFalse();
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

        // TO DO
        [RelayCommand]
        public async Task AddAuthor()
        {

        }

        // TO DO
        [RelayCommand]
        public async Task EditAuthor(AuthorModel selected)
        {

        }

        // TO DO
        [RelayCommand]
        public async Task DeleteAuthor(AuthorModel selected)
        {

        }
    }
}
