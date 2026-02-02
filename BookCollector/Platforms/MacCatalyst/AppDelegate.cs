// <copyright file="AppDelegate.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

using Foundation;

namespace BookCollector
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
