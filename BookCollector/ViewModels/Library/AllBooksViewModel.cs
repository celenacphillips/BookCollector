using BookCollector.Data;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Library
{
    public partial class AllBooksViewModel : BookBaseViewModel
    {
        public AllBooksViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.AllBooksView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                var showHiddenBooks = Preferences.Get("HiddenBooksOn", true  /* Default */);

                // Unit test data
                var bookList = TestData.BookList;

                Task.WaitAll(
                [
                    Task.Run (async () => FullBookList = await FilterLists.GetAllBooksList(bookList, showHiddenBooks) ),
                ]);

                TotalBooksCount = FullBookList.Count;

                FilteredBookList = FullBookList;
                FilteredBooksCount = FilteredBookList.Count;

                TotalBooksString = StringManipulation.SetTotalBooksString(FilteredBooksCount, TotalBooksCount);

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
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
