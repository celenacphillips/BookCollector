// <copyright file="AuthorDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.DatabaseModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using SQLite;

    /// <summary>
    /// AuthorDatabaseModel class.
    /// </summary>
    public partial class AuthorDatabaseModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string firstName;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string lastName;

        /// <summary>
        /// Gets or sets the total number of books assigned to the author, formatted as a string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalBooksString;

        /// <summary>
        /// Gets or sets the author guid.
        /// </summary>
        [PrimaryKey]
        public Guid? AuthorGuid { get; set; }

        /// <summary>
        /// Gets or sets the total number of books assigned to the author.
        /// </summary>
        public int AuthorTotalBooks { get; set; }

        /// <summary>
        /// Gets or sets the total cost of books assigned to the author.
        /// </summary>
        public double TotalCostOfBooks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the author is selected to hide.
        /// </summary>
        public bool HideAuthor { get; set; }
    }
}
