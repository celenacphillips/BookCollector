using BookCollector.Data.Models;

namespace BookCollector
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            GetAppTheme();
            GetColor();

            MainPage = new AppShell();
        }

        private void GetAppTheme()
        {
            var savedTheme = Preferences.Get("AppTheme", "System" /* Default */);
            Application.Current.UserAppTheme = savedTheme switch
            {
                "Light" => AppTheme.Light,
                "Dark" => AppTheme.Dark,
                _ => AppTheme.Unspecified // Follows system
            };
        }

        private void GetColor()
        {
            var savedColorHexCode = Preferences.Get("AppColor", "#336699"  /* Default */);

            Data.Colors.SetColors(savedColorHexCode);
        }
    }
}
