// <copyright file="BookAuthorModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using SQLite;

    /// <summary>
    /// BookAuthorModel class.
    /// </summary>
    public class BookAuthorModel
    {
        /// <summary>
        /// Gets or sets the book author guid.
        /// </summary>
        [PrimaryKey]
        public Guid? BookAuthorGuid { get; set; }

        /// <summary>
        /// Gets or sets the author guid.
        /// </summary>
        public Guid AuthorGuid { get; set; }

        /// <summary>
        /// Gets or sets the book guid.
        /// </summary>
        public Guid BookGuid { get; set; }
    }
}
