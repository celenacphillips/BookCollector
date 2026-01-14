// <copyright file="FAQModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookCollector.Data
{
    public partial class FAQModel : ObservableObject
    {
        public string Question { get; set; }

        public string Answer { get; set; }

        [ObservableProperty]
        public bool questionOpen;

        [ObservableProperty]
        public bool questionNotOpen;

        public FAQModel()
        {
            this.QuestionNotOpen = true;
            this.QuestionOpen = false;
        }

        [RelayCommand]
        public void QuestionExpanderChanged(bool isExpanded)
        {
            this.QuestionOpen = isExpanded;
            this.QuestionNotOpen = !isExpanded;
        }
    }
}
