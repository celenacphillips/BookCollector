// <copyright file="AppShell.xaml.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector
{
    using BookCollector.Resources.Localization;
    using BookCollector.Views.Groupings;
    using BookCollector.Views.Library;

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
            var libraryTabViewsOrder = Preferences.Get(
                "LibraryTabViewsOrder",
                $"{AppStringResources.Reading},{AppStringResources.ToBeRead},{AppStringResources.Read},{AppStringResources.AllBooks}" /* Default */);

            var readingOn = Preferences.Get("ReadingOn", true /* Default */);
            var toBeReadOn = Preferences.Get("ToBeReadOn", true /* Default */);
            var readOn = Preferences.Get("ReadOn", true /* Default */);
            var allBooksOn = Preferences.Get("AllBooksOn", true /* Default */);

            var libraryTabViews = libraryTabViewsOrder.Split(",");

            foreach (var libraryTabView in libraryTabViews)
            {
                DataTemplate template = new ();
                var createTab = false;

                if (readingOn && libraryTabView.Equals(AppStringResources.Reading))
                {
                    template = new DataTemplate(() => new ReadingView());
                    createTab = true;
                }

                if (toBeReadOn && libraryTabView.Equals(AppStringResources.ToBeRead))
                {
                    template = new DataTemplate(() => new ToBeReadView());
                    createTab = true;
                }

                if (readOn && libraryTabView.Equals(AppStringResources.Read))
                {
                    template = new DataTemplate(() => new ReadView());
                    createTab = true;
                }

                if (allBooksOn && libraryTabView.Equals(AppStringResources.AllBooks))
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
            var groupingsTabViewsOrder = Preferences.Get(
                "GroupingsTabViewsOrder",
                $"{AppStringResources.Collections},{AppStringResources.Genres},{AppStringResources.Series},{AppStringResources.Authors},{AppStringResources.Locations}" /* Default */);

            var collectionsOn = Preferences.Get("CollectionsOn", true /* Default */);
            var genresOn = Preferences.Get("GenresOn", true /* Default */);
            var seriesOn = Preferences.Get("SeriesOn", true /* Default */);
            var authorsOn = Preferences.Get("AuthorsOn", true /* Default */);
            var locationsOn = Preferences.Get("LocationsOn", true /* Default */);

            var groupingsTabViews = groupingsTabViewsOrder.Split(",");

            foreach (var groupingsTabView in groupingsTabViews)
            {
                DataTemplate template = new ();
                var createTab = false;

                if (collectionsOn && groupingsTabView.Equals(AppStringResources.Collections))
                {
                    template = new DataTemplate(() => new CollectionsView());
                    createTab = true;
                }

                if (genresOn && groupingsTabView.Equals(AppStringResources.Genres))
                {
                    template = new DataTemplate(() => new GenresView());
                    createTab = true;
                }

                if (seriesOn && groupingsTabView.Equals(AppStringResources.Series))
                {
                    template = new DataTemplate(() => new SeriesView());
                    createTab = true;
                }

                if (authorsOn && groupingsTabView.Equals(AppStringResources.Authors))
                {
                    template = new DataTemplate(() => new AuthorsView());
                    createTab = true;
                }

                if (locationsOn && groupingsTabView.Equals(AppStringResources.Locations))
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
                this.CreateLibraryTab();
                this.CreateGroupingsTab();

                Shell.Current.CurrentItem = this.libraryTab;
            }
        }
    }
}
