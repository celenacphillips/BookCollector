// <copyright file="BookBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class BookBaseViewModel : BaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<BookModel>? fullBookList;

        [ObservableProperty]
        public static ObservableCollection<BookModel>? filteredBookList;

        [ObservableProperty]
        public static ObservableCollection<string>? bookFormats;

        [ObservableProperty]
        public string? totalBooksString;

        [ObservableProperty]
        public int totalBooksCount;

        [ObservableProperty]
        public int filteredBooksCount;

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
        public ImageSource? bookCover;

        [ObservableProperty]
        public ObservableCollection<string>? bookPublisherList;

        [ObservableProperty]
        public ObservableCollection<string>? bookPublishYearList;

        [ObservableProperty]
        public ObservableCollection<string>? bookLanguageList;

        public BookBaseViewModel()
        {
            this.ShowComments = Preferences.Get("CommentsOn", true /* Default */);
            this.ShowChapters = Preferences.Get("ChaptersOn", true /* Default */);
            this.ShowFavorites = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowRatings = Preferences.Get("RatingsOn", true /* Default */);
        }

        public bool ShowHiddenBook { get; set; }

        public bool HiddenAuthorsOn { get; set; }

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

        public static async Task<ObservableCollection<AuthorModel>> ParseOutAuthorsFromstring(string? inputstring, bool showHiddenAuthors = true)
        {
            var authorList = new ObservableCollection<AuthorModel>();

            if (!string.IsNullOrEmpty(inputstring))
            {
                var list = SplitStringIntoAuthorList(inputstring);

                foreach (var item in list)
                {
                    if (item != null)
                    {
                        AuthorModel? author = null;
                        bool skip = false;

                        if (TestData.UseTestData)
                        {
                            author = TestData.AuthorList.FirstOrDefault(x => !string.IsNullOrEmpty(x.FirstName) &&
                                                                             x.FirstName.Equals(item.FirstName) &&
                                                                             !string.IsNullOrEmpty(x.LastName) &&
                                                                             x.LastName.Equals(item.LastName));
                        }
                        else
                        {
                            author = await Database.GetAuthorByNameAsync(item.FirstName, item.LastName);
                        }

                        // if (!showHiddenAuthors)
                        // {
                        //    if (author != null && author.HideAuthor)
                        //    {
                        //        skip = true;
                        //    }
                        // }
                        if (!skip)
                        {
                            if (author == null)
                            {
                                author = new AuthorModel();
                            }

                            author.FirstName = item.FirstName;
                            author.LastName = item.LastName;

                            authorList.Add(author);
                        }
                    }
                }
            }

            return authorList;
        }

        [RelayCommand]
        public async void BookSearchOnTitle(string? input)
        {
            this.SetIsBusyTrue();

            this.Searchstring = input;

            if (this.FilteredBookList != null && this.FullBookList != null)
            {
                this.FilteredBookList = await FilterLists.FilterBookList(
                                this.FullBookList,
                                this.FavoriteBooksOption,
                                this.BookFormatOption,
                                this.BookPublisherOption,
                                this.BookLanguageOption,
                                this.BookRatingOption,
                                this.BookPublishYearOption,
                                this.Searchstring);

                this.FilteredBooksCount = this.FilteredBookList != null ? this.FilteredBookList.Count : 0;

                this.TotalBooksString = StringManipulation.SetTotalBooksString(this.FilteredBooksCount, this.TotalBooksCount);
            }

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task BookSelectionChanged()
        {
            if (this.SelectedBook != null && !string.IsNullOrEmpty(this.SelectedBook.BookTitle))
            {
                var view = new BookMainView(this.SelectedBook, this.SelectedBook.BookTitle);

                await Shell.Current.Navigation.PushAsync(view);
                this.SelectedBook = null;
            }
        }

        [RelayCommand]
        public async Task AddBook()
        {
            this.SetIsBusyTrue();

            var view = new BookEditView(new BookModel(), $"{AppStringResources.AddNewBook}");

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public void ReadingDataChanged()
        {
            this.ReadingDataOpen = this.ReadingDataSectionValue;
            this.ReadingDataNotOpen = !this.ReadingDataSectionValue;
        }

        [RelayCommand]
        public void ChapterListChanged()
        {
            this.ChapterListOpen = this.ChapterListSectionValue;
            this.ChapterListNotOpen = !this.ChapterListSectionValue;
        }

        [RelayCommand]
        public void AuthorListChanged()
        {
            this.AuthorListOpen = this.AuthorListSectionValue;
            this.AuthorListNotOpen = !this.AuthorListSectionValue;
        }

        [RelayCommand]
        public void BookInfoChanged()
        {
            this.BookInfoOpen = this.BookInfoSectionValue;
            this.BookInfoNotOpen = !this.BookInfoSectionValue;
        }

        [RelayCommand]
        public void SummaryChanged()
        {
            this.SummaryOpen = this.SummarySectionValue;
            this.SummaryNotOpen = !this.SummarySectionValue;
        }

        [RelayCommand]
        public void CommentsChanged()
        {
            this.CommentsOpen = this.CommentsSectionValue;
            this.CommentsNotOpen = !this.CommentsSectionValue;
        }

        [RelayCommand]
        public void BookCoverPopup()
        {
            this.View.ShowPopup(new BookCoverPopup(this.BookCover));
        }
    }
}
