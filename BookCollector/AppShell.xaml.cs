namespace BookCollector
{
    public partial class AppShell : Shell
    {
        public string Year { get; set; }

        public AppShell()
        {
            Year = DateTime.Now.Year.ToString();
            InitializeComponent();
            BindingContext = this;
        }
    }
}
