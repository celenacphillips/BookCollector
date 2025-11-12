namespace BookCollector.Views;

public partial class SettingsView : ContentPage
{
    public List<string> AppThemeList { get; set; }
    public string SelectedAppTheme { get; set; }

	public SettingsView()
	{
        AppThemeList = ["Light", "Dark"];
        SelectedAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppThemeList[0] : AppThemeList[1];
        InitializeComponent();
        BindingContext = this;
    }

    void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        ChangeTheme(picker.SelectedItem.ToString());
    }

    private void ChangeTheme(string theme)
    {
        Application.Current.UserAppTheme = theme.Equals("Light") ? AppTheme.Light : AppTheme.Dark;
        Preferences.Set("AppTheme", theme);
    }
}