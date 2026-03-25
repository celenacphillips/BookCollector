// <copyright file="GenreDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.DatabaseModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using SQLite;

    /// <summary>
    /// GenreDatabaseModel class.
    /// </summary>
    public partial class GenreDatabaseModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the genre name.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? genreName;

        /// <summary>
        /// Gets or sets the total number of books assigned to the genre, formatted as a string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalBooksString;

        /// <summary>
        /// Gets or sets a value indicating whether the genre is selected to hide.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool hideGenre;

        /// <summary>
        /// Gets or sets the genre guid.
        /// </summary>
        [PrimaryKey]
        public Guid? GenreGuid { get; set; }

        /// <summary>
        /// Gets or sets the total number of books assigned to the genre.
        /// </summary>
        public int GenreTotalBooks { get; set; }

        /// <summary>
        /// Gets or sets the total cost of books assigned to the genre.
        /// </summary>
        public double TotalCostOfBooks { get; set; }
    }
}
