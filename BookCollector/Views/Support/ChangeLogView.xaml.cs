// <copyright file="ChangeLogView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Resources.Localization;

namespace BookCollector.Views.Support;

public partial class ChangeLogView : ContentPage
{
    public List<ChangeLogModel> Versions { get; set; }

    public ChangeLogView()
    {
        this.Versions = new List<ChangeLogModel>();
        this.CreateVersionChangeLog();
        this.InitializeComponent();
        this.BindingContext = this;
    }

    private void CreateVersionChangeLog()
    {
        this.Versions.Add(new ChangeLogModel()
        {
            Version = "v 1.0.2",
            Changes = AppStringResources.v102_ChangeLogEntry,
        });

        this.Versions.Add(new ChangeLogModel()
        {
            Version = "v 1.0.1",
            Changes = AppStringResources.v101_ChangeLogEntry,
        });
    }
}