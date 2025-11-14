using BookCollector.Data;
using BookCollector.Resources.Localization;

namespace BookCollector.ViewModels.Library
{
    public partial class ReadViewModel : BookBaseViewModel
    {
        public ReadViewModel(ContentPage view)
        {
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;

            SetIsBusyTrue();

            FullBookList = TestData.GetReadBooksList();
            TotalBooksCount = FullBookList.Count;

            FilteredBookList = FullBookList;
            FilteredBooksCount = FilteredBookList.Count;

            TotalBooksString = AppStringResources.Blank1OfBlank2Books.Replace("Blank1", FilteredBooksCount.ToString()).Replace("Blank2", TotalBooksCount.ToString()).Replace("books", TotalBooksCount == 1 ? "book" : "books");

            SetIsBusyFalse();
        }
    }
}
