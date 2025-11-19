using BookCollector.Data;
using BookCollector.Resources.Localization;

namespace BookCollector.ViewModels.Library
{
    public partial class ToBeReadViewModel : BookBaseViewModel
    {
        public ToBeReadViewModel(ContentPage view)
        {
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
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
    }
}
