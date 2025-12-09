using BookCollector.Data;
using BookCollector.Resources.Localization;
using BookCollector.ViewModels;
using BookCollector.ViewModels.BaseViewModels;

namespace BookCollector
{
    public partial class AppShell : Shell
    {
        public string Year { get; set; }

        public AppShell()
        {
            Year = DateTime.Now.Year.ToString();
            InitializeComponent();

            AppShell.RegisterRoutes();
            BookBaseViewModel.bookFormats = [$"{AppStringResources.eBook}", $"{AppStringResources.Paperback}", $"{AppStringResources.Hardcover}", $"{AppStringResources.Audiobook}"];

            TestData.UseTestData = true;

            if (TestData.UseTestData)
            {
                //var testData = new TestData();
                TestData.AddBooksToList();
                TestData.AddWishListBooksToList();
            }

            BindingContext = this;
        }

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute("BookEditView", typeof(Views.Book.BookEditView));
            Routing.RegisterRoute("BookMainView", typeof(Views.Book.BookMainView));
        }
    }
}
