using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Groupings
{
    public partial class LocationsViewModel : BaseViewModel
    {
        public LocationsViewModel(ContentPage view)
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
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        [RelayCommand]
        public async Task AddLocation()
        {

        }
    }
}
