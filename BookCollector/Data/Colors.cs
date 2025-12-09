namespace BookCollector.Data
{
    public class Colors
    {
        public static void SetColors(string hexcode)
        {
            var color = Color.FromArgb(hexcode);
            var secondary = color.AddLuminosity((float)0.1);
            var tertiary = color.AddLuminosity((float)0.2);

            var colorHue = color.GetHue();

            var color2 = color.WithHue((float)(colorHue - 0.2));
            var color3 = color.WithHue((float)(colorHue - 0.4));
            var color4 = color.WithHue((float)(colorHue - 0.6));
            var color5 = color.WithHue((float)(colorHue - 0.8));
            var color6 = color.WithHue((float)(colorHue - 0.5));

            Application.Current?.Resources["Primary"] = Color.FromArgb(hexcode);
            Application.Current?.Resources["Secondary"] = Color.FromArgb(secondary.ToHex());
            Application.Current?.Resources["Tertiary"] = Color.FromArgb(tertiary.ToHex());

            Application.Current?.Resources["Color2"] = Color.FromArgb(color2.ToHex());
            Application.Current?.Resources["Color3"] = Color.FromArgb(color3.ToHex());
            Application.Current?.Resources["Color4"] = Color.FromArgb(color4.ToHex());
            Application.Current?.Resources["Color5"] = Color.FromArgb(color5.ToHex());
            Application.Current?.Resources["Color6"] = Color.FromArgb(color6.ToHex());
        }
    }
}
