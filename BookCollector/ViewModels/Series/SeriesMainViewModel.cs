using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views.Book;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Series
{
    public partial class SeriesMainViewModel : SeriesBaseViewModel
    {
        // TO DO
        // Set InfoText string - 11/26/2025
        public SeriesMainViewModel(SeriesModel series, ContentPage view)
        {
            _view = view;

            SelectedSeries = series;
            CollectionViewHeight = DeviceHeight - SingleMenuBar;
            //InfoText = string.Empty;
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            // Unit test data
            var bookList = TestData.BookList;

            Task.WaitAll(
            [
                Task.Run (async () => FullBookList = await FilterLists.GetAllBooksInSeriesList(bookList, SelectedSeries.SeriesGuid) ),
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

        [RelayCommand]
        public async Task AddNewBook()
        {
            SetIsBusyTrue();

            BookModel newBook = new BookModel()
            {
                BookSeriesGuid = SelectedSeries.SeriesGuid,
            };

            BookEditView view = new BookEditView(newBook, $"{AppStringResources.AddNewBook}");

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        // TO DO
        // Set up AddExistingBook - 11/26/2025
        [RelayCommand]
        public async Task AddExistingBook()
        {

        }
    }
}
