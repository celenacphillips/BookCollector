using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Book;
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

        [ObservableProperty]
        public bool bookIsRead;

        [ObservableProperty]
        public bool showUpNext;

        [ObservableProperty]
        public bool readingDataSectionValue;

        [ObservableProperty]
        public bool readingDataOpen;

        [ObservableProperty]
        public bool readingDataNotOpen;

        [ObservableProperty]
        public bool chapterListSectionValue;

        [ObservableProperty]
        public bool chapterListOpen;

        [ObservableProperty]
        public bool chapterListNotOpen;

        [ObservableProperty]
        public bool authorListSectionValue;

        [ObservableProperty]
        public bool authorListOpen;

        [ObservableProperty]
        public bool authorListNotOpen;

        [ObservableProperty]
        public bool showComments;

        [ObservableProperty]
        public bool showChapters;

        [ObservableProperty]
        public bool showFavorites;

        [ObservableProperty]
        public bool showRatings;

        [ObservableProperty]
        public ObservableCollection<ChapterModel>? chapterList;

        [ObservableProperty]
        public ObservableCollection<AuthorModel>? authorList;

        [ObservableProperty]
        public bool bookInfoSectionValue;

        [ObservableProperty]
        public bool bookInfoOpen;

        [ObservableProperty]
        public bool bookInfoNotOpen;

        [ObservableProperty]
        public bool summarySectionValue;

        [ObservableProperty]
        public bool summaryOpen;

        [ObservableProperty]
        public bool summaryNotOpen;

        [ObservableProperty]
        public bool commentsSectionValue;

        [ObservableProperty]
        public bool commentsOpen;

        [ObservableProperty]
        public bool commentsNotOpen;

        [ObservableProperty]
        public GenreModel? selectedGenre;

        [ObservableProperty]
        public static ObservableCollection<string> bookFormats;

        [ObservableProperty]
        public ImageSource? bookCover;

        public BookBaseViewModel()
        {
            ShowComments = Preferences.Get("CommentsOn", true  /* Default */);
            ShowChapters = Preferences.Get("ChaptersOn", true  /* Default */);
            ShowFavorites = Preferences.Get("FavoritesOn", true  /* Default */);
            ShowRatings = Preferences.Get("RatingsOn", true  /* Default */);
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

        // TO DO:
        // Fix Add Book to not add to main list without save - 11/19/2025
        [RelayCommand]
        public async Task AddBook()
        {
            SetIsBusyTrue();

            BookModel newBook = new BookModel();

            BookMainView view = new BookMainView(newBook, $"{AppStringResources.AddNewBook}");
            TestData.InsertBook(newBook);

            await Shell.Current.Navigation.PushAsync(view);

            //SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task ReadingDataChanged()
        {
            ReadingDataOpen = ReadingDataSectionValue;
            ReadingDataNotOpen = !ReadingDataSectionValue;
        }

        [RelayCommand]
        public async Task ChapterListChanged()
        {
            ChapterListOpen = ChapterListSectionValue;
            ChapterListNotOpen = !ChapterListSectionValue;
        }

        [RelayCommand]
        public async Task AuthorListChanged()
        {
            AuthorListOpen = AuthorListSectionValue;
            AuthorListNotOpen = !AuthorListSectionValue;
        }

        [RelayCommand]
        public async Task BookInfoChanged()
        {
            BookInfoOpen = BookInfoSectionValue;
            BookInfoNotOpen = !BookInfoSectionValue;
        }

        [RelayCommand]
        public async Task SummaryChanged()
        {
            SummaryOpen = SummarySectionValue;
            SummaryNotOpen = !SummarySectionValue;
        }

        [RelayCommand]
        public async Task CommentsChanged()
        {
            CommentsOpen = CommentsSectionValue;
            CommentsNotOpen = !CommentsSectionValue;
        }
    }
}
