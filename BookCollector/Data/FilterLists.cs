using BookCollector.Data.Models;
using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

namespace BookCollector.Data
{
    internal class FilterLists
    {
        public static async Task<ObservableCollection<BookModel>> GetReadingBooksList(ObservableCollection<BookModel> bookList)
        {
            return bookList.Where(x => (x.BookPageRead != x.BookPageTotal &&
                                   x.BookPageRead != 0) ||
                                   (x.UpNext == true)).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetToBeReadBooksList(ObservableCollection<BookModel> bookList)
        {
            return bookList.Where(x => x.BookPageRead == 0 && x.UpNext == false).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetReadBooksList(ObservableCollection<BookModel> bookList)
        {
            return bookList.Where(x => x.BookPageRead == x.BookPageTotal &&
                                   x.BookPageRead != 0).ToObservableCollection();
        }

        public static async Task<ObservableCollection<BookModel>> GetAllBooksList(ObservableCollection<BookModel> bookList)
        {
            return bookList.ToObservableCollection();
        }
    }
}
