// <copyright file="ChapterModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;

    /// <summary>
    /// ChapterModel class.
    /// </summary>
    public partial class ChapterModel : ChapterDatabaseModel, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChapterModel"/> class.
        /// </summary>
        public ChapterModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChapterModel"/> class.
        /// </summary>
        /// <param name="dbModel">Database model to convert from.</param>
        public ChapterModel(ChapterDatabaseModel dbModel)
        {
            this.ChapterGuid = dbModel.ChapterGuid;
            this.BookGuid = dbModel.BookGuid;
            this.ChapterName = dbModel.ChapterName;
            this.PageRange = dbModel.PageRange;
            this.ChapterOrder = dbModel.ChapterOrder;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
