// <copyright file="ChartValues.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    /// <summary>
    /// ChartValues class.
    /// </summary>
    public class ChartValues
    {
        /// <summary>
        /// Gets or sets the color values.
        /// </summary>
        public Color? ColorValue { get; set; }

        /// <summary>
        /// Gets or sets the label value.
        /// </summary>
        public string? LabelValue { get; set; }

        /// <summary>
        /// Gets or sets the number value.
        /// </summary>
        public float Value { get; set; }
    }
}
