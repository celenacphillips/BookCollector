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

            var color = Color.FromArgb(savedColorHexCode);
            var secondary = color.AddLuminosity((float)0.1);
            var tertiary = color.AddLuminosity((float)0.2);

            Application.Current.Resources["Primary"] = Color.FromArgb(savedColorHexCode);
            Application.Current.Resources["Secondary"] = Color.FromArgb(secondary.ToHex());
            Application.Current.Resources["Tertiary"] = Color.FromArgb(tertiary.ToHex());
        }
    }
}
