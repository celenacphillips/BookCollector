// <copyright file="ExportImportViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Main
{
    using BookCollector.CustomPermissions;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Enums;
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

    /// <summary>
    /// ExportImportViewModel class.
    /// </summary>
    public partial class ExportImportViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the start output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? startOutput;

        /// <summary>
        /// Gets or sets the books output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? booksOutput;

        /// <summary>
        /// Gets or sets the wishlist output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? wishListOutput;

        /// <summary>
        /// Gets or sets the collections output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? collectionsOutput;

        /// <summary>
        /// Gets or sets the genres output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? genresOutput;

        /// <summary>
        /// Gets or sets the series output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? seriesOutput;

        /// <summary>
        /// Gets or sets the authors output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? authorsOutput;

        /// <summary>
        /// Gets or sets the locations output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? locationsOutput;

        /// <summary>
        /// Gets or sets the chapters output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? chaptersOutput;

        /// <summary>
        /// Gets or sets the book authors output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? bookAuthorsOutput;

        /// <summary>
        /// Gets or sets the final output string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? finalOutput;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether books is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool booksChecked;

        /// <summary>
        /// Gets or sets a value indicating whether wishlist is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool wishListChecked;

        /// <summary>
        /// Gets or sets a value indicating whether collections is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool collectionsChecked;

        /// <summary>
        /// Gets or sets a value indicating whether genres is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool genresChecked;

        /// <summary>
        /// Gets or sets a value indicating whether series is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool seriesChecked;

        /// <summary>
        /// Gets or sets a value indicating whether authors is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool authorsChecked;

        /// <summary>
        /// Gets or sets a value indicating whether locations is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool locationsChecked;

        /// <summary>
        /// Gets or sets a value indicating whether chapters is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool chaptersChecked;

        /// <summary>
        /// Gets or sets a value indicating whether book authors is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookAuthorsChecked;

        /// <summary>
        /// Gets or sets a value indicating whether images is checked or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool imagesChecked;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether checkboxes are visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool checkboxesVisible;

        /// <summary>
        /// Gets or sets a value indicating whether output is visible or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool outputVisible;

        /********************************************************/

        /// <summary>
        /// Gets or sets a value indicating whether export is enabled or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool exportEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether import is enabled or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool importEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether refresh is enabled or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool refreshEnabled;

        private readonly Color? busyColor = Application.Current?.UserAppTheme == AppTheme.Dark ?
                (Color?)Application.Current?.Resources["Tertiary"] :
                (Color?)Application.Current?.Resources["Primary"];

        private string mainFilePath = string.Empty;

        private string imageLocation = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportImportViewModel"/> class.
        /// </summary>
        /// <param name="view">View related to view model.</param>
        public ExportImportViewModel(ContentPage view)
        {
            this.View = view;
            this.InfoText = AppStringResources.ExportImportView_InfoText;
            this.SetRefreshView(true);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to refresh the view or not.
        /// </summary>
        public static bool RefreshView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to manually upload book covers or not.
        /// </summary>
        public static bool ManuallyUploadLibraryCovers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to manually upload book covers or not.
        /// </summary>
        public static bool ManuallyUploadWishlistCovers { get; set; }

        private static List<Dictionary<string, string>> singleSpreadsheetValues { get; set; }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
            if (!RefreshView)
            {
                return;
            }

            this.SetRefreshView(false);

            await this.SetIsBusyTrue();

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

            singleSpreadsheetValues = [];

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Set whether to refresh view or not.
        /// </summary>
        /// <param name="value">Value to change to.</param>
        public override void SetRefreshView(bool value)
        {
            RefreshView = value;
        }

        /// <summary>
        /// Export data from the device and store it in a spreadsheet workbook.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Export()
        {
            var action = await this.DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureExport_Question, null, null);

            if (action)
            {
                try
                {
                    var exportLocation = DevicePreferences.AppExportLocationValue;

                    if (exportLocation == null || exportLocation.Equals(AppStringResources.DefaultExportLocation))
                    {
                        var result = await FolderPicker.PickAsync(CancellationToken.None);

                        if (result != null && result.Folder != null)
                        {
                            exportLocation = result.Folder.Path;
                            Preferences.Set(DevicePreferences.AppExportLocation.ToString(), exportLocation);
                            DevicePreferences.AppExportLocationValue = exportLocation;
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

                    await this.SetIsBusyTrue();

                    this.ImportEnabled = false;
                    this.ExportEnabled = false;
                    this.RefreshEnabled = false;

                    this.ResetOutput();
                    this.OutputVisible = true;
                    this.CheckboxesVisible = false;

                    var filePath = await ReadWriteSpreadsheet.CreateSpreadsheet(exportLocation!, $"{GetDate()}-{AppInfo.Current.Name.Replace(" ", string.Empty)}{AppStringResources.Export}");
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
                    Preferences.Set(DevicePreferences.AppExportLocation.ToString(), AppStringResources.DefaultExportLocation);
                    DevicePreferences.AppExportLocationValue = AppStringResources.DefaultExportLocation;
#if DEBUG
                    await this.DisplayMessage("Error!", ex.Message);
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
                    await this.ViewModelCatch(ex);
                    this.SetRefreshView(false);
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

        /// <summary>
        /// Import data into the device from a spreadsheet workbook.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task Import()
        {
            var action = await this.DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureImport_Question, null, null);

            if (action)
            {
                try
                {
                    string[] acceptableFileTypes = [
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "application/vnd.ms-excel",
                        ];
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
                            // var version = (int)Build.VERSION.SdkInt;
                            // if (version >= 33)
                            // {
                            //    await DisplayMessage(AppStringResources.PleaseSelectTheImages, null);
                            //    var picker = ServiceHelper.GetService<IAndroidImagePicker>();
                            //    var uris = await picker.PickImagesAsync();
                            //    foreach (var uri in uris)
                            //    {
                            //        var info = this.GetImageInfo(uri);
                            //        this.selectedFiles.Add(info);
                            //    }
                            // }
#endif
                        }

                        await this.SetIsBusyTrue();

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
                            await this.DisplayMessage(AppStringResources.Caution_, AppStringResources.ImportErrors);
                        }

                        this.SetIsBusyFalse();
                        this.RefreshEnabled = true;
                    }
                }
                catch (Exception ex)
                {
                    await this.ViewModelCatch(ex);
                    this.SetRefreshView(false);
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

        private static Guid? ParseGuid(string? input)
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

        private static int ParseInt(string? input)
        {
            var parsed = int.TryParse(input, out int intParse);

            if (!parsed || intParse == 0)
            {
                double doubleParse = ParseDouble(input);
                intParse = (int)doubleParse;
            }

            return intParse;
        }

        private static double ParseDouble(string? input)
        {
            if (!string.IsNullOrEmpty(input) &&
                input.StartsWith('$'))
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

        private static bool ParseBool(string? input)
        {
            var parsed = bool.TryParse(input, out bool boolParse);

            if (!parsed)
            {
                return false;
            }

            return boolParse;
        }

        private static string? ParseString(string? input)
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

        private static Dictionary<string, string?> GetValues(List<string?> columnNames, Dictionary<string, string>? values)
        {
            var stringValues = new Dictionary<string, string?>();

            if (values != null)
            {
                for (int i = 0; i < columnNames.Count; i++)
                {
                    values.TryGetValue(columnNames[i] !.ToLower().Replace(" ", string.Empty), out var stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        stringValue = CheckOtherAppColumns(columnNames[i] !, values);
                    }

                    stringValues.Add(columnNames[i] !, stringValue?.Trim());
                }
            }

            return stringValues;
        }

        private static string? CheckOtherAppColumns(string columnName, Dictionary<string, string>? values)
        {
            var stringValue = string.Empty;

            if (values != null)
            {
                // Try to get the book title
                if (columnName.Equals($"{AppStringResources.BookTitle.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("title", out stringValue);
                }

                // Try to get the book publisher
                if (columnName.Equals($"{AppStringResources.BookPublisher.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("publication", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("publisher", out stringValue);
                    }
                }

                // Try to get the book publish year
                if (columnName.Equals($"{AppStringResources.BookPublishYear.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("date", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("publisheddate", out stringValue);
                    }
                }

                // Try to get the book format
                if (columnName.Equals($"{AppStringResources.BookFormat.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("media", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("format", out stringValue);
                    }

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        if (stringValue.ToLower().Replace(" ", string.Empty).Equals("paperbook") ||
                            stringValue.ToLower().Replace(" ", string.Empty).Equals("book") ||
                            stringValue.ToLower().Replace(" ", string.Empty).Equals("boardbook"))
                        {
                            stringValue = AppStringResources.Paperback;
                        }

                        if (stringValue.ToLower().Replace(" ", string.Empty).Equals("digitalaudiobook"))
                        {
                            stringValue = AppStringResources.Audiobook;
                        }

                        if (stringValue.ToLower().Replace(" ", string.Empty).Equals("ebook"))
                        {
                            stringValue = AppStringResources.eBook;
                        }
                    }
                }

                // Try to get the book comments
                if (columnName.Equals($"{AppStringResources.BookComments.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("comment", out stringValue);
                }

                // Try to get the book summary
                if (columnName.Equals($"{AppStringResources.BookSummary.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("summary", out stringValue);
                }

                // Try to get the book page count
                if (columnName.Equals($"{AppStringResources.TotalPages.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("pagecount", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("pages", out stringValue);
                    }
                }

                // Try to get the book page count read
                if (columnName.Equals($"{AppStringResources.PagesRead.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("pageread", out stringValue);
                }

                // Try to get the book number
                if (columnName.Equals($"{AppStringResources.BookNumber.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("volume", out stringValue);
                }

                // Try to get the book start date
                if (columnName.Equals($"{AppStringResources.ReadingStartDate.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("datestarted", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("startedreadingdate", out stringValue);
                    }
                }

                // Try to get the book end date
                if (columnName.Equals($"{AppStringResources.ReadingEndDate.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("dateread", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("finishedreadingdate", out stringValue);
                    }
                }

                // Try to get the book identifier / isbn
                if (columnName.Equals($"{AppStringResources.BookIdentifier.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("isbn", out stringValue);

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        stringValue = stringValue.Replace("[", string.Empty).Replace("]", string.Empty);
                    }
                }

                // Try to get the book price
                if (columnName.Equals($"{AppStringResources.OriginalBookPrice.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("listprice", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("price", out stringValue);
                    }

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("bookprice", out stringValue);
                    }
                }

                // Try to get the book language
                if (columnName.Equals($"{AppStringResources.BookLanguage.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("languages", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("language", out stringValue);
                    }

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        var languages = stringValue.Split(',');
                        stringValue = languages[0];
                    }
                }

                // Try to get the book rating
                if (columnName.Equals($"{AppStringResources.BookRating.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("rating", out stringValue);
                }

                // Try to get the book url
                if (columnName.Equals($"{AppStringResources.BookURL.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("itemurl", out stringValue);
                }

                // Try to get the book cover url
                if (columnName.Equals($"{AppStringResources.BookCoverUrl.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("imageurl", out stringValue);
                }

                // Try to get the date the book was obtained
                if (columnName.Equals($"{AppStringResources.DateBookObtained.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("acquired", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("addeddate", out stringValue);
                    }
                }

                // Try to get the book location
                if (columnName.Equals($"{AppStringResources.LocationName.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("location", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("bookshelf", out stringValue);
                    }
                }

                // TO DO:
                // Figure out how to get multiple collections, genres, series, and authors of book.
                // Needs to be pulled from one book record and made into multiple object records.

                // Try to get the book collection
                if (columnName.Equals($"{AppStringResources.CollectionName.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("collections", out stringValue);

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        var collections = stringValue.Split(',');
                        stringValue = collections[0];
                    }
                }

                // Try to get the book genre
                if (columnName.Equals($"{AppStringResources.GenreName.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("tags", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("genres", out stringValue);
                    }

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        var genres = stringValue.Split(',');
                        stringValue = genres[0];
                    }
                }

                // Try to get the book series
                if (columnName.Equals($"{AppStringResources.SeriesName.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("series", out stringValue);
                }

                // Try to get the first book author's first name
                if (columnName.Equals($"{AppStringResources.FirstName.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("primaryauthor", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("author", out stringValue);
                    }

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        if (stringValue.Contains('|'))
                        {
                            var authorList = stringValue.Split('|');
                            var authorName = authorList[0].Split(',');
                            stringValue = authorName[1];
                        }
                        else if (stringValue.Contains(';'))
                        {
                            var authorList = stringValue.Split(';');
                            var authorName = authorList[0].Split(',');
                            stringValue = authorName[1];
                        }
                        else
                        {
                            var authorName = stringValue.Split(',');
                            stringValue = authorName[1];
                        }
                    }
                }

                // Try to get the first book author's last name
                if (columnName.Equals($"{AppStringResources.LastName.Replace(" ", string.Empty)}"))
                {
                    values.TryGetValue("primaryauthor", out stringValue);

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        values.TryGetValue("author", out stringValue);
                    }

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        if (stringValue.Contains('|'))
                        {
                            var authorList = stringValue.Split('|');
                            var authorName = authorList[0].Split(',');
                            stringValue = authorName[0];
                        }
                        else if (stringValue.Contains(';'))
                        {
                            var authorList = stringValue.Split(';');
                            var authorName = authorList[0].Split(',');
                            stringValue = authorName[0];
                        }
                        else
                        {
                            var authorName = stringValue.Split(',');
                            stringValue = authorName[0];
                        }
                    }
                }
            }

            return stringValue;
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
                $"{AppStringResources.OriginalBookPrice.Replace(" ", string.Empty)}",
                $"{AppStringResources.PaidBookPrice.Replace(" ", string.Empty)}",
                $"{AppStringResources.DateBookObtained.Replace(" ", string.Empty)}",
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
                $"{AppStringResources.UpNext.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookLoanedTo.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookLoanedOutOn.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookBorrowedFrom.Replace(" ", string.Empty)}",
                $"{AppStringResources.BookBorrowedOn.Replace(" ", string.Empty)}",
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
                $"{AppStringResources.OriginalBookPrice.Replace(" ", string.Empty)}",
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
                $"{AppStringResources.Favorite.Replace(" ", string.Empty)}",
            ];
        }

        private static List<string?> SetGenreColumns()
        {
            return
            [
                $"{AppStringResources.GenreGuid}",
                $"{AppStringResources.GenreName.Replace(" ", string.Empty)}",
                $"{AppStringResources.Hide_Question}",
                $"{AppStringResources.Favorite.Replace(" ", string.Empty)}",
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
                $"{AppStringResources.Favorite.Replace(" ", string.Empty)}",
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
                $"{AppStringResources.Favorite.Replace(" ", string.Empty)}",
            ];
        }

        private static List<string?> SetLocationColumns()
        {
            return
            [
                $"{AppStringResources.LocationGuid}",
                $"{AppStringResources.LocationName.Replace(" ", string.Empty)}",
                $"{AppStringResources.Hide_Question}",
                $"{AppStringResources.Favorite.Replace(" ", string.Empty)}",
            ];
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

        private async Task ManuallySetBookCover(BookModel book, bool imagesChecked)
        {
            if (imagesChecked && ManuallyUploadLibraryCovers)
            {
                var answer = await this.View.ShowPopupAsync<string>(new MissingBookCoverPopup(book.BookTitle!));

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

                            var result = await this.View.ShowPopupAsync<bool>(new BookCoverMatchingPopup(bookCoverImageSource, book.BookTitle!));

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
                var answer = await this.View.ShowPopupAsync<string>(new MissingBookCoverPopup(book.BookTitle!));

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
                            book.BookPricePaid,
                            book.ObtainedDate,
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
                            book.UpNext.ToString(),
                            book.LoanedTo,
                            book.BookLoanedOutOn,
                            book.BorrowedFrom,
                            book.BookBorrowedOn,
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

                var valuesList = new List<Dictionary<string, string>>();
                var message = string.Empty;

                if (singleSpreadsheetValues == null || singleSpreadsheetValues.Count == 0)
                {
                    (valuesList, message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                    if (!message.Contains(tableName))
                    {
                        singleSpreadsheetValues = valuesList;
                    }
                }
                else
                {
                    valuesList = singleSpreadsheetValues;
                }

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.BooksOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);
                }

                this.BooksOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList!.Count}");

                await Task.Delay(1);

                var columnNames = SetBookColumns();

                foreach (var values in valuesList)
                {
                    try
                    {
                        var parsedValues = GetValues(columnNames, values);

                        parsedValues = parsedValues.Distinct().ToDictionary();

                        parsedValues.TryGetValue(columnNames[1] !, out var title);

                        if (parsedValues.Count > 0 &&
                            !string.IsNullOrEmpty(title))
                        {
                            parsedValues.TryGetValue(columnNames[0] !, out var guid);
                            parsedValues.TryGetValue(columnNames[2] !, out var seriesGuid);
                            parsedValues.TryGetValue(columnNames[3] !, out var bookNumber);
                            parsedValues.TryGetValue(columnNames[4] !, out var publisher);
                            parsedValues.TryGetValue(columnNames[5] !, out var publishYear);
                            parsedValues.TryGetValue(columnNames[6] !, out var identifier);
                            parsedValues.TryGetValue(columnNames[7] !, out var format);
                            parsedValues.TryGetValue(columnNames[8] !, out var language);
                            parsedValues.TryGetValue(columnNames[9] !, out var originalPrice);
                            parsedValues.TryGetValue(columnNames[10] !, out var paidPrice);
                            parsedValues.TryGetValue(columnNames[11] !, out var obtainedDate);
                            parsedValues.TryGetValue(columnNames[12] !, out var summary);
                            parsedValues.TryGetValue(columnNames[13] !, out var pagesRead);
                            parsedValues.TryGetValue(columnNames[14] !, out var totalPages);
                            parsedValues.TryGetValue(columnNames[15] !, out var listenHours);
                            parsedValues.TryGetValue(columnNames[16] !, out var listenMinutes);
                            parsedValues.TryGetValue(columnNames[17] !, out var totalHours);
                            parsedValues.TryGetValue(columnNames[18] !, out var totalMinutes);
                            parsedValues.TryGetValue(columnNames[19] !, out var startDate);
                            parsedValues.TryGetValue(columnNames[20] !, out var endDate);
                            parsedValues.TryGetValue(columnNames[21] !, out var locationGuid);
                            parsedValues.TryGetValue(columnNames[22] !, out var comments);
                            parsedValues.TryGetValue(columnNames[23] !, out var collectionGuid);
                            parsedValues.TryGetValue(columnNames[24] !, out var genreGuid);
                            parsedValues.TryGetValue(columnNames[25] !, out var bookUrl);
                            parsedValues.TryGetValue(columnNames[26] !, out var rating);
                            parsedValues.TryGetValue(columnNames[27] !, out var favorite);
                            parsedValues.TryGetValue(columnNames[28] !, out var upNext);
                            parsedValues.TryGetValue(columnNames[29] !, out var loanedTo);
                            parsedValues.TryGetValue(columnNames[30] !, out var loanedOn);
                            parsedValues.TryGetValue(columnNames[31] !, out var borrowedFrom);
                            parsedValues.TryGetValue(columnNames[32] !, out var borrowedOn);
                            parsedValues.TryGetValue(columnNames[33] !, out var hide);
                            parsedValues.TryGetValue(columnNames[34] !, out var bookCoverUrl);
                            parsedValues.TryGetValue(columnNames[35] !, out var bookFileName);

                            var book = new BookModel()
                            {
                                BookGuid = ParseGuid(guid),
                                BookTitle = title,
                                BookSeriesGuid = ParseGuid(seriesGuid),
                                BookNumberInSeries = bookNumber,
                                BookPublisher = publisher,
                                BookPublishYear = publishYear,
                                BookIdentifier = identifier,
                                BookFormat = format,
                                BookLanguage = language,
                                BookPrice = originalPrice,
                                BookPricePaid = paidPrice,
                                ObtainedDate = ParseString(obtainedDate),
                                BookSummary = summary,
                                BookPageRead = ParseInt(pagesRead),
                                BookPageTotal = ParseInt(totalPages),
                                BookHourListened = ParseInt(listenHours),
                                BookMinuteListened = ParseInt(listenMinutes),
                                BookHoursTotal = ParseInt(totalHours),
                                BookMinutesTotal = ParseInt(totalMinutes),
                                BookStartDate = ParseString(startDate),
                                BookEndDate = ParseString(endDate),
                                BookLocationGuid = ParseGuid(locationGuid),
                                BookComments = comments,
                                BookCollectionGuid = ParseGuid(collectionGuid),
                                BookGenreGuid = ParseGuid(genreGuid),
                                BookURL = bookCoverUrl,
                                Rating = ParseInt(rating),
                                IsFavorite = ParseBool(favorite),
                                UpNext = ParseBool(upNext),
                                LoanedTo = loanedTo,
                                BookLoanedOutOn = ParseString(loanedOn),
                                BorrowedFrom = borrowedFrom,
                                BookBorrowedOn = ParseString(borrowedOn),
                                HideBook = ParseBool(hide),
                                BookCoverUrl = bookCoverUrl,
                                BookCoverFileName = bookFileName,
                            };

                            if (singleSpreadsheetValues != null)
                            {
                                var newValues = GetValues(
                                    [AppStringResources.CollectionName.Replace(" ", string.Empty),
                                    AppStringResources.LocationName.Replace(" ", string.Empty),
                                    AppStringResources.GenreName.Replace(" ", string.Empty),
                                    AppStringResources.SeriesName.Replace(" ", string.Empty),
                                    ], values);

                                newValues.TryGetValue(AppStringResources.CollectionName.Replace(" ", string.Empty), out var collectionName);
                                newValues.TryGetValue(AppStringResources.LocationName.Replace(" ", string.Empty), out var locationName);
                                newValues.TryGetValue(AppStringResources.GenreName.Replace(" ", string.Empty), out var genreName);
                                newValues.TryGetValue(AppStringResources.SeriesName.Replace(" ", string.Empty), out var seriesName);

                                if (!string.IsNullOrEmpty(collectionName))
                                {
                                    var collection = CollectionsViewModel.fullCollectionList?.FirstOrDefault(c => c.CollectionName!.Equals(collectionName));
                                    book.BookCollectionGuid = collection?.CollectionGuid;
                                }

                                if (!string.IsNullOrEmpty(locationName))
                                {
                                    var location = LocationsViewModel.fullLocationList?.FirstOrDefault(l => l.LocationName!.Equals(locationName));
                                    book.BookLocationGuid = location?.LocationGuid;
                                }

                                if (!string.IsNullOrEmpty(genreName))
                                {
                                    var genre = GenresViewModel.fullGenreList?.FirstOrDefault(g => g.GenreName!.Equals(genreName));
                                    book.BookGenreGuid = genre?.GenreGuid;
                                }

                                if (!string.IsNullOrEmpty(seriesName))
                                {
                                    var series = SeriesViewModel.fullSeriesList?.FirstOrDefault(s => s.SeriesName!.Equals(seriesName));
                                    book.BookSeriesGuid = series?.SeriesGuid;
                                }
                            }

                            if (!string.IsNullOrEmpty(book.BookStartDate) &&
                                !DateTime.TryParse(book.BookStartDate, out var startDateValue))
                            {
                                book.BookStartDate = book.BookStartDate.Trim()[..4];

                                var parsed = DateTime.TryParse($"{book.BookStartDate}-01", out startDateValue);

                                book.BookStartDate = startDateValue.ToShortDateString();
                            }

                            if (!string.IsNullOrEmpty(book.BookEndDate) &&
                                !DateTime.TryParse(book.BookEndDate, out var endDateValue))
                            {
                                book.BookEndDate = book.BookEndDate.Trim()[..4];

                                var parsed = DateTime.TryParse($"{book.BookEndDate}-01", out endDateValue);

                                book.BookEndDate = endDateValue.ToShortDateString();
                            }

                            if (book.BookPageRead > 0 && book.BookPageTotal <= 0)
                            {
                                book.BookPageRead = 0;
                            }

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

                            if (string.IsNullOrEmpty(book.BookCoverUrl) && string.IsNullOrEmpty(book.BookCoverFileName))
                            {
                                book.HasBookCover = false;
                                book.HasNoBookCover = true;
                            }

                            if (book.BookPublishYear != null && book.BookPublishYear.Trim().Length > 4)
                            {
                                book.BookPublishYear = book.BookPublishYear.Trim()[.. 4];
                            }

                            book = await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
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
            var tableName = AppStringResources.Wishlist.Replace(" ", string.Empty);
            var label = (Label)this.View.FindByName("wishListOutput");

            if (this.WishListChecked)
            {
                label.TextColor = this.busyColor;
                this.WishListOutput = AppStringResources.Table_GettingItems.Replace("Table", tableName);

                await Task.Delay(1);

                (var valuesList, var message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, this.WishListChecked && this.BooksChecked);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.WishListOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);
                }

                this.WishListOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList!.Count}");

                await Task.Delay(1);

                var columnNames = SetWishlistBookColumns();

                foreach (var values in valuesList)
                {
                    try
                    {
                        var parsedValues = GetValues(columnNames, values);

                        parsedValues = parsedValues.Distinct().ToDictionary();

                        parsedValues.TryGetValue(columnNames[1]!, out var title);

                        if (parsedValues.Count > 0 &&
                            !string.IsNullOrEmpty(title))
                        {
                            parsedValues.TryGetValue(columnNames[0]!, out var guid);
                            parsedValues.TryGetValue(columnNames[2]!, out var authors);
                            parsedValues.TryGetValue(columnNames[3]!, out var seriesName);
                            parsedValues.TryGetValue(columnNames[4]!, out var bookNumber);
                            parsedValues.TryGetValue(columnNames[5]!, out var publisher);
                            parsedValues.TryGetValue(columnNames[6]!, out var publishYear);
                            parsedValues.TryGetValue(columnNames[7]!, out var identifier);
                            parsedValues.TryGetValue(columnNames[8]!, out var format);
                            parsedValues.TryGetValue(columnNames[9]!, out var language);
                            parsedValues.TryGetValue(columnNames[10]!, out var originalPrice);
                            parsedValues.TryGetValue(columnNames[11]!, out var summary);
                            parsedValues.TryGetValue(columnNames[12]!, out var totalPages);
                            parsedValues.TryGetValue(columnNames[13]!, out var totalHours);
                            parsedValues.TryGetValue(columnNames[14]!, out var totalMinutes);
                            parsedValues.TryGetValue(columnNames[15]!, out var comments);
                            parsedValues.TryGetValue(columnNames[16]!, out var locationName);
                            parsedValues.TryGetValue(columnNames[17]!, out var hide);
                            parsedValues.TryGetValue(columnNames[18]!, out var bookUrl);
                            parsedValues.TryGetValue(columnNames[19]!, out var bookCoverUrl);
                            parsedValues.TryGetValue(columnNames[20]!, out var bookFileName);

                            var book = new WishlistBookModel()
                            {
                                BookGuid = ParseGuid(guid),
                                BookTitle = title,
                                AuthorListString = authors,
                                BookSeries = seriesName,
                                BookNumberInSeries = bookNumber,
                                BookPublisher = publisher,
                                BookPublishYear = publishYear,
                                BookIdentifier = identifier,
                                BookFormat = format,
                                BookLanguage = language,
                                BookPrice = originalPrice,
                                BookSummary = summary,
                                BookPageTotal = ParseInt(totalPages),
                                BookHoursTotal = ParseInt(totalHours),
                                BookMinutesTotal = ParseInt(totalMinutes),
                                BookComments = comments,
                                BookWhereToBuy = locationName,
                                HideBook = ParseBool(hide),
                                BookURL = bookUrl,
                                BookCoverUrl = bookCoverUrl,
                                BookCoverFileName = bookFileName,
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

                            book = await Database.SaveWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(book));
                            await WishListBookEditViewModel.AddToStaticList(book);

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

                (var valuesList, var message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName, true);

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.ChaptersOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);
                }

                this.ChaptersOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList!.Count}");

                await Task.Delay(1);

                var columnNames = SetChapterColumns();

                foreach (var values in valuesList)
                {
                    try
                    {
                        var parsedValues = GetValues(columnNames, values);

                        if (parsedValues.Count > 0)
                        {
                            parsedValues.TryGetValue(columnNames[0]!, out var guid);
                            parsedValues.TryGetValue(columnNames[1]!, out var name);
                            parsedValues.TryGetValue(columnNames[2]!, out var pageRange);
                            parsedValues.TryGetValue(columnNames[3]!, out var chapterOrder);
                            parsedValues.TryGetValue(columnNames[4]!, out var bookGuid);

                            var chapter = new ChapterModel()
                            {
                                ChapterGuid = ParseGuid(guid),
                                ChapterName = name,
                                PageRange = pageRange,
                                ChapterOrder = ParseInt(chapterOrder),
                                BookGuid = ParseGuid(bookGuid) !.Value,
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
                            collection.IsFavorite.ToString(),
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

                var valuesList = new List<Dictionary<string, string>>();
                var message = string.Empty;

                if (singleSpreadsheetValues == null || singleSpreadsheetValues.Count == 0)
                {
                    (valuesList, message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                    if (!message.Contains(tableName))
                    {
                        singleSpreadsheetValues = valuesList;
                    }
                }
                else
                {
                    valuesList = singleSpreadsheetValues;
                }

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.CollectionsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);
                }

                this.CollectionsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList!.Count}");

                await Task.Delay(1);

                var columnNames = SetCollectionColumns();

                foreach (var values in valuesList)
                {
                    try
                    {
                        var parsedValues = GetValues(columnNames, values);

                        parsedValues = parsedValues.Distinct().ToDictionary();

                        parsedValues.TryGetValue(columnNames[1]!, out var name);

                        if (parsedValues.Count > 0 &&
                            !string.IsNullOrEmpty(name))
                        {
                            if (CollectionsViewModel.fullCollectionList != null &&
                                CollectionsViewModel.fullCollectionList.Any(x => x.CollectionName!.Equals(name)))
                            {
                                continue;
                            }

                            parsedValues.TryGetValue(columnNames[0]!, out var guid);
                            parsedValues.TryGetValue(columnNames[2]!, out var hide);
                            parsedValues.TryGetValue(columnNames[3]!, out var favorite);

                            var collection = new CollectionModel()
                            {
                                CollectionGuid = ParseGuid(guid),
                                CollectionName = name,
                                HideCollection = ParseBool(hide),
                                IsFavorite = ParseBool(favorite),
                            };

                            collection = await Database.SaveCollectionAsync(ConvertTo<CollectionDatabaseModel>(collection));
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
                            genre.IsFavorite.ToString(),
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

                var valuesList = new List<Dictionary<string, string>>();
                var message = string.Empty;

                if (singleSpreadsheetValues == null || singleSpreadsheetValues.Count == 0)
                {
                    (valuesList, message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                    if (!message.Contains(tableName))
                    {
                        singleSpreadsheetValues = valuesList;
                    }
                }
                else
                {
                    valuesList = singleSpreadsheetValues;
                }

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.GenresOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);
                }

                this.GenresOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList!.Count}");

                await Task.Delay(1);

                var columnNames = SetGenreColumns();

                foreach (var values in valuesList)
                {
                    try
                    {
                        var parsedValues = GetValues(columnNames, values);

                        parsedValues = parsedValues.Distinct().ToDictionary();

                        parsedValues.TryGetValue(columnNames[1]!, out var name);

                        if (parsedValues.Count > 0 &&
                            !string.IsNullOrEmpty(name))
                        {
                            if (GenresViewModel.fullGenreList != null &&
                                GenresViewModel.fullGenreList.Any(x => x.GenreName!.Equals(name)))
                            {
                                continue;
                            }

                            parsedValues.TryGetValue(columnNames[0]!, out var guid);
                            parsedValues.TryGetValue(columnNames[2]!, out var hide);
                            parsedValues.TryGetValue(columnNames[3]!, out var favorite);

                            var genre = new GenreModel()
                            {
                                GenreGuid = ParseGuid(guid),
                                GenreName = name,
                                HideGenre = ParseBool(hide),
                                IsFavorite = ParseBool(favorite),
                            };

                            genre = await Database.SaveGenreAsync(ConvertTo<GenreDatabaseModel>(genre));
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
                            series.IsFavorite.ToString(),
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

                var valuesList = new List<Dictionary<string, string>>();
                var message = string.Empty;

                if (singleSpreadsheetValues == null || singleSpreadsheetValues.Count == 0)
                {
                    (valuesList, message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                    if (!message.Contains(tableName))
                    {
                        singleSpreadsheetValues = valuesList;
                    }
                }
                else
                {
                    valuesList = singleSpreadsheetValues;
                }

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.SeriesOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);
                }

                this.SeriesOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList!.Count}");

                await Task.Delay(1);

                var columnNames = SetSeriesColumns();

                foreach (var values in valuesList)
                {
                    try
                    {
                        var parsedValues = GetValues(columnNames, values);

                        parsedValues = parsedValues.Distinct().ToDictionary();

                        parsedValues.TryGetValue(columnNames[1]!, out var name);

                        if (parsedValues.Count > 0 &&
                            !string.IsNullOrEmpty(name))
                        {
                            if (SeriesViewModel.fullSeriesList != null &&
                                SeriesViewModel.fullSeriesList.Any(x => x.SeriesName!.Equals(name)))
                            {
                                continue;
                            }

                            parsedValues.TryGetValue(columnNames[0]!, out var guid);
                            parsedValues.TryGetValue(columnNames[2]!, out var totalBooks);
                            parsedValues.TryGetValue(columnNames[3]!, out var hide);
                            parsedValues.TryGetValue(columnNames[4]!, out var favorite);

                            var series = new SeriesModel()
                            {
                                SeriesGuid = ParseGuid(guid),
                                SeriesName = name,
                                TotalBooksInSeries = totalBooks,
                                HideSeries = ParseBool(hide),
                                IsFavorite = ParseBool(favorite),
                            };

                            series = await Database.SaveSeriesAsync(ConvertTo<SeriesDatabaseModel>(series));
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

                var valuesList = new List<Dictionary<string, string>>();
                var message = string.Empty;

                if (singleSpreadsheetValues == null || singleSpreadsheetValues.Count == 0)
                {
                    (valuesList, message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                    if (!message.Contains(tableName))
                    {
                        singleSpreadsheetValues = valuesList;
                    }
                }
                else
                {
                    valuesList = singleSpreadsheetValues;
                }

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.BookAuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);
                }

                this.BookAuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList!.Count}");

                await Task.Delay(1);

                var columnNames = SetBookAuthorColumns();

                foreach (var values in valuesList)
                {
                    try
                    {
                        var parsedValues = GetValues(columnNames, values);

                        parsedValues.TryGetValue(columnNames[0] !, out var guid);
                        parsedValues.TryGetValue(columnNames[1] !, out var authorGuid);
                        parsedValues.TryGetValue(columnNames[2] !, out var bookGuid);

                        if (singleSpreadsheetValues != null)
                        {
                            var newValues = GetValues(
                                [AppStringResources.BookTitle.Replace(" ", string.Empty),
                                AppStringResources.FirstName.Replace(" ", string.Empty),
                                AppStringResources.LastName.Replace(" ", string.Empty)
                                ], values);

                            newValues.TryGetValue(AppStringResources.BookTitle.Replace(" ", string.Empty), out var bookTitle);
                            newValues.TryGetValue(AppStringResources.FirstName.Replace(" ", string.Empty), out var firstName);
                            newValues.TryGetValue(AppStringResources.LastName.Replace(" ", string.Empty), out var lastName);

                            if (newValues != null &&
                                !string.IsNullOrEmpty(bookTitle) &&
                                !string.IsNullOrEmpty(firstName) &&
                                !string.IsNullOrEmpty(lastName))
                            {
                                var book = AllBooksViewModel.fullBookList?.FirstOrDefault(b => b.BookTitle!.Equals(bookTitle));
                                var author = AuthorsViewModel.fullAuthorList?.FirstOrDefault(a => a.FirstName!.Equals(firstName) && a.LastName!.Equals(lastName));

                                authorGuid = author?.AuthorGuid.ToString();
                                bookGuid = book?.BookGuid.ToString();
                            }
                        }

                        if (parsedValues.Count > 0 &&
                            !string.IsNullOrEmpty(authorGuid) &&
                            !string.IsNullOrEmpty(bookGuid))
                        {
                            var bookAuthor = new BookAuthorModel()
                            {
                                BookAuthorGuid = ParseGuid(guid),
                                AuthorGuid = ParseGuid(authorGuid) !.Value,
                                BookGuid = ParseGuid(bookGuid) !.Value,
                            };

                            await Database.SaveBookAuthorAsync(bookAuthor);
                            await AuthorEditViewModel.AddToStaticList(bookAuthor);

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
                            author.IsFavorite.ToString(),
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

                var valuesList = new List<Dictionary<string, string>>();
                var message = string.Empty;

                if (singleSpreadsheetValues == null || singleSpreadsheetValues.Count == 0)
                {
                    (valuesList, message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                    if (!message.Contains(tableName))
                    {
                        singleSpreadsheetValues = valuesList;
                    }
                }
                else
                {
                    valuesList = singleSpreadsheetValues;
                }

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.AuthorsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);
                }

                this.AuthorsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList!.Count}");

                await Task.Delay(1);

                var columnNames = SetAuthorColumns();

                foreach (var values in valuesList)
                {
                    try
                    {
                        var parsedValues = GetValues(columnNames, values);

                        parsedValues = parsedValues.Distinct().ToDictionary();

                        parsedValues.TryGetValue(columnNames[1]!, out var firstName);
                        parsedValues.TryGetValue(columnNames[2]!, out var lastName);

                        if (parsedValues.Count > 0 &&
                            !string.IsNullOrEmpty(firstName) &&
                            !string.IsNullOrEmpty(lastName))
                        {
                            parsedValues.TryGetValue(columnNames[0]!, out var guid);
                            parsedValues.TryGetValue(columnNames[3]!, out var hide);
                            parsedValues.TryGetValue(columnNames[4]!, out var favorite);

                            var author = new AuthorModel()
                            {
                                AuthorGuid = ParseGuid(guid),
                                FirstName = firstName !,
                                LastName = lastName !,
                                HideAuthor = ParseBool(hide),
                                IsFavorite = ParseBool(favorite),
                            };

                            author = await Database.SaveAuthorAsync(ConvertTo<AuthorDatabaseModel>(author));
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
                            location.IsFavorite.ToString(),
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

                var valuesList = new List<Dictionary<string, string>>();
                var message = string.Empty;

                if (singleSpreadsheetValues == null || singleSpreadsheetValues.Count == 0)
                {
                    (valuesList, message) = ReadWriteSpreadsheet.ReadSpreadSheet(this.mainFilePath, tableName);

                    if (!message.Contains(tableName))
                    {
                        singleSpreadsheetValues = valuesList;
                    }
                }
                else
                {
                    valuesList = singleSpreadsheetValues;
                }

                var importCount = 0;

                if (valuesList == null || valuesList.Count == 0)
                {
                    this.LocationsOutput = AppStringResources.Table_XImported.Replace("Table", tableName).Replace("x", $"{importCount}").Replace("z", $"{valuesList?.Count}");
                    label.TextColor = Application.Current?.UserAppTheme == AppTheme.Dark ? (Color?)Application.Current?.Resources["TextDark"] : (Color?)Application.Current?.Resources["TextLight"];
                    await Task.Delay(1);
                }

                this.LocationsOutput = AppStringResources.Table_XRetrieved.Replace("Table", tableName).Replace("x", $"{valuesList!.Count}");

                await Task.Delay(1);

                var columnNames = SetLocationColumns();

                foreach (var values in valuesList)
                {
                    try
                    {
                        var parsedValues = GetValues(columnNames, values);

                        parsedValues = parsedValues.Distinct().ToDictionary();

                        parsedValues.TryGetValue(columnNames[1]!, out var name);

                        if (parsedValues.Count > 0 &&
                            !string.IsNullOrEmpty(name))
                        {
                            if (LocationsViewModel.fullLocationList != null &&
                                LocationsViewModel.fullLocationList.Any(x => x.LocationName!.Equals(name)))
                            {
                                continue;
                            }

                            parsedValues.TryGetValue(columnNames[0]!, out var guid);
                            parsedValues.TryGetValue(columnNames[2]!, out var hide);
                            parsedValues.TryGetValue(columnNames[3]!, out var favorite);

                            var location = new LocationModel()
                            {
                                LocationGuid = ParseGuid(guid),
                                LocationName = name,
                                HideLocation = ParseBool(hide),
                                IsFavorite = ParseBool(favorite),
                            };

                            location = await Database.SaveLocationAsync(ConvertTo<LocationDatabaseModel>(location));
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
