// <copyright file="AppShell.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector
{
    using BookCollector.Data;
    using BookCollector.Data.Enums;
    using BookCollector.Resources.Localization;
    using BookCollector.Views.Groupings;
    using BookCollector.Views.Library;

    /// <summary>
    /// AppShell class.
    /// </summary>
    public partial class AppShell : Shell
    {
        private bool showBooksLoanedOutField;

        private bool showBorrowedBooksField;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppShell"/> class.
        /// </summary>
        public AppShell()
        {
            this.VersionString = $"v {AppInfo.VersionString}";

            this.InitializeComponent();

            AppShell.RegisterRoutes();

            this.BindingContext = this;

            this.Loaded += this.OnShellLoaded;
        }

        /// <summary>
        /// Gets or sets the application title string, which includes the app name and current year.
        /// </summary>
        public string ApplicationTitleString { get; set; }

        /// <summary>
        /// Gets or sets the version string, which includes the app version number.
        /// </summary>
        public string VersionString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Books Loaned Out view or not.
        /// </summary>
        public bool ShowBooksLoanedOut
        {
            get => this.showBooksLoanedOutField;
            set
            {
                if (this.showBooksLoanedOutField != value)
                {
                    this.showBooksLoanedOutField = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Borrowed Books view or not.
        /// </summary>
        public bool ShowBorrowedBooks
        {
            get => this.showBorrowedBooksField;
            set
            {
                if (this.showBorrowedBooksField != value)
                {
                    this.showBorrowedBooksField = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Resets the order and visibility of the tabs in the Library tab based on user preferences.
        /// </summary>
        public void ResetLibraryTab()
        {
            this.libraryTab.Items.Clear();
            this.CreateLibraryTab();
        }

        /// <summary>
        /// Resets the order and visibility of the tabs in the Groupings tab based on user preferences.
        /// </summary>
        public void ResetGroupingsTab()
        {
            this.groupingsTab.Items.Clear();
            this.CreateGroupingsTab();
        }

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute("BookEditView", typeof(Views.Book.BookEditView));
            Routing.RegisterRoute("BookMainView", typeof(Views.Book.BookMainView));
        }

        private void CreateLibraryTab()
        {
            var libraryTabViews = DevicePreferences.LibraryTabViewsOrderValue.Split(",");

            foreach (var libraryTabView in libraryTabViews)
            {
                DataTemplate template = new ();
                var createTab = false;

                if (DevicePreferences.ReadingViewShowValue && libraryTabView.Equals(AppStringResources.Reading))
                {
                    template = new DataTemplate(() => new ReadingView());
                    createTab = true;
                }

                if (DevicePreferences.ToBeReadViewShowValue && libraryTabView.Equals(AppStringResources.ToBeRead))
                {
                    template = new DataTemplate(() => new ToBeReadView());
                    createTab = true;
                }

                if (DevicePreferences.ReadViewShowValue && libraryTabView.Equals(AppStringResources.Read))
                {
                    template = new DataTemplate(() => new ReadView());
                    createTab = true;
                }

                if (DevicePreferences.AllBooksViewShowValue && libraryTabView.Equals(AppStringResources.AllBooks))
                {
                    template = new DataTemplate(() => new AllBooksView());
                    createTab = true;
                }

                if (createTab)
                {
                    this.libraryTab.Items.Add(new ShellContent
                    {
                        Title = libraryTabView,
                        ContentTemplate = template,
                    });
                }
            }
        }

        private void CreateGroupingsTab()
        {
            var groupingsTabViews = DevicePreferences.GroupingsTabViewOrderValue.Split(",");

            foreach (var groupingsTabView in groupingsTabViews)
            {
                DataTemplate template = new ();
                var createTab = false;

                if (DevicePreferences.CollectionsViewShowValue && groupingsTabView.Equals(AppStringResources.Collections))
                {
                    template = new DataTemplate(() => new CollectionsView());
                    createTab = true;
                }

                if (DevicePreferences.GenresViewShowValue && groupingsTabView.Equals(AppStringResources.Genres))
                {
                    template = new DataTemplate(() => new GenresView());
                    createTab = true;
                }

                if (DevicePreferences.SeriesViewShowValue && groupingsTabView.Equals(AppStringResources.Series))
                {
                    template = new DataTemplate(() => new SeriesView());
                    createTab = true;
                }

                if (DevicePreferences.AuthorsViewShowValue && groupingsTabView.Equals(AppStringResources.Authors))
                {
                    template = new DataTemplate(() => new AuthorsView());
                    createTab = true;
                }

                if (DevicePreferences.LocationsViewShowValue && groupingsTabView.Equals(AppStringResources.Locations))
                {
                    template = new DataTemplate(() => new LocationsView());
                    createTab = true;
                }

                if (createTab)
                {
                    this.groupingsTab.Items.Add(new ShellContent
                    {
                        Title = groupingsTabView,
                        ContentTemplate = template,
                    });
                }
            }
        }

        private void OnShellLoaded(object? sender, EventArgs? e)
        {
            if (Shell.Current?.CurrentItem != null)
            {
                this.ApplicationTitleString = $"{AppInfo.Current.Name}, {DateTime.Now.Year}";

                // Set these values on load to reset the visibility of these tabs.
                this.ShowBorrowedBooks = DevicePreferences.BorrowedBooksShowValue;
                this.ShowBooksLoanedOut = DevicePreferences.LoanedOutBooksShowValue;
                this.CreateLibraryTab();
                this.CreateGroupingsTab();

                Shell.Current.CurrentItem = this.libraryTab;
            }
        }
    }
}
