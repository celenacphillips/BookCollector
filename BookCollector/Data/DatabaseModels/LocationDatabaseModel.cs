// <copyright file="LocationDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.DatabaseModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using SQLite;

    /// <summary>
    /// LocationDatabaseModel class.
    /// </summary>
    public partial class LocationDatabaseModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the location name.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? locationName;

        /// <summary>
        /// Gets or sets the total number of books assigned to the location, formatted as a string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalBooksString;

        /// <summary>
        /// Gets or sets a value indicating whether the location is selected to hide.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool hideLocation;

        /// <summary>
        /// Gets or sets the location guid.
        /// </summary>
        [PrimaryKey]
        public Guid? LocationGuid { get; set; }

        /// <summary>
        /// Gets or sets the total number of books assigned to the location.
        /// </summary>
        public int LocationTotalBooks { get; set; }

        /// <summary>
        /// Gets or sets the total cost of books assigned to the location.
        /// </summary>
        public double TotalCostOfBooks { get; set; }
    }
}
