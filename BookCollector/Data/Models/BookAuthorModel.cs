// <copyright file="BookAuthorModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using SQLite;

    public class BookAuthorModel
    {
        [PrimaryKey]
        public Guid? BookAuthorGuid { get; set; }

        public Guid AuthorGuid { get; set; }

        public Guid BookGuid { get; set; }
    }
}
