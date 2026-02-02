// <copyright file="ChangeLogModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.Data
{
    public partial class ChangeLogModel : ObservableObject
    {
        public string Version { get; set; }

        public string Changes { get; set; }

        [ObservableProperty]
        public bool versionOpen;

        [ObservableProperty]
        public bool versionNotOpen;

        public ChangeLogModel()
        {
            this.VersionNotOpen = true;
            this.VersionOpen = false;
        }

        [RelayCommand]
        public void VersionExpanderChanged(bool isExpanded)
        {
            this.VersionOpen = isExpanded;
            this.VersionNotOpen = !isExpanded;
        }
    }
}
