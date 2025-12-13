using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Data.Spreadsheet;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Color = Microsoft.Maui.Graphics.Color;

namespace BookCollector.ViewModels.Main
{
    public partial class ExportImportViewModel : BaseViewModel
    {
        [ObservableProperty]
        public string? startOutput;

        [ObservableProperty]
        public string? booksOutput;

        [ObservableProperty]
        public string? wishListOutput;

        [ObservableProperty]
        public string? collectionsOutput;

        [ObservableProperty]
        public string? genresOutput;

        [ObservableProperty]
        public string? seriesOutput;

        [ObservableProperty]
        public string? authorsOutput;

        [ObservableProperty]
        public string? locationsOutput;

        [ObservableProperty]
        public string? chaptersOutput;

        [ObservableProperty]
        public string? bookAuthorsOutput;

        [ObservableProperty]
        public string? finalOutput;

        /********************************************************/

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
        public bool imagesChecked;

        /********************************************************/

        [ObservableProperty]
        public bool checkboxesVisible;

        [ObservableProperty]
        public bool outputVisible;

        /********************************************************/

        [ObservableProperty]
        public bool exportEnabled;

        [ObservableProperty]
        public bool importEnabled;

        [ObservableProperty]
        public bool refreshEnabled;

        private readonly Color? busyColor = (Color?)Application.Current?.Resources["Primary"];

        private string mainFilePath = string.Empty;

        private string imageExportLocation = string.Empty;

        public ExportImportViewModel(ContentPage view)
        {
            this.View = view;
            this.InfoText = AppStringResources.ExportImportView_InfoText;
        }

        public async Task SetViewModelData()
        {
            this.SetIsBusyTrue();

            this.ExportEnabled = this.IsBusy;
            this.ImportEnabled = this.IsBusy;
            this.RefreshEnabled = this.IsBusy;
            this.CheckboxesVisible = this.IsBusy;
            this.OutputVisible = !this.IsBusy;
            this.BooksChecked = true;
            this.ChaptersChecked = true;
            this.BookAuthorsChecked = true;
            this.WishListChecked = true;
            this.CollectionsChecked = true;
            this.GenresChecked = true;
            this.SeriesChecked = true;
            this.AuthorsChecked = true;
            this.LocationsChecked = true;
            this.ImagesChecked = true;

            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task Export()
        {
            var action = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureExport_Question, null, null);

            if (action)
            {
                try
                {
                    var exportLocation = Preferences.Get("ExportLocation", AppStringResources.DefaultExportLocation /* Default */);

                    if (exportLocation == null || exportLocation.Equals(AppStringResources.DefaultExportLocation))
                    {
                        var result = await FolderPicker.PickAsync(CancellationToken.None);

                        if (result != null && result.Folder != null)
                        {
                            exportLocation = result.Folder.Path;
                            Preferences.Set("ExportLocation", result.Folder.Path);
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }

                    if (!Directory.Exists(exportLocation))
                    {
                        Directory.CreateDirectory(exportLocation);
                    }

                    if (this.ImagesChecked)
                    {
                        this.imageExportLocation = $"{exportLocation}/BookCovers";

                        if (!Directory.Exists(this.imageExportLocation))
                        {
                            Directory.CreateDirectory(this.imageExportLocation);
                        }
                    }

                    await Task.Delay(1);

                    this.SetIsBusyTrue();

                    this.ImportEnabled = !this.IsBusy;
                    this.ExportEnabled = !this.IsBusy;
                    this.RefreshEnabled = !this.IsBusy;

                    this.ResetOutput();
                    this.OutputVisible = this.IsBusy;
                    this.CheckboxesVisible = !this.IsBusy;

                    var filePath = ReadWriteSpreadsheet.CreateSpreadsheet(exportLocation);
                    this.mainFilePath = filePath;

                    this.StartOutput = AppStringResources.ExportResultsStart;

                    this.SetOutputWaiting();

                    await Task.Delay(1);

                    await this.WriteWishListBooksToSpreadsheet();
                    await this.WriteBooksToSpreadsheet();
                    await this.WriteChaptersToSpreadsheet();
                    await this.WriteCollectionsToSpreadsheet();
                    await this.WriteGenresToSpreadsheet();
                    await this.WriteSeriesToSpreadsheet();
                    await this.WriteLocationsToSpreadsheet();
                    await this.WriteBookAuthorsToSpreadsheet();
                    await this.WriteAuthorsToSpreadsheet();

                    this.FinalOutput = AppStringResources.ExportResultsFinish;

                    await DisplayMessage(AppStringResources.ExportComplete, null);

                    this.SetIsBusyFalse();
                    this.RefreshEnabled = !this.IsBusy;
                }
                catch (Exception)
                {
                    await CanceledAction();
                    this.SetIsBusyFalse();
                    this.ImportEnabled = !this.IsBusy;
                    this.ExportEnabled = !this.IsBusy;
                    this.RefreshEnabled = !this.IsBusy;
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

                    PickOptions pickerOptions = new ()
                    {
                        FileTypes = customFileType,
                    };

                    var result = await FilePicker.Default.PickAsync(pickerOptions);

                    if (result != null)
                    {
                        await Task.Delay(1);
                        this.SetIsBusyTrue();

                        this.mainFilePath = result.FullPath;

                        this.ImportEnabled = !this.IsBusy;
                        this.ExportEnabled = !this.IsBusy;
                        this.RefreshEnabled = !this.IsBusy;

                        this.ResetOutput();
                        this.OutputVisible = this.IsBusy;
                        this.CheckboxesVisible = !this.IsBusy;

                        this.StartOutput = AppStringResources.ImportResultsStart;

                        this.SetOutputWaiting();

                        await Task.Delay(1);

                        await this.ReadWishListBooksFromSpreadsheet();
                        await this.ReadAuthorsFromSpreadsheet();
                        await this.ReadChaptersFromSpreadsheet();
                        await this.ReadCollectionsFromSpreadsheet();
                        await this.ReadGenresFromSpreadsheet();
                        await this.ReadSeriesFromSpreadsheet();
                        await this.ReadLocationsFromSpreadsheet();
                        await this.ReadBooksFromSpreadsheet();
                        await this.ReadBookAuthorsFromSpreadsheet();

                        if (TestData.UseTestData)
                        {
                            TestData.DataCleanup();
                        }
                        else
                        {
                        }

                        this.FinalOutput = AppStringResources.ImportResultsFinish;

                        await DisplayMessage(AppStringResources.ImportComplete, null);

                        this.SetIsBusyFalse();
                        this.RefreshEnabled = !this.IsBusy;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (Exception)
                {
                    await CanceledAction();
                    this.SetIsBusyFalse();
                    this.ImportEnabled = !this.IsBusy;
                    this.ExportEnabled = !this.IsBusy;
                    this.RefreshEnabled = !this.IsBusy;
                }
            }
            else
            {
                await CanceledAction();
            }
        }

        private static Guid? ParseGuid(string input)
        {
            Guid? guid;
            var parsed = Guid.TryParse(input, out Guid guidParse);

            if (!parsed || guidParse == Guid.Empty)
            {
                guid = null;
            }
            else
            {
                guid = guidParse;
            }

            return guid;
        }

        private static int ParseInt(string input)
        {
            var parsed = int.TryParse(input, out int intParse);

            if (!parsed || intParse == 0)
            {
                double doubleParse = ParseDouble(input);
                intParse = (int)doubleParse;
            }

            return intParse;
        }

        private static double ParseDouble(string input)
        {
            if (input.StartsWith('$'))
            {
                input = input[1..];
            }

            var parsed = double.TryParse(input, out double doubleParse);

            if (!parsed)
            {
                return 0;
            }

            return doubleParse;
        }

        private static bool ParseBool(string input)
        {
            var parsed = bool.TryParse(input, out bool boolParse);

            if (!parsed)
            {
                return false;
            }

            return boolParse;
        }

        private void ResetOutput()
        {
            this.BooksOutput = string.Empty;
            this.WishListOutput = string.Empty;
            this.CollectionsOutput = string.Empty;
            this.GenresOutput = string.Empty;
            this.SeriesOutput = string.Empty;
            this.AuthorsOutput = string.Empty;
            this.LocationsOutput = string.Empty;
            this.ChaptersOutput = string.Empty;
            this.BookAuthorsOutput = string.Empty;
            this.FinalOutput = string.Empty;
            this.StartOutput = string.Empty;
        }

        private void SetOutputWaiting()
        {
            this.BooksOutput = AppStringResources.Table_Waiting.Replace("Table", "Books");
            this.WishListOutput = AppStringResources.Table_Waiting.Replace("Table", "WishListBooks");
            this.CollectionsOutput = AppStringResources.Table_Waiting.Replace("Table", "Collections");
            this.GenresOutput = AppStringResources.Table_Waiting.Replace("Table", "Genres");
            this.SeriesOutput = AppStringResources.Table_Waiting.Replace("Table", "Series");
            this.AuthorsOutput = AppStringResources.Table_Waiting.Replace("Table", "Authors");
            this.LocationsOutput = AppStringResources.Table_Waiting.Replace("Table", "Locations");
            this.ChaptersOutput = AppStringResources.Table_Waiting.Replace("Table", "Chapters");
            this.BookAuthorsOutput = AppStringResources.Table_Waiting.Replace("Table", "BookAuthors");
        }

        /*********************** Table Methods ***********************/

        /*********************** Book Methods ***********************/
        private async Task WriteBooksToSpreadsheet()
        {
            var tableName = "Books";
            var label = (Label)this.View.FindByName("booksOutput");

            if (this.BooksChecked)
            {
                label.TextColor = this.busyColor;
                this.BooksOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var bookList = await FilterLists.GetAllBooksList(true);
                var count = bookList != null ? bookList.Count : 0;

                this.BooksOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    new ()
                    {
                        $"{AppStringResources.BookGuid_Blank.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookTitle.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookSeriesGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookNumber.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookPublisher.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookPublishYear.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookIdentifier.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookFormat.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookLanguage.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookPrice.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookSummary.Replace(" ", string.Empty)}",
                        $"{AppStringResources.PagesRead.Replace(" ", string.Empty)}",
                        $"{AppStringResources.TotalPages.Replace(" ", string.Empty)}",
                        $"{AppStringResources.ReadingStartDate.Replace(" ", string.Empty)}",
                        $"{AppStringResources.ReadingEndDate.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookLocationGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookComments.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookCollectionGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookGenreGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookURL.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookRating.Replace(" ", string.Empty)}",
                        $"{AppStringResources.Favorite.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookLoanedTo.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookLoanedOutOn.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookCoverUrl.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookImportExportFileLocation.Replace(" ", string.Empty)}",
                    },
                };

                if (bookList != null)
                {
                    foreach (var book in bookList)
                    {
                        var stringItem = new List<string?>
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

                        if (this.ImagesChecked && !string.IsNullOrEmpty(book.BookCoverFileLocation))
                        {
                            var fi = new FileInfo(book.BookCoverFileLocation);
                            var exportLocation = $"{this.imageExportLocation}/{fi.Name}";
                            File.Copy(book.BookCoverFileLocation, exportLocation, true);
                            stringItem.Add(exportLocation);
                        }
                        else
                        {
                            stringItem.Add(string.Empty);
                        }

                        stringItems.Add(stringItem);
                    }

                    var exportCount = count;

                    ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);

                    this.BooksOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                    await Task.Delay(1);
                }
            }
            else
            {
                this.BooksOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadBooksFromSpreadsheet()
        {
            var tableName = "Books";
            var label = (Label)this.View.FindByName("booksOutput");

            if (this.BooksChecked)
            {
                label.TextColor = this.busyColor;
                this.BooksOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                var importCount = valuesList.Count;

                this.BooksOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var book = new BookModel()
                            {
                                BookGuid = ParseGuid(values[0]).Value,
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

                            if (this.ImagesChecked && !string.IsNullOrEmpty(book.BookCoverUrl))
                            {
                                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                                {
                                    try
                                    {
                                        var byteArray = DownloadImage(book.BookCoverUrl);
                                        book.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else
                                {
                                    await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                                }
                            }

                            if (this.ImagesChecked && !string.IsNullOrEmpty(book.BookCoverFileLocation))
                            {
                                try
                                {
                                    var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                                    if (!Directory.Exists(directory))
                                    {
                                        Directory.CreateDirectory(directory);
                                    }

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
                                catch (Exception)
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
                    catch (Exception)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingBook, null);
                    }
                }

                this.BooksOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                this.BooksOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        /*********************** Book Methods ***********************/

        /*********************** WishListBook Methods ***********************/
        private async Task WriteWishListBooksToSpreadsheet()
        {
            var tableName = "WishListBooks";
            var label = (Label)this.View.FindByName("wishListOutput");

            if (this.WishListChecked)
            {
                label.TextColor = this.busyColor;
                this.WishListOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var bookList = await FilterLists.GetBookWishList(true);
                var count = bookList != null ? bookList.Count : 0;

                this.WishListOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    new ()
                    {
                        $"{AppStringResources.BookGuid_Blank.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookTitle.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookAuthors.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookSeries.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookNumber.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookPublisher.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookPublishYear.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookIdentifier.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookFormat.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookLanguage.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookPrice.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookSummary.Replace(" ", string.Empty)}",
                        $"{AppStringResources.TotalPages.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookComments.Replace(" ", string.Empty)}",
                        $"{AppStringResources.WhereToBuy.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookURL.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookCoverUrl.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookImportExportFileLocation.Replace(" ", string.Empty)}",
                    },
                };

                if (bookList != null)
                {
                    foreach (var book in bookList)
                    {
                        try
                        {
                            var stringItem = new List<string?>
                            {
                                book.BookGuid.ToString(),
                                book.BookTitle,
                                book.AuthorListstring,
                                book.BookSeries,
                                book.BookNumberInSeries.ToString(),
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

                            if (this.ImagesChecked && !string.IsNullOrEmpty(book.BookCoverFileLocation))
                            {
                                var fi = new FileInfo(book.BookCoverFileLocation);
                                var exportLocation = $"{this.imageExportLocation}/{fi.Name}";
                                File.Copy(book.BookCoverFileLocation, exportLocation, true);
                                stringItem.Add(exportLocation);
                            }
                            else
                            {
                                stringItem.Add(string.Empty);
                            }

                            stringItems.Add(stringItem);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    var exportCount = bookList.Count;

                    try
                    {
                        ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);
                    }
                    catch (Exception)
                    {
                    }

                    this.WishListOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                    await Task.Delay(1);
                }
            }
            else
            {
                this.WishListOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadWishListBooksFromSpreadsheet()
        {
            var tableName = "WishListBooks";
            var label = (Label)this.View.FindByName("wishListOutput");

            if (this.WishListChecked)
            {
                label.TextColor = this.busyColor;
                this.WishListOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                var importCount = valuesList.Count;

                this.WishListOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var book = new BookModel()
                            {
                                BookGuid = ParseGuid(values[0]).Value,
                                BookTitle = values[1],
                                AuthorListstring = values[2],
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

                            if (this.ImagesChecked && !string.IsNullOrEmpty(book.BookCoverUrl))
                            {
                                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                                {
                                    try
                                    {
                                        var byteArray = DownloadImage(book.BookCoverUrl);
                                        book.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else
                                {
                                    await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                                }
                            }

                            if (this.ImagesChecked && !string.IsNullOrEmpty(book.BookCoverFileLocation))
                            {
                                try
                                {
                                    var directory = $"{FileSystem.AppDataDirectory}/BookCovers";

                                    if (!Directory.Exists(directory))
                                    {
                                        Directory.CreateDirectory(directory);
                                    }

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
                                catch (Exception)
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
                    catch (Exception)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingBook, null);
                    }
                }

                this.WishListOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                this.WishListOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        /*********************** WishListBook Methods ***********************/

        /*********************** Chapter Methods ***********************/
        private async Task WriteChaptersToSpreadsheet()
        {
            var tableName = "Chapters";
            var label = (Label)this.View.FindByName("chaptersOutput");

            if (this.ChaptersChecked)
            {
                label.TextColor = this.busyColor;
                this.ChaptersOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var chapterList = await FilterLists.GetAllChapters();
                var count = chapterList != null ? chapterList.Count : 0;

                this.ChaptersOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    new ()
                    {
                        $"{AppStringResources.ChapterGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.ChapterName.Replace(" ", string.Empty)}",
                        $"{AppStringResources.PageRange.Replace(" ", string.Empty)}",
                        $"{AppStringResources.ChapterOrder.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookGuid.Replace(" ", string.Empty)}",
                    },
                };

                if (chapterList != null)
                {
                    foreach (var chapter in chapterList)
                    {
                        var stringItem = new List<string?>
                        {
                            chapter.ChapterGuid.ToString(),
                            chapter.ChapterName,
                            chapter.PageRange,
                            chapter.ChapterOrder.ToString(),
                            chapter.BookGuid.ToString(),
                        };

                        stringItems.Add(stringItem);
                    }

                    var exportCount = count;

                    ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);

                    this.ChaptersOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                    await Task.Delay(1);
                }
            }
            else
            {
                this.ChaptersOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadChaptersFromSpreadsheet()
        {
            var tableName = "Chapters";
            var label = (Label)this.View.FindByName("chaptersOutput");

            if (this.ChaptersChecked)
            {
                label.TextColor = this.busyColor;
                this.ChaptersOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                var importCount = valuesList.Count;

                this.ChaptersOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var chapter = new ChapterModel()
                            {
                                ChapterGuid = ParseGuid(values[0]),
                                ChapterName = values[1],
                                PageRange = values[2],
                                ChapterOrder = ParseInt(values[3]),
                                BookGuid = ParseGuid(values[4]).Value,
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
                    catch (Exception)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingChapter, null);
                    }
                }

                this.ChaptersOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                this.ChaptersOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        /*********************** Chapter Methods ***********************/

        /*********************** Collection Methods ***********************/
        private async Task WriteCollectionsToSpreadsheet()
        {
            var tableName = "Collections";
            var label = (Label)this.View.FindByName("collectionsOutput");

            if (this.CollectionsChecked)
            {
                label.TextColor = this.busyColor;
                this.CollectionsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var collectionList = await FilterLists.GetAllCollectionsList(true);
                var count = collectionList != null ? collectionList.Count : 0;

                this.CollectionsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    new ()
                    {
                        $"{AppStringResources.CollectionGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.CollectionName.Replace(" ", string.Empty)}",
                    },
                };

                if (collectionList != null)
                {
                    foreach (var collection in collectionList)
                    {
                        var stringItem = new List<string?>
                        {
                            collection.CollectionGuid.ToString(),
                            collection.CollectionName,
                        };

                        stringItems.Add(stringItem);
                    }

                    var exportCount = count;

                    ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);

                    this.CollectionsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                    await Task.Delay(1);
                }
            }
            else
            {
                this.CollectionsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadCollectionsFromSpreadsheet()
        {
            var tableName = "Collections";
            var label = (Label)this.View.FindByName("collectionsOutput");

            if (this.CollectionsChecked)
            {
                label.TextColor = this.busyColor;
                this.CollectionsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                var importCount = valuesList.Count;

                this.CollectionsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var collection = new CollectionModel()
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
                    catch (Exception)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingCollection, null);
                    }
                }

                this.CollectionsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                this.CollectionsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        /*********************** Collection Methods ***********************/

        /*********************** Genre Methods ***********************/
        private async Task WriteGenresToSpreadsheet()
        {
            var tableName = "Genres";
            var label = (Label)this.View.FindByName("genresOutput");

            if (this.GenresChecked)
            {
                label.TextColor = this.busyColor;
                this.GenresOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var genreList = await FilterLists.GetAllGenresList(true);
                var count = genreList != null ? genreList.Count : 0;

                this.GenresOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    new ()
                    {
                        $"{AppStringResources.GenreGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.GenreName.Replace(" ", string.Empty)}",
                    },
                };

                if (genreList != null)
                {
                    foreach (var genre in genreList)
                    {
                        var stringItem = new List<string?>
                        {
                            genre.GenreGuid.ToString(),
                            genre.GenreName,
                        };

                        stringItems.Add(stringItem);
                    }

                    var exportCount = count;

                    ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);

                    this.GenresOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                    await Task.Delay(1);
                }
            }
            else
            {
                this.GenresOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadGenresFromSpreadsheet()
        {
            var tableName = "Genres";
            var label = (Label)this.View.FindByName("genresOutput");

            if (this.GenresChecked)
            {
                label.TextColor = this.busyColor;
                this.GenresOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                var importCount = valuesList.Count;

                this.GenresOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var genre = new GenreModel()
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
                    catch (Exception)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingGenre, null);
                    }
                }

                this.GenresOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                this.GenresOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        /*********************** Genre Methods ***********************/

        /*********************** Series Methods ***********************/
        private async Task WriteSeriesToSpreadsheet()
        {
            var tableName = "Series";
            var label = (Label)this.View.FindByName("seriesOutput");

            if (this.SeriesChecked)
            {
                label.TextColor = this.busyColor;
                this.SeriesOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var seriesList = await FilterLists.GetAllSeriesList(true);
                var count = seriesList != null ? seriesList.Count : 0;

                this.SeriesOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    new ()
                    {
                        $"{AppStringResources.SeriesGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.SeriesName.Replace(" ", string.Empty)}",
                        $"{AppStringResources.TotalBooksInSeries.Replace(" ", string.Empty)}",
                    },
                };

                if (seriesList != null)
                {
                    foreach (var series in seriesList)
                    {
                        var stringItem = new List<string?>
                        {
                            series.SeriesGuid.ToString(),
                            series.SeriesName,
                            series.TotalBooksInSeries,
                        };

                        stringItems.Add(stringItem);
                    }

                    var exportCount = count;

                    ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);

                    this.SeriesOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                    await Task.Delay(1);
                }
            }
            else
            {
                this.SeriesOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadSeriesFromSpreadsheet()
        {
            var tableName = "Series";
            var label = (Label)this.View.FindByName("seriesOutput");

            if (this.SeriesChecked)
            {
                label.TextColor = this.busyColor;
                this.SeriesOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                var importCount = valuesList.Count;

                this.SeriesOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var series = new SeriesModel()
                            {
                                SeriesGuid = ParseGuid(values[0]),
                                SeriesName = values[1],
                                TotalBooksInSeries = values[2],
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
                    catch (Exception)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingSeries, null);
                    }
                }

                this.SeriesOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                this.SeriesOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        /*********************** Series Methods ***********************/

        /*********************** BookAuthor Methods ***********************/
        private async Task WriteBookAuthorsToSpreadsheet()
        {
            var tableName = "BookAuthors";
            var label = (Label)this.View.FindByName("bookAuthorsOutput");

            if (this.BookAuthorsChecked)
            {
                label.TextColor = this.busyColor;
                this.BookAuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var bookAuthorList = await FilterLists.GetAllBookAuthors();
                var count = bookAuthorList != null ? bookAuthorList.Count : 0;

                this.BookAuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    new ()
                    {
                        $"{AppStringResources.BookAuthorGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.AuthorGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.BookGuid.Replace(" ", string.Empty)}",
                    },
                };

                if (bookAuthorList != null)
                {
                    foreach (var bookAuthor in bookAuthorList)
                    {
                        var stringItem = new List<string?>
                        {
                            bookAuthor.BookAuthorGuid.ToString(),
                            bookAuthor.AuthorGuid.ToString(),
                            bookAuthor.BookGuid.ToString(),
                        };

                        stringItems.Add(stringItem);
                    }

                    var exportCount = count;

                    ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);

                    this.BookAuthorsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                    await Task.Delay(1);
                }
            }
            else
            {
                this.BookAuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadBookAuthorsFromSpreadsheet()
        {
            var tableName = "BookAuthors";
            var label = (Label)this.View.FindByName("bookAuthorsOutput");

            if (this.BookAuthorsChecked)
            {
                label.TextColor = this.busyColor;
                this.BookAuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                var importCount = valuesList.Count;

                this.BookAuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var bookAuthor = new BookAuthorModel()
                            {
                                BookAuthorGuid = ParseGuid(values[0]),
                                AuthorGuid = ParseGuid(values[1]).Value,
                                BookGuid = ParseGuid(values[2]).Value,
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
                    catch (Exception)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingBookAuthor, null);
                    }
                }

                this.BookAuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                this.BookAuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        /*********************** BookAuthor Methods ***********************/

        /*********************** Author Methods ***********************/
        private async Task WriteAuthorsToSpreadsheet()
        {
            var tableName = "Authors";
            var label = (Label)this.View.FindByName("authorsOutput");

            if (this.AuthorsChecked)
            {
                label.TextColor = this.busyColor;
                this.AuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var authorList = await FilterLists.GetAllAuthorsList(true);
                var count = authorList != null ? authorList.Count : 0;

                this.AuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    new ()
                    {
                        $"{AppStringResources.AuthorGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.FirstName.Replace(" ", string.Empty)}",
                        $"{AppStringResources.LastName.Replace(" ", string.Empty)}",
                    }
                };

                if (authorList != null)
                {
                    foreach (var author in authorList)
                    {
                        var stringItem = new List<string?>
                    {
                        author.AuthorGuid.ToString(),
                        author.FirstName,
                        author.LastName,
                    };

                        stringItems.Add(stringItem);
                    }

                    var exportCount = count;

                    ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);

                    this.AuthorsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                    await Task.Delay(1);
                }
            }
            else
            {
                this.AuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadAuthorsFromSpreadsheet()
        {
            var tableName = "Authors";
            var label = (Label)this.View.FindByName("authorsOutput");

            if (this.AuthorsChecked)
            {
                label.TextColor = this.busyColor;
                this.AuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                var importCount = valuesList.Count;

                this.AuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var author = new AuthorModel()
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
                    catch (Exception)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingAuthor, null);
                    }
                }

                this.AuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                this.AuthorsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        /*********************** Author Methods ***********************/

        /*********************** Location Methods ***********************/
        private async Task WriteLocationsToSpreadsheet()
        {
            var tableName = "Locations";
            var label = (Label)this.View.FindByName("locationsOutput");

            if (this.LocationsChecked)
            {
                label.TextColor = this.busyColor;
                this.LocationsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var locationList = await FilterLists.GetAllLocationsList(true);
                var count = locationList != null ? locationList.Count : 0;

                this.LocationsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    new ()
                    {
                        $"{AppStringResources.LocationGuid.Replace(" ", string.Empty)}",
                        $"{AppStringResources.LocationName.Replace(" ", string.Empty)}",
                    },
                };

                if (locationList != null)
                {
                    foreach (var location in locationList)
                    {
                        var stringItem = new List<string?>
                        {
                            location.LocationGuid.ToString(),
                            location.LocationName,
                        };

                        stringItems.Add(stringItem);
                    }

                    var exportCount = count;

                    ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);

                    this.LocationsOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                    await Task.Delay(1);
                }
            }
            else
            {
                this.LocationsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        private async Task ReadLocationsFromSpreadsheet()
        {
            var tableName = "Locations";
            var label = (Label)this.View.FindByName("locationsOutput");

            if (this.LocationsChecked)
            {
                label.TextColor = this.busyColor;
                this.LocationsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                List<List<string>> valuesList = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                var importCount = valuesList.Count;

                this.LocationsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var location = new LocationModel()
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
                    catch (Exception)
                    {
                        importCount--;
                        await DisplayMessage(AppStringResources.ErrorSavingLocation, null);
                    }
                }

                this.LocationsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];

                await Task.Delay(1);
            }
            else
            {
                this.LocationsOutput = AppStringResources.ItemSkipped.Replace("Item", tableName);

                await Task.Delay(1);
            }
        }

        /*********************** Location Methods ***********************/
        /*********************** Table Methods ***********************/
    }
}
