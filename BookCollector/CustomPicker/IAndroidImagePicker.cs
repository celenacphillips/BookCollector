// <copyright file="IAndroidImagePicker.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

#if ANDROID
namespace BookCollector.CustomPicker
{
    /// <summary>
    /// IAndroidImagePicker class.
    /// </summary>
    public interface IAndroidImagePicker
    {
        /// <summary>
        /// Creates a picker that allows the user to select an image from their device.
        /// </summary>
        /// <returns>The Uri of the selected image.</returns>
        Task<Android.Net.Uri> PickImageAsync();

        /// <summary>
        /// Creates a picker that allows the user to select multiple images from their device.
        /// </summary>
        /// <returns>A list of Uris for the selected images.</returns>
        Task<List<Android.Net.Uri>> PickImagesAsync();
    }
}
#endif