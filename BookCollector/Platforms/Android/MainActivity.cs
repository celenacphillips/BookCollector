// <copyright file="MainActivity.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using Android.App;
using Android.Content.PM;

namespace BookCollector
{
    /// <summary>
    /// Main Activity class.
    /// </summary>
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
    }
}
