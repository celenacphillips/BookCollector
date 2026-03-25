// <copyright file="GenreModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;
    using BookCollector.ViewModels.BaseViewModels;

    /// <summary>
    /// GenreModel class.
    /// </summary>
    public partial class GenreModel : GenreDatabaseModel, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenreModel"/> class.
        /// </summary>
        public GenreModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenreModel"/> class.
        /// </summary>
        /// <param name="dbModel">Database model to convert from.</param>
        public GenreModel(GenreDatabaseModel dbModel)
        {
            this.GenreGuid = dbModel.GenreGuid;
            this.GenreName = dbModel.GenreName;
            this.TotalBooksString = dbModel.TotalBooksString;
            this.TotalCostOfBooks = dbModel.TotalCostOfBooks;
            this.HideGenre = dbModel.HideGenre;
            this.GenreTotalBooks = dbModel.GenreTotalBooks;
        }

        /// <summary>
        /// Gets the parsed genre name.
        /// </summary>
        public string? ParsedGenreName
        {
            get => StringManipulation.SetParsedName(this.GenreName);
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
            var list = await FillLists.GetAllBooksInGenreList(this.GenreGuid, showHiddenBooks);
            (this.TotalBooksString, this.GenreTotalBooks) = GroupingBaseViewModel.SetTotalBooksStringAndCounts(list);
        }

        /// <summary>
        /// Sets the total cost of books for the author, and updates the TotalCostOfBooks property accordingly.
        /// </summary>
        /// <param name="showHiddenBooks">Show hidden books.</param>
        /// <returns>A task.</returns>
        public async Task SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInGenreList(this.GenreGuid, showHiddenBooks);
        }
    }
}
