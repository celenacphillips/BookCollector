// <copyright file="ExportImportViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Main
{
    using BookCollector.CustomPermissions;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Data.Spreadsheet;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.Author;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Book;
    using BookCollector.ViewModels.Collection;
    using BookCollector.ViewModels.Genre;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.ViewModels.Library;
    using BookCollector.ViewModels.Location;
    using BookCollector.ViewModels.Series;
    using BookCollector.ViewModels.WishListBook;
    using BookCollector.Views.Popups;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Maui.Storage;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Color = Microsoft.Maui.Graphics.Color;

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

        private readonly Color? busyColor = Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["Tertiary"] :
                (Color?)Application.Current?.Resources["Primary"];

        private string mainFilePath = string.Empty;

        private string imageLocation = string.Empty;

        public ExportImportViewModel(ContentPage view)
        {
            this.View = view;
            this.InfoText = AppStringResources.ExportImportView_InfoText;
            RefreshView = true;
        }

        public static bool RefreshView { get; set; }

        public static bool ManuallyUploadLibraryCovers { get; set; }

        public static bool ManuallyUploadWishlistCovers { get; set; }

        public async Task SetViewModelData()
        {
            if (RefreshView)
            {
                this.SetIsBusyTrue();

                this.ExportEnabled = true;
                this.ImportEnabled = true;
                this.RefreshEnabled = true;
                this.CheckboxesVisible = true;
                this.OutputVisible = false;
                this.BooksChecked = true;
                this.ChaptersChecked = true;
                this.BookAuthorsChecked = true;
                this.WishListChecked = true;
                this.CollectionsChecked = true;
                this.GenresChecked = true;
                this.SeriesChecked = true;
                this.AuthorsChecked = true;
                this.LocationsChecked = true;
                this.ImagesChecked = false;

                this.SetIsBusyFalse();
                RefreshView = false;
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task Export()
        {
            var action = await this.DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureExport_Question, null, null);

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
                            await this.CanceledAction();

                            this.SetIsBusyFalse();
                            this.ImportEnabled = true;
                            this.ExportEnabled = true;
                            this.RefreshEnabled = true;
                        }
                    }

                    if (this.ImagesChecked)
                    {
                        this.imageLocation = $"{exportLocation}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                        if (!Directory.Exists(this.imageLocation))
                        {
                            Directory.CreateDirectory(this.imageLocation);
                        }

                        await this.DisplayMessage(AppStringResources.BookCoverDownloads, AppStringResources.BookCoversDownloadMessage);
                    }

                    await Task.Delay(1);

                    this.SetIsBusyTrue();

                    this.ImportEnabled = false;
                    this.ExportEnabled = false;
                    this.RefreshEnabled = false;

                    this.ResetOutput();
                    this.OutputVisible = true;
                    this.CheckboxesVisible = false;

                    var filePath = await ReadWriteSpreadsheet.CreateSpreadsheet(exportLocation, $"{GetDate()}-{AppInfo.Current.Name.Replace(" ", string.Empty)}Export.xlsx");
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

                    await this.DisplayMessage(AppStringResources.ExportComplete, null);

                    this.SetIsBusyFalse();
                    this.RefreshEnabled = true;
                }
                catch (UnauthorizedAccessException ex)
                {
                    await this.CanceledAction();
                    Preferences.Set("ExportLocation", AppStringResources.DefaultExportLocation);
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.PleaseSelectAnotherFolder, null);
#endif
                    this.SetIsBusyFalse();
                    this.ImportEnabled = true;
                    this.ExportEnabled = true;
                    this.RefreshEnabled = true;
                    this.CheckboxesVisible = true;
                    this.OutputVisible = false;
                }
                catch (Exception ex)
                {
                    await this.CanceledAction();
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    this.ImportEnabled = true;
                    this.ExportEnabled = true;
                    this.RefreshEnabled = true;
                    this.ResetColorOutput();
                }
            }
            else
            {
                await this.CanceledAction();
            }
        }

        [RelayCommand]
        public async Task Import()
        {
            var action = await this.DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureImport_Question, null, null);

            if (action)
            {
                try
                {
                    string[] acceptableFileTypes = ["application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"];
                    var customFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, acceptableFileTypes }, // MIME type
                    });

                    PickOptions pickerOptions = new ()
                    {
                        FileTypes = customFileType,
                    };

                    var result = await FilePicker.Default.PickAsync(pickerOptions);

                    if (result != null)
                    {
                        if (this.ImagesChecked)
                        {
                            ManuallyUploadLibraryCovers = true;
                            ManuallyUploadWishlistCovers = true;
                            // To fix later.
                            // Create a popup for each book that allows the user to select the image. - Done
                            // Display the image title and the book cover for the user to confirm or reselect. - To Do
#if ANDROID
                            //var version = (int)Build.VERSION.SdkInt;

                            //if (version >= 33)
                            //{
                            //    await DisplayMessage(AppStringResources.PleaseSelectTheImages, null);

                            //    var picker = ServiceHelper.GetService<IAndroidImagePicker>();
                            //    var uris = await picker.PickImagesAsync();

                            //    foreach (var uri in uris)
                            //    {
                            //        var info = this.GetImageInfo(uri);
                            //        this.selectedFiles.Add(info);
                            //    }
                            //}
#endif
                        }

                        this.SetIsBusyTrue();

                        this.mainFilePath = result.FullPath;

                        this.ImportEnabled = false;
                        this.ExportEnabled = false;
                        this.RefreshEnabled = false;

                        this.ResetOutput();
                        this.OutputVisible = true;
                        this.CheckboxesVisible = false;

                        this.StartOutput = AppStringResources.ImportResultsStart;

                        this.SetOutputWaiting();

                        var errors = 0;

                        await Task.Delay(1);

                        errors += await this.ReadWishListBooksFromSpreadsheet();
                        errors += await this.ReadAuthorsFromSpreadsheet();
                        errors += await this.ReadChaptersFromSpreadsheet();
                        errors += await this.ReadCollectionsFromSpreadsheet();
                        errors += await this.ReadGenresFromSpreadsheet();
                        errors += await this.ReadSeriesFromSpreadsheet();
                        errors += await this.ReadLocationsFromSpreadsheet();
                        errors += await this.ReadBooksFromSpreadsheet();
                        errors += await this.ReadBookAuthorsFromSpreadsheet();

                        await DataCleanup(this.BooksChecked, this.AuthorsChecked, this.BookAuthorsChecked);

                        this.FinalOutput = AppStringResources.ImportResultsFinish;

                        await this.DisplayMessage(AppStringResources.ImportComplete, null);

                        if (errors > 0)
                        {
                            await this.DisplayMessage(AppStringResources.ImportErrorsTitle, AppStringResources.ImportErrors);
                        }

                        this.SetIsBusyFalse();
                        this.RefreshEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    await this.CanceledAction();
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    this.ImportEnabled = true;
                    this.ExportEnabled = true;
                    this.RefreshEnabled = true;
                    this.ResetColorOutput();
                }
            }
            else
            {
                await this.CanceledAction();
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

        private static string? ParseString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            else
            {
                return input;
            }
        }

        private static async Task DataCleanup(bool booksChecked, bool authorsChecked, bool bookAuthorsChecked)
        {
            if (booksChecked && authorsChecked && bookAuthorsChecked)
            {
                await Database.GetAllAuthorsAsync();
            }

            ReadingViewModel.RefreshView = true;
            ToBeReadViewModel.RefreshView = true;
            ReadViewModel.RefreshView = true;
            AllBooksViewModel.RefreshView = true;
            CollectionsViewModel.RefreshView = true;
            GenresViewModel.RefreshView = true;
            SeriesViewModel.RefreshView = true;
            AuthorsViewModel.RefreshView = true;
            LocationsViewModel.RefreshView = true;
        }

        private static List<string?> SetBookColumns()
        {
            return
            [
                $"{AppStringResources.BookGuid_Blank}",
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
                $"{AppStringResources.ListenHours.Replace(" ", string.Empty)}",
                $"{AppStringResources.ListenMinutes.Replace(" ", string.Empty)}",
                $"{AppStringResources.TotalHours.Replace(" ", string.Empty)}",
                $"{AppStringResources.TotalMinutes.Replace(" ", string.Empty)}",
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
                $"{AppStringResources.Hide_Question}",
                $"{AppStringResources.BookCoverUrl.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookCoverFileName.Replace(" ", string.Empty)}",
            ];
        }

        private static List<string?> SetWishlistBookColumns()
        {
            return
            [
                $"{AppStringResources.BookGuid_Blank}",
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
                $"{AppStringResources.TotalHours.Replace(" ", string.Empty)}",
                $"{AppStringResources.TotalMinutes.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookComments.Replace(" ", string.Empty)}",
                $"{AppStringResources.WhereToBuy.Replace(" ", string.Empty)}",
                $"{AppStringResources.Hide_Question}",
                $"{AppStringResources.BookURL.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookCoverUrl.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookCoverFileName.Replace(" ", string.Empty)}",
            ];
        }

        private static List<string?> SetChapterColumns()
        {
            return
            [
                $"{AppStringResources.ChapterGuid}",
                $"{AppStringResources.ChapterName.Replace(" ", string.Empty)}",
                $"{AppStringResources.PageRange.Replace(" ", string.Empty)}",
                $"{AppStringResources.ChapterOrder.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookGuid.Replace(" ", string.Empty)}",
            ];
        }

        private static List<string?> SetCollectionColumns()
        {
            return
            [
                $"{AppStringResources.CollectionGuid}",
                $"{AppStringResources.CollectionName.Replace(" ", string.Empty)}",
                $"{AppStringResources.Hide_Question}",
            ];
        }

        private static List<string?> SetGenreColumns()
        {
            return
            [
                $"{AppStringResources.GenreGuid}",
                $"{AppStringResources.GenreName.Replace(" ", string.Empty)}",
                $"{AppStringResources.Hide_Question}",
            ];
        }

        private static List<string?> SetSeriesColumns()
        {
            return
            [
                $"{AppStringResources.SeriesGuid}",
                $"{AppStringResources.SeriesName.Replace(" ", string.Empty)}",
                $"{AppStringResources.TotalBooksInSeries.Replace(" ", string.Empty)}",
                $"{AppStringResources.Hide_Question}",
            ];
        }

        private static List<string?> SetBookAuthorColumns()
        {
            return
            [
                $"{AppStringResources.BookAuthorGuid}",
                $"{AppStringResources.AuthorGuid.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookGuid.Replace(" ", string.Empty)}",
            ];
        }

        private static List<string?> SetAuthorColumns()
        {
            return
            [
                $"{AppStringResources.AuthorGuid_Blank}",
                $"{AppStringResources.FirstName.Replace(" ", string.Empty)}",
                $"{AppStringResources.LastName.Replace(" ", string.Empty)}",
                $"{AppStringResources.Hide_Question}",
            ];
        }

        private static List<string?> SetLocationColumns()
        {
            return
            [
                $"{AppStringResources.LocationGuid}",
                $"{AppStringResources.LocationName.Replace(" ", string.Empty)}",
                $"{AppStringResources.Hide_Question}",
            ];
        }

        private async Task ManuallySetBookCover(BookModel book, bool imagesChecked)
        {
            if (imagesChecked && ManuallyUploadLibraryCovers)
            {
                var answer = await this.View.ShowPopupAsync<string>(new MissingBookCoverPopup(book.BookTitle));

                if (!string.IsNullOrEmpty(answer.Result) && answer.Result.Equals(AppStringResources.SkipAll))
                {
                    ManuallyUploadLibraryCovers = false;
                }

                if (!string.IsNullOrEmpty(answer.Result) && answer.Result.Equals(AppStringResources.Yes))
                {
                    MediaPickerOptions pickerOptions = new ();

                    try
                    {
                        var photos = await MediaPicker.PickPhotosAsync(pickerOptions);

                        if (photos?.Count > 0)
                        {
                            var firstPhoto = photos.First();
                            var bookCoverImageSource = ImageSource.FromFile(firstPhoto.FullPath);

                            var result = await this.View.ShowPopupAsync<bool>(new BookCoverMatchingPopup(bookCoverImageSource, book.BookTitle));

                            if (!result.Result)
                            {
                                await this.ManuallySetBookCover(book, imagesChecked);
                            }

                            book.BookCover = ImageSource.FromFile(firstPhoto.FullPath);
                            book.HasBookCover = true;
                            book.HasNoBookCover = false;

                            var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            var fi = new FileInfo(firstPhoto.FullPath);
                            var filePath = $"{directory}/{fi.Name}";
                            File.Copy(firstPhoto.FullPath, filePath, true);

                            book.BookCoverFileName = fi.Name;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private async Task ManuallySetBookCover(WishlistBookModel book, bool imagesChecked)
        {
            if (imagesChecked && ManuallyUploadWishlistCovers)
            {
                var answer = await this.View.ShowPopupAsync<string>(new MissingBookCoverPopup(book.BookTitle));

                if (!string.IsNullOrEmpty(answer.Result) && answer.Result.Equals(AppStringResources.SkipAll))
                {
                    ManuallyUploadWishlistCovers = false;
                }

                if (!string.IsNullOrEmpty(answer.Result) && answer.Result.Equals(AppStringResources.Yes))
                {
                    MediaPickerOptions pickerOptions = new ();

                    try
                    {
                        var photos = await MediaPicker.PickPhotosAsync(pickerOptions);

                        if (photos?.Count > 0)
                        {
                            var firstPhoto = photos.First();
                            book.BookCover = ImageSource.FromFile(firstPhoto.FullPath);
                            book.HasBookCover = true;
                            book.HasNoBookCover = false;

                            var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            var fi = new FileInfo(firstPhoto.FullPath);
                            var filePath = $"{directory}/{fi.Name}";
                            File.Copy(firstPhoto.FullPath, filePath, true);

                            book.BookCoverFileName = fi.Name;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
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

        private void ResetColorOutput()
        {
            var booksOutputLabel = (Label)this.View.FindByName("booksOutput");
            booksOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];

            var chaptersOutputLabel = (Label)this.View.FindByName("chaptersOutput");
            chaptersOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];

            var bookAuthorsOutputLabel = (Label)this.View.FindByName("bookAuthorsOutput");
            bookAuthorsOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];

            var wishListOutputLabel = (Label)this.View.FindByName("wishListOutput");
            wishListOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];

            var collectionsOutputLabel = (Label)this.View.FindByName("collectionsOutput");
            collectionsOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];

            var genresOutputLabel = (Label)this.View.FindByName("genresOutput");
            genresOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];

            var seriesOutputLabel = (Label)this.View.FindByName("seriesOutput");
            seriesOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];

            var authorsOutputLabel = (Label)this.View.FindByName("authorsOutput");
            authorsOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];

            var locationsOutputLabel = (Label)this.View.FindByName("locationsOutput");
            locationsOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];

            var finalOutputLabel = (Label)this.View.FindByName("finalOutput");
            finalOutputLabel.TextColor =
                Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["TextDark"] :
                (Color?)Application.Current?.Resources["TextLight"];
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

        private static string BookCoverFileName(string bookTitle, string format, string extension)
        {
            string output = bookTitle.Replace(" ", "_")
                                     .Replace("<", "_")
                                     .Replace(">", "_")
                                     .Replace(":", "_")
                                     .Replace("\"", "_")
                                     .Replace("|", "_")
                                     .Replace("\\", "_")
                                     .Replace("?", "_")
                                     .Replace("*", "_")
                                     .Replace("/", "_");

            return $"{output}-{format}{extension}";
        }

        /*********************** Table Methods ***********************/

        /*********************** Book Methods ***********************/
        private async Task WriteBooksToSpreadsheet()
        {
            var tableName = AppStringResources.Books.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("booksOutput");

            if (this.BooksChecked)
            {
                label.TextColor = this.busyColor;
                this.BooksOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var bookList = await FillLists.GetAllBooksList();
                var count = bookList != null ? bookList.Count : 0;

                this.BooksOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    SetBookColumns(),
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
                            book.BookNumberInSeries,
                            book.BookPublisher,
                            book.BookPublishYear,
                            book.BookIdentifier,
                            book.BookFormat,
                            book.BookLanguage,
                            book.BookPrice,
                            book.BookSummary,
                            book.BookPageRead.ToString(),
                            book.BookPageTotal.ToString(),
                            book.BookHourListened.ToString(),
                            book.BookMinuteListened.ToString(),
                            book.BookHoursTotal.ToString(),
                            book.BookMinutesTotal.ToString(),
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
                            book.HideBook.ToString(),
                            book.BookCoverUrl,
                            book.BookCoverFileName,
                        };
                        stringItems.Add(stringItem);
                    }

                    var exportCount = 0;

                    ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);

                    foreach (var book in bookList)
                    {
                        if (this.ImagesChecked)
                        {
                            var booksImageExport = $"{this.imageLocation}/{AppStringResources.LibraryBookCovers.Replace(" ", string.Empty)}";

                            if (!Directory.Exists(booksImageExport))
                            {
                                Directory.CreateDirectory(booksImageExport);
                            }

                            if (!string.IsNullOrEmpty(book.BookCoverFileName))
                            {
                                var bookCoverFileLocation = $"{booksImageExport}/{AppStringResources.BookCoverUploads.Replace(" ", string.Empty)}";
                                var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                                if (!Directory.Exists(bookCoverFileLocation))
                                {
                                    Directory.CreateDirectory(bookCoverFileLocation);
                                }

                                var fi = new FileInfo($"{directory}/{book.BookCoverFileName}");
                                var exportLocation = $"{bookCoverFileLocation}/{book.BookCoverFileName}";
                                File.Copy($"{directory}/{book.BookCoverFileName}", exportLocation, true);
                            }

                            if (!string.IsNullOrEmpty(book.BookCoverUrl))
                            {
                                var bookCoverFileLocation = $"{booksImageExport}/{AppStringResources.BookCoverDownloads.Replace(" ", string.Empty)}";

                                if (!Directory.Exists(bookCoverFileLocation))
                                {
                                    Directory.CreateDirectory(bookCoverFileLocation);
                                }

                                try
                                {
                                    var byteArray = DownloadImage(book.BookCoverUrl);
                                    var fi = new FileInfo(book.BookCoverUrl);
                                    var fileName = BookCoverFileName(book.BookTitle!, book.BookFormat!, fi.Extension);
                                    var exportLocation = $"{bookCoverFileLocation}/{fileName}";
                                    File.WriteAllBytes(exportLocation, byteArray);
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }

                        exportCount++;
                        this.BooksOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                        await Task.Delay(1);
                    }

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

        private async Task<int> ReadBooksFromSpreadsheet()
        {
            var tableName = AppStringResources.Books.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("booksOutput");

            if (this.BooksChecked)
            {
                label.TextColor = this.busyColor;
                this.BooksOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var columnNames = SetBookColumns();

                (List<List<string>> valuesList, string message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, columnNames);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.BooksOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);

                    if (message.StartsWith("There is no spreadsheet named") ||
                        message.StartsWith("The column count is not right for") ||
                        message.StartsWith("The columns are not in the right order for"))
                    {
                        return 1;
                    }
                }

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
                                BookGuid = ParseGuid(values[0]).HasValue ? ParseGuid(values[0]).Value : null,
                                BookTitle = values[1],
                                BookSeriesGuid = ParseGuid(values[2]),
                                BookNumberInSeries = values[3],
                                BookPublisher = values[4],
                                BookPublishYear = values[5],
                                BookIdentifier = values[6],
                                BookFormat = values[7],
                                BookLanguage = values[8],
                                BookPrice = values[9],
                                BookSummary = values[10],
                                BookPageRead = ParseInt(values[11]),
                                BookPageTotal = ParseInt(values[12]),
                                BookHourListened = ParseInt(values[13]),
                                BookMinuteListened = ParseInt(values[14]),
                                BookHoursTotal = ParseInt(values[15]),
                                BookMinutesTotal = ParseInt(values[16]),
                                BookStartDate = ParseString(values[17]),
                                BookEndDate = ParseString(values[18]),
                                BookLocationGuid = ParseGuid(values[19]),
                                BookComments = values[20],
                                BookCollectionGuid = ParseGuid(values[21]),
                                BookGenreGuid = ParseGuid(values[22]),
                                BookURL = values[23],
                                Rating = ParseInt(values[24]),
                                IsFavorite = ParseBool(values[25]),
                                LoanedTo = values[26],
                                BookLoanedOutOn = ParseString(values[27]),
                                HideBook = ParseBool(values[28]),
                                BookCoverUrl = values[29],
                                BookCoverFileName = values[30],
                            };

                            if (!string.IsNullOrEmpty(book.BookCoverUrl))
                            {
                                PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                                if (internetStatus != PermissionStatus.Granted)
                                {
                                    internetStatus = await Permissions.RequestAsync<InternetPermission>();
                                }

                                if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                                {
                                    try
                                    {
                                        book.BookCover = new UriImageSource
                                        {
                                            Uri = new Uri(book.BookCoverUrl),
                                            CachingEnabled = true,
                                            CacheValidity = TimeSpan.FromDays(14),
                                        };
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }

                            if (this.ImagesChecked && !string.IsNullOrEmpty(book.BookCoverFileName))
                            {
                                await this.ManuallySetBookCover(book, this.ImagesChecked);
                                book.HasNoBookCover = !book.HasBookCover;
                            }

                            await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                            await BookEditViewModel.AddToStaticList(book);

                            importCount++;
                            this.BooksOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // importCount--;
                        // await DisplayMessage(AppStringResources.ErrorSavingBook, null);
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

            return 0;
        }

        /*********************** Book Methods ***********************/

        /*********************** WishListBook Methods ***********************/
        private async Task WriteWishListBooksToSpreadsheet()
        {
            var tableName = AppStringResources.WishlistBooks.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("wishListOutput");

            if (this.WishListChecked)
            {
                label.TextColor = this.busyColor;
                this.WishListOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var bookList = await FillLists.GetBookWishList();

                var count = bookList != null ? bookList.Count : 0;

                this.WishListOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    SetWishlistBookColumns(),
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
                                book.AuthorListString,
                                book.BookSeries,
                                book.BookNumberInSeries,
                                book.BookPublisher,
                                book.BookPublishYear,
                                book.BookIdentifier,
                                book.BookFormat,
                                book.BookLanguage,
                                book.BookPrice,
                                book.BookSummary,
                                book.BookPageTotal.ToString(),
                                book.BookHoursTotal.ToString(),
                                book.BookMinutesTotal.ToString(),
                                book.BookComments,
                                book.BookWhereToBuy,
                                book.HideBook.ToString(),
                                book.BookURL,
                                book.BookCoverUrl,
                                book.BookCoverFileName,
                            };

                            stringItems.Add(stringItem);
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    var exportCount = 0;

                    try
                    {
                        ReadWriteSpreadsheet.WriteToSpreadsheet(this.mainFilePath, stringItems, tableName);
                    }
                    catch (Exception ex)
                    {
                    }

                    foreach (var book in bookList)
                    {
                        if (this.ImagesChecked)
                        {
                            var wishlistBooksImageExport = $"{this.imageLocation}/{AppStringResources.WishlistBookCovers.Replace(" ", string.Empty)}";

                            if (!Directory.Exists(wishlistBooksImageExport))
                            {
                                Directory.CreateDirectory(wishlistBooksImageExport);
                            }

                            if (!string.IsNullOrEmpty(book.BookCoverFileName))
                            {
                                var bookCoverFileLocation = $"{wishlistBooksImageExport}/{AppStringResources.BookCoverUploads.Replace(" ", string.Empty)}";
                                var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                                if (!Directory.Exists(bookCoverFileLocation))
                                {
                                    Directory.CreateDirectory(bookCoverFileLocation);
                                }

                                var fi = new FileInfo($"{directory}/{book.BookCoverFileName}");
                                var exportLocation = $"{bookCoverFileLocation}/{book.BookCoverFileName}";
                                File.Copy($"{directory}/{book.BookCoverFileName}", exportLocation, true);
                            }

                            if (!string.IsNullOrEmpty(book.BookCoverUrl))
                            {
                                var bookCoverFileLocation = $"{wishlistBooksImageExport}/{AppStringResources.BookCoverDownloads.Replace(" ", string.Empty)}";

                                if (!Directory.Exists(bookCoverFileLocation))
                                {
                                    Directory.CreateDirectory(bookCoverFileLocation);
                                }

                                var byteArray = DownloadImage(book.BookCoverUrl);

                                var fi = new FileInfo(book.BookCoverUrl);
                                var fileName = BookCoverFileName(book.BookTitle!, book.BookFormat!, fi.Extension);
                                var exportLocation = $"{bookCoverFileLocation}/{fileName}";
                                File.WriteAllBytes(exportLocation, byteArray);
                            }
                        }

                        exportCount++;
                        this.WishListOutput = AppStringResources.Table_YExported.Replace("Table", tableName).Replace("y", $"{exportCount}").Replace("z", $"{count}");
                        await Task.Delay(1);
                    }

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

        private async Task<int> ReadWishListBooksFromSpreadsheet()
        {
            var tableName = AppStringResources.WishlistBooks.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("wishListOutput");

            if (this.WishListChecked)
            {
                label.TextColor = this.busyColor;
                this.WishListOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var columnNames = SetWishlistBookColumns();

                (List<List<string>> valuesList, string message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, columnNames);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.WishListOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);

                    if (message.StartsWith("There is no spreadsheet named") ||
                        message.StartsWith("The column count is not right for") ||
                        message.StartsWith("The columns are not in the right order for"))
                    {
                        return 1;
                    }
                }

                this.WishListOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList.Count}");

                await Task.Delay(1);

                foreach (var values in valuesList)
                {
                    try
                    {
                        if (values.Count > 0)
                        {
                            var book = new WishlistBookModel()
                            {
                                BookGuid = ParseGuid(values[0]).HasValue ? ParseGuid(values[0]).Value : null,
                                BookTitle = values[1],
                                AuthorListString = values[2],
                                BookSeries = values[3],
                                BookNumberInSeries = values[4],
                                BookPublisher = values[5],
                                BookPublishYear = values[6],
                                BookIdentifier = values[7],
                                BookFormat = values[8],
                                BookLanguage = values[9],
                                BookPrice = values[10],
                                BookSummary = values[11],
                                BookPageTotal = ParseInt(values[12]),
                                BookHoursTotal = ParseInt(values[13]),
                                BookMinutesTotal = ParseInt(values[14]),
                                BookComments = values[15],
                                BookWhereToBuy = values[16],
                                HideBook = ParseBool(values[17]),
                                BookURL = values[18],
                                BookCoverUrl = values[19],
                                BookCoverFileName = values[20],
                            };

                            if (!string.IsNullOrEmpty(book.BookCoverUrl))
                            {
                                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                                {
                                    try
                                    {
                                        book.BookCover = new UriImageSource
                                        {
                                            Uri = new Uri(book.BookCoverUrl),
                                            CachingEnabled = true,
                                            CacheValidity = TimeSpan.FromDays(14),
                                        };
                                        book.HasBookCover = true;
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }

                            if (this.ImagesChecked && !string.IsNullOrEmpty(book.BookCoverFileName))
                            {
                                await this.ManuallySetBookCover(book, this.ImagesChecked);
                                book.HasNoBookCover = !book.HasBookCover;
                            }

                            await Database.SaveWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(book));
                            WishListBookEditViewModel.AddToStaticList(book);

                            importCount++;
                            this.WishListOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // importCount--;
                        // await DisplayMessage(AppStringResources.ErrorSavingBook, null);
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

            return 0;
        }

        /*********************** WishListBook Methods ***********************/

        /*********************** Chapter Methods ***********************/
        private async Task WriteChaptersToSpreadsheet()
        {
            var tableName = AppStringResources.Chapters.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("chaptersOutput");

            if (this.ChaptersChecked)
            {
                label.TextColor = this.busyColor;
                this.ChaptersOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var chapterList = await FillLists.GetAllChapters();
                var count = chapterList != null ? chapterList.Count : 0;

                this.ChaptersOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    SetChapterColumns(),
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

        private async Task<int> ReadChaptersFromSpreadsheet()
        {
            var tableName = AppStringResources.Chapters.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("chaptersOutput");

            if (this.ChaptersChecked)
            {
                label.TextColor = this.busyColor;
                this.ChaptersOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var columnNames = SetChapterColumns();

                (List<List<string>> valuesList, string message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, columnNames);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.ChaptersOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);

                    if (message.StartsWith("There is no spreadsheet named") ||
                        message.StartsWith("The column count is not right for") ||
                        message.StartsWith("The columns are not in the right order for"))
                    {
                        return 1;
                    }
                }

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

                            await Database.SaveChapterAsync(ConvertTo<ChapterDatabaseModel>(chapter));

                            importCount++;
                            this.ChaptersOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // importCount--;
                        // await DisplayMessage(AppStringResources.ErrorSavingChapter, null);
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

            return 0;
        }

        /*********************** Chapter Methods ***********************/

        /*********************** Collection Methods ***********************/
        private async Task WriteCollectionsToSpreadsheet()
        {
            var tableName = AppStringResources.Collections.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("collectionsOutput");

            if (this.CollectionsChecked)
            {
                label.TextColor = this.busyColor;
                this.CollectionsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var collectionList = await FillLists.GetAllCollectionsList();
                var count = collectionList != null ? collectionList.Count : 0;

                this.CollectionsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    SetCollectionColumns(),
                };

                if (collectionList != null)
                {
                    foreach (var collection in collectionList)
                    {
                        var stringItem = new List<string?>
                        {
                            collection.CollectionGuid.ToString(),
                            collection.CollectionName,
                            collection.HideCollection.ToString(),
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

        private async Task<int> ReadCollectionsFromSpreadsheet()
        {
            var tableName = AppStringResources.Collections.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("collectionsOutput");

            if (this.CollectionsChecked)
            {
                label.TextColor = this.busyColor;
                this.CollectionsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var columnNames = SetCollectionColumns();

                (List<List<string>> valuesList, string message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, columnNames);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.CollectionsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);

                    if (message.StartsWith("There is no spreadsheet named") ||
                        message.StartsWith("The column count is not right for") ||
                        message.StartsWith("The columns are not in the right order for"))
                    {
                        return 1;
                    }
                }

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
                                HideCollection = ParseBool(values[2]),
                            };

                            await Database.SaveCollectionAsync(ConvertTo<CollectionDatabaseModel>(collection));
                            await CollectionEditViewModel.AddToStaticList(collection);

                            importCount++;
                            this.CollectionsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // importCount--;
                        // await DisplayMessage(AppStringResources.ErrorSavingCollection, null);
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

            return 0;
        }

        /*********************** Collection Methods ***********************/

        /*********************** Genre Methods ***********************/
        private async Task WriteGenresToSpreadsheet()
        {
            var tableName = AppStringResources.Genres.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("genresOutput");

            if (this.GenresChecked)
            {
                label.TextColor = this.busyColor;
                this.GenresOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var genreList = await FillLists.GetAllGenresList();
                var count = genreList != null ? genreList.Count : 0;

                this.GenresOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    SetGenreColumns(),
                };

                if (genreList != null)
                {
                    foreach (var genre in genreList)
                    {
                        var stringItem = new List<string?>
                        {
                            genre.GenreGuid.ToString(),
                            genre.GenreName,
                            genre.HideGenre.ToString(),
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

        private async Task<int> ReadGenresFromSpreadsheet()
        {
            var tableName = AppStringResources.Genres.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("genresOutput");

            if (this.GenresChecked)
            {
                label.TextColor = this.busyColor;
                this.GenresOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var columnNames = SetGenreColumns();

                (List<List<string>> valuesList, string message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, columnNames);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.GenresOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);

                    if (message.StartsWith("There is no spreadsheet named") ||
                        message.StartsWith("The column count is not right for") ||
                        message.StartsWith("The columns are not in the right order for"))
                    {
                        return 1;
                    }
                }

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
                                HideGenre = ParseBool(values[2]),
                            };

                            await Database.SaveGenreAsync(ConvertTo<GenreDatabaseModel>(genre));
                            await GenreEditViewModel.AddToStaticList(genre);

                            importCount++;
                            this.GenresOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // importCount--;
                        // await DisplayMessage(AppStringResources.ErrorSavingGenre, null);
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

            return 0;
        }

        /*********************** Genre Methods ***********************/

        /*********************** Series Methods ***********************/
        private async Task WriteSeriesToSpreadsheet()
        {
            var tableName = AppStringResources.Series.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("seriesOutput");

            if (this.SeriesChecked)
            {
                label.TextColor = this.busyColor;
                this.SeriesOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var seriesList = await FillLists.GetAllSeriesList();
                var count = seriesList != null ? seriesList.Count : 0;

                this.SeriesOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    SetSeriesColumns(),
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
                            series.HideSeries.ToString(),
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

        private async Task<int> ReadSeriesFromSpreadsheet()
        {
            var tableName = AppStringResources.Series.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("seriesOutput");

            if (this.SeriesChecked)
            {
                label.TextColor = this.busyColor;
                this.SeriesOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var columnNames = SetSeriesColumns();

                (List<List<string>> valuesList, string message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, columnNames);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.SeriesOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);

                    if (message.StartsWith("There is no spreadsheet named") ||
                        message.StartsWith("The column count is not right for") ||
                        message.StartsWith("The columns are not in the right order for"))
                    {
                        return 1;
                    }
                }

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
                                HideSeries = ParseBool(values[3]),
                            };

                            await Database.SaveSeriesAsync(ConvertTo<SeriesDatabaseModel>(series));
                            await SeriesEditViewModel.AddToStaticList(series);

                            importCount++;
                            this.SeriesOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // importCount--;
                        // await DisplayMessage(AppStringResources.ErrorSavingSeries, null);
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

            return 0;
        }

        /*********************** Series Methods ***********************/

        /*********************** BookAuthor Methods ***********************/
        private async Task WriteBookAuthorsToSpreadsheet()
        {
            var tableName = AppStringResources.BookAuthors.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("bookAuthorsOutput");

            if (this.BookAuthorsChecked)
            {
                label.TextColor = this.busyColor;
                this.BookAuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var bookAuthorList = await FillLists.GetAllBookAuthors();
                var count = bookAuthorList != null ? bookAuthorList.Count : 0;

                this.BookAuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    SetBookAuthorColumns(),
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

        private async Task<int> ReadBookAuthorsFromSpreadsheet()
        {
            var tableName = AppStringResources.BookAuthors.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("bookAuthorsOutput");

            if (this.BookAuthorsChecked)
            {
                label.TextColor = this.busyColor;
                this.BookAuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var columnNames = SetBookAuthorColumns();

                (List<List<string>> valuesList, string message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, columnNames);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.BookAuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);

                    if (message.StartsWith("There is no spreadsheet named") ||
                        message.StartsWith("The column count is not right for") ||
                        message.StartsWith("The columns are not in the right order for"))
                    {
                        return 1;
                    }
                }

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

                            await Database.SaveBookAuthorAsync(bookAuthor);

                            importCount++;
                            this.BookAuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // importCount--;
                        // await DisplayMessage(AppStringResources.ErrorSavingBookAuthor, null);
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

            return 0;
        }

        /*********************** BookAuthor Methods ***********************/

        /*********************** Author Methods ***********************/
        private async Task WriteAuthorsToSpreadsheet()
        {
            var tableName = AppStringResources.Authors.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("authorsOutput");

            if (this.AuthorsChecked)
            {
                label.TextColor = this.busyColor;
                this.AuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var authorList = await FillLists.GetAllAuthorsList();
                var count = authorList != null ? authorList.Count : 0;

                this.AuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    SetAuthorColumns(),
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
                            author.HideAuthor.ToString(),
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

        private async Task<int> ReadAuthorsFromSpreadsheet()
        {
            var tableName = AppStringResources.Authors.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("authorsOutput");

            if (this.AuthorsChecked)
            {
                label.TextColor = this.busyColor;
                this.AuthorsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var columnNames = SetAuthorColumns();

                (List<List<string>> valuesList, string message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, columnNames);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.AuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);

                    if (message.StartsWith("There is no spreadsheet named") ||
                        message.StartsWith("The column count is not right for") ||
                        message.StartsWith("The columns are not in the right order for"))
                    {
                        return 1;
                    }
                }

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
                                HideAuthor = ParseBool(values[3]),
                            };

                            await Database.SaveAuthorAsync(ConvertTo<AuthorDatabaseModel>(author));
                            await AuthorEditViewModel.AddToStaticList(author);

                            importCount++;
                            this.AuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // importCount--;
                        // await DisplayMessage(AppStringResources.ErrorSavingAuthor, null);
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

            return 0;
        }

        /*********************** Author Methods ***********************/

        /*********************** Location Methods ***********************/
        private async Task WriteLocationsToSpreadsheet()
        {
            var tableName = AppStringResources.Locations.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("locationsOutput");

            if (this.LocationsChecked)
            {
                label.TextColor = this.busyColor;
                this.LocationsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var locationList = await FillLists.GetAllLocationsList();
                var count = locationList != null ? locationList.Count : 0;

                this.LocationsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{count}");

                await Task.Delay(1);

                var stringItems = new List<List<string?>>
                {
                    SetLocationColumns(),
                };

                if (locationList != null)
                {
                    foreach (var location in locationList)
                    {
                        var stringItem = new List<string?>
                        {
                            location.LocationGuid.ToString(),
                            location.LocationName,
                            location.HideLocation.ToString(),
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

        private async Task<int> ReadLocationsFromSpreadsheet()
        {
            var tableName = AppStringResources.Locations.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("locationsOutput");

            if (this.LocationsChecked)
            {
                label.TextColor = this.busyColor;
                this.LocationsOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                var columnNames = SetLocationColumns();

                (List<List<string>> valuesList, string message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, columnNames);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.LocationsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);

                    if (message.StartsWith("There is no spreadsheet named") ||
                        message.StartsWith("The column count is not right for") ||
                        message.StartsWith("The columns are not in the right order for"))
                    {
                        return 1;
                    }
                }

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
                                HideLocation = ParseBool(values[2]),
                            };

                            await Database.SaveLocationAsync(ConvertTo<LocationDatabaseModel>(location));
                            await LocationEditViewModel.AddToStaticList(location);

                            importCount++;
                            this.LocationsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        // importCount--;
                        // await DisplayMessage(AppStringResources.ErrorSavingLocation, null);
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

            return 0;
        }

        /*********************** Location Methods ***********************/
        /*********************** Table Methods ***********************/
    }
}
