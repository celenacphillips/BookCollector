// <copyright file="GetItems.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    using BookCollector.Data.Models;
    using BookCollector.ViewModels.BaseViewModels;

    /// <summary>
    /// GetItems class.
    /// </summary>
    public partial class GetItems : BaseViewModel
    {
        /// <summary>
        /// Get genre for book.
        /// </summary>
        /// <param name="inputGuid">Genre guid.</param>
        /// <returns>Selected genre.</returns>
        public static async Task<GenreModel?> GetGenreForBook(Guid? inputGuid)
        {
            GenreModel? genre = null;

            if (inputGuid != null)
            {
                genre = await Database.GetGenreForBookAsync((Guid)inputGuid);
            }

            return genre;
        }

        /// <summary>
        /// Get location for book.
        /// </summary>
        /// <param name="inputGuid">Location guid.</param>
        /// <returns>Selected location.</returns>
        public static async Task<LocationModel?> GetLocationForBook(Guid? inputGuid)
        {
            LocationModel? location = null;

            if (inputGuid != null)
            {
                location = await Database.GetLocationForBookAsync((Guid)inputGuid);
            }

            return location;
        }

        /// <summary>
        /// Get series for book.
        /// </summary>
        /// <param name="inputGuid">Series guid.</param>
        /// <returns>Selected series.</returns>
        public static async Task<SeriesModel?> GetSeriesForBook(Guid? inputGuid)
        {
            SeriesModel? series = null;

            if (inputGuid != null)
            {
                series = await Database.GetSeriesForBookAsync((Guid)inputGuid);
            }

            return series;
        }

        /// <summary>
        /// Get collection for book.
        /// </summary>
        /// <param name="inputGuid">Collection guid.</param>
        /// <returns>Selected collection.</returns>
        public static async Task<CollectionModel?> GetCollectionForBook(Guid? inputGuid)
        {
            CollectionModel? collection = null;

            if (inputGuid != null)
            {
                collection = await Database.GetCollectionForBookAsync((Guid)inputGuid);
            }

            return collection;
        }
    }
}
