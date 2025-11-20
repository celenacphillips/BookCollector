using BookCollector.Data;
using BookCollector.Resources.Localization;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Library
{
    public partial class ToBeReadViewModel : BookBaseViewModel
    {
        public ToBeReadViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.ToBeReadView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            Task.WaitAll(
            [
                Task.Run (async () => FullBookList = await FilterLists.GetToBeReadBooksList(TestData.BookList) ),
            ]);

            TotalBooksCount = FullBookList.Count;

            FilteredBookList = FullBookList;
            FilteredBooksCount = FilteredBookList.Count;

            TotalBooksString = StringManipulation.SetTotalBooksString(FilteredBooksCount, TotalBooksCount);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }
    }
}
