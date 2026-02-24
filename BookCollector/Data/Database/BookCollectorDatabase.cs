// <copyright file="BookCollectorDatabase.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Database
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using CommunityToolkit.Maui.Core.Extensions;
    using SQLite;

    /// <summary>
    /// BookCollectorDatabase class.
    /// </summary>
    public partial class BookCollectorDatabase : BaseViewModel
    {
        private SQLiteAsyncConnection database;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookCollectorDatabase"/> class.
        /// </summary>
        public BookCollectorDatabase()
        {
        }

        /// <summary>
        /// Initializes the database and creates the necessary tables if they do not exist.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task Init()
        {
            this.database ??= new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

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

        /// <summary>
        /// Drops all the tables in the database.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task DropAllTables()
        {
            this.database ??= new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

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

        /// <summary>
        /// Gets a list of all books that are currently being read. A book is considered
        /// to be currently being read if the number of pages read is not equal to the total
        /// number of pages and the number of pages read is not equal to 0, or if the book
        /// is marked as up next, or if the number of hours listened is not equal to the
        /// total number of hours and the number of minutes listened is not equal to the
        /// total number of minutes and the number of hours listened is not equal to 0 and
        /// the number of minutes listened is not equal to 0.
        /// </summary>
        /// <returns>A list of all books that are currently being read.</returns>
        public async Task<List<BookModel>> GetAllReadingBooksAsync()
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => (x.BookPageRead != x.BookPageTotal && x.BookPageRead != 0) ||
                    x.UpNext ||
                    (x.BookHourListened != x.BookHoursTotal && x.BookMinuteListened != x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                return booksList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a list of all books that are yet to be read. A book is considered to be yet
        /// to be read if the number of pages read is equal to 0 and the number of hours
        /// listened is equal to 0 and the number of minutes listened is equal to 0 and the
        /// book is not marked as up next.
        /// </summary>
        /// <returns>A list of all books that are yet to be read.</returns>
        public async Task<List<BookModel>> GetAllToBeReadBooksAsync()
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => (x.BookPageRead == 0 &&
                    (x.BookHourListened == 0 && x.BookMinuteListened == 0))
                    && !x.UpNext)
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                return booksList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a list of all books that have been read. A book is considered to be read if
        /// the number of pages read is equal to the total number of pages and the number of
        /// pages read is not equal to 0, or if the number of hours listened is equal to the
        /// total number of hours and the number of minutes listened is equal to the total
        /// number of minutes and the number of hours listened is not equal to 0 and the number
        /// of minutes listened is not equal to 0.
        /// </summary>
        /// <returns>A list of all books that have have been read.</returns>
        public async Task<List<BookModel>> GetAllReadBooksAsync()
        {
            try
            {
                await this.Init();

                var books = await this.database.Table<BookDatabaseModel>()
                    .Where(x => (x.BookPageRead == x.BookPageTotal && x.BookPageRead != 0) ||
                    (x.BookHourListened == x.BookHoursTotal && x.BookMinuteListened == x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0))
                    .ToListAsync();

                var booksList = books
                        .Select(x => new BookModel(x))
                        .OrderBy(x => x.ParsedTitle)
                        .ToList();

                return booksList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a list of all books in the database, regardless of their reading status.
        /// </summary>
        /// <returns>A list of all books.</returns>
        public async Task<List<BookModel>> GetAllBooksAsync()
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

                return booksList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the genre for a book based on the provided genre guid.
        /// </summary>
        /// <param name="inputGuid">The guid of the genre to retrieve.</param>
        /// <returns>Selected genre.</returns>
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
                throw;
            }
        }

        /// <summary>
        /// Gets the location for a book based on the provided location guid.
        /// </summary>
        /// <param name="inputGuid">The guid of the location to retrieve.</param>
        /// <returns>Selected location.</returns>
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
                throw;
            }
        }

        /// <summary>
        /// Gets the series for a book based on the provided series guid.
        /// </summary>
        /// <param name="inputGuid">The guid of the series to retrieve.</param>
        /// <returns>Selected series.</returns>
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
                throw;
            }
        }

        /// <summary>
        /// Gets the collection for a book based on the provided collection guid.
        /// </summary>
        /// <param name="inputGuid">The guid of the collection to retrieve.</param>
        /// <returns>Selected collection.</returns>
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
                throw;
            }
        }

        /// <summary>
        /// Get all book authors for a book based on the provided book guid.
        /// </summary>
        /// <param name="bookGuid">The guid of the book to retrieve authors for.</param>
        /// <returns>A list of book authors for the book.</returns>
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
                throw;
            }
        }

        /// <summary>
        /// Get all author guids for a book based on the provided book guid.
        /// </summary>
        /// <param name="bookGuid">The guid of the book to retrieve authors for.</param>
        /// <returns>A list of author guids for the book.</returns>
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
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get authors objects based on a list of guids provided.
        /// </summary>
        /// <param name="authorGuids">The guids to retrieve authors for.</param>
        /// <returns>A list of authors for the book.</returns>
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
                throw;
            }
        }

        /// <summary>
        /// Add or update a book in the database.
        /// </summary>
        /// <param name="book">Book to save.</param>
        /// <returns>Book that has been saved.</returns>
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
                throw;
            }
        }

        /// <summary>
        /// Remove book from the database.
        /// </summary>
        /// <param name="book">Book to remove.</param>
        /// <returns>A task.</returns>
        public async Task DeleteBookAsync(BookDatabaseModel book)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(book);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*********************** Book Methods ***********************/

        /*********************** Wishlist Book Methods ***********************/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<WishlistBookModel>> GetAllWishlistBooksAsync()
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

                return booksList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        public async Task DeleteWishlistBookAsync(WishlistBookDatabaseModel book)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(book);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*********************** Wishlist Book Methods ***********************/

        /*********************** Chapter Methods ***********************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookGuid"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        public async Task DeleteChapterAsync(ChapterDatabaseModel chapter)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(chapter);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*********************** Chapter Methods ***********************/

        /*********************** Book Author Methods ***********************/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookAuthor"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorGuid"></param>
        /// <param name="bookGuid"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorGuid"></param>
        /// <param name="bookGuid"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /*********************** Book Author Methods ***********************/

        /*********************** Author Methods ***********************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorGuid"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorGuid"></param>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reverseAuthorName"></param>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<AuthorModel>> GetAllAuthorsAsync()
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

                return authorsList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public async Task DeleteAuthorAsync(AuthorDatabaseModel author)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(author);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="author"></param>
        /// <param name="bookGuid"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorGuid"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorFirstName"></param>
        /// <param name="authorLastName"></param>
        /// <returns></returns>
        public async Task<AuthorModel?> GetAuthorByNameAsync(string authorFirstName, string authorLastName)
        {
            try
            {
                await this.Init();

                var filteredList = new ObservableCollection<AuthorModel>();

                if (AuthorsViewModel.filteredAuthorList1 != null)
                {
                    filteredList = AuthorsViewModel.filteredAuthorList1;
                }
                else
                {
                    var authors = await this.database.Table<AuthorDatabaseModel>()
                    .ToListAsync();

                    var authorsList = authors
                        .Select(x => new AuthorModel(x))
                        .OrderBy(x => x.FirstName)
                        .OrderBy(x => x.LastName)
                        .ToList();

                    filteredList = authorsList.ToObservableCollection();
                }

                var author = filteredList
                    .Where(x => !string.IsNullOrEmpty(x.FirstName) && x.FirstName.Equals(authorFirstName) && !string.IsNullOrEmpty(x.LastName) && x.LastName.Equals(authorLastName))
                    .FirstOrDefault();

                return author;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*********************** Author Methods ***********************/

        /*********************** Collection Methods ***********************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionGuid"></param>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CollectionModel>> GetAllCollectionsAsync()
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

                return collectionsList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public async Task DeleteCollectionAsync(CollectionDatabaseModel collection)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(collection);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*********************** Collection Methods ***********************/

        /*********************** Genre Methods ***********************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genreGuid"></param>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<GenreModel>> GetAllGenresAsync()
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

                return genresList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        public async Task DeleteGenreAsync(GenreDatabaseModel genre)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(genre);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*********************** Genre Methods ***********************/

        /*********************** Series Methods ***********************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seriesGuid"></param>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<SeriesModel>> GetAllSeriesAsync()
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

                return seriesList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        public async Task DeleteSeriesAsync(SeriesDatabaseModel series)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(series);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seriesName"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /*********************** Series Methods ***********************/

        /*********************** Location Methods ***********************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationGuid"></param>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showHiddenBooks"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<LocationModel>> GetAllLocationsAsync()
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

                return locationsList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
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
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task DeleteLocationAsync(LocationDatabaseModel location)
        {
            try
            {
                await this.Init();
                await this.database.DeleteAsync(location);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*********************** Location Methods ***********************/
    }
}
