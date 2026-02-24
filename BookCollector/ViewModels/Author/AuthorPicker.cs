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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string selectedAuthorString;

        public ObservableCollection<AuthorModel>? AuthorList { get; set; }

        public AuthorModel? SelectedAuthor { get; set; }
    }
}
