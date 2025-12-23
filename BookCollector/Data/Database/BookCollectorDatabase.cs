// <copyright file="BookCollectorDatabase.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.ViewModels.BaseViewModels;
using SQLite;

namespace BookCollector.Data.Database
{
    public partial class BookCollectorDatabase : BaseViewModel
    {
        private SQLiteAsyncConnection database;

        public BookCollectorDatabase()
        {
        }

        public async Task Init()
        {
            if (this.database is null)
            {
                this.database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            }

            try
            {
                var loadDataTasks = new Task[]
                {
                    Task.Run(() => this.database.CreateTableAsync<AuthorDatabaseModel>()),
                    Task.Run(() => this.database.CreateTableAsync<BookAuthorModel>()),
                    Task.Run(() => this.database.CreateTableAsync<BookDatabaseModel>()),
                    Task.Run(() => this.database.CreateTableAsync<WishlistBookDatabaseModel>()),
                    Task.Run(() => this.database.CreateTableAsync<ChapterDatabaseModel>()),
                    Task.Run(() => this.database.CreateTableAsync<CollectionDatabaseModel>()),
                    Task.Run(() => this.database.CreateTableAsync<GenreDatabaseModel>()),
                    Task.Run(() => this.database.CreateTableAsync<LocationDatabaseModel>()),
                    Task.Run(() => this.database.CreateTableAsync<SeriesDatabaseModel>()),
                };

                await Task.WhenAll(loadDataTasks);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task DropAllTables()
        {
            if (this.database is null)
            {
                this.database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            }

            await this.database.DropTableAsync<AuthorDatabaseModel>();
            await this.database.DropTableAsync<BookAuthorModel>();
            await this.database.DropTableAsync<BookDatabaseModel>();
            await this.database.DropTableAsync<WishlistBookDatabaseModel>();
            await this.database.DropTableAsync<ChapterDatabaseModel>();
            await this.database.DropTableAsync<CollectionDatabaseModel>();
            await this.database.DropTableAsync<GenreDatabaseModel>();
            await this.database.DropTableAsync<LocationDatabaseModel>();
            await this.database.DropTableAsync<SeriesDatabaseModel>();

            await this.database.CloseAsync();
        }

        /*********************** Book Methods ***********************/
        public async Task<List<BookModel>> GetAllReadingBooksAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => (x.BookPageRead != x.BookPageTotal && x.BookPageRead != 0) || (x.UpNext == true))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllToBeReadBooksAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookPageRead == 0 && x.UpNext == false)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllReadBooksAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookPageRead == x.BookPageTotal && x.BookPageRead != 0)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GenreModel?> GetGenreForBookAsync(Guid? inputGuid)
        {
            try
            {
                await this.Init();

                var genre = await this.database.Table<GenreDatabaseModel>()
                    .Where(x => x.GenreGuid == inputGuid)
                    .FirstOrDefaultAsync();

                return ConvertTo<GenreModel>(genre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LocationModel?> GetLocationForBookAsync(Guid? inputGuid)
        {
            try
            {
                await this.Init();

                var location = await this.database.Table<LocationDatabaseModel>()
                    .Where(x => x.LocationGuid == inputGuid)
                    .FirstOrDefaultAsync();

                return ConvertTo<LocationModel>(location);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SeriesModel?> GetSeriesForBookAsync(Guid? inputGuid)
        {
            try
            {
                await this.Init();

                var series = await this.database.Table<SeriesDatabaseModel>()
                    .Where(x => x.SeriesGuid == inputGuid)
                    .FirstOrDefaultAsync();

                return ConvertTo<SeriesModel>(series);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CollectionModel?> GetCollectionForBookAsync(Guid? inputGuid)
        {
            try
            {
                await this.Init();

                var collection = await this.database.Table<CollectionDatabaseModel>()
                    .Where(x => x.CollectionGuid == inputGuid)
                    .FirstOrDefaultAsync();

                return ConvertTo<CollectionModel>(collection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetBooksReadInYearAsync(int year, bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.BookStartDate) && !string.IsNullOrEmpty(x.BookEndDate))
                    .ToListAsync();

                var booksList = books
                    .Where(x => !string.IsNullOrEmpty(x.BookEndDate) && DateTime.Parse(x.BookEndDate).Year == year)
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksWithAPriceAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookAuthorModel>> GetAllBookAuthorsForBookAsync(Guid? bookGuid)
        {
            try
            {
                await this.Init();

                var bookAuthors = await this.database.Table<BookAuthorModel>()
                    .Where(x => x.BookGuid == bookGuid)
                    .OrderBy(x => x.BookGuid)
                    .ToListAsync();

                return bookAuthors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Guid>> GetAllAuthorGuidsForBookAsync(Guid bookGuid)
        {
            try
            {
                await this.Init();

                var bookAuthorGuids = await this.database.Table<BookAuthorModel>()
                    .Where(x => x.BookGuid == bookGuid)
                    .ToListAsync();

                return [.. bookAuthorGuids
                .Select(x => x.AuthorGuid)
                .Distinct()];
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AuthorModel>> GetAllAuthorsForBookAsync(List<Guid> authorGuids)
        {
            try
            {
                await this.Init();

                List<AuthorModel> authors = [];

                foreach (var authorGuid in authorGuids)
                {
                    var author = await this.database.Table<AuthorDatabaseModel>()
                        .Where(x => x.AuthorGuid == authorGuid)
                        .FirstOrDefaultAsync();

                    if (author != null)
                    {
                        authors.Add(ConvertTo<AuthorModel>(author));
                    }
                }

                return authors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BookDatabaseModel> SaveBookAsync(BookDatabaseModel book)
        {
            try
            {
                await this.Init();

                if (book.BookGuid != null)
                {
                    var existingBook = await this.database.Table<BookDatabaseModel>()
                        .Where(x => x.BookGuid == book.BookGuid)
                        .FirstOrDefaultAsync();

                    if (existingBook != null)
                    {
                        await this.database.UpdateAsync(book);
                    }
                    else
                    {
                        await this.database.InsertAsync(book);
                    }
                }
                else
                {
                    book.BookGuid = Guid.NewGuid();
                    await this.database.InsertAsync(book);
                }

                return book;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteBookAsync(BookDatabaseModel book)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(book);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BookDatabaseModel?> GetBookByGuidAsync(Guid bookGuid)
        {
            try
            {
                await this.Init();

                var book = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookGuid == bookGuid)
                    .FirstOrDefaultAsync();

                return book;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<BookDatabaseModel?> GetBookByNameAsync(string bookName)
        {
            try
            {
                await this.Init();

                var book = await this.database.Table<BookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.BookTitle) && x.BookTitle.Equals(bookName))
                    .FirstOrDefaultAsync();

                return book;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Book Methods ***********************/

        /*********************** Wishlist Book Methods ***********************/
        public async Task<List<WishlistBookModel>> GetAllWishlistBooksAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .ToListAsync();

                var booksList = books
                        .Select(x => new WishlistBookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<WishlistBookModel>> GetAllWishlistBooksWithAPriceAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.BookPrice))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new WishlistBookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<WishlistBookModel>> GetAllWishlistBooksWithALocationAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new WishlistBookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<WishlistBookModel>> GetAllWishlistBooksWithoutALocationAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => string.IsNullOrEmpty(x.BookWhereToBuy))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new WishlistBookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string?>> GetAllWishlistBooksLocationsAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.BookWhereToBuy))
                    .ToListAsync();

                if (!showHiddenBooks)
                {
                    books = [.. books.Where(x => !x.HideBook)];
                }

                var locations = books
                    .Select(x => x.BookWhereToBuy)
                    .Distinct()
                    .ToList();

                return locations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<WishlistBookModel>> GetAllWishlistBooksWithASeriesAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.BookSeries))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new WishlistBookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<WishlistBookModel>> GetAllWishlistBooksWithoutASeriesAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => string.IsNullOrEmpty(x.BookSeries))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new WishlistBookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string?>> GetAllWishlistBooksSeriesAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.BookSeries))
                    .ToListAsync();

                if (!showHiddenBooks)
                {
                    books = [.. books.Where(x => !x.HideBook)];
                }

                var series = books
                    .Select(x => x.BookSeries)
                    .Distinct()
                    .ToList();

                return series;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<WishlistBookModel>> GetAllWishlistBooksWithAuthorsAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.AuthorListString))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new WishlistBookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<WishlistBookModel>> GetAllWishlistBooksWithoutAuthorsAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => string.IsNullOrEmpty(x.AuthorListString))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new WishlistBookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string?>> GetAllWishlistBooksAuthorsAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.AuthorListString))
                    .ToListAsync();

                if (!showHiddenBooks)
                {
                    books = [.. books.Where(x => !x.HideBook)];
                }

                var authors = books
                    .Select(x => x.AuthorListString)
                    .Distinct()
                    .ToList();

                return authors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WishlistBookModel> SaveWishlistBookAsync(WishlistBookDatabaseModel book)
        {
            try
            {
                await this.Init();

                if (book.BookGuid != null)
                {
                    var existingBook = await this.database.Table<WishlistBookDatabaseModel>()
                        .Where(x => x.BookGuid == book.BookGuid)
                        .FirstOrDefaultAsync();

                    if (existingBook != null)
                    {
                        await this.database.UpdateAsync(book);
                    }
                    else
                    {
                        await this.database.InsertAsync(book);
                    }
                }
                else
                {
                    book.BookGuid = Guid.NewGuid();
                    await this.database.InsertAsync(book);
                }

                return ConvertTo<WishlistBookModel>(book);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteWishlistBookAsync(WishlistBookDatabaseModel book)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(book);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WishlistBookDatabaseModel?> GetWishlistBookByGuidAsync(Guid bookGuid)
        {
            try
            {
                await this.Init();

                var book = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => x.BookGuid == bookGuid)
                    .FirstOrDefaultAsync();

                return book;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WishlistBookDatabaseModel?> GetWishlistBookByNameAsync(string bookName)
        {
            try
            {
                await this.Init();

                var book = await this.database.Table<WishlistBookDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.BookTitle) && x.BookTitle.Equals(bookName))
                    .FirstOrDefaultAsync();

                return book;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Wishlist Book Methods ***********************/

        /*********************** Statistics Methods ***********************/
        public async Task<List<BookModel>> GetBooksByFavoriteAsync(bool showHiddenBooks, bool favoriteValue)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.IsFavorite == favoriteValue)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetBooksByRatingAsync(bool showHiddenBooks, int ratingValue)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.Rating == ratingValue)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Statistics Methods ***********************/

        /*********************** Chapter Methods ***********************/
        public async Task<List<ChapterModel>> GetChaptersInBookAsync(Guid bookGuid)
        {
            try
            {
                await this.Init();

                var chapters = await this.database.Table<ChapterDatabaseModel>()
                    .Where(x => x.BookGuid == bookGuid)
                    .OrderBy(x => x.ChapterOrder)
                    .ToListAsync();

                var chaptersList = chapters
                    .Select(x => new ChapterModel(x))
                    .ToList();

                return chaptersList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ChapterModel>> GetAllChaptersAsync()
        {
            try
            {
                await this.Init();

                var chapters = await this.database.Table<ChapterDatabaseModel>()
                    .OrderBy(x => x.ChapterOrder)
                    .OrderBy(x => x.BookGuid)
                    .ToListAsync();

                var chaptersList = chapters
                    .Select(x => new ChapterModel(x))
                    .ToList();

                return chaptersList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveChapterAsync(ChapterDatabaseModel chapter)
        {
            try
            {
                await this.Init();

                if (chapter.ChapterGuid != null)
                {
                    var existingChapter = await this.database.Table<ChapterDatabaseModel>()
                        .Where(x => x.ChapterGuid == chapter.ChapterGuid)
                        .FirstOrDefaultAsync();

                    if (existingChapter != null)
                    {
                        await this.database.UpdateAsync(chapter);
                    }
                    else
                    {
                        await this.database.InsertAsync(chapter);
                    }
                }
                else
                {
                    chapter.ChapterGuid = Guid.NewGuid();
                    await this.database.InsertAsync(chapter);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteChapterAsync(ChapterDatabaseModel chapter)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(chapter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ChapterDatabaseModel?> GetChapterByGuidAsync(Guid chapterGuid)
        {
            try
            {
                await this.Init();

                var series = await this.database.Table<ChapterDatabaseModel>()
                    .Where(x => x.ChapterGuid == chapterGuid)
                    .FirstOrDefaultAsync();

                return series;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Chapter Methods ***********************/

        /*********************** Book Author Methods ***********************/

        public async Task<List<BookAuthorModel>> GetAllBookAuthorsAsync()
        {
            try
            {
                await this.Init();

                var bookAuthors = await this.database.Table<BookAuthorModel>()
                    .OrderBy(x => x.BookGuid)
                    .ToListAsync();

                return bookAuthors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveBookAuthorAsync(BookAuthorModel bookAuthor)
        {
            try
            {
                await this.Init();

                if (bookAuthor.BookAuthorGuid != null)
                {
                    var existingBookAuthor = await this.database.Table<BookAuthorModel>()
                        .Where(x => x.BookAuthorGuid == bookAuthor.BookAuthorGuid)
                        .FirstOrDefaultAsync();

                    if (existingBookAuthor != null)
                    {
                        await this.database.UpdateAsync(bookAuthor);
                    }
                    else
                    {
                        await this.database.InsertAsync(bookAuthor);
                    }
                }
                else
                {
                    bookAuthor.BookAuthorGuid = Guid.NewGuid();
                    await this.database.InsertAsync(bookAuthor);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteBookAuthorAsync(BookAuthorModel bookAuthor)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(bookAuthor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteBookAuthorAsync(Guid authorGuid, Guid bookGuid)
        {
            try
            {
                await this.Init();

                var bookAuthor = await this.database.Table<BookAuthorModel>()
                    .Where(x => x.AuthorGuid == authorGuid && x.BookGuid == bookGuid)
                    .FirstOrDefaultAsync();

                if (bookAuthor != null)
                {
                    await this.database.DeleteAsync(bookAuthor);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddAuthorToBookAsync(Guid? authorGuid, Guid? bookGuid)
        {
            try
            {
                if (authorGuid != null && bookGuid != null)
                {
                    var existingBookAuthor = await this.database.Table<BookAuthorModel>()
                        .Where(x => x.AuthorGuid == authorGuid && x.BookGuid == bookGuid).
                        FirstOrDefaultAsync();

                    if (existingBookAuthor != null)
                    {
                        await this.SaveBookAuthorAsync(existingBookAuthor);
                    }
                    else
                    {
                        var bookAuthor = new BookAuthorModel()
                        {
                            AuthorGuid = (Guid)authorGuid,
                            BookGuid = (Guid)bookGuid,
                        };

                        await this.SaveBookAuthorAsync(bookAuthor);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Book Author Methods ***********************/

        /*********************** Author Methods ***********************/
        public async Task<List<BookAuthorModel>> GetAllBookAuthorsForAuthorAsync(Guid authorGuid)
        {
            try
            {
                await this.Init();

                var bookAuthors = await this.database.Table<BookAuthorModel>()
                    .Where(x => x.AuthorGuid == authorGuid)
                    .OrderBy(x => x.BookGuid)
                    .ToListAsync();

                return bookAuthors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksForAuthorAsync(Guid authorGuid, bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var bookAuthors = await this.database.Table<BookAuthorModel>()
                    .Where(x => x.AuthorGuid == authorGuid)
                    .OrderBy(x => x.BookGuid)
                    .ToListAsync();

                var books = new List<BookDatabaseModel>();

                foreach (var bookAuthor in bookAuthors)
                {
                    var book = await this.database.Table<BookDatabaseModel>()
                        .Where(x => x.BookGuid == bookAuthor.BookGuid)
                        .FirstOrDefaultAsync();

                    if (book != null)
                    {
                        books.Add(book);
                    }
                }

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksWithoutAuthorAsync(string reverseAuthorName, bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => string.IsNullOrEmpty(x.AuthorListString) || (!string.IsNullOrEmpty(x.AuthorListString) && !x.AuthorListString.Contains(reverseAuthorName)))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AuthorModel>> GetAllAuthorsAsync(bool showHiddenAuthors)
        {
            try
            {
                await this.Init();

                var authors = await this.database.Table<AuthorDatabaseModel>()
                    .ToListAsync();

                var authorsList = authors
                    .Select(x => new AuthorModel(x))
                    .OrderBy(x => x.FirstName)
                    .OrderBy(x => x.LastName)
                    .ToList();

                authorsList.ForEach(x => x.SetTotalBooks(true));
                authorsList.ForEach(x => x.SetTotalCostOfBooks(true));

                if (!showHiddenAuthors)
                {
                    return [.. authorsList.Where(x => !x.HideAuthor)];
                }

                return authorsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksWithoutAnAuthorAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => string.IsNullOrEmpty(x.AuthorListString))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AuthorModel> SaveAuthorAsync(AuthorDatabaseModel author)
        {
            try
            {
                await this.Init();

                if (author.AuthorGuid != null)
                {
                    var existingAuthor = await this.database.Table<AuthorDatabaseModel>()
                        .Where(x => x.AuthorGuid == author.AuthorGuid)
                        .FirstOrDefaultAsync();

                    if (existingAuthor != null)
                    {
                        await this.database.UpdateAsync(author);
                    }
                    else
                    {
                        await this.database.InsertAsync(author);
                    }
                }
                else
                {
                    author.AuthorGuid = Guid.NewGuid();
                    await this.database.InsertAsync(author);
                }

                return ConvertTo<AuthorModel>(author);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAuthorAsync(AuthorDatabaseModel author)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(author);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AuthorModel> InsertAuthorAsync(AuthorDatabaseModel author, Guid? bookGuid)
        {
            try
            {
                var authorModel = await this.SaveAuthorAsync(author);

                await this.AddAuthorToBookAsync(author.AuthorGuid, bookGuid);

                return authorModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AuthorDatabaseModel?> GetAuthorByGuidAsync(Guid authorGuid)
        {
            try
            {
                await this.Init();

                var author = await this.database.Table<AuthorDatabaseModel>()
                    .Where(x => x.AuthorGuid == authorGuid)
                    .FirstOrDefaultAsync();

                return author;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AuthorModel?> GetAuthorByNameAsync(string authorFirstName, string authorLastName)
        {
            try
            {
                await this.Init();

                var author = await this.database.Table<AuthorDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.FirstName) && x.FirstName.Equals(authorFirstName) && !string.IsNullOrEmpty(x.LastName) && x.LastName.Equals(authorLastName))
                    .FirstOrDefaultAsync();

                return ConvertTo<AuthorModel>(author);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Author Methods ***********************/

        /*********************** Collection Methods ***********************/
        public async Task<List<BookModel>> GetAllBooksInCollectionAsync(Guid collectionGuid, bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookCollectionGuid == collectionGuid)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksWithoutACollectionAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookCollectionGuid == null)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CollectionModel>> GetAllCollectionsAsync(bool showHiddenCollections)
        {
            try
            {
                await this.Init();

                var collections = await this.database.Table<CollectionDatabaseModel>()
                    .ToListAsync();

                var collectionsList = collections
                    .Select(x => new CollectionModel(x))
                    .OrderBy(x => x.ParsedCollectionName)
                    .ToList();

                collectionsList.ForEach(x => x.SetTotalBooks(true));
                collectionsList.ForEach(x => x.SetTotalCostOfBooks(true));

                if (!showHiddenCollections)
                {
                    return [.. collectionsList.Where(x => !x.HideCollection)];
                }

                return collectionsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CollectionModel> SaveCollectionAsync(CollectionDatabaseModel collection)
        {
            try
            {
                await this.Init();

                if (collection.CollectionGuid != null)
                {
                    var existingCollection = await this.database.Table<CollectionDatabaseModel>()
                        .Where(x => x.CollectionGuid == collection.CollectionGuid)
                        .FirstOrDefaultAsync();

                    if (existingCollection != null)
                    {
                        await this.database.UpdateAsync(collection);
                    }
                    else
                    {
                        await this.database.InsertAsync(collection);
                    }
                }
                else
                {
                    collection.CollectionGuid = Guid.NewGuid();
                    await this.database.InsertAsync(collection);
                }

                return ConvertTo<CollectionModel>(collection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteCollectionAsync(CollectionDatabaseModel collection)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(collection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CollectionDatabaseModel?> GetCollectionByGuidAsync(Guid collectionGuid)
        {
            try
            {
                await this.Init();

                var collection = await this.database.Table<CollectionDatabaseModel>()
                    .Where(x => x.CollectionGuid == collectionGuid)
                    .FirstOrDefaultAsync();

                return collection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CollectionDatabaseModel?> GetCollectionByNameAsync(string collectionName)
        {
            try
            {
                await this.Init();

                var collection = await this.database.Table<CollectionDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.CollectionName) && x.CollectionName.Equals(collectionName))
                    .FirstOrDefaultAsync();

                return collection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Collection Methods ***********************/

        /*********************** Genre Methods ***********************/
        public async Task<List<BookModel>> GetAllBooksInGenreAsync(Guid genreGuid, bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookGenreGuid == genreGuid)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksWithoutAGenreAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookGenreGuid == null)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GenreModel>> GetAllGenresAsync(bool showHiddenGenres)
        {
            try
            {
                await this.Init();

                var genres = await this.database.Table<GenreDatabaseModel>()
                    .ToListAsync();

                var genresList = genres
                        .Select(x => new GenreModel(x))
                        .OrderBy(x => x.ParsedGenreName)
                        .ToList();

                genresList.ForEach(x => x.SetTotalBooks(true));
                genresList.ForEach(x => x.SetTotalCostOfBooks(true));

                if (!showHiddenGenres)
                {
                    return [.. genresList.Where(x => !x.HideGenre)];
                }

                return genresList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GenreModel> SaveGenreAsync(GenreDatabaseModel genre)
        {
            try
            {
                await this.Init();

                if (genre.GenreGuid != null)
                {
                    var existingGenre = await this.database.Table<GenreDatabaseModel>()
                        .Where(x => x.GenreGuid == genre.GenreGuid)
                        .FirstOrDefaultAsync();

                    if (existingGenre != null)
                    {
                        await this.database.UpdateAsync(genre);
                    }
                    else
                    {
                        await this.database.InsertAsync(genre);
                    }
                }
                else
                {
                    genre.GenreGuid = Guid.NewGuid();
                    await this.database.InsertAsync(genre);
                }

                return ConvertTo<GenreModel>(genre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteGenreAsync(GenreDatabaseModel genre)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(genre);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GenreDatabaseModel?> GetGenreByGuidAsync(Guid genreGuid)
        {
            try
            {
                await this.Init();

                var genre = await this.database.Table<GenreDatabaseModel>()
                    .Where(x => x.GenreGuid == genreGuid)
                    .FirstOrDefaultAsync();

                return genre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GenreDatabaseModel?> GetGenreByNameAsync(string genreName)
        {
            try
            {
                await this.Init();

                var genre = await this.database.Table<GenreDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.GenreName) && x.GenreName.Equals(genreName))
                    .FirstOrDefaultAsync();

                return genre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Genre Methods ***********************/

        /*********************** Series Methods ***********************/
        public async Task<List<BookModel>> GetAllBooksInSeriesAsync(Guid seriesGuid, bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookSeriesGuid == seriesGuid)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksWithoutASeriesAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookSeriesGuid == null)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SeriesModel>> GetAllSeriesAsync(bool showHiddenSeries)
        {
            try
            {
                await this.Init();

                var series = await this.database.Table<SeriesDatabaseModel>()
                    .ToListAsync();

                var seriesList = series
                        .Select(x => new SeriesModel(x))
                        .OrderBy(x => x.ParsedSeriesName)
                        .ToList();

                seriesList.ForEach(x => x.SetTotalBooks(true));
                seriesList.ForEach(x => x.SetTotalCostOfBooks(true));

                if (!showHiddenSeries)
                {
                    return [.. seriesList.Where(x => !x.HideSeries)];
                }

                return seriesList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SeriesModel> SaveSeriesAsync(SeriesDatabaseModel series)
        {
            try
            {
                await this.Init();

                if (series.SeriesGuid != null)
                {
                    var existingSeries = await this.database.Table<SeriesDatabaseModel>()
                        .Where(x => x.SeriesGuid == series.SeriesGuid)
                        .FirstOrDefaultAsync();

                    if (existingSeries != null)
                    {
                        await this.database.UpdateAsync(series);
                    }
                    else
                    {
                        await this.database.InsertAsync(series);
                    }
                }
                else
                {
                    series.SeriesGuid = Guid.NewGuid();
                    await this.database.InsertAsync(series);
                }

                return ConvertTo<SeriesModel>(series);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteSeriesAsync(SeriesDatabaseModel series)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(series);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SeriesDatabaseModel?> GetSeriesByGuidAsync(Guid seriesGuid)
        {
            try
            {
                await this.Init();

                var series = await this.database.Table<SeriesDatabaseModel>()
                    .Where(x => x.SeriesGuid == seriesGuid)
                    .FirstOrDefaultAsync();

                return series;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SeriesModel?> GetSeriesByNameAsync(string? seriesName)
        {
            try
            {
                await this.Init();

                var series = await this.database.Table<SeriesDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.Equals(seriesName))
                    .FirstOrDefaultAsync();

                return ConvertTo<SeriesModel>(series);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Series Methods ***********************/

        /*********************** Location Methods ***********************/
        public async Task<List<BookModel>> GetAllBooksInLocationAsync(Guid locationGuid, bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookLocationGuid == locationGuid)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<BookModel>> GetAllBooksWithoutALocationAsync(bool showHiddenBooks)
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => x.BookLocationGuid == null)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                if (!showHiddenBooks)
                {
                    return [.. booksList.Where(x => !x.HideBook)];
                }

                return booksList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<LocationModel>> GetAllLocationsAsync(bool showHiddenLocations)
        {
            try
            {
                await this.Init();

                var locations = await this.database.Table<LocationDatabaseModel>()
                    .ToListAsync();

                var locationsList = locations
                        .Select(x => new LocationModel(x))
                        .OrderBy(x => x.ParsedLocationName)
                        .ToList();

                locationsList.ForEach(x => x.SetTotalBooks(true));
                locationsList.ForEach(x => x.SetTotalCostOfBooks(true));

                if (!showHiddenLocations)
                {
                    return [.. locationsList.Where(x => !x.HideLocation)];
                }

                return locationsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LocationModel> SaveLocationAsync(LocationDatabaseModel location)
        {
            try
            {
                await this.Init();

                if (location.LocationGuid != null)
                {
                    var existingLocation = await this.database.Table<LocationDatabaseModel>()
                        .Where(x => x.LocationGuid == location.LocationGuid)
                        .FirstOrDefaultAsync();

                    if (existingLocation != null)
                    {
                        await this.database.UpdateAsync(location);
                    }
                    else
                    {
                        await this.database.InsertAsync(location);
                    }
                }
                else
                {
                    location.LocationGuid = Guid.NewGuid();
                    await this.database.InsertAsync(location);
                }

                return ConvertTo<LocationModel>(location);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteLocationAsync(LocationDatabaseModel location)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(location);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LocationDatabaseModel?> GetLocationByGuidAsync(Guid locationGuid)
        {
            try
            {
                await this.Init();

                var location = await this.database.Table<LocationDatabaseModel>()
                    .Where(x => x.LocationGuid == locationGuid)
                    .FirstOrDefaultAsync();

                return location;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LocationDatabaseModel?> GetLocationByNameAsync(string locationName)
        {
            try
            {
                await this.Init();

                var location = await this.database.Table<LocationDatabaseModel>()
                    .Where(x => !string.IsNullOrEmpty(x.LocationName) && x.LocationName.Equals(locationName))
                    .FirstOrDefaultAsync();

                return location;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*********************** Location Methods ***********************/
    }
}
