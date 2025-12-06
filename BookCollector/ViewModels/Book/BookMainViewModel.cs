using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Book;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

namespace BookCollector.ViewModels.Book
{
    public partial class BookMainViewModel : BookBaseViewModel
    {
        public BookMainViewModel(BookModel book, ContentPage view)
        {
            _view = view;

            SelectedBook = book;
            InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book",$"{SelectedBook.BookTitle}")}";
            AuthorList = new ObservableCollection<AuthorModel>();
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                ReadingDataSectionValue = true;
                ChapterListSectionValue = true;
                AuthorListSectionValue = true;
                BookInfoSectionValue = true;
                SummarySectionValue = true;
                CommentsSectionValue = true;

                BookIsRead = SelectedBook.BookPageRead == SelectedBook.BookPageTotal && SelectedBook.BookPageTotal != 0;
                ShowUpNext = SelectedBook.BookPageRead == 0;

                if (!string.IsNullOrEmpty(SelectedBook.BookCoverFileLocation) && SelectedBook.BookCover == null)
                {
                    var imageBytes = File.ReadAllBytes(SelectedBook.BookCoverFileLocation);
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    SelectedBook.BookCover = imageSource;
                }

                if (!string.IsNullOrEmpty(SelectedBook.BookCoverUrl) && SelectedBook.BookCover == null)
                {
                    if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                    {
                        var byteArray = new WebClient().DownloadData($"{SelectedBook.BookCoverUrl}");
                        SelectedBook.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    }
                    else
                    {
                        await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                    }
                }

                BookCover = SelectedBook.BookCover;

                AuthorList = !string.IsNullOrEmpty(SelectedBook.AuthorListString) ? ParseOutAuthorsFromString(SelectedBook.AuthorListString) : null;

                Task.WaitAll(
                [
                    Task.Run (async () => ChapterList = await FilterLists.GetAllChaptersInBook(SelectedBook.BookGuid) ),
                    Task.Run (async () => SelectedGenre = await FilterLists.GetGenreForBook(SelectedBook.BookGenreGuid) ),
                    Task.Run (async () => SelectedLocation = await FilterLists.GetLocationForBook(SelectedBook.BookLocationGuid) ),
                    Task.Run (async () => await SelectedBook.SetBookCheckpoints() ),
                    Task.Run (async () => await SelectedBook.SetCoverDisplay() ),
                    Task.Run (async () => await SelectedBook.SetPartOfSeries() ),
                    Task.Run (async () => await SelectedBook.SetPartOfCollection() ),
                    Task.Run (async () => await SelectedBook.SetDates() ),
                    Task.Run (async () => await SelectedBook.SetBookPrice() ),
                    Task.Run (async () => await ReadingDataChanged() ),
                    Task.Run (async () => await ChapterListChanged() ),
                    Task.Run (async () => await AuthorListChanged() ),
                    Task.Run (async () => await BookInfoChanged() ),
                    Task.Run (async () => await SummaryChanged() ),
                    Task.Run (async () => await CommentsChanged() ),
                ]);

                SetIsBusyFalse();
            }
            catch (Exception ex)
            {
                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task Refresh()
        {
            SetRefreshTrue();
            await SetViewModelData();
            SetRefreshFalse();
        }


        [RelayCommand]
        public async Task EditBook()
        {
            SetIsBusyTrue();

            BookEditView view = new BookEditView(SelectedBook, $"{AppStringResources.EditBook}", true, (BookMainView)_view);

            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();
        }

        [RelayCommand]
        public async Task DeleteBook()
        {
            bool answer = await DeleteCheck(SelectedBook.BookTitle);

            if (answer)
            {
                try
                {
                    SetIsBusyTrue();

                    if (TestData.UseTestData)
                    {
                        TestData.DeleteBook(SelectedBook);
                    }
                    else
                    {

                    }

                    await ConfirmDelete(SelectedBook.BookTitle);

                    await Shell.Current.Navigation.PopAsync();

                    SetIsBusyFalse();
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
}
