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

    public partial class FillLists : BaseViewModel
    {
        public static async Task<ObservableCollection<BookModel>?> GetReadingBooksList()
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (AllBooksViewModel.filteredBookList1 != null)
            {
                filteredList = AllBooksViewModel.filteredBookList1
                    .Where(x => (x.BookPageRead != x.BookPageTotal && x.BookPageRead != 0) ||
                    x.UpNext ||
                    (x.BookHourListened != x.BookHoursTotal && x.BookMinuteListened != x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0))
                    .ToObservableCollection();
            }
            else
            {
                var list = await Database.GetAllReadingBooksAsync();
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetToBeReadBooksList()
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (AllBooksViewModel.filteredBookList1 != null)
            {
                filteredList = AllBooksViewModel.filteredBookList1
                    .Where(x => (x.BookPageRead == 0 &&
                    (x.BookHourListened == 0 && x.BookMinuteListened == 0))
                    && !x.UpNext)
                    .ToObservableCollection();
            }
            else
            {
                var list = await Database.GetAllToBeReadBooksAsync();
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetReadBooksList()
        {
            ObservableCollection<BookModel>? filteredList = null;

            if (AllBooksViewModel.filteredBookList1 != null)
            {
                filteredList = AllBooksViewModel.filteredBookList1
                    .Where(x => (x.BookPageRead == x.BookPageTotal && x.BookPageRead != 0) ||
                    (x.BookHourListened == x.BookHoursTotal && x.BookMinuteListened == x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0))
                    .ToObservableCollection();
            }
            else
            {
                var list = await Database.GetAllReadBooksAsync();
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksList()
        {
            ObservableCollection<BookModel>? filteredList = null;

            var list = await Database.GetAllBooksAsync();
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<WishlistBookModel>?> GetBookWishList()
        {
            ObservableCollection<WishlistBookModel>? filteredList = null;

            var list = await Database.GetAllWishlistBooksAsync();
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<ChapterModel>?> GetAllChaptersInBook(Guid? inputGuid)
        {
            ObservableCollection<ChapterModel>? chapterList = null;

            if (inputGuid != null)
            {
                var list = await Database.GetChaptersInBookAsync((Guid)inputGuid);
                chapterList = list.ToObservableCollection();
            }

            return chapterList;
        }

        public static async Task<ObservableCollection<ChapterModel>?> GetAllChapters()
        {
            ObservableCollection<ChapterModel>? chapterList = null;

            var list = await Database.GetAllChaptersAsync();
            chapterList = list.ToObservableCollection();

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

            publisherList = publisherList.Distinct().ToObservableCollection();

            publisherList = publisherList.OrderBy(x => x).ToObservableCollection();

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

            publisherList = publisherList.OrderBy(x => x).ToObservableCollection();

            return publisherList;
        }

        public static async Task<ObservableCollection<string>> GetAllAuthorsInBookList(ObservableCollection<BookModel> bookList)
        {
            var authorListNames = new ObservableCollection<string>();
            var authorList = new ObservableCollection<AuthorModel>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var list = SplitStringIntoAuthorList(book.AuthorListString);

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

        public static async Task<ObservableCollection<string>> GetAllAuthorsInWishlistBookList(ObservableCollection<WishlistBookModel> bookList)
        {
            var authorListNames = new ObservableCollection<string>();
            var authorList = new ObservableCollection<AuthorModel>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.AuthorListString))
                {
                    var list = SplitStringIntoAuthorList(book.AuthorListString);

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
                    locationList.Add(char.ToUpper(book.BookWhereToBuy[0]) + book.BookWhereToBuy[1..].ToLower());
                }
            }

            locationList = locationList.Distinct().ToObservableCollection();

            locationList = locationList.OrderBy(x => x).ToObservableCollection();

            return locationList;
        }

        public static async Task<ObservableCollection<string>> GetAllSeriesInBookList(ObservableCollection<BookModel> bookList)
        {
            var seriesListNames = new ObservableCollection<string>();
            var seriesList = new ObservableCollection<SeriesModel>();

            foreach (var book in bookList)
            {
                if (!string.IsNullOrEmpty(book.BookSeries) && !seriesList.Any(x => x.Equals(book.BookSeries)))
                {
                    seriesList.Add(new SeriesModel()
                    {
                        SeriesName = book.BookSeries,
                    });
                }
            }

            seriesList = seriesList.Distinct().ToObservableCollection();

            seriesList = seriesList.OrderBy(x => x.ParsedSeriesName).ToObservableCollection();

            seriesListNames = seriesList.Select(x => x.SeriesName!).ToObservableCollection();

            return seriesListNames;
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

            seriesList = seriesList.OrderBy(x => x).ToObservableCollection();

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
                    languageList.Add(char.ToUpper(book.BookLanguage[0]) + book.BookLanguage[1..].ToLower());
                }
            }

            languageList = languageList.Distinct().ToObservableCollection();

            languageList = languageList.OrderBy(x => x).ToObservableCollection();

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
                var list = await Database.GetAllBookAuthorsForBookAsync(inputGuid);
                bookAuthorList = list.ToObservableCollection();
            }

            return bookAuthorList;
        }

        public static async Task<ObservableCollection<Guid>?> GetAllAuthorGuidsForBook(Guid? inputGuid)
        {
            ObservableCollection<Guid>? authorGuidList = null;

            if (inputGuid != null)
            {
                var list = await Database.GetAllAuthorGuidsForBookAsync((Guid)inputGuid);
                authorGuidList = list.ToObservableCollection();
            }

            return authorGuidList;
        }

        public static async Task<ObservableCollection<AuthorModel>?> GetAllAuthorsForBook(Guid? inputGuid, bool showHiddenAuthors)
        {
            var authorList = new ObservableCollection<AuthorModel>();

            if (inputGuid != null)
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

            return authorList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthors()
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            var list = await Database.GetAllBookAuthorsAsync();
            bookAuthorList = list.ToObservableCollection();

            return bookAuthorList;
        }

        public static async Task<ObservableCollection<BookAuthorModel>?> GetAllBookAuthorsForAuthor(Guid? inputGuid)
        {
            ObservableCollection<BookAuthorModel>? bookAuthorList = null;

            if (inputGuid != null)
            {
                var list = await Database.GetAllBookAuthorsForAuthorAsync((Guid)inputGuid);
                bookAuthorList = list.ToObservableCollection();
            }

            return bookAuthorList;
        }

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

                    if (!showHiddenBooks)
                    {
                        filteredList = new ObservableCollection<BookModel>(filteredList!.Where(x => !x.HideBook));
                    }
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

            var list = await Database.GetAllBooksWithoutACollectionAsync(showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

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

                    if (!showHiddenBooks)
                    {
                        filteredList = new ObservableCollection<BookModel>(filteredList!.Where(x => !x.HideBook));
                    }
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

            var list = await Database.GetAllBooksWithoutAGenreAsync(showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

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

                    if (!showHiddenBooks)
                    {
                        filteredList = new ObservableCollection<BookModel>(filteredList!.Where(x => !x.HideBook));
                    }
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

            var list = await Database.GetAllBooksWithoutASeriesAsync(showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

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

                    if (!showHiddenBooks)
                    {
                        filteredList = new ObservableCollection<BookModel>(filteredList!.Where(x => !x.HideBook));
                    }
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

            var list = await Database.GetAllBooksWithoutALocationAsync(showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<CollectionModel>?> GetAllCollectionsList()
        {
            ObservableCollection<CollectionModel>? filteredList = null;

            if (CollectionsViewModel.fullCollectionList == null)
            {
                var list = await Database.GetAllCollectionsAsync();
                filteredList = list.ToObservableCollection();
                CollectionsViewModel.fullCollectionList = filteredList;
            }
            else
            {
                filteredList = CollectionsViewModel.fullCollectionList;
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<GenreModel>?> GetAllGenresList()
        {
            ObservableCollection<GenreModel>? filteredList = null;

            if (GenresViewModel.fullGenreList == null)
            {
                var list = await Database.GetAllGenresAsync();
                filteredList = list.ToObservableCollection();
                GenresViewModel.fullGenreList = filteredList;
            }
            else
            {
                filteredList = GenresViewModel.fullGenreList;
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<SeriesModel>?> GetAllSeriesList()
        {
            ObservableCollection<SeriesModel>? filteredList = null;

            if (SeriesViewModel.fullSeriesList == null)
            {
                var list = await Database.GetAllSeriesAsync();
                filteredList = list.ToObservableCollection();
                SeriesViewModel.fullSeriesList = filteredList;
            }
            else
            {
                filteredList = SeriesViewModel.fullSeriesList;
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<LocationModel>?> GetAllLocationsList()
        {
            ObservableCollection<LocationModel>? filteredList = null;

            if (LocationsViewModel.fullLocationList == null)
            {
                var list = await Database.GetAllLocationsAsync();
                filteredList = list.ToObservableCollection();
                LocationsViewModel.fullLocationList = filteredList;
            }
            else
            {
                filteredList = LocationsViewModel.fullLocationList;
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksInAuthorList(Guid? inputGuid, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = [];

            if (inputGuid != null)
            {
                var list = await Database.GetAllBooksForAuthorAsync((Guid)inputGuid, showHiddenBooks);
                filteredList = list.ToObservableCollection();
            }

            return filteredList;
        }

        public static async Task<ObservableCollection<BookModel>?> GetAllBooksWithoutAuthorList(string reverseAuthorName, bool showHiddenBooks)
        {
            ObservableCollection<BookModel>? filteredList = null;

            var list = await Database.GetAllBooksWithoutAuthorAsync(reverseAuthorName, showHiddenBooks);
            filteredList = list.ToObservableCollection();

            return filteredList;
        }

        public static async Task<ObservableCollection<AuthorModel>?> GetAllAuthorsList()
        {
            ObservableCollection<AuthorModel>? filteredList = null;

            if (AuthorsViewModel.fullAuthorList == null)
            {
                var list = await Database.GetAllAuthorsAsync();
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
