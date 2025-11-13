using BookCollector.Data;
using BookCollector.Data.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Book
{
    public partial class BookMainViewModel: BookBaseViewModel
    {
        [ObservableProperty]
        public bool bookIsRead;

        [ObservableProperty]
        public string? partOfSeriesString;

        [ObservableProperty]
        public string? partOfCollectionString;

        [ObservableProperty]
        public bool readingDataValue;

        [ObservableProperty]
        public bool readingDataOpen;

        [ObservableProperty]
        public bool readingDataNotOpen;

        [ObservableProperty]
        public int half;

        [ObservableProperty]
        public int fourth;

        [ObservableProperty]
        public int threeFourth;

        [ObservableProperty]
        public bool chapterListValue;

        [ObservableProperty]
        public bool chapterListOpen;

        [ObservableProperty]
        public bool chapterListNotOpen;

        [ObservableProperty]
        public bool authorListValue;

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
        public bool bookInfoValue;

        [ObservableProperty]
        public bool bookInfoOpen;

        [ObservableProperty]
        public bool bookInfoNotOpen;

        [ObservableProperty]
        public bool summaryValue;

        [ObservableProperty]
        public bool summaryOpen;

        [ObservableProperty]
        public bool summaryNotOpen;

        [ObservableProperty]
        public bool commentsValue;

        [ObservableProperty]
        public bool commentsOpen;

        [ObservableProperty]
        public bool commentsNotOpen;

        [ObservableProperty]
        public GenreModel? selectedGenre;

        public BookMainViewModel(BookModel book, ContentPage view)
        {
            SetIsBusyTrue();

            /* Base ViewModel */
            ShowComments = Preferences.Get("CommentsOn", true  /* Default */);
            ShowChapters = Preferences.Get("ChaptersOn", true  /* Default */);
            ShowFavorites = Preferences.Get("FavoritesOn", true  /* Default */);
            ShowRatings = Preferences.Get("RatingsOn", true  /* Default */);

            SelectedBook = book;

            ChapterList = TestData.AddChaptersToList();
            AuthorList = TestData.AddAuthorsToList();

            ReadingDataValue = true;
            ReadingDataChanged();
            ChapterListValue = true;
            ChapterListChanged();
            AuthorListValue = true;
            AuthorListChanged();
            BookInfoValue = true;
            BookInfoChanged();
            SummaryValue = true;
            SummaryChanged();
            CommentsValue = true;
            CommentsChanged();

            SetIsBusyFalse();
        }

        [RelayCommand]
        public void ReadingDataChanged()
        {
            ReadingDataOpen = ReadingDataValue;
            ReadingDataNotOpen = !ReadingDataValue;
        }

        [RelayCommand]
        public void ChapterListChanged()
        {
            ChapterListOpen = ChapterListValue;
            ChapterListNotOpen = !ChapterListValue;
        }

        [RelayCommand]
        public void AuthorListChanged()
        {
            AuthorListOpen = AuthorListValue;
            AuthorListNotOpen = !AuthorListValue;
        }

        [RelayCommand]
        public void BookInfoChanged()
        {
            BookInfoOpen = BookInfoValue;
            BookInfoNotOpen = !BookInfoValue;
        }

        [RelayCommand]
        public void SummaryChanged()
        {
            SummaryOpen = SummaryValue;
            SummaryNotOpen = !SummaryValue;
        }

        [RelayCommand]
        public void CommentsChanged()
        {
            CommentsOpen = CommentsValue;
            CommentsNotOpen = !CommentsValue;
        }
    }
}
