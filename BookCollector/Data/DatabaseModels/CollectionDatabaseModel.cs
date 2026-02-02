// <copyright file="CollectionDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.DatabaseModels
{
    public partial class CollectionDatabaseModel : ObservableObject
    {
        [ObservableProperty]
        public string? collectionName;

        [ObservableProperty]
        public string? totalBooksString;

        [ObservableProperty]
        public bool hideCollection;

        [PrimaryKey]
        public Guid? CollectionGuid { get; set; }

        public int CollectionTotalBooks { get; set; }

        public double TotalCostOfBooks { get; set; }
    }
}
