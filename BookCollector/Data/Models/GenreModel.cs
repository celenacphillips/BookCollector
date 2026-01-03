// <copyright file="GenreModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.DatabaseModels;
using System.Threading;

namespace BookCollector.Data.Models
{
    public partial class GenreModel : GenreDatabaseModel, ICloneable
    {
        public GenreModel()
        {
        }

        public GenreModel(GenreDatabaseModel dbModel)
        {
            this.GenreGuid = dbModel.GenreGuid;
            this.GenreName = dbModel.GenreName;
            this.TotalBooksString = dbModel.TotalBooksString;
            this.TotalCostOfBooks = dbModel.TotalCostOfBooks;
            this.HideGenre = dbModel.HideGenre;
            this.GenreTotalBooks = dbModel.GenreTotalBooks;
        }

        public string? ParsedGenreName
        {
            get => (!string.IsNullOrEmpty(this.GenreName) &&
                    (this.GenreName.StartsWith("the ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.GenreName.StartsWith("a ", StringComparison.CurrentCultureIgnoreCase) ||
                    this.GenreName.StartsWith("an ", StringComparison.CurrentCultureIgnoreCase)))
                        ? this.GenreName[(this.GenreName.IndexOf(' ') + 1) ..]
                        : this.GenreName;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async Task SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FillLists.GetAllBooksInGenreList(this.GenreGuid, showHiddenBooks);
            var count = 0;
            var unread = 0;

            if (list != null)
            {
                count = list.Count;
                unread = list.Count(x => x.BookPageRead == 0 && !x.UpNext);
            }

            this.TotalBooksString = StringManipulation.SetTotalBooksAndUnreadString(count, unread);
            this.GenreTotalBooks = count;
        }

        public async Task SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInGenreList(this.GenreGuid, showHiddenBooks);
        }
    }
}
