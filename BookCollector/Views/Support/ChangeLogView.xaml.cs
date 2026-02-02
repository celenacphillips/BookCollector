// <copyright file="ChangeLogView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;

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
            Version = "Question 1",
            Changes = "Answer 1",
        });
    }
}