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
        public bool permissionOpen;

        [ObservableProperty]
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
