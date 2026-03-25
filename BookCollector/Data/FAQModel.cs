// <copyright file="FAQModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// FAQModel class.
    /// </summary>
    public partial class FAQModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether the question dropdown is open.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool questionOpen;

        /// <summary>
        /// Gets or sets a value indicating whether the question dropdown is not open.
        /// </summary>
        [ObservableProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "Observable Property")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:Fields should be private", Justification = "Observable Property")]
        public bool questionNotOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="FAQModel"/> class.
        /// </summary>
        public FAQModel()
        {
            this.QuestionNotOpen = true;
            this.QuestionOpen = false;
        }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Sets the properties to show and hide the arrows on the question dropdown.
        /// </summary>
        /// <param name="isExpanded">The value of if the expander is expanded or not.</param>
        [RelayCommand]
        public void QuestionExpanderChanged(bool isExpanded)
        {
            this.QuestionOpen = isExpanded;
            this.QuestionNotOpen = !isExpanded;
        }
    }
}
