// <copyright file="FillLists.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.ViewModels.Groupings;
using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

namespace BookCollector.Data
{
    public partial class FillLists : BaseViewModel
    {
        public static async Task<ObservableCollection<BookModel>?> GetReadingBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetReadingBooks(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllReadingBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetToBeReadBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetToBeReadBooks(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllToBeReadBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetReadBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetReadBooks(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllReadBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetReadingBooks(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<WishlistBookModel>?> GetBookWishList(bool showHiddenBooks)
        {
            ObservableCollection<WishlistBookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllWishlistBooks(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllWishlistBooksAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<ChapterModel>?> GetAllChaptersInBook(Guid? inputGuid)
        {
            ObservableCollection<ChapterModel>? chapterList = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    chapterList = TestData.GetAllChaptersInBook(inputGuid);
                }
                else
                {
                    var list = await Database.GetChaptersInBookAsync((Guid)inputGuid);
                    chapterList = list.ToObservableCollection();
                }
            }

            return chapterList;
        }

        public static async Task<ObservableCollection<ChapterModel>?> GetAllChapters()
        {
            ObservableCollection<ChapterModel>? chapterList = null;

            if (TestData.UseTestData)
            {
                chapterList = TestData.GetAllChapters();
            }
            else
            {
                var list = await Database.GetAllChaptersAsync();
                chapterList = list.ToObservableCollection();
            }

            return chapterList;
        }

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

            return publisherList;
        }

        public static async Task<ObservableCollection<string>> GetAllPublishersInWishlistBookList(ObservableCollection<WishlistBookModel> bookList)
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

            return publisherList;
        }

        public static async Task<ObservableCollection<string>> GetAllAuthorsInBookList(ObservableCollection<BookModel> bookList)
        {
            var authorList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var list = SplitStringIntoAuthorList(book.AuthorListString);

                    foreach (var author in list)
                    {
                        if (!authorList.Any(x => x.Equals(author)))
                        {
                            authorList.Add(author.FullName);
                        }
                    }
                }
            }

            return authorList;
        }

        public static async Task<ObservableCollection<string>> GetAllAuthorsInWishlistBookList(ObservableCollection<WishlistBookModel> bookList)
        {
            var authorList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var list = SplitStringIntoAuthorList(book.AuthorListString);

                    foreach (var author in list)
                    {
                        if (!authorList.Any(x => x.Equals(author)))
                        {
                            authorList.Add(author.FullName);
                        }
                    }
                }
            }

            authorList = authorList.Distinct().ToObservableCollection();

            return authorList;
        }

        public static async Task<ObservableCollection<string>> GetAllLocationsInBookList(ObservableCollection<BookModel> bookList)
        {
            var locationList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookWhereToBuy) && !locationList.Any(x => x.Equals(book.BookWhereToBuy)))
                {
                    locationList.Add(book.BookWhereToBuy);
                }
            }

            return locationList;
        }

        public static async Task<ObservableCollection<string>> GetAllLocationsInWishlistBookList(ObservableCollection<WishlistBookModel> bookList)
        {
            var locationList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookWhereToBuy) && !locationList.Any(x => x.Equals(book.BookWhereToBuy)))
                {
                    locationList.Add(book.BookWhereToBuy);
                }
            }

            locationList = locationList.Distinct().ToObservableCollection();

            return locationList;
        }

        public static async Task<ObservableCollection<string>> GetAllSeriesInBookList(ObservableCollection<BookModel> bookList)
        {
            var seriesList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookSeries) && !seriesList.Any(x => x.Equals(book.BookSeries)))
                {
                    seriesList.Add(book.BookSeries);
                }
            }

            return seriesList;
        }

        public static async Task<ObservableCollection<string>> GetAllSeriesInWishlistBookList(ObservableCollection<WishlistBookModel> bookList)
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

            return seriesList;
        }

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

        public static async Task<ObservableCollection<string>> GetAllPublisherYearsInWishlistBookList(ObservableCollection<WishlistBookModel> bookList)
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

        public static async Task<ObservableCollection<string>> GetAllLanguagesInBookList(ObservableCollection<BookModel> bookList)
        {
            var languageList = new ObservableCollection<string>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookLanguage) && !languageList.Any(x => x.Equals(book.BookLanguage)))
                {
                    languageList.Add(book.BookLanguage);
                }
            }

            return languageList;
        }

        public static async Task<ObservableCollection<string>> GetAllLanguagesInWishlistBookList(ObservableCollection<WishlistBookModel> bookList)
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

        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthorsForBook(Guid? inputGuid)
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    bookAuthorList = TestData.GetAllBookAuthorsForBook(inputGuid);
                }
                else
                {
                    var list = await Database.GetAllBookAuthorsForBookAsync(inputGuid);
                    bookAuthorList = list.ToObservableCollection();
                }
            }

            return bookAuthorList;
        }

        public static async Task<ObservableCollection<Guid>?> GetAllAuthorGuidsForBook(Guid? inputGuid)
        {
            ObservableCollection<Guid>? authorGuidList = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    authorGuidList = TestData.GetAllAuthorGuidsForBook(inputGuid);
                }
                else
                {
                    var list = await Database.GetAllAuthorGuidsForBookAsync((Guid)inputGuid);
                    authorGuidList = list.ToObservableCollection();
                }
            }

            return authorGuidList;
        }

        public static async Task<ObservableCollection<AuthorModel>?> GetAllAuthorsForBook(Guid? inputGuid, bool showHiddenAuthors)
        {
            var authorList = new ObservableCollection<AuthorModel>();

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    var bookAuthorList = TestData.GetAllBookAuthorsForBook(inputGuid);
                }
                else
                {
                    var bookAuthorList = await Database.GetAllBookAuthorsForBookAsync(inputGuid);

                    foreach (var bookAuthor in bookAuthorList)
                    {
                        var author = await Database.GetAuthorByGuidAsync(bookAuthor.AuthorGuid);

                        if (author != null)
                        {
                            authorList.Add(ConvertTo<AuthorModel>(author));
                        }
                    }
                }
            }

            return authorList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthors()
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            if (TestData.UseTestData)
            {
                bookAuthorList = TestData.GetAllBookAuthors();
            }
            else
            {
                var list = await Database.GetAllBookAuthorsAsync();
                bookAuthorList = list.ToObservableCollection();
            }

            return bookAuthorList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthorsForAuthor(Guid? inputGuid)
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    bookAuthorList = TestData.GetAllBookAuthorsForAuthor(inputGuid);
                }
                else
                {
                    var list = await Database.GetAllBookAuthorsForAuthorAsync((Guid)inputGuid);
                    bookAuthorList = list.ToObservableCollection();
                }
            }

            return bookAuthorList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInCollectionList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    filteredList = TestData.GetAllBooksInCollectionList(inputGuid, showHiddenBooks);
                }
                else
                {
                    var list = await Database.GetAllBooksInCollectionAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutACollectionList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
               filteredList = TestData.GetAllBooksWithoutACollectionList(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksWithoutACollectionAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInGenreList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    filteredList = TestData.GetAllBooksInGenreList(inputGuid, showHiddenBooks);
                }
                else
                {
                    var list = await Database.GetAllBooksInGenreAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutAGenreList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBooksWithoutAGenreList(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksWithoutAGenreAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInSeriesList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    filteredList = TestData.GetAllBooksInSeriesList(inputGuid, showHiddenBooks);
                }
                else
                {
                    var list = await Database.GetAllBooksInSeriesAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutASeriesList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBooksWithoutASeriesList(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksWithoutASeriesAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInLocationList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    filteredList = TestData.GetAllBooksInLocationList(inputGuid, showHiddenBooks);
                }
                else
                {
                    var list = await Database.GetAllBooksInLocationAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutALocationList(bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBooksWithoutALocationList(showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksWithoutALocationAsync(showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<CollectionModel>?> GetAllCollectionsList(bool showHiddenCollections)
        {
            ObservableCollection<CollectionModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllCollectionsList(showHiddenCollections);
            }
            else
            {
                if (CollectionsViewModel.fullCollectionList == null)
                {
                    var list = await Database.GetAllCollectionsAsync(showHiddenCollections);
                    filteredList = list.ToObservableCollection();
                }
                else
                {
                    filteredList = CollectionsViewModel.fullCollectionList;
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<GenreModel>?> GetAllGenresList(bool showHiddenGenres)
        {
            ObservableCollection<GenreModel>? filteredList = null;

            if (TestData.UseTestData)
            {
             filteredList = TestData.GetAllGenresList(showHiddenGenres);
            }
            else
            {
                if (GenresViewModel.fullGenreList == null)
                {
                    var list = await Database.GetAllGenresAsync(showHiddenGenres);
                    filteredList = list.ToObservableCollection();
                }
                else
                {
                    filteredList = GenresViewModel.fullGenreList;
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<SeriesModel>?> GetAllSeriesList(bool showHiddenSeries)
        {
            ObservableCollection<SeriesModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllSeriesList(showHiddenSeries);
            }
            else
            {
                if (SeriesViewModel.fullSeriesList == null)
                {
                    var list = await Database.GetAllSeriesAsync(showHiddenSeries);
                    filteredList = list.ToObservableCollection();
                }
                else
                {
                    filteredList = SeriesViewModel.fullSeriesList;
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<LocationModel>?> GetAllLocationsList(bool showHiddenLocations)
        {
            ObservableCollection<LocationModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllLocationsList(showHiddenLocations);
            }
            else
            {
                if (LocationsViewModel.fullLocationList == null)
                {
                    var list = await Database.GetAllLocationsAsync(showHiddenLocations);
                    filteredList = list.ToObservableCollection();
                }
                else
                {
                    filteredList = LocationsViewModel.fullLocationList;
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInAuthorList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = [];

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    filteredList = TestData.GetAllBooksInAuthorList(inputGuid, showHiddenBooks);
                }
                else
                {
                    var list = await Database.GetAllBooksForAuthorAsync((Guid)inputGuid, showHiddenBooks);
                    filteredList = list.ToObservableCollection();
                }
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutAuthorList(string reverseAuthorName, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllBooksWithoutAuthorList(reverseAuthorName, showHiddenBooks);
            }
            else
            {
                var list = await Database.GetAllBooksWithoutAuthorAsync(reverseAuthorName, showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<AuthorModel>?> GetAllAuthorsList(bool showHiddenAuthors)
        {
            ObservableCollection<AuthorModel>? filteredList = null;

            if (TestData.UseTestData)
            {
                filteredList = TestData.GetAllAuthorsList(showHiddenAuthors);
            }
            else
            {
                if (AuthorsViewModel.fullAuthorList == null)
                {
                    var list = await Database.GetAllAuthorsAsync(showHiddenAuthors);
                    filteredList = list.ToObservableCollection();
                }
                else
                {
                    filteredList = AuthorsViewModel.fullAuthorList;
                }
            }

            return filteredList;
        }
    }
}
