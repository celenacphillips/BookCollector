using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Book;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.Book
{
    public partial class BookMainViewModel : BookBaseViewModel
    {
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

                    this.AuthorList = !string.IsNullOrEmpty(this.SelectedBook.AuthorListstring) ? ParseOutAuthorsFromstring(this.SelectedBook.AuthorListstring, this.HiddenAuthorsOn) : null;

                    Task.WaitAll(
                    [
                        Task.Run(async () => this.ChapterList = await FilterLists.GetAllChaptersInBook(this.SelectedBook.BookGuid)),
                        Task.Run(async () => this.SelectedGenre = await FilterLists.GetGenreForBook(this.SelectedBook.BookGenreGuid)),
                        Task.Run(async () => this.SelectedLocation = await FilterLists.GetLocationForBook(this.SelectedBook.BookLocationGuid)),
                        Task.Run(async () => this.SelectedBook.SetBookCheckpoints()),
                        Task.Run(async () => this.SelectedBook.SetCoverDisplay()),
                        Task.Run(async () => await this.SelectedBook.SetPartOfSeries()),
                        Task.Run(async () => await this.SelectedBook.SetPartOfCollection()),
                        Task.Run(async () => await this.SelectedBook.SetBookPrice()),
                        Task.Run(() => this.ReadingDataChanged()),
                        Task.Run(() => this.ChapterListChanged()),
                        Task.Run(() => this.AuthorListChanged()),
                        Task.Run(() => this.BookInfoChanged()),
                        Task.Run(() => this.SummaryChanged()),
                        Task.Run(() => this.CommentsChanged()),
                    ]);

                    this.SetIsBusyFalse();
                }
                catch (Exception)
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
                        }

                        await ConfirmDelete(this.SelectedBook.BookTitle);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception)
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
