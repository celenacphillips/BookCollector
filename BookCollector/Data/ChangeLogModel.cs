// <copyright file="ChangeLogModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using CommunityToolkit.Mvvm.ComponentModel;

    /// <summary>
    /// ChangeLogModel class.
    /// </summary>
    public partial class ChangeLogModel : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeLogModel"/> class.
        /// </summary>
        public ChangeLogModel()
        {
        }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the changes.
        /// </summary>
        public string Changes { get; set; }
    }
}
