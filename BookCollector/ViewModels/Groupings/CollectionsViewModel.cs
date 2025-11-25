using BookCollector.Data;
using BookCollector.Resources.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Groupings
{
    public partial class CollectionsViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string? totalCollectionsString;

        [ObservableProperty]
        public int totalCollectionsCount;

        public CollectionsViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            //InfoText = $"{AppStringResources.AllBooksView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            //Task.WaitAll(
            //[
            //    Task.Run (async () => FullBookList = await FilterLists.GetAllBooksList(TestData.BookList) ),
            //]);

            //TotalBooksCount = FullBookList.Count;

            //FilteredBookList = FullBookList;
            //FilteredBooksCount = FilteredBookList.Count;

            //TotalBooksString = StringManipulation.SetTotalBooksString(FilteredBooksCount, TotalBooksCount);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SearchOnCollection()
        {
            
        }

        [RelayCommand]
        public async Task PopupMenu()
        {

        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        [RelayCommand]
        public async Task AddCollection()
        {

        }
    }
}
