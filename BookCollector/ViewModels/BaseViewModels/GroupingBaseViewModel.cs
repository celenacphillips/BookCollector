// <copyright file="GroupingBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using BookCollector.Data;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.ViewModels.Popups;
    using DocumentFormat.OpenXml.Office2010.ExcelAc;
    using System.Collections.ObjectModel;

    /// <summary>
    /// GroupingBaseViewModel class.
    /// </summary>
    public partial class GroupingBaseViewModel : BookListBaseViewModel
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
                                    StringManipulation.SetTotalBooksString(count, int.Parse(totalBooks), unread) :
                                    StringManipulation.SetTotalBooksAndUnreadString(count, unread);

            return (totalBooksString, count);
        }

        /// <summary>
        /// Set the first filtered list based on the full  list and the show hidden preference.
        /// </summary>
        /// <param name="showHidden">Show hidden.</param>
        /// <returns>A task.</returns>
        public async override Task SetList(bool showHidden)
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
        /// Check if the list is null.
        /// </summary>
        /// <returns>If the list is null.</returns>
        public override bool ListNullCheck()
        {
            return true;
        }

        /// <summary>
        /// Iterate through the list and set necessary data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetListData()
        {
        }

        /// <summary>
        /// Find filters for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetFilters()
        {
        }

        /// <summary>
        /// Find sort values for the list.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetSorts()
        {
        }

        /// <summary>
        /// Set data for view.
        /// </summary>
        public async override void SetViewStrings()
        {
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
    }
}
