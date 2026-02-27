// <copyright file="BookBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.Author;
using BookCollector.ViewModels.Collection;
using BookCollector.ViewModels.Genre;
using BookCollector.ViewModels.Groupings;
using BookCollector.ViewModels.Library;
using BookCollector.ViewModels.Location;
using BookCollector.ViewModels.Series;
using BookCollector.Views.Book;
using BookCollector.Views.Popups;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.BaseViewModels
{
    public partial class BookBaseViewModel : BaseViewModel
    {
        [ObservableProperty]
        public static ObservableCollection<string>? bookFormats;

        [ObservableProperty]
        public string? totalBooksString;

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

        [ObservableProperty]
        public bool showPages;

        [ObservableProperty]
        public bool showTime;

        public BookBaseViewModel()
        {
            this.ShowComments = Preferences.Get("CommentsOn", true /* Default */);
            this.ShowChapters = Preferences.Get("ChaptersOn", true /* Default */);
            this.ShowFavorites = Preferences.Get("FavoritesOn", true /* Default */);
            this.ShowRatings = Preferences.Get("RatingsOn", true /* Default */);
        }

        public static bool ShowHiddenBook { get; set; }

        public bool HiddenAuthorsOn { get; set; }

        public bool ShowFavoriteBooks { get; set; }

        public bool ShowBookRatings { get; set; }

        public string? FavoriteBooksOption { get; set; }

        public string? BookFormatOption { get; set; }

        public string? BookPublisherOption { get; set; }

        public string? BookPublishYearOption { get; set; }

        public string? BookLanguageOption { get; set; }

        public string? BookRatingOption { get; set; }

        public string? BookCoverOption { get; set; }

        public bool BookTitleChecked { get; set; }

        public bool BookReadingDateChecked { get; set; }

        public bool BookReadPercentageChecked { get; set; }

        public bool BookPublisherChecked { get; set; }

        public bool BookPublishYearChecked { get; set; }

        public bool AuthorLastNameChecked { get; set; }

        public bool BookFormatChecked { get; set; }

        public bool PageCountBookTimeChecked { get; set; }

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

                        author = await Database.GetAuthorByNameAsync(item.FirstName, item.LastName);

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
        public async Task BookSelectionChanged()
        {
            if (this.SelectedBook != null && !string.IsNullOrEmpty(this.SelectedBook.BookTitle))
            {
                var view = new BookMainView(this.SelectedBook, this.SelectedBook.BookTitle, this);
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
        public async Task BookCoverPopup()
        {
            this.View.ShowPopup(new BookCoverPopup(this.BookCover));
        }

        public static async Task AddToStaticList(BookModel book, object? previousViewModel = null)
        {
            if (AllBooksViewModel.fullBookList != null)
            {
                AllBooksViewModel.RefreshView = await AddBookToStaticList(book, AllBooksViewModel.fullBookList, AllBooksViewModel.filteredBookList2);
            }

            if ((((book.BookPageRead != book.BookPageTotal && book.BookPageRead != 0) ||
                book.UpNext) ||
                (book.BookHourListened != book.BookHoursTotal && book.BookMinuteListened != book.BookMinutesTotal && book.BookHourListened != 0 && book.BookMinuteListened != 0)) &&
                ReadingViewModel.fullBookList != null)
            {
                ReadingViewModel.RefreshView = await AddBookToStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList2);
                ToBeReadViewModel.RefreshView = RemoveBookFromStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList2);
                ReadViewModel.RefreshView = RemoveBookFromStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList2);
            }

            if (book.BookPageRead == 0 &&
                !book.UpNext &&
                (book.BookHourListened == 0 && book.BookMinuteListened == 0) &&
                ToBeReadViewModel.fullBookList != null)
            {
                ToBeReadViewModel.RefreshView = await AddBookToStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList2);
                ReadingViewModel.RefreshView = RemoveBookFromStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList2);
                ReadViewModel.RefreshView = RemoveBookFromStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList2);
            }

            if ((book.BookPageRead == book.BookPageTotal && book.BookPageRead != 0) ||
                ((book.BookHourListened == book.BookHoursTotal && book.BookMinuteListened == book.BookMinutesTotal && book.BookHourListened != 0 && book.BookMinuteListened != 0) &&
                ReadViewModel.fullBookList != null))
            {
                ReadViewModel.RefreshView = await AddBookToStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList2);
                ToBeReadViewModel.RefreshView = RemoveBookFromStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList2);
                ReadingViewModel.RefreshView = RemoveBookFromStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList2);
            }

            if (CollectionsViewModel.fullCollectionList != null)
            {
                if (book.BookCollectionGuid != null)
                {
                    var selected = CollectionsViewModel.fullCollectionList.FirstOrDefault(x => x.CollectionGuid == book.BookCollectionGuid);

                    if (selected != null)
                    {
                        CollectionMainViewModel collectionViewModel;

                        if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Collection"))
                        {
                            collectionViewModel = (CollectionMainViewModel)previousViewModel;

                            if (collectionViewModel.SelectedCollection != selected)
                            {
                                collectionViewModel.RefreshView = RemoveBookFromStaticList(book, collectionViewModel.FullBookList, collectionViewModel.FilteredBookList2);
                                collectionViewModel = new CollectionMainViewModel(selected, new ContentPage());
                            }
                        }
                        else
                        {
                            collectionViewModel = new CollectionMainViewModel(selected, new ContentPage());
                        }

                        if (collectionViewModel.FullBookList != null)
                        {
                            collectionViewModel.RefreshView = await AddBookToStaticList(book, collectionViewModel.FullBookList, collectionViewModel.FilteredBookList2);
                        }
                    }
                }

                if (book.BookCollectionGuid == null && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Collection"))
                {
                    var collectionViewModel = (CollectionMainViewModel)previousViewModel;
                    collectionViewModel.RefreshView = RemoveBookFromStaticList(book, collectionViewModel.FullBookList, collectionViewModel.FilteredBookList2);

                    var existingBooksViewModel = new ExistingBooksViewModel(new CollectionModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        existingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList2);
                    }
                }
            }

            if (GenresViewModel.fullGenreList != null)
            {
                if (book.BookGenreGuid != null)
                {
                    var selected = GenresViewModel.fullGenreList.FirstOrDefault(x => x.GenreGuid == book.BookGenreGuid);

                    if (selected != null)
                    {
                        GenreMainViewModel genreViewModel;

                        if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Genre"))
                        {
                            genreViewModel = (GenreMainViewModel)previousViewModel;

                            if (genreViewModel.SelectedGenre != selected)
                            {
                                genreViewModel.RefreshView = RemoveBookFromStaticList(book, genreViewModel.FullBookList, genreViewModel.FilteredBookList2);
                                genreViewModel = new GenreMainViewModel(selected, new ContentPage());
                            }
                        }
                        else
                        {
                            genreViewModel = new GenreMainViewModel(selected, new ContentPage());
                        }

                        if (genreViewModel.FullBookList != null)
                        {
                            genreViewModel.RefreshView = await AddBookToStaticList(book, genreViewModel.FullBookList, genreViewModel.FilteredBookList2);
                        }
                    }
                }

                if (book.BookGenreGuid == null && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Genre"))
                {
                    var genreViewModel = (GenreMainViewModel)previousViewModel;
                    genreViewModel.RefreshView = RemoveBookFromStaticList(book, genreViewModel.FullBookList, genreViewModel.FilteredBookList2);

                    var existingBooksViewModel = new ExistingBooksViewModel(new GenreModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        existingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList2);
                    }
                }
            }

            if (SeriesViewModel.fullSeriesList != null)
            {
                if (book.BookSeriesGuid != null)
                {
                    var selected = SeriesViewModel.fullSeriesList.FirstOrDefault(x => x.SeriesGuid == book.BookSeriesGuid);

                    if (selected != null)
                    {
                        SeriesMainViewModel seriesViewModel;

                        if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Series"))
                        {
                            seriesViewModel = (SeriesMainViewModel)previousViewModel;

                            if (seriesViewModel.SelectedSeries != selected)
                            {
                                seriesViewModel.RefreshView = RemoveBookFromStaticList(book, seriesViewModel.FullBookList, seriesViewModel.FilteredBookList2);
                                seriesViewModel = new SeriesMainViewModel(selected, new ContentPage());
                            }
                        }
                        else
                        {
                            seriesViewModel = new SeriesMainViewModel(selected, new ContentPage());
                        }

                        if (seriesViewModel.FullBookList != null)
                        {
                            seriesViewModel.RefreshView = await AddBookToStaticList(book, seriesViewModel.FullBookList, seriesViewModel.FilteredBookList2);
                        }
                    }
                }

                if (book.BookSeriesGuid == null && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Series"))
                {
                    var seriesViewModel = (SeriesMainViewModel)previousViewModel;
                    seriesViewModel.RefreshView = RemoveBookFromStaticList(book, seriesViewModel.FullBookList, seriesViewModel.FilteredBookList2);

                    var existingBooksViewModel = new ExistingBooksViewModel(new SeriesModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        existingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList2);
                    }
                }
            }

            if (AuthorsViewModel.fullAuthorList != null)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var authors = SplitStringIntoAuthorList(book.AuthorListString);

                    foreach (var author in authors)
                    {
                        var selected = AuthorsViewModel.fullAuthorList.FirstOrDefault(x => x.FirstName.Equals(author.FirstName) && x.LastName.Equals(author.LastName));

                        if (selected != null)
                        {
                            AuthorMainViewModel authorViewModel;

                            if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Author"))
                            {
                                authorViewModel = (AuthorMainViewModel)previousViewModel;

                                if (authorViewModel.SelectedAuthor != selected)
                                {
                                    authorViewModel.RefreshView = RemoveBookFromStaticList(book, authorViewModel.FullBookList, authorViewModel.FilteredBookList2);
                                    authorViewModel = new AuthorMainViewModel(selected, new ContentPage());
                                }
                            }
                            else
                            {
                                authorViewModel = new AuthorMainViewModel(selected, new ContentPage());
                            }

                            if (authorViewModel.FullBookList != null)
                            {
                                authorViewModel.RefreshView = await AddBookToStaticList(book, authorViewModel.FullBookList, authorViewModel.FilteredBookList2);
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(book.AuthorListString) && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Author"))
                {
                    var authorViewModel = (AuthorMainViewModel)previousViewModel;
                    authorViewModel.RefreshView = RemoveBookFromStaticList(book, authorViewModel.FullBookList, authorViewModel.FilteredBookList2);

                    var existingBooksViewModel = new ExistingBooksViewModel(new AuthorModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        existingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList2);
                    }
                }
            }

            if (LocationsViewModel.fullLocationList != null)
            {
                if (book.BookLocationGuid != null)
                {
                    var selected = LocationsViewModel.fullLocationList.FirstOrDefault(x => x.LocationGuid == book.BookLocationGuid);

                    if (selected != null)
                    {
                        LocationMainViewModel locationViewModel;

                        if (previousViewModel != null && previousViewModel.GetType().ToString().Contains("Location"))
                        {
                            locationViewModel = (LocationMainViewModel)previousViewModel;

                            if (locationViewModel.SelectedLocation != selected)
                            {
                                locationViewModel.RefreshView = RemoveBookFromStaticList(book, locationViewModel.FullBookList, locationViewModel.FilteredBookList2);
                                locationViewModel = new LocationMainViewModel(selected, new ContentPage());
                            }
                        }
                        else
                        {
                            locationViewModel = new LocationMainViewModel(selected, new ContentPage());
                        }

                        if (locationViewModel.FullBookList != null)
                        {
                            locationViewModel.RefreshView = await AddBookToStaticList(book, locationViewModel.FullBookList, locationViewModel.FilteredBookList2);
                        }
                    }
                }

                if (book.BookLocationGuid == null && previousViewModel != null && previousViewModel.GetType().ToString().Contains("Location"))
                {
                    var locationViewModel = (LocationMainViewModel)previousViewModel;
                    locationViewModel.RefreshView = RemoveBookFromStaticList(book, locationViewModel.FullBookList, locationViewModel.FilteredBookList2);

                    var existingBooksViewModel = new ExistingBooksViewModel(new LocationModel(), new ContentPage(), previousViewModel);

                    if (existingBooksViewModel.FullBookList != null)
                    {
                        existingBooksViewModel.RefreshView = await AddBookToStaticList(book, existingBooksViewModel.FullBookList, existingBooksViewModel.FilteredBookList2);
                    }
                }
            }

            CollectionsViewModel.RefreshView = true;
            GenresViewModel.RefreshView = true;
            SeriesViewModel.RefreshView = true;
            AuthorsViewModel.RefreshView = true;
            LocationsViewModel.RefreshView = true;
        }

        private static async Task<bool> AddBookToStaticList(BookModel book, ObservableCollection<BookModel> bookList, ObservableCollection<BookModel>? filteredBookList)
        {
            var refresh = false;

            await Task.WhenAll(new Task[]
            {
                book.SetReadingProgress(),
                //book.SetAuthorListString(),
                book.SetCoverDisplay(),
            });

            try
            {
                var oldBook = bookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                if (oldBook != null)
                {
                    var index = bookList.IndexOf(oldBook);
                    bookList.Remove(oldBook);
                    bookList.Insert(index, book);
                    refresh = true;
                }
                else
                {
                    bookList.Add(book);
                    refresh = true;
                }

                if (filteredBookList != null)
                {
                    var filteredBook = filteredBookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                    if (filteredBook != null)
                    {
                        var index = filteredBookList.IndexOf(filteredBook);
                        filteredBookList.Remove(filteredBook);
                        filteredBookList.Insert(index, book);
                        refresh = true;
                    }
                    else
                    {
                        filteredBookList.Add(book);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }

        public async Task RemoveFromStaticList(BookModel book)
        {
            if (AllBooksViewModel.fullBookList != null)
            {
                AllBooksViewModel.RefreshView = RemoveBookFromStaticList(book, AllBooksViewModel.fullBookList, AllBooksViewModel.filteredBookList2);
            }

            if (((book.BookPageRead != book.BookPageTotal && book.BookPageRead != 0) ||
                book.UpNext) ||
                ((book.BookHourListened != book.BookHoursTotal && book.BookMinuteListened != book.BookMinutesTotal) &&
                ReadingViewModel.fullBookList != null))
            {
                ReadingViewModel.RefreshView = RemoveBookFromStaticList(book, ReadingViewModel.fullBookList, ReadingViewModel.filteredBookList2);
            }

            if (book.BookPageRead == 0 &&
                !book.UpNext &&
                (book.BookHourListened == 0 && book.BookMinuteListened == 0) &&
                ToBeReadViewModel.fullBookList != null)
            {
                ToBeReadViewModel.RefreshView = RemoveBookFromStaticList(book, ToBeReadViewModel.fullBookList, ToBeReadViewModel.filteredBookList2);
            }

            if ((book.BookPageRead == book.BookPageTotal && book.BookPageRead != 0) ||
                ((book.BookHourListened == book.BookHoursTotal && book.BookMinuteListened == book.BookMinutesTotal && book.BookHourListened != 0 && book.BookMinuteListened != 0) &&
                ReadViewModel.fullBookList != null))
            {
                ReadViewModel.RefreshView = RemoveBookFromStaticList(book, ReadViewModel.fullBookList, ReadViewModel.filteredBookList2);
            }

            if (CollectionsViewModel.fullCollectionList != null)
            {
                if (book.BookCollectionGuid != null)
                {
                    var selected = CollectionsViewModel.fullCollectionList.FirstOrDefault(x => x.CollectionGuid == book.BookCollectionGuid);

                    if (selected != null)
                    {
                        await Task.WhenAll(new Task[]
                        {
                            selected.SetTotalBooks(true),
                            selected.SetTotalCostOfBooks(true),
                        });
                    }
                }
            }

            if (GenresViewModel.fullGenreList != null)
            {
                if (book.BookGenreGuid != null)
                {
                    var selected = GenresViewModel.fullGenreList.FirstOrDefault(x => x.GenreGuid == book.BookGenreGuid);

                    if (selected != null)
                    {
                        await Task.WhenAll(new Task[]
                        {
                            selected.SetTotalBooks(true),
                            selected.SetTotalCostOfBooks(true),
                        });
                    }
                }
            }

            if (SeriesViewModel.fullSeriesList != null)
            {
                if (book.BookSeriesGuid != null)
                {
                    var selected = SeriesViewModel.fullSeriesList.FirstOrDefault(x => x.SeriesGuid == book.BookSeriesGuid);

                    if (selected != null)
                    {
                        await Task.WhenAll(new Task[]
                        {
                            selected.SetTotalBooks(true),
                            selected.SetTotalCostOfBooks(true),
                        });
                    }
                }
            }

            if (AuthorsViewModel.fullAuthorList != null)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var authors = SplitStringIntoAuthorList(book.AuthorListString);

                    foreach (var author in authors)
                    {
                        var selected = AuthorsViewModel.fullAuthorList.FirstOrDefault(x => x.FirstName.Equals(author.FirstName) && x.LastName.Equals(author.LastName));

                        if (selected != null)
                        {
                            await Task.WhenAll(new Task[]
                            {
                                selected.SetTotalBooks(true),
                                selected.SetTotalCostOfBooks(true),
                            });
                        }
                    }
                }
            }

            if (LocationsViewModel.fullLocationList != null)
            {
                if (book.BookLocationGuid != null)
                {
                    var selected = LocationsViewModel.fullLocationList.FirstOrDefault(x => x.LocationGuid == book.BookLocationGuid);

                    if (selected != null)
                    {
                        await Task.WhenAll(new Task[]
                        {
                            selected.SetTotalBooks(true),
                            selected.SetTotalCostOfBooks(true),
                        });
                    }
                }
            }
        }

        public static bool RemoveBookFromStaticList(BookModel book, ObservableCollection<BookModel>? bookList, ObservableCollection<BookModel>? filteredBookList)
        {
            var refresh = false;

            try
            {
                if (bookList != null)
                {
                    var oldBook = bookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                    if (oldBook != null)
                    {
                        bookList.Remove(oldBook);
                        refresh = true;
                    }
                }

                if (filteredBookList != null)
                {
                    var filteredBook = filteredBookList.FirstOrDefault(x => x.BookGuid == book.BookGuid);

                    if (filteredBook != null)
                    {
                        filteredBookList.Remove(filteredBook);
                        refresh = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return refresh;
        }
    }
}
