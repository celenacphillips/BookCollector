// <copyright file="BookMainViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Book
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;
    using BookCollector.Views.Book;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// BookMainViewModel class.
    /// </summary>
    public partial class BookMainViewModel : BookMainBaseViewModel
    {
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

        /********************************************************/

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
            this.SetRefreshView(true);
        }

        /********************************************************/

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        public override void GetPreferences()
        {
            this.HiddenAuthorsOn = Preferences.Get("HiddenAuthorsOn", true /* Default */);
        }

        /// <summary>
        /// Set the view model lists.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetLists()
        {
            var chapters = FillLists.GetAllChaptersInBook(this.SelectedBook!.BookGuid);
            var genre = GetItems.GetGenreForBook(this.SelectedBook.BookGenreGuid);
            var location = GetItems.GetLocationForBook(this.SelectedBook.BookLocationGuid);
            var authors = FillLists.GetAllAuthorsForBook(this.SelectedBook.BookGuid);

            await Task.WhenAll(chapters, genre, location, authors);

            this.ChapterList = chapters.Result;
            this.SelectedGenre = genre.Result;
            this.SelectedLocation = location.Result;
            this.SelectedAuthorList = authors.Result;
        }

        /// <summary>
        /// Set section values.
        /// </summary>
        public override void SetSectionValues()
        {
            this.ReadingDataSectionValue = true;
            this.ChapterListSectionValue = true;
            this.AuthorListSectionValue = true;
            this.BookInfoSectionValue = true;
            this.SummarySectionValue = true;
            this.CommentsSectionValue = true;
        }

        /// <summary>
        /// Get book data for other methods.
        /// </summary>
        /// <param name="returnData">Return type.</param>
        /// <returns>An object of book data.</returns>
        public override object? GetBookData(string? returnData)
        {
            if (returnData != null && returnData.Equals("strings"))
            {
                return (List<string?>)[this.SelectedBook!.BookCoverFileName, this.SelectedBook!.BookCoverUrl];
            }

            return this.SelectedBook;
        }

        /// <summary>
        /// Check book format and set values.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task CheckBookFormat()
        {
            if (!this.SelectedBook!.BookFormat!.Equals(AppStringResources.Audiobook))
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
        }

        /// <summary>
        /// Set other view data.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetViewData()
        {
            var loadDataTasks = new Task[]
            {
                Task.Run(() => this.ReadingDataChanged()),
                Task.Run(() => this.ChapterListChanged()),
                Task.Run(() => this.AuthorListChanged()),
                Task.Run(() => this.BookInfoChanged()),
                Task.Run(() => this.SummaryChanged()),
                Task.Run(() => this.CommentsChanged()),
                Task.Run(() => this.SelectedBook!.SetBookCheckpoints(this.ShowCheckpoints)),
                Task.Run(() => this.SelectedBook!.SetCoverDisplay()),
                Task.Run(() => this.SelectedBook!.SetPartOfSeries()),
                Task.Run(() => this.SelectedBook!.SetPartOfCollection()),
                Task.Run(() => this.SelectedBook!.SetBookPrice()),
                Task.Run(() => this.SelectedBook!.TotalTimeSpan = SetTime(this.SelectedBook.BookHoursTotal, this.SelectedBook.BookMinutesTotal)),
                Task.Run(() => this.SelectedBook!.ListenTimeSpan = SetTime(this.SelectedBook.BookHourListened, this.SelectedBook.BookMinuteListened)),
            };

            await Task.WhenAll(loadDataTasks);

            this.SelectedBook!.TotalTimeString = StringManipulation.FormatTimeString(this.SelectedBook.BookHoursTotal, this.SelectedBook.BookMinutesTotal);
            this.SelectedBook.ListenTimeString = StringManipulation.FormatTimeString(this.SelectedBook.BookHourListened, this.SelectedBook.BookMinuteListened);
        }

        /// <summary>
        /// Set edit view.
        /// </summary>
        /// <returns>Page to navigate to.</returns>
        public override ContentPage SetEditView()
        {
            return new BookEditView(this.SelectedBook!, $"{AppStringResources.EditBook}", true, (BookMainView)this.View, this.PreviousViewModel);
        }

        /// <summary>
        /// Delete book data.
        /// </summary>
        /// <returns>An task.</returns>
        public override async Task DeleteData()
        {
            await BaseViewModel.Database.DeleteBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedBook!));
            await RemoveFromStaticList(this.SelectedBook!);
        }

        /// <summary>
        /// Set whether to refresh view or not.
        /// </summary>
        /// <param name="value">Value to change to.</param>
        public override void SetRefreshView(bool value)
        {
            RefreshView = value;
        }
    }
}
