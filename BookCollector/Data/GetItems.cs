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
                if (TestData.UseTestData)
                {
                    genre = TestData.GetGenreForBook(inputGuid);
                }
                else
                {
                    genre = await Database.GetGenreForBookAsync((Guid)inputGuid);
                }
            }

            return genre;
        }

        public static async Task<LocationModel?> GetLocationForBook(Guid? inputGuid)
        {
            LocationModel? location = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    location = TestData.GetLocationForBook(inputGuid);
                }
                else
                {
                    location = await Database.GetLocationForBookAsync((Guid)inputGuid);
                }
            }

            return location;
        }

        public static async Task<SeriesModel?> GetSeriesForBook(Guid? inputGuid)
        {
            SeriesModel? series = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    series = TestData.GetSeriesForBook(inputGuid);
                }
                else
                {
                    series = await Database.GetSeriesForBookAsync((Guid)inputGuid);
                }
            }

            return series;
        }

        public static async Task<CollectionModel?> GetCollectionForBook(Guid? inputGuid)
        {
            CollectionModel? collection = null;

            if (inputGuid != null)
            {
                if (TestData.UseTestData)
                {
                    collection = TestData.GetCollectionForBook(inputGuid);
                }
                else
                {
                    collection = await Database.GetCollectionForBookAsync((Guid)inputGuid);
                }
            }

            return collection;
        }
    }
}
