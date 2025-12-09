using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Book;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class BookBaseViewModel: BaseViewModel
    {
        public bool ShowHiddenBook { get; set; }
        public bool ShowFavoriteBooks { get; set; }
        public bool ShowBookRatings { get; set; }
        public string? FavoriteBooksOption { get; set; }
        public string? BookFormatOption { get; set; }
        public string? BookPublisherOption { get; set; }
        public string? BookPublishYearOption { get; set; }
        public string? BookLanguageOption { get; set; }
        public string? BookRatingOption { get; set; }
        public bool BookTitleChecked { get; set; }
        public bool BookReadingDateChecked { get; set; }
        public bool BookReadPercentageChecked { get; set; }
        public bool BookPublisherChecked { get; set; }
        public bool BookPublishYearChecked { get; set; }
        public bool AuthorLastNameChecked { get; set; }
        public bool BookFormatChecked { get; set; }
        public bool PageCountChecked { get; set; }
        public bool BookPriceChecked { get; set; }


        [ObservableProperty]
        public string? totalBooksString;

        [ObservableProperty]
        public int totalBooksCount;

        [ObservableProperty]
        public int filteredBooksCount;

        [ObservableProperty]
        internal static ObservableCollection<BookModel>? fullBookList;

        [ObservableProperty]
        internal static ObservableCollection<BookModel>? filteredBookList;

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
        public LocationModel? selectedLocation;

        [ObservableProperty]
        internal static ObservableCollection<string>? bookFormats;

        [ObservableProperty]
        public ImageSource? bookCover;

        [ObservableProperty]
        public ObservableCollection<string>? bookPublisherList;

        [ObservableProperty]
        public ObservableCollection<string>? bookPublishYearList;

        [ObservableProperty]
        public ObservableCollection<string>? bookLanguageList;

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

            if (FilteredBookList != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                    FilteredBookList = FilteredBookList.Where(x => !string.IsNullOrEmpty(x.BookTitle) && x.BookTitle.Contains(SearchString.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase)).ToObservableCollection();
                else
                    FilteredBookList = FullBookList;

                FilteredBooksCount = FilteredBookList != null ? FilteredBookList.Count : 0;

                TotalBooksString = StringManipulation.SetTotalBooksString(FilteredBooksCount, TotalBooksCount);
            }

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task BookSelectionChanged()
        {
            if (SelectedBook != null && !string.IsNullOrEmpty(SelectedBook.BookTitle))
            {
                var view = new BookMainView(SelectedBook, SelectedBook.BookTitle);

                await Shell.Current.Navigation.PushAsync(view);
                SelectedBook = null;
            }
        }

        [RelayCommand]
        public async Task AddBook()
        {
            SetIsBusyTrue();

            var view = new BookEditView(new BookModel(), $"{AppStringResources.AddNewBook}");

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public void ReadingDataChanged()
        {
            ReadingDataOpen = ReadingDataSectionValue;
            ReadingDataNotOpen = !ReadingDataSectionValue;
        }

        [RelayCommand]
        public void ChapterListChanged()
        {
            ChapterListOpen = ChapterListSectionValue;
            ChapterListNotOpen = !ChapterListSectionValue;
        }

        [RelayCommand]
        public void AuthorListChanged()
        {
            AuthorListOpen = AuthorListSectionValue;
            AuthorListNotOpen = !AuthorListSectionValue;
        }

        [RelayCommand]
        public void BookInfoChanged()
        {
            BookInfoOpen = BookInfoSectionValue;
            BookInfoNotOpen = !BookInfoSectionValue;
        }

        [RelayCommand]
        public void SummaryChanged()
        {
            SummaryOpen = SummarySectionValue;
            SummaryNotOpen = !SummarySectionValue;
        }

        [RelayCommand]
        public void CommentsChanged()
        {
            CommentsOpen = CommentsSectionValue;
            CommentsNotOpen = !CommentsSectionValue;
        }

        [RelayCommand]
        public void BookCoverPopup()
        {
            View.ShowPopup(new BookCoverPopup(BookCover));
        }

        public static ObservableCollection<AuthorModel> ParseOutAuthorsFromString(string inputString)
        {
            var authorList = new ObservableCollection<AuthorModel>();

            string[] authorNames = inputString.Split(";");

            foreach (var authorName in authorNames)
            {
                if (!string.IsNullOrEmpty(authorName.Trim()))
                {
                    string[] name = authorName.Split(",");

                    AuthorModel? author = null;

                    if (TestData.UseTestData)
                    {
                        author = TestData.AuthorList.FirstOrDefault(x => !string.IsNullOrEmpty(x.FirstName) &&
                                                                         x.FirstName.Equals(name[1].Trim()) &&
                                                                         !string.IsNullOrEmpty(x.LastName) &&
                                                                         x.LastName.Equals(name[0].Trim()));
                    }
                    else
                    {

                    }

                    author ??= new()
                        {
                            FirstName = name[1].Trim(),
                            LastName = name[0].Trim()
                        };

                    authorList.Add(author);
                }
            }

            return authorList;
        }
    }
}
