// <copyright file="BookMainViewModel.cs" company="Castle Software">
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
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.Views.Book;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// BookMainViewModel class.
    /// </summary>
    public partial class BookMainViewModel : BookBaseViewModel
    {
        /// <summary>
        /// Gets or sets the author list.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<AuthorModel>? authorList;

        /// <summary>
        /// Gets or sets the selected author.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public ObservableCollection<AuthorModel>? selectedAuthorList;

        /// <summary>
        /// Gets or sets a value indicating whether to show checkpoints or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool showCheckpoints;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookMainViewModel"/> class.
        /// </summary>
        /// <param name="book">Book to view.</param>
        /// <param name="view">View related to view model.</param>
        /// <param name="previousViewModel">Previous view model to return to.</param>
        public BookMainViewModel(BookModel book, ContentPage view, object? previousViewModel)
        {
            this.View = view;

            this.SelectedBook = book;
            this.InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book", $"{this.SelectedBook.BookTitle}")}";
            this.PreviousViewModel = previousViewModel;
        }

        /// <summary>
        /// Gets or sets previous view model to return to after closing the popup or saving the book.
        /// </summary>
        private object? PreviousViewModel { get; set; }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
        {
            if (this.SelectedBook != null)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.GetPreferences();

                    var chapters = FillLists.GetAllChaptersInBook(this.SelectedBook.BookGuid);
                    var genre = GetItems.GetGenreForBook(this.SelectedBook.BookGenreGuid);
                    var location = GetItems.GetLocationForBook(this.SelectedBook.BookLocationGuid);
                    var authors = FillLists.GetAllAuthorsForBook(this.SelectedBook.BookGuid);

                    this.ReadingDataSectionValue = true;
                    this.ChapterListSectionValue = true;
                    this.AuthorListSectionValue = true;
                    this.BookInfoSectionValue = true;
                    this.SummarySectionValue = true;
                    this.CommentsSectionValue = true;

                    if (!this.SelectedBook.BookFormat!.Equals(AppStringResources.Audiobook))
                    {
                        this.BookIsRead = this.SelectedBook.BookPageRead == this.SelectedBook.BookPageTotal && this.SelectedBook.BookPageTotal != 0;
                        this.ShowUpNext = this.SelectedBook.BookPageRead == 0;
                        this.ShowPages = true;
                        this.ShowTime = false;
                        this.ShowCheckpoints = this.SelectedBook.BookPageTotal != 0;
                    }
                    else
                    {
                        this.BookIsRead = this.SelectedBook.BookListenedTime == this.SelectedBook.BookTotalTime && this.SelectedBook.BookTotalTime != 0;
                        this.ShowUpNext = this.SelectedBook.BookListenedTime == 0;
                        this.ShowPages = false;
                        this.ShowTime = true;
                        this.ShowCheckpoints = this.SelectedBook.BookTotalTime != 0;
                    }

                    if (!string.IsNullOrEmpty(this.SelectedBook.BookCoverFileName))
                    {
                        var directory = $"{FileSystem.AppDataDirectory}/{AppStringResources.BookCovers.Replace(" ", string.Empty)}";

                        this.SelectedBook.BookCover = ImageSource.FromFile($"{directory}/{this.SelectedBook.BookCoverFileName}");
                    }

                    if (!string.IsNullOrEmpty(this.SelectedBook.BookCoverUrl))
                    {
                        PermissionStatus internetStatus = await Permissions.CheckStatusAsync<InternetPermission>();

                        if (internetStatus != PermissionStatus.Granted)
                        {
                            internetStatus = await Permissions.RequestAsync<InternetPermission>();
                        }

                        if (internetStatus == PermissionStatus.Granted && Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                        {
                            this.SelectedBook.BookCover = new UriImageSource
                            {
                                Uri = new Uri(this.SelectedBook.BookCoverUrl),
                                CachingEnabled = true,
                                CacheValidity = TimeSpan.FromDays(14),
                            };
                        }
                    }

                    this.BookCover = this.SelectedBook.BookCover;

                    var loadDataTasks = new Task[]
                    {
                        Task.Run(() => this.ReadingDataChanged()),
                        Task.Run(() => this.ChapterListChanged()),
                        Task.Run(() => this.AuthorListChanged()),
                        Task.Run(() => this.BookInfoChanged()),
                        Task.Run(() => this.SummaryChanged()),
                        Task.Run(() => this.CommentsChanged()),
                        Task.Run(() => this.SelectedBook.SetBookCheckpoints(this.ShowCheckpoints)),
                        Task.Run(() => this.SelectedBook.SetCoverDisplay()),
                        Task.Run(() => this.SelectedBook.SetPartOfSeries()),
                        Task.Run(() => this.SelectedBook.SetPartOfCollection()),
                        Task.Run(() => this.SelectedBook.SetBookPrice()),
                        Task.Run(() => this.SelectedBook.TotalTimeSpan = BookModel.SetTime(this.SelectedBook.BookHoursTotal, this.SelectedBook.BookMinutesTotal)),
                        Task.Run(() => this.SelectedBook.ListenTimeSpan = BookModel.SetTime(this.SelectedBook.BookHourListened, this.SelectedBook.BookMinuteListened)),
                    };

                    await Task.WhenAll(chapters, genre, location, authors);

                    this.ChapterList = chapters.Result;
                    this.SelectedGenre = genre.Result;
                    this.SelectedLocation = location.Result;
                    this.SelectedAuthorList = authors.Result;

                    await Task.WhenAll(loadDataTasks);

                    this.SelectedBook.TotalTimeString = $"{this.SelectedBook.BookHoursTotal:0}:{this.SelectedBook.BookMinutesTotal:00}";
                    this.SelectedBook.ListenTimeString = $"{this.SelectedBook.BookHourListened:0}:{this.SelectedBook.BookMinuteListened:00}";

                    this.SetIsBusyFalse();
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
        }

        /// <summary>
        /// Show edit book view with selected book.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task EditBook()
        {
            if (this.SelectedBook != null)
            {
                this.SetIsBusyTrue();

                var view = new BookEditView(this.SelectedBook, $"{AppStringResources.EditBook}", true, (BookMainView)this.View, this.PreviousViewModel);

                await Shell.Current.Navigation.PushAsync(view);

                this.SetIsBusyFalse();
            }
        }

        /// <summary>
        /// Delete selected book.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task DeleteBook()
        {
            if (this.SelectedBook != null && !string.IsNullOrEmpty(this.SelectedBook.BookTitle))
            {
                var answer = await this.DeleteCheck(this.SelectedBook.BookTitle);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        await Database.DeleteBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedBook));
                        await RemoveFromStaticList(this.SelectedBook);

                        await this.ConfirmDelete(this.SelectedBook.BookTitle);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await this.DisplayMessage("Error!", ex.Message);
#endif

#if RELEASE
                        await this.DisplayMessage(AppStringResources.AnErrorOccurred, null);
#endif
                        await this.CanceledAction();
                    }
                }
                else
                {
                    await this.CanceledAction();
                }
            }
        }

        /// <summary>
        /// Share book information.
        /// </summary>
        /// <returns>A task.</returns>
        [RelayCommand]
        public async Task ShareList()
        {
            if (this.SelectedBook != null)
            {
                var title = this.SelectedBook.BookTitle;

                string? text;
                if (this.SelectedAuthorList != null && this.SelectedAuthorList.Count > 0)
                {
                    text = $"{AppStringResources.BookTitleByAuthorName.Replace("Book Title", this.SelectedBook.BookTitle).Replace("Author Name", this.SelectedAuthorList[0].FullName)}";

                    if (this.SelectedAuthorList.Count > 1)
                    {
                        text += $", {AppStringResources.EtAl}";
                    }
                }
                else
                {
                    text = $"{AppStringResources.BookTitle_Replace.Replace("Book Title", this.SelectedBook.BookTitle)}";
                }

                if (!string.IsNullOrEmpty(this.SelectedBook.BookURL))
                {
                    text += $" ({this.SelectedBook.BookURL})";
                }

                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = text,
                    Title = title,
                });
            }
        }

        private void GetPreferences()
        {
            this.HiddenAuthorsOn = Preferences.Get("HiddenAuthorsOn", true /* Default */);
        }
    }
}
