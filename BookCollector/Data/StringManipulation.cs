using BookCollector.Resources.Localization;

namespace BookCollector.Data
{
    internal class StringManipulation
    {

        public static string SetTotalBooksString(int filteredBooksCount, int totalBooksCount)
        {
            return AppStringResources.Blank1OfBlank2Books.Replace("Blank1", $"{filteredBooksCount}").Replace("Blank2", $"{totalBooksCount}").Replace("books", totalBooksCount == 1 ? "book" : "books");
        }
    }
}
