// <copyright file="PermissionsModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// PermissionsModel class.
    /// </summary>
    public partial class PermissionsModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether the expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool permissionOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the expander is open or not.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool permissionNotOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsModel"/> class.
        /// </summary>
        public PermissionsModel()
        {
            this.PermissionOpen = false;
            this.PermissionNotOpen = true;
            this.PermissionSectionValue = false;
        }

        /// <summary>
        /// Gets or sets the permission name.
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// Gets or sets the description of the permission.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the expander is open or not.
        /// </summary>
        public bool PermissionSectionValue { get; set; }

        /// <summary>
        /// Sets the expander arrow boolean values on change.
        /// </summary>
        [RelayCommand]
        public void PermissionExpanderChanged()
        {
            this.PermissionOpen = this.PermissionSectionValue;
            this.PermissionNotOpen = !this.PermissionSectionValue;
        }
    }
}
