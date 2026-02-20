// <copyright file="AuthorPicker.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Author
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.Models;
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class AuthorPicker : ObservableObject
    {
        [ObservableProperty]
        public string selectedAuthorString;

        public ObservableCollection<AuthorModel>? AuthorList { get; set; }

        public AuthorModel? SelectedAuthor { get; set; }
    }
}
