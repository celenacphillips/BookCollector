// <copyright file="ChangeLogView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Support;

using BookCollector.Data;
using BookCollector.Resources.Localization;

/// <summary>
/// ChangeLogView class.
/// </summary>
public partial class ChangeLogView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeLogView"/> class.
    /// </summary>
    public ChangeLogView()
    {
        this.Versions = [];
        this.CreateVersionChangeLog();
        this.InitializeComponent();
        this.BindingContext = this;
    }

    /// <summary>
    /// Gets or sets the list of versions.
    /// </summary>
    public List<ChangeLogModel> Versions { get; set; }

    private void CreateVersionChangeLog()
    {
        this.Versions.Add(new ChangeLogModel()
        {
            Version = AppInfo.VersionString,
            Changes = AppStringResources.ChangeLogEntry,
        });
    }
}