namespace BookCollector
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            App.GetAppTheme();
            App.GetColor();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Create the root window and set its page
            return new Window(new AppShell());
        }

        private static void GetAppTheme()
        {
            var savedTheme = Preferences.Get("AppTheme", "System" /* Default */);

            Current?.UserAppTheme = savedTheme switch
                {
                    "Light" => AppTheme.Light,
                    "Dark" => AppTheme.Dark,
                    _ => AppTheme.Unspecified // Follows system
                };
        }

        private static void GetColor()
        {
            var savedColorHexCode = Preferences.Get("AppColor", "#336699" /* Default */);

            Data.Colors.SetColors(savedColorHexCode);
        }
    }
}
