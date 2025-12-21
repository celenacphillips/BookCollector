// <copyright file="AuthorDatabaseModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace BookCollector.Data.DatabaseModels
{
    /// <summary>
    /// Author Database Model class.
    /// </summary>
    public partial class AuthorDatabaseModel : ObservableObject
    {
        [ObservableProperty]
        public string firstName;

        [ObservableProperty]
        public string lastName;

        [ObservableProperty]
        public string? totalBooksString;

        [PrimaryKey]
        public Guid? AuthorGuid { get; set; }

        public int AuthorTotalBooks { get; set; }

        public double TotalCostOfBooks { get; set; }

        public bool HideAuthor { get; set; }
    }
}
