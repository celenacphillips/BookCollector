// <copyright file="PermissionsView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Support;

using BookCollector.Data;
using BookCollector.Resources.Localization;

/// <summary>
/// PermissionsView class.
/// </summary>
public partial class PermissionsView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PermissionsView"/> class.
    /// </summary>
    public PermissionsView()
    {
        this.Permissions = [];
        this.CreatePermissions();
        this.InitializeComponent();
        this.BindingContext = this;
    }

    /// <summary>
    /// Gets or sets the list of permissions.
    /// </summary>
    public List<PermissionsModel> Permissions { get; set; }

    private void CreatePermissions()
    {
        this.Permissions.Add(
        new PermissionsModel()
        {
            Permission = AppStringResources.Camera,
            Description = AppStringResources.Camera_Description,
        });

        this.Permissions.Add(
        new PermissionsModel()
        {
            Permission = AppStringResources.Internet,
            Description = AppStringResources.Internet_Description,
        });

        this.Permissions.Add(
        new PermissionsModel()
        {
            Permission = AppStringResources.Files_Photos,
            Description = AppStringResources.Files_Photos_Description,
        });
    }
}