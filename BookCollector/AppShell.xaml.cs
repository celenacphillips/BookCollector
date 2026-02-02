// <copyright file="AppShell.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector
{
    /// <summary>
    /// App Shell class.
    /// </summary>
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            this.VersionString = $"v {AppInfo.VersionString}";
            this.ApplicationTitleString = $"{AppInfo.Current.Name}, {DateTime.Now.Year}";

            this.InitializeComponent();

            AppShell.RegisterRoutes();

            this.BindingContext = this;
        }

        public string ApplicationTitleString { get; set; }

        public string VersionString { get; set; }

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute("BookEditView", typeof(Views.Book.BookEditView));
            Routing.RegisterRoute("BookMainView", typeof(Views.Book.BookMainView));
        }
    }
}
