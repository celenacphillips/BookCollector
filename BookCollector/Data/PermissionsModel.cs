// <copyright file="PermissionsModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.Data
{
    public partial class PermissionsModel : ObservableObject
    {
        public string Permission { get; set; }

        public string Description { get; set; }

        [ObservableProperty]
        public bool permissionOpen;

        [ObservableProperty]
        public bool permissionNotOpen;

        public bool PermissionSectionValue { get; set; }

        public PermissionsModel()
        {
            this.PermissionOpen = false;
            this.PermissionNotOpen = true;
            this.PermissionSectionValue = false;
        }

        [RelayCommand]
        public void PermissionExpanderChanged()
        {
            this.PermissionOpen = this.PermissionSectionValue;
            this.PermissionNotOpen = !this.PermissionSectionValue;
        }
    }
}
