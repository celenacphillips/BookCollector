// <copyright file="AppShell.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector
{
    using BookCollector.Data;
    using BookCollector.Data.Database;
    using BookCollector.Resources.Localization;
    using BookCollector.ViewModels.BaseViewModels;

    /// <summary>
    /// App Shell class.
    /// </summary>
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            this.Year = DateTime.Now.Year.ToString();
            this.InitializeComponent();

            AppShell.RegisterRoutes();
            BookBaseViewModel.bookFormats = [$"{AppStringResources.eBook}", $"{AppStringResources.Paperback}", $"{AppStringResources.Hardcover}", $"{AppStringResources.Audiobook}"];

            TestData.UseTestData = false;

            if (TestData.UseTestData)
            {
                // var testData = new TestData();
                TestData.AddBooksToList();
                TestData.AddWishListBooksToList();
            }
            else
            {
                BaseViewModel.Database = new BookCollectorDatabase();
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
