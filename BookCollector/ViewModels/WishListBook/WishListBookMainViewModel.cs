using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.WishListBook;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.ViewModels.WishListBook
{
    public partial class WishListBookMainViewModel : BookBaseViewModel
    {
        public WishListBookMainViewModel(BookModel book, ContentPage view)
        {
            this.View = view;

            this.SelectedBook = book;
            this.InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book", $"{this.SelectedBook.BookTitle}")}";
            this.AuthorList = [];
        }

        public async Task SetViewModelData()
        {
            if (this.SelectedBook != null)
            {
                try
                {
                    this.SetIsBusyTrue();

                    this.AuthorListSectionValue = true;
                    this.BookInfoSectionValue = true;
                    this.SummarySectionValue = true;
                    this.CommentsSectionValue = true;

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

                    this.AuthorList = !string.IsNullOrEmpty(this.SelectedBook.AuthorListstring) ? ParseOutAuthorsFromstring(this.SelectedBook.AuthorListstring) : null;

                    Task.WaitAll(
                   [
                        Task.Run(async () => this.SelectedBook.SetBookCheckpoints()),
                        Task.Run(async () => this.SelectedBook.SetCoverDisplay()),
                        Task.Run(async () => await this.SelectedBook.SetPartOfSeries()),
                        Task.Run(async () => await this.SelectedBook.SetBookPrice()),
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

                var view = new WishListBookEditView(this.SelectedBook, $"{AppStringResources.EditBook}", true, (WishListBookMainView)this.View);

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
                            TestData.DeleteWishListBook(this.SelectedBook);
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
        public async Task AddToLibrary()
        {
            if (this.SelectedBook != null)
            {
                var answer = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureYouWantToMoveBookToYourLibrary_Question.Replace("book", this.SelectedBook.BookTitle), null, null);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        SeriesModel? series = null;

                        if (TestData.UseTestData)
                        {
                            series = TestData.SeriesList.SingleOrDefault(x => !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.Equals(this.SelectedBook.BookSeries));
                        }
                        else
                        {
                        }

                        if (series == null)
                        {
                            series = new SeriesModel()
                            {
                                SeriesName = this.SelectedBook.BookSeries,
                            };

                            if (TestData.UseTestData)
                            {
                                TestData.InsertSeries(series);
                            }
                            else
                            {
                            }
                        }

                        this.SelectedBook.BookSeriesGuid = series.SeriesGuid;

                        Task.WaitAll(
                       [
                            Task.Run(async () => this.SelectedBook.SetReadingProgress()),
                        ]);

                        if (TestData.UseTestData)
                        {
                            TestData.InsertBook(this.SelectedBook);
                            TestData.DeleteWishListBook(this.SelectedBook);
                        }
                        else
                        {
                        }

                        await DisplayMessage(AppStringResources.AddToLibrary, AppStringResources.BookWasAddedToLibrary);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception)
                    {
                        this.SetIsBusyFalse();
                    }
                }
                else
                {
                    await CanceledAction();
                }
            }
        }
    }
}
