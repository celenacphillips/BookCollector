// <copyright file="BookMainViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Book;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Book
{
    public partial class BookMainViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public ObservableCollection<AuthorModel>? authorList;

        [ObservableProperty]
        public ObservableCollection<AuthorModel>? selectedAuthorList;

        public BookMainViewModel(BookModel book, ContentPage view)
        {
            this.View = view;

            this.SelectedBook = book;
            this.InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book", $"{this.SelectedBook.BookTitle}")}";
        }

        public async Task SetViewModelData()
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
                    var authors = FillLists.GetAllAuthorsForBook(this.SelectedBook.BookGuid, this.HiddenAuthorsOn);

                    this.ReadingDataSectionValue = true;
                    this.ChapterListSectionValue = true;
                    this.AuthorListSectionValue = true;
                    this.BookInfoSectionValue = true;
                    this.SummarySectionValue = true;
                    this.CommentsSectionValue = true;

                    this.BookIsRead = this.SelectedBook.BookPageRead == this.SelectedBook.BookPageTotal && this.SelectedBook.BookPageTotal != 0;
                    this.ShowUpNext = this.SelectedBook.BookPageRead == 0;

                    if (!string.IsNullOrEmpty(this.SelectedBook.BookCoverFileLocation) && this.SelectedBook.BookCover == null)
                    {
                        var imageBytes = File.ReadAllBytes(this.SelectedBook.BookCoverFileLocation);
                        var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                        this.SelectedBook.BookCover = imageSource;
                    }

                    if (!string.IsNullOrEmpty(this.SelectedBook.BookCoverUrl) && this.SelectedBook.BookCover == null)
                    {
                        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                        {
                            var byteArray = DownloadImage(this.SelectedBook.BookCoverUrl);
                            this.SelectedBook.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                        }
                        else
                        {
                            await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
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
                        Task.Run(() => this.SelectedBook.SetBookCheckpoints()),
                        Task.Run(() => this.SelectedBook.SetCoverDisplay()),
                        Task.Run(() => this.SelectedBook.SetPartOfSeries()),
                        Task.Run(() => this.SelectedBook.SetPartOfCollection()),
                        Task.Run(() => this.SelectedBook.SetBookPrice()),
                    };

                    await Task.WhenAll(chapters, genre, location, authors);

                    this.ChapterList = chapters.Result;
                    this.SelectedGenre = genre.Result;
                    this.SelectedLocation = location.Result;
                    this.SelectedAuthorList = authors.Result;

                    await Task.WhenAll(loadDataTasks);

                    this.SetIsBusyFalse();
                }
                catch (Exception ex)
                {
                    this.SetIsBusyFalse();
                }
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            this.SetRefreshTrue();
            await this.SetViewModelData();
            this.SetRefreshFalse();
        }

        [RelayCommand]
        public async Task EditBook()
        {
            if (this.SelectedBook != null)
            {
                this.SetIsBusyTrue();

                var view = new BookEditView(this.SelectedBook, $"{AppStringResources.EditBook}", true, (BookMainView)this.View);

                await Shell.Current.Navigation.PushAsync(view);

                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task DeleteBook()
        {
            if (this.SelectedBook != null && !string.IsNullOrEmpty(this.SelectedBook.BookTitle))
            {
                var answer = await DeleteCheck(this.SelectedBook.BookTitle);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        if (TestData.UseTestData)
                        {
                            TestData.DeleteBook(this.SelectedBook);
                        }
                        else
                        {
                            await Database.DeleteBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedBook));
                        }

                        await ConfirmDelete(this.SelectedBook.BookTitle);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
                        await CanceledAction();
                    }
                }
                else
                {
                    await CanceledAction();
                }
            }
        }

        [RelayCommand]
        public async Task ShareList()
        {
            if (this.SelectedBook != null)
            {
                var title = this.SelectedBook.BookTitle;

                string? text;
                if (this.AuthorList != null)
                {
                    text = $"{AppStringResources.BookTitleByAuthorName.Replace("Book Title", this.SelectedBook.BookTitle).Replace("Author Name", this.AuthorList[0].FullName)}";

                    if (this.AuthorList.Count > 1)
                    {
                        text += $", {AppStringResources.EtAl}";
                    }
                }
                else
                {
                    text = $"{AppStringResources.BookTitle.Replace("Book Title", this.SelectedBook.BookTitle)}";
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
