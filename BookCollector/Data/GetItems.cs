// <copyright file="GetItems.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using BookCollector.Data.Models;
using BookCollector.ViewModels.BaseViewModels;

namespace BookCollector.Data
{
    public partial class GetItems : BaseViewModel
    {
        public static async Task<GenreModel?> GetGenreForBook(Guid? inputGuid)
        {
            GenreModel? genre = null;

            if (inputGuid != null)
            {
                genre = await Database.GetGenreForBookAsync((Guid)inputGuid);
            }

            return genre;
        }

        public static async Task<LocationModel?> GetLocationForBook(Guid? inputGuid)
        {
            LocationModel? location = null;

            if (inputGuid != null)
            {
                location = await Database.GetLocationForBookAsync((Guid)inputGuid);
            }

            return location;
        }

        public static async Task<SeriesModel?> GetSeriesForBook(Guid? inputGuid)
        {
            SeriesModel? series = null;

            if (inputGuid != null)
            {
                series = await Database.GetSeriesForBookAsync((Guid)inputGuid);
            }

            return series;
        }

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
