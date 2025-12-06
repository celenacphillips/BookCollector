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
using DocumentFormat.OpenXml.Vml.Presentation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Maui.Graphics.Color;

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

        [ObservableProperty]
        public bool refreshEnabled;

        private string _filePath = string.Empty;
        private string _imageExportLocation = string.Empty;
        private Color BusyColor = (Color)Application.Current.Resources["Primary"];

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
            RefreshEnabled = IsBusy;
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
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }

        [RelayCommand]
        public async Task Export()
        {
            var action = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureExport_Question, null, null);

            if (action)
            {
                try
                {
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

                    _imageExportLocation = $"{exportLocation}/BookCovers";

                    if (!Directory.Exists(_imageExportLocation))
                        Directory.CreateDirectory(_imageExportLocation);

                    await Task.Delay(1);

                    SetIsBusyTrue();

                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;
                    RefreshEnabled = !IsBusy;

                    ResetOutput();
                    OutputVisible = IsBusy;
                    CheckboxesVisible = !IsBusy;

                    var filePath = ReadWriteSpreadsheet.CreateSpreadsheet(exportLocation);
                    _filePath = filePath;

                    StartOutput = AppStringResources.ExportResultsStart;

                    SetOutputWaiting();

                    await Task.Delay(1);

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
                    RefreshEnabled = !IsBusy;
                }
                catch (Exception ex)
                {
                    await CanceledAction();
                    SetIsBusyFalse();
                    ImportEnabled = !IsBusy;
                    ExportEnabled = !IsBusy;
                    RefreshEnabled = !IsBusy;
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
                        await Task.Delay(1);
                        SetIsBusyTrue();

                        _filePath = result.FullPath;

                        ImportEnabled = !IsBusy;
                        ExportEnabled = !IsBusy;
                        RefreshEnabled = !IsBusy;

                        ResetOutput();
                        OutputVisible = IsBusy;
                        CheckboxesVisible = !IsBusy;

                        StartOutput = AppStringResources.ImportResultsStart;

                        SetOutputWaiting();

                        await Task.Delay(1);

                        await ReadWishListBooksFromSpreadsheet();
                        await ReadAuthorsFromSpreadsheet();
                        await ReadChaptersFromSpreadsheet();
                        await ReadCollectionsFromSpreadsheet();
                        await ReadGenresFromSpreadsheet();
                        await ReadSeriesFromSpreadsheet();
                        await ReadLocationsFromSpreadsheet();
                        await ReadBooksFromSpreadsheet();
                        await ReadBookAuthorsFromSpreadsheet();

                        if (TestData.UseTestData)
                        {
                            await TestData.DataCleanup();
                        }
                        else
                        {

                        }

                        FinalOutput = AppStringResources.ImportResultsFinish;

                        await DisplayMessage(AppStringResources.ImportComplete, null);

                        SetIsBusyFalse();
                        RefreshEnabled = !IsBusy;
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
                    RefreshEnabled = !IsBusy;
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
            StartOutput = string.Empty;
        }

        private void SetOutputWaiting()
        {
            BooksOutput = AppStringResources.Table_Waiting.Replace("Table", "Books");
            WishListOutput = AppStringResources.Table_Waiting.Replace("Table", "WishListBooks");
            CollectionsOutput = AppStringResources.Table_Waiting.Replace("Table", "Collections");
            GenresOutput = AppStringResources.Table_Waiting.Replace("Table", "Genres");
            SeriesOutput = AppStringResources.Table_Waiting.Replace("Table", "Series");
            AuthorsOutput = AppStringResources.Table_Waiting.Replace("Table", "Authors");
            LocationsOutput = AppStringResources.Table_Waiting.Replace("Table", "Locations");
            ChaptersOutput = AppStringResources.Table_Waiting.Replace("Table", "Chapters");
            BookAuthorsOutput = AppStringResources.Table_Waiting.Replace("Table", "BookAuthors");
        }

        #region Tables
        #region Book
        private async Task WriteBooksToSpreadsheet()
        {
            var tableName = "Books";
            var label = (Label)_view.FindByName("booksOutput");

            if (BooksChecked)
            {
                label.TextColor = BusyColor;
                BooksOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var bookList = await FilterLists.GetAllBooksList(true);

                BooksOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{bookList.Count}");

                await Task.Delay(1);

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
                        $"{AppStringResources.BookLoanedTo.Replace(" ", "")}",
                        $"{AppStringResources.BookLoanedOutOn.Replace(" ", "")}",
                        $"{AppStringResources.BookCoverUrl.Replace(" ", "")}",
                        $"{AppStringResources.BookImportExportFileLocation.Replace(" ", "")}"
                    }
                };

                foreach (var book in bookList)
                {
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
                        book.LoanedTo,
                        book.BookLoanedOutOn,
                        book.BookCoverUrl,
                    };

                    if (!string.IsNullOrEmpty(book.BookCoverFileLocation))
                    {
                        var fi = new FileInfo(book.BookCoverFileLocation);
                        var exportLocation = $"{_imageExportLocation}/{fi.Name}";
                        File.Copy(book.BookCoverFileLocation, exportLocation, true);
                        stringItem.Add(exportLocation);
                    }
                    else
                        stringItem.Add("");

                    stringItems.Add(stringItem);
                }

                var exportCount = bookList.Count;

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                BooksOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{bookList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                BooksOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadBooksFromSpreadsheet()
        {
            var tableName = "Books";
            var label = (Label)_view.FindByName("booksOutput");

            if (BooksChecked)
            {
                label.TextColor = BusyColor;
                BooksOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                var importCount = valuesList.Count;

                BooksOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

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
                                LoanedTo = values[22],
                                BookLoanedOutOn = values[23],
                                BookCoverUrl = values[24],
                                BookCoverFileLocation = values[25],
                            };

                            if (!string.IsNullOrEmpty(book.BookCoverUrl))
                            {
                                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                                {
                                    try
                                    {
                                        var byteArray = new WebClient().DownloadData($"{book.BookCoverUrl}");
                                        book.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                else
                                {
                                    await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                                }
                            }

                            if (!string.IsNullOrEmpty(book.BookCoverFileLocation))
                            {
                                try
                                {
                                    var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                                    if (!Directory.Exists(directory))
                                        Directory.CreateDirectory(directory);

                                    var fi = new FileInfo(book.BookCoverFileLocation);
                                    var filePath = $"{directory}/{fi.Name}";
                                    File.Copy(book.BookCoverFileLocation, filePath, true);

                                    if (File.Exists(filePath))
                                    {
                                        book.BookCoverFileLocation = filePath;
                                        book.BookCover = ImageSource.FromFile(filePath);
                                    }
                                    else
                                    {
                                        book.BookCoverFileLocation = null;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    book.BookCoverFileLocation = null;
                                }
                            }

                            if (TestData.UseTestData)
                            {
                                TestData.UpdateBook(book);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingBook, null);
                    }
                }

                BooksOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                BooksOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }
        #endregion

        #region WishListBook
        private async Task WriteWishListBooksToSpreadsheet()
        {
            var tableName = "WishListBooks";
            var label = (Label)_view.FindByName("wishListOutput");

            if (WishListChecked)
            {
                label.TextColor = BusyColor;
                WishListOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var bookList = await FilterLists.GetBookWishList(true);

                WishListOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{bookList.Count}");

                await Task.Delay(1);

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
                        $"{AppStringResources.BookCoverUrl.Replace(" ", "")}",
                        $"{AppStringResources.BookImportExportFileLocation.Replace(" ", "")}",
                    }
                };

                foreach (var book in bookList)
                {
                    try
                    {

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
                            book.BookCoverUrl,
                        };

                        if (!string.IsNullOrEmpty(book.BookCoverFileLocation))
                        {
                            var fi = new FileInfo(book.BookCoverFileLocation);
                            var exportLocation = $"{_imageExportLocation}/{fi.Name}";
                            File.Copy(book.BookCoverFileLocation, exportLocation, true);
                            stringItem.Add(exportLocation);
                        }
                        else
                            stringItem.Add("");

                        stringItems.Add(stringItem);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                var exportCount = bookList.Count;

                try
                {
                    ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);
                }
                catch (Exception ex)
                {

                }

                WishListOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{bookList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                WishListOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadWishListBooksFromSpreadsheet()
        {
            var tableName = "WishListBooks";
            var label = (Label)_view.FindByName("wishListOutput");

            if (WishListChecked)
            {
                label.TextColor = BusyColor;
                WishListOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                var importCount = valuesList.Count;

                WishListOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

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
                                AuthorListString = values[2],
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
                                BookCoverUrl = values[16],
                                BookCoverFileLocation = values[17],
                            };

                            if (!string.IsNullOrEmpty(book.BookCoverUrl))
                            {
                                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                                {
                                    try
                                    {
                                        var byteArray = new WebClient().DownloadData($"{book.BookCoverUrl}");
                                        book.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                else
                                {
                                    await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                                }
                            }

                            if (!string.IsNullOrEmpty(book.BookCoverFileLocation))
                            {
                                try
                                {
                                    var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                                    if (!Directory.Exists(directory))
                                        Directory.CreateDirectory(directory);

                                    var fi = new FileInfo(book.BookCoverFileLocation);
                                    var filePath = $"{directory}/{fi.Name}";
                                    File.Copy(book.BookCoverFileLocation, filePath, true);

                                    if (File.Exists(filePath))
                                    {
                                        book.BookCoverFileLocation = filePath;
                                        book.BookCover = ImageSource.FromFile(filePath);
                                    }
                                    else
                                    {
                                        book.BookCoverFileLocation = null;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    book.BookCoverFileLocation = null;
                                }
                            }

                            if (TestData.UseTestData)
                            {
                                TestData.UpdateWishListBook(book);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingBook, null);
                    }
                }

                WishListOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                WishListOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }
        #endregion

        #region Chapter
        private async Task WriteChaptersToSpreadsheet()
        {
            var tableName = "Chapters";
            var label = (Label)_view.FindByName("chaptersOutput");

            if (ChaptersChecked)
            {
                label.TextColor = BusyColor;
                ChaptersOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var chapterList = await FilterLists.GetAllChapters();

                ChaptersOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{chapterList.Count}");

                await Task.Delay(1);

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

                var exportCount = chapterList.Count;

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                ChaptersOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{chapterList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                ChaptersOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadChaptersFromSpreadsheet()
        {
            var tableName = "Chapters";
            var label = (Label)_view.FindByName("chaptersOutput");

            if (ChaptersChecked)
            {
                label.TextColor = BusyColor;
                ChaptersOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                var importCount = valuesList.Count;

                ChaptersOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

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

                            if (TestData.UseTestData)
                            {
                                TestData.UpdateChapter(chapter);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingChapter, null);
                    }
                }

                ChaptersOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                ChaptersOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }
        #endregion

        #region Collection
        private async Task WriteCollectionsToSpreadsheet()
        {
            var tableName = "Collections";
            var label = (Label)_view.FindByName("collectionsOutput");

            if (CollectionsChecked)
            {
                label.TextColor = BusyColor;
                CollectionsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                
                var collectionList = await FilterLists.GetAllCollectionsList(true);

                CollectionsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{collectionList.Count}");

                await Task.Delay(1);

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

                var exportCount = collectionList.Count;

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                CollectionsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{collectionList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                CollectionsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadCollectionsFromSpreadsheet()
        {
            var tableName = "Collections";
            var label = (Label)_view.FindByName("collectionsOutput");

            if (CollectionsChecked)
            {
                label.TextColor = BusyColor;
                CollectionsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                var importCount = valuesList.Count;

                CollectionsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

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

                            if (TestData.UseTestData)
                            {
                                TestData.UpdateCollection(collection);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingCollection, null);
                    }
                }

                CollectionsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                CollectionsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }
        #endregion

        #region Genre
        private async Task WriteGenresToSpreadsheet()
        {
            var tableName = "Genres";
            var label = (Label)_view.FindByName("genresOutput");

            if (GenresChecked)
            {
                label.TextColor = BusyColor;
                GenresOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var genreList = await FilterLists.GetAllGenresList(true);

                GenresOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{genreList.Count}");

                await Task.Delay(1);

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

                var exportCount = genreList.Count;

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                GenresOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{genreList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                GenresOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadGenresFromSpreadsheet()
        {
            var tableName = "Genres";
            var label = (Label)_view.FindByName("genresOutput");

            if (GenresChecked)
            {
                label.TextColor = BusyColor;
                GenresOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                var importCount = valuesList.Count;

                GenresOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

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

                            if (TestData.UseTestData)
                            {
                                TestData.UpdateGenre(genre);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingGenre, null);
                    }
                }

                GenresOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                GenresOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }
        #endregion

        #region Series
        private async Task WriteSeriesToSpreadsheet()
        {
            var tableName = "Series";
            var label = (Label)_view.FindByName("seriesOutput");

            if (SeriesChecked)
            {
                label.TextColor = BusyColor;
                SeriesOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var seriesList = await FilterLists.GetAllSeriesList(true);

                SeriesOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{seriesList.Count}");

                await Task.Delay(1);

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

                var exportCount = seriesList.Count;

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                SeriesOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{seriesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                SeriesOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadSeriesFromSpreadsheet()
        {
            var tableName = "Series";
            var label = (Label)_view.FindByName("seriesOutput");

            if (SeriesChecked)
            {
                label.TextColor = BusyColor;
                SeriesOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                var importCount = valuesList.Count;

                SeriesOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

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

                            if (TestData.UseTestData)
                            {
                                TestData.UpdateSeries(series);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingSeries, null);
                    }
                }

                SeriesOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                SeriesOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }
        #endregion

        #region BookAuthor
        private async Task WriteBookAuthorsToSpreadsheet()
        {
            var tableName = "BookAuthors";
            var label = (Label)_view.FindByName("bookAuthorsOutput");

            if (BookAuthorsChecked)
            {
                label.TextColor = BusyColor;
                BookAuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var bookAuthorList = await FilterLists.GetAllBookAuthors();

                BookAuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{bookAuthorList.Count}");

                await Task.Delay(1);

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

                var exportCount = bookAuthorList.Count;

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                BookAuthorsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{bookAuthorList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                BookAuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadBookAuthorsFromSpreadsheet()
        {
            var tableName = "BookAuthors";
            var label = (Label)_view.FindByName("bookAuthorsOutput");

            if (BookAuthorsChecked)
            {
                label.TextColor = BusyColor;
                BookAuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                var importCount = valuesList.Count;

                BookAuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

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

                            if (TestData.UseTestData)
                            {
                                TestData.UpdateBookAuthor(bookAuthor);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingBookAuthor, null);
                    }
                }

                BookAuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                BookAuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }
        #endregion

        #region Author
        private async Task WriteAuthorsToSpreadsheet()
        {
            var tableName = "Authors";
            var label = (Label)_view.FindByName("authorsOutput");

            if (AuthorsChecked)
            {
                label.TextColor = BusyColor;
                AuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var authorList = await FilterLists.GetAllAuthorsList(true);

                AuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{authorList.Count}");

                await Task.Delay(1);

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

                var exportCount = authorList.Count;

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                AuthorsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{authorList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                AuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadAuthorsFromSpreadsheet()
        {
            var tableName = "Authors";
            var label = (Label)_view.FindByName("authorsOutput");

            if (AuthorsChecked)
            {
                label.TextColor = BusyColor;
                AuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                var importCount = valuesList.Count;

                AuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

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

                            if (TestData.UseTestData)
                            {
                                TestData.UpdateAuthor(author);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingAuthor, null);
                    }
                }

                AuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                AuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }
        #endregion

        #region Location
        private async Task WriteLocationsToSpreadsheet()
        {
            var tableName = "Locations";
            var label = (Label)_view.FindByName("locationsOutput");

            if (LocationsChecked)
            {
                label.TextColor = BusyColor;
                LocationsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var locationList = await FilterLists.GetAllLocationsList(true);

                LocationsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{locationList.Count}");

                await Task.Delay(1);

                var stringItems = new List<List<String?>>
                {
                    new List<string?>()
                    {
                        $"{AppStringResources.LocationGuid.Replace(" ", "")}",
                        $"{AppStringResources.LocationName.Replace(" ", "")}",
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

                var exportCount = locationList.Count;

                ReadWriteSpreadsheet.WriteToSpreadsheet(_filePath, stringItems, tableName);

                LocationsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{locationList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                LocationsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadLocationsFromSpreadsheet()
        {
            var tableName = "Locations";
            var label = (Label)_view.FindByName("locationsOutput");

            if (LocationsChecked)
            {
                label.TextColor = BusyColor;
                LocationsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(_filePath, tableName);

                var importCount = valuesList.Count;

                LocationsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

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

                            if (TestData.UseTestData)
                            {
                                TestData.UpdateLocation(location);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingLocation, null);
                    }
                }

                LocationsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)Application.Current.Resources["TextDark"] : (Color)Application.Current.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                LocationsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
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
