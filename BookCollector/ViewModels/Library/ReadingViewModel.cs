using BookCollector.Data;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Views;
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

                var showHiddenBooks = Preferences.Get("HiddenBooksOn", true  /* Default */);
                var favoriteBooksOption = Preferences.Get("Reading_FavoriteSelection", AppStringResources.Both  /* Default */);

                // Unit test data
                var bookList = TestData.BookList;

                FullBookList = new System.Collections.ObjectModel.ObservableCollection<Data.Models.BookModel>();

                //Task.WaitAll(
                //[
                //    Task.Run (async () => FullBookList = await FilterLists.GetReadingBooksList(bookList,
                //                                                                               showHiddenBooks,
                //                                                                               favoriteBooksOption) ),
                //]);

                TotalBooksCount = FullBookList.Count;

                FilteredBookList = FullBookList;
                FilteredBooksCount = FilteredBookList.Count;

                TotalBooksString = StringManipulation.SetTotalBooksString(FilteredBooksCount, TotalBooksCount);

                ShowCollectionViewFooter = FilteredBooksCount > 0;

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

        [RelayCommand]
        public async Task FilterPopup1()
        {
            var result = await _view.ShowPopupAsync(new FilterPopup(AppStringResources.Reading));
            await SetViewModelData();
        }
    }
}
