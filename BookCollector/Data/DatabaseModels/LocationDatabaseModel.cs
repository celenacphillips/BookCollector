// <copyright file="LocationDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.DatabaseModels
{
    public partial class LocationDatabaseModel : ObservableObject
    {
        [ObservableProperty]
        public string? locationName;

        [ObservableProperty]
        public string? totalBooksString;

        [ObservableProperty]
        public bool hideLocation;

        [PrimaryKey]
        public Guid? LocationGuid { get; set; }

        public int LocationTotalBooks { get; set; }

        public double TotalCostOfBooks { get; set; }
    }
}
