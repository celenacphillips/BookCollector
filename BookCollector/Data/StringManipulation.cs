
using BookCollector.Data.Models;
using BookCollector.Resources.Localization;
using System.Collections.ObjectModel;

namespace BookCollector.Data
{
    internal class StringManipulation
    {
        public static string SetTotalBooksString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Book.ToLower() : AppStringResources.Books.ToLower() );
        }

        public static string SetTotalCollectionsString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Collection.ToLower() : AppStringResources.Collections.ToLower() );
        }

        public static string SetTotalGenresString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Genre.ToLower() : AppStringResources.Genres.ToLower() );
        }

        public static string SetTotalSeriesString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Series.ToLower() : AppStringResources.Series.ToLower() );
        }

        public static string SetTotalAuthorsString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Author.ToLower() : AppStringResources.Authors.ToLower() );
        }

        public static string SetTotalLocationsString(int filteredCount, int totalCount)
        {
            return AppStringResources.Blank1OfBlank2Items.Replace("Blank1", $"{filteredCount}").Replace("Blank2", $"{totalCount}").Replace("items", totalCount == 1 ? AppStringResources.Location.ToLower() : AppStringResources.Locations.ToLower() );
        }
    }
}
