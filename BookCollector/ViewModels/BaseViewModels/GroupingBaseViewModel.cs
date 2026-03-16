// <copyright file="GroupingBaseViewModel.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.ViewModels.BaseViewModels
{
    using System.Collections;

    /// <summary>
    /// GroupingBaseViewModel class.
    /// </summary>
    public partial class GroupingBaseViewModel : BookBaseViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether total books is checked or not.
        /// </summary>
        public bool TotalBooksChecked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether total price is checked or not.
        /// </summary>
        public bool TotalPriceChecked { get; set; }
    }
}
