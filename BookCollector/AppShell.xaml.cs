namespace BookCollector
{
    using BookCollector.Data;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;

    public partial class AppShell : Shell
    {
        public AppShell()
        {
            this.Year = DateTime.Now.Year.ToString();
            this.InitializeComponent();

            AppShell.RegisterRoutes();
            BookBaseViewModel.bookFormats = [$"{AppStringResources.eBook}", $"{AppStringResources.Paperback}", $"{AppStringResources.Hardcover}", $"{AppStringResources.Audiobook}"];

            TestData.UseTestData = true;

            if (TestData.UseTestData)
            {
                // var testData = new TestData();
                TestData.AddBooksToList();
                TestData.AddWishListBooksToList();
            }

            this.BindingContext = this;
        }

        public string Year { get; set; }

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute("BookEditView", typeof(Views.Book.BookEditView));
            Routing.RegisterRoute("BookMainView", typeof(Views.Book.BookMainView));
        }
    }
}
