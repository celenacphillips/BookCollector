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

            RegisterRoutes();
            BookBaseViewModel.bookFormats = [$"{AppStringResources.eBook}", $"{AppStringResources.Paperback}", $"{AppStringResources.Hardcover}", $"{AppStringResources.Audiobook}"];

            //SettingPreferences();

            // Unit test data
            //var testData = new TestData();
            TestData.AddBooksToList();

            BindingContext = this;
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("BookEditView", typeof(Views.Book.BookEditView));
            Routing.RegisterRoute("BookMainView", typeof(Views.Book.BookMainView));
        }

        private void SettingPreferences ()
        {
        }
    }
}
