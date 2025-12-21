// <copyright file="GenreDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.DatabaseModels
{
    public partial class GenreDatabaseModel : ObservableObject
    {
        [ObservableProperty]
        public string? genreName;

        [ObservableProperty]
        public string? totalBooksString;

        [ObservableProperty]
        public bool hideGenre;

        [PrimaryKey]
        public Guid? GenreGuid { get; set; }

        public int GenreTotalBooks { get; set; }

        public double TotalCostOfBooks { get; set; }
    }
}
