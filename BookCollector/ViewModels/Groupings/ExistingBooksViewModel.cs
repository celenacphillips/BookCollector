using BookCollector.Data;
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels.BaseViewModels;
using BookCollector.Views.Book;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCollector.ViewModels.Groupings
{
    public partial class ExistingBooksViewModel : BookBaseViewModel
    {
        private object SelectedObject {  get; set; }

        private string SelectedObjectType { get; set; }

        private string SelectedObjectName { get; set; }

        public ExistingBooksViewModel(object selected, ContentPage view)
        {
            _view = view;
            SelectedObject = selected;
            SetSelectedObjectType();
            SetSelectedObjectName();

            CollectionViewHeight = DeviceHeight - SingleMenuBar;
            InfoText = $"{AppStringResources.ExistingBooksView_InfoText.Replace("grouping", SelectedObjectName)}";
        }

        public async Task SetViewModelData()
        {
            try
            {
                SetIsBusyTrue();

                // Unit test data
                var bookList = TestData.BookList;

                switch (SelectedObjectType)
                {
                    case "Collection":
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutACollectionList(bookList) ),
                        ]);
                        break;

                    case "Genre":
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutAGenreList(bookList) ),
                        ]);
                        break;

                    case "Series":
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutASeriesList(bookList) ),
                        ]);
                        break;

                    case "Author":
                        var author = (AuthorModel)SelectedObject;
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutAuthorList(bookList, author.ReverseFullName) ),
                        ]);
                        break;

                    case "Location":
                        Task.WaitAll(
                        [
                            Task.Run (async () => FullBookList = await FilterLists.GetAllBooksWithoutALocationList(bookList) ),
                        ]);
                        break;

                    default:
                        break;
                }

                TotalBooksCount = FullBookList.Count;

                FilteredBookList = FullBookList;
                FilteredBooksCount = FilteredBookList.Count;

                TotalBooksString = StringManipulation.SetTotalBooksString(FilteredBooksCount, TotalBooksCount);

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
        public async Task ExistingBooksSelectionChanged()
        {
            var title = $"{AppStringResources.AddBookToGrouping_Question.Replace("grouping", ViewTitle)}";
            bool answer = await DisplayMessage(title, title, null, null);

            if (answer)
            {
                await AddBookToGrouping();
            }
            else
            {
                await CanceledAction();
                SelectedBook = null;
            }
        }

        private async Task AddBookToGrouping()
        {
            SetIsBusyTrue();

            switch (SelectedObjectType)
            {
                case "Collection":
                    var collection = (CollectionModel)SelectedObject;
                    SelectedBook.BookCollectionGuid = collection.CollectionGuid;
                    break;

                case "Genre":
                    var genre = (GenreModel)SelectedObject;
                    SelectedBook.BookGenreGuid = genre.GenreGuid;
                    break;

                case "Series":
                    var series = (SeriesModel)SelectedObject;
                    SelectedBook.BookSeriesGuid = series.SeriesGuid;
                    break;

                case "Author":
                    var author = (AuthorModel)SelectedObject;
                    if (!string.IsNullOrEmpty(SelectedBook.AuthorListString))
                        SelectedBook.AuthorListString += $"; {author.ReverseFullName}";
                    else
                        SelectedBook.AuthorListString = $"{author.ReverseFullName}";

                        // Unit test data
                        TestData.AddAuthorToBook(author.AuthorGuid, SelectedBook.BookGuid);
                    break;

                case "Location":
                    var location = (LocationModel)SelectedObject;
                    SelectedBook.BookLocationGuid = location.LocationGuid;
                    break;

                default:
                    break;
            }

            BookMainView view = new BookMainView(SelectedBook, $"{SelectedBook.BookTitle}");
            await Shell.Current.Navigation.PushAsync(view);

            SetIsBusyFalse();

            await DisplayMessage($"{AppStringResources.BookHasBeenAddedToGrouping.Replace("Book", SelectedBook.BookTitle).Replace("grouping", SelectedObjectName)}", null);
        }

        private void SetSelectedObjectType()
        {
            if (SelectedObject.GetType().ToString().Contains("Collection"))
                SelectedObjectType = "Collection";

            if (SelectedObject.GetType().ToString().Contains("Genre"))
                SelectedObjectType = "Genre";

            if (SelectedObject.GetType().ToString().Contains("Series"))
                SelectedObjectType = "Series";

            if (SelectedObject.GetType().ToString().Contains("Author"))
                SelectedObjectType = "Author";

            if (SelectedObject.GetType().ToString().Contains("Location"))
                SelectedObjectType = "Location";
        }

        private void SetSelectedObjectName()
        {
            switch (SelectedObjectType)
            {
                case "Collection":
                    var collection = (CollectionModel)SelectedObject;
                    SelectedObjectName = collection.CollectionName;
                    break;

                case "Genre":
                    var genre = (GenreModel)SelectedObject;
                    SelectedObjectName = genre.GenreName;
                    break;

                case "Series":
                    var series = (SeriesModel)SelectedObject;
                    SelectedObjectName = series.SeriesName;
                    break;

                case "Author":
                    var author = (AuthorModel)SelectedObject;
                    SelectedObjectName = author.FullName;
                    break;

                case "Location":
                    var location = (LocationModel)SelectedObject;
                    SelectedObjectName = location.LocationName;
                    break;

                default:
                    break;
            }
        }
    }
}
