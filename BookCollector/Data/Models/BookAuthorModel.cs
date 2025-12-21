// <copyright file="BookAuthorModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using SQLite;

namespace BookCollector.Data.Models
{
    public class BookAuthorModel
    {
        [PrimaryKey]
        public Guid? BookAuthorGuid { get; set; }

        public Guid AuthorGuid { get; set; }

        public Guid BookGuid { get; set; }
    }
}
