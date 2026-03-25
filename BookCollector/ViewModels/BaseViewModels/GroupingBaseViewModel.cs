// <copyright file="GroupingBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.Popups;
    using BookCollector.Views.Groupings;

    /// <summary>
    /// GroupingBaseViewModel class.
    /// </summary>
    public abstract partial class GroupingBaseViewModel : BookListBaseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether total books is checked or not.
        /// </summary>
        public bool TotalBooksChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether total price is checked or not.
        /// </summary>
        public bool TotalPriceChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to insert the main view before or not.
        /// </summary>
        public bool InsertMainViewBefore { get; set; }

        /********************************************************/

        /// <summary>
        /// Update books in list to hide.
        /// </summary>
        /// <param name="books">Books to hide.</param>
        /// <returns>A task.</returns>
        public static async Task UpdateBooksToHide(ObservableCollection<BookModel>? books)
        {
            if (books != null)
            {
                foreach (var book in books)
                {
                    book.HideBook = true;
                    await BaseViewModel.Database.SaveBookAsync(ConvertTo<BookDatabaseModel>(book));
                }
            }
        }

        /// <summary>
        /// Set the count of unread books.
        /// </summary>
        /// <param name="list">List of books.</param>
        /// <returns>The count of unread books.</returns>
        public static int SetUnreadCount(ObservableCollection<BookModel> list)
        {
            return list.Count(x => x.BookPageRead == 0 &&
                    (x.BookHourListened == 0 && x.BookMinuteListened == 0));
        }

        /// <summary>
        /// Set the count of read books.
        /// </summary>
        /// <param name="list">List of books.</param>
        /// <returns>The count of read books.</returns>
        public static int SetReadCount(ObservableCollection<BookModel> list)
        {
            return list.Count(x => (x.BookPageRead == x.BookPageTotal && x.BookPageRead != 0) ||
                    (x.BookHourListened == x.BookHoursTotal && x.BookMinuteListened == x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0));
        }

        /// <summary>
        /// Set the count of reading books.
        /// </summary>
        /// <param name="list">List of books.</param>
        /// <returns>The count of reading books.</returns>
        public static int SetReadingCount(ObservableCollection<BookModel> list)
        {
            return list.Count(x => (x.BookPageRead != x.BookPageTotal && x.BookPageRead != 0) ||
                    (x.BookHourListened != x.BookHoursTotal && x.BookMinuteListened != x.BookMinutesTotal && x.BookHourListened != 0 && x.BookMinuteListened != 0));
        }

        /// <summary>
        /// Set total books string, total book count, unread count, read count, and reading count.
        /// </summary>
        /// <param name="list">List of books.</param>
        /// <param name="totalBooks">Optional total books for series grouping.</param>
        /// <returns>A parsed string and counts.</returns>
        public static (string?, int) SetTotalBooksStringAndCounts(ObservableCollection<BookModel>? list, string? totalBooks = null)
        {
            int count = 0, unread = 0, read = 0, reading = 0;

            if (list != null)
            {
                count = list.Count;
                unread = SetUnreadCount(list);
                read = SetReadCount(list);
                reading = SetReadingCount(list);
            }

            var totalBooksString = !string.IsNullOrEmpty(totalBooks) ?
                                    StringManipulation.SetTotalBooksAndReadingStatusString(count, int.Parse(totalBooks), unread, reading, read) :
                                    StringManipulation.SetTotalBooksAndReadingStatusString(count, unread, reading, read);

            return (totalBooksString, count);
        }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public override async Task SetViewModelData()
        {
        }

        /// <summary>
        /// Set the view model preferences.
        /// </summary>
        /// <returns>The list show hidden preference.</returns>
        public override bool GetPreferences()
        {
            return true;
        }

        /// <summary>
        /// Set data for filter popup.
        /// </summary>
        /// <param name="viewModel">Filter popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override FilterPopupViewModel SetFilterPopupValues(FilterPopupViewModel viewModel)
        {
            return viewModel;
        }

        /// <summary>
        /// Set data for filter popup.
        /// </summary>
        /// <param name="viewModel">Filter popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override FilterPopupViewModel SetFilterPopupLists(FilterPopupViewModel viewModel)
        {
            return viewModel;
        }

        /// <summary>
        /// Set data for sort popup.
        /// </summary>
        /// <param name="viewModel">Sort popup viewmodel.</param>
        /// <returns>The updated viewmodel.</returns>
        public override SortPopupViewModel SetSortPopupValues(SortPopupViewModel viewModel)
        {
            return viewModel;
        }

        /********************************************************/

        /// <summary>
        /// Show existing book view.
        /// </summary>
        /// <param name="selected">Grouping to show on the existing view.</param>
        /// <returns>A task.</returns>
        public async Task ShowExistingBookView(object selected)
        {
            this.SetIsBusyTrue();

            var view = new ExistingBooksView(selected, this.ViewTitle!, this);

            await Shell.Current.Navigation.PushAsync(view);

            this.SetIsBusyFalse();
        }

        /// <summary>
        /// Show popup menu with options and change view depending on option.
        /// </summary>
        /// <param name="selected">Selected object.</param>
        /// <param name="objectName">Selected object name.</param>
        /// <returns>A task.</returns>
        public async Task PopupMenu(object? selected, string? objectName)
        {
            if (selected != null && !string.IsNullOrEmpty(objectName))
            {
                List<string> actions = [AppStringResources.Edit, AppStringResources.Delete];
                var action = await this.PopupActionMenu(objectName, actions);

                if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Edit))
                {
                    await this.Edit(selected);
                }

                if (!string.IsNullOrEmpty(action) && action.Equals(AppStringResources.Delete))
                {
                    await this.Delete(selected, objectName);
                }
            }
        }

        /// <summary>
        /// Delete selected object.
        /// </summary>
        /// <param name="selected">Selected object.</param>
        /// <param name="objectName">Selected object name.</param>
        /// <returns>A task.</returns>
        public async Task Delete(object? selected, string? objectName)
        {
            var answer = await this.DeleteCheck(objectName!);

            if (answer)
            {
                try
                {
                    this.SetIsBusyTrue();

                    await this.DeleteGrouping(selected!);

                    await this.ConfirmDelete(objectName!);

                    await this.SetViewModelData();

                    this.SetIsBusyFalse();
                }
                catch (Exception ex)
                {
                    await this.ViewModelCatch(ex);
                }
            }
            else
            {
                await this.CanceledAction();
            }
        }

        /********************************************************/

        /// <summary>
        /// Show edit view.
        /// </summary>
        /// <param name="selected">Selected object.</param>
        /// <returns>A task.</returns>
        public abstract Task Edit(object selected);

        /// <summary>
        /// Delete grouping from database.
        /// </summary>
        /// <param name="selected">Selected object.</param>
        /// <returns>A task.</returns>
        public abstract Task DeleteGrouping(object selected);

        /********************************************************/

        /// <summary>
        /// Filter and sort list.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="totalCount">Total count.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            ObservableCollection<AuthorModel>?,
            int,
            string)> Search(ObservableCollection<AuthorModel>? hiddenList, int totalCount, bool isNameChecked)
        {
            var filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

            var filteredCount = filteredList?.Count ?? 0;

            var totalString = StringManipulation.SetTotalBooksString(filteredCount, totalCount);

            filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            return (filteredList, filteredCount, totalString);
        }

        /// <summary>
        /// Filter and sort list.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="totalCount">Total count.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            ObservableCollection<CollectionModel>?,
            int,
            string)> Search(ObservableCollection<CollectionModel>? hiddenList, int totalCount, bool isNameChecked)
        {
            var filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

            var filteredCount = filteredList?.Count ?? 0;

            var totalString = StringManipulation.SetTotalBooksString(filteredCount, totalCount);

            filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            return (filteredList, filteredCount, totalString);
        }

        /// <summary>
        /// Filter and sort list.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="totalCount">Total count.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            ObservableCollection<GenreModel>?,
            int,
            string)> Search(ObservableCollection<GenreModel>? hiddenList, int totalCount, bool isNameChecked)
        {
            var filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

            var filteredCount = filteredList?.Count ?? 0;

            var totalString = StringManipulation.SetTotalBooksString(filteredCount, totalCount);

            filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            return (filteredList, filteredCount, totalString);
        }

        /// <summary>
        /// Filter and sort list.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="totalCount">Total count.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            ObservableCollection<LocationModel>?,
            int,
            string)> Search(ObservableCollection<LocationModel>? hiddenList, int totalCount, bool isNameChecked)
        {
            var filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

            var filteredCount = filteredList?.Count ?? 0;

            var totalString = StringManipulation.SetTotalBooksString(filteredCount, totalCount);

            filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            return (filteredList, filteredCount, totalString);
        }

        /// <summary>
        /// Filter and sort list.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="totalCount">Total count.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            ObservableCollection<SeriesModel>?,
            int,
            string)> Search(ObservableCollection<SeriesModel>? hiddenList, int totalCount, bool isNameChecked)
        {
            var filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

            var filteredCount = filteredList?.Count ?? 0;

            var totalString = StringManipulation.SetTotalBooksString(filteredCount, totalCount);

            filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);

            return (filteredList, filteredCount, totalString);
        }

        /********************************************************/

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            int,
            int,
            string,
            bool,
            ObservableCollection<AuthorModel>?)>
            SetViewModelData(ObservableCollection<AuthorModel>? hiddenList, bool isNameChecked)
        {
            this.SetIsBusyTrue();

            int totalCount = 0, filteredCount = 0;
            var showCollectionViewFooter = false;

            var filteredList = new ObservableCollection<AuthorModel>();

            // List filter declaration
            if (hiddenList != null)
            {
                totalCount = hiddenList.Count;

                await Task.WhenAll(hiddenList.Select(x => x.SetTotalBooks(ShowHiddenBooks)));
                await Task.WhenAll(hiddenList.Select(x => x.SetTotalCostOfBooks(ShowHiddenBooks)));

                // List filter calls
                filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

                if (filteredList != null)
                {
                    filteredCount = filteredList.Count;

                    showCollectionViewFooter = filteredCount > 0;

                    filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);
                }

                // Wait and assign list filters to declarations
            }

            var totalString = StringManipulation.SetTotalAuthorsString(filteredCount, totalCount);

            this.SetIsBusyFalse();
            this.SetRefreshView(false);

            return (totalCount, filteredCount, totalString, showCollectionViewFooter, filteredList);
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            int,
            int,
            string,
            bool,
            ObservableCollection<CollectionModel>?)>
            SetViewModelData(ObservableCollection<CollectionModel>? hiddenList, bool isNameChecked)
        {
            this.SetIsBusyTrue();

            int totalCount = 0, filteredCount = 0;
            var showCollectionViewFooter = false;

            var filteredList = new ObservableCollection<CollectionModel>();

            // List filter declaration
            if (hiddenList != null)
            {
                totalCount = hiddenList.Count;

                await Task.WhenAll(hiddenList.Select(x => x.SetTotalBooks(ShowHiddenBooks)));
                await Task.WhenAll(hiddenList.Select(x => x.SetTotalCostOfBooks(ShowHiddenBooks)));

                // List filter calls
                filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

                if (filteredList != null)
                {
                    filteredCount = filteredList.Count;

                    showCollectionViewFooter = filteredCount > 0;

                    filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);
                }

                // Wait and assign list filters to declarations
            }

            var totalString = StringManipulation.SetTotalCollectionsString(filteredCount, totalCount);

            this.SetIsBusyFalse();
            this.SetRefreshView(false);

            return (totalCount, filteredCount, totalString, showCollectionViewFooter, filteredList);
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            int,
            int,
            string,
            bool,
            ObservableCollection<GenreModel>?)>
            SetViewModelData(ObservableCollection<GenreModel>? hiddenList, bool isNameChecked)
        {
            this.SetIsBusyTrue();

            int totalCount = 0, filteredCount = 0;
            var showCollectionViewFooter = false;

            var filteredList = new ObservableCollection<GenreModel>();

            // List filter declaration
            if (hiddenList != null)
            {
                totalCount = hiddenList.Count;

                await Task.WhenAll(hiddenList.Select(x => x.SetTotalBooks(ShowHiddenBooks)));
                await Task.WhenAll(hiddenList.Select(x => x.SetTotalCostOfBooks(ShowHiddenBooks)));

                // List filter calls
                filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

                if (filteredList != null)
                {
                    filteredCount = filteredList.Count;

                    showCollectionViewFooter = filteredCount > 0;

                    filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);
                }

                // Wait and assign list filters to declarations
            }

            var totalString = StringManipulation.SetTotalGenresString(filteredCount, totalCount);

            this.SetIsBusyFalse();
            this.SetRefreshView(false);

            return (totalCount, filteredCount, totalString, showCollectionViewFooter, filteredList);
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            int,
            int,
            string,
            bool,
            ObservableCollection<LocationModel>?)>
            SetViewModelData(ObservableCollection<LocationModel>? hiddenList, bool isNameChecked)
        {
            this.SetIsBusyTrue();

            int totalCount = 0, filteredCount = 0;
            var showCollectionViewFooter = false;

            var filteredList = new ObservableCollection<LocationModel>();

            // List filter declaration
            if (hiddenList != null)
            {
                totalCount = hiddenList.Count;

                await Task.WhenAll(hiddenList.Select(x => x.SetTotalBooks(ShowHiddenBooks)));
                await Task.WhenAll(hiddenList.Select(x => x.SetTotalCostOfBooks(ShowHiddenBooks)));

                // List filter calls
                filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

                if (filteredList != null)
                {
                    filteredCount = filteredList.Count;

                    showCollectionViewFooter = filteredCount > 0;

                    filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);
                }

                // Wait and assign list filters to declarations
            }

            var totalString = StringManipulation.SetTotalLocationsString(filteredCount, totalCount);

            this.SetIsBusyFalse();
            this.SetRefreshView(false);

            return (totalCount, filteredCount, totalString, showCollectionViewFooter, filteredList);
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <param name="hiddenList">Item list.</param>
        /// <param name="isNameChecked">Item named checked.</param>
        /// <returns>A series of values to set on the view.</returns>
        public async Task<(
            int,
            int,
            string,
            bool,
            ObservableCollection<SeriesModel>?)>
            SetViewModelData(ObservableCollection<SeriesModel>? hiddenList, bool isNameChecked)
        {
            this.SetIsBusyTrue();

            int totalCount = 0, filteredCount = 0;
            var showCollectionViewFooter = false;

            var filteredList = new ObservableCollection<SeriesModel>();

            // List filter declaration
            if (hiddenList != null)
            {
                totalCount = hiddenList.Count;

                await Task.WhenAll(hiddenList.Select(x => x.SetTotalBooks(ShowHiddenBooks)));
                await Task.WhenAll(hiddenList.Select(x => x.SetTotalCostOfBooks(ShowHiddenBooks)));

                // List filter calls
                filteredList = await FilterLists.FilterList(
                                 hiddenList!,
                                 this.SearchString);

                if (filteredList != null)
                {
                    filteredCount = filteredList.Count;

                    showCollectionViewFooter = filteredCount > 0;

                    filteredList = await SortLists.SortList(
                                filteredList!,
                                isNameChecked,
                                this.TotalBooksChecked,
                                this.TotalPriceChecked,
                                this.AscendingChecked,
                                this.DescendingChecked);
                }

                // Wait and assign list filters to declarations
            }

            var totalString = StringManipulation.SetTotalSeriesString(filteredCount, totalCount);

            this.SetIsBusyFalse();
            this.SetRefreshView(false);

            return (totalCount, filteredCount, totalString, showCollectionViewFooter, filteredList);
        }
    }
}
