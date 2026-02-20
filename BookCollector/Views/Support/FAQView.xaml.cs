// <copyright file="FAQView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Views.Support;

using BookCollector.Data;
using BookCollector.Resources.Localization;

/// <summary>
/// FAQView class.
/// </summary>
public partial class FAQView : ContentPage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FAQView"/> class.
    /// </summary>
    public FAQView()
    {
        this.Questions = [];
        this.CreateQuestions();
        this.InitializeComponent();
        this.BindingContext = this;
    }

    /// <summary>
    /// Gets or sets the list of questions.
    /// </summary>
    public List<FAQModel> Questions { get; set; }

    private void CreateQuestions()
    {
        this.Questions.Add(new FAQModel()
        {
            Question = AppStringResources.FAQ_Question1,
            Answer = AppStringResources.FAQ_Answer1.Replace("api", "Google Books"),
        });

        this.Questions.Add(new FAQModel()
        {
            Question = AppStringResources.FAQ_Question2,
            Answer = AppStringResources.FAQ_Answer2,
        });
    }
}