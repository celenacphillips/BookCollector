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
            View = view;

            SelectedBook = book;
            InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book",$"{SelectedBook.BookTitle}")}";
            AuthorList = [];
        }

        public async Task SetViewModelData()
        {
            if (SelectedBook != null)
            {
                try
                {
                    SetIsBusyTrue();

                    AuthorListSectionValue = true;
                    BookInfoSectionValue = true;
                    SummarySectionValue = true;
                    CommentsSectionValue = true;

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
                            var byteArray = DownloadImage(SelectedBook.BookCoverUrl);
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
                        Task.Run (async () => SelectedBook.SetBookCheckpoints() ),
                        Task.Run (async () => SelectedBook.SetCoverDisplay() ),
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
            if (SelectedBook != null)
            {
                SetIsBusyTrue();

                var view = new WishListBookEditView(SelectedBook, $"{AppStringResources.EditBook}", true, (WishListBookMainView)View);

                await Shell.Current.Navigation.PushAsync(view);

                SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task DeleteBook()
        {
            if (SelectedBook != null && !string.IsNullOrEmpty(SelectedBook.BookTitle))
            {
                var answer = await DeleteCheck(SelectedBook.BookTitle);

                if (answer)
                {
                    try
                    {
                        SetIsBusyTrue();

                        if (TestData.UseTestData)
                        {
                            TestData.DeleteWishListBook(SelectedBook);
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

        [RelayCommand]
        public async Task AddToLibrary()
        {
            if (SelectedBook != null)
            {
                var answer = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureYouWantToMoveBookToYourLibrary_Question.Replace("book", SelectedBook.BookTitle), null, null);

                if (answer)
                {
                    try
                    {
                        SetIsBusyTrue();

                        SeriesModel? series = null;

                        if (TestData.UseTestData)
                        {
                            series = TestData.SeriesList.SingleOrDefault(x => !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.Equals(SelectedBook.BookSeries));
                        }
                        else
                        {

                        }

                        if (series == null)
                        {
                            series = new SeriesModel()
                            {
                                SeriesName = SelectedBook.BookSeries
                            };

                            if (TestData.UseTestData)
                            {
                                TestData.InsertSeries(series);
                            }
                            else
                            {

                            }
                        }

                        SelectedBook.BookSeriesGuid = series.SeriesGuid;

                        Task.WaitAll(
                        [
                            Task.Run (async () => SelectedBook.SetReadingProgress() ),
                        ]);

                        if (TestData.UseTestData)
                        {
                            TestData.InsertBook(SelectedBook);
                            TestData.DeleteWishListBook(SelectedBook);
                        }
                        else
                        {

                        }


                        await DisplayMessage(AppStringResources.AddToLibrary, AppStringResources.BookWasAddedToLibrary);

                        await Shell.Current.Navigation.PopAsync();

                        SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
                        SetIsBusyFalse();
                    }
                }
                else
                    await CanceledAction();
            }
        }
    }
}
