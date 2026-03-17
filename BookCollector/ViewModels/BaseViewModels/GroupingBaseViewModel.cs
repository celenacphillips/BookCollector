// <copyright file="GroupingBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.DatabaseModels;
    using BookCollector.Data.Models;
    using BookCollector.ViewModels.Popups;

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
        /// Set the first filtered list based on the full  list and the show hidden preference.
        /// </summary>
        /// <param name="showHidden">Show hidden.</param>
        /// <returns>A task.</returns>
        public async override Task SetList(bool showHidden)
        {
        }

        /// <summary>
        /// Set the view model data.
        /// </summary>
        /// <returns>A task.</returns>
        public async override Task SetViewModelData()
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
