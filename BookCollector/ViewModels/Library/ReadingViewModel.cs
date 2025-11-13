using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Library
{
    public partial class ReadingViewModel : BookBaseViewModel
    {
        public ReadingViewModel(ContentPage view)
        {
            CollectionViewHeight = DeviceHeight - DoubleMenuBar;

            SetIsBusyTrue();

            FullBookList = TestData.AddBooksToList();
            TotalBooksCount = FullBookList.Count;

            FilteredBookList = FullBookList;
            FilteredBooksCount = FilteredBookList.Count;

            TotalBooksString = AppStringResources.Blank1OfBlank2Books.Replace("Blank1", FilteredBooksCount.ToString()).Replace("Blank2", TotalBooksCount.ToString()).Replace("books", TotalBooksCount == 1 ? "book" : "book");

            SetIsBusyFalse();
        }

    }
}
