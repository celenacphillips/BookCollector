// <copyright file="ChapterModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Models
{
    using BookCollector.Data.DatabaseModels;

    public partial class ChapterModel : ChapterDatabaseModel, ICloneable
    {
        public ChapterModel()
        {
        }

        public ChapterModel(ChapterDatabaseModel dbModel)
        {
            this.ChapterGuid = dbModel.ChapterGuid;
            this.BookGuid = dbModel.BookGuid;
            this.ChapterName = dbModel.ChapterName;
            this.PageRange = dbModel.PageRange;
            this.ChapterOrder = dbModel.ChapterOrder;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
