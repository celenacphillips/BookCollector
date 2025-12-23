// <copyright file="WishListBookMainViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Data.DatabaseModels;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.WishListBook;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.WishListBook
{
    public partial class WishListBookMainViewModel : BookBaseViewModel
    {
        [ObservableProperty]
        public WishlistBookModel? selectedWishlistBook;

        [ObservableProperty]
        public ObservableCollection<AuthorModel>? authorList;

        public WishListBookMainViewModel(WishlistBookModel book, ContentPage view)
        {
            this.View = view;

            this.SelectedWishlistBook = book;
            this.InfoText = $"{AppStringResources.BookMainView_InfoText.Replace("book", $"{this.SelectedWishlistBook.BookTitle}")}";
            this.AuthorList = [];
        }

        public async Task SetViewModelData()
        {
            if (this.SelectedWishlistBook != null)
            {
                try
                {
                    this.SetIsBusyTrue();

                    var authors = ParseOutAuthorsFromstring(this.SelectedWishlistBook.AuthorListString);

                    this.AuthorListSectionValue = true;
                    this.BookInfoSectionValue = true;
                    this.SummarySectionValue = true;
                    this.CommentsSectionValue = true;

                    if (!string.IsNullOrEmpty(this.SelectedWishlistBook.BookCoverFileLocation) && this.SelectedWishlistBook.BookCover == null)
                    {
                        var imageBytes = File.ReadAllBytes(this.SelectedWishlistBook.BookCoverFileLocation);
                        var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                        this.SelectedWishlistBook.BookCover = imageSource;
                    }

                    if (!string.IsNullOrEmpty(this.SelectedWishlistBook.BookCoverUrl) && this.SelectedWishlistBook.BookCover == null)
                    {
                        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                        {
                            var byteArray = DownloadImage(this.SelectedWishlistBook.BookCoverUrl);
                            this.SelectedWishlistBook.BookCover = ImageSource.FromStream(() => new MemoryStream(byteArray));
                        }
                        else
                        {
                            await DisplayMessage($"{AppStringResources.PleaseConnectToInternetToFindBookCover}", null);
                        }
                    }

                    this.BookCover = this.SelectedWishlistBook.BookCover;

                    var loadDataTasks = new Task[]
                    {
                        Task.Run(() => this.AuthorListChanged()),
                        Task.Run(() => this.BookInfoChanged()),
                        Task.Run(() => this.SummaryChanged()),
                        Task.Run(() => this.CommentsChanged()),
                        Task.Run(() => this.SelectedWishlistBook.SetCoverDisplay()),
                        Task.Run(() => this.SelectedWishlistBook.SetPartOfSeries()),
                        Task.Run(() => this.SelectedWishlistBook.SetBookPrice()),
                    };

                    await Task.WhenAll(authors);

                    this.AuthorList = authors.Result;

                    await Task.WhenAll(loadDataTasks);

                    this.SetIsBusyFalse();
                }
                catch (Exception ex)
                {
#if DEBUG
                    await DisplayMessage("Error!", ex.Message);
#endif
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
            if (this.SelectedWishlistBook != null)
            {
                this.SetIsBusyTrue();

                var view = new WishListBookEditView(this.SelectedWishlistBook, $"{AppStringResources.EditBook}", true, (WishListBookMainView)this.View);

                await Shell.Current.Navigation.PushAsync(view);

                this.SetIsBusyFalse();
            }
        }

        [RelayCommand]
        public async Task DeleteBook()
        {
            if (this.SelectedWishlistBook != null && !string.IsNullOrEmpty(this.SelectedWishlistBook.BookTitle))
            {
                var answer = await DeleteCheck(this.SelectedWishlistBook.BookTitle);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        if (TestData.UseTestData)
                        {
                            TestData.DeleteWishListBook(this.SelectedWishlistBook);
                        }
                        else
                        {
                            await Database.DeleteWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.SelectedWishlistBook));
                        }

                        await ConfirmDelete(this.SelectedWishlistBook.BookTitle);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await DisplayMessage("Error!", ex.Message);
#endif
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
            if (this.SelectedWishlistBook != null)
            {
                var answer = await DisplayMessage(AppStringResources.AreYouSure_Question, AppStringResources.AreYouSureYouWantToMoveBookToYourLibrary_Question.Replace("book", this.SelectedWishlistBook.BookTitle), null, null);

                if (answer)
                {
                    try
                    {
                        this.SetIsBusyTrue();

                        if (!string.IsNullOrEmpty(this.SelectedWishlistBook.BookSeries))
                        {
                            SeriesModel? series = null;

                            if (TestData.UseTestData)
                            {
                                series = TestData.SeriesList.SingleOrDefault(x => !string.IsNullOrEmpty(x.SeriesName) && x.SeriesName.Equals(this.SelectedWishlistBook.BookSeries));
                            }
                            else
                            {
                                series = await Database.GetSeriesByNameAsync(this.SelectedWishlistBook.BookSeries);
                            }

                            if (series == null || series.SeriesGuid == null)
                            {
                                series = new SeriesModel()
                                {
                                    SeriesName = this.SelectedWishlistBook.BookSeries,
                                };

                                if (TestData.UseTestData)
                                {
                                    TestData.InsertSeries(series);
                                }
                                else
                                {
                                    series = await Database.SaveSeriesAsync(ConvertTo<SeriesDatabaseModel>(series));
                                }
                            }

                            this.SelectedWishlistBook.BookSeriesGuid = series.SeriesGuid;
                        }

                        if (TestData.UseTestData)
                        {
                            TestData.InsertBook(ConvertTo<BookModel>(this.SelectedWishlistBook));
                            TestData.DeleteWishListBook(this.SelectedWishlistBook);
                        }
                        else
                        {
                            this.SelectedBook = ConvertTo<BookModel>(await Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(this.SelectedWishlistBook)));
                            await Database.DeleteWishlistBookAsync(ConvertTo<WishlistBookDatabaseModel>(this.SelectedWishlistBook));
                        }

                        if (!string.IsNullOrEmpty(this.SelectedWishlistBook.AuthorListString))
                        {
                            var authorList = SplitStringIntoAuthorList(this.SelectedWishlistBook.AuthorListString);

                            foreach (var author in authorList)
                            {
                                var addAuthor = author;

                                if (TestData.UseTestData)
                                {
                                    TestData.AddAuthorToBook(addAuthor.AuthorGuid, this.SelectedWishlistBook.BookGuid);
                                }
                                else
                                {
                                    var existingAuthor = await Database.GetAuthorByNameAsync(addAuthor.FirstName, addAuthor.LastName);

                                    if (existingAuthor != null && existingAuthor.AuthorGuid != null)
                                    {
                                        addAuthor = existingAuthor;
                                    }
                                    else
                                    {
                                        addAuthor.AuthorGuid = Guid.NewGuid();
                                        await Database.InsertAuthorAsync(ConvertTo<AuthorDatabaseModel>(addAuthor), this.SelectedBook.BookGuid);
                                    }
                                }
                            }
                        }

                        await DisplayMessage(AppStringResources.AddToLibrary, AppStringResources.BookWasAddedToLibrary);

                        await Shell.Current.Navigation.PopAsync();

                        this.SetIsBusyFalse();
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        await DisplayMessage("Error!", ex.Message);
#endif
                        this.SetIsBusyFalse();
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
            if (this.SelectedWishlistBook != null)
            {
                var title = this.SelectedWishlistBook.BookTitle;

                string? text;
                if (this.AuthorList != null)
                {
                    text = $"{AppStringResources.BookTitleByAuthorName.Replace("Book Title", this.SelectedWishlistBook.BookTitle).Replace("Author Name", this.AuthorList[0].FullName)}";

                    if (this.AuthorList.Count > 1)
                    {
                        text += $", {AppStringResources.EtAl}";
                    }
                }
                else
                {
                    text = $"{AppStringResources.BookTitle.Replace("Book Title", this.SelectedWishlistBook.BookTitle)}";
                }

                if (!string.IsNullOrEmpty(this.SelectedWishlistBook.BookURL))
                {
                    text += $" ({this.SelectedWishlistBook.BookURL})";
                }

                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = text,
                    Title = title,
                });
            }
        }
    }
}
