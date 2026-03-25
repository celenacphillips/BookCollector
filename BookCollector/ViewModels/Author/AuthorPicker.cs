// <copyright file="AuthorPicker.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.Author
{
    using System.Collections.ObjectModel;
    using BookCollector.Data.Models;
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// AuthorPicker class.
    /// </summary>
    public partial class AuthorPicker : ObservableObject
    {
        /// <summary>
        /// Gets or sets the selected author name.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public string selectedAuthorString;

        /// <summary>
        /// Gets or sets the author list for the picker.
        /// </summary>
        public ObservableCollection<AuthorModel>? AuthorList { get; set; }

        /// <summary>
        /// Gets or sets the selected author.
        /// </summary>
        public AuthorModel? SelectedAuthor { get; set; }
    }
}
