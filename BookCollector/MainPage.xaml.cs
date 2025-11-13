namespace BookCollector
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            var savedColor = Preferences.Get("AppColor", "#336699"  /* Default */);
            CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb(savedColor));

            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
