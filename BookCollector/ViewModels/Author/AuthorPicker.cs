// <copyright file="AuthorPicker.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BookCollector.ViewModels.Author
{
    public partial class AuthorPicker : ObservableObject
    {
        public ObservableCollection<AuthorModel>? AuthorList { get; set; }

        public AuthorModel? SelectedAuthor { get; set; }

        [ObservableProperty]
        public string selectedAuthorString;
    }
}
