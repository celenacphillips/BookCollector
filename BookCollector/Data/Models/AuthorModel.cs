// <copyright file="AuthorModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;
    using BookCollector.ViewModels.BaseViewModels;

    /// <summary>
    /// AuthorModel class.
    /// </summary>
    public partial class AuthorModel : AuthorDatabaseModel, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorModel"/> class.
        /// </summary>
        public AuthorModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorModel"/> class.
        /// </summary>
        /// <param name="dbModel">Database model to convert from.</param>
        public AuthorModel(AuthorDatabaseModel dbModel)
        {
            this.AuthorGuid = dbModel.AuthorGuid;
            this.FirstName = dbModel.FirstName;
            this.LastName = dbModel.LastName;
            this.TotalBooksString = dbModel.TotalBooksString;
            this.TotalCostOfBooks = dbModel.TotalCostOfBooks;
            this.HideAuthor = dbModel.HideAuthor;
            this.AuthorTotalBooks = dbModel.AuthorTotalBooks;
        }

        /// <summary>
        /// Gets the full name of the author, combining the first name and last name.
        /// </summary>
        public string FullName
        {
            get => $"{this.FirstName} {this.LastName}";
        }

        /// <summary>
        /// Gets the reverse full name of the author, combining the last name and first name, separated by a comma.
        /// </summary>
        public string ReverseFullName
        {
            get => $"{this.LastName}, {this.FirstName}";
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Sets the total books and unread books for the author, and updates the TotalBooksString property accordingly.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
        public async Task SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FillLists.GetAllBooksInAuthorList(this.AuthorGuid, showHiddenBooks);
            (this.TotalBooksString, this.AuthorTotalBooks) = GroupingBaseViewModel.SetTotalBooksStringAndCounts(list);
        }

        /// <summary>
        /// Sets the total cost of books for the author, and updates the TotalCostOfBooks property accordingly.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
        public async Task SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInAuthorList(this.AuthorGuid, showHiddenBooks);
        }
    }
}
