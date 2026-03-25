// <copyright file="CollectionDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.DatabaseModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using SQLite;

    /// <summary>
    /// CollectionDatabaseModel class.
    /// </summary>
    public partial class CollectionDatabaseModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the collection name.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? collectionName;

        /// <summary>
        /// Gets or sets the total number of books assigned to the collection, formatted as a string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalBooksString;

        /// <summary>
        /// Gets or sets a value indicating whether the collection is selected to hide.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool hideCollection;

        /// <summary>
        /// Gets or sets the collection guid.
        /// </summary>
        [PrimaryKey]
        public Guid? CollectionGuid { get; set; }

        /// <summary>
        /// Gets or sets the total number of books assigned to the collection.
        /// </summary>
        public int CollectionTotalBooks { get; set; }

        /// <summary>
        /// Gets or sets the total cost of books assigned to the collection.
        /// </summary>
        public double TotalCostOfBooks { get; set; }
    }
}
