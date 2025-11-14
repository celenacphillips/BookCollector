using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views.Book;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels
{
    public partial class BookBaseViewModel: BaseViewModel
    {
        [ObservableProperty]
        public string? totalBooksString;

        [ObservableProperty]
        public int totalBooksCount;

        [ObservableProperty]
        public int filteredBooksCount;

        [ObservableProperty]
        public string? searchString;

        [ObservableProperty]
        public static ObservableCollection<BookModel>? fullBookList;

        [ObservableProperty]
        public static ObservableCollection<BookModel>? filteredBookList;

        [ObservableProperty]
        public BookModel? selectedBook;

        public BookBaseViewModel()
        {
        }

        [RelayCommand]
        public void BookSearchOnTitle(string? input)
        {
            SetIsBusyTrue();

            SearchString = input;

            if (!string.IsNullOrEmpty(SearchString))
                FilteredBookList = FilteredBookList.Where(x => x.BookTitle.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
            else
                FilteredBookList = FullBookList;

            FilteredBooksCount = FilteredBookList.Count;

            TotalBooksString = AppStringResources.Blank1OfBlank2Books.Replace("Blank1", FilteredBooksCount.ToString()).Replace("Blank2", TotalBooksCount.ToString()).Replace("books", TotalBooksCount == 1 ? "book" : "book");

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task BookSelectionChanged()
        {
            BookMainView view = new BookMainView(SelectedBook, SelectedBook.BookTitle);

            await Shell.Current.Navigation.PushAsync(view);
            SelectedBook = null;
        }

        [RelayCommand]
        public async Task AddBook()
        {

        }
        [RelayCommand]
        public async Task EditBook()
        {

        }

        [RelayCommand]
        public async Task DeleteBook()
        {

        }

    }
}
