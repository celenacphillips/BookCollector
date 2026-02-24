// <copyright file="PermissionsModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    public partial class PermissionsModel : ObservableObject
    {
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool permissionOpen;

        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool permissionNotOpen;

        public PermissionsModel()
        {
            this.PermissionOpen = false;
            this.PermissionNotOpen = true;
            this.PermissionSectionValue = false;
        }

        public string Permission { get; set; }

        public string Description { get; set; }

        public bool PermissionSectionValue { get; set; }

        [RelayCommand]
        public void PermissionExpanderChanged()
        {
            this.PermissionOpen = this.PermissionSectionValue;
            this.PermissionNotOpen = !this.PermissionSectionValue;
        }
    }
}
