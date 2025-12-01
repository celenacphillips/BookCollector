using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views.Book;
using BookCollector.Views.Collection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Collection
{
    public partial class CollectionMainViewModel : CollectionBaseViewModel
    {
        public CollectionMainViewModel(CollectionModel collection, ContentPage view)
        {
            _view = view;

            SelectedCollection = collection;
            CollectionViewHeight = DeviceHeight - SingleMenuBar;
            InfoText = $"{AppStringResources.CollectionMainView_InfoText.Replace("collection", $"{SelectedCollection.CollectionName}")}";
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            // Unit test data
            var bookList = TestData.BookList;

            Task.WaitAll(
            [
                Task.Run (async () => FullBookList = await FilterLists.GetAllBooksInCollectionList(bookList, SelectedCollection.CollectionGuid) ),
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
        public async Task AddNewBook ()
        {
            SetIsBusyTrue();

            BookModel newBook = new BookModel()
            {
                BookCollectionGuid = SelectedCollection.CollectionGuid,
            };

            BookEditView view = new BookEditView(newBook, $"{AppStringResources.AddNewBook}");

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        // TO DO
        // Set up AddExistingBook - 11/25/2025
        [RelayCommand]
        public async Task AddExistingBook()
        {

        }
    }
}
