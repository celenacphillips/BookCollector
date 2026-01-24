// <copyright file="Colors.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data
{
    /// <summary>
    /// Colors class.
    /// </summary>
    public class Colors
    {
        public static void SetColors(string hexcode)
        {
            var color = Color.FromArgb(hexcode);
            var secondary = color.AddLuminosity(0.1f);
            var tertiary = color.AddLuminosity(0.2f);

            var colorHue = color.GetHue();

            var color2 = color.WithHue(colorHue - 0.2f);
            var color3 = color.WithHue(colorHue - 0.4f);
            var color4 = color.WithHue(colorHue - 0.6f);
            var color5 = color.WithHue(colorHue - 0.8f);
            var color6 = color.WithHue(colorHue - 0.5f);

            Application.Current?.Resources["Primary"] = Color.FromArgb(hexcode);
            Application.Current?.Resources["Secondary"] = Color.FromArgb(secondary.ToHex());
            Application.Current?.Resources["Tertiary"] = Color.FromArgb(tertiary.ToHex());
            Application.Current?.Resources["TertiaryBrush"] = Color.FromArgb(tertiary.ToHex()).WithAlpha(0.6f);

            Application.Current?.Resources["Color2"] = Color.FromArgb(color2.ToHex());
            Application.Current?.Resources["Color3"] = Color.FromArgb(color3.ToHex());
            Application.Current?.Resources["Color4"] = Color.FromArgb(color4.ToHex());
            Application.Current?.Resources["Color5"] = Color.FromArgb(color5.ToHex());
            Application.Current?.Resources["Color6"] = Color.FromArgb(color6.ToHex());
        }
    }
}
