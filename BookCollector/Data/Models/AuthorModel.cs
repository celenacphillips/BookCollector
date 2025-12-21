// <copyright file="AuthorModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.DatabaseModels;

namespace BookCollector.Data.Models
{
    public partial class AuthorModel : AuthorDatabaseModel, ICloneable
    {
        public AuthorModel()
        {
        }

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

        public string FullName
        {
            get => $"{this.FirstName} {this.LastName}";
        }

        public string ReverseFullName
        {
            get => $"{this.LastName}, {this.FirstName}";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public async void SetTotalBooks(bool showHiddenBooks)
        {
            var list = await FillLists.GetAllBooksInAuthorList(this.AuthorGuid, showHiddenBooks);
            var count = 0;

            if (list != null)
            {
                count = list.Count;
            }

            this.TotalBooksString = StringManipulation.SetTotalBooksString(count);
            this.AuthorTotalBooks = count;
        }

        public async void SetTotalCostOfBooks(bool showHiddenBooks)
        {
            this.TotalCostOfBooks = await GetCounts.GetAllBookPricesInAuthorList(this.AuthorGuid, showHiddenBooks);
        }
    }
}
