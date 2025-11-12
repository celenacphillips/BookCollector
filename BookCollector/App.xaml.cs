namespace BookCollector
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            GetAppTheme();
            
            MainPage = new AppShell();
        }

        private void GetAppTheme()
        {
            var savedTheme = Preferences.Get("AppTheme", "System");
            Application.Current.UserAppTheme = savedTheme switch
            {
                "Light" => AppTheme.Light,
                "Dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified // Follows system
            };
        }
    }
}
