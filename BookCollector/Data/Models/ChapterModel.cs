// <copyright file="ChapterModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.DatabaseModels;

namespace BookCollector.Data.Models
{
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
