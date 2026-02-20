// <copyright file="ChangeLogModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// ChangeLogModel class.
    /// </summary>
    public partial class ChangeLogModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether the version dropdown is open.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool versionOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the version dropdown is not open.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool versionNotOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeLogModel"/> class.
        /// </summary>
        public ChangeLogModel()
        {
            this.VersionNotOpen = true;
            this.VersionOpen = false;
        }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the changes.
        /// </summary>
        public string Changes { get; set; }

        /// <summary>
        /// Sets the properties to show and hide the arrows on the version dropdown.
        /// </summary>
        /// <param name="isExpanded">The value of if the expander is expanded or not.</param>
        [RelayCommand]
        public void VersionExpanderChanged(bool isExpanded)
        {
            this.VersionOpen = isExpanded;
            this.VersionNotOpen = !isExpanded;
        }
    }
}
