using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Book;
using BookCollector.Views.WishListBook;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;

namespace BookCollector.ViewModels.WishListBook
{
    public partial class WishListBookMainViewModel : BookBaseViewModel
    {
        public WishListBookMainViewModel(BookModel book, ContentPage view)
        {
            _view = view;

            SelectedBook = book;
            InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book",$"{SelectedBook.BookTitle}")}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                AuthorListSectionValue = true;
                BookInfoSectionValue = true;
                SummarySectionValue = true;
                CommentsSectionValue = true;

                if (SelectedBook.BookCoverBytes != null)
                {
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(SelectedBook.BookCoverBytes));
                    BookCover = imageSource;
                    SelectedBook.BookCover = BookCover;
                }
                else if (SelectedBook.BookCoverUrl != null)
                {
                    var byteArray = new WebClient().DownloadData($"{SelectedBook.BookCoverUrl}");
                    BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    SelectedBook.BookCover = BookCover;
                }

                AuthorList = !string.IsNullOrEmpty(SelectedBook.AuthorListString) ? ParseOutAuthorsFromString(SelectedBook.AuthorListString) : null;

                Task.WaitAll(
                [
                    Task.Run (async () => await SelectedBook.SetBookCheckpoints() ),
                    Task.Run (async () => await SelectedBook.SetCoverDisplay() ),
                    Task.Run (async () => await SelectedBook.SetPartOfSeries() ),
                    Task.Run (async () => await SelectedBook.SetBookPrice() ),
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

            WishListBookEditView view = new WishListBookEditView(SelectedBook, $"{AppStringResources.EditBook}", true, (WishListBookMainView)_view);

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

                    // Unit test data
                    TestData.DeleteWishListBook(SelectedBook);

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

        [RelayCommand]
        public async Task AddToLibrary()
        {
            bool answer = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureYouWantToMoveBookToYourLibrary_Question.Replace("book", SelectedBook.BookTitle), null, null);

            if (answer)
            {
                try
                {
                    SetIsBusyTrue();
                    
                    // Unit test data
                    var series = TestData.SeriesList.SingleOrDefault(x => x.SeriesName.Equals(SelectedBook.BookSeries));

                    if (series == null)
                    {
                        series = new SeriesModel()
                        {
                            SeriesName = SelectedBook.BookSeries
                        };

                        // Unit test data
                        TestData.InsertSeries(series);
                    }
                    
                    SelectedBook.BookSeriesGuid = series.SeriesGuid;

                    Task.WaitAll(
                    [
                        Task.Run (async () => await SelectedBook.SetReadingProgress() ),
                    ]);

                    // Unit test data
                    TestData.InsertBook(SelectedBook);
                    TestData.DeleteWishListBook(SelectedBook);

                    await DisplayMessage(AppStringResources.AddToLibrary, AppStringResources.BookWasAddedToLibrary);

                    await Shell.Current.Navigation.PopAsync();

                    SetIsBusyFalse();
                }
                catch(Exception ex)
                {
                    SetIsBusyFalse();
                }
            }
            else
                await CanceledAction();
        }
    }
}
