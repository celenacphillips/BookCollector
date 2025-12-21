// <copyright file="AuthorPicker.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using BookCollector.Data.Models;

namespace BookCollector.ViewModels.Author
{
    public class AuthorPicker
    {
        public ObservableCollection<AuthorModel>? AuthorList { get; set; }

        public AuthorModel? SelectedAuthor { get; set; }
    }
}
