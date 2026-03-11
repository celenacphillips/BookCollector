// <copyright file="CountModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    /// <summary>
    /// CountModel class.
    /// </summary>
    public class CountModel
    {
        /// <summary>
        /// Gets or sets the count integer value.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the count double value.
        /// </summary>
        public double CountDouble { get; set; }

        /// <summary>
        /// Gets or sets the label value.
        /// </summary>
        public string? Label { get; set; }
    }
}
