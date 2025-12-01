using BookCollector.Data;
using BookCollector.Resources.Localization;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Library
{
    public partial class ReadingViewModel : BookBaseViewModel
    {
        public ReadingViewModel(ContentPage view)
        {
            _view = view;
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
            InfoText = $"{AppStringResources.ReadingView_InfoText}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                // Unit test data
                var bookList = TestData.BookList;

                Task.WaitAll(
                [
                    Task.Run (async () => FullBookList = await FilterLists.GetReadingBooksList(bookList) ),
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
