// <copyright file="FillLists.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.Models;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Library;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// FillLists class.
    /// </summary>
    public partial class FillLists : ObservableObject
    {
        /// <summary>
        /// Get a list of all books that are currently being read. A book is considered
        /// to be currently being read if the number of pages read is not equal to the total
        /// number of pages and the number of pages read is not equal to 0, or if the book
        /// is marked as up next, or if the number of hours listened is not equal to the
        /// total number of hours and the number of minutes listened is not equal to the
        /// total number of minutes and the number of hours listened is not equal to 0 and
        /// the number of minutes listened is not equal to 0.
        /// </summary>
        /// <returns>A list of all books that are currently being read.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetReadingBooksList()
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (AllBooksViewModel.hiddenFilteredBookList != null)
            {
                filteredList = AllBooksViewModel.hiddenFilteredBookList
                    .Where(x => (x.BookPageRead != x.BookPageTotal && x.BookPageRead != 0) ||
                    x.UpNext ||
                    (x.BookHourListened != x.BookHoursTotal && x.BookMinuteListened != x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0))
                    .ToObservableCollection();
            }
            else
            {
                var list = await BaseViewModel.Database.GetAllReadingBooksAsync();
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        /// <summary>
        /// Get a list of all books that are yet to be read. A book is considered to be yet
        /// to be read if the number of pages read is equal to 0 and the number of hours
        /// listened is equal to 0 and the number of minutes listened is equal to 0 and the
        /// book is not marked as up next.
        /// </summary>
        /// <returns>A list of all books that are yet to be read.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetToBeReadBooksList()
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (AllBooksViewModel.hiddenFilteredBookList != null)
            {
                filteredList = AllBooksViewModel.hiddenFilteredBookList
                    .Where(x => (x.BookPageRead == 0 &&
                    (x.BookHourListened == 0 && x.BookMinuteListened == 0))
                    && !x.UpNext)
                    .ToObservableCollection();
            }
            else
            {
                var list = await BaseViewModel.Database.GetAllToBeReadBooksAsync();
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        /// <summary>
        /// Get a list of all books that have been read. A book is considered to be read if
        /// the number of pages read is equal to the total number of pages and the number of
        /// pages read is not equal to 0, or if the number of hours listened is equal to the
        /// total number of hours and the number of minutes listened is equal to the total
        /// number of minutes and the number of hours listened is not equal to 0 and the number
        /// of minutes listened is not equal to 0.
        /// </summary>
        /// <returns>A list of all books that have have been read.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetReadBooksList()
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (AllBooksViewModel.hiddenFilteredBookList != null)
            {
                filteredList = AllBooksViewModel.hiddenFilteredBookList
                    .Where(x => (x.BookPageRead == x.BookPageTotal && x.BookPageRead != 0) ||
                    (x.BookHourListened == x.BookHoursTotal && x.BookMinuteListened == x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0))
                    .ToObservableCollection();
            }
            else
            {
                var list = await BaseViewModel.Database.GetAllReadBooksAsync();
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        /// <summary>
        /// Get a list of all books in the database, regardless of their reading status.
        /// </summary>
        /// <returns>A list of all books.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksList()
        {
            ObservableCollection<BookModel>? filteredList = null;

            var list = await BaseViewModel.Database.GetAllBooksAsync();
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        /// <summary>
        /// Get all wishlist books.
        /// </summary>
        /// <returns>A list of wishlist books.</returns>
        public static async Task<ObservableCollection<WishlistBookModel>?> GetBookWishList()
        {
            ObservableCollection<WishlistBookModel>? filteredList = null;

            var list = await BaseViewModel.Database.GetAllWishlistBooksAsync();
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        /// <summary>
        /// Get all chapters in a book based on the provided book guid.
        /// </summary>
        /// <param name="inputGuid">Book guid to get chapters for.</param>
        /// <returns>A list of chapters in the given book.</returns>
        public static async Task<ObservableCollection<ChapterModel>?> GetAllChaptersInBook(Guid? inputGuid)
        {
            ObservableCollection<ChapterModel>? chapterList = null;

            if (inputGuid != null)
            {
                var list = await BaseViewModel.Database.GetChaptersInBookAsync((Guid)inputGuid);
                chapterList = list.ToObservableCollection();
            }

            return chapterList;
        }

        /// <summary>
        /// Get all chapters in the database, regardless of the book they belong to.
        /// </summary>
        /// <returns>A list of all chapters added for every book.</returns>
        public static async Task<ObservableCollection<ChapterModel>?> GetAllChapters()
        {
            ObservableCollection<ChapterModel>? chapterList = null;

            var list = await BaseViewModel.Database.GetAllChaptersAsync();
            chapterList = list.ToObservableCollection();

            return chapterList;
        }

        /// <summary>
        /// Get a list of publishers in the provided book list. The list of publishers is distinct and ordered alphabetically.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of publishers, ordered alphabetically.</returns>
        public static async Task<ObservableCollection<string>> GetAllPublishersInBookList(ObservableCollection<BookModel> bookList)
        {
            var publisherList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookPublisher) && !publisherList.Any(x => x.Equals(book.BookPublisher)))
                {
                    publisherList.Add(book.BookPublisher);
                }
            }

            publisherList = publisherList.Distinct().ToObservableCollection();

            publisherList = publisherList.OrderBy(x => x).ToObservableCollection();

            return publisherList;
        }

        /// <summary>
        /// Get a list of publishers in the provided wishlist book list. The list of publishers is distinct and ordered alphabetically.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of publishers, ordered alphabetically.</returns>
        public static async Task<ObservableCollection<string>> GetAllPublishersInBookList(ObservableCollection<WishlistBookModel> bookList)
        {
            var publisherList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookPublisher) && !publisherList.Any(x => x.Equals(book.BookPublisher)))
                {
                    publisherList.Add(book.BookPublisher);
                }
            }

            publisherList = publisherList.Distinct().ToObservableCollection();

            publisherList = publisherList.OrderBy(x => x).ToObservableCollection();

            return publisherList;
        }

        /// <summary>
        /// Get a list of authors in the provided book list. The list of authors is distinct and ordered alphabetically.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of authors, ordered alphabetically.</returns>
        public static async Task<ObservableCollection<string>> GetAllAuthorsInBookList(ObservableCollection<BookModel> bookList)
        {
            var authorListNames = new ObservableCollection<string>();
            var authorList = new ObservableCollection<AuthorModel>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var list = await StringManipulation.SplitAuthorListStringIntoAuthorList(book.AuthorListString);

                    foreach (var author in list)
                    {
                        authorList.Add(author);
                    }
                }
            }

            authorList = authorList.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).ToObservableCollection();

            foreach (var author in authorList)
            {
                if (!authorListNames.Any(x => x.Equals(author.ReverseFullName)))
                {
                    authorListNames.Add(author.ReverseFullName);
                }
            }

            authorListNames = authorListNames.Distinct().ToObservableCollection();

            return authorListNames;
        }

        /// <summary>
        /// Get a list of authors in the provided wishlist book list. The list of authors is distinct and ordered alphabetically.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of authors, ordered alphabetically.</returns>
        public static async Task<ObservableCollection<string>> GetAllAuthorsInBookList(ObservableCollection<WishlistBookModel> bookList)
        {
            var authorListNames = new ObservableCollection<string>();
            var authorList = new ObservableCollection<AuthorModel>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var list = await StringManipulation.SplitAuthorListStringIntoAuthorList(book.AuthorListString);

                    foreach (var author in list)
                    {
                        authorList.Add(author);
                    }
                }
            }

            authorList = authorList.OrderBy(x => x.FirstName).OrderBy(x => x.LastName).ToObservableCollection();

            foreach (var author in authorList)
            {
                if (!authorListNames.Any(x => x.Equals(author.ReverseFullName)))
                {
                    authorListNames.Add(author.ReverseFullName);
                }
            }

            authorListNames = authorListNames.Distinct().ToObservableCollection();

            return authorListNames;
        }

        /// <summary>
        /// Get a list of locations in the provided wishlist book list. The list of locations is distinct and ordered alphabetically.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of locations, ordered alphabetically.</returns>
        public static async Task<ObservableCollection<string>> GetAllLocationsInBookList(ObservableCollection<WishlistBookModel> bookList)
        {
            var locationList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookWhereToBuy) && !locationList.Any(x => x.Equals(book.BookWhereToBuy)))
                {
                    locationList.Add(char.ToUpper(book.BookWhereToBuy[0]) + book.BookWhereToBuy[1..].ToLower());
                }
            }

            locationList = locationList.Distinct().ToObservableCollection();

            locationList = locationList.OrderBy(x => x).ToObservableCollection();

            return locationList;
        }

        /// <summary>
        /// Get a list of series in the provided wishlist book list. The list of series is distinct and ordered alphabetically.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of series, ordered alphabetically.</returns>
        public static async Task<ObservableCollection<string>> GetAllSeriesInBookList(ObservableCollection<WishlistBookModel> bookList)
        {
            var seriesList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookSeries) && !seriesList.Any(x => x.Equals(book.BookSeries)))
                {
                    seriesList.Add(book.BookSeries);
                }
            }

            seriesList = seriesList.Distinct().ToObservableCollection();

            seriesList = seriesList.OrderBy(x => x).ToObservableCollection();

            return seriesList;
        }

        /// <summary>
        /// Get a list of publish year ranges in the provided book list.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of publish year ranges, ordered numerically.</returns>
        public static async Task<ObservableCollection<string>> GetAllPublisherYearsInBookList(ObservableCollection<BookModel> bookList)
        {
            var publishYearList = new ObservableCollection<string>();

            foreach (var book in bookList.OrderBy(x => x.BookPublishYear))
            {
                if (!string.IsNullOrEmpty(book.BookPublishYear))
                {
                    var publishYearSubstring = book.BookPublishYear[..3];
                    var publishRange = $"{publishYearSubstring}0 - {publishYearSubstring}9";

                    if (!publishYearList.Any(x => x.Equals(publishRange)))
                    {
                        publishYearList.Add(publishRange);
                    }
                }
            }

            return publishYearList;
        }

        /// <summary>
        /// Get a list of publish year ranges in the provided wishlist book list.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of publish year ranges, ordered numerically.</returns>
        public static async Task<ObservableCollection<string>> GetAllPublisherYearsInBookList(ObservableCollection<WishlistBookModel> bookList)
        {
            var publishYearList = new ObservableCollection<string>();

            foreach (var book in bookList.OrderBy(x => x.BookPublishYear))
            {
                if (!string.IsNullOrEmpty(book.BookPublishYear))
                {
                    var publishYearSubstring = book.BookPublishYear[..3];
                    var publishRange = $"{publishYearSubstring}0 - {publishYearSubstring}9";

                    if (!publishYearList.Any(x => x.Equals(publishRange)))
                    {
                        publishYearList.Add(publishRange);
                    }
                }
            }

            publishYearList = publishYearList.Distinct().ToObservableCollection();

            return publishYearList;
        }

        /// <summary>
        /// Get a list of languages in the provided book list. The list of languages is distinct and ordered alphabetically.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of languages, ordered alphabetically.</returns>
        public static async Task<ObservableCollection<string>> GetAllLanguagesInBookList(ObservableCollection<BookModel> bookList)
        {
            var languageList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookLanguage) && !languageList.Any(x => x.Equals(book.BookLanguage)))
                {
                    languageList.Add(char.ToUpper(book.BookLanguage[0]) + book.BookLanguage[1..].ToLower());
                }
            }

            languageList = languageList.Distinct().ToObservableCollection();

            languageList = languageList.OrderBy(x => x).ToObservableCollection();

            return languageList;
        }

        /// <summary>
        /// Get a list of languages in the provided wishlist book list. The list of languages is distinct and ordered alphabetically.
        /// </summary>
        /// <param name="bookList">Book list to search.</param>
        /// <returns>A list of languages, ordered alphabetically.</returns>
        public static async Task<ObservableCollection<string>> GetAllLanguagesInBookList(ObservableCollection<WishlistBookModel> bookList)
        {
            var languageList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookLanguage) && !languageList.Any(x => x.Equals(book.BookLanguage)))
                {
                    languageList.Add(book.BookLanguage);
                }
            }

            languageList = languageList.Distinct().ToObservableCollection();

            return languageList;
        }

        /// <summary>
        /// Get all book authors for a book based on the provided book guid.
        /// </summary>
        /// <param name="inputGuid">The guid of the book to retrieve authors for.</param>
        /// <returns>A list of book authors for the book.</returns>
        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthorsForBook(Guid? inputGuid)
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            if (inputGuid != null)
            {
                var list = await BaseViewModel.Database.GetAllBookAuthorsForBookAsync(inputGuid);
                bookAuthorList = list.ToObservableCollection();
            }

            return bookAuthorList;
        }

        /// <summary>
        /// Get all author guids for a book based on the provided book guid.
        /// </summary>
        /// <param name="inputGuid">The guid of the book to retrieve authors for.</param>
        /// <returns>A list of author guids for the book.</returns>
        public static async Task<ObservableCollection<Guid>?> GetAllAuthorGuidsForBook(Guid? inputGuid)
        {
            ObservableCollection<Guid>? authorGuidList = null;

            if (inputGuid != null)
            {
                var list = await BaseViewModel.Database.GetAllAuthorGuidsForBookAsync((Guid)inputGuid);
                authorGuidList = list.ToObservableCollection();
            }

            return authorGuidList;
        }

        /// <summary>
        /// Get all book authors for a book based on the provided book guid.
        /// </summary>
        /// <param name="inputGuid">The guid of the book to retrieve authors for.</param>
        /// <returns>A list of book authors for the book.</returns>
        public static async Task<ObservableCollection<AuthorModel>?> GetAllAuthorsForBook(Guid? inputGuid)
        {
            var authorList = new ObservableCollection<AuthorModel>();

            if (inputGuid != null)
            {
                var bookAuthorList = await BaseViewModel.Database.GetAllBookAuthorsForBookAsync(inputGuid);

                foreach (var bookAuthor in bookAuthorList)
                {
                    var author = await BaseViewModel.Database.GetAuthorByGuidAsync(bookAuthor.AuthorGuid);

                    if (author != null)
                    {
                        authorList.Add(BaseViewModel.ConvertTo<AuthorModel>(author));
                    }
                }
            }

            return authorList;
        }

        /// <summary>
        /// Get all book authors in the database, regardless of the book they belong to.
        /// </summary>
        /// <returns>A list of all book authors added for every book.</returns>
        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthors()
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            var list = await BaseViewModel.Database.GetAllBookAuthorsAsync();
            bookAuthorList = list.ToObservableCollection();

            return bookAuthorList;
        }

        /// <summary>
        /// Get all book authors for an author based on the provided author guid.
        /// </summary>
        /// <param name="inputGuid">Author guid to get book authors for.</param>
        /// <returns>A list of book authors.</returns>
        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthorsForAuthor(Guid? inputGuid)
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            if (inputGuid != null)
            {
                var list = await BaseViewModel.Database.GetAllBookAuthorsForAuthorAsync((Guid)inputGuid);
                bookAuthorList = list.ToObservableCollection();
            }

            return bookAuthorList;
        }

        /// <summary>
        /// Get all books in a collection based on the provided collection guid. If showHiddenBooks is false, then hidden books will be filtered out.
        /// </summary>
        /// <param name="inputGuid">Collection guid to get books for.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books assigned to the collection.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInCollectionList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (inputGuid != null)
            {
                if (AllBooksViewModel.fullBookList != null)
                {
                    filteredList = AllBooksViewModel.fullBookList
                        .Where(x => x.BookCollectionGuid == inputGuid)
                        .ToObservableCollection();

                    filteredList = BaseViewModel.SetHiddenFilteredList<BookModel>(filteredList!, showHiddenBooks).ToObservableCollection();
                }
                else
                {
                    var list = await BaseViewModel.Database.GetAllBooksInCollectionAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Get all books without a collection assigned.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books without a collection assigned.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutACollectionList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            var list = await BaseViewModel.Database.GetAllBooksWithoutACollectionAsync(showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        /// <summary>
        /// Get all books in a genre based on the provided genre guid. If showHiddenBooks is false, then hidden books will be filtered out.
        /// </summary>
        /// <param name="inputGuid">Genre guid to get books for.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books assigned to the genre.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInGenreList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (inputGuid != null)
            {
                if (AllBooksViewModel.fullBookList != null)
                {
                    filteredList = AllBooksViewModel.fullBookList
                        .Where(x => x.BookGenreGuid == inputGuid)
                        .ToObservableCollection();

                    filteredList = BaseViewModel.SetHiddenFilteredList<BookModel>(filteredList!, showHiddenBooks).ToObservableCollection();
                }
                else
                {
                    var list = await BaseViewModel.Database.GetAllBooksInGenreAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Get all books without a genre assigned.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books without a genre assigned.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutAGenreList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            var list = await BaseViewModel.Database.GetAllBooksWithoutAGenreAsync(showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        /// <summary>
        /// Get all books in a series based on the provided series guid. If showHiddenBooks is false, then hidden books will be filtered out.
        /// </summary>
        /// <param name="inputGuid">Series guid to get books for.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books assigned to the series.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInSeriesList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (inputGuid != null)
            {
                if (AllBooksViewModel.fullBookList != null)
                {
                    filteredList = AllBooksViewModel.fullBookList
                        .Where(x => x.BookSeriesGuid == inputGuid)
                        .ToObservableCollection();

                    filteredList = BaseViewModel.SetHiddenFilteredList<BookModel>(filteredList!, showHiddenBooks).ToObservableCollection();
                }
                else
                {
                    var list = await BaseViewModel.Database.GetAllBooksInSeriesAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Get all books without a series assigned.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books without a series assigned.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutASeriesList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            var list = await BaseViewModel.Database.GetAllBooksWithoutASeriesAsync(showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        /// <summary>
        /// Get all books in a location based on the provided location guid. If showHiddenBooks is false, then hidden books will be filtered out.
        /// </summary>
        /// <param name="inputGuid">Location guid to get books for.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books assigned to the location.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInLocationList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (inputGuid != null)
            {
                if (AllBooksViewModel.fullBookList != null)
                {
                    filteredList = AllBooksViewModel.fullBookList
                        .Where(x => x.BookLocationGuid == inputGuid)
                        .ToObservableCollection();

                    filteredList = BaseViewModel.SetHiddenFilteredList<BookModel>(filteredList!, showHiddenBooks).ToObservableCollection();
                }
                else
                {
                    var list = await BaseViewModel.Database.GetAllBooksInLocationAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Get all books without a location assigned.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books without a location assigned.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutALocationList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            var list = await BaseViewModel.Database.GetAllBooksWithoutALocationAsync(showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        /// <summary>
        /// Get all collections.
        /// </summary>
        /// <returns>A list of collections.</returns>
        public static async Task<ObservableCollection<CollectionModel>?> GetAllCollectionsList()
        {
            ObservableCollection<CollectionModel>? filteredList = null;

            if (CollectionsViewModel.fullCollectionList == null)
            {
                var list = await BaseViewModel.Database.GetAllCollectionsAsync();
                filteredList = list.ToObservableCollection();
                CollectionsViewModel.fullCollectionList = filteredList;
            }
            else
            {
                filteredList = CollectionsViewModel.fullCollectionList;
            }

            return filteredList;
        }

        /// <summary>
        /// Get all genres.
        /// </summary>
        /// <returns>A list of genres.</returns>
        public static async Task<ObservableCollection<GenreModel>?> GetAllGenresList()
        {
            ObservableCollection<GenreModel>? filteredList = null;

            if (GenresViewModel.fullGenreList == null)
            {
                var list = await BaseViewModel.Database.GetAllGenresAsync();
                filteredList = list.ToObservableCollection();
                GenresViewModel.fullGenreList = filteredList;
            }
            else
            {
                filteredList = GenresViewModel.fullGenreList;
            }

            return filteredList;
        }

        /// <summary>
        /// Get all series.
        /// </summary>
        /// <returns>A list of series.</returns>
        public static async Task<ObservableCollection<SeriesModel>?> GetAllSeriesList()
        {
            ObservableCollection<SeriesModel>? filteredList = null;

            if (SeriesViewModel.fullSeriesList == null)
            {
                var list = await BaseViewModel.Database.GetAllSeriesAsync();
                filteredList = list.ToObservableCollection();
                SeriesViewModel.fullSeriesList = filteredList;
            }
            else
            {
                filteredList = SeriesViewModel.fullSeriesList;
            }

            return filteredList;
        }

        /// <summary>
        /// Get all locations.
        /// </summary>
        /// <returns>A list of locations.</returns>
        public static async Task<ObservableCollection<LocationModel>?> GetAllLocationsList()
        {
            ObservableCollection<LocationModel>? filteredList = null;

            if (LocationsViewModel.fullLocationList == null)
            {
                var list = await BaseViewModel.Database.GetAllLocationsAsync();
                filteredList = list.ToObservableCollection();
                LocationsViewModel.fullLocationList = filteredList;
            }
            else
            {
                filteredList = LocationsViewModel.fullLocationList;
            }

            return filteredList;
        }

        /// <summary>
        /// Get all the books for an author based on the provided author guid. If showHiddenBooks is false, then hidden books will be filtered out.
        /// </summary>
        /// <param name="inputGuid">Author guid to get books for.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books for the provided author.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInAuthorList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = [];

            if (inputGuid != null)
            {
                var list = await BaseViewModel.Database.GetAllBooksForAuthorAsync((Guid)inputGuid, showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        /// <summary>
        /// Get books that do not have the provided author as an author. If showHiddenBooks is false, then hidden books will be filtered out.
        /// </summary>
        /// <param name="reverseAuthorName">Author name to search for.</param>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A list of books that aren't assigned to the provided author.</returns>
        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutAuthorList(string reverseAuthorName, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            var list = await BaseViewModel.Database.GetAllBooksWithoutAuthorAsync(reverseAuthorName, showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        /// <summary>
        /// Get all authors.
        /// </summary>
        /// <returns>A list of authors.</returns>
        public static async Task<ObservableCollection<AuthorModel>?> GetAllAuthorsList()
        {
            ObservableCollection<AuthorModel>? filteredList = null;

            if (AuthorsViewModel.fullAuthorList == null)
            {
                var list = await BaseViewModel.Database.GetAllAuthorsAsync();
                filteredList = list.ToObservableCollection();
                AuthorsViewModel.fullAuthorList = filteredList;
            }
            else
            {
                filteredList = AuthorsViewModel.fullAuthorList;
            }

            return filteredList;
        }
    }
}
