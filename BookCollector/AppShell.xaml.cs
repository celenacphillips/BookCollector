// <copyright file="AppShell.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector
{
    /// <summary>
    /// AppShell class.
    /// </summary>
    public partial class AppShell : Shell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppShell"/> class.
        /// </summary>
        public AppShell()
        {
            this.VersionString = $"v {AppInfo.VersionString}";
            this.ApplicationTitleString = $"{AppInfo.Current.Name}, {DateTime.Now.Year}";

            this.InitializeComponent();

            AppShell.RegisterRoutes();

            this.BindingContext = this;
        }

        /// <summary>
        /// Gets or sets the application title string, which includes the app name and current year.
        /// </summary>
        public string ApplicationTitleString { get; set; }

        /// <summary>
        /// Gets or sets the version string, which includes the app version number.
        /// </summary>
        public string VersionString { get; set; }

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute("BookEditView", typeof(Views.Book.BookEditView));
            Routing.RegisterRoute("BookMainView", typeof(Views.Book.BookMainView));
        }
    }
}
