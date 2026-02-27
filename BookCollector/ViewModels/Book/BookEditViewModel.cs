// <copyright file="BookEditViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Book
{
    using System.Collections.ObjectModel;
    using BookCollector.CustomPermissions;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.Author;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.ViewModels.Groupings;
    using BookCollector.Views.Author;
    using BookCollector.Views.Book;
    using BookCollector.Views.Collection;
    using BookCollector.Views.Genre;
    using BookCollector.Views.Location;
    using BookCollector.Views.Popups;
    using BookCollector.Views.Series;
    using CommunityToolkit.Maui.Extensions;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class BookEditViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public BookModel editedBook;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookTitleNotValid;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookFormatNotValid;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfo1SectionValue;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfo1Open;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool bookInfo1NotOpen;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool pagesReadStepperEnabled;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool timeListenedStepperEnabled;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<SeriesModel>? seriesList;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public SeriesModel? selectedSeries;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<CollectionModel>? collectionList;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public CollectionModel? selectedCollection;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<GenreModel>? genreList;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<LocationModel>? locationList;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<AuthorModel>? authorList;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<AuthorPicker>? authorPickers;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showCheckpoints;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string selectedBookFormat;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string selectedSeriesString;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string selectedCollectionString;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string selectedGenreString;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string selectedLocationString;

        public BookEditViewModel(BookModel book, ContentPage view, object? previousViewModel)
        {
            this.View = view;

            this.EditedBook = (BookModel)book.Clone();
            this.SelectedBook = book;
            var infoTextTitle = this.EditedBook.BookTitle ?? AppStringResources.ANewBook.ToLower();
            this.InfoText = $"{AppStringResources.BookEditView_InfoText.Replace("book", $"{infoTextTitle}")}";
            this.PreviousViewModel = previousViewModel;
            this.SelectedBookFormat = this.EditedBook.BookFormat ?? AppStringResources.SelectABookFormat;
            this.PopupWidth = this.DeviceWidth - 50;
            this.RefreshView = true;
        }

        public bool RemoveMainViewBefore { get; set; }

        public BookMainView? MainViewBefore { get; set; }

        public double PopupWidth { get; set; }

        public bool RefreshView { get; set; }

        private bool HiddenCollectionsOn { get; set; }

        private bool HiddenGenresOn { get; set; }

        private bool HiddenSeriesOn { get; set; }

        private bool HiddenLocationsOn { get; set; }

        private List<ChapterModel>? ChaptersToDelete { get; set; }

        private List<AuthorModel>? AuthorsToDelete { get; set; }

        private object? PreviousViewModel { get; set; }

        [RelayCommand]
        public async Task AddSeries()
        {
            var view = new SeriesEditView(new SeriesModel(), $"{AppStringResources.AddNewSeries}");
            await Shell.Current.Navigation.PushAsync(view);
            this.RefreshView = true;
        }

        [RelayCommand]
        public async Task AddCollection()
        {
            var view = new CollectionEditView(new CollectionModel(), $"{AppStringResources.AddNewCollection}");
            await Shell.Current.Navigation.PushAsync(view);
            this.RefreshView = true;
        }

        [RelayCommand]
        public async Task AddGenre()
        {
            var view = new GenreEditView(new GenreModel(), $"{AppStringResources.AddNewGenre}");
            await Shell.Current.Navigation.PushAsync(view);
            this.RefreshView = true;
        }

        [RelayCommand]
        public async Task AddLocation()
        {
            var view = new LocationEditView(new LocationModel(), $"{AppStringResources.AddNewLocation}");
            await Shell.Current.Navigation.PushAsync(view);
            this.RefreshView = true;
        }

        [RelayCommand]
        public async Task AddNewAuthor()
        {
            var view = new AuthorEditView(new AuthorModel(), $"{AppStringResources.AddNewAuthor}");
            await Shell.Current.Navigation.PushAsync(view);
            this.RefreshView = true;
        }

        public async Task SetViewModelData()
        {
            if (this.RefreshView)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    var chapters = FillLists.GetAllChaptersInBook(this.EditedBook.BookGuid);
                    var series = SeriesViewModel.SetList(this.HiddenSeriesOn);
                    var collections = CollectionsViewModel.SetList(this.HiddenCollectionsOn);
                    var genres = GenresViewModel.SetList(this.HiddenGenresOn);
                    var locations = LocationsViewModel.SetList(this.HiddenLocationsOn);
                    var authors = AuthorsViewModel.SetList(this.HiddenAuthorsOn);
                    var bookAuthorList = FillLists.GetAllBookAuthorsForBook(this.EditedBook.BookGuid);

                    this.ChaptersToDelete ??= [];
                    this.AuthorsToDelete ??= [];

                    this.BookInfo1SectionValue = true;
                    this.ReadingDataSectionValue = true;
                    this.ChapterListSectionValue = true;
                    this.AuthorListSectionValue = true;
                    this.BookInfoSectionValue = true;
                    this.SummarySectionValue = true;
                    this.CommentsSectionValue = true;

                    if (this.EditedBook.BookFormat == null || !this.EditedBook.BookFormat.Equals(AppStringResources.Audiobook))
                    {
                        this.BookIsRead = this.EditedBook.BookPageRead == this.EditedBook.BookPageTotal && this.EditedBook.BookPageTotal != 0;
                        this.ShowUpNext = this.EditedBook.BookPageRead == 0;
                        this.PagesReadStepperEnabled = this.EditedBook.BookPageTotal != 0;
                        this.ShowPages = true;
                        this.ShowTime = false;
                        this.ShowCheckpoints = this.EditedBook.BookPageTotal != 0;
                    }
                    else
                    {
                        this.ShowUpNext = this.EditedBook.BookListenedTime == 0;
                        this.ShowTime = true;
                        this.ShowPages = false;
                        this.ShowCheckpoints = this.EditedBook.BookTotalTime != 0;
                        this.BookIsRead = this.EditedBook.BookListenedTime == this.EditedBook.BookTotalTime && this.EditedBook.BookTotalTime != 0;
                        this.TimeListenedStepperEnabled = this.EditedBook.BookHoursTotal != 0 || this.EditedBook.BookMinutesTotal != 0;
                    }

                    if (!string.IsNullOrEmpty(this.EditedBook.BookCoverFileName))
                    {
                        var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                        this.EditedBook.BookCover = ImageSource.FromFile($"{directory}/{this.EditedBook.BookCoverFileName}");
                    }

                    if (!string.IsNullOrEmpty(this.EditedBook.BookCoverUrl))
                    {
                        PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                        if (internetStatus != PermissionStatus.Granted)
                        {
                            internetStatus = await Permissions.RequestAsync<InternetPermission>();
                        }

                        if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                        {
                            this.EditedBook.BookCover = new UriImageSource
                            {
                                Uri = new Uri(this.EditedBook.BookCoverUrl),
                                CachingEnabled = true,
                                CacheValidity = TimeSpan.FromDays(14),
                            };
                        }
                    }

                    this.BookCover = this.EditedBook.BookCover;

                    var loadDataTasks = new Task[]
                    {
                        Task.Run(() => this.ValidateEntry()),
                        Task.Run(() => this.EditedBook.SetBookPrice()),
                        Task.Run(() => this.EditedBook.SetBookCheckpoints(this.ShowCheckpoints)),
                        Task.Run(() => this.EditedBook.SetCoverDisplay()),
                        Task.Run(() => BookModel.SetDate(this.EditedBook.BookStartDate)),
                        Task.Run(() => BookModel.SetDate(this.EditedBook.BookEndDate)),
                        Task.Run(() => BookModel.SetDate(this.EditedBook.BookLoanedOutOn)),
                        Task.Run(() => this.BookInfo1Changed()),
                        Task.Run(() => this.ReadingDataChanged()),
                        Task.Run(() => this.ChapterListChanged()),
                        Task.Run(() => this.AuthorListChanged()),
                        Task.Run(() => this.BookInfoChanged()),
                        Task.Run(() => this.SummaryChanged()),
                        Task.Run(() => this.CommentsChanged()),
                        Task.Run(() => this.EditedBook.TotalTimeSpan = BookModel.SetTime(this.EditedBook.BookHoursTotal, this.EditedBook.BookMinutesTotal)),
                        Task.Run(() => this.EditedBook.ListenTimeSpan = BookModel.SetTime(this.EditedBook.BookHourListened, this.EditedBook.BookMinuteListened)),
                    };

                    await Task.WhenAll(chapters, series, collections, genres, locations, authors, bookAuthorList);

                    this.ChapterList = chapters.Result;
                    this.SeriesList = SeriesViewModel.filteredSeriesList1;
                    this.CollectionList = CollectionsViewModel.filteredCollectionList1;
                    this.GenreList = GenresViewModel.filteredGenreList1;
                    this.LocationList = LocationsViewModel.filteredLocationList1;
                    this.AuthorList = AuthorsViewModel.filteredAuthorList1;
                    var bookAuthors = bookAuthorList.Result;

                    this.AuthorPickers ??= [];

                    if (this.AuthorPickers.Count == 0)
                    {
                        if (bookAuthors != null && bookAuthors.Count > 0)
                        {
                            foreach (var bookAuthor in bookAuthors)
                            {
                                if (this.AuthorList != null)
                                {
                                    var author = this.AuthorList.SingleOrDefault(x => x.AuthorGuid == bookAuthor.AuthorGuid);

                                    author ??= AuthorsViewModel.fullAuthorList?.FirstOrDefault(x => x.AuthorGuid == bookAuthor.AuthorGuid);

                                    this.AuthorPickers.Add(new AuthorPicker()
                                    {
                                        AuthorList = this.AuthorList,
                                        SelectedAuthor = author,
                                        SelectedAuthorString = author?.FullName ?? AppStringResources.SelectAnAuthor,
                                    });
                                }
                            }
                        }
                    }

                    if (this.EditedBook.SelectedAuthors != null && this.EditedBook.SelectedAuthors.Count > 0)
                    {
                        foreach (var selectedAuthor in this.EditedBook.SelectedAuthors)
                        {
                            if (this.AuthorList != null && selectedAuthor != null)
                            {
                                var author = this.AuthorList.SingleOrDefault(x => x.AuthorGuid == selectedAuthor.AuthorGuid);

                                author ??= this.AuthorList.SingleOrDefault(x => x.FirstName == selectedAuthor.FirstName && x.LastName == selectedAuthor.LastName);

                                if (author == null)
                                {
                                    author = await Database.SaveAuthorAsync(ConvertTo<AuthorDatabaseModel>(selectedAuthor));
                                    await AuthorEditViewModel.AddToStaticList(author);
                                    this.AuthorList.Add(author);
                                }

                                this.AuthorPickers.Add(new AuthorPicker()
                                {
                                    AuthorList = this.AuthorList,
                                    SelectedAuthor = author,
                                    SelectedAuthorString = author?.FullName ?? AppStringResources.SelectAnAuthor,
                                });
                            }
                        }

                        foreach (var authorPicker in this.AuthorPickers)
                        {
                            if (this.EditedBook.SelectedAuthors.Any(x => x != null && x.FirstName.Equals(authorPicker.SelectedAuthor?.FirstName) && x.LastName.Equals(authorPicker.SelectedAuthor?.LastName)))
                            {
                                this.EditedBook.SelectedAuthors.RemoveAll(x => x != null && x.FirstName.Equals(authorPicker.SelectedAuthor?.FirstName) && x.LastName.Equals(authorPicker.SelectedAuthor?.LastName));
                            }
                        }
                    }

                    this.SelectedGenre = this.GenreList?.FirstOrDefault(x => x.GenreGuid == this.EditedBook.BookGenreGuid);
                    this.SelectedLocation = this.LocationList?.FirstOrDefault(x => x.LocationGuid == this.EditedBook.BookLocationGuid);
                    this.SelectedCollection = this.CollectionList?.FirstOrDefault(x => x.CollectionGuid == this.EditedBook.BookCollectionGuid);
                    this.SelectedSeries = this.SeriesList?.FirstOrDefault(x => x.SeriesGuid == this.EditedBook.BookSeriesGuid);

                    this.SelectedGenre ??= GenresViewModel.fullGenreList?.FirstOrDefault(x => x.GenreGuid == this.EditedBook.BookGenreGuid);

                    this.SelectedLocation ??= LocationsViewModel.fullLocationList?.FirstOrDefault(x => x.LocationGuid == this.EditedBook.BookLocationGuid);

                    this.SelectedCollection ??= CollectionsViewModel.fullCollectionList?.FirstOrDefault(x => x.CollectionGuid == this.EditedBook.BookCollectionGuid);

                    this.SelectedSeries ??= SeriesViewModel.fullSeriesList?.FirstOrDefault(x => x.SeriesGuid == this.EditedBook.BookSeriesGuid);

                    this.SelectedSeriesString = this.SelectedSeries?.SeriesName ?? AppStringResources.SelectASeries;
                    this.SelectedCollectionString = this.SelectedCollection?.CollectionName ?? AppStringResources.SelectACollection;
                    this.SelectedGenreString = this.SelectedGenre?.GenreName ?? AppStringResources.SelectAGenre;
                    this.SelectedLocationString = this.SelectedLocation?.LocationName ?? AppStringResources.SelectALocation;

                    await Task.WhenAll(loadDataTasks);

                    this.EditedBook.TotalTimeString = $"{this.EditedBook.BookHoursTotal:0}:{this.EditedBook.BookMinutesTotal:00}";
                    this.EditedBook.ListenTimeString = $"{this.EditedBook.BookHourListened:0}:{this.EditedBook.BookMinuteListened:00}";

                    this.SetIsBusyFalse();
                    this.RefreshView = false;
                }
                catch (Exception ex)
                {
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                    await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                    this.SetIsBusyFalse();
                    this.RefreshView = false;
                }
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            this.RefreshView = true;
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task BookSearch()
        {
            this.SetIsBusyTrue();

            var view = new BookSearchView(null, null, null, this.EditedBook, this);

            await Shell.Current.Navigation.PushModalAsync(view);
            this.SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task SaveBook()
        {
            try
            {
                if (this.BookTitleNotValid || this.BookFormatNotValid)
                {
                    if (this.BookTitleNotValid)
                    {
                        await this.DisplayMessage(AppStringResources.BookTitleNotValid, null);
                    }

                    if (this.BookFormatNotValid)
                    {
                        await this.DisplayMessage(AppStringResources.BookFormatNotValid, null);
                    }

                    this.SetIsBusyFalse();
                }
                else
                {
                    this.SetIsBusyTrue();

                    this.EditedBook.BookSeriesGuid = this.SelectedSeries?.SeriesGuid;
                    this.EditedBook.BookCollectionGuid = this.SelectedCollection?.CollectionGuid;
                    this.EditedBook.BookGenreGuid = this.SelectedGenre?.GenreGuid;
                    this.EditedBook.BookLocationGuid = this.SelectedLocation?.LocationGuid;

                    var dataTasks = new Task[]
                    {
                        Task.Run(() => this.EditedBook.SetReadingProgress()),
                        Task.Run(() => this.EditedBook.SetCoverDisplay()),
                        Task.Run(() => this.EditedBook.SetPartOfSeries()),
                        Task.Run(() => this.EditedBook.SetPartOfCollection()),
                        Task.Run(() => this.EditedBook.SetBookPrice()),
                        Task.Run(() => this.EditedBook.SetBookChapters(this.ChapterList)),
                        Task.Run(() => this.EditedBook.RemoveBookChapters(this.ChaptersToDelete)),
                    };

                    if (this.EditedBook.UpNext == true &&
                        (this.EditedBook.BookPageRead > 0 || (this.EditedBook.BookHourListened > 0 && this.EditedBook.BookMinuteListened > 0)))
                    {
                        this.EditedBook.UpNext = false;
                    }

                    if (this.AuthorsToDelete != null)
                    {
                        foreach (var author in this.AuthorsToDelete)
                        {
                            await Database.DeleteBookAuthorAsync((Guid)author.AuthorGuid, (Guid)this.EditedBook.BookGuid);
                        }
                    }

                    var authorList = new ObservableCollection<AuthorModel>();

                    if (this.AuthorPickers != null)
                    {
                        foreach (var authorPicker in this.AuthorPickers)
                        {
                            if (authorPicker.SelectedAuthor != null)
                            {
                                authorList.Add(authorPicker.SelectedAuthor);
                            }
                        }

                        await this.EditedBook.SetAuthorListString(authorList, false);
                    }

                    await Task.WhenAll(dataTasks);

#if ANDROID
                    if (Platform.CurrentActivity != null && Platform.CurrentActivity.Window != null)
                    {
                        Platform.CurrentActivity.Window.DecorView.ClearFocus();
                    }
#endif

                    this.EditedBook = ConvertTo<BookModel>(await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(this.EditedBook)));

                    foreach (var author in authorList)
                    {
                        var author1 = await Database.InsertAuthorAsync(ConvertTo<AuthorDatabaseModel>(author), this.EditedBook.BookGuid);
                        await Database.SaveAuthorAsync(ConvertTo<AuthorDatabaseModel>(author1));
                    }

                    await AddToStaticList(this.EditedBook, this.PreviousViewModel);

                    if (this.RemoveMainViewBefore)
                    {
                        Shell.Current.Navigation.RemovePage(this.MainViewBefore);
                    }

                    var view = new BookMainView(this.EditedBook, $"{this.EditedBook.BookTitle}");
                    Shell.Current.Navigation.InsertPageBefore(view, this.View);

                    await Shell.Current.Navigation.PopAsync();

                    this.SetIsBusyFalse();
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public void BookInfo1Changed()
        {
            this.BookInfo1Open = this.BookInfo1SectionValue;
            this.BookInfo1NotOpen = !this.BookInfo1SectionValue;
        }

        [RelayCommand]
        public async Task AddUploadCoverPhoto()
        {
            var action = await this.PopupMenu_CoverPhoto();

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.UploadExistingFile))
            {
                this.SetIsBusyTrue();

                PermissionStatus storageReadStatus = await Permissions.CheckStatusAsync<Permissions.Media>();

                if (storageReadStatus != PermissionStatus.Granted)
                {
                    storageReadStatus = await Permissions.RequestAsync<Permissions.Media>();
                }

                if (storageReadStatus == PermissionStatus.Granted)
                {
                    MediaPickerOptions pickerOptions = new ();

                    try
                    {
                        var photos = await MediaPicker.PickPhotosAsync(pickerOptions);

                        if (photos?.Count > 0)
                        {
                            var firstPhoto = photos.First();
                            this.BookCover = ImageSource.FromFile(firstPhoto.FullPath);
                            this.EditedBook.BookCover = ImageSource.FromFile(firstPhoto.FullPath);
                            this.EditedBook.HasBookCover = true;
                            this.EditedBook.HasNoBookCover = false;

                            var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            var fi = new FileInfo(firstPhoto.FullPath);
                            var filePath = $"{directory}/{fi.Name}";
                            File.Copy(firstPhoto.FullPath, filePath, true);

                            this.EditedBook.BookCoverFileName = fi.Name;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.SetIsBusyFalse();
                        await this.DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                    }

                    if (this.EditedBook.HasNoBookCover)
                    {
                        await this.DisplayMessage(AppStringResources.PickingCoverCanceled, null);
                    }
                }
                else
                {
                    await this.DisplayMessage(AppStringResources.PleaseAllowPhotoPermissionToAddCover, null);
                }

                this.SetIsBusyFalse();
            }

            if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.BookCoverUrl))
            {
                var result = await this.View.ShowPopupAsync<string>(new BookCoverUrlPopup(this.PopupWidth, this.EditedBook.BookCoverUrl));
                var bookCoverUrl = result.Result;

                if (!string.IsNullOrEmpty(bookCoverUrl))
                {
                    this.SetIsBusyTrue();

                    PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                    if (internetStatus != PermissionStatus.Granted)
                    {
                        internetStatus = await Permissions.RequestAsync<InternetPermission>();
                    }

                    if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        try
                        {
                            this.BookCover = new UriImageSource
                            {
                                Uri = new Uri(bookCoverUrl),
                                CachingEnabled = true,
                                CacheValidity = TimeSpan.FromDays(14),
                            };
                            this.EditedBook.BookCover = this.BookCover;
                            this.EditedBook.HasBookCover = true;
                            this.EditedBook.HasNoBookCover = false;
                            this.EditedBook.BookCoverUrl = bookCoverUrl;

                            this.SetIsBusyFalse();
                        }
                        catch (Exception ex)
                        {
                            this.SetIsBusyFalse();
                            this.BookCover = null;
                            this.EditedBook.BookCover = null;
                            this.EditedBook.HasBookCover = false;
                            this.EditedBook.HasNoBookCover = true;
                            this.EditedBook.BookCoverUrl = null;
                            await this.DisplayMessage(AppStringResources.AnErrorOccurred, AppStringResources.ErrorDownloadingImage);
                        }
                    }
                }
            }
        }

        [RelayCommand]
        public void RemoveCoverPhoto()
        {
            this.EditedBook.HasBookCover = false;
            this.EditedBook.HasNoBookCover = true;
            this.EditedBook.BookCover = null;
            this.EditedBook.BookCoverUrl = null;
            this.EditedBook.BookCoverFileName = null;
        }

        [RelayCommand]
        public void RemoveSeries()
        {
            this.SelectedSeries = null;
            this.SelectedSeriesString = this.SelectedSeries?.SeriesName ?? AppStringResources.SelectASeries;
        }

        [RelayCommand]
        public void RemoveCollection()
        {
            this.SelectedCollection = null;
            this.SelectedCollectionString = this.SelectedCollection?.CollectionName ?? AppStringResources.SelectACollection;
        }

        [RelayCommand]
        public void RemoveGenre()
        {
            this.SelectedGenre = null;
            this.SelectedGenreString = this.SelectedGenre?.GenreName ?? AppStringResources.SelectAGenre;
        }

        [RelayCommand]
        public void RemoveLocation()
        {
            this.SelectedLocation = null;
            this.SelectedLocationString = this.SelectedLocation?.LocationName ?? AppStringResources.SelectALocation;
        }

        [RelayCommand]
        public async Task UpdateProgress()
        {
            this.PagesReadStepperEnabled = this.EditedBook.BookPageTotal != 0;

            if (this.PagesReadStepperEnabled)
            {
                var stepper = (Stepper)this.View.FindByName("PageReadStepper");
                stepper.Maximum = (int)this.EditedBook.BookPageTotal;
            }

            this.TimeListenedStepperEnabled = this.EditedBook.BookTotalTime != 0;
            this.ShowCheckpoints = this.EditedBook.BookTotalTime != 0;
            await this.EditedBook.SetReadingProgress();
            await this.EditedBook.SetBookCheckpoints(this.ShowCheckpoints);
        }

        [RelayCommand]
        public async Task PagesReadPopup()
        {
            try
            {
                var pagesReadPopup = new SliderPopup(AppStringResources.PagesRead, this.PopupWidth, this.EditedBook.BookPageRead, this.EditedBook.BookPageTotal);
                var result = await this.View.ShowPopupAsync<int>(pagesReadPopup);
                this.EditedBook.BookPageRead = result.Result;
                await this.UpdateProgress();
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task TotalTimePopup()
        {
            var totalTimePopup = new TimePopup(
                AppStringResources.TotalTime,
                this.PopupWidth,
                this.EditedBook.BookHoursTotal,
                this.EditedBook.BookMinutesTotal);
            var result = await this.View.ShowPopupAsync<TimeSpan>(totalTimePopup);

            if (!result.WasDismissedByTappingOutsideOfPopup)
            {
                this.EditedBook.TotalTimeSpan = result.Result;

                this.EditedBook.BookHoursTotal = (this.EditedBook.TotalTimeSpan.Days * 24) + this.EditedBook.TotalTimeSpan.Hours;
                this.EditedBook.BookMinutesTotal = this.EditedBook.TotalTimeSpan.Minutes;
                this.EditedBook.TotalTimeString = $"{this.EditedBook.BookHoursTotal:0}:{this.EditedBook.BookMinutesTotal:00}";

                if (this.EditedBook.TotalTimeSpan < this.EditedBook.ListenTimeSpan)
                {
                    this.EditedBook.ListenTimeSpan = this.EditedBook.TotalTimeSpan;
                    this.EditedBook.BookHourListened = this.EditedBook.BookHoursTotal;
                    this.EditedBook.BookMinuteListened = this.EditedBook.BookMinutesTotal;
                }

                await this.EditedBook.SetReadingProgress();
                this.ShowCheckpoints = this.EditedBook.BookTotalTime != 0;
                await this.EditedBook.SetBookCheckpoints(this.ShowCheckpoints);
                this.BookIsRead = this.EditedBook.ListenTimeSpan == this.EditedBook.TotalTimeSpan;
            }
        }

        [RelayCommand]
        public async Task ListenTimePopup()
        {
            var listenTimePopup = new TimePopup(
                AppStringResources.ListenTime,
                this.PopupWidth,
                this.EditedBook.BookHourListened,
                this.EditedBook.BookMinuteListened,
                this.EditedBook.BookHoursTotal,
                this.EditedBook.BookMinutesTotal);
            var result = await this.View.ShowPopupAsync<TimeSpan>(listenTimePopup);

            if (!result.WasDismissedByTappingOutsideOfPopup)
            {
                this.EditedBook.ListenTimeSpan = result.Result;

                this.EditedBook.BookHourListened = (this.EditedBook.ListenTimeSpan.Days * 24) + this.EditedBook.ListenTimeSpan.Hours;
                this.EditedBook.BookMinuteListened = this.EditedBook.ListenTimeSpan.Minutes;
                this.EditedBook.ListenTimeString = $"{this.EditedBook.BookHourListened:0}:{this.EditedBook.BookMinuteListened:00}";

                await this.EditedBook.SetReadingProgress();
                this.BookIsRead = this.EditedBook.ListenTimeSpan == this.EditedBook.TotalTimeSpan;
            }
        }

        [RelayCommand]
        public async Task ReadStepperValueChange(double value)
        {
            this.EditedBook.BookPageRead = (int)value;
            await this.EditedBook.SetReadingProgress();
            this.BookIsRead = this.EditedBook.BookPageRead == this.EditedBook.BookPageTotal;
        }

        [RelayCommand]
        public void AddChapter()
        {
            this.ChapterList ??= [];

            this.ChapterList.Add(new ChapterModel());
        }

        [RelayCommand]
        public void RemoveChapter(ChapterModel chapter)
        {
            this.ChapterList?.Remove(chapter);

            this.ChaptersToDelete ??= [];
            this.ChaptersToDelete?.Add(chapter);
        }

        [RelayCommand]
        public void AddAuthor()
        {
            this.AuthorPickers?.Add(new AuthorPicker
            {
                AuthorList = this.AuthorList,
                SelectedAuthor = null,
                SelectedAuthorString = AppStringResources.SelectAnAuthor,
            });
        }

        [RelayCommand]
        public void RemoveAuthor(AuthorPicker authorPicker)
        {
            this.AuthorPickers?.Remove(authorPicker);

            if (authorPicker.SelectedAuthor != null)
            {
                this.AuthorsToDelete ??= [];
                this.AuthorsToDelete?.Add(authorPicker.SelectedAuthor);
            }
        }

        [RelayCommand]
        public async Task ReadToggle(bool value)
        {
            if (value && this.EditedBook.BookPageRead != this.EditedBook.BookPageTotal)
            {
                this.EditedBook.BookPageRead = (int)this.EditedBook.BookPageTotal;
            }

            if (value && this.EditedBook.BookListenedTime != this.EditedBook.BookTotalTime)
            {
                this.EditedBook.BookHourListened = this.EditedBook.BookHoursTotal;
                this.EditedBook.BookMinuteListened = this.EditedBook.BookMinutesTotal;
                this.EditedBook.ListenTimeSpan = this.EditedBook.TotalTimeSpan;
                this.EditedBook.BookListenedTime = this.EditedBook.BookTotalTime;
                this.EditedBook.ListenTimeString = $"{this.EditedBook.BookHourListened:0}:{this.EditedBook.BookMinuteListened:00}";
            }

            await this.EditedBook.SetReadingProgress();
        }

        [RelayCommand]
        public void UpNextToggle(bool value)
        {
            this.EditedBook.UpNext = value;
        }

        [RelayCommand]
        public void ValidateBookFormat()
        {
            this.ValidateEntry();
        }

        [RelayCommand]
        public void ValidateBookTitle()
        {
            this.ValidateEntry();
        }

        [RelayCommand]
        public async Task BookFormatChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.SelectABookFormat,
                    [.. this.BookFormats!],
                    this.EditedBook.BookFormat,
                    false);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.EditedBook.BookFormat = result.Result;
                    this.SelectedBookFormat = this.EditedBook.BookFormat ?? AppStringResources.SelectABookFormat;
                    this.ValidateBookFormat();
                }

                if (!this.SelectedBookFormat.Equals(AppStringResources.Audiobook))
                {
                    this.ShowPages = true;
                    this.ShowTime = false;
                    this.ShowCheckpoints = this.EditedBook.BookPageTotal != 0;
                }

                if (this.SelectedBookFormat.Equals(AppStringResources.Audiobook))
                {
                    this.ShowPages = false;
                    this.ShowTime = true;
                    this.ShowCheckpoints = this.EditedBook.BookTotalTime != 0 && this.EditedBook.BookTotalTime != null;
                }

                await this.UpdateProgress();
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task SeriesChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.SelectASeries,
                    [.. this.SeriesList!.Select(x => x.SeriesName!)],
                    this.SelectedSeries?.SeriesName,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.SelectedSeries = this.SeriesList!.FirstOrDefault(x => x != null && !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.Equals(result.Result));
                    this.SelectedSeriesString = this.SelectedSeries?.SeriesName ?? AppStringResources.SelectASeries;
                    this.EditedBook.BookSeriesGuid = this.SelectedSeries?.SeriesGuid;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task CollectionChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.SelectACollection,
                    [.. this.CollectionList!.Select(x => x.CollectionName!)],
                    this.SelectedCollection?.CollectionName,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.SelectedCollection = this.CollectionList!.FirstOrDefault(x => x != null && !string.IsNullOrEmpty(x.CollectionName) && x.CollectionName.Equals(result.Result));
                    this.SelectedCollectionString = this.SelectedCollection?.CollectionName ?? AppStringResources.SelectACollection;
                    this.EditedBook.BookCollectionGuid = this.SelectedCollection?.CollectionGuid;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task GenreChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.SelectAGenre,
                    [.. this.GenreList!.Select(x => x.GenreName!)],
                    this.SelectedGenre?.GenreName,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.SelectedGenre = this.GenreList!.FirstOrDefault(x => x != null && !string.IsNullOrEmpty(x.GenreName) && x.GenreName.Equals(result.Result));
                    this.SelectedGenreString = this.SelectedGenre?.GenreName ?? AppStringResources.SelectAGenre;
                    this.EditedBook.BookGenreGuid = this.SelectedGenre?.GenreGuid;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task LocationChanged()
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.SelectALocation,
                    [.. this.LocationList!.Select(x => x.LocationName!)],
                    this.SelectedLocation?.LocationName,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    this.SelectedLocation = this.LocationList!.FirstOrDefault(x => x != null && !string.IsNullOrEmpty(x.LocationName) && x.LocationName.Equals(result.Result));
                    this.SelectedLocationString = this.SelectedLocation?.LocationName ?? AppStringResources.SelectALocation;
                    this.EditedBook.BookLocationGuid = this.SelectedLocation?.LocationGuid;
                }
            }
            catch (Exception ex)
            {
            }
        }

        [RelayCommand]
        public async Task AuthorChanged(AuthorPicker? selectedAuthorPicker)
        {
            try
            {
                var filterablePopup = new FilterableListPopup(
                    AppStringResources.SelectAnAuthor,
                    [.. this.AuthorList!.Select(x => x.ReverseFullName)],
                    selectedAuthorPicker?.SelectedAuthor?.ReverseFullName,
                    true);
                var result = await this.View.ShowPopupAsync<string?>(filterablePopup);

                if (!string.IsNullOrEmpty(result.Result))
                {
                    if (selectedAuthorPicker?.SelectedAuthor != null)
                    {
                        this.AuthorsToDelete?.Add(selectedAuthorPicker.SelectedAuthor);
                    }

                    selectedAuthorPicker?.SelectedAuthor = this.AuthorList!.FirstOrDefault(x => x.ReverseFullName.Equals(result.Result));
                    selectedAuthorPicker?.SelectedAuthorString = selectedAuthorPicker.SelectedAuthor?.FullName ?? AppStringResources.SelectAnAuthor;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ValidateEntry()
        {
            this.BookTitleNotValid = string.IsNullOrEmpty(this.EditedBook.BookTitle);
            this.BookFormatNotValid = string.IsNullOrEmpty(this.EditedBook.BookFormat);
        }

        private void GetPreferences()
        {
            this.HiddenCollectionsOn = Preferences.Get("HiddenCollectionsOn", true /* Default */);
            this.HiddenGenresOn = Preferences.Get("HiddenGenresOn", true /* Default */);
            this.HiddenSeriesOn = Preferences.Get("HiddenSeriesOn", true /* Default */);
            this.HiddenLocationsOn = Preferences.Get("HiddenLocationsOn", true /* Default */);
            this.HiddenAuthorsOn = Preferences.Get("HiddenAuthorsOn", true /* Default */);
        }
    }
}
