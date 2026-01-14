// <copyright file="FAQView.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data;

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
            Question = "Question 1",
            Answer = "Answer 1",
        });
    }
}