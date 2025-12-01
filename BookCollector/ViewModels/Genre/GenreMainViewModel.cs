using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Book;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Genre
{
    public partial class GenreMainViewModel : GenreBaseViewModel
    {
        public GenreMainViewModel(GenreModel genre, ContentPage view)
        {
            _view = view;

            SelectedGenre = genre;
            CollectionViewHeight = DeviceHeight - SingleMenuBar;
            InfoText = $"{AppStringResources.GenreMainView_InfoText.Replace("genre", $"{SelectedGenre.GenreName}")}";
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
                    Task.Run (async () => FullBookList = await FilterLists.GetAllBooksInGenreList(bookList, SelectedGenre.GenreGuid) ),
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

        [RelayCommand]
        public async Task AddNewBook()
        {
            SetIsBusyTrue();

            BookModel newBook = new BookModel()
            {
                BookGenreGuid = SelectedGenre.GenreGuid,
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
