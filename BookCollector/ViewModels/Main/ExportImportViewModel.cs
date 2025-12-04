using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Data.Spreadsheet;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Main
{
    public partial class ExportImportViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string startOutput;

        [ObservableProperty]
        public string booksOutput;

        [ObservableProperty]
        public string wishListOutput;

        [ObservableProperty]
        public string collectionsOutput;

        [ObservableProperty]
        public string genresOutput;

        [ObservableProperty]
        public string seriesOutput;

        [ObservableProperty]
        public string authorsOutput;

        [ObservableProperty]
        public string locationsOutput;

        [ObservableProperty]
        public string chaptersOutput;

        [ObservableProperty]
        public string bookAuthorsOutput;

        [ObservableProperty]
        public string finalOutput;

        [ObservableProperty]
        public bool booksChecked;

        [ObservableProperty]
        public bool wishListChecked;

        [ObservableProperty]
        public bool collectionsChecked;

        [ObservableProperty]
        public bool genresChecked;

        [ObservableProperty]
        public bool seriesChecked;

        [ObservableProperty]
        public bool authorsChecked;

        [ObservableProperty]
        public bool locationsChecked;

        [ObservableProperty]
        public bool chaptersChecked;

        [ObservableProperty]
        public bool bookAuthorsChecked;

        [ObservableProperty]
        public bool checkboxesVisible;

        [ObservableProperty]
        public bool outputVisible;

        [ObservableProperty]
        public bool exportEnabled;

        [ObservableProperty]
        public bool importEnabled;

        private int _excelCellLimit = 32000;
        private string _filePath = string.Empty;

        public ExportImportViewModel(ContentPage view)
        {
            _view = view;
            InfoText = AppStringResources.ExportImportView_InfoText;
        }

        public async Task SetViewModelData()
        {
            SetIsBusyTrue();

            ExportEnabled = IsBusy;
            ImportEnabled = IsBusy;
            CheckboxesVisible = IsBusy;
            OutputVisible = !IsBusy;
            BooksChecked = true;
            ChaptersChecked = true;
            BookAuthorsChecked = true;
            WishListChecked = true;
            CollectionsChecked = true;
            GenresChecked = true;
            SeriesChecked = true;
            AuthorsChecked = true;
            LocationsChecked = true;

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task Export()
        {
            var action = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureExport_Question, null, null);

            if (action)
            {
                try
                {
                    SetIsBusyTrue();

                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;

                    ResetOutput();
                    OutputVisible = IsBusy;
                    CheckboxesVisible = !IsBusy;

                    var exportLocation = Preferences.Get("ExportLocation", AppStringResources.DefaultExportLocation  /* Default */);

                    if (exportLocation == null || exportLocation.Equals(AppStringResources.DefaultExportLocation))
                    {
                        var result = await FolderPicker.PickAsync(CancellationToken.None);

                        if (result != null)
                        {
                            exportLocation = result.Folder.Path;
                            Preferences.Set("ExportLocation", result.Folder.Path);
                        }
                        else
                            throw new Exception();
                    }

                    var filePath = ReadWriteSpreadsheet.CreateSpreadsheet(exportLocation);
                    _filePath = filePath;

                    StartOutput = AppStringResources.ExportResultsStart;

                    await WriteWishListBooksToSpreadsheet();
                    await WriteBooksToSpreadsheet();
                    await WriteChaptersToSpreadsheet();
                    await WriteCollectionsToSpreadsheet();
                    await WriteGenresToSpreadsheet();
                    await WriteSeriesToSpreadsheet();
                    await WriteLocationsToSpreadsheet();
                    await WriteBookAuthorsToSpreadsheet();
                    await WriteAuthorsToSpreadsheet();

                    FinalOutput = AppStringResources.ExportResultsFinish;

                    await DisplayMessage(AppStringResources.ExportComplete, null);

                    SetIsBusyFalse();
                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;
                }
                catch (Exception ex)
                {
                    await CanceledAction();
                    SetIsBusyFalse();
                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;
                }
            }
            else
            {
                await CanceledAction();
            }
        }

        [RelayCommand]
        public async Task Import()
        {
            var action = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureImport_Question, null, null);

            if (action)
            {
                try
                {
                    SetIsBusyTrue();

                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;

                    ResetOutput();
                    OutputVisible = IsBusy;
                    CheckboxesVisible = !IsBusy;

                    var customFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" } }, // MIME type
                    });


                    PickOptions pickerOptions = new()
                    {
                        FileTypes = customFileType,
                    };

                    var result = await FilePicker.Default.PickAsync(pickerOptions);

                    if (result != null)
                    {
                        StartOutput = AppStringResources.ImportResultsStart;

                        await ReadWishListBooksFromSpreadsheet();
                        await ReadAuthorsFromSpreadsheet();
                        await ReadChaptersFromSpreadsheet();
                        await ReadCollectionsFromSpreadsheet();
                        await ReadGenresFromSpreadsheet();
                        await ReadSeriesFromSpreadsheet();
                        await ReadLocationsFromSpreadsheet();
                        await ReadBooksFromSpreadsheet();
                        await ReadBookAuthorsFromSpreadsheet();

                        FinalOutput = AppStringResources.ImportResultsFinish;

                        await DisplayMessage(AppStringResources.ImportComplete, null);

                        SetIsBusyFalse();
                        ImportEnabled = !IsBusy;
                        ExportEnabled = !IsBusy;
                    }
                    else
                        throw new Exception(); 
                }
                catch (Exception ex)
                {
                    await CanceledAction();
                    SetIsBusyFalse();
                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;
                }
            }
            else
            {
                await CanceledAction();
            }
        }

        private void ResetOutput()
        {
            BooksOutput = string.Empty;
            WishListOutput = string.Empty;
            CollectionsOutput = string.Empty;
            GenresOutput = string.Empty;
            SeriesOutput = string.Empty;
            AuthorsOutput = string.Empty;
            LocationsOutput = string.Empty;
            ChaptersOutput = string.Empty;
            BookAuthorsOutput = string.Empty;
            FinalOutput = string.Empty;
        }

        #region Tables
        #region Book
        private async Task WriteBooksToSpreadsheet()
        {
            var tableName = "Books";

            if (BooksChecked)
            {
                BooksOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                // Unit test data
                var bookList = await FilterLists.GetAllBooksList(true);

                BooksOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{bookList.Count}");

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.BookGuid.Replace(" ", "")}",
                        $"{AppStringResources.BookTitle.Replace(" ", "")}",
                        $"{AppStringResources.BookSeriesGuid.Replace(" ", "")}",
                        $"{AppStringResources.BookNumber.Replace(" ", "")}",
                        $"{AppStringResources.BookPublisher.Replace(" ", "")}",
                        $"{AppStringResources.BookPublishYear.Replace(" ", "")}",
                        $"{AppStringResources.BookIdentifier.Replace(" ", "")}",
                        $"{AppStringResources.BookFormat.Replace(" ", "")}",
                        $"{AppStringResources.BookLanguage.Replace(" ", "")}",
                        $"{AppStringResources.BookPrice.Replace(" ", "")}",
                        $"{AppStringResources.BookSummary.Replace(" ", "")}",
                        $"{AppStringResources.PagesRead.Replace(" ", "")}",
                        $"{AppStringResources.TotalPages.Replace(" ", "")}",
                        $"{AppStringResources.ReadingStartDate.Replace(" ", "")}",
                        $"{AppStringResources.ReadingEndDate.Replace(" ", "")}",
                        $"{AppStringResources.BookLocationGuid.Replace(" ", "")}",
                        $"{AppStringResources.BookComments.Replace(" ", "")}",
                        $"{AppStringResources.BookCollectionGuid.Replace(" ", "")}",
                        $"{AppStringResources.BookGenreGuid.Replace(" ", "")}",
                        $"{AppStringResources.BookURL.Replace(" ", "")}",
                        $"{AppStringResources.BookRating.Replace(" ", "")}",
                        $"{AppStringResources.Favorite.Replace(" ", "")}",
                        $"{AppStringResources.BookImageBase64String}"
                    }
                };

                foreach (var book in bookList)
                {
                    var imageString = book.BookImageBase64String;
                    List<string> imagesChunks = new List<string>();
                    if (imageString != null)
                    {
                        while (imageString.Length > _excelCellLimit)
                        {
                            var imageChunk = imageString.Substring(0, _excelCellLimit);
                            imagesChunks.Add(imageChunk);
                            imageString = imageString.Replace(imageChunk, "");
                        }
                        imagesChunks.Add(imageString);
                    }

                    var stringItem = new List<String?>
                    {
                        book.BookGuid.ToString(),
                        book.BookTitle,
                        book.BookSeriesGuid.ToString(),
                        book.BookNumberInSeries.ToString(),
                        book.BookPublisher,
                        book.BookPublishYear,
                        book.BookIdentifier,
                        book.BookFormat,
                        book.BookLanguage,
                        book.BookPrice,
                        book.BookSummary,
                        book.BookPageRead.ToString(),
                        book.BookPageTotal.ToString(),
                        book.BookStartDate,
                        book.BookEndDate,
                        book.BookLocationGuid.ToString(),
                        book.BookComments,
                        book.BookCollectionGuid.ToString(),
                        book.BookGenreGuid.ToString(),
                        book.BookURL,
                        book.Rating.ToString(),
                        book.IsFavorite.ToString(),
                    };

                    foreach (var imageChunk in imagesChunks)
                    {
                        stringItem.Add(imageChunk);
                    }

                    stringItems.Add(stringItem);
                }

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                BooksOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{stringItems.Count}");
            }
            else
            {
                BooksOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }

        private async Task ReadBooksFromSpreadsheet()
        {
            var tableName = "Books";

            if (BooksChecked)
            {
                BooksOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                BooksOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            BookModel book = new BookModel()
                            {
                                BookGuid = ParseGuid(values[0]),
                                BookTitle = values[1],
                                BookSeriesGuid = ParseGuid(values[2]),
                                BookNumberInSeries = ParseInt(values[3]),
                                BookPublisher = values[4],
                                BookPublishYear = values[5],
                                BookIdentifier = values[6],
                                BookFormat = values[7],
                                BookLanguage = values[8],
                                BookPrice = values[9],
                                BookSummary = values[10],
                                BookPageRead = ParseInt(values[11]),
                                BookPageTotal = ParseInt(values[12]),
                                BookStartDate = values[13],
                                BookEndDate = values[14],
                                BookLocationGuid = ParseGuid(values[15]),
                                BookComments = values[16],
                                BookCollectionGuid = ParseGuid(values[17]),
                                BookGenreGuid = ParseGuid(values[18]),
                                BookURL = values[19],
                                Rating = ParseInt(values[20]),
                                IsFavorite = ParseBool(values[21]),
                            };

                            int index = 22;
                            var imagestringItems = string.Empty;

                            while (index < values.Count)
                            {
                                if (!string.IsNullOrEmpty(values[index].Trim()))
                                    imagestringItems += values[index].Trim();

                                index++;
                            }

                            book.BookImageBase64String = imagestringItems;

                            if (!string.IsNullOrEmpty(book.BookImageBase64String))
                            {
                                try
                                {
                                    book.BookCoverBytes = Convert.FromBase64String(book.BookImageBase64String);
                                }
                                catch (Exception ex)
                                {

                                }

                                book.SetCoverDisplay();
                            }

                            // Unit test data
                            TestData.InsertBook(book);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayMessage(AppStringResources.ErrorSavingBook, null);
                    }
                }

                BooksOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{valuesList.Count}");

                await Task.Yield();
            }
            else
            {
                BooksOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }
        #endregion

        #region WishListBook
        private async Task WriteWishListBooksToSpreadsheet()
        {
            var tableName = "WishListBooks";

            if (WishListChecked)
            {
                WishListOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                // Unit test data
                var bookList = await FilterLists.GetBookWishList(true);

                WishListOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{bookList.Count}");

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.BookGuid.Replace(" ", "")}",
                        $"{AppStringResources.BookTitle.Replace(" ", "")}",
                        $"{AppStringResources.BookAuthors.Replace(" ", "")}",
                        $"{AppStringResources.BookSeries.Replace(" ", "")}",
                        $"{AppStringResources.BookNumber.Replace(" ", "")}",
                        $"{AppStringResources.BookPublisher.Replace(" ", "")}",
                        $"{AppStringResources.BookPublishYear.Replace(" ", "")}",
                        $"{AppStringResources.BookIdentifier.Replace(" ", "")}",
                        $"{AppStringResources.BookFormat.Replace(" ", "")}",
                        $"{AppStringResources.BookLanguage.Replace(" ", "")}",
                        $"{AppStringResources.BookPrice.Replace(" ", "")}",
                        $"{AppStringResources.BookSummary.Replace(" ", "")}",
                        $"{AppStringResources.TotalPages.Replace(" ", "")}",
                        $"{AppStringResources.BookComments.Replace(" ", "")}",
                        $"{AppStringResources.WhereToBuy.Replace(" ", "")}",
                        $"{AppStringResources.BookURL.Replace(" ", "")}",
                        $"{AppStringResources.BookImageBase64String}",
                    }
                };

                foreach (var book in bookList)
                {
                    try
                    {
                        var imagestringItems = book.BookImageBase64String;
                        List<string> imagesChunks = new List<string>();

                        if (imagestringItems != null)
                        {
                            while (imagestringItems.Length > _excelCellLimit)
                            {
                                var imageChunk = imagestringItems.Substring(0, _excelCellLimit);
                                imagesChunks.Add(imageChunk);
                                imagestringItems = imagestringItems.Replace(imageChunk, "");
                            }
                            imagesChunks.Add(imagestringItems);
                        }

                        var stringItem = new List<String?>
                        {
                            book.BookGuid.ToString(),
                            book.BookTitle,
                            book.AuthorListString,
                            book.BookSeries,
                            $"{book.BookNumberInSeries}",
                            book.BookPublisher,
                            book.BookPublishYear,
                            book.BookIdentifier,
                            book.BookFormat,
                            book.BookLanguage,
                            book.BookPrice,
                            book.BookSummary,
                            book.BookPageTotal.ToString(),
                            book.BookComments,
                            book.BookWhereToBuy,
                            book.BookURL,
                        };

                        foreach (var imageChunk in imagesChunks)
                        {
                            stringItem.Add(imageChunk);
                        }

                        stringItems.Add(stringItem);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                WishListOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{stringItems.Count}");
            }
            else
            {
                WishListOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }

        private async Task ReadWishListBooksFromSpreadsheet()
        {
            var tableName = "WishListBooks";

            if (WishListChecked)
            {
                WishListOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                WishListOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            BookModel book = new BookModel()
                            {
                                BookGuid = ParseGuid(values[0]),
                                BookTitle = values[1],
                                BookAuthors = values[2],
                                BookSeries = values[3],
                                BookNumberInSeries = ParseInt(values[4]),
                                BookPublisher = values[5],
                                BookPublishYear = values[6],
                                BookIdentifier = values[7],
                                BookFormat = values[8],
                                BookLanguage = values[9],
                                BookPrice = values[10],
                                BookSummary = values[11],
                                BookPageTotal = ParseInt(values[12]),
                                BookComments = values[13],
                                BookWhereToBuy = values[14],
                                BookURL = values[15],
                            };

                            int index = 16;
                            var imagestringItems = string.Empty;

                            while (index < values.Count)
                            {
                                if (!string.IsNullOrEmpty(values[index].Trim()))
                                    imagestringItems += values[index].Trim();

                                index++;
                            }

                            book.BookImageBase64String = imagestringItems;

                            // Unit test data
                            TestData.InsertWishListBook(book);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayMessage(AppStringResources.ErrorSavingBook, null);
                    }
                }

                BooksOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Yield();
            }
            else
            {
                WishListOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }
        #endregion

        #region Chapter
        private async Task WriteChaptersToSpreadsheet()
        {
            var tableName = "Chapters";

            if (ChaptersChecked)
            {
                ChaptersOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                // Unit test data
                var chapterList = TestData.ChapterList.OrderBy(x => x.ChapterOrder).OrderBy(x => x.BookGuid).ToList();

                ChaptersOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{chapterList.Count}");

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.ChapterGuid.Replace(" ", "")}",
                        $"{AppStringResources.ChapterName.Replace(" ", "")}",
                        $"{AppStringResources.PageRange.Replace(" ", "")}",
                        $"{AppStringResources.ChapterOrder.Replace(" ", "")}",
                        $"{AppStringResources.BookGuid.Replace(" ", "")}"
                    }
                };

                foreach (var chapter in chapterList)
                {
                    var stringItem = new List<String?>
                    {
                        chapter.ChapterGuid.ToString(),
                        chapter.ChapterName,
                        chapter.PageRange,
                        chapter.ChapterOrder.ToString(),
                        chapter.BookGuid.ToString(),
                    };

                    stringItems.Add(stringItem);
                }

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                ChaptersOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{stringItems.Count}");
            }
            else
            {
                ChaptersOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }

        private async Task ReadChaptersFromSpreadsheet()
        {
            var tableName = "Chapters";

            if (ChaptersChecked)
            {
                ChaptersOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                ChaptersOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            ChapterModel chapter = new ChapterModel()
                            {
                                ChapterGuid = ParseGuid(values[0]),
                                ChapterName = values[1],
                                PageRange = values[2],
                                ChapterOrder = ParseInt(values[3]),
                                BookGuid = (Guid)ParseGuid(values[4]),
                            };

                            // Unit test data
                            TestData.InsertChapter(chapter);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayMessage(AppStringResources.ErrorSavingChapter, null);
                    }
                }

                ChaptersOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Yield();
            }
            else
            {
                ChaptersOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }
        #endregion

        #region Collection
        private async Task WriteCollectionsToSpreadsheet()
        {
            var tableName = "Collections";

            if (CollectionsChecked)
            {
                CollectionsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                // Unit test data
                var collectionList = await FilterLists.GetAllCollectionsList(true);

                CollectionsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{collectionList.Count}");

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.CollectionGuid.Replace(" ", "")}",
                        $"{AppStringResources.CollectionName.Replace(" ", "")}"
                    }
                };

                foreach (var collection in collectionList)
                {
                    var stringItem = new List<String?>
                    {
                        collection.CollectionGuid.ToString(),
                        collection.CollectionName
                    };

                    stringItems.Add(stringItem);
                }

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                CollectionsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{stringItems.Count}");
            }
            else
            {
                CollectionsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }

        private async Task ReadCollectionsFromSpreadsheet()
        {
            var tableName = "Collections";

            if (CollectionsChecked)
            {
                CollectionsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                CollectionsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            CollectionModel collection = new CollectionModel()
                            {
                                CollectionGuid = ParseGuid(values[0]),
                                CollectionName = values[1],
                            };

                            // Unit test data
                            TestData.InsertCollection(collection);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayMessage(AppStringResources.ErrorSavingCollection, null);
                    }
                }

                CollectionsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Yield();
            }
            else
            {
                CollectionsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }
        #endregion

        #region Genre
        private async Task WriteGenresToSpreadsheet()
        {
            var tableName = "Genres";

            if (GenresChecked)
            {
                GenresOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                var genreList = await FilterLists.GetAllGenresList(true);

                GenresOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{genreList.Count}");

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.GenreGuid.Replace(" ", "")}",
                        $"{AppStringResources.GenreName.Replace(" ", "")}"
                    }
                };

                foreach (var genre in genreList)
                {
                    var stringItem = new List<String?>
                    {
                        genre.GenreGuid.ToString(),
                        genre.GenreName
                    };

                    stringItems.Add(stringItem);
                }

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                GenresOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{genreList.Count}");
            }
            else
            {
                GenresOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }

        private async Task ReadGenresFromSpreadsheet()
        {
            var tableName = "Genres";

            if (GenresChecked)
            {
                GenresOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                GenresOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            GenreModel genre = new GenreModel()
                            {
                                GenreGuid = ParseGuid(values[0]),
                                GenreName = values[1],
                            };

                            // Unit test data
                            TestData.InsertGenre(genre);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayMessage(AppStringResources.ErrorSavingGenre, null);
                    }
                }

                GenresOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Yield();
            }
            else
            {
                GenresOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }
        #endregion

        #region Series
        private async Task WriteSeriesToSpreadsheet()
        {
            var tableName = "Series";

            if (SeriesChecked)
            {
                SeriesOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                // Unit test data
                var seriesList = await FilterLists.GetAllSeriesList(true);

                SeriesOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{seriesList.Count}");

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.SeriesGuid.Replace(" ", "")}",
                        $"{AppStringResources.SeriesName.Replace(" ", "")}",
                        $"{AppStringResources.TotalBooksInSeries.Replace(" ", "")}"
                    }
                };

                foreach (var series in seriesList)
                {
                    var stringItem = new List<String?>
                    {
                        series.SeriesGuid.ToString(),
                        series.SeriesName,
                        series.TotalBooksInSeries
                    };

                    stringItems.Add(stringItem);
                }

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                SeriesOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{seriesList.Count}");
            }
            else
            {
                SeriesOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }

        private async Task ReadSeriesFromSpreadsheet()
        {
            var tableName = "Series";

            if (SeriesChecked)
            {
                SeriesOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                SeriesOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            SeriesModel series = new SeriesModel()
                            {
                                SeriesGuid = ParseGuid(values[0]),
                                SeriesName = values[1],
                                TotalBooksInSeries = values[2]
                            };

                            // Unit test data
                            TestData.InsertSeries(series);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayMessage(AppStringResources.ErrorSavingSeries, null);
                    }
                }

                SeriesOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Yield();
            }
            else
            {
                SeriesOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }
        #endregion

        #region BookAuthor
        private async Task WriteBookAuthorsToSpreadsheet()
        {
            var tableName = "BookAuthors";

            if (BookAuthorsChecked)
            {
                BookAuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                var bookAuthorList = TestData.BookAuthorList.OrderBy(x => x.BookGuid).ToList();

                BookAuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{bookAuthorList.Count}");

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.BookAuthorGuid.Replace(" ", "")}",
                        $"{AppStringResources.AuthorGuid.Replace(" ", "")}",
                        $"{AppStringResources.BookGuid.Replace(" ", "")}"
                    }
                };

                foreach (var bookAuthor in bookAuthorList)
                {
                    var stringItem = new List<String?>
                    {
                        bookAuthor.BookAuthorGuid.ToString(),
                        bookAuthor.AuthorGuid.ToString(),
                        bookAuthor.BookGuid.ToString()
                    };

                    stringItems.Add(stringItem);
                }

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                BookAuthorsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{bookAuthorList.Count}");
            }
            else
            {
                BookAuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }

        private async Task ReadBookAuthorsFromSpreadsheet()
        {
            var tableName = "BookAuthors";

            if (BookAuthorsChecked)
            {
                BookAuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                BookAuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            BookAuthorModel bookAuthor = new BookAuthorModel()
                            {
                                BookAuthorGuid = ParseGuid(values[0]),
                                AuthorGuid = (Guid)ParseGuid(values[1]),
                                BookGuid = (Guid)ParseGuid(values[2])
                            };

                            // Unit test data
                            TestData.BookAuthorList.Add(bookAuthor);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayMessage(AppStringResources.ErrorSavingBookAuthor, null);
                    }
                }

                BookAuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Yield();
            }
            else
            {
                BookAuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }
        #endregion

        #region Author
        private async Task WriteAuthorsToSpreadsheet()
        {
            var tableName = "Authors";

            if (AuthorsChecked)
            {
                AuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                var authorList = await FilterLists.GetAllAuthorsList(true);

                AuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{authorList.Count}");

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.AuthorGuid.Replace(" ", "")}",
                        $"{AppStringResources.FirstName.Replace(" ", "")}",
                        $"{AppStringResources.LastName.Replace(" ", "")}",
                        $"{AppStringResources.FullName.Replace(" ", "")}"
                    }
                };

                foreach (var author in authorList)
                {
                    var stringItem = new List<String?>
                    {
                        author.AuthorGuid.ToString(),
                        author.FirstName,
                        author.LastName,
                        author.FullName
                    };

                    stringItems.Add(stringItem);
                }

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                AuthorsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{authorList.Count}");
            }
            else
            {
                AuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }

        private async Task ReadAuthorsFromSpreadsheet()
        {
            var tableName = "Authors";

            if (AuthorsChecked)
            {
                AuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                AuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            AuthorModel author = new AuthorModel()
                            {
                                AuthorGuid = ParseGuid(values[0]),
                                FirstName = values[1],
                                LastName = values[2],
                            };

                            // Unit test data
                            TestData.InsertAuthor(author);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayMessage(AppStringResources.ErrorSavingAuthor, null);
                    }
                }

                AuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Yield();
            }
            else
            {
                AuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }
        #endregion

        #region Location
        private async Task WriteLocationsToSpreadsheet()
        {
            var tableName = "Locations";

            if (LocationsChecked)
            {
                LocationsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                var locationList = await FilterLists.GetAllLocationsList(true);

                LocationsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{locationList.Count}");

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.AuthorGuid.Replace(" ", "")}",
                        $"{AppStringResources.FirstName.Replace(" ", "")}",
                        $"{AppStringResources.LastName.Replace(" ", "")}",
                        $"{AppStringResources.FullName.Replace(" ", "")}"
                    }
                };

                foreach (var location in locationList)
                {
                    var stringItem = new List<String?>
                    {
                        location.LocationGuid.ToString(),
                        location.LocationName
                    };

                    stringItems.Add(stringItem);
                }

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                LocationsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{locationList.Count}");
            }
            else
            {
                LocationsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }

        private async Task ReadLocationsFromSpreadsheet()
        {
            var tableName = "Locations";

            if (LocationsChecked)
            {
                LocationsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                LocationsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            LocationModel location = new LocationModel()
                            {
                                LocationGuid = ParseGuid(values[0]),
                                LocationName = values[1],
                            };

                            // Unit test data
                            TestData.InsertLocation(location);
                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayMessage(AppStringResources.ErrorSavingLocation, null);
                    }
                }

                LocationsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Yield();
            }
            else
            {
                LocationsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);
            }
        }
        #endregion
        #endregion

        private Guid? ParseGuid(string input)
        {
            Guid? guid;
            Guid guidParse;
            Guid.TryParse(input, out guidParse);

            if (guidParse == Guid.Empty)
                guid = null;
            else
                guid = guidParse;

            return guid;
        }

        private int ParseInt(string input)
        {
            int intParse;

            int.TryParse(input, out intParse);

            if (intParse == 0)
            {
                double doubleParse = ParseDouble(input);

            }
            return intParse;
        }

        private double ParseDouble(string input)
        {
            double doubleParse;

            if (input.StartsWith('$'))
                input = input.Substring(1);

            double.TryParse(input, out doubleParse);

            return doubleParse;
        }

        private bool ParseBool(string input)
        {
            bool boolParse;

            bool.TryParse(input, out boolParse);

            return boolParse;
        }
    }
}
