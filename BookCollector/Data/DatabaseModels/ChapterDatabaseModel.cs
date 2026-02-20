// <copyright file="ChapterDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.DatabaseModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using SQLite;

    /// <summary>
    /// ChapterDatabaseModel class.
    /// </summary>
    public partial class ChapterDatabaseModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the chapter name.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? chapterName;

        /// <summary>
        /// Gets or sets the chapter page range.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? pageRange;

        /// <summary>
        /// Gets or sets the chapter guid.
        /// </summary>
        [PrimaryKey]
        public Guid? ChapterGuid { get; set; }

        /// <summary>
        /// Gets or sets the chapter order.
        /// </summary>
        public int ChapterOrder { get; set; }

        /// <summary>
        /// Gets or sets the book guide assigned.
        /// </summary>
        public Guid BookGuid { get; set; }
    }
}
