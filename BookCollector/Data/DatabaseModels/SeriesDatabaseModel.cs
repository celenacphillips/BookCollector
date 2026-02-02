// <copyright file="SeriesDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.DatabaseModels
{
    public partial class SeriesDatabaseModel : ObservableObject
    {
        [ObservableProperty]
        public string? seriesName;

        [ObservableProperty]
        public string? totalBooksString;

        [ObservableProperty]
        public string? totalBooksInSeries;

        [ObservableProperty]
        public bool hideSeries;

        [PrimaryKey]
        public Guid? SeriesGuid { get; set; }

        public int SeriesTotalBooks { get; set; }

        public double TotalCostOfBooks { get; set; }
    }
}
