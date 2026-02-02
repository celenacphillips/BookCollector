// <copyright file="ChapterDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.DatabaseModels
{
    public partial class ChapterDatabaseModel : ObservableObject
    {
        [ObservableProperty]
        public string? chapterName;
        [ObservableProperty]
        public string? pageRange;

        [PrimaryKey]
        public Guid? ChapterGuid { get; set; }

        public int ChapterOrder { get; set; }

        public Guid BookGuid { get; set; }
    }
}
