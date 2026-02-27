// <copyright file="FAQView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;
using BookCollector.Resources.Localization;

namespace BookCollector.Views.Support;

public partial class FAQView : ContentPage
{
    public List<FAQModel> Questions { get; set; }

    public FAQView()
    {
        this.Questions = new List<FAQModel>();
        this.CreateQuestions();
        this.InitializeComponent();
        this.BindingContext = this;
    }

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

        this.Questions.Add(new FAQModel()
        {
            Question = AppStringResources.FAQ_Question3,
            Answer = AppStringResources.FAQ_Answer3,
        });
    }
}