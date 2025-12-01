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

namespace BookCollector.ViewModels.Author
{
    public partial class AuthorMainViewModel : AuthorBaseViewModel
    {
        public AuthorMainViewModel(AuthorModel author, ContentPage view)
        {
            _view = view;

            SelectedAuthor = author;
            CollectionViewHeight = DeviceHeight - SingleMenuBar;
            InfoText = $"{AppStringResources.AuthorMainView_InfoText.Replace("author", $"{SelectedAuthor.FullName}")}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                // Unit test data
                var bookList = TestData.BookList;
                var bookAuthorList = TestData.BookAuthorList;

                // Need a first Task.WaitAll so that anything dependent on this data will have the correct data.
                Task.WaitAll(
                [
                    Task.Run (async () => bookAuthorList = await FilterLists.GetAllBookAuthorsForAuthor(bookAuthorList, SelectedAuthor.AuthorGuid) ),
                ]);

                Task.WaitAll(
                [
                    Task.Run (async () => FullBookList = await FilterLists.GetAllBooksInAuthorList(bookAuthorList, bookList) ),
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
                AuthorListString = SelectedAuthor.ReverseFullName,
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
