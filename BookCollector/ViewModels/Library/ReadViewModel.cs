using BookCollector.Data;
using BookCollector.Resources.Localization;

namespace BookCollector.ViewModels.Library
{
    public partial class ReadViewModel : BookBaseViewModel
    {
        public ReadViewModel(ContentPage view)
        {
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            FullBookList = FilterLists.GetReadBooksList(TestData.BookList);
            TotalBooksCount = FullBookList.Count;

            FilteredBookList = FullBookList;
            FilteredBooksCount = FilteredBookList.Count;

            TotalBooksString = StringManipulation.SetTotalBooksString(FilteredBooksCount, TotalBooksCount);

            SetIsBusyFalse();
        }
    }
}
