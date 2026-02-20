// <copyright file="SeriesDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.DatabaseModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using SQLite;

    /// <summary>
    /// SeriesDatabaseModel class.
    /// </summary>
    public partial class SeriesDatabaseModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the series name.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? seriesName;

        /// <summary>
        /// Gets or sets the total number of books assigned to the series, formatted as a string.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalBooksString;

        /// <summary>
        /// Gets or sets the total books that are actually in the series, not what the user owns.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string? totalBooksInSeries;

        /// <summary>
        /// Gets or sets a value indicating whether the series is selected to hide.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool hideSeries;

        /// <summary>
        /// Gets or sets the series guid.
        /// </summary>
        [PrimaryKey]
        public Guid? SeriesGuid { get; set; }

        /// <summary>
        /// Gets or sets the total number of books assigned to the series.
        /// </summary>
        public int SeriesTotalBooks { get; set; }

        /// <summary>
        /// Gets or sets the total cost of books assigned to the series.
        /// </summary>
        public double TotalCostOfBooks { get; set; }
    }
}
